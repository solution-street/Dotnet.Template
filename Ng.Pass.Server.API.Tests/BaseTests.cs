using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ng.Pass.Server.API;
using Ng.Pass.Server.API.Responses;
using Ng.Pass.Server.API.Tests;
using Ng.Pass.Server.API.Tests.Helpers;
using Ng.Pass.Server.Common.Tests;
using Ng.Pass.Server.Core.Enums;

/// <summary>
/// All integration tests should inherit from this class. This class will configure all necessary behavior to perform API calls
/// against our controllers while ensuring nearly all business logic is running and not being mocked out.
/// </summary>
public abstract class BaseTests : IDisposable
{
    protected abstract string _controllerRoute { get; }

    //protected CoordinationContext Context { get; private set; }
    protected Dependencies Dependencies { get; private set; }

    /// <summary>
    /// Determines whether the test will add authentication details to the request (and DB).
    /// You may set this as 'false' for unauthenticated requests.
    /// </summary>
    protected bool SetupAuthenticationDetails { get; set; } = true;

    private CustomWebApplicationFactory<Startup> _factory;
    private Lazy<HttpClient> _testServerClient;
    private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();
    private IServiceScope _scope;

    protected BaseTests()
    {
        TestSetupHelpers.SetEnvironmentVariables();
        _factory = new CustomWebApplicationFactory<Startup>();
        Dependencies = _factory.Dependencies;
        _testServerClient = new Lazy<HttpClient>(() => _factory.CreateClient());

        _scope = _factory.Services.CreateScope();
        {
            //Context = _scope.ServiceProvider.GetRequiredService<CoordinationContext>();
            //Context.Database.EnsureCreated();
        }

        UpsertExecutingUserInDatabase();
    }

    /// <summary>
    /// Clean up resources after each test.
    /// </summary>
    public void Dispose()
    {
        //Context.Database.EnsureDeleted();
        _scope?.Dispose();
        _testServerClient.Value?.Dispose();
    }

    internal async Task<TResult> SendPostRequest<TCommand, TResult>(string url, TCommand command)
    {
        return await SendRequestWithContent<TCommand, TResult>(HttpMethod.Post, url, command);
    }

    internal async Task<TResult> SendPostRequest<TResult>(string url)
    {
        return await SendRequestWithoutContent<TResult>(HttpMethod.Post, url);
    }

    internal async Task<TResult> SendGetRequest<TResult>(string url)
    {
        return await SendRequestWithoutContent<TResult>(HttpMethod.Get, url);
    }

    internal async Task<TResult> SendDeleteRequest<TResult>(string url)
    {
        return await SendRequestWithoutContent<TResult>(HttpMethod.Delete, url);
    }

    internal async Task<TResult> SendPatchRequest<TCommand, TResult>(string url, TCommand command)
    {
        return await SendRequestWithContent<TCommand, TResult>(HttpMethod.Patch, url, command);
    }

    internal async Task<TResult> SendPutRequest<TCommand, TResult>(string url, TCommand command)
    {
        return await SendRequestWithContent<TCommand, TResult>(HttpMethod.Put, url, command);
    }

    private async Task<TResult> SendRequestWithContent<TCommand, TResult>(HttpMethod httpMethod, string url, TCommand command)
    {
        HttpRequestMessage messageWithHeaders = GetHttpRequestMessageWithHeaders(httpMethod, url);
        messageWithHeaders.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

        return await GetResponseContent<TResult>(await SendRequest(messageWithHeaders));
    }

    private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
    {
        try
        {
            ClearDatabaseChangeTracker();
            return await _testServerClient.Value.SendAsync(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private static async Task<T> GetResponseContent<T>(HttpResponseMessage response)
    {
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode && typeof(T) != typeof(ApiError))
        {
            throw new Exception($"Unexpected error response. Content: {content}");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return default!;
        }

        if (typeof(T) == typeof(string))
        {
            return (T)(object)content;
        }

        T deserializedObject =
            JsonConvert.DeserializeObject<T>(content)
            ?? throw new InvalidOperationException($"Unable to convert type: {typeof(T).Name} using content: {content}.");

        return deserializedObject;
    }

    /// <summary>
    /// In order to ensure the API calls (and thus unit tests post-API call) properly recognize changes, we want to clear any changes
    /// that occured in the unit test setup. This lets the service(s) start with a fresh state, which simulates a real world scenario.
    /// This also allows the unit test to use the same <see cref="CoordinationContext"/> as the service, which is important when verifying
    /// results.
    /// </summary>
    private void ClearDatabaseChangeTracker()
    {
        //Context.ChangeTracker.Clear();
    }

    private async Task<TResult> SendRequestWithoutContent<TResult>(HttpMethod httpMethod, string url)
    {
        return await GetResponseContent<TResult>(await SendRequest(GetHttpRequestMessageWithHeaders(httpMethod, url)));
    }

    private HttpRequestMessage GetHttpRequestMessageWithHeaders(HttpMethod httpMethod, string url)
    {
        HttpRequestMessage request = new(httpMethod, GetFullUrl(url));

        // If this is false, this will simulate an unauthenticated request.
        if (SetupAuthenticationDetails)
        {
            //var authId = ExecutingUser.AuthProviderId!;

            //_headers["Authorization"] = $"Bearer {authId}";
            //_headers?.Keys?.ToList()?.ForEach(h => request.Headers.Add(h, _headers[h]));

            ///**
            // * Everytime an HTTP request with authentications is made, then we must ensure
            // * that the ExecutingUser is seeded in the database or updated with the latest changes
            // * that may have occured in the test setup.
            // */
            //SetupExecutingUserAuthentication(authId);
        }

        return request;
    }

    /// <summary>
    /// In cases where you need to add non-default headers to the HTTP request.
    /// </summary>
    /// <param name="key">Header key.</param>
    /// <param name="value">Header value.</param>
    protected void AddCustomHeader(string key, string value)
    {
        _headers[key] = value;
    }

    private string GetFullUrl(string url)
    {
        return _controllerRoute + "/" + url.TrimStart('/');
    }

    /// <summary>
    /// Sets up the executing user in the <see cref="TestAuthHandler"/> and ensures that a corresponding database record exists.
    /// The configuration skips Auth0's Bearer token verifiction but still uses <see cref="Ng.Pass.Server.API.Authorization.Configurations.AuthorizeAnyPolicyFilter"/>
    /// to verify proper authentication and authorization.
    /// </summary>
    /// <param name="authId"></param>
    private void SetupExecutingUserAuthentication(string authId)
    {
        TestAuthHandler.AddUser(authId);

        // MDR: for now, do not add the executing user if they have already been added to the DB.
        // This is to allow multiple API calls in a single test for the same user (without adding the user multiple times, causing a unique constraint violation).
        UpsertExecutingUserInDatabase();
    }

    /// <summary>
    /// This method will be adding the ExecutingUser to the database if they do not already exist.
    /// If it does exist in the database, it will update the existing record IF changes are detected.
    /// </summary>
    private void UpsertExecutingUserInDatabase()
    {
        //var existingUser = Context.Users.FirstOrDefault(u => u.Id == ExecutingUser.Id);
        //if (existingUser == null)
        //{
        //    Context.Users.Add(ExecutingUser);
        //    Context.SaveChanges();
        //}
        //else
        //{
        //    Context.Entry(existingUser).CurrentValues.SetValues(ExecutingUser);

        //    if (Context.ChangeTracker.HasChanges())
        //    {
        //        Context.SaveChanges();
        //    }
        //}
    }
}

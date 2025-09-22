namespace Ng.Pass.Server.Services.Secrets.Constants;

public static class SecretHubConstants
{
    public static readonly string HubName = "SecretsHub";
    public static readonly string GroupName = "SecretsGroup";

    public static class Events
    {
        public static readonly string ListRecieved = "SecretListRecieved";
        public static readonly string SecretCreated = "SecretCreated";
        public static readonly string SecretDeleted = "SecretDeleted";
    }
}

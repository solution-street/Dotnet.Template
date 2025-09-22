namespace Ng.Pass.Server.Services.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class HubRouteAttribute : Attribute
{
    public string RouteName { get; }

    public HubRouteAttribute(string routeName)
    {
        RouteName = routeName ?? throw new ArgumentNullException(nameof(routeName));
    }
}

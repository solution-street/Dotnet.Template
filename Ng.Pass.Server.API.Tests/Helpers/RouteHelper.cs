using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Ng.Pass.Server.API.Tests.Helpers;

public class RouteHelper
{
    /// <summary>
    /// Retrieves the route from the <see cref="RouteAttribute"/> property on the controller.
    /// </summary>
    /// <typeparam name="TController">A controller.</typeparam>
    /// <returns>The route.</returns>
    public static string GetControllerRoute<TController>()
        where TController : ControllerBase
    {
        var controllerType = typeof(TController);
        var routeAttribute = controllerType.GetCustomAttributes<RouteAttribute>().FirstOrDefault();

        if (routeAttribute == null)
        {
            throw new ArgumentException($"{controllerType.Name} must be a controller with a ${nameof(RouteAttribute)}.");
        }

        return routeAttribute.Template.Replace(
            "[controller]",
            controllerType.Name.Replace("Controller", string.Empty),
            StringComparison.OrdinalIgnoreCase
        );
    }
}

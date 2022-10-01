using AppBrix;
using BiscuitMachine.Controller.Services;

namespace BiscuitMachine.Controller;

/// <summary>
/// Extension methods for the controller module.
/// </summary>
public static class ControllerExtensions
{
    /// <summary>
    /// Gets the currently registered <see cref="IController"/>.
    /// </summary>
    /// <param name="app">The currently running application.</param>
    /// <returns>The <see cref="IController"/>.</returns>
    public static IController GetController(this IApp app) => (IController)app.Get(typeof(IController));
}

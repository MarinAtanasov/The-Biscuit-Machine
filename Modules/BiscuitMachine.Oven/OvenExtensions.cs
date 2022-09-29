using AppBrix;
using AppBrix.Configuration;
using BiscuitMachine.Oven.Configuration;
using BiscuitMachine.Oven.Services;

namespace BiscuitMachine.Oven;

/// <summary>
/// Extension methods for the oven module.
/// </summary>
public static class OvenExtensions
{
    /// <summary>
    /// Gets the currently registered <see cref="IOven"/>.
    /// </summary>
    /// <param name="app">The currently running application.</param>
    /// <returns>The <see cref="IOven"/>.</returns>
    public static IOven GetOven(this IApp app) => (IOven)app.Get(typeof(IOven));

    /// <summary>
    /// Gets the <see cref="OvenConfig"/> from <see cref="OvenConfig"/>.
    /// </summary>
    /// <param name="service">The configuration service.</param>
    /// <returns>The <see cref="OvenConfig"/>.</returns>
    public static OvenConfig GetOvenConfig(this IConfigService service) => (OvenConfig)service.Get(typeof(OvenConfig));
}

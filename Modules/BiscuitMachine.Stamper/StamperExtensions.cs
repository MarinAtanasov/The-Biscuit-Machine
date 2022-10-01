using AppBrix.Configuration;
using BiscuitMachine.Stamper.Configuration;
using BiscuitMachine.Stamper.Services;

namespace AppBrix;

/// <summary>
/// Extension methods for the stamper module.
/// </summary>
public static class StamperExtensions
{
    /// <summary>
    /// Gets the currently registered <see cref="IStamper"/>.
    /// </summary>
    /// <param name="app">The currently running application.</param>
    /// <returns>The <see cref="IStamper"/>.</returns>
    public static IStamper GetStamper(this IApp app) => (IStamper)app.Get(typeof(IStamper));

    /// <summary>
    /// Gets the <see cref="StamperConfig"/> from <see cref="StamperConfig"/>.
    /// </summary>
    /// <param name="service">The configuration service.</param>
    /// <returns>The <see cref="StamperConfig"/>.</returns>
    public static StamperConfig GetStamperConfig(this IConfigService service) => (StamperConfig)service.Get(typeof(StamperConfig));
}

using AppBrix.Configuration;
using Conveyor.Configuration;
using Conveyor.Services;

namespace AppBrix;

/// <summary>
/// Extension methods for the conveyor module.
/// </summary>
public static class ConveyorExtensions
{
    /// <summary>
    /// Gets the currently registered conveyor.
    /// </summary>
    /// <param name="app">The currently running application.</param>
    /// <returns>The conveyor.</returns>
    public static IConveyor GetConveyor(this IApp app) => (IConveyor)app.Get(typeof(IConveyor));

    /// <summary>
    /// Gets the <see cref="ConveyorConfig"/> from <see cref="ConveyorConfig"/>.
    /// </summary>
    /// <param name="service">The configuration service.</param>
    /// <returns>The <see cref="ConveyorConfig"/>.</returns>
    public static ConveyorConfig GetConveyorConfig(this IConfigService service) => (ConveyorConfig)service.Get(typeof(ConveyorConfig));
}

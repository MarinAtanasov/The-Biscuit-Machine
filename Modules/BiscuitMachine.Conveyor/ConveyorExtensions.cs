using AppBrix.Configuration;
using BiscuitMachine.Conveyor.Configuration;
using BiscuitMachine.Conveyor.Services;

namespace AppBrix;

/// <summary>
/// Extension methods for the conveyor module.
/// </summary>
public static class ConveyorExtensions
{
    /// <summary>
    /// Gets the currently registered <see cref="IConveyor"/>.
    /// </summary>
    /// <param name="app">The currently running application.</param>
    /// <returns>The <see cref="IConveyor"/>.</returns>
    public static IConveyor GetConveyor(this IApp app) => (IConveyor)app.Get(typeof(IConveyor));

    /// <summary>
    /// Gets the <see cref="ConveyorConfig"/> from <see cref="ConveyorConfig"/>.
    /// </summary>
    /// <param name="service">The configuration service.</param>
    /// <returns>The <see cref="ConveyorConfig"/>.</returns>
    public static ConveyorConfig GetConveyorConfig(this IConfigService service) => (ConveyorConfig)service.Get(typeof(ConveyorConfig));
}

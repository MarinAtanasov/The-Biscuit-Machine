using AppBrix;
using BiscuitMachine.Extruder.Services;

namespace BiscuitMachine.Extruder;

/// <summary>
/// Extension methods for the extruder module.
/// </summary>
public static class ExtruderExtensions
{
    /// <summary>
    /// Gets the currently registered <see cref="IExtruder"/>.
    /// </summary>
    /// <param name="app">The currently running application.</param>
    /// <returns>The <see cref="IExtruder"/>.</returns>
    public static IExtruder GetExtruder(this IApp app) => (IExtruder)app.Get(typeof(IExtruder));
}

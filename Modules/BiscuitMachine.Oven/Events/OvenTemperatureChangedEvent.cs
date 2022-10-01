using AppBrix.Events.Contracts;

namespace BiscuitMachine.Oven.Events;

/// <summary>
/// Event that is fired whenever the oven's temperature changes.
/// </summary>
public sealed class OvenTemperatureChangedEvent : IEvent
{
    /// <summary>
    /// Creates a new instance of <see cref="OvenTemperatureChangedEvent"/>.
    /// </summary>
    public OvenTemperatureChangedEvent(int previousTemperature, int currentTemperature)
    {
        this.CurrentTemperature = currentTemperature;
        this.PreviousTemperature = previousTemperature;
    }

    /// <summary>
    /// Gets the current temperature of the oven.
    /// </summary>
    public int CurrentTemperature { get; }

    /// <summary>
    /// Gets the previous temperature reading of the oven.
    /// </summary>
    public int PreviousTemperature { get; }
}

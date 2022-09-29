using BiscuitMachine.Extruder.Contracts;

namespace BiscuitMachine.Extruder.Services;

/// <summary>
/// Defines a biscuit machine conveyor extruder.
/// </summary>
public interface IExtruder
{
    /// <summary>
    /// Returns the current state of the extruder.
    /// </summary>
    ExtruderState State { get; }

    /// <summary>
    /// Turns off the extruder.
    /// </summary>
    void TurnOff();

    /// <summary>
    /// Turns on the extruder.
    /// </summary>
    void TurnOn();
}

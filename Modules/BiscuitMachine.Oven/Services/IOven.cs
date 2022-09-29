using BiscuitMachine.Oven.Contracts;

namespace BiscuitMachine.Oven.Services;

/// <summary>
/// Defines a biscuit machine conveyor oven.
/// </summary>
public interface IOven
{
    /// <summary>
    /// Gets whether the oven is heated.
    /// </summary>
    bool IsHeated { get; }

    /// <summary>
    /// Returns the current state of the oven.
    /// </summary>
    OvenState State { get; }

    /// <summary>
    /// Returns the current temperature in the oven.
    /// </summary>
    int Temperature { get; }

    /// <summary>
    /// Turns off the oven.
    /// </summary>
    void TurnOff();

    /// <summary>
    /// Turns on the oven.
    /// </summary>
    void TurnOn();
}

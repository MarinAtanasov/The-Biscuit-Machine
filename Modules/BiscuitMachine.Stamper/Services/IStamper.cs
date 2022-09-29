using BiscuitMachine.Stamper.Contracts;

namespace BiscuitMachine.Stamper.Services;

/// <summary>
/// Defines a biscuit machine conveyor stamper.
/// </summary>
public interface IStamper
{
    /// <summary>
    /// Returns the current state of the stamper.
    /// </summary>
    StamperState State { get; }

    /// <summary>
    /// Turns off the stamper.
    /// </summary>
    void TurnOff();

    /// <summary>
    /// Turns on the stamper.
    /// </summary>
    void TurnOn();
}

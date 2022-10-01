using BiscuitMachine.Controller.Contracts;

namespace BiscuitMachine.Controller.Services;

/// <summary>
/// Defines a biscuit machine conveyor controller.
/// </summary>
public interface IController
{
    /// <summary>
    /// Returns the current state of the controller.
    /// </summary>
    ControllerState State { get; }

    /// <summary>
    /// Switches the controller to a paused state.
    /// </summary>
    void Pause();

    /// <summary>
    /// Switches the controller off.
    /// </summary>
    void TurnOff();

    /// <summary>
    /// Switches the controller on.
    /// </summary>
    void TurnOn();
}

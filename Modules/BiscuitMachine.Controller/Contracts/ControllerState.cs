namespace BiscuitMachine.Controller.Contracts;

/// <summary>
/// Defines all states of the controller.
/// </summary>
public enum ControllerState
{
    /// <summary>
    /// Indicates that the controller is in off state.
    /// </summary>
    Off,
    /// <summary>
    /// Indicates that the Controller is in pause state.
    /// </summary>
    Pause,
    /// <summary>
    /// Indicates that the Controller is in on state.
    /// </summary>
    On
}

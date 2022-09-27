using BiscuitMachine.Motor.Contracts;

namespace BiscuitMachine.Motor.Services;

/// <summary>
/// Defines a biscuit machine conveyor motor.
/// </summary>
public interface IMotor
{
    /// <summary>
    /// Returns the current state of the motor.
    /// </summary>
    MotorState State { get; }
    
    /// <summary>
    /// Turns the motor. Called automatically on a set timer.
    /// Calling this manually should be primarily done for testing purposes.
    /// </summary>
    void ExecuteRevolution();

    /// <summary>
    /// Turns off the motor.
    /// </summary>
    void TurnOff();

    /// <summary>
    /// Turns on the motor.
    /// </summary>
    void TurnOn();
}

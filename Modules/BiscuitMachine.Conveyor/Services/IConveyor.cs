namespace BiscuitMachine.Conveyor.Services;

/// <summary>
/// Defines a biscuit machine conveyor.
/// </summary>
public interface IConveyor
{
    /// <summary>
    /// Gets whether there are biscuits on the conveyor belt.
    /// </summary>
    bool HasBiscuits { get; }

    /// <summary>
    /// Adds a biscuit to the conveyor belt.
    /// </summary>
    void AddBiscuit();
    
    /// <summary>
    /// Moves the conveyor belt with one step.
    /// </summary>
    void Move();
}

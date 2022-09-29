using BiscuitMachine.Conveyor.Contracts;

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
    /// Gets the length of the conveyor.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Adds a biscuit to the conveyor belt.
    /// </summary>
    void AddBiscuit();
    
    /// <summary>
    /// Gets the biscuit that is on the conveyor at a given index.
    /// </summary>
    /// <param name="index">The index ont he conveyor.</param>
    /// <returns>The biscuit, or null if there is no biscuit.</returns>
    Biscuit? GetBiscuit(int index);

    /// <summary>
    /// Moves the conveyor belt with one step.
    /// </summary>
    void Move();
}

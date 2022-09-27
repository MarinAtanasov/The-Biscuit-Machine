namespace Conveyor.Services;

/// <summary>
/// Defines a biscuit machine conveyor.
/// </summary>
public interface IConveyor
{
    /// <summary>
    /// Gets whether there are cookies on the conveyor belt.
    /// </summary>
    bool HasCookies { get; }

    /// <summary>
    /// Adds a cookie to the conveyor belt.
    /// </summary>
    void AddCookie();
    
    /// <summary>
    /// Moves the conveyor belt with one step.
    /// </summary>
    void Move();
}

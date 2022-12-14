namespace BiscuitMachine.Conveyor.Contracts;

/// <summary>
/// Defines all states of a biscuit.
/// </summary>
public enum BiscuitState
{
    /// <summary>
    /// The biscuit's initial state.
    /// </summary>
    Raw,
    /// <summary>
    /// The biscuit has been stamped.
    /// </summary>
    Stamped,
    /// <summary>
    /// The biscuit is in the process of being baked. 
    /// </summary>
    Underbaked,
    /// <summary>
    /// The biscuit is ready.
    /// </summary>
    Baked,
    /// <summary>
    /// The biscuit is baked too long.
    /// </summary>
    Overbaked,
    /// <summary>
    /// The biscuit is burnt.
    /// </summary>
    Burnt,
}

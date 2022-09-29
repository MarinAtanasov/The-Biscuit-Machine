using System;

namespace BiscuitMachine.Conveyor.Contracts;

/// <summary>
/// Defines a biscuit that will be created by the machine.
/// </summary>
public class Biscuit
{
    /// <summary>
    /// Gets or sets the state of the biscuit.
    /// </summary>
    public BiscuitState State { get; private set; } = BiscuitState.Raw;

    /// <summary>
    /// Stamps the biscuit dough.
    /// </summary>
    /// <exception cref="InvalidOperationException">The current state is not supported.</exception>
    public void Stamp() => this.State = this.State switch
    {
        BiscuitState.Raw => BiscuitState.Stamped,
        _ => throw new InvalidOperationException($"Cannot stamp a biscuit in {this.State} state.")
    };
}

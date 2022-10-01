using System;

namespace BiscuitMachine.Conveyor.Contracts;

/// <summary>
/// Defines a biscuit that will be created by the machine.
/// </summary>
public sealed class Biscuit
{
    /// <summary>
    /// Creates a new instance of <see cref="Biscuit"/>.
    /// </summary>
    public Biscuit() : this(BiscuitState.Raw)
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="Biscuit"/> with the specified initial state.
    /// Used inside unit tests.
    /// </summary>
    /// <param name="state">The initial state.</param>
    public Biscuit(BiscuitState state)
    {
        this.State = state;
    }

    /// <summary>
    /// Gets or sets the state of the biscuit.
    /// </summary>
    public BiscuitState State { get; private set; }

    /// <summary>
    /// Bakes the biscuit.
    /// </summary>
    /// <exception cref="InvalidOperationException">The current state is not supported.</exception>
    public void Bake() => this.State = this.State switch
    {
        BiscuitState.Stamped => BiscuitState.Underbaked,
        BiscuitState.Underbaked => BiscuitState.Baked,
        BiscuitState.Baked => BiscuitState.Overbaked,
        BiscuitState.Overbaked => BiscuitState.Burnt,
        BiscuitState.Burnt => BiscuitState.Burnt,
        _ => throw new InvalidOperationException($"Cannot bake a biscuit in {this.State} state.")
    };

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

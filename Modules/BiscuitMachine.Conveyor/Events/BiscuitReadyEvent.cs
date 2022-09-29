using AppBrix.Events.Contracts;
using BiscuitMachine.Conveyor.Contracts;

namespace BiscuitMachine.Conveyor.Events;

/// <summary>
/// An event that is fired when a biscuit leaves the conveyor.
/// </summary>
public sealed class BiscuitReadyEvent : IEvent
{
    /// <summary>
    /// Creates a new instance of <see cref="BiscuitReadyEvent"/>.
    /// </summary>
    public BiscuitReadyEvent(Biscuit biscuit)
    {
        this.Biscuit = biscuit;
    }

    /// <summary>
    /// Gets the Biscuit.
    /// </summary>
    public Biscuit Biscuit { get; }
}

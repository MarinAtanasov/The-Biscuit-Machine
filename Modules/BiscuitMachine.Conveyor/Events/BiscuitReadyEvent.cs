using AppBrix.Events.Contracts;

namespace BiscuitMachine.Conveyor.Events;

/// <summary>
/// An event that is fired when a biscuit leaves the conveyor.
/// </summary>
public sealed class BiscuitReadyEvent : IEvent
{
}

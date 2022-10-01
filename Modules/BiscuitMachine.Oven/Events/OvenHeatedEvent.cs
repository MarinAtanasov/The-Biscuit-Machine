using AppBrix.Events.Contracts;

namespace BiscuitMachine.Oven.Events;

/// <summary>
/// Event that is fired after the oven is done heating up.
/// </summary>
public sealed class OvenHeatedEvent : IEvent
{
}

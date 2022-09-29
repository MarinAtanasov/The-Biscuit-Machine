using AppBrix;
using AppBrix.Lifecycle;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Stamper.Contracts;
using BiscuitMachine.Stamper.Services;

namespace BiscuitMachine.Stamper.Impl;

internal sealed class DefaultStamper : IApplicationLifecycle, IStamper
{
    #region Construction
    public DefaultStamper(int index)
    {
        this.Index = index;
    }
    #endregion

    #region IApplicationLifecycle implementation
    public void Initialize(IInitializeContext context)
    {
        this.app = context.App;
        this.app.GetEventHub().Subscribe<PulseEvent>(this.PulseEvent);
    }

    public void Uninitialize()
    {
        this.app.GetEventHub().Unsubscribe<PulseEvent>(this.PulseEvent);
        this.app = null;
    }
    #endregion

    #region Properties
    public int Index { get; }

    public StamperState State { get; private set; }
    #endregion

    #region Public and overriden methods
    public void TurnOff() => this.State = StamperState.Off;

    public void TurnOn() => this.State = StamperState.On;
    #endregion

    #region Private methods
    private void PulseEvent(PulseEvent _)
    {
        if (this.State == StamperState.On)
        {
            this.app.GetConveyor().GetBiscuit(this.Index)?.Stamp();
        }
    }
    #endregion

    #region Private field and constants
    #nullable disable
    private IApp app;
    #nullable restore
    #endregion
}

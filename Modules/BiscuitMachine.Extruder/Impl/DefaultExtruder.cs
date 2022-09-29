using AppBrix;
using AppBrix.Lifecycle;
using BiscuitMachine.Extruder.Contracts;
using BiscuitMachine.Extruder.Services;
using BiscuitMachine.Motor.Events;

namespace BiscuitMachine.Extruder.Impl;

internal sealed class DefaultExtruder : IApplicationLifecycle, IExtruder
{
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
    public ExtruderState State { get; private set; }
    #endregion

    #region Public and overriden methods
    public void TurnOff() => this.State = ExtruderState.Off;

    public void TurnOn() => this.State = ExtruderState.On;
    #endregion

    #region Private methods
    private void PulseEvent(PulseEvent _)
    {
        if (this.State == ExtruderState.On)
        {
            this.app.GetConveyor().AddBiscuit();
        }
    }
    #endregion

    #region Private field and constants
    #nullable disable
    private IApp app;
    #nullable restore
    #endregion
}

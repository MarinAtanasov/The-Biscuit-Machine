using AppBrix;
using AppBrix.Lifecycle;
using BiscuitMachine.Controller.Contracts;
using BiscuitMachine.Controller.Services;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Oven.Events;

namespace BiscuitMachine.Controller.Impl;

internal sealed class DefaultController : IApplicationLifecycle, IController
{
    #region IApplicationLifecycle implementation
    public void Initialize(IInitializeContext context)
    {
        this.app = context.App;
        this.State = ControllerState.Off;

        this.app.GetEventHub().Subscribe<OvenHeatedEvent>(this.OvenHeatedEvent);
        this.app.GetEventHub().Subscribe<PulseEvent>(this.PulseEvent);
    }

    public void Uninitialize()
    {
        this.TurnOff();

        this.app.GetEventHub().Unsubscribe<PulseEvent>(this.PulseEvent);
        this.app.GetEventHub().Unsubscribe<OvenHeatedEvent>(this.OvenHeatedEvent);

        this.app = null;
    }
    #endregion

    #region Properties
    public ControllerState State { get; private set; }
    #endregion

    #region Public and overriden methods
    public void Pause()
    {
        if (this.State == ControllerState.Pause)
            return;

        this.State = ControllerState.Pause;
        this.app.GetExtruder().TurnOff();
        this.app.GetOven().TurnOn();
    }

    public void TurnOff()
    {
        if (this.State == ControllerState.Off)
            return;

        this.State = ControllerState.Off;
        this.app.GetMotor().TurnOff();
        this.app.GetExtruder().TurnOff();
        this.app.GetStamper().TurnOff();
        this.app.GetOven().TurnOff();
    }

    public void TurnOn()
    {
        if (this.State == ControllerState.On)
            return;

        this.State = ControllerState.On;
        if (this.app.GetOven().IsHeated)
        {
            this.TurnOnComponents();
        }
        else
        {
            this.app.GetOven().TurnOn();
        }
    }
    #endregion

    #region Private methods
    private void OvenHeatedEvent(OvenHeatedEvent _)
    {
        if (this.State == ControllerState.On)
        {
            this.TurnOnComponents();
        }
    }

    private void PulseEvent(PulseEvent _)
    {
        if (this.State == ControllerState.Pause && !this.app.GetConveyor().HasBiscuits)
        {
            this.app.GetMotor().TurnOff();
            this.app.GetStamper().TurnOff();
        }
    }

    private void TurnOnComponents()
    {
        this.app.GetExtruder().TurnOn();
        this.app.GetStamper().TurnOn();
        this.app.GetOven().TurnOn();
        this.app.GetMotor().TurnOn();
    }
    #endregion

    #region Private field and constants
    #nullable disable
    private IApp app;
    #nullable restore
    #endregion
}

using AppBrix;
using AppBrix.Events.Schedule.Contracts;
using AppBrix.Lifecycle;
using BiscuitMachine.Motor.Contracts;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Motor.Services;

namespace BiscuitMachine.Motor.Impl;

internal sealed class DefaultMotor : IApplicationLifecycle, IMotor
{
    #region IApplicationLifecycle implementation
    public void Initialize(IInitializeContext context)
    {
        this.app = context.App;
        this.app.GetEventHub().Subscribe<MotorTimerEvent>(this.MotorTimerEvent);
    }

    public void Uninitialize()
    {
        this.UnscheduleTimerEvent();
        this.app.GetEventHub().Unsubscribe<MotorTimerEvent>(this.MotorTimerEvent);
        this.app = null;
    }
    #endregion

    #region Properties
    public MotorState State { get; private set; }
    #endregion

    #region Public and overriden methods
    public void ExecuteRevolution()
    {
        if (this.State == MotorState.On)
        {
            this.app.GetEventHub().Raise(new PulseEvent());
        }
    }

    public void TurnOff()
    {
        if (this.State == MotorState.Off)
            return;
        
        this.State = MotorState.Off;
        this.UnscheduleTimerEvent();
    }

    public void TurnOn()
    {
        if (this.State == MotorState.On)
            return;

        this.State = MotorState.On;
        var delay = this.app.ConfigService.GetMotorConfig().PulseDelay;
        this.scheduledEvent = this.app.GetTimerScheduledEventHub().Schedule(new MotorTimerEvent(), delay, delay);
    }
    #endregion

    #region Private methods
    private void MotorTimerEvent(MotorTimerEvent _) => this.ExecuteRevolution();

    private void UnscheduleTimerEvent()
    {
        if (this.scheduledEvent is not null)
        {
            this.app.GetTimerScheduledEventHub().Unschedule(this.scheduledEvent);
            this.scheduledEvent = null;
        }
    }
    #endregion

    #region Private field and constants
    #nullable disable
    private IApp app;
    #nullable restore
    private IScheduledEvent<MotorTimerEvent>? scheduledEvent;
    #endregion
}

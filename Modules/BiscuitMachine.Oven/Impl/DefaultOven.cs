using AppBrix;
using AppBrix.Events.Schedule.Contracts;
using AppBrix.Lifecycle;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Oven.Contracts;
using BiscuitMachine.Oven.Events;
using BiscuitMachine.Oven.Services;

namespace BiscuitMachine.Oven.Impl;

internal sealed class DefaultOven : IApplicationLifecycle, IOven
{
    #region IApplicationLifecycle implementation
    public void Initialize(IInitializeContext context)
    {
        this.app = context.App;
        this.HeatingElementState = HeatingElementState.Off;
        this.Index = this.app.ConfigService.GetOvenConfig().Index;
        this.Length = 2;
        this.State = OvenState.Off;
        this.Temperature = this.app.ConfigService.GetOvenConfig().AmbientTemperature;
        this.app.GetEventHub().Subscribe<HeatingElementTimerEvent>(this.HeatingElementTimerEvent);
        this.app.GetEventHub().Subscribe<PulseEvent>(this.PulseEvent);

        var delay = this.app.ConfigService.GetOvenConfig().TemperatureCheckDelay;
        this.scheduledEvent = this.app.GetTimerScheduledEventHub().Schedule(new HeatingElementTimerEvent(), delay, delay);
    }

    public void Uninitialize()
    {
        this.app.GetEventHub().Unsubscribe<PulseEvent>(this.PulseEvent);
        this.app.GetEventHub().Unsubscribe<HeatingElementTimerEvent>(this.HeatingElementTimerEvent);
        this.UnscheduleTimerEvent();
        this.TurnOff();

        this.Temperature = this.app.ConfigService.GetOvenConfig().AmbientTemperature;
        this.Length = 2;
        this.Index = this.app.ConfigService.GetOvenConfig().Index;
        this.app = null;
    }
    #endregion

    #region Properties
    public HeatingElementState HeatingElementState { get; private set; }

    public int Index { get; private set;  }

    public bool IsHeated => this.Temperature >= this.app.ConfigService.GetOvenConfig().MinTemperature;

    public int Length { get; private set;  }

    public OvenState State { get; private set; }

    public int Temperature { get; private set; }
    #endregion

    #region Public and overriden methods

    public void TurnOff()
    {
        if (this.State == OvenState.Off)
            return;

        this.State = OvenState.Off;
        this.HeatingElementState = HeatingElementState.Off;
    }

    public void TurnOn()
    {
        if (this.State == OvenState.On)
            return;

        this.State = OvenState.On;
    }
    #endregion

    #region Private methods
    private void HeatingElementTimerEvent(HeatingElementTimerEvent _)
    {
        var config = this.app.ConfigService.GetOvenConfig();
        var isHeated = this.IsHeated;

        if (this.State == OvenState.Off)
        {
            this.Temperature -= config.TemperatureDecreasePerInterval;
        }
        else if (this.HeatingElementState == HeatingElementState.On)
        {
            this.Temperature += config.TemperatureIncreasePerInterval;
            this.HeatingElementState = this.Temperature + config.TemperatureIncreasePerInterval > config.MaxTemperature ?
                HeatingElementState.Off : HeatingElementState.On;
        }
        else
        {
            this.Temperature -= config.TemperatureDecreasePerInterval;
            this.HeatingElementState = this.Temperature - config.TemperatureDecreasePerInterval < config.MinTemperature ?
                HeatingElementState.On : HeatingElementState.Off;
        }

        if (!isHeated && this.IsHeated)
        {
            this.app.GetEventHub().Raise(new OvenHeatedEvent());
        }
    }

    private void PulseEvent(PulseEvent _)
    {
        if (this.State == OvenState.On)
        {
            for (var i = 0; i < this.Length; i++)
            {
                this.app.GetConveyor().GetBiscuit(this.Index + i)?.Bake();
            }
        }
    }

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
    private IScheduledEvent<HeatingElementTimerEvent>? scheduledEvent;
    #endregion
}

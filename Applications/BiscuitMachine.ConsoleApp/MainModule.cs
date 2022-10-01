using AppBrix;
using AppBrix.Lifecycle;
using AppBrix.Logging.Console;
using AppBrix.Modules;
using BiscuitMachine.Controller;
using BiscuitMachine.Conveyor.Events;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Oven.Contracts;
using BiscuitMachine.Oven.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiscuitMachine.ConsoleApp;

public sealed class MainModule : MainModuleBase
{
    #region Properties
    public override IEnumerable<Type> Dependencies => new[] { typeof(ControllerModule), typeof(ConsoleLoggingModule) };
    #endregion

    #region Public and overriden methods
    protected override void Configure(IConfigureContext context)
    {
        base.Configure(context);

        this.App.ConfigService.GetConveyorConfig().ConveyorLength = 6;
        this.App.ConfigService.GetStamperConfig().Index = 1;
        this.App.ConfigService.GetOvenConfig().Index = 3;
        this.App.ConfigService.GetOvenConfig().Length = 2;
        this.App.ConfigService.GetScheduledEventsConfig().ExecutionCheck = TimeSpan.FromMilliseconds(50);
    }

    protected override void Initialize(IInitializeContext context)
    {
        this.biscuits = 0;

        this.App.GetEventHub().Subscribe<BiscuitReadyEvent>(BiscuitReadyEvent);
        this.App.GetEventHub().Subscribe<OvenHeatedEvent>(OvenHeatedEvent);
        this.App.GetEventHub().Subscribe<OvenTemperatureChangedEvent>(OvenTemperatureChangedEvent);
        this.App.GetEventHub().Subscribe<PulseEvent>(PulseEvent);
    }

    protected override void Uninitialize()
    {
        this.App.GetEventHub().Unsubscribe<BiscuitReadyEvent>(BiscuitReadyEvent);
        this.App.GetEventHub().Unsubscribe<OvenHeatedEvent>(OvenHeatedEvent);
        this.App.GetEventHub().Unsubscribe<OvenTemperatureChangedEvent>(OvenTemperatureChangedEvent);
        this.App.GetEventHub().Unsubscribe<PulseEvent>(PulseEvent);

        this.biscuits = 0;
    }
    #endregion

    #region Private methods
    private void BiscuitReadyEvent(BiscuitReadyEvent args)
    {
        this.biscuits++;
        Console.WriteLine($"Biscuit is ready. State: {args.Biscuit.State} | Total: {this.biscuits}");
    }

    private void OvenHeatedEvent(OvenHeatedEvent _)
    {
        Console.WriteLine("The oven is done heating up.");
    }

    private void OvenTemperatureChangedEvent(OvenTemperatureChangedEvent args)
    {
        if (args.PreviousTemperature < args.CurrentTemperature && !this.App.GetOven().IsHeated)
        {
            Console.WriteLine($"The oven is heating up: {args.PreviousTemperature}°C -> {args.CurrentTemperature}°C");
        }
        else if (args.PreviousTemperature > args.CurrentTemperature && this.App.GetOven().State == OvenState.Off)
        {
            Console.WriteLine($"The oven is cooling down: {args.PreviousTemperature}°C -> {args.CurrentTemperature}°C");
        }
    }

    private void PulseEvent(PulseEvent _)
    {
        var conveyor = this.App.GetConveyor();
        var biscuits = Enumerable.Range(0, conveyor.Length)
            .Select(x => conveyor.GetBiscuit(x)?.State.ToString() ?? string.Empty);
        var belt = string.Join(" | ", biscuits.Select(x => $"{x,10}"));
        Console.WriteLine($"Oven temperature: {this.App.GetOven().Temperature}°C | Conveyor: {belt}");
    }
    #endregion

    #region Private fields and constants
    private int biscuits;
    #endregion
}

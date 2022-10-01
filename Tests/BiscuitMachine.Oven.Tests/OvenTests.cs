using AppBrix;
using AppBrix.Configuration.Memory;
using BiscuitMachine.Conveyor.Contracts;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Oven.Contracts;
using BiscuitMachine.Oven.Events;
using BiscuitMachine.Oven.Services;
using BiscuitMachine.Testing;
using FluentAssertions;
using System;
using Xunit;

namespace BiscuitMachine.Oven.Tests;

public sealed class OvenTests : IDisposable
{
    #region Setup and cleanup
    public OvenTests() => this.app = App.Start<TestMainModule<OvenModule>>(new MemoryConfigService());

    public void Dispose() => this.app.Stop();
    #endregion

    #region Tests
    [Fact]
    public void TestGetOven()
    {
        IOven oven = null;

        var action = () => oven = this.app.GetOven();

        action.Should().NotThrow("an oven should have been registered");
        oven.Should().NotBeNull("an oven should have been registered");
    }

    [Fact]
    public void TestOvenTurnOn()
    {
        var oven = this.app.GetOven();

        oven.TurnOn();

        oven.State.Should().Be(OvenState.On, "the oven should have been turned on");

        oven.TurnOn();

        oven.State.Should().Be(OvenState.On, "the oven should stay turned on after a second call");
    }
    
    [Fact]
    public void TestOvenTurnOff()
    {
        var oven = this.app.GetOven();
        oven.TurnOn();

        oven.TurnOff();

        oven.State.Should().Be(OvenState.Off, "the oven should have been turned off");
    }

    [Fact]
    public void TestPulseOffState()
    {
        this.app.GetConveyor().AddBiscuit();

        this.app.GetEventHub().Raise(new PulseEvent());

        var biscuit = this.app.GetConveyor().GetBiscuit(0);
        biscuit.Should().NotBeNull("the oven shouldn't move the biscuit");
        biscuit!.State.Should().Be(BiscuitState.Raw, "the oven is off");
    }

    [Fact]
    public void TestPulseOnState()
    {
        this.app.GetOven().TurnOn();
        this.app.GetConveyor().AddBiscuit();
        this.app.GetConveyor().GetBiscuit(0)!.Stamp();

        this.app.GetEventHub().Raise(new PulseEvent());
        var biscuit = this.app.GetConveyor().GetBiscuit(0);
        biscuit.Should().NotBeNull("the oven shouldn't move the biscuit");
        biscuit!.State.Should().Be(BiscuitState.Underbaked, "the oven is on");

        this.app.GetEventHub().Raise(new PulseEvent());
        biscuit = this.app.GetConveyor().GetBiscuit(0);
        biscuit.Should().NotBeNull("the oven shouldn't move the biscuit");
        biscuit!.State.Should().Be(BiscuitState.Baked, "the oven is on");
    }

    [Fact]
    public void TestOvenHeating()
    {
        this.app.ConfigService.GetScheduledEventsConfig().ExecutionCheck = TimeSpan.FromMilliseconds(1);
        var config = this.app.ConfigService.GetOvenConfig();
        config.AmbientTemperature = config.MinTemperature - config.TemperatureIncreasePerInterval * 2;
        config.TemperatureCheckDelay = TimeSpan.FromMilliseconds(1);
        this.app.Reinitialize();
        var isHeatedEventCalled = false;
        this.app.GetEventHub().Subscribe<OvenHeatedEvent>(_ => isHeatedEventCalled = true);

        var oven = this.app.GetOven();

        oven.TurnOn();

        var isHeated = () => oven.IsHeated;
        isHeated.ShouldReturn(true, "the oven should heat up while it is on");
        isHeatedEventCalled.Should().BeTrue("the oven heated event should have been called");
    }

    [Fact]
    public void TestOvenCoolingOnState()
    {
        this.app.ConfigService.GetScheduledEventsConfig().ExecutionCheck = TimeSpan.FromMilliseconds(1);
        var config = this.app.ConfigService.GetOvenConfig();
        config.AmbientTemperature = config.MinTemperature - config.TemperatureIncreasePerInterval;
        config.TemperatureCheckDelay = TimeSpan.FromMilliseconds(1);
        this.app.Reinitialize();
        this.app.GetEventHub().Subscribe<OvenTemperatureChangedEvent>(x =>
        {
            x.CurrentTemperature.Should().BeGreaterOrEqualTo(config.AmbientTemperature);
            x.PreviousTemperature.Should().BeGreaterOrEqualTo(config.AmbientTemperature);
            x.CurrentTemperature.Should().BeLessOrEqualTo(config.MaxTemperature);
            x.PreviousTemperature.Should().BeLessOrEqualTo(config.MaxTemperature);
        });

        var oven = this.app.GetOven();
        oven.TurnOn();

        var previousTemperature = oven.Temperature;
        var action = () =>
        {
            var prev = previousTemperature;
            previousTemperature = oven.Temperature;
            previousTemperature.Should().BeLessOrEqualTo(config.MaxTemperature);
            return prev < previousTemperature;
        };
        action.ShouldReturn(true, "the oven should cool down before overheating");
    }

    [Fact]
    public void TestOvenCoolingOffState()
    {
        this.app.ConfigService.GetScheduledEventsConfig().ExecutionCheck = TimeSpan.FromMilliseconds(1);
        var config = this.app.ConfigService.GetOvenConfig();
        config.AmbientTemperature = config.MinTemperature - config.TemperatureIncreasePerInterval * 2;
        config.TemperatureCheckDelay = TimeSpan.FromMilliseconds(1);
        this.app.Reinitialize();
        this.app.GetEventHub().Subscribe<OvenTemperatureChangedEvent>(x =>
        {
            x.CurrentTemperature.Should().BeGreaterOrEqualTo(config.AmbientTemperature);
            x.PreviousTemperature.Should().BeGreaterOrEqualTo(config.AmbientTemperature);
            x.CurrentTemperature.Should().BeLessOrEqualTo(config.MaxTemperature);
            x.PreviousTemperature.Should().BeLessOrEqualTo(config.MaxTemperature);
        });

        var oven = this.app.GetOven();
        var action = () => oven.IsHeated;

        oven.TurnOn();
        action.ShouldReturn(true, "the oven should heat up while it is on");

        oven.TurnOff();
        action.ShouldReturn(false, "the oven should cool down while it is off");
    }
    #endregion

    #region Private fields and constants
    private readonly IApp app;
    #endregion
}

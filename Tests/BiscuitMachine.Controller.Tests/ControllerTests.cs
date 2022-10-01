using AppBrix;
using AppBrix.Configuration.Memory;
using BiscuitMachine.Controller.Contracts;
using BiscuitMachine.Controller.Services;
using BiscuitMachine.Extruder.Contracts;
using BiscuitMachine.Motor.Contracts;
using BiscuitMachine.Oven.Contracts;
using BiscuitMachine.Stamper.Contracts;
using BiscuitMachine.Testing;
using FluentAssertions;
using System;
using Xunit;

namespace BiscuitMachine.Controller.Tests;

public class ControllerTests : IDisposable
{
    #region Setup and cleanup
    public ControllerTests()
    {
        this.app = App.Create<TestMainModule<ControllerModule>>(new MemoryConfigService());
        var ovenConfig = this.app.ConfigService.GetOvenConfig();
        ovenConfig.AmbientTemperature = ovenConfig.MinTemperature - 2 * ovenConfig.TemperatureIncreasePerInterval;
        ovenConfig.TemperatureCheckDelay = TimeSpan.FromMilliseconds(1);
        this.app.ConfigService.GetMotorConfig().PulseDelay = TimeSpan.FromMilliseconds(1);
        this.app.ConfigService.GetScheduledEventsConfig().ExecutionCheck = TimeSpan.FromMilliseconds(1);
        this.app.Start();
    }

    public void Dispose() => this.app.Stop();
    #endregion

    #region Tests
    [Fact]
    public void TestGetController()
    {
        IController controller = null;

        var action = () => controller = this.app.GetController();

        action.Should().NotThrow("an controller should have been registered");
        controller.Should().NotBeNull("an controller should have been registered");
    }

    [Fact]
    public void TestControllerPause()
    {
        var controller = this.app.GetController();

        controller.Pause();

        controller.State.Should().Be(ControllerState.Pause, "the controller should have been paused");

        controller.Pause();

        controller.State.Should().Be(ControllerState.Pause, "the controller should stay paused after a second call");
    }

    [Fact]
    public void TestControllerTurnOn()
    {
        var controller = this.app.GetController();

        controller.TurnOn();

        controller.State.Should().Be(ControllerState.On, "the controller should have been turned on");

        controller.TurnOn();

        controller.State.Should().Be(ControllerState.On, "the controller should stay turned on after a second call");
    }

    [Fact]
    public void TestControllerTurnOff()
    {
        var controller = this.app.GetController();
        controller.TurnOn();

        controller.TurnOff();

        controller.State.Should().Be(ControllerState.Off, "the controller should have been turned off");
    }

    [Fact]
    public void TestControllerTurnOnInitially()
    {
        var controller = this.app.GetController();
        var extruder = this.app.GetExtruder();
        var motor = this.app.GetMotor();
        var oven = this.app.GetOven();
        var stamper = this.app.GetStamper();

        controller.TurnOn();

        extruder.State.Should().Be(ExtruderState.Off, "the extruder should be off before the oven is heated");
        motor.State.Should().Be(MotorState.Off, "the motor should be off before the oven is heated");
        oven.State.Should().Be(OvenState.On, "the oven should be turned on as soon as the controller is");
        stamper.State.Should().Be(StamperState.Off, "the stamper should be off before the oven is heated");

        var heatingDone = () => oven.IsHeated;
        heatingDone.ShouldReturn(true, "the oven should heat up after the controller is switched on");
        
        extruder.State.Should().Be(ExtruderState.On, "the extruder should be turned on after the oven is heated");
        motor.State.Should().Be(MotorState.On, "the motor should be turned on after the oven is heated");
        oven.State.Should().Be(OvenState.On, "the oven should stay on after it is heated");
        stamper.State.Should().Be(StamperState.On, "the stamper should be turned on after the oven is heated");
    }

    [Fact]
    public void TestControllerPauseRunningConveyor()
    {
        var conveyor = this.app.GetConveyor();
        var controller = this.app.GetController();
        var extruder = this.app.GetExtruder();
        var motor = this.app.GetMotor();
        var oven = this.app.GetOven();
        var stamper = this.app.GetStamper();

        controller.TurnOn();

        var hasBiscuits = () => conveyor.HasBiscuits;
        hasBiscuits.ShouldReturn(true, "the conveyor should start receiving biscuits after the controller is turned on");

        controller.Pause();

        extruder.State.Should().Be(ExtruderState.Off, "the extruder should be off while the controller is paused");
        motor.State.Should().Be(MotorState.On, "the motor should stay on while the controller is paused");
        oven.State.Should().Be(OvenState.On, "the oven should stay on while the controller is paused");
        stamper.State.Should().Be(StamperState.On, "the stamper should stay on while the controller is paused");

        hasBiscuits.ShouldReturn(false, "the machine should process all cookies before stopping components");
        
        extruder.State.Should().Be(ExtruderState.Off, "the extruder should be off after there are no more biscuits");
        motor.State.Should().Be(MotorState.Off, "the motor should be off after there are no more biscuits");
        oven.State.Should().Be(OvenState.On, "the oven should stay on after there are no more biscuits");
        stamper.State.Should().Be(StamperState.Off, "the stamper should be off after there are no more biscuits");
    }

    [Fact]
    public void TestControllerTurnOffRunningConveyor()
    {
        var conveyor = this.app.GetConveyor();
        var controller = this.app.GetController();
        var extruder = this.app.GetExtruder();
        var motor = this.app.GetMotor();
        var oven = this.app.GetOven();
        var stamper = this.app.GetStamper();

        controller.TurnOn();

        var hasBiscuits = () => conveyor.HasBiscuits;
        hasBiscuits.ShouldReturn(true, "the conveyor should start receiving biscuits after the controller is turned on");

        controller.TurnOff();

        extruder.State.Should().Be(ExtruderState.Off, "the extruder should be off while the controller is off");
        motor.State.Should().Be(MotorState.Off, "the motor should be off while the controller is off");
        oven.State.Should().Be(OvenState.Off, "the oven should be off while the controller is off");
        stamper.State.Should().Be(StamperState.Off, "the stamper should be off while the controller is off");

        conveyor.HasBiscuits.Should().Be(true, "emergency stop should not clean the conveyor belt");
    }
    #endregion

    #region Private fields and constants
    private readonly IApp app;
    #endregion
}

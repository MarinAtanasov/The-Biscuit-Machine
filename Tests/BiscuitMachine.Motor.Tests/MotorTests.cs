using AppBrix;
using AppBrix.Configuration;
using AppBrix.Configuration.Memory;
using BiscuitMachine.Motor.Contracts;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Motor.Services;
using BiscuitMachine.Testing;
using FluentAssertions;
using System;
using Xunit;

namespace BiscuitMachine.Motor.Tests;

public class MotorTests : IDisposable
{
    #region Setup and cleanup
    public MotorTests() => this.app = App.Start<TestMainModule<MotorModule>>(new MemoryConfigService());

    public void Dispose() => this.app.Stop();
    #endregion

    #region Tests
    [Fact]
    public void TestGetMotor()
    {
        IMotor motor = null;

        var action = () => motor = this.app.GetMotor();

        action.Should().NotThrow("a motor should have been registered");
        motor.Should().NotBeNull("a motor should have been registered");
    }
    
    [Fact]
    public void TestGetMotorConfig()
    {
        IConfig config = null;

        var action = () => config = this.app.ConfigService.GetMotorConfig();

        action.Should().NotThrow("a motor config should be accessible");
        config.Should().NotBeNull("a motor config should be accessible");
    }

    [Fact]
    public void TestMotorDefaultState()
    {
        this.app.GetMotor().State.Should().Be(MotorState.Off, "the motor should be off by default");
    }
    
    [Fact]
    public void TestMotorTurnOn()
    {
        var motor = this.app.GetMotor();

        motor.TurnOn();

        motor.State.Should().Be(MotorState.On, "the motor should have been turned on");
    }
    
    [Fact]
    public void TestMotorTurnOff()
    {
        var motor = this.app.GetMotor();
        motor.TurnOn();

        motor.TurnOff();

        motor.State.Should().Be(MotorState.Off, "the motor should have been turned off");
    }
    
    [Fact]
    public void TestMotorExecuteRevolutionOffState()
    {
        var motor = this.app.GetMotor();
        var pulseCalled = false;
        this.app.GetEventHub().Subscribe<PulseEvent>(_ => pulseCalled = true);
        
        motor.ExecuteRevolution();

        pulseCalled.Should().Be(false, "pulse should not be called when the motor is off");
    }
    
    [Fact]
    public void TestMotorExecuteRevolutionOnState()
    {
        this.app.ConfigService.GetConveyorConfig().ConveyorLength = 1;
        this.app.Reinitialize();
        var conveyor = this.app.GetConveyor();
        var motor = this.app.GetMotor();
        var pulseCalled = false;
        this.app.GetEventHub().Subscribe<PulseEvent>(_ =>
        {
            pulseCalled = true;
            conveyor.HasBiscuits.Should().BeFalse("the cookie should have been pushed out of the conveyor");
        });
        motor.TurnOn();
        
        conveyor.AddBiscuit();
        motor.ExecuteRevolution();

        pulseCalled.Should().Be(true, "pulse should be called when the motor is on");
    }

    [Fact]
    public void TestMotorScheduledPulse()
    {
        this.app.ConfigService.GetScheduledEventsConfig().ExecutionCheck = TimeSpan.FromMilliseconds(1);
        this.app.ConfigService.GetMotorConfig().PulseDelay = TimeSpan.FromMilliseconds(1);
        this.app.Reinitialize();

        var motor = this.app.GetMotor();
        var pulseCalled = false;
        this.app.GetEventHub().Subscribe<PulseEvent>(_ => pulseCalled = true);
        motor.TurnOn();

        var func = () => pulseCalled;
        func.ShouldReturn(true, "pulse should be called on a timer");
    }

    [Fact]
    public void TestMotorScheduledRepeatedPulses()
    {
        this.app.ConfigService.GetScheduledEventsConfig().ExecutionCheck = TimeSpan.FromMilliseconds(1);
        this.app.ConfigService.GetMotorConfig().PulseDelay = TimeSpan.FromMilliseconds(1);
        this.app.Reinitialize();

        var motor = this.app.GetMotor();
        var pulseCalled = 0;
        this.app.GetEventHub().Subscribe<PulseEvent>(_ => pulseCalled = Math.Min(2, pulseCalled + 1));
        motor.TurnOn();

        var func = () => pulseCalled;
        func.ShouldReturn(2, "pulse should be called repeatedly on a timer");
    }
    #endregion

    #region Private fields and constants
    private readonly IApp app;
    #endregion
}

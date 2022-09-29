using AppBrix;
using AppBrix.Configuration.Memory;
using BiscuitMachine.Extruder.Contracts;
using BiscuitMachine.Extruder.Services;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Testing;
using FluentAssertions;
using System;
using Xunit;

namespace BiscuitMachine.Extruder.Tests;

public class ExtruderTests : IDisposable
{
    #region Setup and cleanup
    public ExtruderTests() => this.app = App.Start<TestMainModule<ExtruderModule>>(new MemoryConfigService());

    public void Dispose() => this.app.Stop();
    #endregion

    #region Tests
    [Fact]
    public void TestGetExtruder()
    {
        IExtruder extruder = null;

        var action = () => extruder = this.app.GetExtruder();

        action.Should().NotThrow("an extruder should have been registered");
        extruder.Should().NotBeNull("an extruder should have been registered");
    }

    [Fact]
    public void TestExtruderTurnOn()
    {
        var extruder = this.app.GetExtruder();

        extruder.TurnOn();

        extruder.State.Should().Be(ExtruderState.On, "the extruder should have been turned on");
    }
    
    [Fact]
    public void TestExtruderTurnOff()
    {
        var extruder = this.app.GetExtruder();
        extruder.TurnOn();

        extruder.TurnOff();

        extruder.State.Should().Be(ExtruderState.Off, "the extruder should have been turned off");
    }

    [Fact]
    public void TestPulseOffState()
    {
        this.app.GetEventHub().Raise(new PulseEvent());

        this.app.GetConveyor().HasBiscuits.Should().BeFalse("the extruder is off");
    }

    [Fact]
    public void TestPulseOnState()
    {
        this.app.GetExtruder().TurnOn();

        this.app.GetEventHub().Raise(new PulseEvent());

        this.app.GetConveyor().HasBiscuits.Should().BeTrue("the extruder is on");
    }
    #endregion

    #region Private fields and constants
    private readonly IApp app;
    #endregion
}

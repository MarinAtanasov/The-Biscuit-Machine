using AppBrix;
using AppBrix.Configuration.Memory;
using BiscuitMachine.Conveyor.Contracts;
using BiscuitMachine.Motor.Events;
using BiscuitMachine.Stamper.Contracts;
using BiscuitMachine.Stamper.Services;
using BiscuitMachine.Testing;
using FluentAssertions;
using System;
using Xunit;

namespace BiscuitMachine.Stamper.Tests;

public class StamperTests : IDisposable
{
    #region Setup and cleanup
    public StamperTests() => this.app = App.Start<TestMainModule<StamperModule>>(new MemoryConfigService());

    public void Dispose() => this.app.Stop();
    #endregion

    #region Tests
    [Fact]
    public void TestGetStamper()
    {
        IStamper stamper = null;

        var action = () => stamper = this.app.GetStamper();

        action.Should().NotThrow("an stamper should have been registered");
        stamper.Should().NotBeNull("an stamper should have been registered");
    }

    [Fact]
    public void TestStamperTurnOn()
    {
        var stamper = this.app.GetStamper();

        stamper.TurnOn();

        stamper.State.Should().Be(StamperState.On, "the stamper should have been turned on");

        stamper.TurnOn();

        stamper.State.Should().Be(StamperState.On, "the stamper should stay turned on after a second call");
    }
    
    [Fact]
    public void TestStamperTurnOff()
    {
        var stamper = this.app.GetStamper();
        stamper.TurnOn();

        stamper.TurnOff();

        stamper.State.Should().Be(StamperState.Off, "the stamper should have been turned off");
    }

    [Fact]
    public void TestPulseOffState()
    {
        this.app.GetConveyor().AddBiscuit();

        this.app.GetEventHub().Raise(new PulseEvent());

        var biscuit = this.app.GetConveyor().GetBiscuit(0);
        biscuit.Should().NotBeNull("the stamper shouldn't move the biscuit");
        biscuit!.State.Should().Be(BiscuitState.Raw, "the stamper is off");
    }

    [Fact]
    public void TestPulseOnState()
    {
        this.app.GetStamper().TurnOn();
        this.app.GetConveyor().AddBiscuit();

        this.app.GetEventHub().Raise(new PulseEvent());

        var biscuit = this.app.GetConveyor().GetBiscuit(0);
        biscuit.Should().NotBeNull("the stamper shouldn't move the biscuit");
        biscuit!.State.Should().Be(BiscuitState.Stamped, "the stamper is on");
    }
    #endregion

    #region Private fields and constants
    private readonly IApp app;
    #endregion
}

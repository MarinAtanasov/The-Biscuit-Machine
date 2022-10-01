using AppBrix;
using AppBrix.Configuration;
using AppBrix.Configuration.Memory;
using BiscuitMachine.Conveyor.Contracts;
using BiscuitMachine.Testing;
using BiscuitMachine.Conveyor.Events;
using BiscuitMachine.Conveyor.Services;
using FluentAssertions;
using System;
using Xunit;

namespace BiscuitMachine.Conveyor.Tests;

public class ConveyorTests : IDisposable
{
    #region Setup and cleanup
    public ConveyorTests() => this.app = App.Start<TestMainModule<ConveyorModule>>(new MemoryConfigService());

    public void Dispose() => this.app.Stop();
    #endregion

    #region Tests
    [Fact]
    public void TestGetConveyor()
    {
        IConveyor conveyor = null;

        var action = () => conveyor = this.app.GetConveyor();

        action.Should().NotThrow("a conveyor should have been registered");
        conveyor.Should().NotBeNull("a conveyor should have been registered");
    }

    [Fact]
    public void TestGetConveyorConfig()
    {
        IConfig config = null;

        var action = () => config = this.app.ConfigService.GetConveyorConfig();

        action.Should().NotThrow("a conveyor config should be accessible");
        config.Should().NotBeNull("a conveyor config should be accessible");
    }

    [Fact]
    public void TestConveyorGetLength()
    {
        var config = this.app.ConfigService.GetConveyorConfig();
        var conveyor = this.app.GetConveyor();

        conveyor.Length.Should().Be(config.ConveyorLength, "the conveyor's length should be the same as in the config");
    }

    [Fact]
    public void TestConveyorGetLengthChangedConfig()
    {
        var config = this.app.ConfigService.GetConveyorConfig();
        config.ConveyorLength *= 2;
        var conveyor = this.app.GetConveyor();

        conveyor.Length.Should().Be(config.ConveyorLength / 2, "the conveyor's length should be a snapshot of the original value");
    }

    [Fact]
    public void TestConveyorAddBiscuit()
    {
        var conveyor = this.app.GetConveyor();

        conveyor.AddBiscuit();

        conveyor.HasBiscuits.Should().BeTrue("a biscuit has been added to the conveyor");
    }

    [Fact]
    public void TestConveyorGetBiscuit()
    {
        var conveyor = this.app.GetConveyor();
        conveyor.AddBiscuit();

        conveyor.GetBiscuit(0).Should().NotBeNull("a biscuit has been added on the first slot");
        conveyor.GetBiscuit(1).Should().BeNull("a biscuit has not been added on the second slot");
    }

    [Fact]
    public void TestConveyorHasBiscuitsEmptyConveyor()
    {
        this.app.GetConveyor().HasBiscuits.Should().BeFalse("no biscuits have been added to the conveyor");
    }

    [Fact]
    public void TestConveyorHasBiscuitsOneBiscuit()
    {
        var conveyor = this.app.GetConveyor();
        var config = this.app.ConfigService.GetConveyorConfig();

        conveyor.AddBiscuit();
        for (var i = 0; i < config.ConveyorLength; i++)
        {
            conveyor.HasBiscuits.Should().BeTrue("the biscuit should be still on the conveyor belt");
            conveyor.Move();
        }

        conveyor.HasBiscuits.Should().BeFalse("the biscuit should no longer be on the conveyor belt");
    }

    [Fact]
    public void TestBiscuitReadyEvent()
    {
        var conveyor = this.app.GetConveyor();
        var config = this.app.ConfigService.GetConveyorConfig();
        Biscuit biscuit = null;
        this.app.GetEventHub().Subscribe<BiscuitReadyEvent>(x => biscuit = x.Biscuit);
        conveyor.AddBiscuit();
        var initialBiscuit = conveyor.GetBiscuit(0);

        for (var i = 0; i < config.ConveyorLength; i++)
        {
            biscuit.Should().BeNull("the biscuit should not have reached the end of the conveyor");
            conveyor.Move();
        }

        biscuit.Should().NotBeNull("the biscuit should have reached the end of the conveyor");
        biscuit.Should().BeSameAs(initialBiscuit, "the returned biscuit should be the same as the interted");
    }

    [Fact]
    public void TestGetBiscuitEmptyConveyor()
    {
        this.app.GetConveyor().GetBiscuit(0).Should().BeNull("no biscuit have been added");
    }
    #endregion

    #region Private fields and constants
    private readonly IApp app;
    #endregion
}

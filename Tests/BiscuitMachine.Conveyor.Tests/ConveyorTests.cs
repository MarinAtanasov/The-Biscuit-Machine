using AppBrix;
using AppBrix.Configuration;
using AppBrix.Configuration.Memory;
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
    public void TestConveyorAddBiscuit()
    {
        var conveyor = this.app.GetConveyor();

        conveyor.AddBiscuit();

        conveyor.HasBiscuits.Should().BeTrue("a biscuit has been added to the conveyor");
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
        var biscuitReady = false;
        this.app.GetEventHub().Subscribe<BiscuitReadyEvent>(_ => biscuitReady = true);

        conveyor.AddBiscuit();
        for (var i = 0; i < config.ConveyorLength; i++)
        {
            biscuitReady.Should().BeFalse("the biscuit should not have reached the end of the conveyor");
            conveyor.Move();
        }

        biscuitReady.Should().BeTrue("the biscuit should have reached the end of the conveyor");
    }
    #endregion

    #region Private fields and constants
    private readonly IApp app;
    #endregion
}

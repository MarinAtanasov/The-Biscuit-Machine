using BiscuitMachine.Conveyor.Contracts;
using FluentAssertions;
using System;
using Xunit;

namespace BiscuitMachine.Conveyor.Tests;

public sealed class BiscuitTests
{
    #region Tests
    [Fact]
    public void TestBiscuitInitialState()
    {
        new Biscuit().State.Should().Be(BiscuitState.Raw, "the biscuit should start from a raw state");
    }

    [Fact]
    public void TestBiscuitStampFromRawState()
    {
        var biscuit = new Biscuit();

        biscuit.Stamp();

        biscuit.State.Should().Be(BiscuitState.Stamped, "the raw biscuit should be successfully stamped");
    }
    
    [Fact]
    public void TestBiscuitStampFromStampedState()
    {
        var biscuit = new Biscuit(BiscuitState.Stamped);

        var action = () => biscuit.Stamp();

        action.Should().Throw<InvalidOperationException>("the biscuit is already stamped");
    }
    
    [Fact]
    public void TestBiscuitStampFromUnderbakedState()
    {
        var biscuit = new Biscuit(BiscuitState.Underbaked);

        var action = () => biscuit.Stamp();

        action.Should().Throw<InvalidOperationException>("cannot stamp an underbaked biscuit");
    }
    
    [Fact]
    public void TestBiscuitStampFromBakedState()
    {
        var biscuit = new Biscuit(BiscuitState.Baked);

        var action = () => biscuit.Stamp();

        action.Should().Throw<InvalidOperationException>("cannot stamp a baked biscuit");
    }
    
    [Fact]
    public void TestBiscuitStampFromOverbakedState()
    {
        var biscuit = new Biscuit(BiscuitState.Overbaked);

        var action = () => biscuit.Stamp();

        action.Should().Throw<InvalidOperationException>("cannot stamp an overbaked biscuit");
    }
    
    [Fact]
    public void TestBiscuitStampFromBurntState()
    {
        var biscuit = new Biscuit(BiscuitState.Burnt);

        var action = () => biscuit.Stamp();

        action.Should().Throw<InvalidOperationException>("cannot stamp a burnt biscuit");
    }

    [Fact]
    public void TestBiscuitBakeFromRawState()
    {
        var biscuit = new Biscuit();

        var action = () => biscuit.Bake();

        action.Should().Throw<InvalidOperationException>("cannot bake an unstamped biscuit");
    }
    
    [Fact]
    public void TestBiscuitBakeFromStampedState()
    {
        var biscuit = new Biscuit(BiscuitState.Stamped);
        
        biscuit.Bake();

        biscuit.State.Should().Be(BiscuitState.Underbaked, "the stamped biscuit should have started to bake");
    }
    
    [Fact]
    public void TestBiscuitBakeFromUnderbakedState()
    {
        var biscuit = new Biscuit(BiscuitState.Underbaked);
        
        biscuit.Bake();

        biscuit.State.Should().Be(BiscuitState.Baked, "the underbaked biscuit should have been baked");
    }
    
    [Fact]
    public void TestBiscuitBakeFromBakedState()
    {
        var biscuit = new Biscuit(BiscuitState.Baked);
        
        biscuit.Bake();

        biscuit.State.Should().Be(BiscuitState.Overbaked, "the baked biscuit should have become overbaked");
    }
    
    [Fact]
    public void TestBiscuitBakeFromOverbakedState()
    {
        var biscuit = new Biscuit(BiscuitState.Overbaked);
        
        biscuit.Bake();

        biscuit.State.Should().Be(BiscuitState.Burnt, "the overbaked biscuit should have become burnt");
    }
    
    [Fact]
    public void TestBiscuitBakeFromBurntState()
    {
        var biscuit = new Biscuit(BiscuitState.Burnt);
        
        biscuit.Bake();

        biscuit.State.Should().Be(BiscuitState.Burnt, "the burnt biscuit should stay burnt");
    }
    #endregion
}

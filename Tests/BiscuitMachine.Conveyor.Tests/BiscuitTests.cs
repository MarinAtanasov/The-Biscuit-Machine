using BiscuitMachine.Conveyor.Contracts;
using FluentAssertions;
using System;
using Xunit;

namespace BiscuitMachine.Conveyor.Tests;

public class BiscuitTests
{
    #region Tests
    [Fact]
    public void TestBiscuitInitialState()
    {
        new Biscuit().State.Should().Be(BiscuitState.Raw, "the biscuit should start from a raw state");
    }

    [Fact]
    public void TestBiscuitStamp()
    {
        var biscuit = new Biscuit();

        biscuit.Stamp();

        biscuit.State.Should().Be(BiscuitState.Stamped, "the raw biscuit should be successfully stamped");
    }
    
    [Fact]
    public void TestBiscuitStampFromStampedState()
    {
        var biscuit = new Biscuit();
        biscuit.Stamp();

        var action = () => biscuit.Stamp();

        action.Should().Throw<InvalidOperationException>("the biscuit is already stamped");
    }
    #endregion
}

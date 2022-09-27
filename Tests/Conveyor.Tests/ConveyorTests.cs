using AppBrix;
using AppBrix.Configuration.Memory;
using BiscuitMachine.Testing;
using Conveyor.Configuration;
using Conveyor.Events;
using Conveyor.Services;
using FluentAssertions;
using System;
using Xunit;

namespace Conveyor.Tests;

public class ConveyorTests : IDisposable
{
    #region Setup and cleanup
    public ConveyorTests()
    {
        this.app = App.Start<TestMainModule<ConveyorModule>>(new MemoryConfigService());
    }

    public void Dispose() => this.app.Stop();
    #endregion

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
        ConveyorConfig config = null;

        var action = () => config = this.app.ConfigService.GetConveyorConfig();

        action.Should().NotThrow("a conveyor config should be accessible");
        config.Should().NotBeNull("a conveyor config should be accessible");
    }

    [Fact]
    public void TestConveyorAddCookie()
    {
        var conveyor = this.app.GetConveyor();

        conveyor.AddCookie();

        conveyor.HasCookies.Should().BeTrue("a cookie has been added to the conveyor");
    }

    [Fact]
    public void TestConveyorHasCookiesEmptyConveyor()
    {
        this.app.GetConveyor().HasCookies.Should().BeFalse("no cookies have been added to the conveyor");
    }

    [Fact]
    public void TestConveyorHasCookieOneCookie()
    {
        var conveyor = this.app.GetConveyor();
        var config = this.app.ConfigService.GetConveyorConfig();

        conveyor.AddCookie();
        for (var i = 0; i < config.ConveyorLength; i++)
        {
            conveyor.HasCookies.Should().BeTrue("the cookie should be still on the conveyor belt");
            conveyor.Move();
        }

        conveyor.HasCookies.Should().BeFalse("the cookie should no longer be on the conveyor belt");
    }

    [Fact]
    public void TestCookieReadyEvent()
    {
        var conveyor = this.app.GetConveyor();
        var config = this.app.ConfigService.GetConveyorConfig();
        var cookieReady = false;
        this.app.GetEventHub().Subscribe<CookieReadyEvent>(_ => cookieReady = true);

        conveyor.AddCookie();
        for (var i = 0; i < config.ConveyorLength; i++)
        {
            cookieReady.Should().BeFalse("the cookie should not have reached the end of the conveyor");
            conveyor.Move();
        }

        cookieReady.Should().BeTrue("the cookie should have reached the end of the conveyor");
    }

    #region Private fields and constants
    private readonly IApp app;
    #endregion
}

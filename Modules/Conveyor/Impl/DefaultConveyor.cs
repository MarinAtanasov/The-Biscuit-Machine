using AppBrix;
using AppBrix.Lifecycle;
using Conveyor.Events;
using Conveyor.Services;
using System;
using System.Linq;

namespace Conveyor.Impl;

internal sealed class DefaultConveyor : IApplicationLifecycle, IConveyor
{
    #region IApplicationLifecycle implementation
    public void Initialize(IInitializeContext context)
    {
        this.app = context.App;
        this.belt = new bool[this.app.ConfigService.GetConveyorConfig().ConveyorLength];
    }

    public void Uninitialize()
    {
        this.belt = Array.Empty<bool>();
        this.app = null;
    }
    #endregion

    #region Properties
    public bool HasCookies => this.belt.Any(x => x);
    #endregion

    #region Public and overriden methods
    public void AddCookie()
    {
        this.belt[0] = true;
    }

    public void Move()
    {
        var cookieReady = this.belt[^1];
        for (var i = this.belt.Length - 1; i > 0; i--)
        {
            this.belt[i] = this.belt[i - 1];
        }

        this.belt[0] = false;
        if (cookieReady)
        {
            this.app.GetEventHub().Raise(new CookieReadyEvent());
        }
    }
    #endregion

    #region Private field and constants
    #nullable disable
    private IApp app;
    #nullable restore
    private bool[] belt = Array.Empty<bool>();
    #endregion
}

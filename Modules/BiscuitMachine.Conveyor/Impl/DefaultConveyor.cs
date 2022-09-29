using AppBrix;
using AppBrix.Lifecycle;
using BiscuitMachine.Conveyor.Contracts;
using BiscuitMachine.Conveyor.Events;
using BiscuitMachine.Conveyor.Services;
using System;
using System.Linq;

namespace BiscuitMachine.Conveyor.Impl;

internal sealed class DefaultConveyor : IApplicationLifecycle, IConveyor
{
    #region IApplicationLifecycle implementation
    public void Initialize(IInitializeContext context)
    {
        this.app = context.App;
        this.belt = new Biscuit?[this.app.ConfigService.GetConveyorConfig().ConveyorLength];
    }

    public void Uninitialize()
    {
        this.belt = Array.Empty<Biscuit?>();
        this.app = null;
    }
    #endregion

    #region Properties
    public bool HasBiscuits => this.belt.Any(x => x is not null);

    public int Length => this.belt.Length;
    #endregion

    #region Public and overriden methods
    public void AddBiscuit()
    {
        this.belt[0] = new Biscuit();
    }

    public Biscuit? GetBiscuit(int index) => this.belt[index];

    public void Move()
    {
        var biscuit = this.belt[^1];
        for (var i = this.belt.Length - 1; i > 0; i--)
        {
            this.belt[i] = this.belt[i - 1];
        }

        this.belt[0] = null;
        if (biscuit is not null)
        {
            this.app.GetEventHub().Raise(new BiscuitReadyEvent(biscuit));
        }
    }
    #endregion

    #region Private field and constants
    #nullable disable
    private IApp app;
    #nullable restore
    private Biscuit?[] belt = Array.Empty<Biscuit?>();
    #endregion
}

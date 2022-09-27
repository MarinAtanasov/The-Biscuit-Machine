using AppBrix.Events;
using AppBrix.Lifecycle;
using AppBrix.Modules;
using BiscuitMachine.Conveyor.Impl;
using System;
using System.Collections.Generic;

namespace BiscuitMachine.Conveyor;

/// <summary>
/// Module used for working with a conveyor.
/// </summary>
public class ConveyorModule : ModuleBase
{
    #region Properties
    /// <summary>
    /// Gets the types of the modules which are direct dependencies for the current module.
    /// This is used to determine the order in which the modules are loaded.
    /// </summary>
    public override IEnumerable<Type> Dependencies => new[]
    {
        typeof(EventsModule)
    };
    #endregion

    #region Public and overriden methods
    /// <summary>
    /// Initializes the module.
    /// Automatically called by <see cref="ModuleBase.Initialize"/>
    /// </summary>
    /// <param name="context">The initialization context.</param>
    protected override void Initialize(IInitializeContext context)
    {
        this.App.Container.Register(this);
        this.conveyor.Initialize(context);
        this.App.Container.Register(this.conveyor);
    }

    /// <summary>
    /// Uninitializes the module.
    /// Automatically called by <see cref="ModuleBase.Uninitialize"/>
    /// </summary>
    protected override void Uninitialize()
    {
        this.conveyor.Uninitialize();
    }
    #endregion

    #region Private fields and constants
    private readonly DefaultConveyor conveyor = new DefaultConveyor();
    #endregion
}

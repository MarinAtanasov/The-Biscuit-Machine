using AppBrix.Lifecycle;
using AppBrix.Modules;
using BiscuitMachine.Conveyor;
using BiscuitMachine.Extruder.Impl;
using BiscuitMachine.Motor;
using System;
using System.Collections.Generic;

namespace BiscuitMachine.Extruder;

/// <summary>
/// Module used for working with a conveyor belt extruder.
/// </summary>
public sealed class ExtruderModule : ModuleBase
{
    #region Properties
    /// <summary>
    /// Gets the types of the modules which are direct dependencies for the current module.
    /// This is used to determine the order in which the modules are loaded.
    /// </summary>
    public override IEnumerable<Type> Dependencies => new[]
    {
        typeof(ConveyorModule),
        typeof(MotorModule)
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
        this.extruder.Initialize(context);
        this.App.Container.Register(this.extruder);
    }

    /// <summary>
    /// Uninitializes the module.
    /// Automatically called by <see cref="ModuleBase.Uninitialize"/>
    /// </summary>
    protected override void Uninitialize()
    {
        this.extruder.Uninitialize();
    }
    #endregion

    #region Private fields and constants
    private readonly DefaultExtruder extruder = new DefaultExtruder();
    #endregion
}

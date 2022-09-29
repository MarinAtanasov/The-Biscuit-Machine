using AppBrix.Lifecycle;
using AppBrix.Modules;
using BiscuitMachine.Conveyor;
using BiscuitMachine.Motor;
using BiscuitMachine.Stamper.Impl;
using System;
using System.Collections.Generic;

namespace BiscuitMachine.Stamper;

/// <summary>
/// Module used for working with a conveyor belt stamper.
/// </summary>
public class StamperModule : ModuleBase
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

        this.stamper.Initialize(context);
        this.App.Container.Register(this.stamper);
    }

    /// <summary>
    /// Uninitializes the module.
    /// Automatically called by <see cref="ModuleBase.Uninitialize"/>
    /// </summary>
    protected override void Uninitialize()
    {
        this.stamper.Uninitialize();
    }
    #endregion

    #region Private fields and constants
    private DefaultStamper stamper = new DefaultStamper();
    #endregion
}

using AppBrix.Lifecycle;
using AppBrix.Modules;
using BiscuitMachine.Controller.Impl;
using BiscuitMachine.Conveyor;
using BiscuitMachine.Extruder;
using BiscuitMachine.Motor;
using BiscuitMachine.Oven;
using BiscuitMachine.Stamper;
using System;
using System.Collections.Generic;

namespace BiscuitMachine.Controller;

/// <summary>
/// Module used for working with a conveyor belt controller.
/// </summary>
public class ControllerModule : ModuleBase
{
    #region Properties
    /// <summary>
    /// Gets the types of the modules which are direct dependencies for the current module.
    /// This is used to determine the order in which the modules are loaded.
    /// </summary>
    public override IEnumerable<Type> Dependencies => new[]
    {
        typeof(ConveyorModule),
        typeof(ExtruderModule),
        typeof(MotorModule),
        typeof(OvenModule),
        typeof(StamperModule)
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

        this.controller.Initialize(context);
        this.App.Container.Register(this.controller);
    }

    /// <summary>
    /// Uninitializes the module.
    /// Automatically called by <see cref="ModuleBase.Uninitialize"/>
    /// </summary>
    protected override void Uninitialize()
    {
        this.controller.Uninitialize();
    }
    #endregion

    #region Private fields and constants
    private DefaultController controller = new DefaultController();
    #endregion
}

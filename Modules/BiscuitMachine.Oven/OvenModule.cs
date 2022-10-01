using AppBrix.Events.Schedule.Timer;
using AppBrix.Lifecycle;
using AppBrix.Modules;
using BiscuitMachine.Conveyor;
using BiscuitMachine.Motor;
using BiscuitMachine.Oven.Impl;
using System;
using System.Collections.Generic;

namespace BiscuitMachine.Oven;

/// <summary>
/// Module used for working with a conveyor belt oven.
/// </summary>
public sealed class OvenModule : ModuleBase
{
    #region Properties
    /// <summary>
    /// Gets the types of the modules which are direct dependencies for the current module.
    /// This is used to determine the order in which the modules are loaded.
    /// </summary>
    public override IEnumerable<Type> Dependencies => new[]
    {
        typeof(ConveyorModule),
        typeof(MotorModule),
        typeof(TimerScheduledEventsModule)
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

        this.oven.Initialize(context);
        this.App.Container.Register(this.oven);
    }

    /// <summary>
    /// Uninitializes the module.
    /// Automatically called by <see cref="ModuleBase.Uninitialize"/>
    /// </summary>
    protected override void Uninitialize()
    {
        this.oven.Uninitialize();
    }
    #endregion

    #region Private fields and constants
    private DefaultOven oven = new DefaultOven();
    #endregion
}

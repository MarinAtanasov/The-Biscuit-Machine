using AppBrix.Events.Schedule.Timer;
using AppBrix.Lifecycle;
using AppBrix.Modules;
using Conveyor;
using Motor.Impl;
using System;
using System.Collections.Generic;

namespace Motor;

/// <summary>
/// Module used for working with a conveyor belt motor.
/// </summary>
public class MotorModule : ModuleBase
{
    #region Properties
    /// <summary>
    /// Gets the types of the modules which are direct dependencies for the current module.
    /// This is used to determine the order in which the modules are loaded.
    /// </summary>
    public override IEnumerable<Type> Dependencies => new[]
    {
        typeof(ConveyorModule),
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
        this.motor.Initialize(context);
        this.App.Container.Register(this.motor);
    }

    /// <summary>
    /// Uninitializes the module.
    /// Automatically called by <see cref="ModuleBase.Uninitialize"/>
    /// </summary>
    protected override void Uninitialize()
    {
        this.motor.Uninitialize();
    }
    #endregion

    #region Private fields and constants
    private readonly DefaultMotor motor = new DefaultMotor();
    #endregion
}

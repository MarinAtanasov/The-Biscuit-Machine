using AppBrix.Configuration;
using System;

namespace BiscuitMachine.Motor.Configuration;

/// <summary>
/// Configuration which sets the motor's behavior.
/// </summary>
public sealed class MotorConfig : IConfig
{
    #region Construction
    /// <summary>
    /// Creates a new instance of <see cref="MotorConfig"/>.
    /// </summary>
    public MotorConfig()
    {
        this.PulseDelay = TimeSpan.FromSeconds(1);
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets time between automatic motor pulses.
    /// </summary>
    public TimeSpan PulseDelay { get; set; }
    #endregion
}

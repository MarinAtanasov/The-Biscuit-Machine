using AppBrix.Configuration;
using System;

namespace BiscuitMachine.Oven.Configuration;

/// <summary>
/// Configuration which sets the oven's behavior.
/// </summary>
public sealed class OvenConfig : IConfig
{
    #region Construction
    /// <summary>
    /// Creates a new instance of <see cref="OvenConfig"/>.
    /// </summary>
    public OvenConfig()
    {
        this.AmbientTemperature = 20;
        this.Index = 0;
        this.MaxTemperature = 240;
        this.MinTemperature = 220;
        this.TemperatureCheckDelay = TimeSpan.FromSeconds(1);
        this.TemperatureDecreasePerInterval = 5;
        this.TemperatureIncreasePerInterval = 10;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the ambient temperature in the room.
    /// </summary>
    public int AmbientTemperature { get; set; }
    
    /// <summary>
    /// Gets or sets the oven's index on the conveyor.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the maximal required temperature for normal operation of the oven.
    /// </summary>
    public int MaxTemperature { get; set; }

    /// <summary>
    /// Gets or sets the minimal required temperature for normal operation of the oven.
    /// </summary>
    public int MinTemperature { get; set; }

    /// <summary>
    /// Gets or sets the interval between over temperature checks.
    /// </summary>
    public TimeSpan TemperatureCheckDelay { get; set; }

    /// <summary>
    /// Gets or sets the decrease in temperature per interval while the heating element is turned off.
    /// </summary>
    public int TemperatureDecreasePerInterval { get; set; }

    /// <summary>
    /// Gets or sets the increase in temperature per interval while the heating element is turned on.
    /// </summary>
    public int TemperatureIncreasePerInterval { get; set; }
    #endregion
}

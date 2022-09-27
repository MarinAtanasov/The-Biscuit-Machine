using AppBrix.Configuration;

namespace BiscuitMachine.Conveyor.Configuration;

/// <summary>
/// Configuration which sets the conveyor's behavior.
/// </summary>
public sealed class ConveyorConfig : IConfig
{
    #region Construction
    /// <summary>
    /// Creates a new instance of <see cref="ConveyorConfig"/>.
    /// </summary>
    public ConveyorConfig()
    {
        this.ConveyorLength = 6;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the conveyor's length.
    /// </summary>
    public int ConveyorLength { get; set; }
    #endregion
}

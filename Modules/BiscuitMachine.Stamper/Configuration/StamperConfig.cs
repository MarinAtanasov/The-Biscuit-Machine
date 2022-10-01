using AppBrix.Configuration;

namespace BiscuitMachine.Stamper.Configuration;

/// <summary>
/// Configuration which sets the stamper's behavior.
/// </summary>
public sealed class StamperConfig : IConfig
{
    #region Construction
    /// <summary>
    /// Creates a new instance of <see cref="StamperConfig"/>.
    /// </summary>
    public StamperConfig()
    {
        this.Index = 0;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the stamper's index on the conveyor.
    /// </summary>
    public int Index { get; set; }
    #endregion
}

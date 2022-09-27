using AppBrix.Configuration;
using BiscuitMachine.Motor.Configuration;
using BiscuitMachine.Motor.Services;

namespace AppBrix;

/// <summary>
/// Extension methods for the motor module.
/// </summary>
public static class MotorExtensions
{
    /// <summary>
    /// Gets the currently registered <see cref="IMotor"/>.
    /// </summary>
    /// <param name="app">The currently running application.</param>
    /// <returns>The <see cref="IMotor"/>.</returns>
    public static IMotor GetMotor(this IApp app) => (IMotor)app.Get(typeof(IMotor));

    /// <summary>
    /// Gets the <see cref="MotorConfig"/> from <see cref="MotorConfig"/>.
    /// </summary>
    /// <param name="service">The configuration service.</param>
    /// <returns>The <see cref="MotorConfig"/>.</returns>
    public static MotorConfig GetMotorConfig(this IConfigService service) => (MotorConfig)service.Get(typeof(MotorConfig));
}

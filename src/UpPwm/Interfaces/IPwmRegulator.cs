namespace UpPwm.Interfaces
{
    public interface IPwmRegulator
    {
        /// <summary>
        /// Whether it has a pwm environment
        /// </summary>
        /// <returns></returns>
        bool IsHasPwmEnvironment();
        bool JudePwmExists(int port);
        void SetPwmExport(int port, bool isExport);
        void SetPwmPeriod(int port, int period);
        void SetPwmDutyCycle(int port, int duty_cycle);
        void SetPwmEnable(int port, bool isEnable);
    }
}
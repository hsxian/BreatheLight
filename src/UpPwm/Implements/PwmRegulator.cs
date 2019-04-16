using System;
using System.Linq;
using UpPwm.Interfaces;

namespace UpPwm.Implements
{
    public class PwmRegulator : IPwmRegulator
    {
        private readonly SystemCommander _sysCmd;
        private readonly int[] ports = new int[] { 0, 1, 3 };
        public PwmRegulator()
        {
            _sysCmd = new SystemCommander();
        }

        public bool JudePwmExists(int port)
        {
            return System.IO.Directory.Exists($"/sys/class/pwm/pwmchip0/pwm{port}/");
        }
        public void SetPwmExport(int port, bool isExport)
        {
            if (ports.Contains(port) && !JudePwmExists(port) && IsHasPwmEnvironment())
            {
                var str = $"echo {port} > /sys/class/pwm/pwmchip0/{(isExport ? "export" : "unexport")}";
                _sysCmd.ExecCommand(str);
            }
        }
        public void SetPwmPeriod(int port, int period)
        {
            if (ports.Contains(port) && period > 0 && JudePwmExists(port))
            {
                var str = $"echo {period} > /sys/class/pwm/pwmchip0/pwm{port}/period";
                _sysCmd.ExecCommand(str);
            }
        }
        public void SetPwmDutyCycle(int port, int duty_cycle)
        {
            if (ports.Contains(port) && duty_cycle > 0 && JudePwmExists(port))
            {
                var str = $"echo {duty_cycle} > /sys/class/pwm/pwmchip0/pwm{port}/duty_cycle";
                //Console.WriteLine(str);
                _sysCmd.ExecCommand(str);
            }
        }
        public void SetPwmEnable(int port, bool isEnable)
        {
            if (ports.Contains(port) && JudePwmExists(port))
            {
                var str = $"echo {(isEnable ? 1 : 0)} > /sys/class/pwm/pwmchip0/pwm{port}/enable";
                _sysCmd.ExecCommand(str);
            }
        }

        public bool IsHasPwmEnvironment()
        {
            return System.IO.Directory.Exists($"/sys/class/pwm/pwmchip0/");
        }
    }
}
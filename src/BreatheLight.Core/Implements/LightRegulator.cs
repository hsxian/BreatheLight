using System;
using BreatheLight.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using UpPwm.Interfaces;

namespace BreatheLight.Implements
{

    public class LightRegulator : ILightRegulator
    {
        private readonly IPwmRegulator _pwm;
        private readonly int _period = 10000;
        private readonly int _pwnPort = 1;
        private readonly IConfiguration _configuration;

        public LightRegulator(IPwmRegulator pwm, IConfiguration configuration)
        {
            _configuration = configuration;
            _pwm = pwm;
            if (int.TryParse(_configuration["UpBoard:PwmPort"], out int port))
            {
                _pwnPort = port;
            }
            else { throw new ArgumentNullException("UpBoard:PwmPort undefind in appsetting.json"); }
            if (int.TryParse(_configuration["UpBoard:PwmPeriod"], out int period))
            {
                _period = period;
            }
            else { throw new ArgumentNullException("UpBoard:PwmPort undefind in appsetting.json"); }
            _pwm.SetPwmExport(_pwnPort, true);
            _pwm.SetPwmPeriod(_pwnPort, _period);
        }

        /// <summary>
        /// 设置亮度0~100.100最亮
        /// </summary>
        /// <param name="brightness"></param>
        public void SetLightBrightness(float brightness)
        {
            if (brightness < 1)
            {
                _pwm.SetPwmEnable(_pwnPort, false);
                return;
            }
            brightness = brightness > 100 ? 100 : brightness;
            var duty_cucle = brightness / 100.0 * _period;
            _pwm.SetPwmEnable(_pwnPort, true);
            _pwm.SetPwmDutyCycle(_pwnPort, (int)duty_cucle);
        }
        public void SetLightStatus(bool isOpen)
        {
            if (isOpen) _pwm.SetPwmEnable(_pwnPort, true);
            else _pwm.SetPwmEnable(_pwnPort, false);
        }
    }
}
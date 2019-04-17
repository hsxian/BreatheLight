using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using BreatheLight.Core.Data;
using BreatheLight.Core.Interfaces;
using BreatheLight.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using BreatheLight.Implements;
using UpPwm.Implements;

namespace BreatheLight.Core.Implements
{
    public class TimeMonitor : ITimeMonitor
    {
        private readonly List<LightStatus> _statuss;
        private readonly ILightRegulator _light;
        private readonly IConfiguration _configuration;
        private readonly Timer _timer;
        private int _timeInterval = 10;
        private readonly string _logFileName;
        private DateTime _zeroOfDay = new DateTime(1, 1, 1, 0, 0, 0);
        public TimeMonitor(
            IConfiguration configuration
        )
        {
            _statuss = new List<LightStatus>();
            _light = new LightRegulator(new PwmRegulator(), configuration);
            _configuration = configuration;
            if (int.TryParse(_configuration["UpTimer:TimeInterval"], out int interver))
            {
                _timeInterval = interver;
            }
            _logFileName = _configuration["Logging:FileName"];
            _timer = new Timer(_timeInterval);
            ConfigTimer();
        }
        private void ConfigTimer()
        {
            _timer.Enabled = true;
            _timer.Elapsed += (sen, e) =>
            {
                var macths = _statuss
                    .Where(t => IsArrivalTime(t.Time, DateTime.Now))
                    .ToList();

                foreach (var item in macths)
                {
                    var str = $"{item.Time.TimeOfDay:t} : set brightness to {item.Brightness} %.";
                    Console.WriteLine(str);
                    _light.SetLightBrightness(item.Brightness);
                }
                if (IsArrivalTime(DateTime.Now, _zeroOfDay) && File.Exists(_logFileName))
                {
                    File.Move(_logFileName, $"{DateTime.Now.AddDays(-1).ToShortDateString()}-{_logFileName}");
                }
            };

        }
        public async Task RefreshLightTask(IEnumerable<LightSequence> lights)
        {
            _statuss.Clear();
            await ReadLightTask(lights);
        }
        private async Task ReadLightTask(IEnumerable<LightSequence> lights) => await Task.Run(() =>
        {
            lights?
                .ToList()
                .ForEach(t => t.ProductionSequenceByOperate()
                    .ForEach(item => AddLightTimePoint(item)));
        });

        private bool IsArrivalTime(DateTime a, DateTime b)
        {
            var abs = Math.Abs(a.TimeOfDay.TotalMilliseconds - b.TimeOfDay.TotalMilliseconds);
            var jude = abs < _timeInterval / 2.0;
            //if (jude) Console.WriteLine($@"{t.Time.TimeOfDay.TotalMilliseconds} - {DateTime.Now.TimeOfDay.TotalMilliseconds} = {abs}");
            return jude;
        }
        public void AddLightTimePoint(LightStatus timePoint)
        {
            if (timePoint != null) _statuss.Add(timePoint);
        }

        public void RemoveLightTimePoint(string id)
        {
            _statuss.Remove(_statuss.Find(t => t.Id == id));
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

    }
}
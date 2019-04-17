using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BreatheLight.Core.Models
{
    public class LightSequence
    {
        public int Id { get; set; }
        [Display(Name = "亮度A"), Range(0f, 100f)]
        public float BrightnessA { get; set; }
        [Display(Name = "亮度B"), Range(0f, 100f)]
        public float BrightnessB { get; set; }
        [Display(Name = "步长"), Range(0.001, 99.99)]
        public float BrightnessStep { get; set; }
        [Display(Name = "开始")]
        public DateTime StartTime { get; set; }
        [Display(Name = "结束")]
        public DateTime EndTime { get; set; }
        public List<LightStatus> ProductionSequenceByOperate()
        {
            var result = new List<LightStatus>();
            float a = BrightnessA, b = BrightnessB, step = BrightnessStep;
            if (StartTime.TimeOfDay.Equals(EndTime.TimeOfDay) || a.Equals(b)) return result;

            var intervel = (EndTime - StartTime).TotalSeconds / (b - a);
            Func<float, bool> jude = i => i <= b && i <= 100;
            Func<float, DateTime> getTime = i => StartTime.AddSeconds(i * intervel);

            if (BrightnessA > BrightnessB)
            {
                step = -step;
                jude = i => i >= b && i >= 0;
                getTime = i => EndTime.AddSeconds(i * intervel);
            }
            //Console.WriteLine($"st: {StartTime}, et: {EndTime}");
            #region first item
            var first = new LightStatus
            {
                Id = Guid.NewGuid().ToString(),
                Time = StartTime,
                Brightness = a
            };
            result.Add(first);
            #endregion
            #region auto generate items
            DateTime lastTime = StartTime;
            for (float i = a; jude(i); i += step)
            {
                var ls = new LightStatus
                {
                    Id = Guid.NewGuid().ToString(),
                    Time = getTime(i),
                    Brightness = i
                };
                if ((ls.Time - lastTime).TotalMilliseconds < 1) continue;
                lastTime = ls.Time;
                //Console.WriteLine($"time:{ls.Time}, light: {ls.Brightness}");
                result.Add(ls);
            }
            #endregion
            #region last item
            var last = new LightStatus
            {
                Id = Guid.NewGuid().ToString(),
                Time = EndTime,
                Brightness = b
            };
            result.Add(last);
            #endregion
            return result;
        }
    }
}
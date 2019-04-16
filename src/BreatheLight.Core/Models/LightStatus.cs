using System;
using System.Collections.Generic;

namespace BreatheLight.Core.Models
{
    public class LightStatus
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public float Brightness { get; set; }
    }
}
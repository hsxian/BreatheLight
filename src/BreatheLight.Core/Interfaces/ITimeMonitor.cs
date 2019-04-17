using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreatheLight.Core.Models;

namespace BreatheLight.Core.Interfaces
{
    public interface ITimeMonitor
    {
        void AddLightTimePoint(LightStatus timePoint);
        void RemoveLightTimePoint(string id);
        void Start();
        void Stop();
        Task RefreshLightTask(IEnumerable<LightSequence> lights);
    }
}
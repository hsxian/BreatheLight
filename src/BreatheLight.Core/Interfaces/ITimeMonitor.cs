using System;
using BreatheLight.Core.Models;

namespace BreatheLight.Core.Interfaces
{
    public interface ITimeMonitor
    {
        void AddLightTimePoint(LightStatus timePoint);
        void RemoveLightTimePoint(string id);
        void Start();
        void Stop();
        void RefreshLightTask();
    }
}
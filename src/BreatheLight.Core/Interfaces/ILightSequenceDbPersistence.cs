using System.Collections.Generic;
using System.Threading.Tasks;
using BreatheLight.Core.Models;

namespace BreatheLight.Core.Interfaces
{
    public interface ILightSequenceDbPersistence
    {
        int Add(LightSequence model);
        LightSequence Remove(int id);
        LightSequence Modify(int id, LightSequence model);
        IEnumerable<LightSequence> Get(params int[] id);
        Task<int> SaveChangeAsync();
    }
}
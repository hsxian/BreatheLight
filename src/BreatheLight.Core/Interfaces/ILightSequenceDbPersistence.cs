using System.Collections.Generic;
using System.Threading.Tasks;
using BreatheLight.Core.Models;

namespace BreatheLight.Core.Interfaces
{
    public interface ILightSequenceDbPersistence
    {
        Task<int> Add(LightSequence model);
        Task<LightSequence> Remove(int id);
        Task<LightSequence> Modify(int id, LightSequence model);
        IEnumerable<LightSequence> Get(params int[] id);
        Task<int> SaveChangeAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BreatheLight.Core.Data;
using BreatheLight.Core.Interfaces;
using BreatheLight.Core.Models;

namespace BreatheLight.Core.Implements
{
    public class LightSequenceDbPersistence : ILightSequenceDbPersistence
    {
        private readonly LightDbContext _lightdb;

        public LightSequenceDbPersistence(LightDbContext lightdb)
        {
            _lightdb = lightdb;
        }

        public int Add(LightSequence model)
        {
            if (model == null) return -1;
            model.Id = 0;
            if (_lightdb.LightSequences.Any())
            {
                model.Id = _lightdb.LightSequences.Max(t => t.Id) + 1;
            }
            _lightdb.LightSequences.Add(model);
            return model.Id;
        }

        public IEnumerable<LightSequence> Get(params int[] id)
        {
            if (id.Length < 1) return _lightdb.LightSequences.OrderBy(t => t.StartTime.TimeOfDay).ToList();
            return _lightdb.LightSequences.Where(t => id.Any(any => any.Equals(t.Id))).OrderBy(t => t.StartTime.TimeOfDay).ToList();
        }

        public LightSequence Modify(int id, LightSequence model)
        {
            var temp = _lightdb.LightSequences.SingleOrDefault(t => t.Id.Equals(id));
            if (temp == null) return null;
            temp.BrightnessA = model.BrightnessA;
            temp.BrightnessB = model.BrightnessB;
            temp.BrightnessStep = model.BrightnessStep;
            temp.StartTime = model.StartTime;
            temp.EndTime = model.EndTime;
            return model;
        }

        public LightSequence Remove(int id)
        {
            var item = _lightdb.LightSequences.SingleOrDefault(t => t.Id.Equals(id));
            if (item == null) return null;
            var entity = _lightdb.LightSequences.Remove(item);
            return entity?.Entity;
        }
        public Task<int> SaveChangeAsync() => _lightdb.SaveChangesAsync();
    }
}
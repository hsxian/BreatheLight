using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BreatheLight.Models;
using BreatheLight.Core.Interfaces;
using System.Timers;
using Microsoft.Extensions.Caching.Memory;
using BreatheLight.Core.Data;
using BreatheLight.Core.Models;

namespace BreatheLight.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILightRegulator _lightRegulator;
        private readonly IMemoryCache _cache;
        private readonly ILightSequenceDbPersistence _lightDB;
        private readonly ITimeMonitor _timeMonitor;

        public HomeController(
            ILightRegulator lightRegulator,
            IMemoryCache memoryCache,
            ILightSequenceDbPersistence lightDB,
            ITimeMonitor timeMonitor
            )
        {
            _lightRegulator = lightRegulator;
            _cache = memoryCache;
            _lightDB = lightDB;
            _timeMonitor = timeMonitor;
        }
        public IActionResult Index()
        {
            var ca = _cache.GetOrCreate("brightness", t =>
            {
                return t.Value = 9;
            });
            ViewData["brightness"] = ca;
            var lights = _lightDB.Get();
            return View(lights);
        }
        public void SetLightBrightness(float brightness)
        {
            _lightRegulator.SetLightBrightness(brightness);
            _cache.Set("brightness", brightness);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new LightSequence());
        }
        [HttpPost]
        public async Task<IActionResult> Create(LightSequence model)
        {
            await _lightDB.Add(model);
            await _lightDB.SaveChangeAsync();
            await _timeMonitor.RefreshLightTask();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _lightDB.Get(id).SingleOrDefault();
            if (item == null) return RedirectToAction(nameof(Index));

            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, LightSequence model)
        {
            await _lightDB.Modify(id, model);
            await _lightDB.SaveChangeAsync();
            await _timeMonitor.RefreshLightTask();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            await _lightDB.Remove(id);
            await _lightDB.SaveChangeAsync();
            await _timeMonitor.RefreshLightTask();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

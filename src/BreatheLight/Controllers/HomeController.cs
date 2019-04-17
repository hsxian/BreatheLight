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
using BreatheLight.Core.Implements;

namespace BreatheLight.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILightSequenceDbPersistence _lightsDb;
        private readonly ILightRegulator _lightRegulator;
        private readonly ITimeMonitor _timerMonitor;

        public HomeController(
            ILightSequenceDbPersistence lightsDb,
            ILightRegulator lightRegulator,
            ITimeMonitor timerMonitor
            )
        {
            _lightsDb = lightsDb;
            _lightRegulator = lightRegulator;
            _timerMonitor = timerMonitor;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["brightness"] = await _lightsDb.GetBrightness();

            var lights = await _lightsDb.Get();
            return View(lights);
        }
        public void SetLightBrightness(float brightness)
        {
            _lightsDb.SetBrightness(brightness);
            _lightRegulator.SetLightBrightness(brightness);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new LightSequence());
        }
        [HttpPost]
        public async Task<IActionResult> Create(LightSequence model)
        {
            await _lightsDb.Add(model);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = (await _lightsDb.Get(id))?.SingleOrDefault();
            if (item == null) return RedirectToAction(nameof(Index));

            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, LightSequence model)
        {
            await _lightsDb.Modify(id, model);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            await _lightsDb.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Distribute(int id)
        {
            await _timerMonitor.RefreshLightTask(await _lightsDb.Get());
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> UnDistribute(int id)
        {
            await _timerMonitor.RefreshLightTask(new List<LightSequence>());
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

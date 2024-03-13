using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarWatchProject.Contracts;
using SolarWatchProject.Models;
using SolarWatchProject.Services.ProcessData;
using SolarWatchProject.Services.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolarWatchProject.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolarWatchController : ControllerBase
    {
        private readonly ISunDataRepository _sunDataRepository;
        private readonly ISolarWatchRepository _solarRepository;
        private readonly IGeoRepository _geoRepository;
        private readonly IJsonProcessor _jsonProcessor;
        private readonly ILogger<SolarWatchController> _logger;

        public SolarWatchController(
            ISunDataRepository repository,
            ISolarWatchRepository solarRepository,
            IGeoRepository geoRepository,
            IJsonProcessor jsonProcessor,
            ILogger<SolarWatchController> logger)
        {
            _sunDataRepository = repository;
            _solarRepository = solarRepository;
            _geoRepository = geoRepository;
            _jsonProcessor = jsonProcessor;
            _logger = logger;
        }

        [HttpPost("NewRequest"), Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> NewRequest( SunDataRequest sunDataInput)
        {
            var tryParse = DateTime.TryParse(sunDataInput.Date, out var parsedDate);
            if (tryParse)
            {
                var city = _sunDataRepository.GetCityByName(sunDataInput.City);



                var sunRiseAndSetTimeInDb = _sunDataRepository.GetSunData(sunDataInput.City, parsedDate);

                if (sunRiseAndSetTimeInDb != null) return Ok(sunRiseAndSetTimeInDb);

                if (city == null)
                {
                    try
                    {
                        var latlng = await _geoRepository.GetLatLngByCity(sunDataInput.City);

                        if (latlng != null)
                        {
                            city = _jsonProcessor.GetProcessedGeoData(latlng);
                            _sunDataRepository.AddCity(city);
                            var sundata = _solarRepository.GetSunRiseAndSet(parsedDate, city.Latitude, city.Longitude);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error getting geocoding data");
                        return NotFound("City data not found");
                    }
                }

                try
                {
                    var sunData = await _solarRepository.GetSunRiseAndSet(parsedDate, city.Latitude, city.Longitude);

                    var resultSunRiseAndSetTime = _jsonProcessor.GetProcessedSunData(sunData, city, parsedDate);

                    _sunDataRepository.AddSunData(resultSunRiseAndSetTime);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error getting sun data");
                    return NotFound("Error getting sun data");
                }
            }
            return NotFound("Date input is incorrect");
        }

        [HttpPost("Edit"), Authorize(Roles = "Admin, User")]
        public IActionResult Edit(int id, SunRiseAndSetTime sunData)
        {
            if (id != sunData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _sunDataRepository.UpdateSunData(sunData);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SunDataExists(sunData.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return Ok(sunData);
        }

        [HttpPost("Delete"), Authorize(Roles = "Admin, User")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sunData = _sunDataRepository.GetSunData(id);
            if (sunData == null)
            {
                return NotFound();
            }
            
            _sunDataRepository.DeleteSunData(sunData);

            return RedirectToAction(nameof(Index));
        }

        private bool SunDataExists(int? id)
        {
            return _sunDataRepository.GetSunData(id) != null;
        }

    }
}

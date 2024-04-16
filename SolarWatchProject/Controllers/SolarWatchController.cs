using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarWatchProject.Contracts;
using SolarWatchProject.Models;
using SolarWatchProject.Repositories;
using SolarWatchProject.Services.ProcessData;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolarWatchProject.Controllers
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

        [HttpGet("GetAll")]
        public IEnumerable<SunRiseAndSetTime> GetAll()
        {
            try
            {
                var data = _sunDataRepository.GetAllSunData();
                return data;
            }
            catch
            {
                throw new Exception();
            }
        }

        [HttpPost("NewRequest")]//, Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> NewRequest(SunDataRequest sunDataInput)
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
                        System.Diagnostics.Debug.WriteLine("latlng: " + latlng);
                        if (latlng != null)
                        {
                            city = _jsonProcessor.GetProcessedGeoData(latlng);
                            _sunDataRepository.AddCity(city);
                            var sundata = await _solarRepository.GetSunRiseAndSet(parsedDate, city.Latitude, city.Longitude);
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

                    return Ok();
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
                return Ok();
            }
            return Ok(sunData);
        }

        [HttpPost("Delete"), Authorize(Roles = "Admin, User")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var sunData = _sunDataRepository.GetSunData(id);
            if (sunData == null)
            {
                return NotFound();
            }

            _sunDataRepository.DeleteSunData(sunData);

            return Ok();
        }

        private bool SunDataExists(int? id)
        {
            return _sunDataRepository.GetSunData(id) != null;
        }

    }
}

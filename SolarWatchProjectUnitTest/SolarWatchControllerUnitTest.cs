using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatchProjectBackend.Contracts;
using SolarWatchProjectBackend.Controllers;
using SolarWatchProjectBackend.Models;
using SolarWatchProjectBackend.Repositories;
using SolarWatchProjectBackend.Services.ProcessData;


namespace SolarWatchProjectUnitTest
{
    public class SolarWatchControllerUnitTest
    {
        private Mock<ISunDataRepository> _sunDataRepositoryMock;
        private Mock<ISolarWatchRepository> _solarWatchRepositoryMock;
        private Mock<IGeoRepository> _geoRepositoryMock;
        private Mock<IJsonProcessor> _jsonProcessorMock;
        private Mock<ILogger<SolarWatchController>> _loggerMock;
        private SolarWatchController _controller;
        
        [SetUp]
        public void GetSetup()
        {
            _sunDataRepositoryMock = new Mock<ISunDataRepository>();
            _solarWatchRepositoryMock =  new Mock<ISolarWatchRepository>();
            _geoRepositoryMock = new Mock<IGeoRepository>();
            _jsonProcessorMock = new Mock<IJsonProcessor>();
            _loggerMock = new Mock<ILogger<SolarWatchController>>();
            _controller = new SolarWatchController(
                _sunDataRepositoryMock.Object,
                _solarWatchRepositoryMock.Object,
                _geoRepositoryMock.Object,
                _jsonProcessorMock.Object,
                _loggerMock.Object);

            _sunDataRepositoryMock.Setup(r => r.GetCityByName(It.IsAny<string>())).Returns((City)null);
            _sunDataRepositoryMock.Setup(r => r.GetSunData(It.IsAny<string>(), It.IsAny<DateTime>())).Returns((SunRiseAndSetTime)null);
            _sunDataRepositoryMock.Setup(r => r.GetSunData(It.IsAny<int>())).Returns(new SunRiseAndSetTime() { Id = 1, Date = new DateTime(1000 - 10 - 10), City = "Budapest", SunRise = "sun rise", SunSet = "sun set" });
            _sunDataRepositoryMock.Setup(r => r.AddCity(It.IsAny<City>()));
            _sunDataRepositoryMock.Setup(r => r.AddSunData(It.IsAny<SunRiseAndSetTime>()));
            _sunDataRepositoryMock.Setup(r => r.UpdateSunData(It.IsAny<SunRiseAndSetTime>()));
            _sunDataRepositoryMock.Setup(r => r.DeleteSunData(It.IsAny<SunRiseAndSetTime>())).Returns(new bool());

            _geoRepositoryMock.Setup(r => r.GetLatLngByCity(It.IsAny<string>())).ReturnsAsync("latlng");

            _solarWatchRepositoryMock.Setup(r => r.GetSunRiseAndSet(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<double>()))
                      .ReturnsAsync("");

            _jsonProcessorMock.Setup(p => p.GetProcessedGeoData(It.IsAny<string>())).Returns(new City());
            _jsonProcessorMock.Setup(p => p.GetProcessedSunData(It.IsAny<string>(), It.IsAny<City>(), It.IsAny<DateTime>()))
                .Returns(new SunRiseAndSetTime() { Id = 1, Date = new DateTime(1000 - 10 - 10), City = "Budapest", SunRise = "sun rise", SunSet = "sun set" });

        }

        [Test]
        public void GetAllTest()
        {
            var result = _controller.GetAll();
            var expected = new List<SunRiseAndSetTime>();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task NewRequestTest()
        {
            var req = new SunDataRequest("1000-10-10", "Budapest");

            var result = await _controller.NewRequest(req);

            Assert.That(result.GetType(), Is.EqualTo(new OkResult().GetType()));
        }

        [Test]
        public async Task NewRequestBadDateTest()
        {
            var req = new SunDataRequest("", "Budapest");

            var result = await _controller.NewRequest(req);
            var message = "Date input is incorrect";

            Assert.That(result.GetType(), Is.EqualTo(new NotFoundObjectResult(message).GetType()));
        }

        [Test]
        public void EditTest()
        {
            var sunData = new SunRiseAndSetTime() { Id = 1, Date = new DateTime(1000 - 10 - 10), City = "Budapest", SunRise = "sun rise", SunSet = "sun set" };
            
            var result = _controller.Edit(1, sunData);

            Assert.That(result.GetType(), Is.EqualTo(new OkResult().GetType()));
        }

        [Test]
        public void EditNotFoundTest()
        {
            var sunData = new SunRiseAndSetTime() { Id = 1, Date = new DateTime(1000 - 10 - 10), City = "Budapest", SunRise = "sun rise", SunSet = "sun set" };

            var result = _controller.Edit(2, sunData);

            Assert.That(result.GetType(), Is.EqualTo(new NotFoundResult().GetType()));
        }

        [Test]
        public void DeleteConfirmedTest()
        {
            var result = _controller.DeleteConfirmed(1);

            Assert.That(result.GetType(), Is.EqualTo(new OkResult().GetType()));
        }

    }
}

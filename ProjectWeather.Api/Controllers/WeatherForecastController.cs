using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectWeather.Api.Models;
using System;
using System.Linq;

namespace ProjectWeather.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Startup.WeatherForecasts);
        }

        [HttpPost]
        public IActionResult Post(WeatherForecast weatherForecast)
        {
            weatherForecast.Id = Guid.NewGuid();

            Startup.WeatherForecasts.Add(weatherForecast);

            return Created(nameof(Post), weatherForecast);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var weatherForecast = Startup.WeatherForecasts.FirstOrDefault(t => t.Id == id);
            return Ok(weatherForecast);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_WebAPI_Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController()
        {
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            WeatherForecast[] weatherForecasts = new WeatherForecast[] {
                WeatherForecast.GetRandom(),
                WeatherForecast.GetRandom(),
                WeatherForecast.GetRandom(),
                WeatherForecast.GetRandom(),
                WeatherForecast.GetRandom(),
            };

            return weatherForecasts;
        }
    }
}

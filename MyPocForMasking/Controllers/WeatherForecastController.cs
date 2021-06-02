using System;
using System.Collections.Generic;
using System.Linq;
using JsonMasking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyPocForMasking.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISanitizeDataService _sanitizeDataService;
        private readonly SensitiveAuditingFieldsContext sensitiveAuditingFields;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISanitizeDataService sanitizeDataService, SensitiveAuditingFieldsContext sensitiveAuditingFields)
        {
            _logger = logger;
            _sanitizeDataService = sanitizeDataService;
            this.sensitiveAuditingFields = sensitiveAuditingFields;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var obj = new { Password = "password" };
            var feature = obj.MaskFields(sensitiveAuditingFields.Values);
            // var feature = _sanitizeDataService.Sanitize(obj);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

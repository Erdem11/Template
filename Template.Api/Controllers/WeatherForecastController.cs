﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Template.Common.Structs;
using Template.Service;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IIdentityService _identityService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBookService bookService, IIdentityService identityService)
        {
            _logger = logger;
            _bookService = bookService;
            _identityService = identityService;
        }

        [Authorize]
        [HttpGet]
        public object Get(int day = 10)
        {
            var pageHolder = PageHolder.Create(0, 200);

            for (int i = 0; i < 1000 * 1000; i++)
            {
                var a = PageHolder.Create(0, 200);
            }

            return new
            {
                Books = _bookService.GetNewBooks(day, pageHolder),
                TotalCount = pageHolder.TotalCount
            };
        }

        [HttpGet]
        public object Register(string email, string password)
        {
            return _identityService.Register(email,password);
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public object TokenTest()
        {
            return default;
        }
    }
}
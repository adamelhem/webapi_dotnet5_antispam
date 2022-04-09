using IsPrimeNumber.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace IsPrimeNumber.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RateLimitFillter]
    public class PrimeNumbersController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PrimeNumbersController> _logger;

        public PrimeNumbersController(ILogger<PrimeNumbersController> logger)
        {
            _logger = logger;
        }

  
        [HttpPost("IsPrimeNumber")]
        //ToDo: add authontication fillter and exctract the user ip from the request header
        public IActionResult IsPrimeNumber(int number)
        {
            var rateLimit = HttpContext.Items["RateLimit"];
            if(rateLimit==null)
            {
                var isPrimeNum = isPrimeNumberCheck(number);
                return Ok(new SuccessResponse
                {
                    Number = number,
                    IsPrime = isPrimeNum
                });
            }
            else
            {
                var blokedDate = (DateTime)rateLimit;
                var varTime = DateTime.Now - blokedDate;
                int intMinutes = 60-(int)varTime.TotalMinutes;
                return Ok( new ErrorResponse
                {
                    ErrorMessage = $"You have eceeded your call limit. Remaining time until restriction removed is {intMinutes} Minutes"
                });
            }
        }

        bool isPrimeNumberCheck(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;
            return true;
        }

    }
}
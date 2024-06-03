using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CovidChart.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CovidChart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SaveCovid(covid);

            return Ok(_covidService.GetCovidChartList());
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random random = new Random();

            Enumerable.Range(1, 10).ToList().ForEach(number =>
            {
                foreach (City item in Enum.GetValues(typeof(City)))
                {
                    var newCovid = new Covid
                    {
                        City = item, 
                        Count = random.Next(100, 1000), 
                        CovidDate = DateTime.Now.AddDays(number)
                    };
                    _covidService.SaveCovid(newCovid).Wait();
                    
                    Thread.Sleep(1000);
                }
            });

            return Ok("Covid19 dataları veritabanına kaydedildi");
        }
    }
}
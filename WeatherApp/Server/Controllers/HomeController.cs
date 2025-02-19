using Amazon.Runtime;
using WeatherApp.BusinessLogic.IBusinessLogic;
using WeatherApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Runtime;
using System.Text.Json;

namespace WeatherApp.Server.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHome _iHome;
        public HomeController(IHome iHome)
        {
            _iHome = iHome;
        }

        [HttpPost]
        [Route("api/GetWeatherInfo")]
        public async Task<IActionResult> GetWeatherInfo([FromBody] CityInfo cityInfo)
        {
            WeatherModel weatherModel = new WeatherModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Message = "Invalid data provided." });
                }
                if (cityInfo is not null && cityInfo.CityName is not null)
                {
                    weatherModel = await _iHome.GetWeatherInfo(cityInfo.CityName);
                    if (weatherModel.Status == "failed")
                    {
                        return BadRequest(new { Message = weatherModel.Message });
                    }
                }
                return Ok(weatherModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred: " + ex.Message });
            }
        }


        [HttpPost("api/GetLogsInfo")]
        public async Task<IActionResult> GetLogsInfo()
        {
            try
            {
                List<LogModel> logModel = await _iHome.GetLogsInfo();
                return Ok(logModel);
            }
            catch (Exception ex)
            {
                var errorResponse = new { Message = "An error occurred", Error = ex.Message };
                return StatusCode(500, errorResponse); // Ensure JSON format
            }
        }

       
    }
}

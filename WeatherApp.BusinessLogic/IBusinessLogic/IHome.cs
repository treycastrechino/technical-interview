using WeatherApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.BusinessLogic.IBusinessLogic
{
    public interface IHome
    {
        Task<WeatherModel> GetWeatherInfo(string CityName);
        Task<List<LogModel>> GetLogsInfo();


    }
}

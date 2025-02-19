using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared
{
    public class WeatherData
    {
        public string? DateTime { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public string? Condition { get; set; }
    }

}

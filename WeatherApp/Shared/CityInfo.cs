using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared
{   
    public class CityInfo
    {
        [Required(ErrorMessage = "City Name is required.")]
        [StringLength(30)]
        public string? CityName { get; set; }
          
    }
    public class IpLocation
    {
        public string Query { get; set; } = "";
        public string City { get; set; } = "";
        public string RegionName { get; set; } = "";
        public string Country { get; set; } = "";
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

}

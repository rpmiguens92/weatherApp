using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherApp.ViewModels
{
    public class AirPollutionViewModel
    {
        public string AirQuality { get; }

        public AirPollutionViewModel(string airQuality)
        {
            AirQuality = airQuality;

        }
    }
}

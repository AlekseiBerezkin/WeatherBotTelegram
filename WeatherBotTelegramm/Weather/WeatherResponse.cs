using System;
using System.Collections.Generic;
using System.Text;
using WeatherBotTelegramm.Weather;

namespace WeatherBotTelegramm
{
    class WeatherResponse
    {
        public TemperatureInfo Main { get; set; }
        public string Name { get; set; }
        public WindInfo Wind { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WeatherBotTelegramm.Weather;

namespace WeatherBotTelegramm.Forecast
{
    class ForecastResponse
    {
        public TemperatureInfo Main { get; set; }
        public WindInfo Wind { get; set; }
        public string dt_txt { get; set; }
    }
}

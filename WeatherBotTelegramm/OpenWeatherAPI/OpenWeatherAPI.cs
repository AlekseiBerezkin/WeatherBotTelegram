using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using WeatherBotTelegramm.Forecast;

namespace WeatherBotTelegramm
{
    class OpenWeatherAPI
    {
        private string response;
        private string request;
        private string key;
        private HttpWebResponse httpWebResponse;
        private HttpWebRequest httpWebRequest;

        public OpenWeatherAPI(string key)
        {
            this.key = key;
        }

        public ForecastList forecastList(string city)
        {
            request = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={key}";
            httpWebRequest = (HttpWebRequest)WebRequest.Create(request);

            ForecastList forecastList;

            try
            {
                
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                 forecastList = JsonConvert.DeserializeObject<ForecastList>(response);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:"+ex);
                forecastList = null;
            }

            

            return forecastList;
        }

        public WeatherResponse Weather(string city)
        {
            request = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";
            httpWebRequest = (HttpWebRequest)WebRequest.Create(request);
            
            WeatherResponse weatherResponse;

            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:"+ex);
                weatherResponse = null;
            }

            return weatherResponse;
        }
    }
}

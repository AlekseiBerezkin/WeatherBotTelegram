using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using WeatherBotTelegramm.Forecast;

namespace WeatherBotTelegramm
{
    class Program
    {
        private static bool flag_exit = true;
        static void Main(string[] args)
        {
            
            
            Console.WriteLine(Environment.CurrentDirectory);
            
            string jsonFile = "Api_key.json";
            string jsonFromFile;
            
            ApiKey key=new ApiKey();
            try
            {
                using(var reader=new StreamReader(jsonFile))
                {
                    jsonFromFile = reader.ReadToEnd();
                }

                 key = JsonConvert.DeserializeObject<ApiKey>(jsonFromFile);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка чтения файла json");
                flag_exit = false;
            }

            while (flag_exit)
            {
                try
                {
                    GetMessage(key.TOKEN, key.API_KEY).Wait();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Err:"+ex);
                }
            }
            Console.WriteLine("Выход");
            
        }

        static async Task GetMessage(string TOKEN,  string key)
        {
            TelegramBotClient bot = new TelegramBotClient(TOKEN);
            int offset = 0;
            int timeout = 0;

            try
            {
                await bot.SetWebhookAsync("");

                while (flag_exit)
                {
                    var updates = await bot.GetUpdatesAsync(offset, timeout); 
                    foreach (var update in updates)
                    {
                        var message = update.Message;
                        string[] command = message.Text.Split(" ");

                        string city = " ";
                        try
                        {
                            city = command[1];
                        }
                        catch (Exception ex)
                        {
                           switch(command[0])
                            {
                                case "/start":
                                    await bot.SendTextMessageAsync(message.Chat.Id, "Weather_bot предоставляет информацию о погоде. Команды:" +
                                        "Погода ГОРОД");
                                    break;
                            }
                        }

                        switch (command[0])
                        {
                            case "Погода":

                                OpenWeatherAPI openWeatherAPI = new OpenWeatherAPI(key);

                                WeatherResponse weatherResponse = openWeatherAPI.Weather(command[1]);
                                if(weatherResponse!=null)
                                {
                                    await bot.SendTextMessageAsync(message.Chat.Id, $"Температура:{weatherResponse.Main.Temp}\n" +
                                        $"Давление:{weatherResponse.Main.Pressure}mm\n" +
                                        $"Ветер:{weatherResponse.Wind.Speed}м/c");
                                }
                                else
                                {
                                    await bot.SendTextMessageAsync(message.Chat.Id, $"Ошибка запроса");
                                }
                                    
                                
                                break;


                            case "Прогноз":

                                 openWeatherAPI = new OpenWeatherAPI(key);

                                ForecastList forecastList = openWeatherAPI.forecastList(command[1]);
                                if (forecastList != null)
                                {
                                    foreach(var s in forecastList.List)
                                    {
                                        await bot.SendTextMessageAsync(message.Chat.Id, $"{s.dt_txt} Температура {s.Main.Temp}" +
                                            $",давление {s.Main.Pressure},ветер {s.Wind.Speed}");
                                    }

                                }
                                else
                                {
                                    await bot.SendTextMessageAsync(message.Chat.Id, $"Ошибка запроса");
                                }

                                break;

                            default:
                                await bot.SendTextMessageAsync(message.Chat.Id, $"Команда {command[0]} отсутствует.");
                                
                                break;
                        }

                        offset = update.Id + 1;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Err:" + ex);
            }
        }
    }

    



}

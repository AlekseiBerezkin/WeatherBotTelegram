using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using WeatherBotTelegramm.Forecast;
using WeatherBotTelegramm.Sticker;

namespace WeatherBotTelegramm
{
    class Program
    {
        private static bool flag_exit = true;
        static void Main(string[] args)
        {


            Console.WriteLine("Старт");

            string jsonFile = "Api_key.json";
            string jsonFromFile;

            ApiKey key = new ApiKey();
            try
            {
                using (var reader = new StreamReader(jsonFile))
                {
                    jsonFromFile = reader.ReadToEnd();
                }

                key = JsonConvert.DeserializeObject<ApiKey>(jsonFromFile);
                Console.WriteLine("Файл Api_key прочитан");
            }
            catch (Exception ex)
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
                catch (Exception ex)
                {
                    Console.WriteLine("Err:" + ex);
                }
            }
            Console.WriteLine("Выход");

        }

        static async Task GetMessage(string TOKEN, string key)
        {
            TelegramBotClient bot = new TelegramBotClient(TOKEN);
            int offset = 0;
            int timeout = 0;
            Console.WriteLine("Старт получения сообщений");
            try
            {
                await bot.SetWebhookAsync("");

                while (flag_exit)
                {
                    var updates = await bot.GetUpdatesAsync(offset, timeout);
                    foreach (var update in updates)
                    {
                        string[] command = { };
                        var message = update.Message;
                        try
                        {
                            command = filterMessage(message.Text);
                            Console.WriteLine($"Получено сообщение от "+message.From.FirstName+" "+message.From.LastName+":"+ message.Text);
                            if (command != null)
                            {
                                if (command.Length == 1)
                                {
                                    switch (command[0])
                                    {
                                        case "/start":
                                            await bot.SendTextMessageAsync(message.Chat.Id, "Вас приветствует MyBotWeather. Моя цель оповещать тебя о погоде по быстрому набору команд" +
                                                "для получения доступных команд набери /help");
                                            break;
                                        case "/help":
                                            await bot.SendTextMessageAsync(message.Chat.Id, "Weather_bot предоставляет информацию о погоде. Команды:\n" +
                                                "-Погода ГОРОД\n" +
                                                "-Прогноз ГОРОД\n");
                                            break;


                                        default:
                                            await bot.SendTextMessageAsync(message.Chat.Id, $"Команда {command[0]} отсутствует.");

                                            break;
                                    }
                                }
                                else
                                {

                                    try
                                    {
                                        string city = command[1];

                                        switch (command[0])
                                        {
                                            case "Погода":

                                                OpenWeatherAPI openWeatherAPI = new OpenWeatherAPI(key);

                                                WeatherResponse weatherResponse = openWeatherAPI.Weather(command[1]);
                                                if (weatherResponse != null)
                                                {
                                                    
                                                    
                                                    await bot.SendTextMessageAsync(message.Chat.Id, $"Температура:{weatherResponse.Main.Temp}\n" +
                                                        $"Давление:{weatherResponse.Main.Pressure}mm\n" +
                                                        $"Ветер:{weatherResponse.Wind.Speed}м/c\n" +
                                                        $"Влажность:{weatherResponse.Main.humidity}%");
                                                    await bot.SendStickerAsync(message.Chat.Id, StickerWeather.GetSticker(weatherResponse.Weather[0].icon));
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
                                                    foreach (var s in forecastList.List)
                                                    {
                                                        await bot.SendTextMessageAsync(message.Chat.Id, $"{s.dt_txt}\n" +
                                                            $"Температура:{s.Main.Temp},\n" +
                                                            $"Давление:{s.Main.Pressure}кПа,\n" +
                                                            $"Ветер:{s.Wind.Speed}м/c\n" +
                                                            $"Влажность:{s.Main.humidity}%");
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

                                    }
                                    catch (Exception ex)
                                    {

                                        await bot.SendTextMessageAsync(message.Chat.Id, $"Ошибка обработки команды");
                                    }

                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Ошибка фильтра");
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

        static string[] filterMessage(string message)
        {
            string[] strMsg;
            try
            {
                strMsg = message.Split(" ");

                if (strMsg.Length > 2)
                {
                    strMsg = null;
                    return strMsg;
                }

            }
            catch
            {
                strMsg = null;
            }
            return strMsg;

        }
    }





}

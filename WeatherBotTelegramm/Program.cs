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
            string city ="Rostov";
            string jsonFile = "Api_key.json";
            string jsonFromFile;
            

            string request="";
            
            //ApiKey key=new ApiKey();
            ApiKey key=new ApiKey();
            try
            {
                using(var reader=new StreamReader(jsonFile))
                {
                    jsonFromFile = reader.ReadToEnd();
                    //Console.WriteLine(jsonFromFile);
                }

                 key = JsonConvert.DeserializeObject<ApiKey>(jsonFromFile);

                //Console.WriteLine(key.API_KEY);
                //request = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={key.API_KEY}&units=metric";
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка чтения файла json");
                flag_exit = false;
            }

            //Console.WriteLine(key.TOKEN); ;
            
            while (flag_exit)
            {
                try
                {
                    GetMessage(key.TOKEN, key.API_KEY).Wait();
                    Console.WriteLine("тут");                  //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(request);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Err:"+ex);
                    //отправить инфу в телегу
                }
            }
            Console.WriteLine("Вышли");
            
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
                    //Console.WriteLine(offset);
                    foreach (var update in updates)
                    {
                        var message = update.Message;

                        string[] command = message.Text.Split(" ");
                        


                        switch (command[0])
                        {
                            case "Погода":
                                
                                string city = command[1];
                                string request = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=metric";

                                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(request);

                                try
                                {
                                    string response;
                                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                                    {
                                        response = streamReader.ReadToEnd();
                                    }

                                    WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
                                    Console.WriteLine($"Температура в {weatherResponse.Name} {weatherResponse.Main.Temp}С," +
                                        $"Давление {weatherResponse.Main.Pressure}мм,Ветер {weatherResponse.Wind.Speed}м/c");
                                    
                                    await bot.SendTextMessageAsync(message.Chat.Id, $"Температура в {weatherResponse.Name} {weatherResponse.Main.Temp}С," +
                                        $"Давление {weatherResponse.Main.Pressure}мм,Ветер {weatherResponse.Wind.Speed}м/с");
                                    
                                }
                                catch
                                {
                                    Console.WriteLine("Error");
                                    await bot.SendTextMessageAsync(message.Chat.Id, $"Ошибка запроса. Такого населенного пункта не существует");
                                }

                                break;

                            case "/start":
                                await bot.SendTextMessageAsync(message.Chat.Id, "Weather_bot предоставляет информацию о погоде. Команды:" +
                                    "Погода ГОРОД");

                                break;

                            case "Прогноз":

                                 city = command[1];
                                 request = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={key}";

                                 httpWebRequest = (HttpWebRequest)WebRequest.Create(request);

                                try
                                {
                                    string response;
                                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                                    {
                                        response = streamReader.ReadToEnd();
                                    }
                                    ///////////////
                                   // WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);

                                    ForecastList forecastList= JsonConvert.DeserializeObject<ForecastList>(response);

                                    foreach(var s in forecastList.List)
                                    {
                                        await bot.SendTextMessageAsync(message.Chat.Id,$"{s.dt_txt} Температура {s.Main.Temp}" +
                                            $",давление {s.Main.Pressure},ветер {s.Wind.Speed}");
                                    }

                                    /*Console.WriteLine($"Температура в {weatherResponse.Name} {weatherResponse.Main.Temp}С," +
                                        $"Давление {weatherResponse.Main.Pressure}мм,Ветер {weatherResponse.Wind.Speed}м/c");

                                    await bot.SendTextMessageAsync(message.Chat.Id, $"Температура в {weatherResponse.Name} {weatherResponse.Main.Temp}С," +
                                        $"Давление {weatherResponse.Main.Pressure}мм,Ветер {weatherResponse.Wind.Speed}м/с");*/

                                }
                                catch
                                {
                                    Console.WriteLine("Error");
                                    await bot.SendTextMessageAsync(message.Chat.Id, $"Ошибка запроса. Такого населенного пункта не существует");
                                }

                                break;

                            default:
                                await bot.SendTextMessageAsync(message.Chat.Id, $"Команда {command[0]} отсутствует.");
                                
                                break;
                        }
                        /*
                                                if (message.Text == "MyFirstBot")
                                                {
                                                    Console.WriteLine("Получено сообщение:" + message.Text);

                                                    await bot.SendTextMessageAsync(message.Chat.Id, "Ку,епта " + message.Chat.FirstName);
                                                }
                                                if (message.Text == "/start")
                                                {
                                                    await bot.SendTextMessageAsync(message.Chat.Id, "Поехали" + message.Chat.FirstName);
                                                }
                                                if (message.Text == "/exit")
                                                {
                                                    await bot.SendTextMessageAsync(message.Chat.Id, "Выход");
                                                    flag_exit = false;


                                                }
                                                if (message.Text == "Погода")
                                                {
                                                    await bot.SendTextMessageAsync(message.Chat.Id, "Введите город");

                                                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(request);


                                                    try
                                                    {
                                                        string response;
                                                        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                                                        using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                                                        {

                                                            response = streamReader.ReadToEnd();

                                                        }

                                                        WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
                                                        Console.WriteLine($"Temperature in {weatherResponse.Name} {weatherResponse.Main.Temp}С," +
                                                            $"Pressure {weatherResponse.Main.Pressure}mm,Wind {weatherResponse.Wind.Speed}m/s");
                                                        await bot.SendTextMessageAsync(message.Chat.Id, $"Temperature in {weatherResponse.Name} {weatherResponse.Main.Temp}С," +
                                                            $"Pressure {weatherResponse.Main.Pressure}mm,Wind {weatherResponse.Wind.Speed}m/s");


                                                    }
                                                    catch
                                                    {
                                                        Console.WriteLine("Error");
                                                    }


                                                }

                                                //break;*/
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

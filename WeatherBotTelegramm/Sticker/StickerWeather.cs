using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherBotTelegramm.Sticker
{
    static class StickerWeather
    {
        public static string GetSticker(string icon)
        {
            switch(icon)
            {
                case "01d":
                    return "CAACAgIAAxkBAAECUQpgpLVyGcoFgrq4340mLE_sTLcuVwACAgQAAtJaiAECKCdNruu1MR8E";
                    
                case "02d":
                    return "CAACAgIAAxkBAAECURBgpLXEQJ3px7d57hMZs7p7p4Z3NQACJAQAAtJaiAGGA7TDrrPgrx8E"; 

                case "03d":
                    return "CAACAgIAAxkBAAECURpgpLaGjwix7LsWArJAlxJqgskprAACIgQAAtJaiAFjc2CbQBVg3B8E";
                    
                case "04d":
                    return "CAACAgIAAxkBAAECUR5gpLbWriWozqsAAQbpVOYCAAExng7aAAJxAQACpkRIC7ZGOBtloYcRHwQ";
                  
                case "09d":
                    return "CAACAgIAAxkBAAECUSBgpLck1DczjnZ5NQ8f0r7tatjdHgACSwQAAtJaiAFXp6lBpcqOEx8E";
                 
                case "10d":
                    return "AACAgIAAxkBAAECUSRgpLdN1LtJ1o7nPcRQ_dQaz4h0IQACXQEAAqZESAvs_oFvEWLK4R8E";
                   
                case "11d":
                    return "CAACAgIAAxkBAAECUSZgpLduhtl2vtp6q2-xbzIH_PmDhgACLAEAAqZESAs1JufkLCR1wx8E";
                   
                case "13d":
                    return "CAACAgIAAxkBAAECUSpgpLevs32iVj3HkDVHZGCkzSos8gACRwQAAtJaiAE_LcKbC-PZCh8E";
                
                case "50d":
                    return "CAACAgIAAxkBAAECURxgpLal207d_AwppSgvle4qZBbpuQACaQEAAqZESAt_3dxMvA3vCR8E";
                case "01n":
                    return "CAACAgIAAxkBAAECUS5gpLhSldoGWBRm7uU0OzrxGjlpdwAC7gMAAtJaiAEi8XR1XvuXSR8E";

                case "02n":
                    return "CAACAgIAAxkBAAECURBgpLXEQJ3px7d57hMZs7p7p4Z3NQACJAQAAtJaiAGGA7TDrrPgrx8E";

                case "03n":
                    return "CAACAgIAAxkBAAECURpgpLaGjwix7LsWArJAlxJqgskprAACIgQAAtJaiAFjc2CbQBVg3B8E";

                case "04n":
                    return "CAACAgIAAxkBAAECUR5gpLbWriWozqsAAQbpVOYCAAExng7aAAJxAQACpkRIC7ZGOBtloYcRHwQ";

                case "09n":
                    return "CAACAgIAAxkBAAECUSBgpLck1DczjnZ5NQ8f0r7tatjdHgACSwQAAtJaiAFXp6lBpcqOEx8E";

                case "10n":
                    return "AACAgIAAxkBAAECUSRgpLdN1LtJ1o7nPcRQ_dQaz4h0IQACXQEAAqZESAvs_oFvEWLK4R8E";

                case "11n":
                    return "CAACAgIAAxkBAAECUSZgpLduhtl2vtp6q2-xbzIH_PmDhgACLAEAAqZESAs1JufkLCR1wx8E";

                case "13n":
                    return "CAACAgIAAxkBAAECUSpgpLevs32iVj3HkDVHZGCkzSos8gACRwQAAtJaiAE_LcKbC-PZCh8E";

                case "50n":
                    return "CAACAgIAAxkBAAECURxgpLal207d_AwppSgvle4qZBbpuQACaQEAAqZESAt_3dxMvA3vCR8E";
                default:
                    return null;
            }

            
        }
    }
}

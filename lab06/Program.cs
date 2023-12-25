using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;


namespace lab06
{
    class Executable
    {
        static void Main(string[] args)
        {
            string lat, lon, parse;
            Random random = new Random();
            List<DeserializatedWeather> list = new List<DeserializatedWeather>();

            DeserializatedWeather maxByTemp;
            DeserializatedWeather minByTemp;
            float avgByTemp;
            int count;
            DeserializatedWeather country;

            for (int i = 0; i < random.Next(50, 100); i++)
            {
                lat = Convert.ToString(Math.Round(random.NextDouble(), 1) * 180.0 - 90.0);
                lon = Convert.ToString(Math.Round(random.NextDouble(), 1) * 180.0 - 90.0);

                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid=ce5c837a5868b6cd3e3d30107a7e6b78";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                parse = reader.ReadToEnd();
                reader.Close();

                DeserializatedWeather weather = JsonConvert.DeserializeObject<DeserializatedWeather>(parse);

                if (weather.Name == "" || weather.Name == "Globe")
                {
                    Console.WriteLine($"{i}: weather.Name пусто. i = {i - 1}");
                    --i;
                }
                else
                {
                    Console.WriteLine($"{i}: weather.Name = {weather.Name}");
                    list.Add(weather);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{list[i].Name} {list[i].Main.Temp} {list[i].Sys.Country} {list[i].Weather[0].Description}");
            }

            maxByTemp = list.OrderByDescending(i => i.Main.Temp).FirstOrDefault();
            minByTemp = list.OrderByDescending(i => i.Main.Temp).LastOrDefault();
            avgByTemp = list.Average(i => i.Main.Temp);
            count = list.Count();

            bool DescrMatch(DeserializatedWeather element) => (element.Weather[0].Description == "clear sky" ||
                                                               element.Weather[0].Description == "rain" ||
                                                               element.Weather[0].Description == "few clouds");
            country = list.Find(DescrMatch);

            Console.WriteLine($"Max temp {maxByTemp.Name} {maxByTemp.Main.Temp}\n" +
                              $"Min temp {minByTemp.Name} {minByTemp.Main.Temp}\n" +
                              $"Avg temp {avgByTemp}\n" +
                              $"Count {count}\n" +
                              $"Match string {country.Name} {country.Sys.Country}");
            Console.ReadLine();
        }
    }
}

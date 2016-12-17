using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FormFlow
{
    public class WeatherClient
    {
        private string AppID { get; set; }

        private readonly HttpClient cli = new HttpClient();
        public WeatherClient(string AppID)
        {
            this.AppID = AppID;
        }

        public async Task<WeatherRecord[]> Forecast(string city)
        {
            var res = await cli.GetStringAsync($"http://api.openweathermap.org/data/2.5/forecast/daily?q={city}&mode=json&units=metric&cnt=7&APPID={AppID}");
            var f = new List<WeatherRecord>();
            dynamic x = Newtonsoft.Json.JsonConvert.DeserializeObject(res);
            foreach (var z in x.list)
            {
                f.Add(new WeatherRecord()
                {
                    When = Convert((long)z.dt),
                    Temp = z.temp.day,
                    Pressure = z.pressure,
                    Humidity = z.humidity,
                });
            }
            return f.ToArray();
        }
        private DateTime Convert(long x)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(x).ToLocalTime();
            return dtDateTime;
        }

    }
}
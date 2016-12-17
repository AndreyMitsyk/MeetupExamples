using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow;

namespace FormFlow
{
    [Serializable]
    public class WeatherParam
    {
        public static IForm<WeatherParam> BuildForm()
        {
            return new FormBuilder<WeatherParam>()
                .Message("Welcome to weather bot")
                .Build();
        }

        [Prompt("Which city do you want to know the weather in?")]
        public string Location { get; set; }

        [Prompt("Which day you are interested in?")]
        public DateTime When { get; set; }

        private int Offset => (int)Math.Ceiling((When - DateTime.Now).TotalDays);

        public async Task<string> BuildResult()
        {
            WeatherClient OWM = new WeatherClient("{openWeatherMapAppId}");
            var weatherRecords = await OWM.Forecast(Location);
            var weather = weatherRecords[Offset];

            return $"The temperature on {weather.Date} in {Location} is {weather.Temp}.\r\n";
        }
    }
}
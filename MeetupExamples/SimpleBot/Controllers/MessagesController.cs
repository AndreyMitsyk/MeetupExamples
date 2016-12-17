using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using System.Text;
using System.Collections.Generic;

namespace SimpleBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                var rep = await Reply(activity.Text);
                Activity reply = activity.CreateReply(rep);

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        async Task<string> Reply(string msg)
        {
            IDictionary<string, bool> keyWords = new Dictionary<string, bool>
            {
                {"temperature", false},
                {"humidity", false},
                {"pressure", false}
            };
            string city = "Ivanovo";
            int when = 0; var words = msg.ToLower().Split(' ').ToList();

            if (words.Any(t => t == "help"))
            {
                return @"This is a simple weather bot.
 Example of commands include:
   - temperature today,
   - temperature in Ivanovo,
   - pressure and humidity,
   - humidity tomorrow";
            }

            foreach (string word in words)
            {
                if (keyWords.ContainsKey(word))
                {
                    keyWords[word] = true;
                }
            }

            if (words.Any(t => t == "today")) { when = 0; }
            if (words.Any(t => t == "tomorrow")) { when = 1; }

            if (words.IndexOf("in") != -1 && words.IndexOf("in") + 1 < words.Count)
            {
                city = words[words.IndexOf("in") + 1];
            }

            WeatherClient OWM = new WeatherClient("{openWeatherMapAppId}");
            var weatherRecords = await OWM.Forecast(city);
            var weather = weatherRecords[when];

            StringBuilder sb = new StringBuilder();

            if (keyWords["temperature"])
            {
                sb.Append($"The temperature on {weather.Date} in {city} is {weather.Temp}.\r\n");
            }
            if (keyWords["humidity"])
            {
                sb.Append($"Humidity on {weather.Date} in {city} is {weather.Humidity}.\r\n");
            }
            if (keyWords["pressure"])
            {
                sb.Append($"The pressure on {weather.Date} in {city} is {weather.Pressure}.\r\n");
            }
            if (sb.Length == 0) return "I do not understand :-(";

            return sb.ToString();
        }
        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
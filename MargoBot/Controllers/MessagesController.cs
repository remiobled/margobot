using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace MargoBot
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

                var entities = await GetEntitiesFromLUIS(activity.Text);
                // return our reply to the user
                string answer = "";
                if (entities.Any() == false)
                    answer = "Désolé, je n'ai pas bien compris.";
                else
                {
                    foreach (string entity in entities)
                    {
                        switch (entity)
                        {
                            case ("PHP"):
                                answer += "En PHP, on a une offre très intéressante chez Engie, si tu maîtrises Symfony 3 envoie vite ton CV à Maïlys.\n";
                                break;
                            case ("JAVA"):
                                answer += "Malheureusement rien en ce moment en Java.\n";
                                break;
                            case ("JAVASCRIPT"):
                                answer += "On recherche pleins de profils Javascript, particulièrement en Node.js.\n";
                                break;
                            case ("VBA"):
                                answer += "Rien pour l'instant en VBA, désolé :-(\n";
                                break;
                            case ("MOA"):
                                answer += "La MOA... Voyons voir, on a une super opportunité sur un moteur de risques chez BNP!\n";
                                break;
                            case ("VB.NET"):
                                answer += "Nous n'avons plus rien à proposer en VB.NET depuis assez longtemps, désolé.\n";
                                break;
                            case ("C#"):
                            case ("CSHARP"):
                                answer += "Plein de possibilités en C# en ce moment, toutes les frameworks, toutes les technos.\n";
                                break;
                            case ("WEB"):
                                answer += "Ah! Si tu as fait HTML en LV2 au collège, appelle nous sans tarder.\n";
                                break;
                            case ("C"):
                            case ("C++"):
                                answer += "Rien de très intéressant à te proposer en C/C++, mais nos commerciaux y travaillent. Reviens régulièrement pour avoir des nouvelles.\n";
                                break;
                            case("F#"):
                                answer += "Les meilleurs missions sont en F#. Il y a des missions uniquement pour les meilleurs\n";
                                break;
                            default:
                                answer += "C'est une demande trop précise, appelle directement Maïlys.\n";
                                break;
                        }
                    }
                }

                Activity reply = activity.CreateReply(answer);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private static async Task<string[]> GetEntitiesFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            LUISAnswer Data = new LUISAnswer();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://api.projectoxford.ai/luis/v2.0/apps/2c0f3fdd-9cc0-45fa-85ae-29df2812a57b?subscription-key=e371904887674dc0ab400305cc671bdc&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<LUISAnswer>(JsonDataResponse);
                }
            }
            return Data.entities.Select(entity => entity.entity.ToUpperInvariant().Replace(" ", "")).ToArray();
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
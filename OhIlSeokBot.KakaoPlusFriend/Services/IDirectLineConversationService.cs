using Microsoft.Bot.Connector.DirectLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhIlSeokBot.KakaoPlusFriend.Services
{
    public interface IDirectLineConversationService
    {
        Task ConnectAsync(string userkey);

        Task DisconnectAsync(string userkey);

        Task SendMessageAsync(string userkey, Activity activity);

        Task<IList<Activity>> ReceiveMessageAsync(string userkey);

        Task<IList<Activity>> SendAndReceiveMessageAsync(string userkey, Activity activity);
    }
}

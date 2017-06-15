using OhIlSeokBot.KakaoPlusFriend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhIlSeokBot.KakaoPlusFriend.Services
{
    public interface ISessionService
    {
        Task<ConversationInfo> GetInfoAsync(string userkey);

        Task SetInfoAsync(ConversationInfo info);

        Task DeleteInfoAsync(string userkey);
    }
}

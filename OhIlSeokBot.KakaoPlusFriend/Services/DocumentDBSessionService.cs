using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using OhIlSeokBot.KakaoPlusFriend.Models;

namespace OhIlSeokBot.KakaoPlusFriend.Services
{
    public class DocumentDBSessionService : ISessionService
    {
        public async Task DeleteInfoAsync(string userkey)
        {
            await DocumentDBRepository<ConversationInfo>.DeleteItemAsync(userkey);
        }

        public async Task<ConversationInfo> GetInfoAsync(string userkey)
        {
            var items = await DocumentDBRepository<ConversationInfo>.GetItemsAsync(d => d.id == userkey);
            return items.FirstOrDefault();
        }

        public async Task SetInfoAsync(ConversationInfo info)
        {
            var items = await DocumentDBRepository<ConversationInfo>.GetItemsAsync(d => d.id == info.id);
            var item = items.FirstOrDefault();
            if (item == null)
            {
                await DocumentDBRepository<ConversationInfo>.CreateItemAsync(info);
            }
            else
            {
                await DocumentDBRepository<ConversationInfo>.UpdateItemAsync(info.id, info);
            }
        }
    }
}
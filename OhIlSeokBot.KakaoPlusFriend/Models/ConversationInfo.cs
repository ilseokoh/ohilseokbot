using Microsoft.Bot.Connector.DirectLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OhIlSeokBot.KakaoPlusFriend.Models
{
    public class ConversationInfo
    {
        public string id { get; set; }

        public Conversation coversation { get; set; }

        public DateTimeOffset? timestamp { get; set; }

        public string watermark { get; set; }
    }
}
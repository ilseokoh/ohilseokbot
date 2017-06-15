using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OhIlSeokBot.KakaoPlusFriend.Models
{
    public class MessageResponse
    {
        public Message message { get; set; }

        public Keyboard keyboard { get; set; }
    }

    public class Message
    {
        public string text { get; set; }

        public Photo photo { get; set; }

        public MessageButton message_button { get; set; }
    }

    public class Photo
    {
        public string url { get; set; }

        public int width { get; set; }

        public int height { get; set; }
    }

    public class MessageButton
    {
        public string label { get; set; }

        public string url { get; set; }
    }

    public class Keyboard
    {
        public string type { get; set; }

        public string[] buttons { get; set; }
    }


}
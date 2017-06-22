using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using System.Configuration;
using OhIlSeokBot.KakaoPlusFriend.Models;
using System.Threading;

namespace OhIlSeokBot.KakaoPlusFriend.Services
{
    public class DirectLineCoversationService : IDirectLineConversationService
    {
        private ISessionService sessionService;
        private static string directLineSecret = ConfigurationManager.AppSettings["DirectLineSecret"];
        private static string botId = ConfigurationManager.AppSettings["BotId"];
        private DirectLineClient client;
        private ConversationInfo conversationinfo;
        private Conversation conversation;

        public DirectLineCoversationService(ISessionService sessionsvc)
        {
            sessionService = sessionsvc;
            client = new DirectLineClient(directLineSecret);
            client.SetUserAgent("kakao");
        }

        public async Task ConnectAsync(string userkey)
        {
            if (conversation != null) return;

            // 세션에서 ConversationInfo 가져옴 
            conversationinfo = await sessionService.GetInfoAsync(userkey);
            if (conversationinfo == null)
            {
                conversation = await client.Conversations.StartConversationAsync();
                await SaveConversationInfoAsync(conversation, userkey, "", DateTimeOffset.Now);
            }
            else
            {
                if (!conversationinfo.coversation.ExpiresIn.HasValue || !conversationinfo.timestamp.HasValue)
                {
                    conversation = await client.Conversations.ReconnectToConversationAsync(conversationinfo.coversation.ConversationId);
                    await SaveConversationInfoAsync(conversation, userkey, conversationinfo.watermark, DateTimeOffset.Now);
                }
                // timeout 체크. 
                var now = DateTimeOffset.Now;
                var timeoutdate = conversationinfo.timestamp.Value.AddSeconds(conversationinfo.coversation.ExpiresIn.Value - 300);
                var diff = timeoutdate - now;
                if (diff > TimeSpan.MinValue)
                {
                    conversation = conversationinfo.coversation;
                }
                else
                {
                    conversation = await client.Conversations.ReconnectToConversationAsync(conversationinfo.coversation.ConversationId);
                    await SaveConversationInfoAsync(conversation, userkey, conversationinfo.watermark, DateTimeOffset.Now);
                }
            }
        }

        private async Task SaveConversationInfoAsync(Conversation conversation, string userkey, string watermark, DateTimeOffset timeout)
        {
            conversationinfo = new ConversationInfo
            {
                coversation = conversation,
                id = userkey,
                timestamp = timeout,
                watermark = watermark
            };
            await sessionService.SetInfoAsync(conversationinfo);
        }

        public async Task DisconnectAsync(string userkey)
        {
            await sessionService.DeleteInfoAsync(userkey);
            // TODO : DirectLine 의 EndingConversation을 사용해서 대화 종료해줘야 함. DirectLine 라이브러리에서 찾아보든가

        }

        public async Task<IList<Activity>> ReceiveMessageAsync(string userkey)
        {
            await ConnectAsync(userkey);
            
            bool messageReceived = false;
            int requestCount = 0;
            List<Activity> responseActivity = null;
            // 5번의 Retry 로직 
            while (!messageReceived)
            {
                requestCount += 1;

                var activitySet = await client.Conversations.GetActivitiesAsync(conversationinfo.coversation.ConversationId, conversationinfo.watermark);
                conversationinfo.watermark = activitySet?.Watermark;

                await SaveConversationInfoAsync(conversation, userkey, conversationinfo.watermark, conversationinfo.timestamp.Value);

                // appSettings 에 설정한 BotId 는 bot을 등록할 때 사용한 Bot handler 와 같아야 한다. 
                var activities = from x in activitySet.Activities
                                 where x.From.Id == botId
                                 select x;

                responseActivity = activities.ToList();
                // 메시지를 받으면 루프를 벗어남. 
                if (responseActivity.Count > 0 || requestCount > 5) messageReceived = true;

                Thread.Sleep(100);
            }

            return responseActivity;
        }

        public async Task<IList<Activity>> SendAndReceiveMessageAsync(string userkey, Activity activity)
        {
            // send message through direct line 
            await SendMessageAsync(userkey, activity);
            // Receive
            var activities = await ReceiveMessageAsync(userkey);

            return activities; 
        }

        public async Task SendMessageAsync(string userkey, Activity activity)
        {
            await ConnectAsync(userkey);
            await client.Conversations.PostActivityAsync(conversation.ConversationId, activity);
        }
    }
}
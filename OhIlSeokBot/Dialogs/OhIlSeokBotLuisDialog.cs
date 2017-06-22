using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OhIlSeokBot.Dialogs
{
    [Serializable]
    [LuisModel("{Your Id form luis.ai}", "{Your secret from luis.ai}")]
    public class OhIlSeokBotLuisDialog: LuisDialog<object>
    {

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"죄송해요. 제가 할말이 없네요. ";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("인사")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            if (context.Activity.From.Name == "kakao")
            {
                string message = $"안녕하세요. 저는 오일석 봇입니다. 저를 만든 오일석을 대신해서 제가 도움을 드릴 수 있으면 좋겠네요.  저는 이런걸 할 수 있어요.\n 제 소개를 해드릴 수 있어요.\n 제 인사를 할수도 있죠.";
                await context.PostAsync(message);
            }
            else
            {
                string message = $"안녕하세요. 저는 오일석 봇입니다. 저를 만든 오일석을 대신해서 제가 도움을 드릴 수 있으면 좋겠네요.  ";
                await context.PostAsync(message);

                string message2 = $"저는 이런걸 할 수 있어요.\n 제 소개를 해드릴 수 있어요.\n 제 인사를 할수도 있죠.";
                await context.PostAsync(message2);
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("소개요청")]
        public async Task Introduce(IDialogContext context, LuisResult result)
        {
            if (context.Activity.From.Name == "kakao")
            {
                var activity = context.MakeMessage();
                activity.Text = $"제 소개를 해볼께요. 저는 오일석 봇이죠. 오일석이라는 사람 그 자체는 아니에요. 그러니까 저와 오일석이라는 사람은 다르죠. 헷갈리면 안되요.\n" +
                    $"물론 저는 오일석이라는 사람이 만들었어요.\n Microsoft Bot Framework를 사용했죠. 사용한 기술들은 이런거에요.\n " +
                    $"Microsoft Bot Framework\n" +
                    $"Visual Studio 2017, C#\n" +
                    $"Direct Line REST API 3.0\n" +
                    $"Bot Builder SDK\n" +
                    $"카카오톡 플러스 친구 자동응답 API" +
                    $"마지막으로 제 사진을 보여드릴께요";
                activity.Attachments.Add(new Attachment
                {
                    ContentType = "image/png",
                    ContentUrl = "http://kakaobot.ilseokoh.com/images/bot.png"
                });
                await context.PostAsync(activity);
                context.Wait(MessageReceived);
            }
            else
            {
                string message = $"제 소개를 해볼께요. 저는 오일석 봇이죠. 오일석이라는 사람 그 자체는 아니에요. 그러니까 저와 오일석이라는 사람은 다르죠. 헷갈리면 안되요.";
                await context.PostAsync(message);

                string message2 = $"물론 저는 오일석이라는 사람이 만들었어요.\n Microsoft Bot Framework를 사용했죠. 사용한 기술들은 이런거에요.\n " +
                    $"Microsoft Bot Framework\n" +
                    $"Visual Studio 2017, C#\n" +
                    $"Direct Line REST API 3.0\n" +
                    $"Bot Builder SDK\n" +
                    $"카카오톡 플러스 친구 자동응답 API";

                await context.PostAsync(message2);

                var activity = context.MakeMessage();
                activity.Text = $"마지막으로 제 사진을 보여드릴께요";
                activity.Attachments.Add(new Attachment
                {
                    ContentType = "image/png",
                    ContentUrl = "http://kakaobot.ilseokoh.com/images/bot.png"
                });
                await context.PostAsync(activity);
                context.Wait(MessageReceived);
            }
        }
    }
}
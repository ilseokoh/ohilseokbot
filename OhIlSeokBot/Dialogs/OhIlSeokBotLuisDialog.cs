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
    [LuisModel("c11e240f-b920-43cf-a82a-374a876c90fb", "ddc7fee2d34b4878962ad58e0ef01302")]
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
            string message = $"안녕하세요. 저는 오일석 봇입니다. 저를 만든 오일석을 대신해서 제가 도움을 드릴 수 있으면 좋겠네요.  ";
            await context.PostAsync(message);

            string message2 = $"저는 이런걸 할 수 있어요.\n 날씨를 물어보세요.(내일 서울 날씨는 어때?)\n 제 소개를 해드릴 수 있어요.\n 제 사진을 보여 드릴 수 도 있죠.";
            await context.PostAsync(message2);
            context.Wait(MessageReceived);
        }

        [LuisIntent("소개요청")]
        public async Task Introduce(IDialogContext context, LuisResult result)
        {
            if (context.Activity.From.Name == "kakao")
            {

            }

            string message = $"제 소개를 해볼께요. 저는 오일석 봇이죠. 오일석이라는 사람 그 자체는 아니에요. 그러니까 저와 오일석이라는 사람은 다르죠. 헷갈리면 안되요.";
            await context.PostAsync(message);

            string message2 = $"물론 저는 오일석이라는 사람이 만들었어요.\n Microsoft Bot Framework를 사용했죠. 사용한 기술들은 이런거에요.\n " +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"";
            await context.PostAsync(message2);

            var activity = context.MakeMessage();
            activity.Text = $"마지막으로 제 사진을 보여드릴께요";
            activity.Attachments.Add(new Attachment
            {
                ContentType = "image/jpeg",
                ContentUrl = ""
            });
            await context.PostAsync(activity);
            context.Wait(MessageReceived);
        }
    }
}
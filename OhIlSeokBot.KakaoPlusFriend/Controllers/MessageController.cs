using OhIlSeokBot.KakaoPlusFriend.Helpers;
using OhIlSeokBot.KakaoPlusFriend.Models;
using OhIlSeokBot.KakaoPlusFriend.Services;
using Microsoft.Bot.Connector.DirectLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OhIlSeokBot.KakaoPlusFriend.Controllers
{
    public class MessageController : Controller
    {
        private IDirectLineConversationService conversationService;

        public MessageController(IDirectLineConversationService service)
        {
            conversationService = service;
        }

        /// <summary>
        /// 실제 카카오톡과 메시지를 주고 받는 부분. 
        /// 
        /// </summary>
        /// <param name="user_key">해시된 사용자 키</param>
        /// <param name="type">text / photo </param>
        /// <param name="content">내용</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Index(string user_key, string type, string content)
        {
            try
            {
                // covert from Kakao talk message to Bot Builder Activity
                Activity activity = new Activity
                {
                    // Bot 에서 메시지가 kakao로 부터 요청되었음을 알수 있도록 name에 kakao를 써준다. 
                    From = new ChannelAccount(id:user_key,name:"kakao"),
                    Type = ActivityTypes.Message
                };
                if (type == "text")
                {
                    activity.Text = content;
                }
                else if (type == "photo")
                {
                    activity.Attachments = new List<Attachment>();
                    activity.Attachments.Add(new Attachment
                    {
                        ContentUrl = content
                    });
                }
                var response = await conversationService.SendAndReceiveMessageAsync(user_key, activity);
                // 발견된 복수의 Activity를 넘겨서 처리
                var msg = MessageConvertor.DirectLineToKakao(response);
                return Json(msg);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Direct Line 연결오류", ex);
            }
        }
    }
}
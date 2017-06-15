using OhIlSeokBot.KakaoPlusFriend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OhIlSeokBot.KakaoPlusFriend.Controllers
{

    public class ChatroomController : Controller
    {
        private IDirectLineConversationService conversationService; 

        public ChatroomController(IDirectLineConversationService service)
        {
            conversationService = service;
        }
        /// <summary>
        /// 사용자가 채팅방 나가기를 해서 채팅방을 목록에서 삭제했을 경우 해당 정보를 파트너사 서버로 전달하는 API입니다.
        /// http(s)://:your_server_url/chat_room/:user_key
        /// application/json; charset=utf-8
        /// </summary>
        /// <param name="userkey"></param>
        /// <returns>HTTP Status 200</returns>
        [AcceptVerbs(HttpVerbs.Delete)]
        public async Task<ActionResult> Index(string userkey)
        {
            // 카카오 채팅방을 삭제해서 나가버리면 
            // direct line api를 통해서 coversation을 종료한다. 
            try
            {
                await conversationService.DisconnectAsync(userkey);
            }
            catch(Exception ex)
            {
                // session에서 지울 때 아이템이 없으면 에러가 발생하는데 굳이 500 에러를 던지지 않음. 중요한게 아님 
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
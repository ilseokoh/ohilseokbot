using OhIlSeokBot.KakaoPlusFriend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhIlSeokBot.KakaoPlusFriend.Controllers
{
    public class KeyboardController : Controller
    {
        /// <summary>
        /// 이용자가 최초로 채팅방에 들어올 때 기본으로 키보드 영역에 표시될 자동응답 명령어의 목록을 호출하는 API입니다.
        /// 챗팅방을 지우고 다시 재 진입시에도 호출되거나 자동응답 메시지를 주고받았을 경우 마지막 메시지의 명령어 목록이 표시. 
        /// 10분이 지난 다음에는 다시 keyboard 가 호출되어 초기화됨. 
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            var buttons = new Keyboard
            {
                type = "buttons",
                buttons = new string[] {"인사","소개"}
            };
            return Json(buttons, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OhIlSeokBot.KakaoPlusFriend.Controllers
{
    public class FriendController : Controller
    {
        /// <summary>
        /// 특정 카카오톡 이용자가 플러스친구/옐로아이디를 친구로 추가하거나 차단하는 경우 해당 정보를 파트너사 서버로 전달하는 API입니다.
        /// 
        /// </summary>
        /// <param name="userkey"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Post)]
        public ActionResult Index(string userkey)
        {
            // 친구추가/차단은 카카오톡 기능이고 Bot framework와 연결되는 기능이나 역할이 없으므로 사용하지 않음. 
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
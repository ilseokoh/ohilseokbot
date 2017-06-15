using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OhIlSeokBot.KakaoPlusFriend.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 단순 홈페이지로 플러스 친구 추가 안내
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
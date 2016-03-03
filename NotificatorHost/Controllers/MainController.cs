using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotificatorHost.Controllers
{
    public class MainController : Controller
    {

        // GET: Main
        public ActionResult Index()
        {
            var list = new List<Entry>
            {
                new Entry { id = 1, body = "body1", title = "title1" },
                new Entry { id = 1, body = "body2", title = "title2" },
                new Entry { id = 1, body = "body3", title = "title3" }
            };
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AjaxJsonTest.Models;

namespace AjaxJsonTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetJson(Person person) {            
            var str = person.Name == null ? "No" : "Ok";
            var js = new JavaScriptSerializer();
            var personStr = js.Serialize(person);
            var a = Json(person);
            var v = new { Result = str, Data = personStr };
            return Json(v, JsonRequestBehavior.AllowGet);
        }
    }
}

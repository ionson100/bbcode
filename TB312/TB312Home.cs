using System.Web.Mvc;

namespace TB312
{
    public class Tb312Controller : Controller
    {
        [HttpPost]
        public string Index(string id)
        {
            if (id == "o")
            {
                var data=  this.HttpContext.Request.Form["data"];
               var ddata = Server.UrlDecode(data);
               var res = ControlActivator.DecodeText(ddata, true);
                return res;
            }
            return "хуй";

        }
    }
}

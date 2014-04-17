using System.Web.Mvc;
using TB312;

namespace TextBox.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var f=new ModelTest {Message = ""};

            return View(f);
        }

        [HttpPost]
        public ActionResult Index(ModelTest model)
        {

            model.MessageOut = ControlActivator.DecodeText(model.Message,false);
            model.Message = string.Empty;

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
     
    }
    public class ModelTest
    {   [AllowHtml]
        public string Message { get; set; }
        public string MessageOut { get; set; }
    }
}

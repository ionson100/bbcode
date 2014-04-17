using System;
using TB312;

namespace WebApplication1
{
    public partial class Textdecoder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          var data=  this.Request.Form["data"];
          var ddata = Server.UrlDecode(data);
          var res = ControlActivator.DecodeText(ddata, true);
          Response.Write(res);

        }
    }
}
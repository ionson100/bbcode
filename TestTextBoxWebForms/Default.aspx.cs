using System;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1Click(object sender, EventArgs e)
        {
        }

        protected void TextBoxForm1SaveText(object sender, string str)
        {
            Literal1.Text = str;
        }
    }
}
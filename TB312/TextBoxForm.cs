using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace TB312
{   [DefaultEvent("SaveText")]
    [DesignerAttribute(typeof(TextBoxControlDesigner))]
    [ToolboxData("<{0}:TextBoxForm runat=server></{0}:TextBoxForm>")]
    public class TextBoxForm : WebControl
    {
        /// <summary>
        ///Делегат для событие сохранения текста
        /// </summary>
        /// <param name="sender">я</param>
        /// <param name="str">Строка защищенная</param>
        public delegate void TextBoxEventHandler(object sender, string str);
          /// <summary>
        /// Событие сохранения текста
          /// </summary>
        public event TextBoxEventHandler SaveText;

        protected virtual void OnSave(string str) 
        {
            var handler = SaveText;
            if (handler == null) return;
            DirtyString = string.Empty;
            handler(this, str);
        }
        /// <summary>
        /// Не защищенная строка
        /// </summary>
       
        public  string DirtyString { get; set; }
        /// <summary>
        /// Чистая защищенная строка
        /// </summary>
        public string CleanString{get { return ControlActivator.DecodeText(DirtyString, false); }}
        private const string  T="texteditor_";

        protected override void OnLoad(EventArgs e)
        { 
            base.OnInit(e);
            if(Context.Request.RequestType!="POST")return;
            DirtyString = Context.Request.Form[T+ID];
            if (!string.IsNullOrEmpty(Context.Request.Form["bt-save-tb312"]))
            OnSave(ControlActivator.DecodeText(DirtyString,false));
        }

        

        protected override void RenderContents(HtmlTextWriter output)
        {
            var sb = new StringBuilder();
            if(string.IsNullOrEmpty(ControlActivator.Path))
               ControlActivator. Path = Context.Server.MapPath("/activatescript.txt");
            foreach (var s in ControlActivator.Ledydi.Value)
            {
                sb.AppendFormat("<li onmousedown=\"insertCode('[SCR {1}]','{3}')\">{0}</li>{2}", s.Key, s.Value, Environment.NewLine, "[/SCR]");
            }
            output.Write(Properties.Resources.TextBox.Replace("#data#", sb.ToString()).Replace("#name#", T + ID).Replace("#value#", DirtyString));
        }

    }
    class TextBoxControlDesigner : ControlDesigner
    {
        protected override void OnClick(DesignerRegionMouseEventArgs e)
        {
            DoDefaultAction();
            base.OnClick(e);
        }

        public override string GetDesignTimeHtml()
        {
            return Properties.Resources.TextBox.Replace("#data#", "")
                             .Replace("#name#", ID)
                             .Replace("#value#", " http://codearticles.ru/  -Рецепты по различным технологиям");
        }
    }
}

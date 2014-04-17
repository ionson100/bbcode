using System;
using System.Collections.Generic;
using System.Text;

namespace TB312
{
    internal  class  RenderTextBox
    {
        public string RenderingCore(string name, object value, Dictionary<string, string> ledydi)
        {
            var sb=new StringBuilder();
           
            foreach (var s in ledydi)
            {
                sb.AppendFormat("<li onmousedown=\"insertCode('[SCR {1}]','{3}')\">{0}</li>{2}", s.Key, s.Value, Environment.NewLine, "[/SCR]");
            }
            return Properties.Resources.TextBox.Replace("#data#",  sb.ToString()).Replace("#name#",name).Replace("#value#",value==null?string.Empty:value.ToString());
        }
        
    }
}

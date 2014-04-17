using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Encoder = Microsoft.Security.Application.Encoder;

namespace TB312
{
    public static class ControlActivator
    {
        
        public static string Path;
        //регулируем список языков Внимание!!Если вы добавили язык - пристегните скрипт этого языка к вьюхе
        public  static readonly Lazy<Dictionary<string, string>> Ledydi = new Lazy<Dictionary<string, string>>(() =>
                                                                  {
                                                                      var res = File.ReadLines(Path).ToDictionary(str => str.Split('#')[0],
                                                                                            str => str.Split('#')[1]);
                                                                      return res;
                                                                  }, LazyThreadSafetyMode.ExecutionAndPublication);
        public static MvcHtmlString TextBoxScriptFor<TModel, T>(this HtmlHelper<TModel> htmlHelper,
                                                                Expression<Func<TModel, T>> expression)
        {
            if (string.IsNullOrEmpty(Path))
            {
                htmlHelper.RouteCollection.MapRoute("textboxscr", "tb/{controller}/{action}/{id}",
                                            namespaces: new[] {typeof(Tb312Controller).Namespace },
                                            defaults: new { controller = "Tb312", action = "Index", id = UrlParameter.Optional });
                Path = htmlHelper.ViewContext.RequestContext.HttpContext .Server.MapPath( "/TB12/activatescript.txt");
            }
            var name= ExpressionHelper.GetExpressionText(expression);
            var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            return MvcHtmlString.Create(new RenderTextBox().RenderingCore(name,value,Ledydi.Value));
        }
        /// <summary>
        /// Декодирование текста от всякой хуйни
        /// </summary>
        /// <param name="str">стрка с клиента</param>
        /// <param name="isAjax"> каким способом принимаем строку</param>
        /// <returns>строка защищенная от всякой хуйни</returns>
        public static string DecodeText(string str, bool isAjax)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var sb = new StringBuilder(Encoder.HtmlEncode(str));
            // var sb = new StringBuilder(str);
            //заменяем все пререводы на br
            sb= sb.Replace(isAjax ? "&#10;" : "&#13;&#10;", "<br />");
           
            //заменяем все пробелы где два и более на символы пробела
            sb = new Regex(@"\s{2,}").Matches(str).Cast<object>().
                Aggregate(sb, (current, s) => current.Replace(s.ToString(), s.ToString().Replace(" ", "&nbsp;")));


                sb = ParseColor(sb);
                sb = ParsrLink(sb);
                sb = ParseS(sb);
                sb = ParseU(sb);
                sb = ParseI(sb);
                sb = ParseB(sb);
                sb = ParsrLi(sb);
                sb = ParseSize(sb);
                sb = ParseFixed(sb);
                sb = ParseSupSub(sb);
                sb = ParseQuote(sb);
                sb=ParseImage(sb);
                sb = ParseCsv(sb);


            //Заменяем в этих кусках  представления, символьные пробелы на пробелы и символы превода строки на перевод Ansi
                var matches22 = new Regex("[[]SCR [a-z]*](.|\n)*[[]/SCR]").Matches(sb.ToString());
                sb = matches22.Cast<object>().
                    Aggregate(sb, (current, ss) => current.Replace(ss.ToString(), ss.ToString().
                        Replace("&nbsp;", " ").Replace("<br />", "&#13;&#10;")));




                // парсим  подсетку кода cvdsdsdvsd
                var matches12 = new Regex("[[]SCR [a-z]*]").Matches(sb.ToString());//Находим все блоки начала блока кода[SCR ..]
                sb = sb.Replace("[/SCR]", "</code></pre>");//заменяем все блоки окончания кода [/SCR]
                foreach (var match in matches12)//заменяем все блоки начала кода
                {
                    var ee = match.ToString().Substring(match.ToString().IndexOf(" ", StringComparison.Ordinal) + 1).
                        Substring(0, match.ToString().Substring(match.ToString().IndexOf(" ", StringComparison.Ordinal) + 1).IndexOf(']'));
                    sb = sb.Replace(match.ToString(), string.Format("<pre class=\"code\"> <code class='{0}'>", ee));
                }
                return sb.ToString().Replace("&#10;", ""); // убираем новую строку, хуй его знает какя пизда его генерит через чур в конце кажой строки, он тут нах не нужен


        }

        private static StringBuilder ParseCsv(StringBuilder sb)
        {
            var mat4 = new Regex(@"[[]CSV](.|\n)*[[]\/CSV]").Matches(sb.ToString());
            foreach (var match in mat4)
            {
                var text1 = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                  Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                var text = GetHtmlTable(text1);
                sb = sb.Replace(match.ToString(), "<b>" + text + "</b>");
            }
            return sb;
        }

        private static StringBuilder ParseQuote(StringBuilder sb)
        {
            var matquote = new Regex(@"[[]quote (.|\n)*](.|\n)*[[]\/quote]").Matches(sb.ToString());
            foreach (var match in matquote)
            {
                var autor = match.ToString().Substring(match.ToString().IndexOf(' ')).
                                  Substring(0, match.ToString().Substring(match.ToString().IndexOf(' ')).IndexOf(']'));
                var te = match.ToString().Replace(@"[/quote]", "").
                               Substring(match.ToString().Replace(@"[/quote]", "").IndexOf(']') + 1);

                sb = sb.Replace(match.ToString(), Properties.Resources.Quote.Replace("#autor#", autor).Replace("#text#", te));
            }
            return sb;
        }

        private static StringBuilder ParseSupSub(StringBuilder sb)
        {
            var matsup = new Regex(@"[[]SUP(.|\n)*](.|\n)*[[]\/SUP]").Matches(sb.ToString());
            foreach (var match in matsup)
            {
                var text = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                 Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                sb = sb.Replace(match.ToString(), string.Format("<sup>{0}</sup>", text));
            }

            var matsub = new Regex(@"[[]SUB(.|\n)*](.|\n)*[[]\/SUB]").Matches(sb.ToString());
            foreach (var match in matsub)
            {
                var text = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                 Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                sb = sb.Replace(match.ToString(), string.Format("<sub>{0}</sub>", text));
            }
            return sb;
        }

        private static StringBuilder ParseFixed(StringBuilder sb)
        {
            var matfix = new Regex(@"[[]FIXED(.|\n)*](.|\n)*[[]\/FIXED]").Matches(sb.ToString());
            foreach (var match in matfix)
            {
                var text = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                 Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                sb = sb.Replace(match.ToString(),
                                string.Format("<pre style=\"margin: 1px; font: 9pt Courier New, monospace\">{0}</pre>", text));
            }
            return sb;
        }

        private static StringBuilder ParseSize(StringBuilder sb)
        {
            sb = sb.Replace("[/SIZE]", "</font>");
            var matlsize = new Regex(@"[[]SIZE=[1,2,3,4,5]]").Matches(sb.ToString());
            foreach (var match in matlsize)
            {
                var num = match.ToString().Replace("[SIZE=", "").Replace("]", "");
                sb = sb.Replace(match.ToString(), string.Format("<font size={0}>", num));
            }
            return sb;
        }

        private static StringBuilder ParsrLi(StringBuilder sb)
        {
            var matli = new Regex(@"[[]li](.|\n)*[[]li]").Matches(sb.ToString());
            foreach (var match in matli)
            {
                var text = match.ToString().Replace("[li]", "");
                var textrepl = match.ToString().Substring(0, match.ToString().LastIndexOf('['));
                sb = sb.Replace(textrepl, string.Format("<li>{0}</li>", text));
            }
            var matli1 = new Regex(@"[[]li](.|\n)*$").Matches(sb.ToString());
            foreach (var match in matli1)
            {
                var text = match.ToString().Replace("[li]", "");
                sb = sb.Replace(match.ToString(), string.Format("<li>{0}</li>", text));
            }
            return sb;
        }

        private static StringBuilder ParseB(StringBuilder sb)
        {
            var mat4 = new Regex(@"[[]b](.|\n)*[[]\/b]").Matches(sb.ToString());
            foreach (var match in mat4)
            {
                var text = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                 Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                sb = sb.Replace(match.ToString(), string.Format("<b>{0}</b>", text));
            }
            return sb;
        }

        private static StringBuilder ParseI(StringBuilder sb)
        {
            var mat3 = new Regex(@"[[]i](.|\n)*[[]\/i]").Matches(sb.ToString());
            foreach (var match in mat3)
            {
                var text = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                 Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                sb = sb.Replace(match.ToString(), string.Format("<i>{0}</i>", text));
            }
            return sb;
        }

        private static StringBuilder ParseU(StringBuilder sb)
        {
            var mat2 = new Regex(@"[[]u](.|\n)*[[]\/u]").Matches(sb.ToString());
            foreach (var match in mat2)
            {
                var text = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                 Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                sb = sb.Replace(match.ToString(), string.Format("<u>{0}</u>", text));
            }
            return sb;
        }

        private static StringBuilder ParseS(StringBuilder sb)
        {
            var mat1 = new Regex(@"[[]s](.|\n)*[[]\/s]").Matches(sb.ToString());
            foreach (var match in mat1)
            {
                var text = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                 Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                sb = sb.Replace(match.ToString(), string.Format("<s>{0}</s>", text));
            }
            return sb;
        }

        private static StringBuilder ParsrLink(StringBuilder sb)
        {
            var mat = new Regex(@"[[]url=(.|\n)*](.|\n)*[[]\/url]").Matches(sb.ToString());
            foreach (var match in mat)
            {
                var url = match.ToString().Substring(match.ToString().IndexOf('=') + 1).
                                Substring(0, match.ToString().Substring(match.ToString().IndexOf('=')).IndexOf(']') - 1);
                var name = match.ToString().Substring(match.ToString().IndexOf(']') + 1).
                                 Substring(0, match.ToString().Substring(match.ToString().IndexOf(']') + 1).IndexOf('['));
                if (url.IndexOf("http://", StringComparison.Ordinal) == -1) //если пользователь забыл указать http://
                {
                    url = string.Format("http://{0}", url);
                }

                sb = sb.Replace(match.ToString(), string.Format("<a target=\"_blank\" href=\"{0}\">{1}</a>", url, name));
            }
            return sb;
        }

        private static StringBuilder ParseColor(StringBuilder sb)
        {
            var matches = new Regex("[[]color=[a-z]*]").Matches(sb.ToString());
            sb = sb.Replace("[/color]", "</font>");
            foreach (var match in matches)
            {
                var color = match.ToString().Substring(match.ToString().IndexOf('=') + 1).Trim(']');
                sb = sb.Replace(match.ToString(), string.Format("<font color=\"{0}\">", color));
            }
            return sb;
        }
        private static StringBuilder ParseImage(StringBuilder sb)
        {
            var matches = new Regex("[[]img=(.|\n)*]").Matches(sb.ToString());
           
            foreach (var match in matches)
            {
                var url = match.ToString().Substring(match.ToString().IndexOf('=') + 1).Trim(']');
               sb=sb.Replace(match.ToString(),string.Format("<img alt=\"\" src=\"{0}\">", url));
            }
            return sb;
        }
       
       

        private  static string GetHtmlTable(string datastr)
        {
            if (string.IsNullOrEmpty(datastr)) return string.Empty;
            datastr = datastr.Replace("<br />", "\n");
            var rov = datastr.Trim("\n".ToArray()).Split("\n".ToArray());
            var baselist= rov.Select(s => new List<string>(s.Split(",".ToArray()))).ToList();
            var table = new Table {CssClass = "csv-table",CellPadding = 2,CellSpacing = 1};
            var i = 0;
            foreach (var rs in baselist)
            {
                if (i == 0)
                {
                    var rh=new TableHeaderRow();
                    foreach (var r in rs) 
                    {
                        rh.Cells.Add(new TableHeaderCell { Text = r});
                    }
                    table.Rows.Add(rh);
                }
                else
                {
                    var rc = new TableRow();
                    foreach (var r in rs)
                    {
                        rc.Cells.Add(new TableCell { Text = r });
                    }
                    table.Rows.Add(rc);
                   
                }
                i++;
            }
            return RenderControl(table);
         }
        private static string RenderControl(Control control)
        {
            using (var sw = new StringWriter())
            {
                var tw = new HtmlTextWriter(sw);

                control.RenderControl(tw);
                return sw.ToString();

            }
        }
   
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcActiveDirectoryAuthentication.Helpers
{
    public class ObjectModel1
    {
        public string Name { get; set; }
    }

    public class ObjectModel2
    {
        public string Name { get; set; }
    }
    public class ObjectModel3
    {
        public string Name { get; set; }
    }
    public class Widget
    {
        public int WidgetOrder { get; set; }
        public string WidgetPartialView { get; set; }
        public object Model { get; set; }
    }
    public static class HtmlRenderHelper
    {
        private static object LockObject = new object();
        public static string RenderZone(this HtmlHelper helper, IList<Widget> widgets, ViewContext viewContext)
        {
            SortedDictionary<int, string> result = new SortedDictionary<int, string>();
            StringBuilder sb = new StringBuilder();

            foreach (var widget in widgets)
            {
                var viewEngineResult = ViewEngines.Engines.FindPartialView(viewContext.Controller.ControllerContext, widget.WidgetPartialView);
                
                if (viewEngineResult.View != null)
                {
                    var partialData = PartialExtensions.Partial(helper, widget.WidgetPartialView, widget.Model);
                    result.Add(widget.WidgetOrder, partialData.ToString());                    
                }
            }
            IEnumerable<string> outputs = result.Select(kvp => kvp.Value);
            foreach (string html in outputs)
            {
                sb.Append(html);
            }
            return sb.ToString();
        }

        public static string RenderZoneParallel(this HtmlHelper helper, IList<Widget> widgets, ViewContext viewContext)
        {
            SortedDictionary<int, string> result = new SortedDictionary<int, string>();
            StringBuilder sb = new StringBuilder();

            var context = HttpContext.Current;
            Parallel.ForEach(widgets, widget => {
                HttpContext.Current = context;

                var viewEngineResult = ViewEngines.Engines.FindPartialView(viewContext.Controller.ControllerContext, widget.WidgetPartialView);

                if (viewEngineResult.View != null)
                {
                    var partialData = PartialExtensions.Partial(helper, widget.WidgetPartialView, widget.Model);
                    lock (LockObject)
                    {
                        result.Add(widget.WidgetOrder, partialData.ToString());
                    }
                }
            });
            IEnumerable<string> outputs = result.Select(kvp => kvp.Value);
            foreach (string html in outputs)
            {
                sb.Append(html);
            }
            return sb.ToString();
        }

    }
}
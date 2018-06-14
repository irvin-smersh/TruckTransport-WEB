using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace TruckTransport_Web.Helper
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this HtmlHelper html, string action, string control)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl;

            return returnActive ? "active" : "";
        }

        public static string IsMenuOpen(this HtmlHelper html, string action, string control)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl;

            return returnActive ? "menu-open" : "";
        }

        public static string ShowItems(this HtmlHelper html, string action, string control)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl;

            return returnActive ? "display:block" : "";
        }
    }
}

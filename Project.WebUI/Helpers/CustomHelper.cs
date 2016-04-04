using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project.WebUI.Helpers
{
    /// <summary>
    /// Used to create a link to another web site outside of a project
    /// </summary>
    public static class CustomHelpers
    {

        /// <summary>
        /// Create an html link. Useful for creating a link to external sites
        /// 
        /// Usage:  @Html.Link("Text or image to Show", "the link", new { attriblute = "an-attribute" }, new { parameter = aparameter })           
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkContents">Contents that are shown to be a link</param>
        /// <param name="href">Location that link will go to</param>
        /// <param name="attr">Html attributes to add to the link</param>
        /// <param name="param">URL Parameters</param>
        /// <returns></returns>
        public static IHtmlString UrlLink(this HtmlHelper htmlHelper, string linkContents, string href, object attr = null, object param = null)
        {

            var htmlAttributes = new RouteValueDictionary();

            if (attr != null)
            {

                if (attr.GetType() == typeof(RouteValueDictionary))
                {
                    htmlAttributes = (RouteValueDictionary)attr;
                } else
                {
                    htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attr);
                }

            }

            var sb = new StringBuilder();

            if (param != null)
            {

                var rv = param.ToDictionary<string>();

                if (rv != null)
                {

                    var count = 0;

                    foreach (var key in rv.Keys)
                    {

                        sb.Append(count == 0 ? "?" : "&");
                        sb.Append(key + "=" + rv[key]);

                        count++;
                    }

                }
            }

            var url = href + sb;

            var tagBuilder = new TagBuilder("a") { InnerHtml = htmlHelper.Raw(linkContents).ToString() };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return new HtmlString(tagBuilder.ToString(TagRenderMode.Normal));

        }

    }

}

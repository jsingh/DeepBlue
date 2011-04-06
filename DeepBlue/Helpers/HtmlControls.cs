using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Collections.Specialized;
using System.Text;

namespace DeepBlue.Helpers
{
    public static class HtmlControls
    {

        #region Image
        public static MvcHtmlString Image(this HtmlHelper helper, string imagename)
        {
            TagBuilder tag = new TagBuilder("img");
            tag.Attributes.Add("src", "/Assets/images/" + imagename);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString Image(this HtmlHelper helper, string imagename, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("img");
            tag.Attributes.Add("src", "/Assets/images/" + imagename);
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString ImageButton(this HtmlHelper helper, string imagename)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.Attributes.Add("type", "image");
            tag.Attributes.Add("src", "/Assets/images/" + imagename);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString ImageButton(this HtmlHelper helper, string imagename, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.Attributes.Add("type", "image");
            tag.Attributes.Add("src", "/Assets/images/" + imagename);
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        #endregion

        #region Span
        public static MvcHtmlString Span(this HtmlHelper helper, string innerHTML)
        {
            TagBuilder tag = new TagBuilder("span");
            tag.InnerHtml = innerHTML;
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString Span(this HtmlHelper helper, string innerHTML, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("span");
            tag.InnerHtml = innerHTML;
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        #endregion

        #region Anchor
        public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML, string href, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("a");
            if (string.IsNullOrEmpty(href) == false)
            {
                tag.Attributes.Add("href", href);
            }
            tag.InnerHtml = innerHTML;
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML, object htmlAttributes)
        {
            return Anchor(helper, innerHTML, "#", htmlAttributes);
        }
        public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML)
        {
            return Anchor(helper, innerHTML, "#", new { });
        }
        #endregion

        #region javascript
        public static string JavascriptInclueTag(this HtmlHelper helper, string scriptname)
        {
            return string.Format("<script src=\"/Assets/javascripts/{0}\" type=\"text/javascript\"></script>", scriptname);
        }
        #endregion

        #region stylesheet
        public static string StylesheetLinkTag(this HtmlHelper helper, string cssname)
        {
            return string.Format("<link href=\"/Assets/stylesheets/{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssname);
        }
        #endregion

        #region Tab
        public static MvcHtmlString Tab(this HtmlHelper helper, string innerHTML)
        {
            TagBuilder tag = new TagBuilder("div");
            tag.InnerHtml = innerHTML;
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString Tab(this HtmlHelper helper, string innerHTML, string cssClassName)
        {
            TagBuilder tag = new TagBuilder("div");
            tag.InnerHtml = innerHTML;
            tag.AddCssClass(cssClassName);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString Tab(this HtmlHelper helper, string innerHTML, string cssClassName, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("div");
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.InnerHtml = innerHTML;
            tag.AddCssClass(cssClassName);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        #endregion

        #region File
        public static MvcHtmlString File(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.Attributes.Add("type", "file");
            tag.Attributes.Add("name", name);
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        #endregion

        #region Menu
        public static MvcDiv TopMenu(this HtmlHelper helper, bool isSelect)
        {
            return TopMenu(helper, isSelect, new { });
        }
        public static MvcDiv TopMenu(this HtmlHelper helper, bool isSelect, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("topmenu");
            if (isSelect) tagBuilder.AddCssClass("tab-sel");
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
            httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            System.Text.StringBuilder sb = new StringBuilder();
            httpResponse.Write("<div class=\"topmenu-left\"></div><div class=\"topmenu-center\">");
            MvcDiv menu = new MvcDiv(helper.ViewContext.HttpContext.Response);
            menu.OnDispose += new MvcDiv.DisposeEventHandler(menu_OnDispose);
            return menu;
        }
        public static void menu_OnDispose(HttpResponseBase httpResponse)
        {
            httpResponse.Write("</div><div class=\"topmenu-right\"></div>");
        }
        public static MvcHtmlString TopMenuLink(this HtmlHelper helper, string menuName)
        {
            return Anchor(helper, menuName, "#", new { });
        }
        public static MvcHtmlString TopMenuLink(this HtmlHelper helper, string menuName, string subMenuId)
        {
            return Anchor(helper, menuName, "", new { @onmouseover = "mopen(this,'" + subMenuId + "')", @onmouseout = "mclosetime()" });
        }
        public static MvcDiv SubMenu(this HtmlHelper helper, string id)
        {
            return SubMenu(helper, id, false);
        }
        public static MvcDiv SubMenu(this HtmlHelper helper, string id, bool disableMouseOut)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("mdiv");
            tagBuilder.MergeAttributes(new RouteValueDictionary(new { @id = id }));
            tagBuilder.Attributes.Add("onmouseover", "mcancelclosetime()");
            if (disableMouseOut == false)
            {
                tagBuilder.Attributes.Add("onmouseout", "mclosetime()");
            }
            HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
            httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            System.Text.StringBuilder sb = new StringBuilder();
            return new MvcDiv(helper.ViewContext.HttpContext.Response);
        }
        public static MvcHtmlString SubMenuLink(this HtmlHelper helper, string menuName, string subMenuId)
        {
            return Anchor(helper, menuName, "#", new { @class = "submenubg", @onmouseover = "msubopen(this,'" + subMenuId + "')", @onmouseout = "msubclosetime()" });
        }
        public static MvcDiv InnerSubMenu(this HtmlHelper helper, string id)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("mdiv");
            tagBuilder.MergeAttributes(new RouteValueDictionary(new { @id = id }));
            tagBuilder.Attributes.Add("onmouseover", "msubcancelclosetime()");
            tagBuilder.Attributes.Add("onmouseout", "msubclosetime()");
            HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
            httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            System.Text.StringBuilder sb = new StringBuilder();
            return new MvcDiv(helper.ViewContext.HttpContext.Response);
        }
        #endregion

    }
}
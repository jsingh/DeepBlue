﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Collections.Specialized;
using System.Text;

namespace DeepBlue.Helpers {
	public static class HtmlControls {

		#region Image
		public static MvcHtmlString Image(this HtmlHelper helper, string imagename) {
			TagBuilder tag = new TagBuilder("img");
			tag.Attributes.Add("src", "/Assets/images/" + imagename);
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString Image(this HtmlHelper helper, string imagename, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("img");
			tag.Attributes.Add("src", "/Assets/images/" + imagename);
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString ImageButton(this HtmlHelper helper, string imagename) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "image");
			tag.Attributes.Add("src", "/Assets/images/" + imagename);
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString ImageButton(this HtmlHelper helper, string imagename, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "image");
			tag.Attributes.Add("src", "/Assets/images/" + imagename);
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		#endregion

		#region Span
		public static MvcHtmlString Span(this HtmlHelper helper, string innerHTML) {
			TagBuilder tag = new TagBuilder("span");
			tag.InnerHtml = innerHTML;
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString Span(this HtmlHelper helper, string innerHTML, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("span");
			tag.InnerHtml = innerHTML;
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		#endregion

		#region Display
		public static MvcHtmlString Literal(this HtmlHelper helper, string value) {
			return MvcHtmlString.Create(value);
		}
 		#endregion

		#region Anchor
		public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML, string href) {
			return Anchor(helper, innerHTML, href, new { });
		}
		public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML, string href, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("a");
			if (string.IsNullOrEmpty(href) == false) {
				tag.Attributes.Add("href", href);
			}
			tag.InnerHtml = innerHTML;
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML, object htmlAttributes) {
			return Anchor(helper, innerHTML, "#", htmlAttributes);
		}
		public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML) {
			return Anchor(helper, innerHTML, "#", new { });
		}
		#endregion

		#region javascript
		public static string JavascriptInclueTag(this HtmlHelper helper, string scriptname) {
			return string.Format("<script src=\"/Assets/javascripts/{0}\" type=\"text/javascript\"></script>", scriptname);
		}
		#endregion

		#region stylesheet
		public static string StylesheetLinkTag(this HtmlHelper helper, string cssname) {
			return string.Format("<link href=\"/Assets/stylesheets/{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssname);
		}
		#endregion

		#region Tab
		public static MvcHtmlString Tab(this HtmlHelper helper, string innerHTML) {
			TagBuilder tag = new TagBuilder("div");
			tag.InnerHtml = innerHTML;
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString Tab(this HtmlHelper helper, string innerHTML, string cssClassName) {
			TagBuilder tag = new TagBuilder("div");
			tag.InnerHtml = innerHTML;
			tag.AddCssClass(cssClassName);
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString Tab(this HtmlHelper helper, string innerHTML, string cssClassName, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("div");
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			tag.InnerHtml = innerHTML;
			tag.AddCssClass(cssClassName);
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		#endregion

		#region File
		public static MvcHtmlString File(this HtmlHelper htmlHelper, string name, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "file");
			tag.Attributes.Add("name", name);
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		#endregion

		#region Menu
		public static MvcDiv TopMenu(this HtmlHelper helper, bool isSelect, string subMenuId) {
			return TopMenu(helper, isSelect, subMenuId, new { });
		}
		public static MvcDiv TopMenu(this HtmlHelper helper, bool isSelect, string subMenuId, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.AddCssClass("topmenu");
			if (isSelect) {
				tagBuilder.AddCssClass("tab-sel");
				tagBuilder.AddCssClass("current");
			}
			if (string.IsNullOrEmpty(subMenuId) == false) {
				tagBuilder.Attributes.Add("onclick", "menu.mopen(this,'" + subMenuId + "')");
				//tagBuilder.Attributes.Add("onmouseover", "menu.mopen(this,'" + subMenuId + "')");
				//tagBuilder.Attributes.Add("onmouseout", "menu.mclosetime()");
			}
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			System.Text.StringBuilder sb = new StringBuilder();
			MvcDiv menu = new MvcDiv(helper.ViewContext.HttpContext.Response);
			return menu;
		}
		public static void menu_OnDispose(HttpResponseBase httpResponse) {
			httpResponse.Write("</div><div class=\"topmenu-right\"></div>");
		}
		public static MvcHtmlString TopMenuLink(this HtmlHelper helper, string menuName) {
			return Anchor(helper, "<br/><br/>" + menuName, "");
		}
		public static MvcHtmlString TopMenuLink(this HtmlHelper helper, string menuName, string iconName) {
			return Anchor(helper, Image(helper, iconName).ToHtmlString() + "<br/>" + menuName, "");
		}
		public static MvcDiv SubMenu(this HtmlHelper helper, string id, bool visible) {
			return SubMenu(helper, id, false, visible);
		}
		public static MvcDiv SubMenu(this HtmlHelper helper, string id, bool disableMouseOut, bool visible) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.AddCssClass("mdiv");
			tagBuilder.MergeAttributes(new RouteValueDictionary(new { @id = id }));
			tagBuilder.Attributes.Add("onmouseover", "menu.mcancelclosetime()");
			if (disableMouseOut == false) {
				tagBuilder.Attributes.Add("onmouseout", "menu.mclosetime()");
			}
			if (visible) {
				tagBuilder.AddCssClass("sub-select");
				tagBuilder.AddCssClass("current");
			}
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			System.Text.StringBuilder sb = new StringBuilder();
			return new MvcDiv(helper.ViewContext.HttpContext.Response);
		}
		public static MvcHtmlString SubMenuLink(this HtmlHelper helper, string menuName, string subMenuId, bool select) {
			return Anchor(helper, menuName, "#", new { @class = "submenubg" + (select == true ? " sel" : ""), @onmouseover = "menu.msubopen(this,'" + subMenuId + "')" });
		}
		public static MvcDiv InnerSubMenu(this HtmlHelper helper, string id, bool visible) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.AddCssClass("subul");
			tagBuilder.MergeAttributes(new RouteValueDictionary(new { @id = id }));
			tagBuilder.Attributes.Add("onmouseover", "menu.msubcancelclosetime()");
			//tagBuilder.Attributes.Add("onmouseout", "menu.msubclosetime()");
			if (visible) {
				tagBuilder.AddCssClass("innersub-select");
				tagBuilder.AddCssClass("current");
			}
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			System.Text.StringBuilder sb = new StringBuilder();
			return new MvcDiv(helper.ViewContext.HttpContext.Response);
		}
		#endregion

		#region Div
		public static MvcDiv Div(this HtmlHelper helper, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			MvcDiv div = new MvcDiv(helper.ViewContext.HttpContext.Response);
			return div;
		}
		public static MvcDiv Div(this HtmlHelper helper) {
			return Div(helper, new { });
		}
		#endregion

		#region Form
		public static MvcForm Form(this HtmlHelper helper, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("form");
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			MvcForm form = new MvcForm(helper.ViewContext.HttpContext.Response);
			return form;
		}
		#endregion

		#region CheckBox
		public static MvcHtmlString InputCheckBox(this HtmlHelper helper, string name, bool isChecked) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "checkbox");
			tag.Attributes.Add("name", name);
			if (isChecked) tag.Attributes.Add("checked", "");
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString InputCheckBox(this HtmlHelper helper, string name,  bool isChecked, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "checkbox");
			tag.Attributes.Add("name", name);
			if (isChecked) tag.Attributes.Add("checked", "");
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString InputCheckBox(this HtmlHelper helper, string name, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "checkbox");
			tag.Attributes.Add("name", name);
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		#endregion
	}
}
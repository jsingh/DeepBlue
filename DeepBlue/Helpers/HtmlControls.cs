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

		#region Url
		public static string Url(this HtmlHelper helper) {
			return new UrlHelper(helper.ViewContext.RequestContext).Content("~/");
		}
		public static string Url(this HtmlHelper helper, string content) {
			if (content != null) {
				if (content.StartsWith("~/") == false) {
					content = "~" + (content.StartsWith("/") == false ? "/" + content : content);
				}
			}
			return new UrlHelper(helper.ViewContext.RequestContext).Content(content);
		}
		public static string Url(this HtmlHelper helper, string actionName, string controllerName) {
			return new UrlHelper(helper.ViewContext.RequestContext).Action(actionName, controllerName);
		}
		#endregion

		#region Lable
		public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
			return LabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
		}
		public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
			string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
			string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
			if (String.IsNullOrEmpty(labelText)) {
				return MvcHtmlString.Empty;
			}
			TagBuilder tag = new TagBuilder("label");
			tag.MergeAttributes(htmlAttributes);
			tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
			tag.SetInnerText(labelText);
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		#endregion

		#region TextBox

		public static MvcHtmlString NumericTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
			TagBuilder tag = new TagBuilder("input");
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			string value = string.Empty;
			if (metadata.Model != null) {
				value = String.Format("{0:0.##;-0.##;\\}", (decimal)metadata.Model);
			}
			tag.Attributes.Add("type", "text");
			tag.Attributes.Add("value", value);
			if (tag.Attributes.Keys.Contains("id") == false) {
				tag.Attributes.Add("id", metadata.PropertyName);
			}
			tag.Attributes.Add("name", metadata.PropertyName);
			tag.Attributes.Add("for", htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString DeepBlueTextBox(this HtmlHelper htmlHelper, string name) {
			return DeepBlueTextBox(htmlHelper, name, string.Empty);
		}

		public static MvcHtmlString DeepBlueTextBox(this HtmlHelper htmlHelper, string name, object value) {
			return DeepBlueTextBox(htmlHelper, name, value, new { });
		}

		public static MvcHtmlString DeepBlueTextBox(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "text");
			string textBoxName = name;
			StringBuilder html = new StringBuilder();
			if (htmlAttributes.ContainsKey("onkeydown")) {
				string isCurrency = Convert.ToString((from v in htmlAttributes where v.Key == "onkeydown" select v.Value).FirstOrDefault());
				if (isCurrency.Contains("jHelper.isCurrency")) {
					string key = new Guid().ToString();
					textBoxName = "amount_" + name;
					html.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{1}\" value=\"{2}\" />", key, name, value);
				}
			}
			tag.Attributes.Add("name", textBoxName);
			tag.MergeAttributes(htmlAttributes);
			html.Append(tag.ToString(TagRenderMode.Normal));
			return MvcHtmlString.Create(html.ToString());
		}

		public static MvcHtmlString DeepBlueTextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes) {
			return DeepBlueTextBox(htmlHelper, name, value, new RouteValueDictionary(htmlAttributes));
		}

		public static MvcHtmlString DeepBlueTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
			TagBuilder tag = new TagBuilder("input");
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			string value = string.Empty;
			if (metadata.Model != null) {
				value = String.Format("{0:0.##;-0.##;\\}", (decimal)metadata.Model);
			}
			tag.Attributes.Add("type", "text");
			tag.Attributes.Add("value", value);
			if (tag.Attributes.Keys.Contains("id") == false) {
				tag.Attributes.Add("id", metadata.PropertyName);
			}
			tag.Attributes.Add("name", metadata.PropertyName);
			tag.Attributes.Add("for", htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		#endregion

		#region Image
		public static MvcHtmlString Image(this HtmlHelper helper, string imagename) {
			TagBuilder tag = new TagBuilder("img");
			tag.Attributes.Add("src", Url(helper, string.Format("/Assets/images/{0}", imagename)));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString Image(this HtmlHelper helper, string imagename, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("img");
			tag.Attributes.Add("src", Url(helper, string.Format("/Assets/images/{0}", imagename)));
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString ImageButton(this HtmlHelper helper, string imagename) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "image");
			tag.Attributes.Add("src", Url(helper, string.Format("/Assets/images/{0}", imagename)));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString ImageButton(this HtmlHelper helper, string imagename, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "image");
			tag.Attributes.Add("src", Url(helper, string.Format("/Assets/images/{0}", imagename)));
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		#endregion

		#region Image
		public static MvcHtmlString Button(this HtmlHelper helper, string value, IDictionary<string, object> htmlAttributes) {
			TagBuilder tag = new TagBuilder("button");
			tag.MergeAttributes(htmlAttributes);
			return MvcHtmlString.Create(string.Format("{0}{1}{2}", tag.ToString(TagRenderMode.StartTag), value, "</button>"));
		}
		public static MvcHtmlString Button(this HtmlHelper helper, string value, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("button");
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(string.Format("{0}{1}{2}", tag.ToString(TagRenderMode.StartTag), value, "</button>"));
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
				if (href.StartsWith("javascript") == false && href != "#") {
					href = Url(helper, href);
				}
				tag.Attributes.Add("href", href);
			}
			tag.InnerHtml = innerHTML;
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML, string href, IDictionary<string, object> htmlAttributes) {
			TagBuilder tag = new TagBuilder("a");
			if (string.IsNullOrEmpty(href) == false) {
				if (href.StartsWith("javascript") == false && href != "#") {
					href = Url(helper, href);
				}
				tag.Attributes.Add("href", href);
			}
			tag.InnerHtml = innerHTML;
			tag.MergeAttributes(htmlAttributes);
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
			return string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>",
				Url(helper, string.Format("~/Assets/javascripts/{0}", scriptname)));
		}
		#endregion

		#region stylesheet
		public static string StylesheetLinkTag(this HtmlHelper helper, string cssname) {
			return string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />",
				Url(helper, string.Format("~/Assets/stylesheets/{0}", cssname)));
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
		public static MvcDiv TopMenu(this HtmlHelper helper, bool isSelect, string subMenuID) {
			return TopMenu(helper, isSelect, subMenuID, new { });
		}
		public static MvcDiv TopMenu(this HtmlHelper helper, bool isSelect, string subMenuID, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			tagBuilder.AddCssClass("topmenu");
			if (isSelect) {
				tagBuilder.AddCssClass("tab-sel");
				tagBuilder.AddCssClass("current");
			}
			if (string.IsNullOrEmpty(subMenuID) == false) {
				tagBuilder.Attributes.Add("onclick", "menu.mopen(this,'" + subMenuID + "')");
				//tagBuilder.Attributes.Add("onmouseover", "menu.mopen(this,'" + subMenuID + "')");
				//tagBuilder.Attributes.Add("onmouseout", "menu.mclosetime()");
			}
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
			return Div(helper, menuName, new { @class = "mnu-name" });
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
		public static MvcHtmlString SubMenuLink(this HtmlHelper helper, string menuName, string subMenuID, bool select) {
			return Anchor(helper, menuName, "#", new { @class = "submenubg" + (select == true ? " sel" : ""), @onmouseover = "menu.msubopen(this,'" + subMenuID + "')" });
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
			return new MvcDiv(helper.ViewContext.HttpContext.Response);
		}

		#endregion

		#region LeftMenu
		public static MvcDiv LeftMenu(this HtmlHelper helper, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.Attributes.Add("id", "leftmenu");
			if (htmlAttributes != null) {
				tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			}
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			return new MvcDiv(helper.ViewContext.HttpContext.Response);
		}
		public static MvcDiv LeftMenu(this HtmlHelper helper) {
			return LeftMenu(helper, null);
		}
		#endregion

		#region Div
		public static MvcHtmlString Div(this HtmlHelper helper, string innerHTML, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("div");
			tag.InnerHtml = innerHTML;
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
		public static MvcDiv Div(this HtmlHelper helper, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			MvcDiv div = new MvcDiv(helper.ViewContext.HttpContext.Response);
			return div;
		}
		public static MvcDiv Div(this HtmlHelper helper, IDictionary<string, object> htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(htmlAttributes);
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			MvcDiv div = new MvcDiv(helper.ViewContext.HttpContext.Response);
			return div;
		}
		public static MvcDiv Div(this HtmlHelper helper, RouteValueDictionary htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(htmlAttributes);
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			MvcDiv div = new MvcDiv(helper.ViewContext.HttpContext.Response);
			return div;
		}
		public static MvcDiv Div(this HtmlHelper helper) {
			return Div(helper, new { });
		}
		#endregion

		#region Tab
		public static MvcTab Tab(this HtmlHelper helper, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			tagBuilder.AddCssClass("section-tab");
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag) + "<div class=left></div><div class=center>");
			return new MvcTab(helper.ViewContext.HttpContext.Response);
		}
		public static MvcTab Tab(this HtmlHelper helper) {
			return Tab(helper, new { });
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

		public static MvcHtmlString InputCheckBox(this HtmlHelper helper, string name, bool isChecked, object htmlAttributes) {
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

		#region GreenButton
		public static MvcHtmlString DeepBlueButton(this HtmlHelper helper, string name, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			tagBuilder.AddCssClass("db-btn");
			return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.StartTag) + string.Format("<div class=left></div><div class=center>{0}</div><div class=right></div>", name) + tagBuilder.ToString(TagRenderMode.EndTag));
		}
		public static MvcHtmlString DeepBlueButton(this HtmlHelper helper, string name) {
			return DeepBlueButton(helper, name, new { });
		}
		#endregion

		#region GreenButton
		public static MvcButton GreenButton(this HtmlHelper helper, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			tagBuilder.AddCssClass("green-btn");
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag) + "<div class=left></div><div class=center>");
			return new MvcButton(helper.ViewContext.HttpContext.Response);
		}
		public static MvcButton GreenButton(this HtmlHelper helper) {
			return GreenButton(helper, new { });
		}
		#endregion

		#region BlueButton
		public static MvcButton BlueButton(this HtmlHelper helper, object htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			tagBuilder.AddCssClass("green-btn blue-bg");
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag) + "<div class=left></div><div class=center>");
			return new MvcButton(helper.ViewContext.HttpContext.Response);
		}
		public static MvcButton BlueButton(this HtmlHelper helper) {
			return BlueButton(helper, new { });
		}
		#endregion

		#region javascipt

		public static JavaScript JavaScript(this HtmlHelper helper) {
			TagBuilder tagBuilder = new TagBuilder("script");
			tagBuilder.Attributes.Add("type", "text/javascript");
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			return new JavaScript(helper.ViewContext.HttpContext.Response);
		}
		#endregion

		#region JQueryTemplate Controls

		public static JQueryTemplateScript jQueryTemplateScript(this HtmlHelper helper, string id) {
			TagBuilder tagBuilder = new TagBuilder("script");
			tagBuilder.Attributes.Add("id", id);
			tagBuilder.Attributes.Add("type", "text/x-jquery-tmpl");
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			return new JQueryTemplateScript(helper.ViewContext.HttpContext.Response);
		}

		public static string jQueryTemplateTextArea(this HtmlHelper htmlHelper, string name, string value, int rows, int cols, object htmlAttributes) {
			RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
			StringBuilder attributes = new StringBuilder();
			foreach (var attribute in dictionary) {
				attributes.AppendFormat(" {0}=\"{1}\"", attribute.Key, attribute.Value);
			}
			return string.Format("<textarea id=\"{0}\" name=\"{0}\" rows=\"{1}\" cols=\"{2}\" {3}>{4}</textarea>", name, rows, cols, attributes.ToString(), value);
		}

		public static string jQueryTemplateDisplayFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, bool isjqueryTemplateDisplay) {
			return jQueryTemplateDisplayFor(htmlHelper, expression, isjqueryTemplateDisplay, string.Empty);
		}

		public static string jQueryTemplateDisplayFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, bool isjqueryTemplateDisplay, string formatName) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			string displayData = string.Empty;
			if (isjqueryTemplateDisplay) {
				string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
				string dataType = metadata.ModelType.FullName;
				if (String.IsNullOrEmpty(htmlFieldName) == false) {
					displayData = "${" + (string.IsNullOrEmpty(formatName) ? htmlFieldName : formatName + "(" + htmlFieldName + ")") + "}";
				}
			}
			else {
				if (metadata.Model != null) {
					displayData = metadata.Model.ToString();
				}
			}
			return displayData;
		}

		public static MvcHtmlString jQueryTemplateSpan(this HtmlHelper htmlHelper, string name, string className) {
			return MvcHtmlString.Create(string.Format("<span id=\"{0}\" name=\"{0}\" class=\"{1}\">{2}</span>", name, className, (name.Contains("$") ? name : "${" + name + "}")));
		}

		public static MvcHtmlString jQueryTemplateTextBox(this HtmlHelper htmlHelper, string name) {
			return jQueryTemplateTextBox(htmlHelper, name, "${" + name + "}", new { });
		}

		public static MvcHtmlString jQueryTemplateTextBox(this HtmlHelper htmlHelper, string name, object htmlAttributes) {
			return jQueryTemplateTextBox(htmlHelper, name, "${" + name + "}", htmlAttributes);
		}

		public static MvcHtmlString jQueryTemplateTextBox(this HtmlHelper htmlHelper, string name, string value) {
			return jQueryTemplateTextBox(htmlHelper, name, value, new { });
		}

		public static string jQueryTemplateTextBoxExpression(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes) {
			RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
			StringBuilder attributes = new StringBuilder();
			foreach (var attribute in dictionary) {
				attributes.AppendFormat(" {0}=\"{1}\"", attribute.Key, attribute.Value);
			}
			return string.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" value=\"{1}\" {2} />", name, value, attributes.ToString());
		}

		public static string jQueryTemplateTextBoxExpression(this HtmlHelper htmlHelper, string name, string value, string className) {
			return string.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" value=\"{1}\" class=\"{2}\" />", name, value, className);
		}

		public static string jQueryTemplateHiddenExpression(this HtmlHelper htmlHelper, string name, string value) {
			return string.Format("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\" />", name, value);
		}

		public static MvcHtmlString jQueryTemplateTextBox(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "text");
			tag.Attributes.Add("value", value);
			tag.Attributes.Add("name", name);
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			if (tag.Attributes.Keys.Contains("id") == false) {
				tag.Attributes.Add("id", name);
			}
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString jQueryTemplateHidden(this HtmlHelper htmlHelper, string name) {
			return jQueryTemplateHidden(htmlHelper, name, "${" + name + "}", new { });
		}

		public static MvcHtmlString jQueryTemplateHidden(this HtmlHelper htmlHelper, string name, string value) {
			return jQueryTemplateHidden(htmlHelper, name, value, new { });
		}

		public static MvcHtmlString jQueryTemplateHidden(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "hidden");
			tag.Attributes.Add("value", value);
			tag.Attributes.Add("name", name);
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			if (tag.Attributes.Keys.Contains("id") == false) {
				tag.Attributes.Add("id", name);
			}
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString jQueryTemplateHiddenFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) {
			return jQueryTemplateHiddenFor(htmlHelper, expression, new { });
		}

		public static MvcHtmlString jQueryTemplateHiddenFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
			if (String.IsNullOrEmpty(htmlFieldName)) {
				return MvcHtmlString.Empty;
			}
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "hidden");
			tag.Attributes.Add("value", "${" + htmlFieldName + "}");
			tag.Attributes.Add("name", htmlFieldName);
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			if (tag.Attributes.Keys.Contains("id") == false) {
				tag.Attributes.Add("id", htmlFieldName);
			}
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString jQueryTemplateTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) {
			return jQueryTemplateTextBoxFor(htmlHelper, expression, new { }, string.Empty);
		}

		public static MvcHtmlString jQueryTemplateTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
			return jQueryTemplateTextBoxFor(htmlHelper, expression, htmlAttributes, string.Empty);
		}

		public static MvcHtmlString jQueryTemplateTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string formatName) {
			if (string.IsNullOrEmpty(formatName)) {
				IDictionary<string, object> dictionaries = new RouteValueDictionary(htmlAttributes);
				if (dictionaries.ContainsKey("onkeydown")) {
					int? count = (from dic in dictionaries
								  where
									  (
									  dic.Value.ToString().Contains("jHelper.isCurrency") == true ||
									  dic.Value.ToString().Contains("jHelper.isNumeric") == true
									  )
								  select dic).Count();
					if ((count ?? 0) > 0) {
						formatName = "formatNumber";
					}
				}
			}
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
			if (String.IsNullOrEmpty(htmlFieldName)) {
				return MvcHtmlString.Empty;
			}
			TagBuilder tag = new TagBuilder("input");
			tag.Attributes.Add("type", "text");
			if (string.IsNullOrEmpty(formatName) == false) {
				tag.Attributes.Add("value", "${" + formatName + "(" + htmlFieldName + ")" + "}");
			}
			else {
				tag.Attributes.Add("value", "${" + htmlFieldName + "}");
			}
			tag.Attributes.Add("name", htmlFieldName);
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			if (tag.Attributes.Keys.Contains("id") == false) {
				tag.Attributes.Add("id", htmlFieldName);
			}
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}

		#endregion

		#region UnorderList
		public static MvcControl UnorderList(this HtmlHelper helper) {
			return UnorderList(helper, new { });
		}
		public static MvcControl UnorderList(this HtmlHelper helper, object htmlAttributes) {
			return UnorderList(helper, new RouteValueDictionary(htmlAttributes));
		}
		public static MvcControl UnorderList(this HtmlHelper helper, RouteValueDictionary htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("ul");
			tagBuilder.MergeAttributes(htmlAttributes);
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			MvcControl ul = new MvcControl(helper.ViewContext.HttpContext.Response, "ul");
			return ul;
		}
		#endregion

		#region OrderList
		public static MvcControl OrderList(this HtmlHelper helper) {
			return OrderList(helper, new { });
		}
		public static MvcControl OrderList(this HtmlHelper helper, object htmlAttributes) {
			return OrderList(helper, new RouteValueDictionary(htmlAttributes));
		}
		public static MvcControl OrderList(this HtmlHelper helper, RouteValueDictionary htmlAttributes) {
			TagBuilder tagBuilder = new TagBuilder("li");
			tagBuilder.MergeAttributes(htmlAttributes);
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			MvcControl ul = new MvcControl(helper.ViewContext.HttpContext.Response, "li");
			return ul;
		}
		#endregion

		#region Table

		public static MvcHtmlString Table(this HtmlHelper helper, TableOptions tableOption) {
			TagBuilder tagBuilder = new TagBuilder("table");
			tagBuilder.Attributes.Add("cellpadding", tableOption.CellPadding.ToString());
			tagBuilder.Attributes.Add("cellspacing", tableOption.CellSpacing.ToString());
			if (tableOption.Border > 0) {
				tagBuilder.Attributes.Add("border", tableOption.Border.ToString());
			}
			if (string.IsNullOrEmpty(tableOption.Name) == false) {
				tagBuilder.Attributes.Add("name", tableOption.Name);
			}
			if (string.IsNullOrEmpty(tableOption.ID) == false) {
				tagBuilder.Attributes.Add("id", tableOption.ID);
			}
			if (string.IsNullOrEmpty(tableOption.Width)) {
				tagBuilder.Attributes.Add("width", tableOption.Width);
			}
			tagBuilder.MergeAttributes(new RouteValueDictionary(tableOption.HtmlAttributes));
			StringBuilder columnHTML = new StringBuilder();
			columnHTML.Append("<thead><tr>");
			foreach (var column in tableOption.Columns) {
				TagBuilder th = new TagBuilder("th");
				if (string.IsNullOrEmpty(column.Name) == false) {
					th.Attributes.Add("name", column.Name);
				}
				if (string.IsNullOrEmpty(column.ID) == false) {
					th.Attributes.Add("id", column.ID);
				}
				if (string.IsNullOrEmpty(column.SortName) == false) {
					th.Attributes.Add("sortname", column.SortName);
				}
				if (string.IsNullOrEmpty(column.InnerHtml) == false) {
					th.InnerHtml = column.InnerHtml;
				}
				th.MergeAttributes(new RouteValueDictionary(column.HtmlAttributes));
				columnHTML.Append(th.ToString());
			}
			columnHTML.Append("</tr></thead>");
			tagBuilder.InnerHtml = columnHTML.ToString();
			return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
		}

		public static MvcControl TableRow(this HtmlHelper helper, object htmlAttributes) {
			StringBuilder row = new StringBuilder();
			StringBuilder attributes = new StringBuilder();
			RouteValueDictionary values = new RouteValueDictionary(htmlAttributes);
			foreach (var value in values) {
				if (value.Key == "if") {
					attributes.Append(" " + value.Value);
				}
				else {
					attributes.AppendFormat(" {0}=\"{1}\"", value.Key, value.Value);
				}
			}
			row.AppendFormat("<tr {0}>", attributes.ToString());
			HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
			httpResponse.Write(row.ToString());
			return new MvcControl(helper.ViewContext.HttpContext.Response, "tr");
		}

		public static MvcHtmlString TableRow(this HtmlHelper helper, object htmlAttributes, List<TableColumnOptions> columns) {
			StringBuilder row = new StringBuilder();
			StringBuilder attributes = new StringBuilder();
			RouteValueDictionary values = new RouteValueDictionary(htmlAttributes);
			foreach (var value in values) {
				if (value.Key == "if") {
					attributes.Append(" " + value.Value);
				}
				else {
					attributes.AppendFormat(" {0}=\"{1}\"", value.Key, value.Value);
				}
			}
			row.AppendFormat("<tr {0}>{1}</tr>", attributes.ToString(), TableCell(helper, columns));
			return MvcHtmlString.Create(row.ToString());
		}

		public static MvcHtmlString TableCell(this HtmlHelper helper, List<TableColumnOptions> columns) {
			StringBuilder row = new StringBuilder();
			TagBuilder td = null;
			foreach (var column in columns) {
				td = new TagBuilder("td");
				if (string.IsNullOrEmpty(column.Name) == false) {
					td.Attributes.Add("name", column.Name);
				}
				if (string.IsNullOrEmpty(column.ID) == false) {
					td.Attributes.Add("id", column.ID);
				}
				if (string.IsNullOrEmpty(column.SortName) == false) {
					td.Attributes.Add("sortname", column.SortName);
				}
				if (string.IsNullOrEmpty(column.InnerHtml) == false) {
					td.InnerHtml = column.InnerHtml;
				}
				td.MergeAttributes(new RouteValueDictionary(column.HtmlAttributes));
				row.Append(td.ToString());
			}
			return MvcHtmlString.Create(row.ToString());
		}

		#endregion

	}
}
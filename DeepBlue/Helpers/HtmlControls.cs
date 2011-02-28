using System;
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
		 

		#region Anchor
		public static MvcHtmlString Anchor(this HtmlHelper helper, string innerHTML, string href, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("a");
			tag.Attributes.Add("href", href);
			tag.InnerHtml = innerHTML;
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
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
			
	}
}
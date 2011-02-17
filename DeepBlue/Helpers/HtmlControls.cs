using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;

namespace DeepBlue.Helpers {
	public static class HtmlControls {
		public static string ImageTag(this HtmlHelper helper, string imagename) {
			return string.Format("<img src=\"/Assets/images/{0}\" />", imagename);
		}
		public static MvcHtmlString ImageTag(this HtmlHelper helper, string imagename, object htmlAttributes) {
			TagBuilder tag = new TagBuilder("img");
			tag.Attributes.Add("src", "/Assets/images/" + imagename);
			tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
		}
	}
}
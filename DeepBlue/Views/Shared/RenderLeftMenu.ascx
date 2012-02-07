<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DeepBlue.Models.Admin.MenuModel>>" %>
<%@ Import Namespace="DeepBlue.Models.Admin" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% if (Model.Count() > 0) {%>
<div class="menubox">
	<ul>
		<%foreach (MenuModel menu in Model) {%>
		<li class="<%=(ViewData["PageName"] == menu.Name ? "sel" : "")%>">
			<%if ((string.IsNullOrEmpty(menu.ActionName) && (string.IsNullOrEmpty(menu.ControllerName)))) {%>
			<%: Html.Anchor(menu.DisplayName,"#", menu.HtmlAttributes)%>
			<%}
	 else {%>
			<%: Html.ActionLink(menu.DisplayName, menu.ActionName, menu.ControllerName, menu.RouteValues, menu.HtmlAttributes)%>
			<%}%>
		</li>
		<%}%>
	</ul>
</div>
<%}%>

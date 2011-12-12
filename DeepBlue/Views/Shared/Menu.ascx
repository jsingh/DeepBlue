<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DeepBlue.Models.Admin.MenuModel>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% 
	List<DeepBlue.Models.Admin.MenuModel> topMenus = (from menu in Model
													  where menu.IsTopMenu == true
													  select menu).ToList();
%>
<%using (Html.Div(new { @id = "menubox", @class = "topmenu-item" })) {%>
<%
	  StringBuilder subMenus = new StringBuilder();
	  string className = string.Empty;
	  foreach (DeepBlue.Models.Admin.MenuModel menu in topMenus) {
		  className = "topmenu";
		  if (menu.Name == ViewData["MenuName"]) {
			  className += " current tab-sel";
		  }
		  var dic = new RouteValueDictionary(menu.HtmlAttributes);
		  dic.Add("class", className);
		  if (menu.Childs.Count() > 0) {
			  dic.Add("onclick", "menu.mopen(this,'" + menu.Name + "')");
		  }
%>
<%using (Html.Div(dic)) {%><%:Html.Div(menu.DisplayName, new { @class = "mnu-name" })%><%}%><%:Html.Div("&nbsp;", new { @class = "sep" })%><%}%><%}%>
<%using (Html.Div(new { @id = "submenu", @style = "display:none" })) {%><%using (Html.Div(new { @id = "submenubox" })) {%>
<%  foreach (DeepBlue.Models.Admin.MenuModel menu in topMenus) {
		string parentName = menu.Name;
		string className = "mdiv";
		if (menu.Name == ViewData["MenuName"]) {
			className += " current sub-select";
		}
%>
<%using (Html.Div(new { @id = parentName, @class = className })) {%><%using (Html.UnorderList()) {%>
<%
																		  foreach (DeepBlue.Models.Admin.MenuModel childMenu in menu.Childs) {
%>
<%if ((childMenu.IsAdmin && AdminAuthorizeHelper.IsAdmin) || childMenu.IsAdmin == false) {
	  var dic = new RouteValueDictionary(childMenu.HtmlAttributes);
	  dic.Add("class", (ViewData["SubmenuName"] == childMenu.Name ? "sel" : ""));
%>
<%using (Html.OrderList()) {%><%: Html.ActionLink(childMenu.DisplayName, childMenu.ActionName, childMenu.ControllerName,new RouteValueDictionary(childMenu.RouteValues), dic)%><%}%>
<%}%>
<%}%>
<%}%>
<%}%>
<%}%>
<%}%>
<%}%>

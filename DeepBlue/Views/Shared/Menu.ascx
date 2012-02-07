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
		  if (ViewData["MenuName"] != null) {
			  if (menu.Name == ViewData["MenuName"].ToString()) {
				  className += " current tab-sel";
			  }
		  }
		  IDictionary<string, object> dic = menu.HtmlAttributes;
		  if (dic.Keys.Contains("class") == false) {
			  dic.Add("class", className);
		  }
		  else {
			  dic["class"] = className;
		  }
		  if (menu.Childs.Count() > 0) {
			  if (dic.Keys.Contains("onclick") == false) {
				  dic.Add("onclick", "menu.mopen(this,'" + menu.Name + "')");
			  }
			  else {
				  dic["onclick"] = "menu.mopen(this,'" + menu.Name + "')";
			  }
		  }
%>
<%using (Html.Div(dic)) {%><%:Html.Div(menu.DisplayName, new { @class = "mnu-name" })%><%}%><%:Html.Div("&nbsp;", new { @class = "sep" })%><%}%><%}%>
<%using (Html.Div(new { @id = "submenu", @style = "display:none" })) {%><%using (Html.Div(new { @id = "submenubox" })) {%>
<%  foreach (DeepBlue.Models.Admin.MenuModel menu in topMenus) {
		string parentName = menu.Name;
		string className = "mdiv";
		if (ViewData["MenuName"] != null) {
			if (menu.Name == ViewData["MenuName"].ToString()) {
				className += " current sub-select";
			}
		}
%>
<%using (Html.Div(new { @id = parentName, @class = className })) {%><%using (Html.UnorderList()) {%>
<%foreach (DeepBlue.Models.Admin.MenuModel submenu in menu.Childs) {
	  DeepBlue.Models.Admin.MenuModel childMenu = submenu.Childs.FirstOrDefault();
	  if (childMenu == null) {
		  childMenu = submenu;
	  }
%>
<%if (childMenu != null) {
	  IDictionary<string, object> dic = submenu.HtmlAttributes;
	  string clsname = string.Empty;
	  if (ViewData["SubmenuName"] != null) {
		  clsname = (ViewData["SubmenuName"].ToString() == submenu.Name ? "sel" : "");
	  }
	  if (dic.Keys.Contains("class") == false) {
		  dic.Add("class", clsname);
	  }
	  else {
		  dic["class"] = clsname;
	  }
%>
<%using (Html.OrderList()) {%>
<%if ((string.IsNullOrEmpty(childMenu.ActionName) && (string.IsNullOrEmpty(childMenu.ControllerName)))) {%>
<%: Html.Anchor(submenu.DisplayName, "#", submenu.HtmlAttributes)%>
<%}
  else {%>
<%: Html.ActionLink(submenu.DisplayName, childMenu.ActionName, childMenu.ControllerName, submenu.RouteValues, dic)%>
<%}%>
<%}%>
<%}%>
<%}%>
<%}%>
<%}%>
<%}%>
<%}%>
<%}%>

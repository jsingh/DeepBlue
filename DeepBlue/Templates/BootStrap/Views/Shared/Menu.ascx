<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DeepBlue.Models.Admin.EntityMenuModel>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%List<DeepBlue.Models.Admin.EntityMenuModel> _Menus = MenuHelper.GetMenus();%>
<%if (Model.Count() > 0) {%>
<ul class="dropdown-menu">
	<%foreach (var menu in Model) {
	   string url = menu.URL;
	   if (url.Contains("javascript") == false) {
		   if (menu.URL.Contains("?")) {
			   url += "&menuid=" + menu.MenuID;
		   }
		   else {
			   url += "?menuid=" + menu.MenuID;
		   }
	   }
	%>
	<% var childMenus = (from m in _Menus
					  where m.ParentMenuID == menu.MenuID
					  select m).ToList();
	%>
	<li>
		<%:Html.Anchor(menu.DisplayName, url)%>
		<%}%>
	</li>
</ul>
<%}%>

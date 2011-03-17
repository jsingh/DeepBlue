<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Module
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("Module.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:module.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Module</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="ModuleList">
				<thead>
					<tr>
						<th sortname="ModuleID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="ModuleName">
							Module Name
						</th>
						<th align="center" style="width: 5%;">
						</th>
					</tr>
				</thead>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("ModuleList", new FlexigridOptions { ActionName = "ModuleList", ControllerName = "Admin", HttpMethod = "GET", SortName = "ModuleID", Paging = true })%>
</asp:Content>

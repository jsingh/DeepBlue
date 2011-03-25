<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Custom Field
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("CustomField.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:customField.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Custom Field</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="CustomFieldList">
				<thead>
					<tr>
						<th sortname="CustomFieldID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="CustomFieldText" style="width: 50%">
							Custom Field
						</th>
						<th sortname="ModuleName" style="width: 20%;">
							Module Name
						</th>
						<th sortname="DataTypeName" style="width: 15%;">
							Data Type
						</th>
						<th sortname="Search" style="width: 5%;" align="center">
							Search
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
	<%=Html.jQueryFlexiGrid("CustomFieldList", new FlexigridOptions { ActionName = "CustomFieldList", ControllerName = "Admin", HttpMethod = "GET", SortName = "CustomFieldID", Paging = true })%>
</asp:Content>

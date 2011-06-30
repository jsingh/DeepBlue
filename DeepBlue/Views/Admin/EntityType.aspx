<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Entity Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("InvestorEntityType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:invEntityType.add(0);" style="font-weight:bold;">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add EntityType</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="InvEntityTypeList">
				<thead>
					<tr>
						<th sortname="InvestorEntityTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="InvestorEntityTypeName" style="width: 80%">
							EntityType
						</th>
						<th datatype="Boolean" sortname="Enabled" align="center" style="width: 10%;">
							Enable
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
	<%=Html.jQueryFlexiGrid("InvEntityTypeList", new FlexigridOptions { 
    ActionName = "EntityTypeList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "InvestorEntityTypeName", Paging = true 
	, OnSuccess= "invEntityType.onGridSuccess"
	, OnRowClick = "invEntityType.onRowClick"
})%>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("CommunicationType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:communicationType.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Communication Type</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="CommunicationTypeList">
				<thead>
					<tr>
						<th sortname="CommunicationTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="CommunicationTypeName" style="width: 40%">
							Communication Type
						</th>
						<th sortname="CommunicationGroupingName" style="width: 40%">
							Communication Group
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
	<%=Html.jQueryFlexiGrid("CommunicationTypeList", new FlexigridOptions { ActionName = "CommunicationTypeList", ControllerName = "Admin"
	, HttpMethod = "GET"
	, SortName = "CommunicationTypeName"
	, Paging = true
	,
	OnSuccess = "communicationType.onGridSuccess"
	,
	OnRowClick = "communicationType.onRowClick"
})%>
</asp:Content>

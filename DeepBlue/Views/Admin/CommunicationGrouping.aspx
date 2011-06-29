<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Communication Grouping
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("CommunicationGrouping.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:communicationGrouping.add(0);" style="font-weight:bold;">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Communication Grouping</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="CommunicationGroupingList">
				<thead>
					<tr>
						<th sortname="CommunicationGroupingID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="CommunicationGroupingName" style="width: 40%">
							Communication Grouping
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
	<%=Html.jQueryFlexiGrid("CommunicationGroupingList", new FlexigridOptions { ActionName = "CommunicationGroupingList", ControllerName = "Admin"
	, HttpMethod = "GET"
	, SortName = "CommunicationGroupingName"
	, Paging = true
	,
	OnSuccess = "communicationGrouping.onGridSuccess"
	,
	OnRowClick = "communicationGrouping.onRowClick"
})%>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FundClosing.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:fundClosing.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Fund Closing</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="FundClosingList">
				<thead>
					<tr>
						<th sortname="FundClosingID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="FundClosingDate" style="width: 15%;">
							Closing Date
						</th>
						<th sortname="Name" style="width: 25%;">
							Name
						</th>
						<th sortname="FundName" style="width: 35%">
							Fund Name
						</th>
						<th datatype="Boolean" sortname="IsFirstClosing" align="center" style="width: 10%;">
							First Closing
						</th>
						<th align="center" style="width: 10%;">
						</th>
					</tr>
				</thead>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("FundClosingList", new FlexigridOptions { ActionName = "FundClosingList", 
	ControllerName = "Admin", HttpMethod = "GET", SortName = "FundClosingDate", SortOrder = "desc" 
	, Paging = true , OnRowBound="fundClosing.onRowBound" })%>
</asp:Content>

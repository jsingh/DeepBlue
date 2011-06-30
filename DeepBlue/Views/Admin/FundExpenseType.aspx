<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund Expense Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FundExpenseType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:fundExpenseType.add(0);" style="font-weight: bold;">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Fund Expense Type</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="FundExpenseTypeList">
				<thead>
					<tr>
						<th sortname="FundExpenseTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="Name" style="width: 40%">
							Fund Expense Type
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
	<%=Html.jQueryFlexiGrid("FundExpenseTypeList", new FlexigridOptions { ActionName = "FundExpenseTypeList", ControllerName = "Admin"
	, HttpMethod = "GET"
	, SortName = "Name"
	, Paging = true
	, OnSuccess = "fundExpenseType.onGridSuccess"
	, OnRowClick = "fundExpenseType.onRowClick"
})%>
</asp:Content>

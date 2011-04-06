<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.JavascriptInclueTag("Fund.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
	<%= Html.StylesheetLinkTag("fund.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="fund-main">
		<div class="fund-header">
			<a href="javascript:fund.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Fund </a>
		</div>
		<div class="fund-content">
			<table id="FundList" cellpadding="0" cellspacing="0" border="0">
				<thead>
					<tr>
						<th sortname="FundID" style="width: 1%;display:none;">
							Fund Id
						</th>
						<th sortname="FundName" style="width: 40%">
							Fund Name
						</th>
						<th sortname="TaxId" align="center" style="width: 20%">
							Tax ID
						</th>
						<th sortname="FundStartDate" align="center" style="width: 15%">
							Fund Start Date
						</th>
						<th sortname="ScheduleTerminationDate" align="center" style="width: 20%">
							Schedule termination Date
						</th>
						<th align="center" style="width: 5%">
						</th>
					</tr>
				</thead>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("FundList", new FlexigridOptions { ActionName = "List", ControllerName = "Fund", SortName = "FundName", Paging = true, OnRowBound = "fund.onRowBound" })%>
</asp:Content>

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
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">FUNDS</span><span class="arrow"></span><span class="pname">Fund
					Setup</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="cc-box">
		<div class="header">
			<div class="tabbg">
				<%using (Html.Tab(new { @id = "NewCCTab", @class = "select", @onclick = "javascript:capitalCall.selectTab('C',this);" })) {%>fasdf asdf asdf asdf
				<%}%>
				<%using (Html.Tab(new { @id = "ManCCTab", @onclick = "javascript:capitalCall.selectTab('M',this);" })) {%>&nbsp;
				<%}%>
			</div>
		</div>
	</div>
	<div class="cc-main" id="FundDetail" style="width: 90%; margin-left: 20px;">
		<a href="javascript:fund.add(0);">
			<%: Html.Image("add_icon.png") %>
			&nbsp;Add Fund </a>
		<table id="FundList" cellpadding="0" cellspacing="0" border="0">
			<thead>
				<tr>
					<th sortname="FundID" style="width: 1%; display: none;">
						Fund Id
					</th>
					<th sortname="FundName" style="width: 40%">
						Fund Name
					</th>
					<th sortname="TaxId" style="width: 20%">
						Tax ID
					</th>
					<th sortname="InceptionDate" style="width: 15%">
						Fund Start Date
					</th>
					<th sortname="ScheduleTerminationDate" style="width: 20%">
						Schedule termination Date
					</th>
					<th align="center" style="width: 5%">
					</th>
				</tr>
			</thead>
		</table>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("FundList", new FlexigridOptions { ActionName = "List", 
	ControllerName = "Fund", SortName = "FundName", RowOptions = new int[] { 10, 15, 20, 50, 100 }, RowsLength = 10,
	ResizeWidth = false, Paging = true, OnRowBound = "fund.onRowBound" })%>
</asp:Content>

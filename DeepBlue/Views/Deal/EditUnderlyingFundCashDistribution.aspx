<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.UnderlyingFundCashDistributionModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Underlying Fund Cash Distribution
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%=Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.JavascriptInclueTag("DealActivity.js") %>
	<%=Html.StylesheetLinkTag("dealactivity.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("CreateUnderlyingFundCashDistribution", Model); %>
	<div style="clear: both">
		<h1>
			Cash Distribution</h1>
		<table cellpadding="0" cellspacing="0" border="0" id="CashDistributionList">
			<thead>
				<tr>
					<th sortname="CashDistributionId" style="width: 15%" align="center">
						ID
					</th>
					<th sortname="DealName" style="width: 45%">
						Deal Name
					</th>
					<th sortname="Amount" style="width: 40%" datatype="money" align="right">
						Amount
					</th>
				</tr>
			</thead>
		</table>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.selectFund(ui.item.id);}"})%>
	<%= Html.jQueryAutoComplete("UnderlyingFundName", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.selectUnderlyingFund(ui.item.id);}"})%>
	<%=Html.jQueryFlexiGrid("CashDistributionList", new FlexigridOptions {
	ActionName = "CashDistributionList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, SortName = "CashDistributionId"
	, SortOrder = "asc"
	, Paging = false
	, OnSubmit = "dealActivity.onCDListSubmit"
	, OnSuccess = "jHelper.resizeIframe"
	})%>
	<%=Html.jQueryDatePicker("NoticeDate")%>
	<%=Html.jQueryDatePicker("PaidDate")%>
	<%=Html.jQueryDatePicker("ReceivedDate")%>
	<script type="text/javascript">		dealActivity.init();</script>
</asp:Content>

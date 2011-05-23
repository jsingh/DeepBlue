<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateActivityModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Activities
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("DealActivity.js")%>
	<%=Html.JavascriptInclueTag("DealActivityCapitalCall.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealactivity.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="ActivityMain">
		<div class="header">
			<%: Html.Anchor("Underlying Activities",new { @class="select", @onclick="javascript:dealActivity.tabSelect(1);" })%>|<%: Html.Anchor("Security Activities",new { @onclick="javascript:dealActivity.tabSelect(1);" })%>
		</div>
		<div class="content">
			<div class="line">
			</div>
			<div id="NewCapitalCall" class="group">
				<div class="title">
					<%: Html.Span("New Capital Call")%>
				</div>
				<div class="detail">
					<div class="search-header">
						<div class="cell">
							New Capital Call
						</div>
						<div class="cell">
							<%: Html.TextBox("CC_UnderlyingFund", "Search Underlying Fund", new { @class = "ufsearch" })%></div>
						<div class="cell">
							<%: Html.Anchor("Make New Capital Call", "javascript:dealActivity.addCC();")%>
						</div>
						<div class="cell" id="CCLoading">
						</div>
					</div>
					<div class="clear">
						<%: Html.Span("", new { @id="SpnCCUFName" })%>
					</div>
					<div class="clear">
						<table cellpadding="0" cellspacing="0" border="0" id="CapitalCallList" class="grid"
							style="width: 100%">
							<thead>
								<tr>
									<th style="width: 5%; display: none;" align="center">
										ID
									</th>
									<th style="width: 20%" align="center">
										Fund Name
									</th>
									<th style="width: 20%; display: none;" align="center">
										Underlying Fund Name
									</th>
									<th style="width: 10%" datatype="money" align="center">
										Amount
									</th>
									<th style="width: 15%" align="center">
										Notice Date
									</th>
									<th style="width: 15%" align="center">
										Received Date
									</th>
									<th style="width: 15%" align="center">
										Deemed Capital Call
									</th>
									<th style="width: 15%;" align="right">
									</th>
								</tr>
							</thead>
							<tbody>
							</tbody>
						</table>
						<br />
						<div id="AddNewCC">
						</div>
					</div>
					<%:Html.Hidden("CCUnderlyingFundId", new { @id = "CCUnderlyingFundId" })%>
				</div>
			</div>
			<div class="line">
			</div>
			<div id="NewCashDistribution" class="group">
				<div class="title">
					<%: Html.Span("New Cash Distribution")%>
				</div>
				<div class="detail">
					<div id="CashDistributions">
						<div>
							<%: Html.Image("S_CD.png", new { @class = "expandbtn" })%></div>
						<div class="fieldbox" style="display: block">
							<table cellpadding="0" cellspacing="0" border="0" id="CashDistributionList" class="grid"
								style="width: 98%">
								<thead>
									<tr>
										<th style="width: 5%" align="center">
											ID
										</th>
										<th style="width: 20%" align="center">
											Fund Name
										</th>
										<th style="width: 20%" align="center">
											Underlying Fund Name
										</th>
										<th style="width: 10%" datatype="money" align="center">
											Amount
										</th>
										<th style="width: 15%" align="center">
											Notice Date
										</th>
										<th style="width: 15%" align="center">
											Record Date
										</th>
										<th style="width: 15%;" align="center">
										</th>
									</tr>
								</thead>
								<tbody>
								</tbody>
							</table>
							<br />
							<div id="AddNewCD">
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="line">
			</div>
			<div id="UnderlyingFundValuation" class="group">
				<div class="title">
					<%: Html.Span("Underlying Fund Valuation")%>
				</div>
				<div class="detail">
				</div>
			</div>
			<div class="line">
			</div>
			<div id="UnderlyingDirectValuation" class="group">
				<div class="title">
					<%: Html.Span("Underlying Direct Valuation")%>
				</div>
				<div class="detail">
				</div>
			</div>
			<div class="line">
			</div>
			<div id="UnfundedAdjustments" class="group">
				<div class="title">
					<%: Html.Span("Unfunded Adjustments")%>
				</div>
				<div class="detail">
				</div>
			</div>
			<div class="line">
			</div>
			<div id="FundLevelExpenses" class="group">
				<div class="title">
					<%: Html.Span("Fund Level Expenses")%>
				</div>
				<div class="detail">
				</div>
			</div>
		</div>
	</div>
	<div id="EditCD">
	</div>
	<div id="EditCC">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%--<%=Html.jQueryAjaxTable("CashDistributionList", new AjaxTableOptions {
	ActionName = "UnderlyingFundCashDistributionList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, SortName = "NoticeDate"
	, SortOrder = "desc"
	, Paging = true
	, OnRowBound = "dealActivity.onRowBound"
	, OnSuccess = "dealActivity.onListSuccess"
	, AppendExistRows=true
	, RowsLength = 50
})%>
	<%=Html.jQueryAjaxTable("CapitalCallList", new AjaxTableOptions {
	ActionName = "UnderlyingFundCapitalCallList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, SortName = "NoticeDate"
	, SortOrder = "desc"
	, Paging = true
	, OnRowBound = "dealActivity.onRowBound"
	, OnSuccess = "dealActivity.onListSuccess"
	, AppendExistRows=true
	, RowsLength = 50
})%>--%>
	<%= Html.jQueryAutoComplete("CC_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setCCUnderlyingFund(ui.item.id);}"
	})%>
	<script type="text/javascript">
		var newCDData = <%= JsonSerializer.ToJsonObject(Model.UnderlyingFundCashDistributionModel)%>;
		var newCCData = <%= JsonSerializer.ToJsonObject(Model.UnderlyingFundCapitalCallModel)%>;
	</script>
	<script type="text/javascript">		dealActivity.init();</script>
	<script id="CashDistributionAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CreateUnderlyingFundCashDistribution", Model.UnderlyingFundCashDistributionModel); %>
	</script>
	<script id="CapitalCallAddTemplate" type="text/x-jquery-tmpl">
	<% Html.RenderPartial("CreateUnderlyingFundCapitalCall", Model.UnderlyingFundCapitalCallModel); %>
	</script>
</asp:Content>

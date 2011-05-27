<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateActivityModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Activities
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("DealActivity.js")%>
	<%=Html.JavascriptInclueTag("DealActivityCapitalCall.js")%>
	<%=Html.JavascriptInclueTag("DealActivityPRCapitalCall.js")%>
	<%=Html.JavascriptInclueTag("DealActivityCashDistribution.js")%>
	<%=Html.JavascriptInclueTag("DealActivityPRCashDistribution.js")%>
	<%=Html.JavascriptInclueTag("DealActivityUFValuation.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealactivity.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="ActivityMain">
		<div class="header">
			<%: Html.Anchor("Underlying Activities", new { @class = "select tablnk", @onclick = "javascript:dealActivity.selectTab('U',this);" })%>|<%: Html.Anchor("Security Activities", new { @class = "tablnk", @onclick = "javascript:dealActivity.selectTab('S',this);" })%>
		</div>
		<div class="content">
			<div id="UnderlyingActivity" class="act-group">
				<div class="line">
				</div>
				<div id="NewCapitalCall" class="group">
					<div class="title">
						<%: Html.Span("New Capital Call")%>
					</div>
					<div class="search-tool">
						<div class="cell" style="padding-left: 10px">
							<%: Html.TextBox("CC_UnderlyingFund", "Search Underlying Fund", new { @class = "ufsearch wm" })%></div>
						<div class="cell" style="padding-left: 10px">
							<%: Html.Anchor("Make New Capital Call", "javascript:dealActivity.makeNewCC();")%>
						</div>
					</div>
					<div class="detail">
						<div class="search-header">
							<%: Html.Span("", new { @id="SpnCCUFName" })%>
						</div>
						<div class="cell loading" id="CCLoading">
						</div>
						<div class="clear">
							<table cellpadding="0" cellspacing="0" border="0" id="CapitalCallList" class="grid">
								<thead>
									<tr>
										<th style="width: 20%">
											Fund Name
										</th>
										<th style="width: 20%">
											Call Amount
										</th>
										<th style="width: 15%">
											Notice Date
										</th>
										<th style="width: 15%">
											Received Date
										</th>
										<th style="width: 15%">
											Deemed Capital Call
										</th>
										<th>
										</th>
									</tr>
								</thead>
								<tbody>
								</tbody>
							</table>
							<br />
							<div id="PRCapitalCall" class="clear" style="display: none">
								<div class="line">
								</div>
								<div class="cell checkbox" style="margin-left: 183px;">
									Post Record Date Capital Call&nbsp;&nbsp;&nbsp;<%: Html.CheckBox("IsPostRecordCapitalCall", false, new { @class = "checkbox", @onclick="javascript:dealActivity.checkPRCC(this);" })%>
								</div>
								<div class="cell" style="padding-left: 10px">
									<%: Html.Anchor("Add New Post Record Date Capital Call", "javascript:dealActivity.makeNewPRCC();")%>
								</div>
								<div class="cell loading" id="PRCCLoading">
								</div>
								<div id="PRCCListBox" class="clear" style="display: none">
									<table cellpadding="0" cellspacing="0" border="0" id="PRCapitalCallList" class="grid">
										<thead>
											<tr>
												<th style="width: 20%">
													Fund Name
												</th>
												<th style="width: 20%">
													Deal Name
												</th>
												<th style="width: 15%">
													Capital Call Amount
												</th>
												<th style="width: 15%">
													Capital Call Date
												</th>
												<th>
												</th>
											</tr>
										</thead>
										<tbody>
										</tbody>
									</table>
								</div>
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
					<div class="search-tool">
						<div class="cell" style="padding-left: 10px">
							<%: Html.TextBox("CD_UnderlyingFund", "Search Underlying Fund", new { @class = "ufsearch wm" })%></div>
						<div class="cell" style="padding-left: 10px">
							<%: Html.Anchor("Raise Distribution Call", "javascript:dealActivity.makeNewCD();")%>
						</div>
					</div>
					<div class="detail">
						<div class="search-header">
							<%: Html.Span("", new { @id="SpnCDUFName" })%>
						</div>
						<div class="cell loading" id="CDLoading">
						</div>
						<div class="clear">
							<table cellpadding="0" cellspacing="0" border="0" id="CashDistributionList" class="grid">
								<thead>
									<tr>
										<th style="width: 15%">
											Fund Name
										</th>
										<th style="width: 12%">
											Cash Distribution
										</th>
										<th style="width: 12%">
											Amount
										</th>
										<th style="width: 12%">
											Notice Date
										</th>
										<th style="width: 12%">
											Record Date
										</th>
										<th style="width: 12%">
											Deemed Distribution
										</th>
										<th style="width: 12%">
											Netted Distribution
										</th>
										<th>
										</th>
									</tr>
								</thead>
								<tbody>
								</tbody>
							</table>
							<br />
							<div id="PRCashDistribution" class="clear">
								<div class="line">
								</div>
								<div class="cell checkbox">
									Post Record Date Distribution
								</div>
								<div class="cell" style="padding-left: 10px">
									<%: Html.Anchor("Add New Post Record Date Distribution", "javascript:dealActivity.makeNewPRCD();")%>
								</div>
								<div class="cell loading" id="PRCDLoading">
								</div>
								<div id="PRCDListBox" class="clear">
									<table cellpadding="0" cellspacing="0" border="0" id="PRCashDistributionList" class="grid">
										<thead>
											<tr>
												<th style="width: 15%">
													Fund Name
												</th>
												<th style="width: 12%">
													Deal Name
												</th>
												<th style="width: 12%">
													Distribution Amount
												</th>
												<th style="width: 12%">
													Distribution Date
												</th>
												<th>
												</th>
											</tr>
										</thead>
										<tbody>
										</tbody>
									</table>
								</div>
							</div>
						</div>
						<%:Html.Hidden("CDUnderlyingFundId", new { @id = "CDUnderlyingFundId" })%>
					</div>
				</div>
				<div class="line">
				</div>
				<div id="UnderlyingFundValuation" class="group">
					<div class="title">
						<%: Html.Span("Underlying Fund Valuation")%>
					</div>
					<div class="search-tool">
						<div class="cell" style="padding-left: 10px">
							<%: Html.TextBox("UFV_UnderlyingFund", "Search Underlying Fund", new { @class = "ufsearch wm" })%></div>
						<div class="cell" style="padding-left: 10px">
							<%: Html.Anchor("Add Underlying Fund Valuation", "javascript:dealActivity.makeNewUFV();")%>
						</div>
					</div>
					<div class="detail">
						<div class="search-header">
							<%: Html.Span("", new { @id="SpnUFVName" })%>
						</div>
						<div class="cell loading" id="UFVLoading">
						</div>
						<div class="clear">
							<table cellpadding="0" cellspacing="0" border="0" id="UnderlyingFundValuationList"
								class="grid">
								<thead>
									<tr>
										<th style="width: 15%">
											Underlying Fund Name
										</th>
										<th style="width: 12%">
											Fund Name
										</th>
										<th style="width: 12%">
											Reported NAV
										</th>
										<th style="width: 12%">
											Reporting Date
										</th>
										<th style="width: 12%">
											Calculated NAV
										</th>
										<th style="width: 12%">
											Update NAV
										</th>
										<th style="width: 12%">
											Update Date
										</th>
										<th>
										</th>
									</tr>
								</thead>
								<tbody>
								</tbody>
							</table>
						</div>
						<%:Html.Hidden("UFVUnderlyingFundId", new { @id = "UFVUnderlyingFundId" })%>
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
				<div class="line">
				</div>
			</div>
			<div id="SecurityActivity" class="act-group">
			</div>
		</div>
	</div>
	<div id="EditCD">
	</div>
	<div id="EditCC">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("CC_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setCCUnderlyingFund(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("CD_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setCDUnderlyingFund(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("UFV_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setUFVUnderlyingFund(ui.item.id,ui.item.value);}"
	})%>
	<script type="text/javascript">
		dealActivity.newCDData = <%= JsonSerializer.ToJsonObject(Model.UnderlyingFundCashDistributionModel)%>;
		dealActivity.newPRCDData = <%= JsonSerializer.ToJsonObject(Model.UnderlyingFundPostRecordCashDistributionModel)%>;
		dealActivity.newCCData = <%= JsonSerializer.ToJsonObject(Model.UnderlyingFundCapitalCallModel)%>;
		dealActivity.newPRCCData = <%= JsonSerializer.ToJsonObject(Model.UnderlyingFundPostRecordCapitalCallModel)%>;
		dealActivity.newUFVData = <%= JsonSerializer.ToJsonObject(Model.UnderlyingFundValuationModel)%>;
	</script>
	<script type="text/javascript">		dealActivity.init();</script>
	<script id="CashDistributionAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("UnderlyingFundCashDistribution", Model.UnderlyingFundCashDistributionModel); %>
	</script>
	<script id="PRCashDistributionAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("UnderlyingFundPostRecordCashDistribution", Model.UnderlyingFundPostRecordCashDistributionModel); %>
	</script>
	<script id="CapitalCallAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("UnderlyingFundCapitalCall", Model.UnderlyingFundCapitalCallModel); %>
	</script>
	<script id="PRCapitalCallAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("UnderlyingFundPostRecordCapitalCall", Model.UnderlyingFundPostRecordCapitalCallModel); %>
	</script>
	<script id="UFValuationAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("UnderlyingFundValuation", Model.UnderlyingFundValuationModel); %>
	</script>
</asp:Content>

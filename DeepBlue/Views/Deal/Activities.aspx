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
	<%=Html.JavascriptInclueTag("DealActivityEquitySplit.js")%>
	<%=Html.JavascriptInclueTag("DealActivitySecConversion.js")%>
	<%=Html.JavascriptInclueTag("DealActivityFundExpense.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealactivity.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="ActivityMain">
		<div class="header">
			<div class="cell">
				<%: Html.Anchor("Underlying Activities", new { @class = "select tablnk", @onclick = "javascript:dealActivity.selectTab('U',this);" })%>|<%: Html.Anchor("Security Activities", new { @class = "tablnk", @onclick = "javascript:dealActivity.selectTab('S',this);" })%></div>
			<div id="SearchUDirect" class="cell" style="float: right; display: none;">
				<%: Html.TextBox("S_UnderlyingDirect", "Search Underlying Direct", new { @class = "wm" , @style="width:250px" })%>
			</div>
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
							<%: Html.TextBox("CC_UnderlyingFund", "Search Underlying Fund", new { @class = "wm" })%></div>
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
							<%: Html.TextBox("CD_UnderlyingFund", "Search Underlying Fund", new { @class = "wm" })%></div>
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
							<%: Html.TextBox("UFV_UnderlyingFund", "Search Underlying Fund", new { @class = "wm" })%></div>
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
					<div class="search-tool">
						<div class="cell" style="padding-left: 10px">
							<%: Html.TextBox("FLE_Fund", "Search Fund", new { @class = "wm" })%></div>
						<div class="cell">
							<%: Html.Span("", new { @id = "SpnFLEDetLoading" })%></div>
					</div>
					<div class="detail">
						<% Html.RenderPartial("FundExpense", Model.FundLevelExpenseModel);%>
						<div class="cell clear">
							<%: Html.Anchor("Add Expense to Deal", "javascript:dealActivity.makeNewExpenseDeal();")%>
						</div>
						<table cellpadding="0" cellspacing="0" border="0" id="ExpenseToDealList" class="grid">
							<thead>
								<tr>
									<th style="width: 15%">
										Deal No.
									</th>
									<th style="width: 15%">
										Deal Name
									</th>
									<th style="width: 15%">
										Expense Amount
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
				<div class="line">
				</div>
			</div>
			<div id="SecurityActivity" class="act-group" style="display: none">
				<div class="line">
				</div>
				<div class="search-header">
					<%: Html.Span("", new { @id="SpnUDirectName" })%>
				</div>
				<div class="clear sec-box">
					<div class="editor-label">
						<%: Html.Label("Corporate Action-") %>
					</div>
					<div class="editor-field" style="margin-left: 10px">
						<%: Html.DropDownList("ActivityTypeId", Model.ActivityTypes, new { @onchange = "javascript:dealActivity.changeAType(this);" })%>
					</div>
					<div id="SplitDetail" style="display: none; height: 110px; clear: both;">
						<%using (Html.Form(new { @id = "frmEquitySplit", @onsubmit = "return dealActivity.createSA(this);", @style = "float:left;" })) {%>
						<% Html.RenderPartial("EquitySplit", Model.EquitySplitModel);%>
						<%}%>
					</div>
					<div id="ConversionDetail" style="display: none; height: 110px; clear: both;">
						<%using (Html.Form(new { @id = "frmSecurityConversion", @onsubmit = "return dealActivity.createSecConversion(this);", @style = "float:left;" })) {%>
						<% Html.RenderPartial("SecurityConversion", Model.SecurityConversionModel);%>
						<%}%>
					</div>
					<%: Html.Hidden("DealUnderlyingDirectId", "0", new { @id = "DealUnderlyingDirectId" })%>
				</div>
				<div class="line">
				</div>
				<div class="heading">
					New Holding Pattern
				</div>
				<div class="cell" id="NHPLoading">
				</div>
				<div class="clear sec-box">
					<table cellpadding="0" cellspacing="0" border="0" id="NewHoldingPatternList" class="grid">
						<thead>
							<tr>
								<th style="width: 20%" align="center">
									Fund Name
								</th>
								<th style="width: 20%" align="center">
									Old Number of Shares
								</th>
								<th style="width: 20%" align="center">
									New Number of Shares
								</th>
								<th>
								</th>
							</tr>
						</thead>
					</table>
				</div>
			</div>
		</div>
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
	<%= Html.jQueryAutoComplete("S_UnderlyingDirect", new AutoCompleteOptions {
																	  Source = "/Deal/FindDealUnderlyingDirects", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.selectUD(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("SplitEquityName", new AutoCompleteOptions {
																	  Source = "/Deal/FindEquityDirects",	MinLength = 1,
																	  OnSearch = "dealActivity.onESDirectSearch",
																	  OnSelect = "function(event, ui) { dealActivity.setESDirect(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("NewSecurity", new AutoCompleteOptions {
																	  Source = "/Deal/FindEquityDirects",	MinLength = 1,
																	  OnSearch = "dealActivity.onNewSecuritySearch",
																	  OnSelect = "function(event, ui) { dealActivity.setNewSecurity(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("OldSecurity", new AutoCompleteOptions {
																	  Source = "/Deal/FindEquityDirects",	MinLength = 1,
																	  OnSearch = "dealActivity.onOldSecuritySearch",
																	  OnSelect = "function(event, ui) { dealActivity.setOldSecurity(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("FLE_Fund", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds",	MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setFLEFund(ui.item.id,ui.item.value);}"
	})%>
	<%=Html.jQueryAjaxTable("NewHoldingPatternList", new AjaxTableOptions {
	ActionName = "NewHoldingPatternList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, OnSubmit = "dealActivity.onNHPSubmit"
	, OnSuccess = "dealActivity.onNHPSuccess"
	, OnRowBound = "dealActivity.onNHPRowBound"
	, Autoload = false
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
	<script id="ExpenseToDealTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("ExpenseToDeal"); %>
	</script>
</asp:Content>

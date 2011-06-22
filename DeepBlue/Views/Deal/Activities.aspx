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
	<%=Html.JavascriptInclueTag("DealActivityUDValuation.js")%>
	<%=Html.JavascriptInclueTag("DealActivityUFAdjustment.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealactivity.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<div class="title">
					INVESTMENTS</div>
				<div class="arrow">
				</div>
				<div class="pname">
					ACTIVITIES
				</div>
			</div>
		</div>
	</div>
	<div id="ActivityMain">
		<div class="header">
			<div class="tabbg">
				<%using (Html.Div(new { @id = "UATab", @class = "select", @onclick = "javascript:dealActivity.selectTab('U',this);" })) {%>&nbsp;
				<%}%>
				<%using (Html.Div(new { @id = "SATab", @onclick = "javascript:dealActivity.selectTab('S',this);" })) {%>&nbsp;
				<%}%>
				<div id="SearchUDirect" class="cell" style="float: right; display: none; margin: 10px 50px 0px 0px;">
					<%: Html.TextBox("S_UnderlyingDirect", "Search Underlying Direct", new { @class = "wm" , @style="width:200px" })%>
				</div>
			</div>
		</div>
		<div class="content">
			<div id="UnderlyingActivity" class="act-group">
				<div class="act-box">
					<div id="NewCapitalCall" class="group">
						<div class="headerbox">
							<div class="title">
								<%: Html.Span("New Capital Call")%>
							</div>
						</div>
						<div class="expandheader expandsel" style="display: none">
							<div class="expandbtn">
								<div class="expandtitle" style="display: block;">
									New Capital Call
								</div>
							</div>
							<div class="addbtn" style="display: block;">
								<%: Html.Anchor(Html.Image("mncc.png").ToHtmlString(), "javascript:dealActivity.makeNewCC();")%></div>
							<div style="display: block; float: right; margin-right: 15%;">
								<%: Html.Span("", new { @id="SpnCCUFName" })%>
								<%: Html.TextBox("CC_UnderlyingFund", "Search Underlying Fund", new { @style="width:200px", @class = "wm" })%></div>
						</div>
						<div class="detail">
							<div class="search-header">
								<div class="cell">
								</div>
							</div>
							<div class="cell" id="CCLoading">
							</div>
							<div id="CapitalCall" class="gridbox" style="display: none;">
								<div style="clear: both; width: 100%">
									<%using (Html.Form(new { @id = "frmUFCapitalCall", @onsubmit = "return dealActivity.submitUFCapitalCall(this);" })) {%>
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
									<center>
										<span>
											<%: Html.ImageButton("Save.png")%></span><span id="SpnCCSaveLoading"></span></center>
								</div>
								<%}%>
							</div>
							<div id="PRCapitalCall" class="gridbox" style="display: none">
								<div class="subheader">
									<div class="name">
										Post Record Capital Call</div>
									<div class="addbtn">
										<%: Html.Anchor(Html.Image("addnewprcc.png").ToHtmlString(), "javascript:dealActivity.makeNewPRCC();")%></div>
								</div>
								<div class="cell" id="PRCCLoading">
								</div>
								<div id="PRCCListBox" class="clear" style="display: none">
									<%using (Html.Form(new { @id = "frmUFPRCapitalCall", @onsubmit = "return dealActivity.submitUFPRCapitalCall(this);" })) {%>
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
									<br />
									<center>
										<span>
											<%: Html.ImageButton("Save.png")%></span><span id="SpnPRCCSaveLoading"></span></center>
									<%}%>
								</div>
							</div>
							<%:Html.Hidden("CCUnderlyingFundId", new { @id = "CCUnderlyingFundId" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="NewCashDistribution" class="group">
						<div class="headerbox">
							<div class="title">
								<%: Html.Span("New Cash Distribution")%>
							</div>
						</div>
						<div class="expandheader expandsel" style="display: none">
							<div class="expandbtn">
								<div id="title" class="expandtitle" style="display: block;">
									<div class="expandtitle">
										New Cash Distribution</div>
								</div>
							</div>
							<div class="addbtn" style="display: block;">
								<%: Html.Anchor(Html.Image("rdc.png").ToHtmlString(), "javascript:dealActivity.makeNewCD();")%>
							</div>
							<div style="display: block; float: right; margin-right: 15%;">
								<%: Html.TextBox("CD_UnderlyingFund", "Search Underlying Fund", new { @style = "width:200px", @class = "wm" })%></div>
						</div>
						<div class="detail">
							<div class="search-header">
								<%: Html.Span("", new { @id="SpnCDUFName" })%>
							</div>
							<div class="cell" id="CDLoading">
							</div>
							<div class="clear">
								<div id="CashDistribution" class="gridbox" style="display: none">
									<%using (Html.Form(new { @id = "frmUFCashDistribution", @onsubmit = "return dealActivity.submitUFCashDistribution(this);" })) {%>
									<table cellpadding="0" cellspacing="0" border="0" id="CashDistributionList" class="grid">
										<thead>
											<tr>
												<th style="width: 15%">
													Fund Name
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
													Distribution Type
												</th>
												<th>
												</th>
											</tr>
										</thead>
										<tbody>
										</tbody>
									</table>
									<br />
									<center>
										<span>
											<%: Html.ImageButton("Save.png")%></span><span id="SpnCDSaveLoading"></span></center>
									<%}%>
								</div>
								<div id="PRCashDistribution" class="gridbox" style="display: none">
									<div class="subheader">
										<div class="name">
											Post Record Date Distribution</div>
										<div class="addbtn">
											<%: Html.Anchor(Html.Image("anprcd.png").ToHtmlString(), "javascript:dealActivity.makeNewPRCD();")%></div>
									</div>
									<div class="cell" id="PRCDLoading">
									</div>
									<div id="PRCDListBox" class="clear" style="display: none">
										<%using (Html.Form(new { @id = "frmUFPRCashDistribution", @onsubmit = "return dealActivity.submitUFPRCashDistribution(this);" })) {%>
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
										<br />
										<center>
											<span>
												<%: Html.ImageButton("Save.png")%></span><span id="SpnPRCDSaveLoading"></span></center>
										<%}%>
									</div>
								</div>
							</div>
							<%:Html.Hidden("CDUnderlyingFundId", new { @id = "CDUnderlyingFundId" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="UnderlyingFundValuation" class="group">
						<div class="headerbox">
							<div class="title">
								<%: Html.Span("Underlying Fund Valuation")%>
							</div>
						</div>
						<div class="expandheader expandsel" style="display: none">
							<div class="expandbtn">
								<div class="expandtitle" style="display: block;">
									<div class="expandtitle">
										Underlying Fund Valuation</div>
								</div>
							</div>
							<div style="display: block; float: right; margin-right: 15%;">
								<%: Html.TextBox("UFV_UnderlyingFund", "Search Underlying Fund", new { @style = "width:200px",  @class = "wm" })%></div>
						</div>
						<div class="detail">
							<div class="search-header">
								<%: Html.Span("", new { @id="SpnUFVName" })%>
							</div>
							<div class="cell loading" id="UFVLoading">
							</div>
							<div class="gridbox">
								<table cellpadding="0" cellspacing="0" border="0" id="UnderlyingFundValuationList"
									class="grid">
									<thead>
										<tr>
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
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="UnderlyingDirectValuation" class="group">
						<div class="headerbox">
							<div class="title">
								<%: Html.Span("Underlying Direct Valuation")%>
							</div>
						</div>
						<div class="expandheader expandsel" style="display: none">
							<div class="expandbtn">
								<div class="expandtitle" style="display: block;">
									Underlying Direct Valuation
								</div>
							</div>
							<div style="display: block; float: right; margin-right: 15%;">
								<%: Html.TextBox("UDV_UnderlyingDirect", "Search Underlying Direct", new { @style = "width:200px", @class = "wm" })%>
							</div>
						</div>
						<div class="detail">
							<div class="search-header">
								<%: Html.Span("", new { @id="SpnUDVName" })%>
							</div>
							<div class="cell loading" id="UDVLoading">
							</div>
							<div class="gridbox" id="UDValuation" style="display: none">
								<%using (Html.Form(new { @id = "frmUDValuation", @onsubmit = "return dealActivity.submitUDV(this);" })) {%>
								<table cellpadding="0" cellspacing="0" border="0" id="UDValuationList" class="grid">
									<thead>
										<tr>
											<th style="width: 25%">
												Direct Name
											</th>
											<th style="width: 20%">
												Fund Name
											</th>
											<th style="width: 12%">
												Last Price
											</th>
											<th style="width: 12%">
												Last Price Date
											</th>
											<th style="width: 12%">
												New Price
											</th>
											<th style="width: 12%">
												New Price Date
											</th>
											<th>
											</th>
										</tr>
									</thead>
									<tbody>
									</tbody>
								</table>
								<br />
								<center>
									<span>
										<%: Html.ImageButton("Save.png")%></span><span id="SpnUDVSaveLoading"></span></center>
								<%}%>
								<%:Html.Hidden("UDVIssuerId","0", new { @id = "UDVIssuerId" })%>
							</div>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="UnfundedAdjustments" class="group">
						<div class="headerbox">
							<div class="title">
								<%: Html.Span("Unfunded Adjustments")%>
							</div>
						</div>
						<div class="expandheader expandsel" style="display: none">
							<div class="expandbtn">
								<div class="expandtitle" style="display: block;">
									Unfunded Adjustments
								</div>
							</div>
							<div style="display: block; float: right; margin-right: 15%;">
								<%: Html.TextBox("UFA_UnderlyingFund", "Search Underlying Fund", new { @style = "width:200px", @class = "wm" })%>
							</div>
						</div>
						<div class="detail">
							<div class="search-header">
								<div class="cell">
									<%: Html.Span("", new { @id="SpnUFAUFName" })%></div>
							</div>
							<div class="cell" id="UFALoading">
							</div>
							<div id="UFAdjustment" style="display: none" class="gridbox">
								<%using (Html.Form(new { @id = "frmUFAAdjustment", @onsubmit = "return dealActivity.submitUFA(this);" })) {%>
								<table cellpadding="0" cellspacing="0" border="0" id="UnfundedAdjustmentList" class="grid">
									<thead>
										<tr>
											<th style="width: 20%">
												Fund Name
											</th>
											<th style="width: 20%">
												Commitment Amount
											</th>
											<th style="width: 15%">
												Unfunded Amount
											</th>
											<th>
											</th>
										</tr>
									</thead>
									<tbody>
									</tbody>
								</table>
								<br />
								<center>
									<span>
										<%: Html.ImageButton("Save.png")%></span><span id="SpnUFASaveLoading"></span></center>
								<%}%>
							</div>
							<%:Html.Hidden("UFAUnderlyingFundId", new { @id = "UFAUnderlyingFundId" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="FundLevelExpenses" class="group">
						<div class="headerbox">
							<div class="title">
								<%: Html.Span("Fund Level Expenses")%>
							</div>
						</div>
						<div class="expandheader expandsel" style="display: none">
							<div class="expandbtn">
								<div class="expandtitle" style="display: block;">
									Fund Level Expenses
								</div>
							</div>
							<div style="display: block; float: right; margin-right: 15%;">
								<%: Html.Span("", new { @id = "SpnFLEDetLoading" })%><%: Html.TextBox("FLE_Fund", "Search Fund", new { @style = "width:200px", @class = "wm" })%>
							</div>
						</div>
						<div class="detail">
							<% Html.RenderPartial("FundExpense", Model.FundLevelExpenseModel);%>
							<div class="cell clear">
								<%: Html.Anchor(Html.Image("addexpense.png").ToHtmlString(), "#")%>
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
					<div id="SplitDetail" style="display: none; clear: both; margin-left: 88px;">
						<%using (Html.Form(new { @id = "frmEquitySplit", @onsubmit = "return dealActivity.createSA(this);", @style = "float:left;" })) {%>
						<% Html.RenderPartial("EquitySplit", Model.EquitySplitModel);%>
						<%}%>
					</div>
					<div id="ConversionDetail" style="display: none; margin-left: 60px; clear: both;">
						<%using (Html.Form(new { @id = "frmSecurityConversion", @onsubmit = "return dealActivity.createSecConversion(this);", @style = "float:left;" })) {%>
						<% Html.RenderPartial("SecurityConversion", Model.SecurityConversionModel);%>
						<%}%>
					</div>
					<%: Html.Hidden("DealUnderlyingDirectId", "0", new { @id = "DealUnderlyingDirectId" })%>
				</div>
				<div class="line">
				</div>
				<div class="subheading">
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
	<%= Html.jQueryAutoComplete("UDV_UnderlyingDirect", new AutoCompleteOptions {
																	  Source = "/Issuer/FindIssuers", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setUDV(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("S_UnderlyingDirect", new AutoCompleteOptions {
																	  Source = "/Issuer/FindIssuers",	MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.selectUD(ui.item.id,ui.item.value);}"
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
	<%= Html.jQueryAutoComplete("UFA_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setUFAUnderlyingFund(ui.item.id,ui.item.value);}"
	})%>
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
	<script id="UDVAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("UnderlyingDirectValuation", Model.UnderlyingDirectValuationModel); %>
	</script>
	<script id="UFAAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("UnfundedAdjustment", Model.UnfundedAdjustmentModel); %>
	</script>
</asp:Content>

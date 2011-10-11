<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateActivityModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Activities
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.textshadow.js")%>
	<%=Html.JavascriptInclueTag("DealActivity.js")%>
	<%=Html.JavascriptInclueTag("DealActivityCapitalCall.js")%>
	<%=Html.JavascriptInclueTag("DealActivityPRCapitalCall.js")%>
	<%=Html.JavascriptInclueTag("DealActivityCashDistribution.js")%>
	<%=Html.JavascriptInclueTag("DealActivityPRCashDistribution.js")%>
	<%=Html.JavascriptInclueTag("DealActivityStockDistribution.js")%>
	<%=Html.JavascriptInclueTag("DealActivityUFValuation.js")%>
	<%=Html.JavascriptInclueTag("DealActivityEquitySplit.js")%>
	<%=Html.JavascriptInclueTag("DealActivitySecConversion.js")%>
	<%=Html.JavascriptInclueTag("DealActivityFundExpense.js")%>
	<%=Html.JavascriptInclueTag("DealActivityUDValuation.js")%>
	<%=Html.JavascriptInclueTag("DealActivityUFAdjustment.js")%>
	<%=Html.JavascriptInclueTag("DealReconcile.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealactivity.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
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
	<div class="header">
		<div class="tabbg">
			<%using (Html.Tab(new { @id = "UATab", @class = "section-tab-sel", @onclick = "javascript:dealActivity.selectTab('U',this);" })) {%>Fund
			Activities
			<%}%>
			<%using (Html.Tab(new { @id = "SATab", @onclick = "javascript:dealActivity.selectTab('S',this);" })) {%>Direct
			Activities
			<%}%>
			<%using (Html.Tab(new { @id = "RETab", @onclick = "javascript:dealActivity.selectTab('R',this);" })) {%>Reconciliation
			<%}%>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="ActivityMain">
		<div class="content">
			<div id="UnderlyingActivity" class="act-group">
				<div class="act-box">
					<div id="NewCapitalCall" class="group">
						<div class="addbtn" style="display: none;">
							<div class="tblcell">
								<%: Html.TextBox("CC_UnderlyingFund", "SEARCH UNDERLYING FUND", new { @style="width:200px", @class = "wm" })%></div>
							<div class="tblcell">
								<%: Html.CheckBox("IsManualCapitalCall", false, new { @onclick = "javascript:dealActivity.showManualCCCtl('CCDetail');" })%>&nbsp;Manual
								Capital Call
							</div>
							<div class="tblcell rightcell">
								<%using (Html.GreenButton(new { @onclick = "javascript:dealActivity.makeNewCC();" })) {%>Make
								new capital call<%}%>
							</div>
						</div>
						<div class="headerbox">
							<div class="title">
								<%: Html.Span("New Capital Call")%>
							</div>
						</div>
						<div class="expandheader expandsel" style="display: none;">
							<div class="expandbtn">
								<div class="expandtitle shadow" style="display: block;">
									New Capital Call
								</div>
							</div>
						</div>
						<div class="detail" id="CCDetail">
							<div class="search-header">
								<div class="cell">
									<%: Html.Span("", new { @id="SpnCCUFName" })%>
								</div>
								<div class="loadingcell" id="CCLoading">
								</div>
							</div>
							<div id="CapitalCall" class="gridbox" style="display: none;">
								<%using (Html.Form(new { @id = "frmUFCapitalCall", @onsubmit = "return dealActivity.submitUFCapitalCall(this);" })) {%>
								<div style="clear: both; width: 100%">
									<div>
										<% Html.RenderPartial("TBoxTop"); %>
										<table cellpadding="0" cellspacing="0" border="0" id="CapitalCallList" class="grid">
											<thead>
												<tr>
													<th id="CCExpand" style="width: 5%; display: none;" class="calign ismanual">
													</th>
													<th class="lalign" style="width: 20%">
														Fund Name
													</th>
													<th class="lalign" style="width: 20%">
														Call Amount
													</th>
													<th class="lalign" style="width: 15%;">
														Due Date
													</th>
													<th class="lalign" style="width: 35%">
														Deemed Capital Call
													</th>
													<th id="CCTC" style="display: none;" class="ralign ismanual">
														Total Commitment Amount
													</th>
													<th>
													</th>
												</tr>
											</thead>
											<tbody>
											</tbody>
										</table>
										<% Html.RenderPartial("TBoxBottom"); %>
									</div>
									<center>
										<span>
											<%: Html.ImageButton("Save_active.png")%></span><span id="SpnCCSaveLoading"></span></center>
								</div>
								<%}%>
							</div>
							<div id="PRCapitalCall" class="gridbox" style="display: none">
								<div class="subheader">
									<div class="name">
										Post Record Capital Call</div>
									<div class="addbtn">
										<div class="tblcell rightcell">
											<%using (Html.GreenButton(new { @onclick = "javascript:dealActivity.makeNewPRCC();" })) {%>Add
											new post record date capital call<%}%>
										</div>
									</div>
									<div class="selectloading" id="PRCCLoading">
									</div>
								</div>
								<div id="PRCCListBox" class="clear" style="display: none">
									<%using (Html.Form(new { @id = "frmUFPRCapitalCall", @onsubmit = "return dealActivity.submitUFPRCapitalCall(this);" })) {%>
									<div style="margin-top: 45px;">
										<% Html.RenderPartial("TBoxTop"); %>
										<table cellpadding="0" cellspacing="0" border="0" id="PRCapitalCallList" class="grid">
											<thead>
												<tr>
													<th class="lalign" style="width: 20%">
														Fund Name
													</th>
													<th class="lalign" style="width: 20%">
														Deal Name
													</th>
													<th class="lalign" style="width: 15%">
														Capital Call Amount
													</th>
													<th class="lalign" style="width: 15%">
														Capital Call Date
													</th>
													<th class="ralign">
													</th>
												</tr>
											</thead>
											<tbody>
											</tbody>
										</table>
										<% Html.RenderPartial("TBoxBottom"); %>
									</div>
									<center>
										<span>
											<%: Html.ImageButton("Save_active.png")%></span><span id="SpnPRCCSaveLoading"></span></center>
									<%}%>
								</div>
							</div>
							<%:Html.Hidden("CCUnderlyingFundId", "0", new { @id = "CCUnderlyingFundId" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="NewCashDistribution" class="group">
						<div class="addbtn" style="display: none;">
							<div class="tblcell">
								<%: Html.TextBox("CD_UnderlyingFund", "SEARCH UNDERLYING FUND", new { @style = "width:200px", @class = "wm" })%>
							</div>
							<div class="tblcell">
								<%: Html.CheckBox("IsManualCashDistribution", false, new { @onclick = "javascript:dealActivity.showManualCDCtl('CDDetail');" })%>&nbsp;Manual
								Cash Distribution
							</div>
							<div class="tblcell rightcell">
								<%using (Html.GreenButton(new { @onclick = "javascript:dealActivity.makeNewCD();" })) {%>Raise
								distribution call<%}%>
							</div>
						</div>
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
						</div>
						<div class="detail" id="CDDetail">
							<div class="search-header">
								<div class="cell">
									<%: Html.Span("", new { @id="SpnCDUFName" })%></div>
								<div class="loadingcell" id="CDLoading">
								</div>
							</div>
							<div class="clear">
								<div id="CashDistribution" class="gridbox" style="display: none">
									<%using (Html.Form(new { @id = "frmUFCashDistribution", @onsubmit = "return dealActivity.submitUFCashDistribution(this);" })) {%>
									<div>
										<% Html.RenderPartial("TBoxTop"); %>
										<table cellpadding="0" cellspacing="0" border="0" id="CashDistributionList" class="grid">
											<thead>
												<tr>
													<th style="width: 5%; display: none;" class="calign ismanual">
													</th>
													<th class="lalign" style="width: 20%">
														Fund Name
													</th>
													<th class="lalign" style="width: 20%">
														Amount
													</th>
													<th class="lalign" style="width: 15%">
														Due Date
													</th>
													<th class="lalign" style="width: 30%">
														Distribution Type
													</th>
													<th style="display: none;" class="ralign ismanual">
														Total Commitment Amount
													</th>
													<th>
													</th>
												</tr>
											</thead>
											<tbody>
											</tbody>
										</table>
										<% Html.RenderPartial("TBoxBottom"); %>
									</div>
									<center>
										<span>
											<%: Html.ImageButton("Save_active.png")%></span><span id="SpnCDSaveLoading"></span></center>
									<%}%>
								</div>
								<div id="PRCashDistribution" class="gridbox" style="display: none">
									<div class="subheader">
										<div class="name">
											Post Record Date Distribution</div>
										<div class="addbtn">
											<div class="tblcell rightcell">
												<%using (Html.GreenButton(new { @onclick = "javascript:dealActivity.makeNewPRCD();" })) {%>Add
												new post record date distribution
												<%}%>
											</div>
										</div>
										<div class="selectloading" id="PRCDLoading">
										</div>
									</div>
									<div id="PRCDListBox" class="clear" style="display: none">
										<%using (Html.Form(new { @id = "frmUFPRCashDistribution", @onsubmit = "return dealActivity.submitUFPRCashDistribution(this);" })) {%>
										<div style="margin-top: 45px;">
											<% Html.RenderPartial("TBoxTop"); %>
											<table cellpadding="0" cellspacing="0" border="0" id="PRCashDistributionList" class="grid">
												<thead>
													<tr>
														<th class="lalign" style="width: 20%;">
															Fund Name
														</th>
														<th class="lalign" style="width: 20%">
															Deal Name
														</th>
														<th class="lalign" style="width: 15%">
															Distribution Amount
														</th>
														<th class="lalign" style="width: 15%">
															Distribution Date
														</th>
														<th>
														</th>
													</tr>
												</thead>
												<tbody>
												</tbody>
											</table>
											<% Html.RenderPartial("TBoxBottom"); %>
										</div>
										<center>
											<span>
												<%: Html.ImageButton("Save_active.png")%></span><span id="SpnPRCDSaveLoading"></span></center>
										<%}%>
									</div>
								</div>
							</div>
							<%:Html.Hidden("CDUnderlyingFundId", "0", new { @id = "CDUnderlyingFundId" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="New Stock Distribution" class="group">
						<div class="addbtn" style="display: none;">
							<div class="tblcell">
								<%: Html.TextBox("SD_UnderlyingFund", "SEARCH UNDERLYING FUND", new { @style = "width:200px", @class = "wm" })%></div>
							<div class="tblcell">
								<%: Html.CheckBox("IsManualStockDistribution", false, new { @onclick = "javascript:dealActivity.showManualSDCtl('SDDetail');" })%>&nbsp;Manual
								Stock Distribution
							</div>
						</div>
						<div class="headerbox">
							<div class="title">
								<%: Html.Span("New Stock Distribution")%>
							</div>
						</div>
						<div class="expandheader expandsel" style="display: none">
							<div class="expandbtn">
								<div class="expandtitle" style="display: block;">
									<div class="expandtitle">
										New Stock Distribution</div>
								</div>
							</div>
						</div>
						<div class="detail" id="SDDetail">
							<div class="search-header">
								<div class="cell">
									<%: Html.Span("", new { @id="SpnSDUFName" })%></div>
								<div class="cell" style="margin-left: 20px">
									<div style="float: left">
										Issuer&nbsp;&nbsp;
									</div>
									<div style="float: left">
										<%: Html.TextBox("UFSDIssuerName","SEARCH ISSUER", new { @class = "wm" })%>
										<%: Html.Hidden("UFSDIssuerId","")%>
									</div>
								</div>
								<div class="loadingcell" id="SDLoading">
								</div>
							</div>
							<div class="clear">
								<div id="StockDistribution" class="gridbox" style="display: none">
									<%using (Html.Form(new { @id = "frmUFStockDistribution", @onsubmit = "return dealActivity.submitUFStockDistribution(this);" })) {%>
									<div>
										<% Html.RenderPartial("TBoxTop"); %>
										<table cellpadding="0" cellspacing="0" border="0" id="StockDistributionList" class="grid">
											<thead>
												<tr>
													<th style="width: 20px; display: none;" class="lalign ismanual">
													</th>
													<th class="lalign">
														Fund Name
													</th>
													<th class="lalign">
														Equity
													</th>
													<th class="ralign">
														NumberOfShares
													</th>
													<th class="ralign">
														Purchase Price
													</th>
													<th class="lalign">
														Notice Date
													</th>
													<th class="lalign">
														Distribution Date
													</th>
													<th class="ralign">
														Tax Cost Basis Per Share
													</th>
													<th class="lalign">
														Tax Cost Date
													</th>
													<th class="ralign" style="display: none">
													</th>
													<th>
													</th>
												</tr>
											</thead>
											<tbody>
											</tbody>
										</table>
										<% Html.RenderPartial("TBoxBottom"); %>
									</div>
									<center>
										<span>
											<%: Html.ImageButton("Save_active.png")%></span><span id="SpnSDSaveLoading"></span></center>
									<%}%>
								</div>
							</div>
							<%:Html.Hidden("SDUnderlyingFundId", "0", new { @id = "SDUnderlyingFundId" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="UnderlyingFundValuation" class="group">
						<div class="addbtn" style="display: none">
							<div class="tblcell">
								<%: Html.TextBox("UFV_UnderlyingFund", "SEARCH UNDERLYING FUND", new { @style = "width:200px",  @class = "wm" })%></div>
						</div>
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
						</div>
						<div class="detail" id="UFVDetail">
							<div class="search-header">
								<div class="cell">
									<%: Html.Span("", new { @id="SpnUFVName" })%></div>
								<div class="loadingcell" id="UFVLoading">
								</div>
							</div>
							<div style="width: 90%; clear: both; margin-left: 65px;">
								<% Html.RenderPartial("TBoxTop"); %>
								<table cellpadding="0" cellspacing="0" border="0" id="UnderlyingFundValuationList"
									class="grid">
									<thead>
										<tr>
											<th class="lalign" style="width: 20%">
												Fund Name
											</th>
											<th class="ralign" style="width: 12%">
												Reported NAV
											</th>
											<th class="lalign" style="width: 12%">
												Reporting Date
											</th>
											<th class="ralign" style="width: 12%">
												Calculated NAV
											</th>
											<th class="ralign" style="width: 12%">
												Update NAV
											</th>
											<th class="lalign" style="width: 12%">
												Update Date
											</th>
											<th class="ralign">
											</th>
										</tr>
									</thead>
									<tbody>
									</tbody>
								</table>
								<% Html.RenderPartial("TBoxBottom"); %><br />
							</div>
							<%:Html.Hidden("UFVUnderlyingFundId", "0", new { @id = "UFVUnderlyingFundId" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="UnderlyingDirectValuation" class="group">
						<div class="addbtn" style="display: none">
							<div class="tblcell">
								<%: Html.TextBox("UDV_UnderlyingDirect", "SEARCH UNDERLYING DIRECT", new { @style = "width:200px", @class = "wm" })%></div>
						</div>
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
						</div>
						<div class="detail" id="UDVDetail">
							<div class="search-header">
								<div class="cell">
									<%: Html.Span("", new { @id="SpnUDVName" })%></div>
								<div class="loadingcell" id="UDVLoading">
								</div>
							</div>
							<div class="gridbox" id="UDValuation" style="display: none">
								<%using (Html.Form(new { @id = "frmUDValuation", @onsubmit = "return dealActivity.submitUDV(this);" })) {%>
								<div>
									<% Html.RenderPartial("TBoxTop"); %>
									<table cellpadding="0" cellspacing="0" border="0" id="UDValuationList" class="grid">
										<thead>
											<tr>
												<th class="lalign" style="width: 25%">
													Direct Name
												</th>
												<th class="lalign" style="width: 20%">
													Fund Name
												</th>
												<th class="lalign" style="width: 12%">
													Last Price
												</th>
												<th class="lalign" style="width: 12%">
													Last Price Date
												</th>
												<th class="ralign" style="width: 12%">
													New Price
												</th>
												<th class="lalign" style="width: 12%">
													New Price Date
												</th>
												<th>
												</th>
											</tr>
										</thead>
										<tbody>
										</tbody>
									</table>
									<% Html.RenderPartial("TBoxBottom"); %>
								</div>
								<center>
									<span>
										<%: Html.ImageButton("Save_active.png")%></span><span id="SpnUDVSaveLoading"></span></center>
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
						<div class="addbtn" style="display: none">
							<div class="tblcell">
								<%: Html.TextBox("UFA_UnderlyingFund", "SEARCH UNDERLYING FUND", new { @style = "width:200px", @class = "wm" })%>
							</div>
						</div>
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
						</div>
						<div class="detail" id="UFADetail">
							<div class="search-header">
								<div class="cell">
									<%: Html.Span("", new { @id="SpnUFAUFName" })%></div>
								<div class="loadingcell" id="UFALoading">
								</div>
							</div>
							<div id="UFAdjustment" style="display: none" class="gridbox">
								<div>
									<% Html.RenderPartial("TBoxTop"); %>
									<table cellpadding="0" cellspacing="0" border="0" id="UnfundedAdjustmentList" class="grid">
										<thead>
											<tr>
												<th class="lalign" style="width: 20%">
													Fund Name
												</th>
												<th class="ralign" style="width: 18%">
													Commitment Amount
												</th>
												<th class="ralign" style="width: 18%">
													Unfunded Amount
												</th>
												<th class="lalign" style="width: 20%;" id="UFA_NCA">
												</th>
												<th class="lalign" style="width: 20%;" id="UFA_NUA">
												</th>
												<th class="ralign">
												</th>
											</tr>
										</thead>
										<tbody>
										</tbody>
									</table>
									<% Html.RenderPartial("TBoxBottom"); %>
								</div>
							</div>
							<%:Html.Hidden("UFAUnderlyingFundId","0", new { @id = "UFAUnderlyingFundId" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="act-box">
					<div id="FundLevelExpenses" class="group">
						<div class="addbtn" style="display: none">
							<div class="tblcell">
								<%: Html.TextBox("FLE_Fund", "SEARCH FUND", new { @style = "width:200px", @class = "wm" })%>
							</div>
							<div class="tblcell rightcell">
								<%using (Html.GreenButton(new { @onclick = "javascript:dealActivity.makeNewFLE();" })) {%>Add
								expense<%}%>
							</div>
						</div>
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
							<div class="cell">
								<%: Html.Span("", new { @id = "SpnFLEDetLoading" })%>
							</div>
						</div>
						<div class="detail" id="FLEDetail">
							<div class="search-header">
								<div class="cell">
									<%:Html.Span("", new { @id = "FLE_FundName" })%></div>
								<div class="loadingcell" id="FLELoading">
								</div>
							</div>
							<div class="gridbox" id="FLE">
								<div>
									<% Html.RenderPartial("TBoxTop"); %>
									<table cellpadding="0" cellspacing="0" border="0" id="FundExpenseList" class="grid">
										<thead>
											<tr>
												<th class="lalign" style="width: 20%">
													Expense Type
												</th>
												<th class="ralign" style="width: 15%">
													Amount
												</th>
												<th class="lalign" style="width: 15%">
													Date
												</th>
												<th class="ralign" style="width: 50%">
												</th>
											</tr>
										</thead>
										<tbody>
										</tbody>
									</table>
									<% Html.RenderPartial("TBoxBottom"); %>
								</div>
								<%:Html.Hidden("FLE_FundId", "0", new { @id = "FLE_FundId" })%>
							</div>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
			</div>
			<div id="SecurityActivity" class="act-group" style="display: none">
				<div class="expandheader expandsel" style="display: none" id="SATitle">
					<div class="expandbtn">
						<div class="expandtitle" style="display: block;">
							<%: Html.Span("", new { @id="SpnUDirectName" })%>
						</div>
					</div>
				</div>
				<div class="clear sec-box" id="SADetailBox">
					<div class="editor-label">
						<%: Html.Label("Corporate Action") %>
					</div>
					<div class="editor-field" style="margin-left: 10px">
						<%: Html.DropDownList("ActivityTypeId", Model.ActivityTypes, new { @refresh="true", @action="ActivityType", @onchange = "javascript:dealActivity.changeAType(this);" })%>
					</div>
					<div id="SplitDetail" style="display: none; clear: both; margin-left: 95px;">
						<%using (Html.Form(new { @id = "frmEquitySplit", @onsubmit = "return dealActivity.createSA(this);", @style = "float:left;" })) {%>
						<% Html.RenderPartial("EquitySplit", Model.EquitySplitModel);%>
						<%}%>
					</div>
					<div id="ConversionDetail">
						<%using (Html.Form(new { @id = "frmSecurityConversion", @onsubmit = "return dealActivity.createSecConversion(this);", @style = "float:left;" })) {%>
						<% Html.RenderPartial("SecurityConversion", Model.SecurityConversionModel);%>
						<%}%>
					</div>
					<%: Html.Hidden("DealUnderlyingDirectId", "0", new { @id = "DealUnderlyingDirectId" })%>
				</div>
				<div class="line">
				</div>
				<div class="gridbox" style="margin-bottom: 20px; display: none;" id="NHPList">
					<div class="subheading">
						New Holding Pattern
					</div>
					<div class="cell" id="NHPLoading">
					</div>
					<div class="clear sec-box">
						<div>
							<% Html.RenderPartial("TBoxTop"); %>
							<table cellpadding="0" cellspacing="0" border="0" id="NewHoldingPatternList" class="grid">
								<thead>
									<tr>
										<th style="width: 20%" align="left">
											Fund Name
										</th>
										<th style="width: 20%; text-align: right;" align="right">
											Old Number of Shares
										</th>
										<th style="width: 20%; text-align: right;" align="right">
											New Number of Shares
										</th>
										<th>
										</th>
									</tr>
								</thead>
							</table>
							<% Html.RenderPartial("TBoxBottom"); %>
						</div>
					</div>
				</div>
			</div>
			<div id="Reconciliation" class="act-group" style="display: none">
				<div class="navigation">
					<div class="heading">
						<div class="leftcol">
							RECONCILIATION
						</div>
						<div class="rightcol">
							<div style="float: left">
								<%: Html.Span("", new { @id = "SpnReconLoading" }) %>
							</div>
							<div style="float: left">
								<%using (Html.Form(new { @id = "frmReconcile", @onsubmit = "return dealReconcile.submit();" })) { %>
								<div class="cell">
									<%: Html.TextBox("StartDate", "START DATE", new { @id = "ReconStartDate",  @style = "width:100px" })%>&nbsp;&nbsp;
								</div>
								<div class="cell">
									<%: Html.TextBox("EndDate", "END DATE", new { @id = "ReconEndDate",   @style = "width:100px" })%>&nbsp;&nbsp;
								</div>
								<div class="cell">
									<%: Html.TextBox("ReconcileFundName", "SEARCH AMBERBROOK FUND", new { @id = "ReconcileFundName", @class = "wm", @style = "width:200px" })%>
								</div>
								<div class="cell" style="padding-left: 10px;">
									<%: Html.TextBox("ReconcileUnderlyingFundName", "SEARCH UNDERLYING FUND", new { @id = "ReconcileUnderlyingFundName", @class = "wm", @style = "width:200px" })%>
								</div>
								<div class="cell" style="padding-left: 10px;">
									<%: Html.Image("search_active.png", new { @onclick = "javascript:dealReconcile.submit(0);" })%>
								</div>
								<%: Html.Hidden("FundId","", new { @id = "ReconcileFundId" })%>
								<%: Html.Hidden("UnderlyingFundId", "", new { @id = "ReconcileUnderlyingFundId" })%>
								<%}%>
							</div>
						</div>
					</div>
				</div>
				<div id="ReconcilReport">
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("CC_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) {  dealActivity.setCCUnderlyingFund(ui.item.id,ui.item.value); }"
	})%>
	<%= Html.jQueryAutoComplete("CD_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setCDUnderlyingFund(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("SD_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setSDUnderlyingFund(ui.item.id,ui.item.value);}"
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
	<%= Html.jQueryAutoComplete("UFSDIssuerName", new AutoCompleteOptions {
																	  SearchFunction = "dealActivity.searchUFSDIssuer",	MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setUFSDIssuer(ui.item.id);}"
	})%>
	<%= Html.jQueryAutoComplete("FLE_Fund", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds",	MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setFLEFund(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryAutoComplete("UDV_UnderlyingDirect", new AutoCompleteOptions {
	Source = "/Deal/FindIssuers",
	MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setUDV(ui.item.id,ui.item.value);}"
	})%>
	<%=Html.jQueryAjaxTable("NewHoldingPatternList", new AjaxTableOptions {
	ActionName = "NewHoldingPatternList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, OnSubmit = "dealActivity.onNHPSubmit"
	, OnSuccess = "dealActivity.onNHPSuccess"
	, OnRowBound = "dealActivity.onNHPRowBound"
	, RowClass = "row"
	, AlternateRowClass = "arow"
	, Autoload = false
	})%>
	<%= Html.jQueryAutoComplete("UFA_UnderlyingFund", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.setUFAUnderlyingFund(ui.item.id,ui.item.value);}"
	})%>
	<%=Html.jQueryDatePicker("FE_Date")%>
	<%=Html.jQueryDatePicker("SplitDate")%>
	<%=Html.jQueryDatePicker("ConversionDate")%>
	<%=Html.jQueryDatePicker("ReconStartDate", new DatePickerOptions { OnSelect = "dealReconcile.changeDate" })%>
	<%=Html.jQueryDatePicker("ReconEndDate", new DatePickerOptions { OnSelect = "dealReconcile.changeDate" })%>
	<script type="text/javascript">dealActivity.init();dealActivity.newFLEData=<%=JsonSerializer.ToJsonObject(new DeepBlue.Models.Deal.FundExpenseModel())%>;dealReconcile.init();</script>
	<script id="CashDistributionAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("UnderlyingFundCashDistribution", Model.UnderlyingFundCashDistributionModel); %>
		<% Html.RenderPartial("ManualUnderlyingFundCashDistribution"); %>
	</script>
	<script id="PRCashDistributionAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("UnderlyingFundPostRecordCashDistribution", Model.UnderlyingFundPostRecordCashDistributionModel); %>
	</script>
	<script id="CapitalCallAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("UnderlyingFundCapitalCall", Model.UnderlyingFundCapitalCallModel); %>
		<% Html.RenderPartial("ManualUnderlyingFundCapitalCall"); %>
	</script>
	<script id="PRCapitalCallAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("UnderlyingFundPostRecordCapitalCall", Model.UnderlyingFundPostRecordCapitalCallModel); %>
	</script>
	<script id="StockDistributionAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("UnderlyingFundStockDistribution"); %>
		<% Html.RenderPartial("ManualUnderlyingFundStockDistribution"); %>
	</script>
	<script id="StockDistributionDirectTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("ManualUnderlyingFundStockDistribution"); %>
	</script>
	<script id="UFValuationAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("UnderlyingFundValuation", Model.UnderlyingFundValuationModel); %>
	</script>
	<script id="UDVAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("UnderlyingDirectValuation", Model.UnderlyingDirectValuationModel); %>
	</script>
	<script id="UFAAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("UnfundedAdjustment", Model.UnfundedAdjustmentModel); %>
	</script>
	<script id="FLEAddTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("FundExpense", Model.FundLevelExpenseModel); %>
	</script>
	<script id="ReconcileReportTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("ReconcileReport"); %>
	</script>
	<script id="ReconcileGridTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("ReconcileGrid"); %>
	</script>
</asp:Content>

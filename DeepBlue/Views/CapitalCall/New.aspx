<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateCapitalCallModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Capital Call
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("CapitalCall.js") %>
	<%=Html.JavascriptInclueTag("CapitalCallManual.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">FUNDS</span><span class="arrow"></span><span class="pname">Capital
					Call</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="cc-box">
		<div class="header">
			<div class="tabbg">
				<%using (Html.Div(new { @id = "NewCCTab", @class = "select", @onclick = "javascript:capitalCall.selectTab('C',this);" })) {%>&nbsp;
				<%}%>
				<%using (Html.Div(new { @id = "ManCCTab", @onclick = "javascript:capitalCall.selectTab('M',this);" })) {%>&nbsp;
				<%}%>
				<%using (Html.Div(new { @id = "SerCCTab" })) {%>&nbsp;
				<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...", new { @id = "SpnLoading", @style = "display:none" })%>&nbsp;<%: Html.TextBox("Fund", "SEARCH FUND", new { @class = "wm", @style = "width:200px" })%>
				<%}%>
			</div>
		</div>
	</div>
	<div class="cc-main" id="CCDetail" style="display: none">
		<div class="cc-box">
			<div class="section ccdetail">
				<div class="cell">
					<label>
						<%:Html.Span("",new { id = "TitleFundName" })%></label>
					<%: Html.HiddenFor(model => model.FundId)%>
					<%: Html.HiddenFor(model => model.CapitalCallNumber)%>
				</div>
				<div class="cell">
					<label>
						Committed Amount:-<%: Html.Span("", new { @id = "CommittedAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						Unfunded Amount-<%: Html.Span("", new { @id = "UnfundedAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						<%: Html.Anchor("Previous Capital Calls","#", new { @id="lnkPCC", @target = "_blank", @style="color:Blue" })%>
					</label>
				</div>
			</div>
		</div>
		<div class="line">
		</div>
		<div id="NewCapitalCall">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "CapitalCall", @onsubmit = "return false" })) {%>
				<div class="line">
				</div>
				<div class="cc-box-det">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.CapitalAmountCalled)%>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBoxFor(model => model.CapitalAmountCalled, new { @onkeypress = "return jHelper.isCurrency(event);", @style = "width:110px", @onkeyup = "javascript:capitalCall.calcExistingInvestmentAmount();" })%>
					</div>
					<div class="editor-label" style="clear: right; margin-left: 142px;">
						<%: Html.LabelFor(model => model.CapitalCallDate) %>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBoxFor(model => model.CapitalCallDate, new { @style = "width:110px" })%>
					</div>
					<div class="editor-label" style="clear: right; width: 148px;">
						<%: Html.LabelFor(model => model.CapitalCallDueDate) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBoxFor(model => model.CapitalCallDueDate, new { @style = "width:110px" })%>
					</div>
					<div class="editor-label" style="width: auto">
						<%: Html.CheckBox("AddManagementFees",false,new { @style="width:auto", @onclick="javascript:capitalCall.selectMFee(this);" })%>&nbsp;Add
						Management Fees
					</div>
					<div id="ManFeeMain" style="display: none; float: left; margin-left: 115px;">
						<div class="editor-label fromcell">
							<%: Html.LabelFor(model => model.FromDate) %>
						</div>
						<div class="editor-field" style="width: auto;">
							<%: Html.TextBox("FromDate", "", new { @class = "datetxt", @id = "FromDate", @style="width:111px", @onchange = "javascript:capitalCall.changeFromDate();" })%>&nbsp;<%: Html.LabelFor(model => model.ToDate) %>&nbsp;<%: Html.TextBox("ToDate", "", new { @class = "datetxt", @id = "ToDate", @style = "width:111px", @onchange = "javascript:capitalCall.changeToDate();" })%>
						</div>
						<div class="editor-label" style="width: auto; clear: right; margin-left: 84px;">
							Fee Amount-&nbsp;<%: Html.Span("",new { @id = "SpnMFA" })%>&nbsp;<%: Html.Span(Html.Image("detail.png", new { @onclick = "javascript:capitalCall.showDetail(this);" , @style="cursor:pointer", @align="absmiddle" , @title = "View Rate Schedule" }).ToHtmlString(), new { @id = "SpnDetail" , @style="display:none" })%>
						</div>
					</div>
					<div class="editor-label">
						<%: Html.CheckBox("AddFundExpenses", false, new { @style = "width:auto", @onclick = "javascript:capitalCall.selectFundExp(this);" })%>&nbsp;Add
						Fund Expenses
					</div>
					<div id="FunExpAmount" style="display: none; float: left; margin-left: 206px;">
						<div class="editor-label fromcell">
							Fund Expense Amount:
						</div>
						<div class="editor-field" style="width: auto">
							<%: Html.TextBox("FundExpenseAmount", "", new { @class = "datetxt", @style="width:111px", @onkeypress = "return jHelper.isCurrency(event);", @onkeyup="javascript:capitalCall.calcExistingInvestmentAmount();" })%>
						</div>
					</div>
					<div class="editor-label" style="text-align: left;">
						Capital Call Split For
					</div>
					<div class="editor-label fromcell" style="clear: both;">
						<%: Html.LabelFor(model => model.NewInvestmentAmount) %>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBoxFor(model => model.NewInvestmentAmount, new { @class = "datetxt", @style="width:110px;", @onkeypress = "return jHelper.isCurrency(event);", @onkeyup="javascript:capitalCall.calcExistingInvestmentAmount();" })%>
					</div>
					<div class="editor-label fromcell" style="width: auto">
						<%: Html.LabelFor(model => model.ExistingInvestmentAmount) %>
					</div>
					<div class="editor-field" style="width: auto; padding-top: 10px;">
						<b>
							<%: Html.Span("", new { @id = "SpnExistingInvestmentAmount" })%></b>
						<%: Html.HiddenFor(model => model.ExistingInvestmentAmount)%>
					</div>
					<div class="editor-button" style="margin: 0 0 0 25%; padding-top: 10px; width: auto;">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Image("submit.png", new { @class = "default-button", @onclick = "javascript:capitalCall.save('CapitalCall');" })%>
						</div>
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Span("", new { @id = "UpdateLoading" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<%: Html.HiddenFor(model => model.ManagementFees)%>
				<% } %>
			</div>
			<div id="UpdateTargetId" style="display: none">
			</div>
			<div id="TierDetailMain" class="TierDetail-Main">
				<table cellpadding="0" cellspacing="0" border="0" id="TierDetail">
					<thead>
						<tr>
							<th style="width: 20%" align="center">
								From Date
							</th>
							<th style="width: 20%" align="center">
								To Date
							</th>
							<th style="width: 20%">
								Calculation Type
							</th>
							<th style="width: 20%" align="right">
								Rate %
							</th>
							<th style="width: 20%" align="right">
								Flat Fee
							</th>
						</tr>
					</thead>
				</table>
			</div>
		</div>
		<div id="NewManualCapitalCall" style="display: none">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "ManualCapitalCall", @onsubmit = "return false" })) {%>
				<div class="line">
				</div>
				<div class="cc-box-det">
					<table cellpadding="0" cellspacing="0" border="0" class="mancctbl">
						<tr>
							<td>
								<%: Html.LabelFor(model => model.CapitalCallNumber)%>
							</td>
							<td>
								<b>
									<%: Html.Span("", new {  @id="SpnCapitalCallNumber"})%></b>
							</td>
							<td>
								<%: Html.LabelFor(model => model.CapitalCallDate) %>
							</td>
							<td>
								<%: Html.TextBoxFor(model => model.CapitalCallDate, new { @id= "ManCapitalCallDate", @style = "width:110px" })%>
							</td>
							<td>
								<%: Html.LabelFor(model => model.CapitalCallDueDate) %>
							</td>
							<td>
								<%: Html.TextBoxFor(model => model.CapitalCallDueDate, new { @id = "ManCapitalCallDueDate", @style = "width:110px" })%>
							</td>
						</tr>
						<tr>
							<td>
								<%: Html.LabelFor(model => model.CapitalAmountCalled) %><%: Html.HiddenFor(model => model.CapitalAmountCalled) %>
							</td>
							<td>
								<b>
									<%: Html.Span("$0",new { @id = "SpnCapitalAmountCalled" }) %></b>
							</td>
							<td>
								<%: Html.LabelFor(model => model.ManagementFeeInterest) %><%: Html.HiddenFor(model => model.ManagementFeeInterest)%>
							</td>
							<td>
								<b>
									<%: Html.Span("$0", new { @id = "SpnManagementFeeInterest" })%></b>
							</td>
							<td>
								<%: Html.LabelFor(model => model.InvestedAmountInterest) %><%: Html.HiddenFor(model => model.InvestedAmountInterest)%>
							</td>
							<td>
								<b>
									<%: Html.Span("$0", new { @id = "SpnInvestedAmountInterest" })%></b>
							</td>
						</tr>
						<tr>
							<td>
								<%: Html.LabelFor(model => model.FundExpenses) %><%: Html.HiddenFor(model => model.FundExpenses)%>
							</td>
							<td>
								<b>
									<%: Html.Span("$0", new { @id = "SpnFundExpenses" })%></b>
							</td>
							<td>
								<%: Html.LabelFor(model => model.ManagementFees) %><%: Html.HiddenFor(model => model.ManagementFees)%>
							</td>
							<td>
								<b>
									<%: Html.Span("$0", new { @id = "SpnManagementFees" })%></b>
							</td>
							<td>
							</td>
						</tr>
					</table>
				</div>
				<div class="line">
				</div>
				<div class="cc-box-det" style="padding-top: 10px">
					<div class="cc-manual">
						<div class="editor-label">
							<%: Html.Anchor(Html.Image("addinvestor.png").ToHtmlString(),"javascript:manualCapitalCall.addInvestor();") %>
						</div>
						<div id="InvestorDetail" class="investor-detail">
							<div class="gbox">
								<table cellpadding="0" cellspacing="0" id="InvestorList" class="grid">
									<thead>
										<tr>
											<th style="width: 15%; text-align: left;">
												Investor Name
											</th>
											<th style="text-align: right;">
												Capital Call Amount
											</th>
											<th style="text-align: right;">
												Management Fees Interest
											</th>
											<th style="text-align: right;">
												Invested Amount Interest
											</th>
											<th style="text-align: right;">
												Management Fees
											</th>
											<th style="text-align: right;">
												Fund Expenses
											</th>
											<th style="width: 5%; text-align: right;">
											</th>
										</tr>
									</thead>
									<tbody>
									</tbody>
								</table>
							</div>
						</div>
					</div>
					<%: Html.HiddenFor(model => model.InvestorCount)%>
					<div class="editor-button" style="margin: 0 0 0 40%; padding-top: 10px; width: auto;">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Image("submit.png", new { @class = "default-button", @onclick = "javascript:manualCapitalCall.save('ManualCapitalCall');" })%>
						</div>
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Span("", new { @id = "ManualUpdateLoading" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<div id="ManualUpdateTargetId" style="display: none">
				</div>
			</div>
			<%}%>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("CapitalCallDate")%>
	<%= Html.jQueryDatePicker("CapitalCallDueDate")%>
	<%= Html.jQueryDatePicker("ManCapitalCallDate")%>
	<%= Html.jQueryDatePicker("ManCapitalCallDueDate")%>
	<%= Html.jQueryDatePicker("FromDate")%>
	<%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { capitalCall.selectFund(ui.item.id);}"})%>
	<%= Html.jQueryFlexiGrid("TierDetail", new FlexigridOptions { Height = 0, ResizeWidth=false })%>
	<script id="CapitalCallInvestorTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CapitalCallInvestorDetail"); %>
	</script>
	<script type="text/javascript">
		capitalCall.init();
	</script>
</asp:Content>

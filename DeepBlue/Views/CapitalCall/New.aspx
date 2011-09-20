﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateCapitalCallModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Capital Call
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("CapitalCall.js") %>
	<%=Html.JavascriptInclueTag("CapitalCallManual.js")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
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
				<div class="tabinnerbox">
					<%using (Html.Tab(new { @id = "NewCCTab", @class = "section-tab-sel", @onclick = "javascript:capitalCall.selectTab('C',this);" })) {%>New
					Capital Call
					<%}%>
					<%using (Html.Tab(new { @id = "ManCCTab", @class = "section-tab", @onclick = "javascript:capitalCall.selectTab('M',this);" })) {%>Manual
					Capital Call
					<%}%>
					<%using (Html.Div(new { @id = "SerCCTab" })) {%>
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...&nbsp;", new { @id = "SpnLoading", @style = "display:none;float:left;" })%><%: Html.TextBox("Fund", "SEARCH FUND", new { @class = "wm", @style = "width:200px" })%>
					<%}%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
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
		<div id="NewCapitalCall">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "CapitalCall", @onsubmit = "return false" })) {%>
				<div class="line">
				</div>
				<div class="cc-box-det">
					<div class="editor-label" style="width:118px;text-align:left;">
						<%: Html.LabelFor(model => model.CapitalAmountCalled)%>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBox("CapitalAmountCalled","", new { @onkeydown = "return jHelper.isCurrency(event);", @style = "width:110px", @onkeyup = "javascript:capitalCall.calcExistingInvestmentAmount();" })%>
					</div>
					<div class="editor-label" style="clear: right; margin-left: 142px;">
						<%: Html.LabelFor(model => model.CapitalCallDate) %>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBox("CapitalCallDate","", new { @style = "width:110px" })%>
					</div>
					<div class="editor-label" style="clear: right; width: 145px;">
						<%: Html.LabelFor(model => model.CapitalCallDueDate) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("CapitalCallDueDate","", new { @style = "width:110px" })%>
					</div>
					<div class="editor-label" style="width: auto;text-align:left;">
						<%: Html.CheckBox("AddManagementFees", false, new { @style = "width:auto;", @displaywidth = "118px", @display = "Add Management Fees", @onclick = "javascript:capitalCall.selectMFee(this);" })%>
					</div>
					<div id="ManFeeMain" style="display: none; float: left; margin-left: 42px;">
						<div class="editor-label fromcell">
							<%: Html.LabelFor(model => model.FromDate) %>
						</div>
						<div class="editor-field" style="width: auto;">
							<%: Html.TextBox("FromDate", "", new { @class = "datetxt", @id = "FromDate", @style="width:111px" })%>&nbsp;<%: Html.LabelFor(model => model.ToDate) %>&nbsp;<%: Html.TextBox("ToDate", "", new { @class = "datetxt", @id = "ToDate", @style = "width:111px" })%>
						</div>
						<div class="editor-label" style="width: auto; clear: right; margin-left: 84px;">
							Fee Amount
						</div>
						<div class="editor-field">
							<%: Html.TextBox("ManagementFees", "", new { @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:capitalCall.calcExistingInvestmentAmount();" })%>
						</div>
					</div>
					<div class="editor-label" style="text-align:left;">
						<%: Html.CheckBox("AddFundExpenses", false, new { @style = "width:auto;", @displaywidth = "118px", @display = "Add Fund Expenses", @onclick = "javascript:capitalCall.selectFundExp(this);" })%>
					</div>
					<div id="FunExpAmount" style="display: none; float: left; margin-left: 115px;">
						<div class="editor-label fromcell">
							Fund Expense Amount:
						</div>
						<div class="editor-field" style="width: auto">
							<%: Html.TextBox("FundExpenseAmount", "", new { @class = "datetxt", @style="width:111px", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup="javascript:capitalCall.calcExistingInvestmentAmount();" })%>
						</div>
					</div>
					<div class="editor-label" style="text-align: left;">
						Capital Call Split For
					</div>
					<div class="editor-label fromcell" style="clear: both;">
						<%: Html.LabelFor(model => model.NewInvestmentAmount) %>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBoxFor(model => model.NewInvestmentAmount, new { @class = "datetxt", @style="width:110px;", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup="javascript:capitalCall.calcExistingInvestmentAmount();" })%>
					</div>
					<div class="editor-label fromcell" style="width: auto">
						<%: Html.LabelFor(model => model.ExistingInvestmentAmount) %>
					</div>
					<div class="editor-field" style="width: auto;">
						<%: Html.Span("", new { @id = "SpnExistingInvestmentAmount" })%>
						<%: Html.HiddenFor(model => model.ExistingInvestmentAmount)%>
					</div>
					<div class="editor-button" style="margin: 0 0 0 57%; padding-top: 10px; width: auto;">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Image("submit_active.png", new { @class = "default-button", @onclick = "javascript:capitalCall.save('CapitalCall');" })%>
						</div>
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Span("", new { @id = "UpdateLoading" })%>
						</div>
					</div>
				</div>
				<div class="line">
				</div>
				<% } %>
			</div>
			<div id="UpdateTargetId" style="display: none">
			</div>
			<div id="TierDetailMain" class="TierDetail-Main">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="TierDetail" class="grid">
					<thead>
						<tr>
							<th style="width: 10%">
								From Date
							</th>
							<th style="width: 10%">
								To Date
							</th>
							<th style="width: 20%">
								Calculation Type
							</th>
							<th style="width: 20%" class="ralign">
								Rate %
							</th>
							<th style="width: 20%" class="ralign">
								Flat Fee
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
		<div id="NewManualCapitalCall" style="display: none">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "ManualCapitalCall", @onsubmit = "return false" })) {%>
				<div class="line">
				</div>
				<div class="cc-box-det manual">
					<table cellpadding="0" cellspacing="0" border="0" class="mancctbl">
						<tr>
							<td style="width:172px;">
								<%: Html.LabelFor(model => model.CapitalCallNumber)%>
							</td>
							<td>
								<%: Html.Span("", new {  @id="SpnCapitalCallNumber"})%>
							</td>
							<td>
								<%: Html.LabelFor(model => model.CapitalCallDate) %>
							</td>
							<td>
								<%: Html.TextBox("CapitalCallDate","", new { @id= "ManCapitalCallDate", @style = "width:110px" })%>
							</td>
							<td>
								<%: Html.LabelFor(model => model.CapitalCallDueDate) %>
							</td>
							<td>
								<%: Html.TextBox("CapitalCallDueDate","", new { @id = "ManCapitalCallDueDate", @style = "width:110px" })%>
							</td>
						</tr>
						<tr>
							<td>
								<%: Html.LabelFor(model => model.CapitalAmountCalled) %><%: Html.HiddenFor(model => model.CapitalAmountCalled) %>
							</td>
							<td>
								<%: Html.Span("$0",new { @id = "SpnCapitalAmountCalled" }) %>
							</td>
							<td>
								<%: Html.LabelFor(model => model.ManagementFeeInterest) %><%: Html.HiddenFor(model => model.ManagementFeeInterest)%>
							</td>
							<td>
								<%: Html.Span("$0", new { @id = "SpnManagementFeeInterest" })%>
							</td>
							<td>
								<%: Html.LabelFor(model => model.InvestedAmountInterest) %><%: Html.HiddenFor(model => model.InvestedAmountInterest)%>
							</td>
							<td>
								<%: Html.Span("$0", new { @id = "SpnInvestedAmountInterest" })%>
							</td>
						</tr>
						<tr>
							<td>
								<%: Html.LabelFor(model => model.FundExpenses) %><%: Html.HiddenFor(model => model.FundExpenses)%>
							</td>
							<td>
								<%: Html.Span("$0", new { @id = "SpnFundExpenses" })%>
							</td>
							<td>
								<%: Html.LabelFor(model => model.ManagementFees) %><%: Html.HiddenFor(model => model.ManagementFees)%>
							</td>
							<td>
								<%: Html.Span("$0", new { @id = "SpnManagementFees" })%>
							</td>
							<td>
							</td>
						</tr>
					</table>
				</div>
				<div class="line">
				</div>
				<div class="cc-box-det manual">
					<div class="cc-manual">
						<div class="editor-label" style="padding-left:0px;">
							<%: Html.Anchor(Html.Image("addinvestor.png").ToHtmlString(),"javascript:manualCapitalCall.addInvestor();") %>
						</div>
						<div id="InvestorDetail" class="investor-detail">
							<div>
								<% Html.RenderPartial("TBoxTop"); %>
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
								<% Html.RenderPartial("TBoxBottom"); %>
							</div>
						</div>
					</div>
					<%: Html.HiddenFor(model => model.InvestorCount)%>
					<div class="editor-button" style="margin: 0 0 0 40%; padding-top: 10px; width: auto;">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Image("submit_active.png", new { @class = "default-button", @onclick = "javascript:manualCapitalCall.save('ManualCapitalCall');" })%>
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
	<script id="CapitalCallInvestorTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CapitalCallInvestorDetail"); %>
	</script>
	<script type="text/javascript">
		capitalCall.init();
	</script>
	<script id="TierDetailTemplate" type="text/x-jquery-tmpl"> 
		{{each(i,tier) Tiers}}
		<tr {{if i%2>0}}class="arow"{{else}}class="row"{{/if}}>
			<td>${formatDate(tier.StartDate)}</td>
			<td>${formatDate(tier.EndDate)}</td>
			{{if tier.MultiplierTypeId==1}}
			<td>Capital Committed</td>
			<td class="ralign">${tier.Multiplier}</td>
			<td></td>
			{{else}}
			<td>Flat Fee</td>
			<td></td>
			<td class="ralign">${tier.Multiplier}</td>
			{{/if}}
		</tr>
		{{/each}}
	</script>
	<script type="text/javascript">
		$(document).ready(function () {
			var bdy=$("body");
			jHelper.jqCheckBox(bdy);
			jHelper.jqComboBox(bdy);
		});
	</script>
	<%if (Model.FundId > 0) {%>
	<script type="text/javascript">$(document).ready(function(){capitalCall.selectFund(<%=Model.FundId%>);});</script>
	<%}%>
</asp:Content>

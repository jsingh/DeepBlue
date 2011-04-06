<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateManualModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Manual Capital Call
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("ManualCapitalCall.js") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="cc-header">
		<div class="page-title">
			<h2>
				New Manual Capital Call</h2>
		</div>
		<div class="editor-label" style="width: auto">
			<%: Html.LabelFor(model => model.FundId) %>&nbsp;<%: Html.TextBox("Fund","", new { @style = "width:200px" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
		</div>
	</div>
	<div class="cc-main" id="CCDetail" style="display: none">
		<% Html.EnableClientValidation(); %>
		<% using (Ajax.BeginForm("CreateManualCapitalCall", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "manualCapitalCall.onCreateCapitalCallBegin", OnSuccess = "manualCapitalCall.onCreateCapitalCallSuccess" }, new { @id = "CapitalCall" })) {%>
		
		<div class="box">
			<div class="box-top">
				<div class="box-left">
				</div>
				<div class="box-center">
					Fund:&nbsp;
					<%:Html.Span("",new { id = "TitleFundName" })%>
				</div>
				<div class="box-right">
				</div>
			</div>
			<div class="box-content">
				<div class="edit-left">
					<div class="cc-manual-detail">
						<table cellpadding="0" cellspacing="5" border="0">
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
									<%: Html.TextBoxFor(model => model.CapitalCallDate, new { @style = "width:110px" })%>
								</td>
								<td>
									<%: Html.LabelFor(model => model.CapitalCallDueDate) %>
								</td>
								<td>
									<%: Html.TextBoxFor(model => model.CapitalCallDueDate, new { @style = "width:110px" })%>
								</td>
							</tr>
							<tr>
								<td>
									<%: Html.LabelFor(model => model.CapitalAmountCalled) %>
								</td>
								<td>
									<b>
										<%: Html.Span("$0",new { @id = "SpnCapitalCallAmount" }) %></b>
								</td>
								<td>
									<%: Html.LabelFor(model => model.ManagementFeeInterest) %>
								</td>
								<td>
									<b>
										<%: Html.Span("$0", new { @id = "SpnManagementFeeInterest" })%></b>
								</td>
								<td>
									<%: Html.LabelFor(model => model.InvestedAmountInterest) %>
								</td>
								<td>
									<b>
										<%: Html.Span("$0", new { @id = "SpnInvestedAmountInterest" })%></b>
								</td>
							</tr>
							<tr>
								<td>
									<%: Html.LabelFor(model => model.FundExpenses) %>
								</td>
								<td>
									<b>
										<%: Html.Span("$0", new { @id = "SpnFundExpenses" })%></b>
								</td>
								<td>
									<%: Html.LabelFor(model => model.ManagementFees) %>
								</td>
								<td colspan="2">
									<b>
										<%: Html.Span("$0", new { @id = "SpnManagementFees" })%></b>
								</td>
							</tr>
						</table>
					</div>
				</div>
				<div class="edit-right" id="accordion">
					<h3>
						<a href="#">Fund Details</a></h3>
					<div>
						<div id="FundDetails">
							<div class="editor-label">
								Committed Amount:
							</div>
							<div class="editor-label" style="clear: right">
								<b>
									<%: Html.Span("",new { @id="CommittedAmount"})%></b>
							</div>
							<div class="editor-label" style="width: 109px">
								Unfunded Amount:
							</div>
							<div class="editor-label" style="clear: right">
								<b>
									<%: Html.Span("",new { @id="UnfundedAmount"})%></b>
							</div>
							<div class="editor-label">
								<%: Html.Anchor("Previous Capital Calls","#", new { @id="lnkPCC", @target = "_blank", @style="color:Blue" })%>
							</div>
						</div>
					</div>
				</div>
				<div class="cc-manual">
					<div class="editor-label">
						<%: Html.Label("Investor:") %>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("Investor", "", new { @id = "Investor", @style = "width:200px"})%>
					</div>
					<div id="InvestorDetail" class="investor-detail">
						<table cellpadding="0" cellspacing="0" id="InvestorList">
							<thead>
								<tr>
									<th style="width: 15%">
										Investor Name
									</th>
									<th style="width: 15%">
										Capital Call Amount
									</th>
									<th style="width: 15%">
										Management Fees Interest
									</th>
									<th style="width: 20%">
										Invested Amount Interest
									</th>
									<th style="width: 15%">
										Management Fees
									</th>
									<th style="width: 15%">
										Fund Expenses
									</th>
									<th style="width: 5%" align="center">
									</th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td style="width: 15%">
										<div>
											<%: Html.Hidden("1_InvestorId","0",new { @id = "InvestorId" })%>
											<%: Html.Span("",new { @id = "SpnInvestorName" })%>
										</div>
									</td>
									<td style="width: 15%">
										<div>
											<%:Html.TextBox("1_CapitalCallAmount","",new { @id="txtCapitalCallAmount", @onkeypress="return jHelper.isCurrency(event);", @onkeyup="javascript:manualCapitalCall.calcCCA();" })%>
										</div>
									</td>
									<td style="width: 15%">
										<div>
											<%:Html.TextBox("1_ManagementFeeInterest", "", new { @id = "txtManagementFeeInterest", @onkeypress = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualCapitalCall.calcMFIAmt();" })%>
										</div>
									</td>
									<td style="width: 20%">
										<div>
											<%:Html.TextBox("1_InvestedAmountInterest", "", new { @id = "txtInvestedAmountInterest", @onkeypress = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualCapitalCall.calcIAI();" })%>
										</div>
									</td>
									<td style="width: 15%">
										<div>
											<%:Html.TextBox("1_ManagementFees", "", new { @id = "txtManagementFees", @onkeypress = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualCapitalCall.calcMF();" })%>
										</div>
									</td>
									<td style="width: 15%">
										<div>
											<%:Html.TextBox("1_FundExpenses", "", new { @id = "txtFundExpenses", @onkeypress = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualCapitalCall.calcFE();" })%>
										</div>
									</td>
									<td style="width: 5%">
										<div>
											<%: Html.Image("Delete.png", new { @onclick = "javascript:manualCapitalCall.deleteInvestor(this);" })%>
										</div>
									</td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
				<div class="status">
					<%: Html.Span("", new { id = "UpdateLoading" })%></div>
				<div class="editor-button">
					<div style="float: left; padding: 0 0 10px 5px;">
						<%: Html.ImageButton("submit.png", new { @style = "width: 73px; height: 23px;", @onclick = "javascript:manualCapitalCall.onSubmit('CapitalCall');" })%>
					</div>
					<div style="float: left; padding: 0 0 10px 5px;">
						<%: Html.Span("", new { @id = "UpdateLoading" })%>
					</div>
				</div>
			</div>
		</div>
		<%: Html.HiddenFor(model => model.FundId)%>
		<%: Html.HiddenFor(model => model.CapitalCallNumber)%>
		<%: Html.HiddenFor(model => model.ManagementFees)%>
		<%: Html.HiddenFor(model => model.ManagementFeeInterest)%>
		<%: Html.HiddenFor(model => model.InvestedAmountInterest)%>
		<%: Html.HiddenFor(model => model.CapitalAmountCalled)%>
		<%: Html.HiddenFor(model => model.FundExpenses)%>
		<%: Html.HiddenFor(model => model.InvestorCount)%>
		<%: Html.ValidationMessageFor(model => model.CapitalCallDueDate) %>
		<%: Html.ValidationMessageFor(model => model.CapitalCallDate) %>
		<%: Html.ValidationMessageFor(model => model.CapitalAmountCalled) %>
		<%: Html.ValidationMessageFor(model => model.NewInvestmentAmount)%>
		<%: Html.ValidationMessageFor(model => model.ExistingInvestmentAmount)%>
		<%: Html.ValidationMessageFor(model => model.FundId) %>
		<%: Html.ValidationMessageFor(model => model.InvestorCount)%>
		<div id="UpdateTargetId" style="display: none">
		</div>
		<% } %>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("CapitalCallDate")%><%= Html.jQueryDatePicker("FromDate")%><%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryDatePicker("CapitalCallDueDate")%><%= Html.jQueryAccordion("accordion", new AccordionOptions { Disabled = true, Active = 0 })%>
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { manualCapitalCall.selectFund(ui.item.id);}"})%>
	<%= Html.jQueryAutoComplete("Investor", new AutoCompleteOptions {
																	  Source = "/Investor/FindInvestors", MinLength = 1, 
																	  OnSelect = "function(event, ui) { manualCapitalCall.selectInvestor(ui.item.id,ui.item.value);}"
																	  })%>
	<%= Html.jQueryFlexiGrid("InvestorList", new FlexigridOptions { Height = 0 })%>

	<script type="text/javascript">
		manualCapitalCall.init();
	</script>

</asp:Content>

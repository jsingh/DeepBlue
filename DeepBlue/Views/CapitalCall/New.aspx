<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateReqularCapitalCallModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Capital Call
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("CapitalCall.js") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="cc-header">
		<div class="page-title">
			<h2>
				New Capital Call</h2>
		</div>
		<div class="editor-label" style="width: auto">
			<%: Html.LabelFor(model => model.FundId) %>&nbsp;<%: Html.TextBox("Fund","", new { @style = "width:200px" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
		</div>
	</div>
	<div class="cc-main" id="CCDetail">
		<% Html.EnableClientValidation(); %>
		<% using (Ajax.BeginForm("Create", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "capitalCall.onCreateCapitalCallBegin", OnSuccess = "capitalCall.onCreateCapitalCallSuccess" }, new { @id = "CapitalCall" })) {%>
		<%: Html.ValidationSummary(true) %>
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
					<div class="editor-label">
						<%: Html.LabelFor(model => model.CapitalAmount) %>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBoxFor(model => model.CapitalAmount,new { @onkeypress = "return jHelper.isCurrency(event);" , @style="width:110px" }) %>
					</div>
					<div class="editor-label" style="clear: right">
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
						<%: Html.CheckBox("AddManagementFees",false,new { @style="width:auto", @onclick="javascript:capitalCall.selectMFee(this);" })%>&nbsp;Add Management Fees
					</div>
					<div id="ManFeeMain" style="display: none;">
						<div class="editor-label fromcell">
							<%: Html.LabelFor(model => model.FromDate) %>
						</div>
						<div class="editor-field" style="width: auto;">
							<%: Html.TextBox("FromDate", "", new { @class = "datetxt", @id = "FromDate", @onchange = "javascript:capitalCall.changeFromDate();" })%>&nbsp;<%: Html.LabelFor(model => model.ToDate) %>&nbsp;<%: Html.TextBox("ToDate", "", new { @class = "datetxt", @id = "ToDate", @onchange = "javascript:capitalCall.changeToDate();" })%>
						</div>
						<div class="editor-label" style="width: auto; clear: right">
							Management Fees Amount:&nbsp;<%: Html.Span("",new { @id = "SpnMFA" })%>&nbsp;<%: Html.Span(Html.Image("detail.png", new { @onclick = "javascript:capitalCall.showDetail(this);" , @style="cursor:pointer", @align="absmiddle" , @title = "View Rate Schedule" }).ToHtmlString(), new { @id = "SpnDetail" , @style="display:none" })%>
						</div>
					</div>
					<div class="editor-label" style="width: 143px;">
						<%: Html.CheckBox("AddFundExpenses", false, new { @style = "width:auto", @onclick = "javascript:capitalCall.selectFundExp(this);" })%>&nbsp;Add Fund Expenses
					</div>
					<div id="FunExpAmount" style="display: none;">
						<div class="editor-label fromcell">
							Fund Expense Amount:
						</div>
						<div class="editor-field" style="width: auto">
							<%: Html.TextBox("FundExpenseAmount", "", new { @class = "datetxt",@onkeypress = "return jHelper.isCurrency(event);" })%>
						</div>
					</div>
					<div class="editor-label" style="width: 143px;">
						Capital Call Split For:
					</div>
					<div class="editor-label fromcell">
						<%: Html.LabelFor(model => model.NewInvestmentAmount) %>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBoxFor(model => model.NewInvestmentAmount, new { @class = "datetxt", @onkeypress = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label fromcell" style="width: auto">
						<%: Html.LabelFor(model => model.ExistingInvestmentAmount) %>
					</div>
					<div class="editor-field" style="width: auto">
						<%: Html.TextBoxFor(model => model.ExistingInvestmentAmount, new { @class = "datetxt", @onkeypress = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="status">
						<%: Html.Span("", new { id = "UpdateLoading" })%></div>
					<div class="editor-button">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.ImageButton("submit.png", new { @style = "width: 73px; height: 23px;", @onclick = "javascript:capitalCall.onSubmit('CapitalCall');" })%>
						</div>
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Span("", new { @id = "UpdateLoading" })%>
						</div>
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
								<a id="lnkPCC" target="_blank" href="#" style="color: Blue">Previous Capital Calls</a>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<%: Html.HiddenFor(model => model.FundId)%>
		<%: Html.HiddenFor(model => model.ManagementFees)%>
		<%: Html.HiddenFor(model => model.CapitalCallNumber)%>
		<%: Html.ValidationMessageFor(model => model.CapitalCallDueDate) %>
		<%: Html.ValidationMessageFor(model => model.CapitalCallDate) %>
		<%: Html.ValidationMessageFor(model => model.CapitalAmount) %>
		<%: Html.ValidationMessageFor(model => model.NewInvestmentAmount)%>
		<%: Html.ValidationMessageFor(model => model.ExistingInvestmentAmount)%>
		<%: Html.ValidationMessageFor(model => model.FundId) %>
		<% } %>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("CapitalCallDate")%><%= Html.jQueryDatePicker("FromDate")%><%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryDatePicker("CapitalCallDueDate")%><%= Html.jQueryAccordion("accordion", new AccordionOptions { Disabled = true, Active = 0 })%>
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { capitalCall.selectFund(ui.item.id);}"})%>
	<%= Html.jQueryFlexiGrid("TierDetail", new FlexigridOptions { Height = 0 })%>

	<script type="text/javascript">
		capitalCall.init();
	</script>

</asp:Content>

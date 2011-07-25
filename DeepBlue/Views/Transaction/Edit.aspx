<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Transaction.EditModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("jquery.validate.min.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftAjax.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcAjax.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcValidation.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcCustomValidation.js")%>
	<%= Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%= Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%= Html.JavascriptInclueTag("EditTransaction.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="transaction-edit" style="width: 99%;">
		<%Html.EnableClientValidation(); %>
		<% using (Ajax.BeginForm("CreateFundTransaction", "Transaction", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "editTransaction.onTransactionBegin", OnSuccess = "editTransaction.onTransactionSuccess" }, new { @id = "EditTransaction" })) {%>
		
		<div class="header">
			<div style="float: left">
				<div class="editor-label" style="width: 100px">
					<%: Html.LabelFor(model => model.InvestorName) %>
				</div>
				<div class="editor-label" style="clear: right">
					<b>
						<%: Html.Span(Model.InvestorName) %></b>
				</div>
			</div>
			<div style="float: right">
				<div class="editor-label" style="clear: right;">
					<%: Html.LabelFor(model => model.OriginalCommitmentAmount) %>
				</div>
				<div class="editor-label" style="clear: right; width: 75px; text-align: right;">
					<b>
						<%: Html.Span(FormatHelper.CurrencyFormat(Model.OriginalCommitmentAmount))%></b>
				</div>
			</div>
		</div>
		<div class="header">
			<div style="float: left">
				<div class="editor-label" style="clear: right; width: 100px">
					TransactionType:
				</div>
				<div class="editor-label" style="clear: right">
					<b>Sell</b>
				</div>
			</div>
			<div style="float: right">
				<div class="editor-label" style="clear: right;">
					Unfunded Amount:
				</div>
				<div class="editor-label" style="clear: right; width: 75px; text-align: right;">
					<b>
						<%: Html.Span(FormatHelper.CurrencyFormat(Model.UnfundedAmount))%></b>
				</div>
			</div>
		</div>
		<div class="edit-detail" style="margin-bottom: 30px;">
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.CommitmentAmount) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("CommitmentAmount", "", new { @id = "CommitmentAmount", @style = "width:100px" })%>
					<br />
					<%: Html.ValidationMessageFor(model => model.CommitmentAmount) %>
				</div>
			</div>
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Date) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Date","", new { @id = "Date", @style = "width:100px" })%>
					<%: Html.ValidationMessageFor(model => model.Date) %>
				</div>
			</div>
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.CounterPartyInvestor) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.CounterPartyInvestor,new { @style = "width:330px",@onblur = "javascript:editTransaction.onInvestorBlur(this);"})%>
					<%: Html.ValidationMessageFor(model => model.CounterPartyInvestorId) %>
				</div>
			</div>
			<div id="InvestorTypeRow" class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.InvestorTypeId) %>
				</div>
				<div class="editor-field">
					<%: Html.Span("", new { @id = "disp_InvestorTypeId" , @style = "display:none" })%>
					<%: Html.DropDownListFor(model => model.InvestorTypeId, Model.InvestorTypes, new { @style = "width:334px" })%>
					<%: Html.ValidationMessageFor(model => model.InvestorTypeId)%>
				</div>
			</div>
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Notes) %>
				</div>
				<div class="editor-field">
					<%: Html.TextAreaFor(model => model.Notes, 5, 39, new { @style = "width:331px" })%>
					<%: Html.ValidationMessageFor(model => model.Notes) %>
				</div>
			</div>
			<%: Html.HiddenFor(model => model.CounterPartyInvestorId) %>
			<%: Html.HiddenFor(model => model.InvestorFundId)%>
			<%: Html.HiddenFor(model => model.FundId)%>
			<%: Html.HiddenFor(model => model.TransactionTypeId)%>
			<%: Html.HiddenFor(model => model.InvestorId)%>
			<%: Html.HiddenFor(model => model.InvestorName)%>
			<%: Html.HiddenFor(model => model.OriginalCommitmentAmount)%>
			<%: Html.HiddenFor(model => model.UnfundedAmount)%>
		</div>
		<div class="editor-button" style="width: 225px">
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.Span("",new { @id = "UpdateLoading" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.ImageButton("Save.png", new { @class="default-button", @onclick = "return editTransaction.onSubmit('EditTransaction');" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.Image("Close.png", new { @class="default-button", @onclick = "editTransaction.closeDialog(false);" })%>
			</div>
		</div>
		<% } %>
	</div>
	<div id="UpdateTargetId" style="display:none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("CounterPartyInvestor", new AutoCompleteOptions {
	Source = "/Investor/FindOtherInvestors?investorId=" + Model.InvestorId.ToString(), MinLength = 1,
																			OnSelect = "function(event, ui){ editTransaction.selectInvestor(ui.item.id);}"
		})%>
	<%= Html.jQueryDatePicker("Date")%>

	<script type="text/javascript">
		editTransaction.init();
	</script>

</asp:Content>

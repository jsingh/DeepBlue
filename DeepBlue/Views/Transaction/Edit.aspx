<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Transaction.EditModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%= Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%= Html.JavascriptInclueTag("EditTransaction.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="transaction-edit">
		<%Html.EnableClientValidation(); %>
		<% using (Ajax.BeginForm("Update", null, new AjaxOptions { OnBegin = "editTransaction.onBegin", OnSuccess = "editTransaction.closeDialog" }, new { @id = "EditTransaction" } )) {%>
		<%: Html.ValidationSummary(true) %>
		<div class="header">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.InvestorName) %>
			</div>
			<div class="editor-label" style="clear: right">
				<b>
					<%: Html.Span(Model.InvestorName) %></b>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.LabelFor(model => model.OriginalCommitmentAmount) %>
			</div>
			<div class="editor-label" style="clear: right">
				<b>
					<%: Html.Span(Model.OriginalCommitmentAmount.ToString())%></b>
			</div>
		</div>
		<div class="edit-detail">
			<div class="editor-label">
				<%: Html.RadioButton("TransactionType", (int)DeepBlue.Models.Transaction.Enums.TransactionType.Buy, (Model.TransactionTypeId == (int)DeepBlue.Models.Transaction.Enums.TransactionType.Buy ? true : false), new { id = "Buy", onclick = "javascript:editTransaction.onClickTransactionType(this);" })%>
				&nbsp;<%: Html.Span(DeepBlue.Models.Transaction.Enums.TransactionType.Buy.ToString())%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.RadioButton("TransactionType", (int)DeepBlue.Models.Transaction.Enums.TransactionType.Sell, (Model.TransactionTypeId == (int)DeepBlue.Models.Transaction.Enums.TransactionType.Sell ? true : false), new { id = "Sell", onclick = "javascript:editTransaction.onClickTransactionType(this);" })%>&nbsp;<%: Html.Span(DeepBlue.Models.Transaction.Enums.TransactionType.Sell.ToString())%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.RadioButton("TransactionType", (int)DeepBlue.Models.Transaction.Enums.TransactionType.Split, (Model.TransactionTypeId == (int)DeepBlue.Models.Transaction.Enums.TransactionType.Split ? true : false), new { id = "Split", onclick = "javascript:editTransaction.onClickTransactionType(this);" })%>&nbsp;<%: Html.Span(DeepBlue.Models.Transaction.Enums.TransactionType.Split.ToString())%>
			</div>
			<div class="editor-label">
				<%: Html.Span("Commitment Amount:",new { id = "CommitmentAmount" })%>
			</div>
			<div class="editor-field">
				<%: Html.TextBoxFor(model => model.CommitmentAmount, String.Format("{0:F}", Model.CommitmentAmount)) %>
				<%: Html.ValidationMessageFor(model => model.CommitmentAmount) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Date) %>
			</div>
			<div class="editor-field">
				<%: Html.TextBoxFor(model => model.Date) %>
				<%: Html.ValidationMessageFor(model => model.Date) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.CounterParty) %>
			</div>
			<div class="editor-field">
				<%: Html.TextBoxFor(model => model.CounterParty) %>
				<%: Html.ValidationMessageFor(model => model.CounterParty) %>
			</div>
			<div id="splitdetail" class="split-detail">
				<div class="editor-label" style="width: 200px">
					<b>Other Investor Information</b>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.InvestorName) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.OtherInvestorName)%>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.OtherInvestorCommitmentAmount) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.OtherInvestorCommitmentAmount) %>
				</div>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Notes) %>
			</div>
			<div class="editor-field">
				<%: Html.TextAreaFor(model => model.Notes,5,40,new {}) %>
				<%: Html.ValidationMessageFor(model => model.Notes) %>
			</div>
			<%: Html.HiddenFor(model => model.OtherInvestorId) %>
			<%: Html.HiddenFor(model => model.InvestorFundId)%>
			<%: Html.HiddenFor(model => model.InvestorFundTransactionId)%>
			<%: Html.HiddenFor(model => model.TransactionTypeId)%>
		</div>
		<div class="editor-button" style="width: 225px">
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.Span("",new { id = "UpdateLoading" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return editTransaction.onSubmit();" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.ImageLink("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "editTransaction.closeDialog();" })%>
			</div>
		</div>
		<% } %>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoCompleteScript("OtherInvestorName", new AutoCompleteOptions {
	Source = "/Investor/FindOtherInvestors?investorid=" + Model.InvestorId.ToString(), MinLength = 1,
																			OnSelect = "function(event, ui){ editTransaction.selectInvestor(ui.item.id);}"
		})%>
	<%= Html.jQueryDatePickerScript("Date")%>

	<script type="text/javascript">
		editTransaction.init();
	</script>

</asp:Content>

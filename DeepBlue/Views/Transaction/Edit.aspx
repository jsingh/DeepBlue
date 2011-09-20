<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Transaction.EditModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%= Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%= Html.JavascriptInclueTag("EditTransaction.js")%>
	<%= Html.StylesheetLinkTag("transaction.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="transaction-edit">
		<%Html.EnableClientValidation(); %>
		<%using (Html.Form(new { @id = "EditTransaction", @onsubmit = "return editTransaction.save(this);" })) { %>
		<div class="header">
			<div style="float: left">
				<div class="editor-label" style="width: 100px">
					<%: Html.LabelFor(model => model.InvestorName) %>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Span(Model.InvestorName) %>
				</div>
			</div>
			<div style="float: right">
				<div class="editor-label" style="clear: right;">
					<%: Html.LabelFor(model => model.OriginalCommitmentAmount) %>
				</div>
				<div class="editor-label" style="clear: right; width: 75px; text-align: right;">
					<%: Html.Span(FormatHelper.CurrencyFormat(Model.OriginalCommitmentAmount))%>
				</div>
			</div>
		</div>
		<div class="header">
			<div style="float: left">
				<div class="editor-label" style="clear: right; width: 100px">
					TransactionType:
				</div>
				<div class="editor-label" style="clear: right">
					Sell
				</div>
			</div>
			<div style="float: right">
				<div class="editor-label" style="clear: right;">
					Unfunded Amount:
				</div>
				<div class="editor-label" style="clear: right; width: 75px; text-align: right;">
					<%: Html.Span(FormatHelper.CurrencyFormat(Model.UnfundedAmount))%>
				</div>
			</div>
		</div>
		<div class="edit-detail" style="margin-bottom: 30px;">
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.CommitmentAmount) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("CommitmentAmount", "", new { @id = "CommitmentAmount", @onkeydown = "return jHelper.isCurrency(event);" })%>
				</div>
			</div>
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Date) %>
				</div>
				<div class="editor-field">
					<%: Html.EditorFor(model => model.Date)%>
				</div>
			</div>
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.CounterPartyInvestor) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.CounterPartyInvestor,new { @style = "width:310px",@onblur = "javascript:editTransaction.onInvestorBlur(this);"})%>
				</div>
			</div>
			<div id="InvestorTypeRow" class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.InvestorTypeId) %>
				</div>
				<div class="editor-field">
					<%: Html.Span("", new { @id = "disp_InvestorTypeId" , @style = "display:none" })%>
					<%: Html.DropDownListFor(model => model.InvestorTypeId, Model.InvestorTypes, new { @style = "width:334px" })%>
				</div>
			</div>
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Notes) %>
				</div>
				<div class="editor-field">
					<%: Html.TextAreaFor(model => model.Notes, 5, 39, new { @style = "width:331px" })%>
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
		<div class="editor-button" style="width: 350px">
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.Span("",new { @id = "UpdateLoading" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.ImageButton("Save_active.png", new { @class="default-button" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.Image("Close_active.png", new { @class="default-button", @onclick = "editTransaction.closeDialog(false);" })%>
			</div>
		</div>
		<% } %>
	</div>
	<div id="UpdateTargetId" style="display: none">
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

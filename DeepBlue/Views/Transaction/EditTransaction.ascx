<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Transaction.EditModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @id = "frmEditTransaction", @onsubmit = "return false" })) { %>
<div class="header">
	<div style="float: left">
		<div class="editor-label" style="width: 100px">
			<%: Html.LabelFor(model => model.InvestorName) %>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Span("${InvestorName}")%>
		</div>
	</div>
	<div style="float: right">
		<div class="editor-label" style="clear: right;">
			<%: Html.LabelFor(model => model.OriginalCommitmentAmount) %>
		</div>
		<div class="editor-label" style="clear: right; width: 75px; text-align: right;">
			<%: Html.Span("${formatCurrency(OriginalCommitmentAmount)}")%>
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
			<%: Html.Span("${formatCurrency(UnfundedAmount)}")%>
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
			<%: Html.TextBoxFor(model => model.CounterPartyInvestor,new { @style = "width:290px",@onblur = "javascript:editTransaction.onInvestorBlur(this);"})%>
		</div>
	</div>
	<div id="InvestorTypeRow" class="editor-row">
		<div class="editor-label">
			<%: Html.LabelFor(model => model.InvestorTypeId) %>
		</div>
		<div class="editor-field">
			<%: Html.Span("", new { @id = "disp_InvestorTypeId" , @style = "display:none" })%>
			<%: Html.DropDownListFor(model => model.InvestorTypeId, Model.InvestorTypes)%>
		</div>
	</div>
	<div class="editor-row">
		<div class="editor-label">
			<%: Html.LabelFor(model => model.Notes) %>
		</div>
		<div class="editor-field">
			<%=Html.jQueryTemplateTextArea("Notes","", 5, 39, new { @style = "width:331px" })%>
		</div>
	</div>
	<%: Html.jQueryTemplateHiddenFor(model => model.CounterPartyInvestorId)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.InvestorFundId)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.FundId)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.TransactionTypeId)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.InvestorId)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.InvestorName)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.OriginalCommitmentAmount)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.UnfundedAmount)%>
</div>
<div class="editor-footer">
	<div class="editor-label">
		<%: Html.Span("",new { @id = "UpdateLoading" })%>
	</div>
	<div class="editor-field">
		&nbsp;&nbsp;<%: Html.Image("Save_active.png", new { @class = "default-button", @onclick = "return editTransaction.save(this);" })%>
		&nbsp;&nbsp;<%: Html.Image("Close_active.png", new { @class="default-button", @onclick = "$('#EditTransaction').dialog('close');" })%>
	</div>
</div>
<% } %>

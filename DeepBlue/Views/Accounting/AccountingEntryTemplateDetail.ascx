<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Accounting.AccountingEntryTemplateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @id = "${getFormIndex()}", @onsubmit = "return false;" })) {%>
<div class="accountingEntryTemplate-box">
	<div class="accountingEntryTemplate-box-title">
		Template Details</div>
</div>
<div class="line">
</div>
<div class="accountingEntryTemplate-box-det">
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FundName) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.FundName)%>
	</div>
	<div class="editor-label" style="clear: right;">
		<%: Html.LabelFor(model => model.AccountingTransactionTypeName)%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.AccountingTransactionTypeName)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.VirtualAccountName)%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.VirtualAccountName)%>
	</div>
	<div class="editor-label" style="clear: right;">
		<%: Html.LabelFor(model => model.Amount)%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("Amount", "${checkNullOrZero(Amount)}", new { @id = "Amount", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.AccountingEntryAmountTypeName)%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.AccountingEntryAmountTypeName)%>
	</div>
	<div id="percentage" style="display: none">
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.AccountingEntryAmountTypeData)%>
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.AccountingEntryAmountTypeData)%>
		</div>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.IsCredit)%>
	</div>
	<div class="editor-field" style="width: auto">
		<%: Html.CheckBox("IsCredit", false, new { @val = "${IsCredit}" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Description)%>
	</div>
	<div class="editor-field" style="width: auto">
		<%=Html.jQueryTemplateTextArea("Description", "${Description}", 5, 64, new { @id = "Description" })%>
	</div>
	<%: Html.jQueryTemplateHiddenFor(model => model.AccountingEntryTemplateID)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.FundID)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.AccountingTransactionTypeID)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.VirtualAccountID)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.AccountingEntryAmountTypeID)%>
</div>
<div class="line">
</div>
<div class="editor-button" style="width: 328px; padding: 20px 0 0 0">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("{{if AccountingEntryTemplateID>0}}mod_temp_active.png{{else}}add_temp_active.png{{/if}}", new { @class = "default-button", onclick = "return accountingEntryTemplate.save(this);" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("cancel_active.png", new { @class = "default-button", onclick = "javascript:accountingEntryTemplate.cancel(${AccountingEntryTemplateID});" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateLoading" })%>
	</div>
</div>
<%}%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Accounting.VirtualAccountModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @id = "${getFormIndex()}", @onsubmit = "return false;" })) {%>
<div class="virtualAccount-box">
	<div class="virtualAccount-box-title">
		Virtual Account Details</div>
</div>
<div class="line">
</div>
<div class="virtualAccount-box-det">
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FundName) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.FundName)%>
	</div>
	<div class="editor-label" style="clear: right;">
		<%: Html.LabelFor(model => model.AccountName)%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.AccountName)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ParentVirtualAccountName)%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.ParentVirtualAccountName)%>
	</div>
	<div class="editor-label" style="clear: right;">
		<%: Html.LabelFor(model => model.LedgerBalance)%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("LedgerBalance", "${checkNullOrZero(LedgerBalance)}", new { @id = "LedgerBalance", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ActualAccountName)%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.ActualAccountName)%>
	</div>
	<%: Html.jQueryTemplateHiddenFor(model => model.VirtualAccountID)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.ParentVirtualAccountID)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.FundID)%>
	<%: Html.jQueryTemplateHiddenFor(model => model.ActualAccountID)%>
</div>
<div class="line">
</div>
<div class="editor-button" style="width: 328px; padding: 20px 0 0 0">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("{{if VirtualAccountID>0}}mod_va_active.png{{else}}add_va_active.png{{/if}}", new { @class = "default-button", onclick = "return virtualAccount.save(this);" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("cancel_active.png", new { @class = "default-button", onclick = "javascript:virtualAccount.cancel(${VirtualAccountID});" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateLoading" })%>
	</div>
</div>
<%: Html.jQueryTemplateHiddenFor(model => model.VirtualAccountID)%>
<%}%>
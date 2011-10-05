<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Transaction.EditModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @id = "frmEditTransaction", @onsubmit = "return false" })) { %>
<div class="info">
	<div class="info-title" style="overflow: hidden">
		${InvestorName}
	</div>
</div>
<div class="line">
</div>
<div class="info">
	<div class="editor-label" style="overflow: hidden; width: auto;">
		<%: Html.LabelFor(model => model.OriginalCommitmentAmount) %>&nbsp;<%: Html.Span("${formatCurrency(OriginalCommitmentAmount)}")%>
	</div>
	<div class="editor-label" style="overflow: hidden; width: auto; float: right; clear: right;">
		Unfunded Amount &nbsp;<%: Html.Span("${formatCurrency(UnfundedAmount)}")%>
	</div>
</div>
<div class="line">
</div>
<div class="editor-label">
	Transaction Type
</div>
<div class="editor-field">
	Sell
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.CommitmentAmount) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("CommitmentAmount", "", new { @id = "CommitmentAmount", @onkeydown = "return jHelper.isCurrency(event);" })%>
</div>
<div class="editor-label" style="clear: right;width:50px;">
	<%: Html.LabelFor(model => model.Date) %>
</div>
<div class="editor-field">
	<%: Html.EditorFor(model => model.Date)%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.CounterPartyInvestor) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.CounterPartyInvestor,new { @onblur = "javascript:editTransaction.onInvestorBlur(this);"})%>
</div>
<div id="InvestorTypeRow">
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.InvestorTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.Span("", new { @id = "disp_InvestorTypeId" , @style = "display:none" })%>
		<%: Html.DropDownListFor(model => model.InvestorTypeId, Model.InvestorTypes)%>
	</div>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.Notes) %>
</div>
<div class="editor-field">
	<%=Html.jQueryTemplateTextArea("Notes", "", 5, 99, new { })%>
</div>
<%: Html.jQueryTemplateHiddenFor(model => model.CounterPartyInvestorId)%>
<%: Html.jQueryTemplateHiddenFor(model => model.InvestorFundId)%>
<%: Html.jQueryTemplateHiddenFor(model => model.FundId)%>
<%: Html.jQueryTemplateHiddenFor(model => model.TransactionTypeId)%>
<%: Html.jQueryTemplateHiddenFor(model => model.InvestorId)%>
<%: Html.jQueryTemplateHiddenFor(model => model.InvestorName)%>
<%: Html.jQueryTemplateHiddenFor(model => model.OriginalCommitmentAmount)%>
<%: Html.jQueryTemplateHiddenFor(model => model.UnfundedAmount)%>
<div class="editor-field editor-btn">
	<%: Html.Span("",new { @id = "UpdateLoading" })%>
	&nbsp;&nbsp;<%: Html.Image("Save_active.png", new { @class = "default-button", @onclick = "return editTransaction.save(this);" })%>
	&nbsp;&nbsp;<%: Html.Image("Close_active.png", new { @class="default-button", @onclick = "$('#EditTransaction').dialog('close');" })%>
</div>
<% } %>

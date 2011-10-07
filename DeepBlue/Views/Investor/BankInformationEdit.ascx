<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @onsubmit = "return false" })) {%>
<div id="accountInfo${AccountId}" class="editinfo accountInfo">
	<div class="line">
	</div>
	<div class="info-detail">
		<%: Html.jQueryTemplateHidden("AccountId")%>
		<div class="editor-label">
			<%: Html.Label("Bank Name") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("BankName", "${BankName}", new { @class = "hide" })%>
			<%: Html.Span("${BankName}", new { @class = "show", @id = "BankName" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("ABA Number")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox( "ABANumber", "${ABANumber}", new { @class = "hide", @onkeydown = "return jHelper.isNumeric(event);" })%>
			<%: Html.Span("${ABANumber}", new { @class = "show", @id = "ABANumber" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("Account Name")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Account", "${Account}", new { @class = "hide" })%>
			<%: Html.Span("${Account}", new { @class = "show", @id = "Account" })%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Account Number")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("AccountNumber", "${AccountNumber}", new { @class = "hide" })%>
			<%: Html.Span("${AccountNumber}", new { @class = "show", @id = "AccountNumber" })%>
		</div>
		<div class="editor-label"  style="clear: right">
			<%: Html.Label("FFC")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FFC", "${FFC}", new { @class = "hide" })%>
			<%: Html.Span("${FFC}", new { @class = "show", @id = "FFC" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("FFC Number")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FFCNumber", "${FFCNumber}", new { @class = "hide" })%>
			<%: Html.Span("${FFCNumber}", new { @class = "show", @id = "FFCNumber" })%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Reference")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Reference", "${Reference}", new { @class = "hide" })%>
			<%: Html.Span("${Reference}", new { @class = "show", @id = "Reference" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("Swift")%>
		</div>
		<div class="editor-field" style="clear: right">
			<%: Html.TextBox("Swift", "${Swift}", new { @class = "hide" })%>
			<%: Html.Span("${Swift}", new { @class = "show", @id = "Swift" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("IBAN")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("IBAN", "${IBAN}", new { @class = "hide" })%>
			<%: Html.Span("${IBAN}", new { @class = "show", @id = "IBAN" })%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Phone")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("AccountPhone", "${AccountPhone}", new { @class = "hide" })%>
			<%: Html.Span("${AccountPhone}", new { @class = "show", @id = "AccountPhone" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("Fax")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("AccountFax", "${AccountFax}", new { @class = "hide" })%>
			<%: Html.Span("${AccountFax}", new { @class = "show", @id = "AccountFax" })%>
		</div>
		<%--<div class="editor-label" style="clear: right">
			<%: Html.Label("Account Of")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("AccountOf", "${AccountOf}", new { @class = "hide" })%>
			<%: Html.Span("${AccountOf}", new { @class = "show", @id = "AccountOf" })%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Attention")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Attention", "${Attention}", new { @class = "hide" })%>
			<%: Html.Span("${Attention}", new { @class = "show", @id = "Attention" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("ByOrderOf")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("ByOrderOf", "${ByOrderOf}", new { @class = "hide" })%>
			<%: Html.Span("${ByOrderOf}", new { @class = "show", @id = "ByOrderOf" })%>
		</div>--%>
		<div class="editor-row">
			<div class="editor-editbtn">
				<div class="EditInvestorInfo show" style="float: left">
					<%: Html.Anchor(Html.Image("Editbtn_active.png", new { @title = "Edit" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
					&nbsp;&nbsp;
					<%: Html.Anchor(Html.Image("delete_active.png", new { @title = "Delete Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.deleteAccount(this,${AccountId});" })%>
				</div>
				<div class="UpdateInvestorInfo hide" style="float: left; display: none;">
					<%: Html.Span("", new { @id = "Loading" })%>
					<%: Html.Image("Update_active.png", new { @style="cursor:pointer", @onclick = "javascript:editInvestor.saveBankDetail(this);",  @class = "hide" })%>
					<%: Html.Anchor(Html.Image("Cancel_active.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
				</div>
			</div>
		</div>
	</div>
</div>
<%}%>
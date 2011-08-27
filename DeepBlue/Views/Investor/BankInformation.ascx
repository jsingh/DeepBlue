<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.BankDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>

<%using (Html.Form(new { @onsubmit = "return false" })) {%>
<div id="accountInfo${AccountId}" class="editinfo accountInfo">
	<div class="editor-row">
		<div class="editor-editbtn">
			<div class="EditInvestorInfo show" style="float: left">
				<%: Html.Anchor(Html.Image("add.png", new { @title = "Add Account" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.addAccountInfo(this,${AccountId});" })%>
				&nbsp;&nbsp;
				<%: Html.Anchor(Html.Image("Editbtn.png", new { @title = "Edit" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
				&nbsp;&nbsp;
				<%: Html.Anchor(Html.Image("Delete.png", new { @title = "Delete Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.deleteAccount(this,${AccountId});" })%>
			</div>
			<div class="UpdateInvestorInfo hide" style="float: left; display: none;">
				<%: Html.Span("", new { @id = "Loading" })%>
				<%: Html.Image("Update.png", new { @style="cursor:pointer", @onclick = "javascript:editInvestor.saveBankDetail(this);",  @class = "hide" })%>
				<%: Html.Anchor(Html.Image("Cancel.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
			</div>
		</div>
	</div>
	<%: Html.jQueryTemplateHidden("AccountId")%>
	<%: Html.jQueryTemplateHidden("InvestorId")%>
	<div class="editor-label">
		<%: Html.Label("Bank Name") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("BankName", "${BankName}", new { @class = "hide" })%>
		<%: Html.Span("${BankName}", new { @class = "show", @id = "BankName" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Account Number")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("AccountNumber", "${AccountNumber}", new { @class = "hide" })%>
		<%: Html.Span("${AccountNumber}", new { @class = "show", @id = "AccountNumber" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("ABA Number")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox( "ABANumber", "${ABANumber}", new { @class = "hide", @onkeydown = "return jHelper.isNumeric(event);" })%>
		<%: Html.Span("${ABANumber}", new { @class = "show", @id = "ABANumber" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Account Of")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("AccountOf", "${AccountOf}", new { @class = "hide" })%>
		<%: Html.Span("${AccountOf}", new { @class = "show", @id = "AccountOf" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("FFC")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FFC", "${FFC}", new { @class = "hide" })%>
		<%: Html.Span("${FFC}", new { @class = "show", @id = "FFC" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("FFCNO")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FFCNO", "${FFCNO}", new { @class = "hide" })%>
		<%: Html.Span("${FFCNO}", new { @class = "show", @id = "FFCNO" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Attention")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Attention", "${Attention}", new { @class = "hide" })%>
		<%: Html.Span("${Attention}", new { @class = "show", @id = "Attention" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Swift")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Swift", "${Swift}", new { @class = "hide" })%>
		<%: Html.Span("${Swift}", new { @class = "show", @id = "Swift" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("IBAN")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("IBAN", "${IBAN}", new { @class = "hide" })%>
		<%: Html.Span("${IBAN}", new { @class = "show", @id = "IBAN" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Reference")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Reference", "${Reference}", new { @class = "hide" })%>
		<%: Html.Span("${Reference}", new { @class = "show", @id = "Reference" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("ByOrderOf")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("ByOrderOf", "${ByOrderOf}", new { @class = "hide" })%>
		<%: Html.Span("${ByOrderOf}", new { @class = "show", @id = "ByOrderOf" })%>
	</div>
</div>
<%}%>
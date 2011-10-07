<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.BankDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="accountinfo-box">
	<div class="line">
	</div>
	<div class="info-detail">
		<%: Html.Hidden("${i}_BankIndex","${i}")%>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.BankName) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_"+"BankName","${BankName}") %>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ABANumber) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "ABANumber", "${ABANumber}", new { @onkeydown = "return jHelper.isNumeric(event);" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.Account) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "Account", "${Account}")%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.AccountNumber) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "AccountNumber", "${AccountNumber}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FFC) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "FFC", "${FFC}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FFCNumber) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "FFCNumber", "${FFCNumber}")%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.Reference) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "Reference", "${Reference}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.Swift) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "Swift", "${Swift}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.IBAN) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "IBAN", "${IBAN}")%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.AccountPhone) %>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "AccountPhone", "${AccountPhone}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.AccountFax)%>
		</div>
		<div class="editor-field text">
			<%: Html.TextBox("${i}_" + "AccountFax", "${AccountFax}")%>
		</div>
		<div class="editor-row">
			<div class="editor-editbtn" style="padding-right: 115px">
				<div class="EditInvestorInfo" style="float: left">
					<%: Html.Anchor(Html.Image("Delete_active.png", new { @title = "Delete Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:investor.deleteAccount(this,0);" })%>
				</div>
			</div>
		</div>
	</div>
</div>

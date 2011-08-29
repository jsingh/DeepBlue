<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.BankDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="accountinfo-box">
	<%: Html.Hidden("${i}_BankIndex","${i}")%>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.BankName) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_"+"BankName","${BankName}") %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.AccountNumber) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "AccountNumber", "${AccountNumber}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ABANumber) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "ABANumber", "${ABANumber}", new { @onkeydown = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.AccountOf) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "AccountOf", "${AccountOf}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FFC) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "FFC", "${AccountOf}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FFCNO) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "FFCNO", "${AccountOf}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Attention) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "Attention", "${AccountOf}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Swift) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "Swift", "${Swift}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.IBAN) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "IBAN", "${IBAN}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Reference) %>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "Reference", "${Reference}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ByOrderOf)%>
	</div>
	<div class="editor-field text">
		<%: Html.TextBox("${i}_" + "ByOrderOf", "${ByOrderOf}")%>
	</div>
</div>


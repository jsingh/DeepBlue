<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Member.MemberAccountModel>" %>
<div id="AccountInfo" class="accountinfo">
	<div class="title">
		<h2>
			Account
		</h2>
	</div>
	<div class="delete">
		<img src="../../Assets/images/Delete.png" onclick="javascript:Member.deleteAccount(this);" />
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.BankName) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.BankName, new { name = Model.Index + "_" + "BankName"}) %>
		<%: Html.ValidationMessageFor(model => model.BankName) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.AccountNumber) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.AccountNumber, new { name = Model.Index + "_" + "AccountNumber" })%>
		<%: Html.ValidationMessageFor(model => model.AccountNumber) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ABANumber) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.ABANumber, new { name = Model.Index + "_" + "ABANumber" })%>
		<%: Html.ValidationMessageFor(model => model.ABANumber) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.AccountOf) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.AccountOf, new { name = Model.Index + "_" + "AccountOf" })%>
		<%: Html.ValidationMessageFor(model => model.AccountOf) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Reference) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.Reference, new { name = Model.Index + "_" + "Reference" })%>
		<%: Html.ValidationMessageFor(model => model.Reference) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Attention) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.Attention, new { name = Model.Index + "_" + "Attention" })%>
		<%: Html.ValidationMessageFor(model => model.Attention) %>
	</div>
</div>

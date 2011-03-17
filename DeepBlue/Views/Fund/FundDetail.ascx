<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Fund.FundDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="editor-label">
	<%: Html.LabelFor(model => model.FundName) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.FundName) %>
	<%: Html.ValidationMessageFor(model => model.FundName) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.TaxId) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.TaxId) %>
	<%: Html.ValidationMessageFor(model => model.TaxId) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.FundStartDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("FundStartDate",(Model.FundStartDate.Year > 1900 ? Model.FundStartDate.ToString("MM/dd/yyyy") : ""),new { @id = "FundStartDate" }) %>
	<%: Html.ValidationMessageFor(model => model.FundStartDate) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.ScheduleTerminationDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("ScheduleTerminationDate", ((Model.ScheduleTerminationDate ?? Convert.ToDateTime("01/01/1900")).Year > 1900 ? (Model.ScheduleTerminationDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "ScheduleTerminationDate" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.FinalTerminationDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("FinalTerminationDate", ((Model.FinalTerminationDate ?? Convert.ToDateTime("01/01/1900")).Year > 1900 ? (Model.FinalTerminationDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "FinalTerminationDate" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.NumofAutoExtensions) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("NumofAutoExtensions", ((Model.NumofAutoExtensions ?? 0) > 0 ? (Model.NumofAutoExtensions ?? 0).ToString() : ""), new { @id = "NumofAutoExtensions" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.DateClawbackTriggered) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("DateClawbackTriggered", ((Model.DateClawbackTriggered ?? Convert.ToDateTime("01/01/1900")).Year > 1900 ? (Model.DateClawbackTriggered ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "DateClawbackTriggered" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.RecycleProvision) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("RecycleProvision", ((Model.RecycleProvision ?? 0) > 0 ? (Model.RecycleProvision ?? 0).ToString() : ""), new { @id = "RecycleProvision" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.MgmtFeesCatchUpDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("MgmtFeesCatchUpDate", ((Model.MgmtFeesCatchUpDate ?? Convert.ToDateTime("01/01/1900")).Year > 1900 ? (Model.MgmtFeesCatchUpDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "MgmtFeesCatchUpDate" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Carry) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("Carry", ((Model.Carry ?? 0) > 0 ? (Model.Carry ?? 0).ToString() : ""), new { @id = "Carry" })%>
</div>
<% Html.RenderPartial("CustomFieldList", Model.CustomField);%>
<div class="editor-label">
	<b>Bank Details</b>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.BankName) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.BankName) %>
	<%: Html.ValidationMessageFor(model => model.BankName) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Account) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Account) %>
	<%: Html.ValidationMessageFor(model => model.Account) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.ABANumber) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.ABANumber) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Swift) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Swift) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.AccountNumberCash) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.AccountNumberCash) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.FFCNumber) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.FFCNumber) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.IBAN) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.IBAN) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Reference) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Reference) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.AccountOf) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.AccountOf) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Attention) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Attention) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.Telephone) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Telephone) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Fax) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Fax) %>
</div>
<div class="editor-label" style="height: 10px;">
</div>
<div class="editor-button" style="width: 300px">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateLoading" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return fund.onSubmit('AddNewFund');" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "javascript:parent.fund.closeDialog(false);" })%>
	</div>
</div>
<%: Html.HiddenFor(model => model.FundId)%>
<%: Html.HiddenFor(model => model.AccountId)%>
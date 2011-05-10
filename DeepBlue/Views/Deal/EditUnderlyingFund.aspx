﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateUnderlyingFundModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Underlying Fund
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("UnderlyingFund.js")%>
	<%= Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%= Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<style type="text/css">
		.editor-label {
			width: 97px;
		}
		.auto-width {
			width: auto;
		}
	</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateUnderlyingFund", null, new AjaxOptions {
		UpdateTargetId = "UpdateTargetId",
		HttpMethod = "Post",
		OnBegin = "underlyingFund.onCreateUnderlyingFundBegin",
		OnSuccess = "underlyingFund.onCreateUnderlyingFundSuccess"
	}, new { @id = "AddNewInvEnityType" })) {%>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FundName) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.FundName)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.LegalFundName) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.LegalFundName)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Description) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.Description)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.FundTypeId) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.DropDownListFor(model => model.FundTypeId,Model.UnderlyingFundTypes)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.VintageYear) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.VintageYear, new { @onkeypress = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label" style="clear:right">
		<%: Html.LabelFor(model => model.FiscalYearEnd) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.FiscalYearEnd)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.TotalSize) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.TotalSize, new { @onkeypress = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.TerminationYear) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.TerminationYear, new { @onkeypress = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.IssuerId) %>
	</div>
	<div class="editor-field checkbox">
		<%: Html.DropDownListFor(model => model.IssuerId,Model.Issuers)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.IndustryId) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.DropDownListFor(model => model.IndustryId, Model.Industries)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.GeographyId) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.DropDownListFor(model => model.GeographyId, Model.Geographyes)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ReportingFrequencyId) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.DropDownListFor(model => model.ReportingFrequencyId, Model.Reportings)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.ReportingTypeId) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.DropDownListFor(model => model.ReportingTypeId, Model.ReportingTypes)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.IsFeesIncluded) %>
	</div>
	<div class="editor-field checkbox">
		<%: Html.CheckBoxFor(model => model.IsFeesIncluded)%>
	</div>
	<div class="editor-label auto-width">
		<b>Contact Information</b>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ContactName) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.ContactName)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.WebAddress) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.WebAddress)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Address) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.Address)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.Phone) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.Phone)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Email) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.Email)%>
	</div>
	<div class="editor-label auto-width">
		<b>Bank Information</b>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.BankName) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.BankName)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.Routing)%>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.Routing, new { @onkeypress = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.AccountOf)%>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.AccountOf)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.Account)%>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.Account)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Attention)%>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.Attention)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.Reference)%>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.Reference)%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 26px;", onclick = "return underlyingFund.onSubmit('AddNewInvEnityType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 26px;cursor:pointer;", onclick = "javascript:parent.underlyingFund.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.UnderlyingFundId)%>
	<%: Html.ValidationMessageFor(model => model.FundName)%>
	<%: Html.ValidationMessageFor(model => model.FundTypeId)%>
	<%: Html.ValidationMessageFor(model => model.IssuerId)%>
	<%: Html.ValidationMessageFor(model => model.BankName)%>
	<%: Html.ValidationMessageFor(model => model.Account)%>
	<%: Html.ValidationMessageFor(model => model.VintageYear)%>
	<%: Html.ValidationMessageFor(model => model.TerminationYear)%>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		underlyingFund.init();
	</script>
	<%=Html.jQueryDatePicker("FiscalYearEnd")%>
</asp:Content>

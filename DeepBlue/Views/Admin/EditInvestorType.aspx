﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditInvestorTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Investor Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("InvestorType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateInvestorType", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId", HttpMethod = "Post",
		 OnBegin = "investorType.onCreateInvestorTypeBegin", OnSuccess = "investorType.onCreateInvestorTypeSuccess"
	 }, new { @id = "AddNewInvestorType" })) {%>
	<div class="editor-label auto-width">
		<%: Html.LabelFor(model => model.InvestorTypeName) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.InvestorTypeName) %>
		<%: Html.ValidationMessageFor(model => model.InvestorTypeName) %>
	</div>
	<div class="editor-label" style="width: 112px">
		<%: Html.LabelFor(model => model.Enabled)%>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Enabled, new { @style = "width:auto" })%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return investorType.onSubmit('AddNewInvestorType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.investorType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.InvestorTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display:none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		investorType.init();
	</script>

</asp:Content>

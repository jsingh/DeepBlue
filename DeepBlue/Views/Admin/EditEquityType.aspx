<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditEquityTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Equity Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("EquityType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateEquityType", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "equityType.onCreateEquityTypeBegin",
		 OnSuccess = "equityType.onCreateEquityTypeSuccess"
	 }, new { @id = "AddNewEquityType" })) {%>
	<div class="editor-label" style="width: 130px">
		<%: Html.LabelFor(model => model.Equity) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.Equity) %>
		<%: Html.ValidationMessageFor(model => model.Equity) %>
	</div>
	<div class="editor-label" style="width: 130px">
		<%: Html.LabelFor(model => model.Enabled)%>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Enabled, new { @style = "width:auto" })%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return equityType.onSubmit('AddNewEquityType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.equityType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.EquityTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		equityType.init();
	</script>
</asp:Content>

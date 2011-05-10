<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditFixedIncomeTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit FixedIncome Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FixedIncomeType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateFixedIncomeType", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "fixedIncomeType.onCreateFixedIncomeTypeBegin",
		 OnSuccess = "fixedIncomeType.onCreateFixedIncomeTypeSuccess"
	 }, new { @id = "AddNewFixedIncomeType" })) {%>
	<div class="editor-label" style="width: 130px">
		<%: Html.LabelFor(model => model.FixedIncomeType) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.FixedIncomeType)%>
		<%: Html.ValidationMessageFor(model => model.FixedIncomeType)%>
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
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 26px;", onclick = "return fixedIncomeType.onSubmit('AddNewFixedIncomeType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 26px;cursor:pointer;", onclick = "javascript:parent.fixedIncomeType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.FixedIncomeTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		fixedIncomeType.init();
	</script>
</asp:Content>

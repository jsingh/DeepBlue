<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditFundExpenseTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund Expense Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FundExpenseType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateFundExpenseType", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "fundExpenseType.onCreateBegin",
		 OnSuccess = "fundExpenseType.onCreateSuccess"
	 }, new { @id = "AddNewFundExpenseType" })) {%>
	<div class="editor-label" style="width: 130px">
		<%: Html.LabelFor(model => model.Name) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.Name) %>
		<%: Html.ValidationMessageFor(model => model.Name) %>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return fundExpenseType.onSubmit('AddNewFundExpenseType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.fundExpenseType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.FundExpenseTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		fundExpenseType.init();
	</script>
</asp:Content>

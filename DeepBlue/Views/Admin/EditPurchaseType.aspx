<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditPurchaseTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Purchase Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("PurchaseType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdatePurchaseType", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "purchaseType.onCreatePurchaseTypeBegin",
		 OnSuccess = "purchaseType.onCreatePurchaseTypeSuccess"
	 }, new { @id = "AddNewPurchaseType" })) {%>
	<div class="editor-label" style="width: 50px">
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
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return purchaseType.onSubmit('AddNewPurchaseType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.purchaseType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.PurchaseTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		purchaseType.init();
	</script>
</asp:Content>

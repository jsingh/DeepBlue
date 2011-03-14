<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditCustomFieldModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditCustomField
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("CustomField.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateCustomField", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "customField.onCreateCustomFieldBegin", OnSuccess = "customField.onCreateCustomFieldSuccess" }, new { @id = "AddNewCustomField" })) {%>
	<div class="editor-label auto-width">
		<%: Html.LabelFor(model => model.CustomFieldText) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.CustomFieldText) %>
		<%: Html.ValidationMessageFor(model => model.CustomFieldText) %>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return customField.onSubmit('AddNewCustomField');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "javascript:parent.customField.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.CustomFieldId) %>
	<% } %>
	<div id="UpdateTargetId">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		customField.init();
	</script>

</asp:Content>

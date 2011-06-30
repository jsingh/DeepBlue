<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditFileTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit File Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FileType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateFileType", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "fileType.onCreateFileTypeBegin",
		 OnSuccess = "fileType.onCreateFileTypeSuccess"
	 }, new { @id = "AddNewFileType" })) {%>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FileTypeName) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.FileTypeName) %>
		<%: Html.ValidationMessageFor(model => model.FileTypeName) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FileExtension) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.FileExtension)%>
		<%: Html.ValidationMessageFor(model => model.FileExtension)%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Description) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.Description)%>
		<%: Html.ValidationMessageFor(model => model.Description)%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return fileType.onSubmit('AddNewFileType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.fileType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.FileTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		fileType.init();
	</script>
</asp:Content>

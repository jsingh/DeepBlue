<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditDataTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Data Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("DataType.js")%>
	<%= Html.StylesheetLinkTag("customfield.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateDataType", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "dataType.onCreateDataTypeBegin", OnSuccess = "dataType.onCreateDataTypeSuccess" }, new { @id = "AddNewDataType" })) {%>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.DataTypeName) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.DataTypeName) %>
		<%: Html.ValidationMessageFor(model => model.DataTypeName) %>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return dataType.onSubmit('AddNewDataType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.dataType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.DataTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display:none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		dataType.init();
	</script>

</asp:Content>

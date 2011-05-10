<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditCommunicationTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Communication Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("CommunicationType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateCommunicationType", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "communicationType.onCreateCommunicationTypeBegin",
		 OnSuccess = "communicationType.onCreateCommunicationTypeSuccess"
	 }, new { @id = "AddNewCommunicationType" })) {%>
	<div class="editor-label" style="width: 130px">
		<%: Html.LabelFor(model => model.CommunicationTypeName) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.CommunicationTypeName) %>
		<%: Html.ValidationMessageFor(model => model.CommunicationTypeName) %>
	</div>
	<div class="editor-label" style="width: 130px">
		<%: Html.LabelFor(model => model.Enabled)%>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Enabled, new { @style = "width:auto" })%>
	</div>
	<div class="editor-label" style="width: 130px">
		<%: Html.LabelFor(model => model.CommunicationGroupId)%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownListFor(model => model.CommunicationGroupId,Model.CommunicationGroupings)%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 26px;", onclick = "return communicationType.onSubmit('AddNewCommunicationType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 26px;cursor:pointer;", onclick = "javascript:parent.communicationType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.CommunicationTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		communicationType.init();
	</script>
</asp:Content>

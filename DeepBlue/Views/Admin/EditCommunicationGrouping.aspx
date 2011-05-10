<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditCommunicationGroupingModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Communication Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("CommunicationGrouping.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateCommunicationGrouping", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "communicationGrouping.onCreateCommunicationGroupingBegin",
		 OnSuccess = "communicationGrouping.onCreateCommunicationGroupingSuccess"
	 }, new { @id = "AddNewCommunicationGrouping" })) {%>
	<div class="editor-label" style="width: 130px">
		<%: Html.LabelFor(model => model.CommunicationGroupingName) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.CommunicationGroupingName) %>
		<%: Html.ValidationMessageFor(model => model.CommunicationGroupingName) %>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 26px;", onclick = "return communicationGrouping.onSubmit('AddNewCommunicationGrouping');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 26px;cursor:pointer;", onclick = "javascript:parent.communicationGrouping.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.CommunicationGroupingId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		communicationGrouping.init();
	</script>
</asp:Content>

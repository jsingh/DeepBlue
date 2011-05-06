<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditIndustryModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditIndustry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("Industry.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateIndustry", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "industry.onIndustryBegin", OnSuccess = "industry.onIndustrySuccess" }, new { @id = "AddNewIndustry" })) {%>
	<div class="editor-label auto-width">
		<%: Html.LabelFor(model => model.Industry) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.Industry) %>
		<%: Html.ValidationMessageFor(model => model.Industry) %>
	</div>
	<div class="editor-label" style="width: 46px">
		<%: Html.LabelFor(model => model.Enabled)%>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Enabled, new { @style = "width:auto" })%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return industry.onSubmit('AddNewIndustry');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "javascript:parent.industry.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.IndustryId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		industry.init();
	</script>
</asp:Content>

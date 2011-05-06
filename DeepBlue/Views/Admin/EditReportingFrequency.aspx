<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditReportingFrequencyModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditReportingFrequency
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("ReportingFrequency.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateReportingFrequency", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "reportingFrequency.onReportingFrequencyBegin", OnSuccess = "reportingFrequency.onReportingFrequencySuccess" }, new { @id = "AddNewReportingFrequency" })) {%>
	<div class="editor-label auto-width">
		<%: Html.LabelFor(model => model.ReportingFrequency) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.ReportingFrequency) %>
		<%: Html.ValidationMessageFor(model => model.ReportingFrequency) %>
	</div>
	<div class="editor-label" style="width: 117px">
		<%: Html.LabelFor(model => model.Enabled)%>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Enabled, new { @style = "width:auto" })%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return reportingFrequency.onSubmit('AddNewReportingFrequency');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "javascript:parent.reportingFrequency.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.ReportingFrequencyId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		reportingFrequency.init();
	</script>
</asp:Content>

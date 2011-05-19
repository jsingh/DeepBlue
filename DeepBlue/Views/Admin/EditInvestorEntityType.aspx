<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditInvestorEntityTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditInvestorEntityType
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("InvestorEntityType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateInvestorEntityType", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "invEntityType.onCreateInvEnityTypeBegin", OnSuccess = "invEntityType.onCreateInvEnityTypeSuccess" }, new { @id = "AddNewInvEnityType" })) {%>
	<div class="editor-label auto-width">
		<%: Html.LabelFor(model => model.InvestorEntityTypeName) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.InvestorEntityTypeName) %>
		<%: Html.ValidationMessageFor(model => model.InvestorEntityTypeName) %>
	</div>
	<div class="editor-label" style="width: 143px">
		<%: Html.LabelFor(model => model.Enabled)%>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Enabled, new { @style = "width:auto" })%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return invEntityType.onSubmit('AddNewInvEnityType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.invEntityType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.InvestorEntityTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display:none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		invEntityType.init();
	</script>

</asp:Content>

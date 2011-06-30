<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditUnderlyingFundTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Underlying Fund Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("UnderlyingFundType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateUnderlyingFundType", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "underlyingFundType.onUnderlyingFundTypeBegin", OnSuccess = "underlyingFundType.onUnderlyingFundTypeSuccess" }, new { @id = "AddNewUnderlyingFundType" })) {%>
	<div class="editor-label auto-width">
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
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return underlyingFundType.onSubmit('AddNewUnderlyingFundType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.underlyingFundType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.UnderlyingFundTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display:none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		underlyingFundType.init();
	</script>

</asp:Content>

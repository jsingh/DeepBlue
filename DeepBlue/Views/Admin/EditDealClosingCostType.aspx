<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditDealClosingCostTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Deal Closing Cost Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("DealClosingCostType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateDealClosingCostType", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "dealClosingCostType.onCreateDealClosingCostTypeBegin",
		 OnSuccess = "dealClosingCostType.onCreateDealClosingCostTypeSuccess"
	 }, new { @id = "AddNewDealClosingCostType" })) {%>
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
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return dealClosingCostType.onSubmit('AddNewDealClosingCostType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "javascript:parent.dealClosingCostType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.DealClosingCostTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		dealClosingCostType.init();
	</script>
</asp:Content>

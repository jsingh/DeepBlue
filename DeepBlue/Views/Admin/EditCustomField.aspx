<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditCustomFieldModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditCustomField
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("CustomField.js")%>
	<%= Html.StylesheetLinkTag("customfield.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateCustomField", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "customField.onCreateCustomFieldBegin", OnSuccess = "customField.onCreateCustomFieldSuccess" }, new { @id = "AddNewCustomField" })) {%>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.CustomFieldText) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.CustomFieldText) %>
		<%: Html.ValidationMessageFor(model => model.CustomFieldText) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ModuleId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownListFor(model => model.ModuleId, Model.Modules, new { @onchange = "javascript:customField.changeModule(this);" })%>
		<%: Html.ValidationMessageFor(model => model.ModuleId) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.DataTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownListFor(model => model.DataTypeId,Model.DataTypes) %>
		<%: Html.ValidationMessageFor(model => model.DataTypeId) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.OptionalText) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.OptionalText) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Search) %>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Search, new { @style="width:auto" }) %>
	</div>
	<div id="OptionalFields" style="display: none">
		<%if (Model.OptionFields.Count > 0) {%>
		<table cellpadding="0" cellspacing="0" border="0">
			<thead>
				<tr>
					<th>
						Text
					</th>
					<th>
						Default
					</th>
					<th>
					</th>
				</tr>
			</thead>
			<%foreach (var field in Model.OptionFields) {%>
			<%Html.RenderPartial("EditOptionalField", field);%>
			<%}%>
		</table>
		<%}%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 26px;", onclick = "return customField.onSubmit('AddNewCustomField');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 26px;cursor:pointer;", onclick = "javascript:parent.customField.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.CustomFieldId) %>
	<% } %>
	<div id="UpdateTargetId" style="display:none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		customField.init();
	</script>

</asp:Content>

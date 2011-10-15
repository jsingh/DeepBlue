<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Helpers.CustomFieldModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{each(index,field) CustomField.Fields}} {{if index%3 && CustomField.IsDisplayTwoColumn}}
<div class="editor-label" style="clear: right;">
	{{else}}
	<div class="editor-label">
		{{/if}}
		<label>
			${field.CustomFieldText}</label>
	</div>
	<div class="editor-field">
		<%if (Model.IsDisplayMode) {%>
		<%: Html.Span("${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", new { @class = "show" })%>
		<%}%>
		{{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.Boolean%>}}
		<%: Html.CheckBox("CustomField_${field.CustomFieldId}", false, new { @class=(Model.IsDisplayMode ? "hide" : ""), @val = "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", style = "width:auto" })%>
		{{/if}} {{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.Text%>}}
		<%: Html.TextBox("CustomField_${field.CustomFieldId}", "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", new { @class = (Model.IsDisplayMode ? "hide" : "") })%>
		{{/if}} {{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.DateTime%>}}
		<%: Html.TextBox("CustomField_${field.CustomFieldId}", "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", new { @class = (Model.IsDisplayMode ? "hide" : "")  + " datefield" })%>
		{{/if}} {{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.Currency%>}}
		<%: Html.TextBox("CustomField_${field.CustomFieldId}", "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", new { @class = (Model.IsDisplayMode ? "hide" : ""), @onkeydown = "return jHelper.isCurrency(event);" })%>
		{{/if}} {{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.Integer%>}}
		<%: Html.TextBox("CustomField_${field.CustomFieldId}", "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", new { @class = (Model.IsDisplayMode ? "hide" : ""), @onkeydown = "return jHelper.isNumeric(event);" })%>
		{{/if}}
	</div>
{{/each}}
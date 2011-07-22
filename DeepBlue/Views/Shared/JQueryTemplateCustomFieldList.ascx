﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Helpers.CustomFieldModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{each(index,field) CustomField.Fields}} {{if index%3 && CustomField.DisplayTwoColumn}}
<div class="editor-label" style="clear: right">
	{{else}}
	<div class="editor-label">
		{{/if}}
		<label>
			${field.CustomFieldText}-</label>
	</div>
	<div class="editor-field">
		{{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.Boolean%>}}
		<%: Html.CheckBox("CustomField_${field.CustomFieldId}", false, new { @val = "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", style = "width:auto" })%>
		{{/if}} {{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.Text%>}}
		<%: Html.TextBox("CustomField_${field.CustomFieldId}", "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}")%>
		{{/if}} {{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.DateTime%>}}
		<%: Html.TextBox("CustomField_${field.CustomFieldId}", "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", new { @class = "datefield" })%>
		{{/if}} {{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.Currency%>}}
		<%: Html.TextBox("CustomField_${field.CustomFieldId}", "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
		{{/if}} {{if field.DataTypeId ==
		<%=(int)DeepBlue.Models.Admin.Enums.CustomFieldDataType.Integer%>}}
		<%: Html.TextBox("CustomField_${field.CustomFieldId}", "${getCustomFieldValue(CustomField.Values,field.CustomFieldId,field.DataTypeId)}", new { @onkeypress = "return jHelper.isNumeric(event);" })%>
		{{/if}}
	</div>
	<%--<% int index = 0;
	foreach (var item in Model.Fields) {
		DeepBlue.Helpers.CustomFieldValueDetail customFieldValue = Model.Values.SingleOrDefault(fieldValue => fieldValue.CustomFieldId == item.CustomFieldId);
		if (customFieldValue == null)
			customFieldValue = new DeepBlue.Helpers.CustomFieldValueDetail();
	%>
	<%if ((index % 2) > 0 && Model.DisplayTwoColumn) {%>
	<div class="editor-label" style="clear: right">
		<%}
   else {%>
		<div class="editor-label">
			<%}%>
			<%: Html.Label(item.CustomFieldText + ":") %>
		</div>
		<div class="editor-field">
			<% switch ((DeepBlue.Models.Admin.Enums.CustomFieldDataType)item.DataTypeId) {%>
			<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.MultiSelectOpiton:%>
			<%break;%>
			<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.SingleSelectOption:%>
			<%break;%>
			<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.Boolean:%>
			<%: Html.CheckBox("CustomField_" + item.CustomFieldId.ToString(),false, new {  style = "width:auto" })%>
			<%break;%>
			<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.Text:%>
			<%: Html.TextBox("CustomField_" + item.CustomFieldId.ToString(), customFieldValue.TextValue)%>
			<%break;%>
			<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.DateTime:%>
			<%: Html.TextBox("CustomField_" + item.CustomFieldId.ToString(), customFieldValue.DateValue, new {  @id = "CustomField_" + item.CustomFieldId.ToString() })%>
			<%if (Model.InitializeDatePicker) {%>
			<%=Html.jQueryDatePicker("CustomField_" + item.CustomFieldId.ToString())%>
			<%}%>
			<%break;%>
			<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.Currency:%>
			<%: Html.TextBox("CustomField_" + item.CustomFieldId.ToString(),  (customFieldValue.CurrencyValue > 0 ? customFieldValue.CurrencyValue.ToString("0.00") : string.Empty), new { @onkeypress = "return jHelper.isCurrency(event);" })%>
			<%break;%>
			<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.Integer:%>
			<%: Html.TextBox("CustomField_" + item.CustomFieldId.ToString(), (customFieldValue.IntegerValue > 0 ? customFieldValue.IntegerValue.ToString() : string.Empty), new { @onkeypress = "return jHelper.isNumeric(event);" })%>
			<%break;%>
			<%}%>
		</div>
		<% index++;
   } %>--%>
{{/each}}
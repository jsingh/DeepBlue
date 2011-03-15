<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.CustomFieldModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% foreach (var item in Model.Fields) {
	   DeepBlue.Models.Investor.CustomFieldValueDetail customFieldValue = Model.Values.SingleOrDefault(fieldValue => fieldValue.CustomFieldId == item.CustomFieldID);
	   if (customFieldValue == null)
		   customFieldValue = new DeepBlue.Models.Investor.CustomFieldValueDetail();
%>
<div class="editor-label">
	<%: Html.Label(item.CustomFieldText + ":") %>
</div>
<div class="editor-field">
	<% switch ((DeepBlue.Models.Admin.Enums.CustomFieldDataType)item.DataTypeID) {%>
	<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.MultiSelectOpiton:%>
	<%break;%>
	<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.SingleSelectOption:%>
	<%break;%>
	<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.Boolean:%>
	<%: Html.CheckBox("CustomField_" + item.CustomFieldID.ToString(),customFieldValue.BooleanValue ?? false, new { style = "width:auto" })%>
	<%break;%>
	<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.Text:%>
	<%: Html.TextBox("CustomField_" + item.CustomFieldID.ToString(), customFieldValue.TextValue)%>
	<%break;%>
	<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.DateTime:%>
	<%: Html.TextBox("CustomField_" + item.CustomFieldID.ToString(), customFieldValue.DateValue, new { @id = item.CustomFieldID.ToString() })%>
	<%=Html.jQueryDatePicker(item.CustomFieldID.ToString())%>
	<%break;%>
	<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.Currency:%>
	<%: Html.TextBox("CustomField_" + item.CustomFieldID.ToString(), customFieldValue.CurrencyValue)%>
	<%break;%>
	<%case DeepBlue.Models.Admin.Enums.CustomFieldDataType.Integer:%>
	<%: Html.TextBox("CustomField_" + item.CustomFieldID.ToString(), customFieldValue.IntegerValue)%>
	<%break;%>
	<%}%>
</div>
<% } %>

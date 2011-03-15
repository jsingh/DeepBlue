<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Admin.EditOptionFieldModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr>
	<td>
		<%: Html.Hidden(Model.Index.ToString() + "_" + "OptionFieldId",Model.OptionFieldId) %>
		<%: Html.Hidden(Model.Index.ToString() + "_" + "CustomFieldId", Model.CustomFieldId)%>
		<%: Html.TextBox(Model.Index.ToString() + "_" + "OptionText", Model.OptionText)%>
	</td>
	<td>
		<%: Html.CheckBox(Model.Index.ToString() + "_" + "IsDefault", Model.IsDefault)%>
	</td>
	<td>
		<%: Html.Image("Delete.png", new {  @onclick = "javascript:customField.deleteOptionalField(" + Model.OptionFieldId.ToString() + ",this);" })%>
	</td>
</tr>

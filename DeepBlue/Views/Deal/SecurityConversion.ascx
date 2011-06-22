<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.SecurityConversionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<table cellspacing="20" cellpadding="0" border="0">
	<tr>
		<td>
			<%: Html.LabelFor(model => model.OldSecurityId)%>
		</td>
		<td>
			<%: Html.TextBox("OldSecurity", "Direct Name", new { @id = "OldSecurity", @class = "wm" })%>
		</td>
		<td>
			<%: Html.LabelFor(model => model.OldSecurityTypeId)%>
		</td>
		<td>
			<%: Html.DropDownListFor(model => model.OldSecurityTypeId, Model.SecurityTypes, new { @onchange = "javascript:dealActivity.changeOldSecurityType(this);" })%>
		</td>
	</tr>
	<tr>
		<td>
			<%: Html.LabelFor(model => model.NewSecurityId)%>
		</td>
		<td>
			<%: Html.TextBox("NewSecurity", "Direct Name", new { @id = "NewSecurity", @class = "wm" })%>
		</td>
		<td>
			<%: Html.LabelFor(model => model.NewSecurityTypeId)%>
		</td>
		<td>
			<%: Html.DropDownListFor(model => model.NewSecurityTypeId, Model.SecurityTypes, new { @onchange = "javascript:dealActivity.changeNewSecurityType(this);" })%>
		</td>
	</tr>
	<tr>
		<td>
			<%: Html.LabelFor(model => model.NewSymbol)%>
		</td>
		<td>
			<%: Html.Span("Symbol", new { @id = "SpnNewSymbol", @style = "width:200px;" })%>
		</td>
		<td>
			<%: Html.LabelFor(model => model.SplitFactor) %>
		</td>
		<td>
			<%: Html.TextBoxFor(model => model.SplitFactor) %>&nbsp;Stocks
		</td>
	</tr>
	<tr>
		<td>
		</td>
		<td>
		</td>
		<td>
			<%: Html.ImageButton("Save.png", new { @class="default-button" })%>
		</td>
		<td>
			<%: Html.Span("", new { id = "SpnSecCoversionLoading" })%>
		</td>
	</tr>
</table>
<%: Html.HiddenFor(model => model.OldSecurityId)%>
<%: Html.HiddenFor(model => model.NewSecurityId)%>

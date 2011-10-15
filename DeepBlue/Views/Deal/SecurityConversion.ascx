<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.SecurityConversionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<table cellspacing="15" cellpadding="0" border="0">
	<tr>
		<td>
			<%: Html.LabelFor(model => model.OldSecurityTypeId)%>
		</td>
		<td>
			<%: Html.DropDownListFor(model => model.OldSecurityTypeId, Model.SecurityTypes, new { @onchange = "javascript:dealActivity.changeOldSecurityType(this);" })%>
		</td>
		<td>
			<%: Html.LabelFor(model => model.OldSecurityId)%>
		</td>
		<td>
			<%: Html.TextBox("OldSecurity", "Direct Name", new { @id = "OldSecurity", @class = "wm" })%>
		</td>
	</tr>
	<tr>
		<td>
			<%: Html.LabelFor(model => model.NewSecurityTypeId)%>
		</td>
		<td>
			<%: Html.DropDownListFor(model => model.NewSecurityTypeId, Model.SecurityTypes, new { @onchange = "javascript:dealActivity.changeNewSecurityType(this);" })%>
		</td>
		<td>
			<%: Html.LabelFor(model => model.NewSecurityId)%>
		</td>
		<td>
			<%: Html.TextBox("NewSecurity", "Direct Name", new { @id = "NewSecurity", @class = "wm" })%>
		</td>
	</tr>
	<tr>
		<td>
			<%: Html.Span(Html.LabelFor(model => model.NewSymbol).ToHtmlString(), new { @id = "SpnNewSymbollbl", @style="display:none;" })%>
		</td>
		<td>
			<%: Html.Span("", new { @id = "SpnNewSymbol", @style = "width:200px;display:none;" })%>
		</td>
		<td>
			<%: Html.LabelFor(model => model.SplitFactor) %>
		</td>
		<td>
			<%: Html.TextBoxFor(model => model.SplitFactor, new { @style = "width:216px" })%>&nbsp;Stocks
		</td>
	</tr>
	<tr>
		<td>
		</td>
		<td>
		</td>
		<td>
			<%: Html.LabelFor(model => model.ConversionDate) %>
		</td>
		<td>
			<%: Html.EditorFor(model => model.ConversionDate)%>
		</td>
	</tr>
	<tr>
		<td>
		</td>
		<td>
		</td>
		<td>
			<%: Html.ImageButton("Save_active.png", new { @class="default-button" })%>
		</td>
		<td>
			<%: Html.Span("", new { id = "SpnSecCoversionLoading" })%>
		</td>
	</tr>
</table>
<%: Html.HiddenFor(model => model.OldSecurityId)%>
<%: Html.HiddenFor(model => model.NewSecurityId)%>

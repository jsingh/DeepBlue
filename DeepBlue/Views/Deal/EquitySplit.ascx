<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.EquitySplitModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<table cellpadding="0" cellspacing="20" border="0">
	<tr>
		<td>
			<%: Html.LabelFor(model => model.EquityId)%>
		</td>
		<td>
			<%: Html.TextBox("SplitEquityName", "Direct Name", new { @id = "SplitEquityName", @style="width:200px", @class = "wm" })%>
			<%: Html.HiddenFor(model => model.EquityId) %>
		</td>
		<td>
			<%: Html.LabelFor(model => model.SecurityTypeId)%>
		</td>
		<td>
			<%: Html.Span("", new { @id = "SpnSecurityType" })%>
			<%: Html.DropDownListFor(model => model.SecurityTypeId, Model.SecurityTypes, new { @style = "display:none;" })%>
		</td>
	</tr>
	<tr>
		<td>
			<%: Html.LabelFor(model => model.Symbol)%>
		</td>
		<td>
			<%: Html.Span("", new { @id = "SpnSymbol", @style="width:200px;" })%>
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
			<%: Html.Span("", new { id = "SpnEquitySplitLoading" })%>
		</td>
	</tr>
</table>
 
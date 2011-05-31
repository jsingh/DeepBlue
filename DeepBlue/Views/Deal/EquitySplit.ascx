<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.EquitySplitModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="editor-label">
	<%: Html.LabelFor(model => model.EquityId)%>
</div>
<div class="editor-field">
	<div class="cell" style="width: 195px; padding: 0px;">
		<%: Html.TextBox("SplitEquityName", "Direct Name", new { @id = "SplitEquityName", @class = "wm" })%>
		<%: Html.HiddenFor(model => model.EquityId) %>
	</div>
	<div class="cell" style="width: 65px; padding: 0px;">
		<%: Html.LabelFor(model => model.SecurityTypeId)%></div>
	<div class="cell">
		<%: Html.Span("", new { @id = "SpnSecurityType" })%>
		<%: Html.DropDownListFor(model => model.SecurityTypeId, Model.SecurityTypes, new { @style = "display:none;" })%>
	</div>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.Symbol)%>
</div>
<div class="editor-field">
	<div class="cell" style="width: 195px; padding: 0px;">
		<%: Html.Span("", new { @id = "SpnSymbol", @style="width:200px;" })%>
	</div>
	<div class="cell" style="width: 65px; padding: 0px;">
		<%: Html.LabelFor(model => model.SplitFactor) %></div>
	<div class="cell">
		<%: Html.TextBoxFor(model => model.SplitFactor) %>&nbsp;Stocks
	</div>
</div>
<div class="clear" style="margin-left: 200px; float: left;">
	<div class="cell auto">
		<%: Html.ImageButton("Save.png", new { @class="default-button" })%></div>
	<div class="cell auto">
		<%: Html.Span("", new { id = "SpnEquitySplitLoading" })%>
	</div>
</div>
 

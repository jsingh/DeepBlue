<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.SecurityConversionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="editor-label">
	<%: Html.LabelFor(model => model.OldSecurityId)%>
</div>
<div class="editor-field">
	<div class="cell" style="width: 195px; padding: 0px;">
		<%: Html.TextBox("OldSecurity", "Direct Name", new { @id = "OldSecurity", @class = "wm" })%>
	</div>
	<div class="cell auto" style="width: 92px; padding: 0px;">
		<%: Html.LabelFor(model => model.OldSecurityTypeId)%>
	</div>
	<div class="cell auto">
		<%: Html.DropDownListFor(model => model.OldSecurityTypeId, Model.SecurityTypes, new { @onchange = "javascript:dealActivity.changeOldSecurityType(this);" })%>
	</div>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.NewSecurityId)%>
</div>
<div class="editor-field">
	<div class="cell" style="width: 195px; padding: 0px;">
		<%: Html.TextBox("NewSecurity", "Direct Name", new { @id = "NewSecurity", @class = "wm" })%>
	</div>
	<div class="cell" style="padding: 0px;">
		<%: Html.LabelFor(model => model.NewSecurityTypeId)%>
	</div>
	<div class="cell auto">
		<%: Html.DropDownListFor(model => model.NewSecurityTypeId, Model.SecurityTypes, new { @onchange = "javascript:dealActivity.changeNewSecurityType(this);" })%>
	</div>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.NewSymbol)%>
</div>
<div class="editor-field">
	<div class="cell" style="width: 195px; padding: 0px;">
		<%: Html.Span("Symbol", new { @id = "SpnNewSymbol", @style = "width:200px;" })%>
	</div>
	<div class="cell" style="width: 92px; padding: 0px;">
		<%: Html.LabelFor(model => model.SplitFactor) %></div>
	<div class="cell auto">
		<%: Html.TextBoxFor(model => model.SplitFactor) %>&nbsp;Stocks
	</div>
</div>
<div class="clear" style="margin-left: 200px; float: left;">
	<div class="cell auto">
		<%: Html.ImageButton("Save.png", new { @class="default-button" })%></div>
	<div class="cell auto">
		<%: Html.Span("", new { id = "SpnSecCoversionLoading" })%>
	</div>
</div>
<%: Html.HiddenFor(model => model.OldSecurityId)%>
<%: Html.HiddenFor(model => model.NewSecurityId)%>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.EquityDetailModel>" %>
<div id="equitysymboldiv">
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Symbol) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("EQ_Symbol", "${Symbol}")%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.ISINO) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("ISINO", "${ISINO}") %>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.Seniority) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Seniority", "${Seniority}")%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Public) %>
	</div>
	<div class="editor-field">
		<%: Html.CheckBox("Public", false, new { @val = "${Public}", @id = "Public" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.IndustryId) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("EQ_Industry", "${Industry}", new { @id = "EQ_Industry", @style = "width:157px;" })%>
		<%: Html.Hidden("EQ_IndustryId", "${IndustryId}")%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.CurrencyId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("EQ_CurrencyId", Model.Currencies, new { @id = "Currency", @val = "${CurrencyId}" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ShareClassTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("ShareClassTypeId", Model.ShareClassTypes, new { @id = "ShareClassType", @val = "${ShareClassTypeId}" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.EquityTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("EquityTypeId", Model.EquityTypes, new { @id = "EquityType", @val = "${EquityTypeId}" })%>
	</div>
	<%: Html.Hidden("EquityId", "${EquityId}") %>
</div>

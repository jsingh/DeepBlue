<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.EquityDetailModel>" %>
<%: Html.HiddenFor(model => model.EquityId) %>
<%: Html.HiddenFor(model => model.IssuerId) %>

<div id="equitysymboldiv" >
<table cellpadding="0" cellspacing="0" width="100%" >
 <tr>
  <td>
  <div class="line"></div>
  <div class="editor-label">
	<%: Html.LabelFor(model => model.Symbol) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Symbol) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.ISINO) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.ISINO) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Seniority) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Seniority) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.Public) %>
</div>
<div class="editor-field">
	<%: Html.CheckBoxFor(model => model.Public) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.IndustryId) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("Industry", "${Industry}")%>
	<%: Html.HiddenFor(model => model.IndustryId)%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.CurrencyId) %>
</div>
<div class="editor-field">
	<%: Html.DropDownList("CurrencyId", Model.Currencies)%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.ShareClassTypeId) %>
</div>
<div class="editor-field">
	<%: Html.DropDownListFor(model => model.ShareClassTypeId, Model.ShareClassTypes) %>
</div>
<div class="editor-label" style="clear:right">
	<%: Html.LabelFor(model => model.EquityTypeId) %>
</div>
<div class="editor-field">
	<%: Html.DropDownListFor(model => model.EquityTypeId, Model.EquityTypes)%>
</div>
  <div class="line"></div>
  </td>
 </tr>
</table>
</div>

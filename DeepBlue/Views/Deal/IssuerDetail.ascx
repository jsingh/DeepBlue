<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.IssuerDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%: Html.Hidden("IssuerId", "${IssuerId}")%>
<%: Html.Hidden("CountryId", "${CountryId}")%>
<div class="editor-label">
	<%: Html.LabelFor(model => model.Name)%>
</div>
<div class="editor-field">
	<%: Html.TextBox("Name", "${Name}", new { @class = "wm" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.ParentName)%>
</div>
<div class="editor-field">
	<%: Html.TextBox("ParentName", "${ParentName}", new { @class = "wm" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.CountryId)%>
</div>
<div class="editor-field">
	<%: Html.TextBox("Country", "${Country}", new { @id="I_Country", @class = "wm" })%>
</div>

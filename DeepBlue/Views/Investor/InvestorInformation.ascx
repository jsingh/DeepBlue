<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.EditModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @onsubmit = "return false" })) {%>
<div class="editor-row">
	<div class="editor-editbtn">
		<div class="EditInvestorInfo show" style="float: left">
			<%: Html.Anchor(Html.Image("Editbtn_active.png", new { @title = "Edit" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
		</div>
		<div class="UpdateInvestorInfo hide" style="float: left; display: none;">
			<%: Html.Span("", new { @id = "Loading" })%>
			<%: Html.Anchor(Html.Image("Update_active.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.saveInvestorDetail(this);" })%>&nbsp;&nbsp;
			<%: Html.Anchor(Html.Image("Cancel_active.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
		</div>
	</div>
</div>
<%: Html.Hidden("InvestorId", "${InvestorId}")%>
<div class="editor-label">
	<%: Html.LabelFor(model => model.SocialSecurityTaxId)%>
</div>
<div class="editor-field">
	<%: Html.Span("${SocialSecurityTaxId}", new { id = "SocialSecurityTaxId" })%>
</div>
<div class="editor-label">
	<%: Html.Label("State of Residency")%>
</div>
<div class="editor-field dropdown">
	<%: Html.Span("${StateOfResidencyName}", new { @id = "Disp_StateOfResidency", @class = "show" })%>
	<%: Html.TextBox("StateOfResidencyName", "${StateOfResidencyName}", new { @class = "hide" })%>
	<%: Html.Hidden("StateOfResidency", "${StateOfResidency}")%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.DomesticForeign) %>
</div>
<div class="editor-field dropdown">
	<%: Html.Span("${DomesticForeignName}", new { @id = "Disp_DomesticForeigns", @class = "show" })%>
	<%: Html.DropDownList("DomesticForeign", Model.SelectList.DomesticForeigns, new { @class = "hide", @val = "${DomesticForeign}" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.EntityType) %>
</div>
<div class="editor-field dropdown">
	<%: Html.Span("${EntityTypeName}", new { @id = "Disp_EntityType", @class = "show" })%>
	<%: Html.DropDownList("EntityType", Model.SelectList.InvestorEntityTypes, new { @class = "hide", @val = "${EntityType}" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.DisplayName) %>
</div>
<div class="editor-field dropdown">
	<%: Html.Span("${DisplayName}", new { @id = "Disp_DisplayName", @class = "show" })%>
	<%: Html.TextBox("DisplayName","${DisplayName}", new { @class = "hide" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.Notes) %>
</div>
<div class="editor-field">
	<%: Html.Span("${formatEditor(Notes)}", new { @id = "Disp_Notes", @class = "notes show" })%>
	<%=Html.jQueryTemplateTextArea("Notes", "${Notes}", 4, 28, new { @class = "hide" })%>
</div>
<% Html.RenderPartial("JQueryTemplateCustomFieldList", Model.CustomField);%>
<%}%>

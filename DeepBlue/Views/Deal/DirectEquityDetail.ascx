<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.EquityDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="direct-det">
<div id="equitysymboldiv">
	<div class="editor-label">
		<%: Html.LabelFor(model => model.EquitySymbol)%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("EquitySymbol", "${EquitySymbol}")%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.EquityISINO) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("EquityISINO", "${EquityISINO}")%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.EquityCurrencyId)%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("EquityCurrencyId", Model.Currencies, new { @id = "EquityCurrencyId", @val = "${CurrencyId}" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Public) %>
	</div>
	<div class="editor-field">
		<%: Html.CheckBox("Public", false, new { @val = "${Public}", @id = "Public" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.EquityIndustryId)%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("EquityIndustry", "${EquityIndustry}", new { @id = "EquityIndustry"  })%>
		<%: Html.Hidden("EquityIndustryId", "${EquityIndustryId}", new { @id = "EquityIndustryId" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.ShareClassTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("ShareClassTypeId", Model.ShareClassTypes, new { @id = "ShareClassType", @val = "${ShareClassTypeId}" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.EquityTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("EquityTypeId", Model.EquityTypes, new { @id = "EquityType", @val = "${EquityTypeId}" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.EquityComments) %>
	</div>
	<div class="editor-field">
		<%: Html.TextArea("EquityComments","${EquityComments}", 5, 50, new { })%>
	</div>
	<%: Html.Hidden("EquityId", "${EquityId}") %>
</div>
</div>
<div class="line">
</div>
<br />
<div class="line">
</div>
<div class="direct-det">
<div id="eqdocument">
	<div class="editor-label">
		<%: Html.Label("Document Type") %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("EquityDocumentTypeId", Model.DocumentTypes)%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Document Date") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("EquityDocumentDate", "${EquityDocumentDate}", new { @class = "datefield", @id = "EquityDocumentDate" })%>
	</div>
	<div class="editor-label">
		<%: Html.DropDownList("EquityUploadTypeId", Model.UploadTypes, new { @style = "width:80px", @onchange = "javascript:dealDirect.changeUploadType(this,'equitysymboldiv');" })%>
	</div>
	<div id="FileRow" class="editor-field" style="width: auto;">
		<div class="cell" style="padding: 0; margin: 0; width: auto;">
			<%: Html.File("EquityFile", new { @id = "equityFileToUpload" })%>
			<%: Html.Hidden("EquityFileId")%>
		</div>
		<div class="cell" style="padding: 0; margin: 0;">
			<%: Html.Span("", new { @id = "SpnEquityDocLoading" })%>
		</div>
	</div>
	<div id="LinkRow" style="display: none; width: auto;" class="editor-field">
		<%: Html.TextBox("EquityFilePath")%>
	</div>
</div>
</div>
<div class="line">
</div>

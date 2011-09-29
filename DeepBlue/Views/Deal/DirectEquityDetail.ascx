<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.EquityDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="direct-det">
	<div id="equitysymboldiv">
		<div class="editor-label-first">
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
			<%: Html.DropDownList("EquityCurrencyId", Model.Currencies, new { @id = "EquityCurrencyId", @val = "${CurrencyId}", @refresh="true", @action="Currency" })%>
		</div>
		<div class="editor-label-first">
			<%: Html.LabelFor(model => model.EquitySecurityTypeId)%>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("EquitySecurityTypeId", Model.EquitySecurityTypes, new { @id = "EquitySecurityTypeId", @val = "${EquitySecurityTypeId}" })%>
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
			<%: Html.DropDownList("ShareClassTypeId", Model.ShareClassTypes, new { @id = "ShareClassType", @val = "${ShareClassTypeId}", @refresh = "true", @action = "ShareClassType" })%>
		</div>
		<div class="editor-label-first">
			<%: Html.LabelFor(model => model.EquityTypeId) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("EquityTypeId", Model.EquityTypes, new { @id = "EquityType", @val = "${EquityTypeId}", @refresh = "true", @action = "EquityType" })%>
		</div>
		<div class="editor-label-first">
			<%: Html.LabelFor(model => model.EquityComments) %>
		</div>
		<div class="editor-field">
			<%: Html.TextArea("EquityComments","${EquityComments}", 5, 50, new { })%>
		</div>
		<%: Html.Hidden("EquityId", "${EquityId}") %>
	</div>
</div>
<div class="direct-det" style="display:none;">
	<div class="line">
	</div>
	<br />
	<div class="line">
	</div>
	<div id="eqdocument">
		<div class="editor-label-first">
			<%: Html.Label("Document Type") %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("EquityDocumentTypeId", Model.DocumentTypes, new { @val = "0" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("Document Date") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("EquityDocumentDate", "${EquityDocumentDate}", new { @class = "datefield", @id = "EquityDocumentDate" })%>
		</div>
		<div class="editor-label" style="width:143px;padding-right:11px;">
			<%: Html.DropDownList("EquityUploadTypeId", Model.UploadTypes, new { @val = "1", @style = "width:85px", @onchange = "javascript:dealDirect.changeUploadType(this,'eqdocument');" })%>
		</div>
		<div id="FileRow" class="editor-field" style="width: auto;">
			<div style="padding: 0; margin: 0; width: auto; float: left;">
				<%: Html.File("EquityFile", new { @id = "equityFileToUpload" })%>
				<%: Html.Hidden("EquityFileId")%>
			</div>
			<div style="padding: 0; margin: 0; float: left;">
				<%: Html.Span("", new { @id = "SpnEquityDocLoading" })%>
			</div>
		</div>
		<div id="LinkRow" style="display: none; width: auto;" class="editor-field">
			<%: Html.TextBox("EquityFilePath", "", new { @style = "width:250px" })%>
		</div>
	</div>
</div>
<div class="line">
</div>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.EquityDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if EquityId>0}}
<%using (Html.Form(new { @class = "frm-equity frm-security", @id = "frm_${EquityId}_Equity", @onsubmit = "return false" })) {%>
{{/if}}
<div class="direct-det">
	<div id="equitysymboldiv">
		<div class="editor-label">
			<%: Html.LabelFor(model => model.EquitySymbol)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("EquitySymbol", "${EquitySymbol}", new { @class = "hide" })%>
			<%: Html.Span("${EquitySymbol}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.EquitySecurityTypeId)%>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("EquitySecurityTypeId", Model.EquitySecurityTypes, new { @class = "hide", @id = "EquitySecurityTypeId", @val = "${EquitySecurityTypeId}" })%>
			<%: Html.Span("${EquitySecurityType}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.EquityTypeId) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("EquityTypeId", Model.EquityTypes, new { @class = "hide", @id = "EquityType", @val = "${EquityTypeId}", @refresh = "true", @action = "EquityType" })%>
			<%: Html.Span("${EquityType}", new { @class = "show" })%>
		</div>
		<div class="line">
		</div>
		<br /> 
		<div class="editor-label">
			<%: Html.LabelFor(model => model.EquityCurrencyId)%>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("EquityCurrencyId", Model.Currencies, new { @class = "hide", @id = "EquityCurrencyId", @val = "${EquityCurrencyId}", @refresh = "true", @action = "Currency" })%>
			<%: Html.Span("${EquityCurrency}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.EquityISINO) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("EquityISINO", "${EquityISINO}", new { @class = "hide" })%>
			<%: Html.Span("${EquityISINO}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.EquityIndustryId)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("EquityIndustry", "${EquityIndustry}", new { @class = "hide", @id = "EquityIndustry"  })%>
			<%: Html.Hidden("EquityIndustryId", "${EquityIndustryId}", new { @id = "EquityIndustryId" })%>
			<%: Html.Span("${EquityIndustry}", new { @class = "show" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.ShareClassTypeId) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("ShareClassTypeId", Model.ShareClassTypes, new { @class = "hide", @id = "ShareClassType", @val = "${ShareClassTypeId}", @refresh = "true", @action = "ShareClassType" })%>
			<%: Html.Span("${ShareClassType}", new { @class = "show" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.EquityComments) %>
		</div>
		<div class="editor-field">
			<%=Html.jQueryTemplateTextArea("EquityComments","${EquityComments}", 5, 50, new { @class = "hide" })%>
			<%: Html.Span("${EquityComments}", new { @class = "show" })%>
		</div>
		<%: Html.Hidden("EquityId", "${EquityId}") %>
	</div>
</div>
<div class="direct-det">
	<div class="line">
	</div>
	<div id="eqdocument">
		<div class="editor-label">
			<%: Html.Label("Document Type") %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("EquityDocumentTypeId", Model.DocumentTypes, new { @val = "0", @refresh = "true", @action = "DocumentType" })%>
		</div>
		<div class="editor-label" style="clear: right">
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
		<div style="width: 900px; clear: both; float: left; margin-left: 57px;">
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" border="0" class="grid" id="DocumentList">
				<thead>
					<tr>
						<th style="display: none">
							ID
						</th>
						<th style="width: 30%">
							Document Type
						</th>
						<th style="width: 20%;">
							Document Date
						</th>
						<th style="width: 30%">
							File Name
						</th>
						<th align="right">
						</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="direct-det">
	<div style="float: right; clear: both; margin-right: 135px">
		{{if EquityId>0}}
		<div class="button-box">
			<div class="editor-label button" style="float: right; width: auto;">
				<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:dealDirect.cancel(this);" })%>
			</div>
			<div class="editor-label show button" style="float: right; width: auto;">
				<%: Html.Image("Editbtn_active.png", new { @onclick = "javascript:dealDirect.edit(this);" })%>
			</div>
			<div class="editor-label hide" style="float: right; width: auto;">
				<%: Html.Image("Modify-Direct_active.png", new { @onclick = "javascript:dealDirect.modifyEquity(this);" })%>
			</div>
			<div class="editor-label" style="float: right; width: auto;">
				<%: Html.Span("", new { @id = "SpnLoading" })%>
			</div>
		</div>
		<%: Html.Hidden("IssuerId", "${IssuerId}")%>
		{{/if}}</div>
</div>
{{if EquityId>0}}
<%}%>
{{/if}}
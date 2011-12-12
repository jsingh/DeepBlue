<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.FixedIncomeDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if FixedIncomeId>0}}
<%using (Html.Form(new { @class = "frm-fixedincome", @id = "frm_${FixedIncomeId}_FixedIncome", @onsubmit = "return false" })) {%>
{{/if}}
<div class="direct-det">
	<div id="fixincomediv">
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FaceValue)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FaceValue", "${FaceValue}", new { @class="hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
			<%: Html.Span("${FaceValue}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FixedIncomeISINO)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FixedIncomeISINO", "${FixedIncomeISINO}", new { @class = "hide" })%>
			<%: Html.Span("${FixedIncomeISINO}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.Maturity)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Maturity", "${formatDate(Maturity)}", new { @class = "datefield hide", @id = "FI_Maturity" })%>
			<%: Html.Span("${formatDate(Maturity)}", new { @class = "show" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.IssuedDate)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("IssuedDate", "${formatDate(IssuedDate)}", new { @class = "datefield hide", @id = "FI_IssuedDate" })%>
			<%: Html.Span("${formatDate(IssuedDate)}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.CouponInformation)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("CouponInformation", "${CouponInformation}", new { @class = "hide" })%>
			<%: Html.Span("${CouponInformation}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FixedIncomeCurrencyId)%>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("FixedIncomeCurrencyId", Model.Currencies, new { @class = "hide", @val = "${FixedIncomeCurrencyId}" })%>
			<%: Html.Span("${FixedIncomeCurrency}", new { @class = "show" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.Frequency)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Frequency", "${Frequency}", new { @class = "hide", @onkeydown = "return jHelper.isNumeric(event);" })%>
			<%: Html.Span("${Frequency}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FirstCouponDate)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FirstCouponDate", "${formatDate(FirstCouponDate)}", new { @class = "datefield hide", @id = "FI_FirstCouponDate" })%>
			<%: Html.Span("${formatDate(FirstCouponDate)}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FirstAccrualDate)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FirstAccrualDate", "${formatDate(FirstAccrualDate)}", new { @class = "datefield hide", @id = "FI_FirstAccrualDate" })%>
			<%: Html.Span("${formatDate(FirstAccrualDate)}", new { @class = "show" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FixedIncomeTypeId)%>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("FixedIncomeTypeId", Model.FixedIncomeTypes, new { @class = "hide", @val = "${FixedIncomeTypeId}", @refresh = "true", @action = "FixedIncomeType" })%>
			<%: Html.Span("${FixedIncomeType}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FixedIncomeIndustryId)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FixedIncomeIndustry", "${FixedIncomeIndustry}", new { @class="hide", @id = "FixedIncomeIndustry" })%>
			<%: Html.Hidden("FixedIncomeIndustryId", "${FixedIncomeIndustryId}")%>
			<%: Html.Span("${FixedIncomeIndustry}", new { @class = "show" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FixedIncomeSymbol)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FixedIncomeSymbol", "${FixedIncomeSymbol}", new { @class = "hide" })%>
			<%: Html.Span("${FixedIncomeSymbol}", new { @class = "show" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FixedIncomeComments)%>
		</div>
		<div class="editor-field">
			<%=Html.jQueryTemplateTextArea("FixedIncomeComments", "${FixedIncomeComments}", 5, 50, new { @class = "hide" })%>
			<%: Html.Span("${FixedIncomeComments}", new { @class = "show" })%>
		</div>
		<%: Html.Hidden("FixedIncomeId", "${FixedIncomeId}")%>
	</div>
</div>
<div class="line">
</div>
<div class="direct-det">
	<div id="fidocument">
		<div class="editor-label">
			<%: Html.Label("Document Type")%>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("FixedIncomeDocumentTypeId", Model.DocumentTypes, new { @val = "0", @refresh = "true", @action = "DocumentType" })%>
		</div>
		<div class="editor-field" style="clear: righ; width: auto;">
			<%: Html.DropDownList("FixedIncomeUploadTypeId", Model.UploadTypes, new { @val = "1", @style = "width:100px", @onchange = "javascript:dealDirect.changeUploadType(this,'fidocument');" })%>
		</div>
		<div id="FileRow" class="editor-field" style="width: auto;">
			<div style="padding: 0; margin: 0; width: auto; float: left;">
				<%: Html.File("FixedIncomeFile", new { @id = "equityFileToUpload" })%>
				<%: Html.Hidden("FixedIncomeFileId")%>
			</div>
			<div style="padding: 0; margin: 0; float: left;">
				<%: Html.Span("", new { @id = "SpnFixedIncomeDocLoading" })%>
			</div>
		</div>
		<div id="LinkRow" style="display: none; width: auto; margin: 0" class="editor-field">
			<%: Html.TextBox("FixedIncomeFilePath", "", new { @style = "width:250px" })%>
		</div>
		{{if Documents.length > 0}}
		<div class="editor-label">&nbsp;
		</div>
		<div class="editor-field" style="width: 500px;">
			<table cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th>
							Document Type
						</th>
						<th>
							File Name
						</th>
					</tr>
				</thead>
				<tbody>
					{{each(f,file) Documents}}
					<tr id="Row${file.DocumentID}" {{if f%2>0}}class="erow"{{/if}}>
						<td>
							${file.DocumentTypeName}
						</td>
						<td>
							${file.DocumentName}
						</td>
					</tr>
					{{/each}}
				</tbody>
			</table>
		</div>
		{{/if}}
	</div>
</div>
{{if FixedIncomeId>0}}
<div class="line"></div>
<div class="direct-det"  style="width:90%;">
	<%: Html.Hidden("IssuerId", "${IssuerId}")%>
	<div class="editor-label" style="float: right;width:auto;">
		<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:dealDirect.cancel(this);" })%>
	</div>
	<div class="editor-label show" style="float: right;width:auto;">
		<%: Html.Image("Editbtn_active.png", new { @onclick = "javascript:dealDirect.edit(this);" })%>
	</div>
	<div class="editor-label hide" style="float: right;width:auto;">
		<%: Html.Image("Modify-Direct_active.png", new { @onclick = "javascript:dealDirect.modifyFixedIncome(this);" })%>
	</div>
	<div class="editor-label" style="float: right;width:auto;">
		<%: Html.Span("", new { @id = "SpnLoading" })%>
	</div>
</div>
<%}%>
{{/if}}

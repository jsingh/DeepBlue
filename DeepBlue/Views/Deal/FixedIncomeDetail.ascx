<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.FixedIncomeDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="direct-det">
	<div id="fixincomediv">
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FaceValue) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FaceValue", "${FaceValue}", new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FixedIncomeISINO) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FixedIncomeISINO", "${FixedIncomeISINO}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.Maturity) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Maturity", "${Maturity}", new { @class = "datefield", @id = "FI_Maturity" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.IssuedDate) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("IssuedDate", "${IssuedDate}", new { @class = "datefield", @id = "FI_IssuedDate" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.CouponInformation) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("CouponInformation", "${CouponInformation}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FixedIncomeCurrencyId) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("FixedIncomeCurrencyId", Model.Currencies, new { @val = "${FixedIncomeCurrencyId}" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.Frequency) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Frequency", "${Frequency}", new { @onkeydown = "return jHelper.isNumeric(event);" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FirstCouponDate) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FirstCouponDate", "${FirstCouponDate}", new { @class = "datefield", @id = "FI_FirstCouponDate" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FirstAccrualDate) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FirstAccrualDate", "${FirstAccrualDate}", new { @class = "datefield", @id = "FI_FirstAccrualDate" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FixedIncomeTypeId) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("FixedIncomeTypeId", Model.FixedIncomeTypes, new { @val = "${FixedIncomeTypeId}" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FixedIncomeIndustryId) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FixedIncomeIndustry", "${FixedIncomeIndustry}", new { @id = "FixedIncomeIndustry" })%>
			<%: Html.Hidden("FixedIncomeIndustryId", "${FixedIncomeIndustryId}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FixedIncomeSymbol) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FixedIncomeSymbol", "${FixedIncomeSymbol}")%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FixedIncomeComments) %>
		</div>
		<div class="editor-field">
			<%: Html.TextArea("FixedIncomeComments","${FixedIncomeComments}", 5, 50, new { })%>
		</div>
		<%: Html.Hidden("FixedIncomeId","${FixedIncomeId}")%>
	</div>
</div>
<div class="line">
</div>
<br />
<div class="line">
</div>
<div class="direct-det">
	<div id="fidocument">
		<div class="editor-label">
			<%: Html.Label("Document Type") %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("FixedIncomeDocumentTypeId", Model.DocumentTypes, new { @val = "0" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("Document Date") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FixedIncomeDocumentDate", "${FixedIncomeDocumentDate}", new { @class = "datefield", @id = "FixedIncomeDocumentDate" })%>
		</div>
		<div class="editor-label">
			<%: Html.DropDownList("FixedIncomeUploadTypeId", Model.UploadTypes, new { @val="1", @style = "width:80px", @onchange = "javascript:dealDirect.changeUploadType(this,'fidocument');" })%>
		</div>
		<div id="FileRow" class="editor-field" style="width: auto;">
			<div style="padding: 0; margin: 0; width: auto;float:left;">
				<%: Html.File("FixedIncomeFile", new { @id = "equityFileToUpload" })%>
				<%: Html.Hidden("FixedIncomeFileId")%>
			</div>
			<div style="padding: 0; margin: 0;float:left;">
				<%: Html.Span("", new { @id = "SpnFixedIncomeDocLoading" })%>
			</div>
		</div>
		<div id="LinkRow" style="display: none; width: auto; margin: 0" class="editor-field">
			<%: Html.TextBox("FixedIncomeFilePath", "", new { @style = "width:250px" })%>
		</div>
	</div>
</div>
<div class="line">
</div>

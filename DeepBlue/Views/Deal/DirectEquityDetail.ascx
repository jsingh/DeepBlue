<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.EquityDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%--<div class="db-tab">
	<div class="db-left tabselect" id="NewEqTab" onclick="javascript:dealDirect.tabEquitySelect('N');">
		<div class="db-center">
			<div class="db-right">
				New Equity
			</div>
		</div>
	</div>
	<div class="db-left" id="ExistingEqTab" onclick="javascript:dealDirect.tabEquitySelect('E');">
		<div class="db-center">
			<div class="db-right">
				Existing Equities
			</div>
		</div>
	</div>
</div>--%>
<div class="line">
</div>
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
		<%: Html.TextBox("EquityDocumentDate", "", new { @class = "datefield", @id = "EquityDocumentDate" })%>
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
	<%: Html.Hidden("EquityId", "${EquityId}") %>
</div>
<div id="existingEquity">
	<table cellpadding="0" cellspacing="0" border="0" id="tblExistingEquity">
		<thead>
			<tr>
				<th>
					Equity Id
				</th>
				<th>
					Stock Symbol
				</th>
				<th>
					Industry
				</th>
				<th>
					Equity Type
				</th>
			</tr>
		</thead>
	</table>
</div>
<div class="line">
</div>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateUnderlyingFundModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="content">
	<%: Html.Hidden("IssuerId", "${IssuerId}")%>
	<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
	<div class="editor-label">
		<%: Html.Label("GP")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Issuer", "${IssuerName}", new { @id = "Issuer", @class="hide",  @onblur = "javascript:underlyingFund.checkIssuer(this);" })%>
		<%: Html.Span("${IssuerName}", new { @class = "show" }) %>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Legal Fund Name")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FundName", "${FundName}", new { @class = "wm hide" })%>
		<%: Html.Span("${FundName}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Fund Type")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("FundTypeId", Model.UnderlyingFundTypes, new { @class = "hide", @val = "${FundTypeId}", @refresh="true", @action="FundType" })%>
		<%: Html.Span("${FundType}", new { @class = "show" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Vintage Year")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("VintageYear", "${VintageYear}", new { @class = "hide", @onkeydown = "return jHelper.isNumeric(event);" })%>
		<%: Html.Span("${VintageYear}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Fund Size")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("TotalSize", "${formatNumber(TotalSize,0)}", new { @class = "hide", @onkeydown = "return jHelper.isNumeric(event);" })%>
		<%: Html.Span("${formatCurrency(TotalSize)}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Termination Year")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("TerminationYear", "${TerminationYear}", new { @class = "wm hide", @onkeydown = "return jHelper.isNumeric(event);" })%>
		<%: Html.Span("${TerminationYear}", new { @class = "show" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Reporting")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("ReportingFrequencyId", Model.Reportings, new { @class="hide", @val = "${ReportingFrequencyId}", @refresh = "true", @action = "ReportingFrequency" })%>
		<%: Html.Span("${ReportingFrequency}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Reporting Type")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("ReportingTypeId", Model.ReportingTypes, new { @class="hide", @val = "${ReportingTypeId}", @refresh = "true", @action = "ReportingType" })%>
		<%: Html.Span("${ReportingType}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Fees Included")%>
	</div>
	<div class="editor-field" style="width: auto;">
		<%: Html.CheckBox("IsFeesIncluded", false, new { @val = "${IsFeesIncluded}" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Industry")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("IndustryId", Model.Industries, new { @class="hide", @val = "${IndustryId}", @refresh = "true", @action = "Industry" })%>
		<%: Html.Span("${Industry}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Geography")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("GeographyId", Model.Geographyes, new { @class="hide", @val = "${GeographyId}", @refresh = "true", @action = "Geography" })%>
		<%: Html.Span("${Geography}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Website")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Website", "${Website}", new { @class = "hide", @webaddress = "true" })%>
		<%: Html.Span("${Website}", new { @class = "show" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Web User Name")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("WebUserName", "${WebUserName}", new { @class = "hide" })%>
		<%: Html.Span("${WebUserName}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Web Password")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("WebPassword", "${WebPassword}", new { @class = "hide" })%>
		<%: Html.Span("${WebPassword}", new { @class = "show" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Description")%>
	</div>
	<div class="editor-field">
		<%: Html.TextArea("Description", "${Description}", new { @class="hide", @style = "width:486px;height:160px;" })%>
		<%: Html.Span("${Description}", new { @class = "show" })%>
	</div>
</div>
<div id="ExpandUnderlying">
	<div class="line">
	</div>
	<div>
		<div class="headerbox">
			<div class="title">
				<%: Html.Span("ADDRESS INFORMATION")%>
			</div>
			<div class="rightdarrow">
				<%: Html.ImageButton("downarrow.png")%>
			</div>
		</div>
		<div class="expandheader expandsel" style="display: none">
			<div class="expandbtn">
				<div class="expandtitle">
					ADDRESS INFORMATION
				</div>
			</div>
		</div>
		<div class="detail" style="display: none;" id="AddressInformation">
			{{tmpl "#AddressTemplate"}}
		</div>
	</div>
	<div class="line">
	</div>
	<div>
		<div class="headerbox">
			<div class="title">
				<%: Html.Span("CONTACT INFORMATION")%>
			</div>
			<div class="rightdarrow">
				<%: Html.ImageButton("downarrow.png")%>
			</div>
		</div>
		<div class="expandheader expandsel" style="display: none">
			<div class="expandbtn">
				<div class="expandtitle">
					CONTACT INFORMATION
				</div>
			</div>
		</div>
		<div class="detail" style="display: none; padding-left: 36px;" id="ContactInformation">
			<div style="clear: both; width: 89%; padding-left: 38px;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="ContactList" class="grid">
					<thead>
						<tr>
							<th sortname="ContactName" style="width: 20%">
								Contact Name
							</th>
							<th sortname="ContactTitle" style="width: 20%">
								Title
							</th>
							<th sortname="Phone" style="width: 20%">
								Phone
							</th>
							<th sortname="Email" style="width: 30%">
								Email
							</th>
							<th style="width: 10%">
							</th>
						</tr>
					</thead>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
	<div class="line">
	</div>
	<div>
		<div class="headerbox">
			<div class="title">
				<%: Html.Span("BANK INFORMATION")%>
			</div>
			<div class="rightdarrow">
				<%: Html.ImageButton("downarrow.png")%>
			</div>
		</div>
		<div class="expandheader expandsel" style="display: none">
			<div class="expandbtn">
				<div class="expandtitle">
					BANK INFORMATION
				</div>
			</div>
		</div>
		<div class="detail" style="display: none;" id="BankInformation">
			<div class="info-detail bank-info-detail">
				<div class="editor-label">
					<%: Html.Label("Bank Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("BankName", "${BankName}", new { @class = "wm hide" })%>
					<%: Html.Span("${BankName}", new { @class="show" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("ABA Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("ABANumber", "${ABANumber}", new { @class = "wm hide", @onkeydown = "return jHelper.isNumeric(event);" })%>
					<%: Html.Span("${ABANumber}", new { @class = "show" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("Account Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Account", "${Account}", new { @class = "wm hide" })%>
					<%: Html.Span("${Account}", new { @class = "show" })%>
				</div>
				<div class="editor-label">
					<%: Html.Label("Account Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountNumber", "${AccountNumber}", new { @class = "wm hide" })%>
					<%: Html.Span("${AccountNumber}", new { @class = "show" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("FFC Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("FFC", "${FFC}", new { @class = "wm hide" })%>
					<%: Html.Span("${FFC}", new { @class = "show" })%>
				</div>
				<div class="editor-label" style="clear: right;">
					<%: Html.Label("FFC Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("FFCNumber", "${FFCNumber}", new { @class = "wm hide" })%>
					<%: Html.Span("${FFCNumber}", new { @class = "show" })%>
				</div>
				<div class="editor-label">
					<%: Html.Label("Reference")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Reference", "${Reference}", new { @class = "wm hide" })%>
					<%: Html.Span("${Reference}", new { @class = "show" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("Swift Code")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Swift", "${Swift}", new { @class = "wm hide" })%>
					<%: Html.Span("${Swift}", new { @class = "show" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("IBAN")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("IBAN", "${IBAN}", new { @class = "wm hide" })%>
					<%: Html.Span("${IBAN}", new { @class = "show" })%>
				</div>
				<div class="editor-label">
					<%: Html.Label("Phone")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountPhone", "${AccountPhone}", new { @class = "wm hide" })%>
					<%: Html.Span("${AccountPhone}", new { @class = "show" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("Fax")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountFax", "${AccountFax}", new { @class = "wm hide" })%>
					<%: Html.Span("${AccountFax}", new { @class = "show" })%>
				</div>
				<%--<div class="editor-label" style="clear: right">
					<%: Html.Label("Account Of")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountOf", "${AccountOf}", new { @class = "wm" })%>
				</div>
				<div class="editor-label">
					<%: Html.Label("Attention")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Attention", "${Attention}", new { @class = "wm" })%>
				</div>--%>
				<div>
					{{if UnderlyingFundId>0}}
					<div class="editor-label" style="float: right; width: auto;">
						<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:underlyingFund.cancelEdit(this,'.bank-info-detail');" })%>
					</div>
					<div class="editor-label show" style="float: right; width: auto;">
						<%: Html.Image("Editbtn_active.png", new { @onclick = "javascript:underlyingFund.edit(this,'.bank-info-detail');" })%>
					</div>
					<div class="editor-label hide" style="float: right; width: auto;">
						<%: Html.Image("Save_active.png", new { @onclick = "javascript:underlyingFund.saveTemp(this,${UnderlyingFundId})" })%></div>
					<div class="editor-label" style="float: right; width: auto;">
						<%: Html.Span("", new { @id = "BILoading" })%>
					</div>
					{{/if}}
				</div>
			</div>
		</div>
	</div>
	<div>
		<div class="line">
		</div>
		<div class="headerbox">
			<div class="title">
				<%: Html.Span("DOCUMENT INFORMATION")%>
			</div>
			<div class="rightdarrow">
				<%: Html.ImageButton("downarrow.png")%>
			</div>
		</div>
		<div class="expandheader expandsel" style="display: none">
			<div class="expandbtn">
				<div class="expandtitle">
					Document Information
				</div>
			</div>
		</div>
		<div class="detail" style="display: none; padding-left: 29px;" id="DocumentInformation">
			<%using (Html.Form(new { @id = "frmDocumentInfo", @onsubmit = "return underlyingFund.saveDocument(this);" })) {%>
			<div class="editor-label">
				<%: Html.Label("Document Type") %>
			</div>
			<div class="editor-field">
				<%: Html.DropDownList("DocumentTypeId", Model.DocumentTypes, new { @refresh = "true", @action = "DocumentType" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Document Date ") %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("DocumentDate", DateTime.Now.ToString("MM/dd/yyyy"), new { @id = "Doc_DocumentDate" })%>
			</div>
			<div id="dropbox" class="drop-files" style="padding: 38px 0 0 45px; margin: 0; width: 91%;">
				<div id="FilesList">
				</div>
				<div class="editor-label" style="padding-right:0">
					<%: Html.DropDownList("UploadTypeId", Model.UploadTypes, new { @style = "width:85px", @onchange = "javascript:underlyingFund.changeUploadType(this);" })%>
				</div>
				<div id="LinkRow" style="display: none; margin: 0 0;" class="editor-field">
					<%: Html.TextBox("FilePath", "", new { @style = "width:250px" })%>
					<%: Html.Hidden("FileId", "0")%>
				</div>
				<div id="FileRow" class="editor-field" style="padding-left: 12px;">
					<div style="padding: 0; margin: 0 0; width: auto; float: left;">
						<%: Html.File("File", new { @id = "fileToUpload" })%></div>
				</div>
				<div class="editor-field" style="width: auto;">
				</div>
				<div class="editor-label" style="width: 317px">
				</div>
				<div style="clear: both; float: right; color: #B3A8A8; margin: 18px 9px 0;">
					Drop files here to upload.
				</div>
			</div>
			<div class="editor-field" style="float: right; padding-right: 55px; padding-top: 21px;
				width: auto;">
				<%: Html.ImageButton("Upload_active.png", new { @id = "btnSaveDocument" })%>
			</div>
			<div class="cell" style="padding: 0; margin: 0;">
				<%: Html.Span("", new { @id = "SpnDocLoading" })%>
			</div>
			<div style="clear: both; width: 92%; padding-left: 38px;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table id="DocumentList" cellpadding="0" cellspacing="0" border="0" class="grid">
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
			<%} %>
		</div>
	</div>
	<div class="line">
	</div>
	<div class="uf-btn-box">
		<div class="editor-label" style="float: right; width: auto;">
			<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:underlyingFund.cancel(this);" })%>
		</div>
		{{if UnderlyingFundId>0}}
		<div class="editor-label show" style="float: right; width: auto;">
			<%: Html.Image("Editbtn_active.png", new { @onclick = "javascript:underlyingFund.edit(this,'.content','.uf-btn-box');" })%>
		</div>
		{{/if}}
		<div class="editor-label hide" style="float: right; width: auto;">
			{{if UnderlyingFundId>0}}
			<%: Html.Image("muf_active.png", new { @onclick = "javascript:underlyingFund.save(${UnderlyingFundId});" })%>
			{{else}}
			<%: Html.Image("adduf_active.png", new { @onclick = "javascript:underlyingFund.save(${UnderlyingFundId});" })%>
			{{/if}}
		</div>
		<div class="editor-label" style="float: right; width: auto;">
			<%: Html.Span("", new { @id = "SpnSaveLoading" })%>
		</div>
	</div>
</div>

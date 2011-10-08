<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateUnderlyingFundModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="content">
	<%: Html.Hidden("IssuerId", "${IssuerId}")%>
	<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
	<div class="editor-label">
		<%: Html.Label("GP")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Issuer", "${IssuerName}", new { @id = "Issuer",   @onblur = "javascript:underlyingFund.checkIssuer(this);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Legal Fund Name")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FundName", "${FundName}", new { @class = "wm" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Fund Type")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("FundTypeId", Model.UnderlyingFundTypes, new { @val = "${FundTypeId}", @refresh="true", @action="FundType" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Vintage Year")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("VintageYear", "${VintageYear}", new { @onkeydown = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Fund Size")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("TotalSize", "${TotalSize}", new { @onkeydown = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Termination Year")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("TerminationYear", "${TerminationYear}", new { @class = "wm", @onkeydown = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Reporting")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("ReportingFrequencyId", Model.Reportings, new { @val = "${ReportingFrequencyId}", @refresh = "true", @action = "ReportingFrequency" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Reporting Type")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("ReportingTypeId", Model.ReportingTypes, new { @val = "${ReportingTypeId}", @refresh = "true", @action = "ReportingType" })%>
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
		<%: Html.DropDownList("IndustryId", Model.Industries, new { @val = "${IndustryId}", @refresh = "true", @action = "Industry" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Geography")%>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("GeographyId", Model.Geographyes, new { @val = "${GeographyId}", @refresh = "true", @action = "Geography" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.Label("Web User Name")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("WebUserName","${WebUserName}")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Web Password")%>
	</div>
	<div class="editor-field">
		<%: Html.Password("WebPassword", "${WebPassword}")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Description")%>
	</div>
	<div class="editor-field">
		<%: Html.TextArea("Description", "${Description}", new { @style = "width:486px;height:160px;" })%>
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
		<div class="detail" style="display: none; padding-left: 61px;" id="AddressInformation">
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
		<div class="detail" style="display: none; padding-left: 61px;" id="BankInformation">
			<div class="info-detail">
				<div class="editor-label">
					<%: Html.Label("Bank Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("BankName", "${BankName}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("ABA Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("ABANumber", "${ABANumber}", new { @class = "wm", @onkeydown = "return jHelper.isNumeric(event);" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("Account Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Account", "${Account}", new { @class = "wm" })%>
				</div>
				<div class="editor-label">
					<%: Html.Label("Account Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountNumber", "${AccountNumber}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("FFC Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("FFC", "${FFC}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right;">
					<%: Html.Label("FFC Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("FFCNumber", "${FFCNumber}", new { @class = "wm" })%>
				</div>
				<div class="editor-label">
					<%: Html.Label("Reference")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Reference", "${Reference}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("Swift Code")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Swift", "${Swift}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("IBAN")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("IBAN", "${IBAN}", new { @class = "wm" })%>
				</div>
				<div class="editor-label">
					<%: Html.Label("Phone")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountPhone", "${AccountPhone}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("Fax")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountFax", "${AccountFax}", new { @class = "wm" })%>
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
				<div class="savebox">
					<div class="resetbtn">
						<%: Html.Span("Reset", new { @onclick = "javascript:underlyingFund.reset('BankInformation');" })%></div>
					<div class="btn">
						<%: Html.Image("Save_active.png", new { @onclick = "javascript:underlyingFund.saveTemp('BILoading')" })%></div>
					<div class="btn" id="BILoading">
					</div>
				</div>
			</div>
		</div>
	</div>
	<div style="display: none;">
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
			<div class="editor-label">
				<%: Html.DropDownList("UploadTypeId", Model.UploadTypes, new { @style = "width:85px", @onchange = "javascript:underlyingFund.changeUploadType(this);" })%>
			</div>
			<div id="FileRow" class="editor-field" style="padding-left: 12px;">
				<div style="padding: 0; margin: 0 0; width: auto; float: left;">
					<%: Html.File("File", new { @id = "fileToUpload" })%></div>
			</div>
			<div id="LinkRow" style="display: none; margin: 0 0;" class="editor-field">
				<%: Html.TextBox("FilePath", "", new { @style = "width:250px" })%>
				<%: Html.Hidden("FileId", "0")%>
			</div>
			<div class="editor-field" style="width: auto;">
			</div>
			<div class="editor-label" style="width: 317px">
			</div>
			<div class="editor-field" style="width: auto;">
				<%: Html.ImageButton("Save_active.png", new { @id = "btnSaveDocument" })%>
			</div>
			<div class="cell" style="padding: 0; margin: 0;">
				<%: Html.Span("", new { @id = "SpnDocLoading" })%>
			</div>
			<div style="clear: both; width: 70%; padding-left: 38px;">
				<br />
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
	<div class="btnbox">
		<div style="padding: 30px 0px; float: right;">
			<span id="SpnSaveLoading"></span><span>
				<%: Html.ImageButton("adduf_active.png", new { @id = "btnSave" })%>
			</span>
		</div>
	</div>
</div>
<script id="AddContactButtonTemplate" type="text/x-jquery-tmpl">
	<%using (Html.GreenButton(new { @onclick = "javascript:underlyingFundContact.add(this,${UnderlyingFundId});" })) {%>Add Contact<%}%>
</script>
<script id="ContactGridTemplate" type="text/x-jquery-tmpl">
	{{each(i,row) rows}}
	<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow disprow"{{else}}class="disprow"{{/if}}>
		<td style="width: 20%">
			<%: Html.Span("${row.cell[3]}", new { @class = "show" })%>
		</td>
		<td style="width: 20%">
			<%: Html.Span("${row.cell[4]}", new { @class = "show" })%>
		</td>
		<td style="width: 20%">
			<%: Html.Span("${row.cell[7]}", new { @class = "show" })%>
		</td>
		<td style="width: 30%">
			<%: Html.Span("${row.cell[6]}", new { @class = "show" })%>
		</td>
		<td style="text-align:right;width:10%;">
			<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:underlyingFundContact.edit(this,${row.cell[0]});" })%>
			<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:underlyingFundContact.deleteRow(this,${row.cell[0]});" })%>
			<%: Html.Hidden("UnderlyingFundContactId", "${row.cell[0]}") %>
		</td>
	</tr>
	<tr id="EditRow${row.cell[0]}" style="background-image:none;">
		<td colspan=6 style="width: 100%;display:none;">
			<%using(Html.Form(new { @class="UFContactDetail", @id="frm${row.cell[0]}", @onsubmit = "return false;" })){%>
			<div class="editor-label">
				<label>
					Contact Name</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("ContactName", "${row.cell[3]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Title</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("ContactTitle", "${row.cell[4]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Phone Number</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Phone", "${row.cell[7]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Email</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Email", "${row.cell[6]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Web Address</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("WebAddress", "${row.cell[8]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Notes</label>
			</div>
			<div class="editor-field">
				<%=Html.jQueryTemplateTextArea("ContactNotes", "${row.cell[5]}", 6,50, new {} )%>
			</div>
			<%: Html.Hidden("UnderlyingFundContactId", "${row.cell[0]}")%>
			<%: Html.Hidden("UnderlyingFundId", "${row.cell[1]}")%>
			<div class="editor-label" style="margin-left:35%;margin-top:10px;width:200px;text-align:left;">
				<%: Html.Image("Save_active.png", new { @class="submitbtn", @onclick = "javscript:underlyingFundContact.save(this,${row.cell[0]});" } )%>
				&nbsp;&nbsp;<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:underlyingFundContact.cacelEdit(${row.cell[0]});" }) %>
				&nbsp;&nbsp;<%:Html.Span("", new { @id = "Loading" })%>
			</div>

			<%}%>
		</td>
	</tr>
	{{/each}}
</script>

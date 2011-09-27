<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateUnderlyingFundModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="content">
	<%: Html.Hidden("IssuerId", "${IssuerId}")%>
	<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
	<div class="editor-label-first">
		<label>
			GP</label>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Issuer", "${IssuerName}", new { @id = "Issuer",   @onblur = "javascript:underlyingFund.checkIssuer(this);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<label>
			Legal Fund Name</label>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FundName", "${FundName}", new { @class = "wm" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<label>
			Fund Type</label>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("FundTypeId", Model.UnderlyingFundTypes, new { @val = "${FundTypeId}" })%>
	</div>
	<div class="editor-label-first">
		<label>
			Vintage Year</label>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("VintageYear", "${VintageYear}", new { @onkeydown = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<label>
			Fund Size</label>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("TotalSize", "${TotalSize}", new { @onkeydown = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<label>
			Termination Year</label>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("TerminationYear", "${TerminationYear}", new { @class = "wm", @onkeydown = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label-first">
		<label>
			Reporting</label>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("ReportingFrequencyId", Model.Reportings, new { @val = "${ReportingFrequencyId}" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<label>
			Reporting Type</label>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("ReportingTypeId", Model.ReportingTypes, new { @val = "${ReportingTypeId}" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<label>
			Fees Included</label>
	</div>
	<div class="editor-field" style="width: auto;">
		<%: Html.CheckBox("IsFeesIncluded", false, new { @val = "${IsFeesIncluded}" })%>
	</div>
	<div class="editor-label-first">
		<label>
			Industry</label>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("IndustryId", Model.Industries, new { @val = "${IndustryId}" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<label>
			Geography</label>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("GeographyId", Model.Geographyes, new { @val = "${GeographyId}" })%>
	</div>
	<div class="editor-label-first">
		<label>
			Description</label>
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
				<%: Html.Span("CONTACT INFORMATION")%>
			</div>
			<div class="rightdarrow">
				<%: Html.ImageButton("downarrow.png")%>
			</div>
		</div>
		<div class="expandheader expandsel" style="display: none">
			<div class="expandbtn">
				<div class="expandtitle">
					Contact Information
				</div>
			</div>
		</div>
		<div class="detail" style="display: none; padding-left: 36px;" id="ContactInformation">
			<div style="clear: both; width: 70%; padding-left: 38px;">
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
					Bank Information
				</div>
			</div>
		</div>
		<div class="detail" style="display: none; padding-left: 6px;" id="BankInformation">
			<div>
				<div class="editor-label-first">
					<%: Html.Label("Bank Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("BankName", "${BankName}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("ABA Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Routing", "${Routing}", new { @class = "wm", @onkeydown = "return jHelper.isNumeric(event);" })%>
				</div>
				<div class="editor-label-first">
					<%: Html.Label("Account Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Account", "${Account}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear:right;">
					<%: Html.Label("Account Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountNumber", "${AccountNumber}", new { @class = "wm" })%>
				</div>
				<div class="editor-label-first">
					<%: Html.Label("FFC Name")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("FFC", "${FFC}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear:right;">
					<%: Html.Label("FFC Number")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("FFCNumber", "${FFCNumber}", new { @class = "wm" })%>
				</div>
				<div class="editor-label-first">
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
				<div class="editor-label-first">
					<%: Html.Label("IBAN")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("IBAN", "${IBAN}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("Account Of")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountOf", "${AccountOf}", new { @class = "wm" })%>
				</div>
				<div class="editor-label-first">
					<%: Html.Label("Attention")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Attention", "${Attention}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.Label("Telephone")%>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountPhone", "${AccountPhone}", new { @class = "wm" })%>
				</div>
			</div>
			<div class="savebox" style="width:652px;">
				<div class="resetbtn">
					<%: Html.Span("Reset", new { @onclick = "javascript:underlyingFund.reset('BankInformation');" })%></div>
				<div class="btn">
					<%: Html.Image("Save_active.png", new { @onclick = "javascript:underlyingFund.saveTemp('BILoading')" })%></div>
				<div class="btn" id="BILoading">
				</div>
			</div>
		</div>
	</div>
	<div style="display:none;">
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
			<div class="editor-label-first">
				<%: Html.Label("Document Type") %>
			</div>
			<div class="editor-field">
				<%: Html.DropDownList("DocumentTypeId", Model.DocumentTypes)%>
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
			<div id="FileRow" class="editor-field" style="padding-left:12px;">
				<div  style="padding: 0; margin: 0 0; width: auto;float:left;">
					<%: Html.File("File", new { @id = "fileToUpload" })%></div>
			</div>
			<div id="LinkRow" style="display: none;margin:0 0;" class="editor-field">
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
	<%using (Html.GreenButton(new { @onclick = "javascript:underlyingFundContact.add(this);" })) {%>Add Contact<%}%>
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
			<%: Html.Span("${row.cell[8]}", new { @class = "show" })%>
		</td>
		<td style="width: 30%">
			<%: Html.Span("${row.cell[7]}", new { @class = "show" })%>
		</td>
		<td style="text-align:right;width:10%;">
			{{if row.cell[0]==0}}
			<%: Html.Image("add_active.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:underlyingFundContact.save(this,${row.cell[0]});" })%>
			{{else}}
			<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:underlyingFundContact.save(this,${row.cell[0]});" })%>
			<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:underlyingFundContact.edit(this,${row.cell[0]});" })%>
			<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:underlyingFundContact.deleteRow(this,${row.cell[0]});" })%>
			{{/if}}
			<%: Html.Hidden("UnderlyingFundContactId", "${row.cell[0]}") %>
		</td>
	</tr>
	<tr id="EditRow${row.cell[0]}" style="background-image:none;">
		<td colspan=6 style="width: 100%;display:none;">
			<%using(Html.Form(new { @id="frm${row.cell[0]}", @onsubmit = "return false;" })){%>
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
				<%: Html.TextBox("Phone", "${row.cell[8]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Email</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Email", "${row.cell[7]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Web Address</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("WebAddress", "${row.cell[9]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Web User name</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("WebUsername", "${row.cell[10]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Web Password</label>
			</div>
			
			<div class="editor-field" style="padding-right:0px;">
				{{if row.cell[11] != null }}
				<%: Html.Password("WebPassword", "", new { @class = "wm",@style="width:160px;",@disabled = "disabled"})%>
				{{else}}
				<%: Html.Password("WebPassword", "", new { @class = "wm", @style="width:160px;"})%>
				{{/if}}
			</div>
			<div class="editor-label" style="clear: none; padding: 0px; width: 175px; text-align: left;">
				<%: Html.Hidden("ChangeWebPassword", "{{if row.cell[11] == null }}true{{else}}false{{/if}}")%>
				<%: Html.Image("Editbtn_active.png", new { @id = "EditWebPassword", @onclick = "javascript:underlyingFundContact.editWebPassword(${row.cell[0]});", @style = "{{if row.cell[11] == null }}display:none{{/if}}" })%>
				<%: Html.Image("Cancel_active.png", new { @id = "CancelWebPassword", @onclick = "javascript:underlyingFundContact.cancelWebPassword(${row.cell[0]});", @style = "{{if row.cell[11] == null }}display:none{{/if}}" })%>
			</div>
			<div class="editor-label">
				<label>
					Registered Address</label>
			</div>
			<div class="editor-field">
				<%: Html.TextArea("Address", "${row.cell[7]}", new { @style = "width:484px;height:70px;" })%>
			</div>
			<div class="editor-label">
				<label>
					Notes</label>
			</div>
			<div class="editor-field">
				<%: Html.TextArea("ContactNotes", "${row.cell[6]}", new { @style = "width:484px;height:100px;" })%>
			</div>
			<%: Html.Hidden("UnderlyingFundContactId", "${row.cell[0]}")%>
			<div class="editor-label" style="margin-left:35%;margin-top:10px;width:200px;text-align:left;">
				<%: Html.Image("Save_active.png", new { @class="submitbtn", @onclick = "javscript:underlyingFundContact.save(${row.cell[0]});" } )%>
				&nbsp;&nbsp;<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:underlyingFundContact.cacelEdit(${row.cell[0]});" }) %>
				&nbsp;&nbsp;<%:Html.Span("", new { @id = "Loading" })%>
			</div>

			<%}%>
		</td>
	</tr>
	{{/each}}
</script>

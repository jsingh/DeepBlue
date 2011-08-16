<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateUnderlyingFundModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="content">
	<%: Html.Hidden("IssuerId", "${IssuerId}")%>
	<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
	<div class="editor-label">
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
	<div class="editor-label">
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
	<div class="editor-label">
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
	<div class="editor-label">
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
	<div class="editor-label">
		<label>
			Description</label>
	</div>
	<div class="editor-field">
		<%: Html.TextArea("Description", "${Description}", new { @style = "width:519px;height:160px;" })%>
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
			<div class="rightuarrow">
			</div>
		</div>
		<div class="detail" style="display: none; padding-left: 95px;" id="ContactInformation">
			<div class="editor-label">
				<label>
					Contact Name</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("ContactName", "${ContactName}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Phone Number</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Phone", "${Phone}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Email</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Email", "${Email}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Web Address</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("WebAddress", "${WebAddress}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Registered Address</label>
			</div>
			<div class="editor-field">
				<%: Html.TextArea("Address", "${Address}", new { @style = "width:515px;height:140px;" })%>
			</div>
			<div class="savebox">
				<div class="btn" id="CILoading">
				</div>
				<div class="btn">
					<%: Html.Image("Save.png", new { @onclick = "javascript:underlyingFund.saveTemp('CILoading')" })%></div>
				<div class="resetbtn">
					<%: Html.Span("Reset", new { @onclick = "javascript:underlyingFund.reset('ContactInformation');" })%></div>
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
			<div class="rightuarrow">
			</div>
		</div>
		<div class="detail" style="display: none; padding-left: 95px;" id="BankInformation">
			<div>
				<div class="editor-label">
					<label>
						Bank Name</label>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("BankName", "${BankName}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<label>
						ABA No.</label>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Routing", "${Routing}", new { @class = "wm", @onkeydown = "return jHelper.isNumeric(event);" })%>
				</div>
				<div class="editor-label">
					<label>
						Account Of</label>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("AccountOf", "${AccountOf}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<label>
						Account No.</label>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Account", "${Account}", new { @class = "wm" })%>
				</div>
				<div class="editor-label">
					<label>
						Attention</label>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Attention", "${Attention}", new { @class = "wm" })%>
				</div>
				<div class="editor-label" style="clear: right">
					<label>
						Reference</label>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("Reference", "${Reference}", new { @class = "wm" })%>
				</div>
			</div>
			<div class="savebox">
				<div class="btn" id="BILoading">
				</div>
				<div class="btn">
					<%: Html.Image("Save.png", new { @onclick = "javascript:underlyingFund.saveTemp('BILoading')" })%></div>
				<div class="resetbtn">
					<%: Html.Span("Reset", new { @onclick = "javascript:underlyingFund.reset('BankInformation');" })%></div>
			</div>
		</div>
	</div>
	<div class="line">
	</div>
	<div>
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
			<div class="rightuarrow">
			</div>
		</div>
		<div class="detail" style="display: none; padding-left: 65px;" id="DocumentInformation">
			<%using (Html.Form(new { @id = "frmDocumentInfo", @onsubmit = "return underlyingFund.saveDocument(this);" })) {%>
			<div class="editor-label">
				<%: Html.Label("Document Type") %>
			</div>
			<div class="editor-field">
				<%: Html.DropDownList("DocumentTypeId", Model.DocumentTypes)%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Document Date ") %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("DocumentDate", "", new { @id = "Doc_DocumentDate" })%>
			</div>
			<div class="editor-label">
				<%: Html.DropDownList("UploadTypeId", Model.UploadTypes, new { @style = "width:80px", @onchange = "javascript:underlyingFund.changeUploadType(this);" })%>
			</div>
			<div id="FileRow" class="editor-field">
				<div class="cell" style="padding: 0; margin: 0; width: auto;">
					<%: Html.File("File", new { @id = "fileToUpload" })%></div>
			</div>
			<div id="LinkRow" style="display: none" class="editor-field">
				<%: Html.TextBox("FilePath")%>
				<%: Html.Hidden("FileId", "0")%>
			</div>
			<div class="editor-field" style="width: auto;">
			</div>
			<div class="editor-label" style="width: 317px">
			</div>
			<div class="editor-field" style="width: auto;">
				<%: Html.ImageButton("Save.png", new { @id = "btnSaveDocument" })%>
			</div>
			<div class="cell" style="padding: 0; margin: 0;">
				<%: Html.Span("", new { @id = "SpnDocLoading" })%>
			</div>
			<div style="clear: both; width: 70%; padding-left: 52px;">
				<br />
				<table id="DocumentList" cellpadding="0" cellspacing="0" border="0">
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
			</div>
			<%} %>
		</div>
	</div>
	<div class="line">
	</div>
	<div style="padding-top: 50px; float: right; padding-right: 200px;">
		<span id="SpnSaveLoading"></span><span>
			<%: Html.ImageButton("adduf.png", new { @id = "btnSave" })%>
		</span>
	</div>
</div>

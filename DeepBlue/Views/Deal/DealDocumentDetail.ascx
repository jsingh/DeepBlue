<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="expandaddbtn">
	<%using (Html.GreenButton(new { @onclick = "javascript:deal.addDealDocument();" })) {%>Add
	documents<%}%>
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			DEAL DOCUMENTS</div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Deal Documents</div>
		</div>
	</div>
</div>
<div class="fieldbox">
	<div class="section" id="AddDealDocument" style="display: none;">
		<div class="dealdetail">
			<% using (Html.Form(new { @id = "frmDealDocument", @onsubmit = "return deal.saveDealDocument(this);", @enctype = "multipart/form-data" })) {%>
			<div class="editor-label">
				<%: Html.Label("Document Type")%>
			</div>
			<div class="editor-field" style="width: 278px">
				<%: Html.TextBox("DocumentType", "", new { @style = "width:163px" })%>
				<%: Html.Hidden("DocumentTypeId", "0")%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("For")%>
			</div>
			<div class="editor-field" style="width: 235px">
				<div id="InvestorRow" style="display: none; float: left;">
					<%: Html.TextBox("DocumentInvestorName", "", new { @onblur = "javascript:deal.documentInvestorBlur(this);", @style = "width:172px" })%>
				</div>
				<div id="FundRow" style="float: left;">
					<%: Html.TextBox("DocumentFundName", "SEARCH AMBERBROOK FUND", new { @onblur = "javascript:deal.documentFundBlur(this);", @style = "width:185px" })%>
				</div>
			</div>
			<div id="dropbox" class="drop-files">
				<div id="FilesList">
				</div>
				<table cellpadding="0" cellspacing="10" border="0" style="width: 100%">
					<tr>
						<td style="text-align: center;">
							<div class="editor-label">
								<%: Html.Label("File / Link ")%>
							</div>
							<div class="editor-field" style="width: auto;">
								<div id="FileRow" style="float: left">
									<%: Html.File("DocumentFile", new { @id = "fileToUpload" })%>
								</div>
								<div id="LinkRow" style="display: none; float: left;">
									<%: Html.TextBox("DocumentFilePath", "", new { @style = "width:213px" })%>
								</div>
								<div style="float: left; margin-left: 2px;">
									<%: Html.DropDownList("DocumentUploadTypeId", Model.UploadTypes, new { @style = "width:85px", @val = "1", @onchange = "javascript:deal.documentChangeUploadType(this);" })%>
								</div>
							</div>
						</td>
					</tr>
				</table>
				<div style="clear: both; float: right; color: #B3A8A8; margin: 18px 9px 0;">
					Drop files here to upload.
				</div>
			</div>
			<div class="editor-label" style="width: 317px">
			</div>
			<div class="editor-field" style="width: auto; float: right; margin: 10px 0 0;">
				<div class="cell" style="width: auto;">
					<%: Html.Span("", new { @id = "SpnDealDocLoading" })%>
				</div>
				<div class="cell" style="width: auto;">
					<%: Html.ImageButton("Upload_active.png", new { @id = "btnSaveDocument" })%></div>
				<div class="cell" style="width: auto;">
					<%: Html.Anchor("Reset", "javascript:deal.documentInfoReset();")%></div>
			</div>
			<%: Html.Hidden("DocumentInvestorId","")%>
			<%: Html.Hidden("DocumentFundId","")%>
			<%}%>
		</div>
	</div>
	<div class="section" style="width: 95%">
		<div class="dealdetail">
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" border="0" id="DealDocumentList" style="width: 100%"
				class="grid">
				<thead>
					<tr>
						<th sortname="DocumentType">
							Document Type
						</th>
						<th style="width: 20%" sortname="DocumentDate">
							Document Date
						</th>
						<th style="width: 20%" sortname="FundName">
							For
						</th>
						<th style="width: 20%" sortname="FileName">
							File/Link
						</th>
						<th align="right" style="width: 20%">
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

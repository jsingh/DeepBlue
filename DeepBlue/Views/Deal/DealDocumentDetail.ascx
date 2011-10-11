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
			ADD DEAL DOCUMENTS</div>
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
			<div class="editor-label-first">
				<%: Html.Label("Document Type")%>
			</div>
			<div class="editor-field" style="width: 278px">
				<%: Html.TextBox("DocumentType", "", new { @style = "width:163px" })%>
				<%: Html.Hidden("DocumentTypeId", "0")%>
			</div>
			<div class="editor-label" style="clear: right; width: 124px;">
				<%: Html.Label("Document Date ")%>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("DocumentDate")%>
			</div>
			<div class="editor-label-first">
				<%: Html.Label("For")%>
			</div>
			<div class="editor-field" style="width: auto;">
				<div id="InvestorRow" style="display: none; float: left;">
					<%: Html.TextBox("DocumentInvestorName", "", new { @onblur = "javascript:deal.documentInvestorBlur(this);", @style = "width:172px" })%>
				</div>
				<div id="FundRow" style="float: left;">
					<%: Html.TextBox("DocumentFundName", "", new { @onblur = "javascript:deal.documentFundBlur(this);", @style = "width:163px" })%>
				</div>
				<div style="float: left; margin-left: 2px;">
					<%: Html.DropDownList("DocumentStatusId", Model.DocumentStatusTypes, new { @style = "width:85px", @val="2", @onchange = "javascript:deal.documentChangeType(this);" })%>
				</div>
			</div>
			<div class="editor-label" style="clear: right; width: 114px;">
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
			<div class="editor-label" style="width: 317px">
			</div>
			<div class="editor-field" style="width: auto; float: right; margin: 20px 128px 5px;">
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
						<th>
							Document Type
						</th>
						<th style="width: 20%">
							Document Date
						</th>
						<th style="width: 20%">
							For
						</th>
						<th style="width: 20%">
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

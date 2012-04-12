<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="accordion-group">
	<div class="accordion-heading">
		<a href="#DealDocumentBox" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle">Deal Documents</a>
	</div>
	<div id="DealDocumentBox" class="accordion-body collapse">
		<div class="deal-detail-list">
			<div class="pull-right">
				<%: Html.Button("Add document", new { @class = "btn btn-primary", @onclick = "javascript:deal.addDealDocument();" })%>
			</div>
		</div>
		<div class="clear">
			&nbsp;</div>
		<br />
		<div id="AddDealDocument" class="deal-detail-list">
			<% using (Html.Form(new { @id = "frmDealDocument", @class = "form-horizontal", @onsubmit = "return deal.saveDealDocument(this);", @enctype = "multipart/form-data" })) {%>
			<div class="control-group pull-left">
				<label class="control-label">
					Document Type</label><div class="controls">
						<%: Html.TextBox("DocumentType", "", new { @placeholder = "SEARCH DOCUMENT TYPE", @class = "input-large" })%>
					</div>
			</div>
			<div class="control-group pull-left">
				<label class="control-label">
					For</label><div class="controls">
						<%: Html.TextBox("DocumentInvestorName", "", new { @class = "input-large", @placeholder = "SEARCH INVESTOR", @style="display:none" })%>
						<%: Html.TextBox("DocumentFundName", "", new { @class = "input-large", @placeholder = "SEARCH FUND" })%>
					</div>
			</div>
			<div class="clear">
				&nbsp;</div>
			<div class="control-group drop-files">
				<div id="FilesList">
				</div>
				<div class="control-group pull-left">
					<label class="control-label">
						File / Link</label><div class="controls">
							<span id="Span1">
								<%: Html.File("DocumentFile", new { @id = "fileToUpload", @class = "input-large" })%></span> <span id="Span2" style="display: none">
									<%: Html.TextBox("DocumentFilePath", "", new { @class = "input-large" })%></span>
							<%: Html.DropDownList("DocumentUploadTypeId", Model.UploadTypes, new {  @class = "input-large", @val = "1", @onchange = "javascript:deal.documentChangeUploadType(this);" })%>
						</div>
				</div>
				<div class="clear">
					&nbsp;</div>
				<div class="control-group pull-right">
					Drop files here to upload.</div>
			</div>
			<div class="clear">
				&nbsp;</div>
			<div class="control-group center">
				<button class="btn btn-success" id="btnSaveDocument">
					Upload</button>
				<%: Html.Anchor("Reset", "javascript:deal.documentInfoReset();", new { @class = "btn" })%>
			</div>
			<%: Html.Hidden("DocumentTypeId", "0")%>
			<%: Html.Hidden("DocumentInvestorId","")%>
			<%: Html.Hidden("DocumentFundId","")%>
			<%}%>
		</div>
		<div class="clear">
			&nbsp;</div>
		<br />
		<div class="deal-detail-list">
			<table id="DealDocumentList" class="table table-striped table-bordered">
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
		</div>
	</div>
</div>

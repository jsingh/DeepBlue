<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			<%: Html.Image("DealDocuments.png")%></div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Deal Documents</div>
		</div>
	</div>
	<div class="expandaddbtn">
		<%: Html.Anchor(Html.Image("add_doc.png").ToHtmlString(), "javascript:void(0);")%>
	</div> 
</div>
<div class="fieldbox">
	<div class="section">
		<div class="editor-label">
			<%: Html.Label("Document Type") %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("DocumentType", Model.DocumentTypes)%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("Document Date ") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("DocumentDate")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("For") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Fund")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("File / Link ") %>
		</div>
		<div class="editor-field" style="width: auto;">
			<div class="cell" style="padding: 0; margin: 0; width: auto;">
				<%: Html.File("File", new { @id="fileToUpload" })%></div>
			<%--<div class="cell" style="padding: 0pt; background-color: White; height: 16px; width: 100px;
				margin: 5px 0pt 0pt 5px;">
				<div id="DocProgress" style="background-color: Red; float: left; height: 16px; width: 10px;">
					&nbsp;
				</div>
			</div>
			<div class="cell" style="padding: 0; margin: 0;">
				<%: Html.Span("", new { @id = "SpnDocProgress" })%>
			</div>--%>
		</div>
		<div class="editor-label" style="width: 317px">
		</div>
		<div class="editor-field" style="width: auto;">
			<%: Html.ImageButton("Save.png", new { @id = "btnSaveDocument" })%>
		</div>
		<div class="cell" style="padding: 0; margin: 0;">
			<%: Html.Span("", new { @id = "SpnDocLoading" })%>
		</div>
		<div style="clear: both">
			<br />
			<div class="gbox">
				<table cellpadding="0" cellspacing="0" border="0" class="grid" style="width: 80%;">
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
							<th style="width: 20%">
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
			</div>
		</div>
	</div>
</div>

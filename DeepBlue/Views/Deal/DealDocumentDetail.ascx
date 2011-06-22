﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
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
</div>
<div class="fieldbox">
	<div class="section">
    <div class="editor-label">
			<%: Html.Label("Document Type-") %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("DocumentType", Model.DocumentTypes)%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("Document Date -") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("DocumentDate")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Fund -") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Fund")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("File/Link -") %>
		</div>
		<div class="editor-field">
			<%: Html.File("File", new { })%>
		</div>
		<table cellpadding="0" cellspacing="0" border="0" class="grid" style="width: 80%; padding-top:20px;">
			<thead>
				<tr class="dealhead_tr">
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

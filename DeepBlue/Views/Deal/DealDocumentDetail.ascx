<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div>
	<%: Html.Image("DealDocuments.png", new { @class="expandbtn" })%>
</div>
<div class="fieldbox">
	<table cellpadding="0" cellspacing="0" border="0" class="grid" style="width: 80%">
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
			<tr>
				<td style="text-align: center">
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
					<%: Html.Image("Delete_Btn.png")%>
				</td>
			</tr>
		</tbody>
	</table>
	<br />
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
</div>

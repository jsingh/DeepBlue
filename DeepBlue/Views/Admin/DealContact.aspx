<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DealContact
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("DealContact.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">ADMIN</span><span class="arrow"></span><span class="pname">DEAL
					MANAGEMENT</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-content">
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" border="0" id="DealContactList" class="grid">
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("DealContactList", new FlexigridOptions { 
    ActionName = "DealContactList", ControllerName = "Admin",
	HttpMethod = "GET",
	SortName = "ContactName",
	Paging = true 
	, OnSuccess= "dealContact.onGridSuccess"
	, OnRowClick = "dealContact.onRowClick"
	, OnInit = "dealContact.onInit"
	, OnTemplate = "dealContact.onTemplate"
	, TableName = "Contact"
	, ExportExcel = true
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:dealContact.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
	<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow disprow"{{else}}class="disprow"{{/if}}>
		<td>
			<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		</td>
		<td>
			<%: Html.Span("${row.cell[2]}", new { @class = "show" })%>
		</td>
		<td>
			<%: Html.Span("${row.cell[5]}", new { @class = "show" })%>
		</td>
		<td>
			<%: Html.Span("${row.cell[4]}", new { @class = "show" })%>
		</td>
		<td style="text-align:right;">
			<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:dealContact.edit(this,${row.cell[0]});" })%>
			<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:dealContact.deleteRow(this,${row.cell[0]});" })%>
			<%: Html.Hidden("ContactId", "${row.cell[0]}") %>
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
				<%: Html.TextBox("ContactName", "${row.cell[1]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Title</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("ContactTitle", "${row.cell[2]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Phone Number</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Phone", "${row.cell[5]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Email</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Email", "${row.cell[4]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Web Address</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("WebAddress", "${row.cell[6]}", new { @class = "wm" })%>
			</div>
			<div class="editor-label">
				<label>
					Notes</label>
			</div>
			<div class="editor-field">
				<%=Html.jQueryTemplateTextArea("ContactNotes", "${row.cell[3]}", 6,50, new {} )%>
			</div>
			<%: Html.Hidden("ContactId", "${row.cell[0]}")%>
			<div class="editor-label" style="margin-left:35%;margin-top:10px;width:200px;text-align:left;">
				<%: Html.Image("Save_active.png", new { @class="submitbtn", @onclick = "javscript:dealContact.save(${row.cell[0]});" } )%>
				&nbsp;&nbsp;<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:dealContact.cacelEdit(${row.cell[0]});" }) %>
				&nbsp;&nbsp;<%:Html.Span("", new { @id = "Loading" })%>
			</div>

			<%}%>
		</td>
	</tr>
	{{/each}}
	</script>
</asp:Content>

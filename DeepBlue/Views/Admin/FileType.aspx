﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	File Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("FileType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">ADMIN</span><span class="arrow"></span><span class="pname">DOCUMENT
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
			<table cellpadding="0" cellspacing="0" border="0" id="FileTypeList" class="grid">
				<thead>
					<tr>
						<th sortname="FileTypeName" style="width: 50%">
							File Type
						</th>
						<th sortname="FileExtension" style="width: 40%">
							File Extension
						</th>
						<th align="center" style="width: 5%;">
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("FileTypeList", new FlexigridOptions { 
    ActionName = "FileTypeList", ControllerName = "Admin",
	HttpMethod = "GET",
	SortName = "FileTypeName",
	Paging = true 
	, OnSuccess= "fileType.onGridSuccess"
	, OnRowClick = "fileType.onRowClick"
	, OnInit = "fileType.onInit"
	, OnTemplate = "fileType.onTemplate"
	, TableName = "FileTypeName"
	, ExportExcel = true
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:fileType.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
	<td style="width: 40%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		<%: Html.TextBox("FileTypeName", "${row.cell[1]}", new { @class = "hide" })%>
	</td>
	<td style="width: 30%;text-align:left;">
		<%: Html.Span("${row.cell[2]}", new { @class = "show" })%>
		<%: Html.TextBox("FileExtension", "${row.cell[2]}", new { @class = "hide" })%>
	</td>
	<td style="text-align:right;">
		{{if row.cell[0]==0}}
		<%: Html.Image("add_active.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:fileType.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:fileType.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton editbtn show", @onclick = "javascript:fileType.edit(this);" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:fileType.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("FileTypeId", "${row.cell[0]}") %>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Export
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("Export.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">ADMIN</span><span class="arrow"></span><span class="pname">Export
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
			<table cellpadding="0" cellspacing="0" border="0" id="ExportList" class="grid">
				<thead>
					<tr>
						<th style="width: 20%" sortname="TableName">
							Table Name
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
	<%=Html.jQueryFlexiGrid("ExportList", new FlexigridOptions { 
    ActionName = "ExportList", ControllerName = "Admin",
	HttpMethod = "GET",
	SortName = "TableName",
	SortOrder = "asc",
	Paging = true 
	, OnSubmit = "exportExcel.onSubmit"
	, OnSuccess= "exportExcel.onGridSuccess"
	, OnRowClick = "exportExcel.onRowClick"
	, OnInit = "exportExcel.onInit"
	, OnTemplate = "exportExcel.onTemplate"
})%>
	<%using (Html.jQueryTemplateScript("AddButtonTemplate")) { %>
	<%--<div class="editor-label" style="margin-top: 12px; padding: 0; width: auto;">
		<%:Html.Label("Search")%></div>
	<div class="editor-field" style="margin-top: 16px; padding: 0 0 0 5px;">
		<%: Html.TextBox("Query", "", new  { @style = "text-align:left" })%></div>
	<div class="editor-label" style="clear: right; margin: 3px 0 0; padding: 0 0 0 10px;
		width: auto;">
		<%: Html.Image("search_active.png", new { @onclick = "javascript:$('#ExportList').flexReload();" })%>
	</div>--%>
	<%}%>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row}" {{if i%2>0}}class="erow disprow"{{else}}class="disprow"{{/if}}>
	<td style="width: 20%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
	</td>
	<td style="text-align:right;width:10%;">
		<%: Html.Image("Export-Excel_active.png", new { @class="gbutton", @onclick = "javascript:exportExcel.exportExcel('${row.cell[0]}');" })%>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

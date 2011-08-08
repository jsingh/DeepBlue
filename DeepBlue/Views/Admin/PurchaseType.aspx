<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Purchase Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("PurchaseType.js")%>
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
			<table cellpadding="0" cellspacing="0" border="0" id="PurchaseTypeList">
				<thead>
					<tr>
						<th sortname="Name" style="width: 40%">
							Purchase Type
						</th>
						<th>
						</th>
					</tr>
				</thead>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("PurchaseTypeList", new FlexigridOptions { 
    ActionName = "PurchaseTypeList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "Name", Paging = true 
	, OnSuccess= "purchaseType.onGridSuccess"
	, OnRowClick = "purchaseType.onRowClick"
	, OnInit = "purchaseType.onInit"
	, OnTemplate = "purchaseType.onTemplate"
	, ExportExcel = true
	, TableName = "PurchaseType"
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:purchaseType.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
	<td style="width: 40%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		<%: Html.TextBox("Name", "${row.cell[1]}", new { @class = "hide" })%>
	</td>
	<td style="text-align:right;">
		{{if row.cell[0]==0}}
		<%: Html.Image("Add.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:purchaseType.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:purchaseType.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:purchaseType.edit(this);" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:purchaseType.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("PurchaseTypeId", "${row.cell[0]}") %>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund Closing
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("FundClosing.js")%>
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
			<table cellpadding="0" cellspacing="0" border="0" id="FundClosingList" class="grid">
				<thead>
					<tr>
						<th sortname="Name" style="width: 15%">
							Name
						</th>
						<th sortname="FundName" style="width: 30%">
							Fund
						</th>
						<th sortname="FundClosingDate" style="width: 20%">
							Closing Date
						</th>
						<th sortname="IsFirstClosing" style="width: 30%">
							First Closing
						</th>
						<th>
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("FundClosingList", new FlexigridOptions { 
    ActionName = "FundClosingList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "Name", Paging = true 
	, OnSuccess= "fundClosing.onGridSuccess"
	, OnRowClick = "fundClosing.onRowClick"
	, OnInit = "fundClosing.onInit"
	, OnTemplate = "fundClosing.onTemplate"
	, ExportExcel = true
	, TableName = "FundClosing"
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:fundClosing.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
	<td style="width:15%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		<%: Html.TextBox("Name", "${row.cell[1]}", new { @class = "hide" })%>
	</td>
	<td style="width:30%">
		<%: Html.Span("${row.cell[2]}", new { @class = "show" })%>
		<%: Html.TextBox("FundName", "${row.cell[2]}", new { @class = "hide" })%>
		<%: Html.Hidden("FundId","${row.cell[5]}")%>
	</td>
	<td style="width:20%">
		<%: Html.Span("${formatDate(row.cell[3])}", new { @class = "show" })%>
		<%: Html.TextBox("FundClosingDate", "${formatDate(row.cell[3])}", new { @id="${i}_FundClosingDate", @class = "hide datefield" })%>
	</td>
	<td style="width: 30%;">
		<%: Html.Span("{{if row.cell[4]}}"+Html.Image("tick.png").ToHtmlString()+"{{/if}}", new { @class = "show" })%>		
		<%: Html.CheckBox("IsFirstClosing",false, new { @class = "hide", @val="${row.cell[4]}" })%>
	</td>
	<td style="text-align:right;">
		{{if row.cell[0]==0}}
		<%: Html.Image("add_active.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:fundClosing.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:fundClosing.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:fundClosing.edit(this);" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:fundClosing.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("FundClosingId", "${row.cell[0]}") %>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Reporting Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("ReportingType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">ADMIN</span><span class="arrow"></span><span class="pname">INVESTOR
					MANAGEMENT</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="admin-main">
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="ReportingTypeList">
				<thead>
					<tr>
						<th sortname="Reporting" style="width: 40%">
							Reporting Type
						</th>
						<th datatype="Boolean" sortname="Enabled" align="center" style="width: 10%;">
							Enable
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
	<%=Html.jQueryFlexiGrid("ReportingTypeList", new FlexigridOptions { 
    ActionName = "ReportingTypeList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "Reporting", Paging = true 
	, OnSuccess= "reportingType.onGridSuccess"
	, OnRowClick = "reportingType.onRowClick"
	, OnInit = "reportingType.onInit"
	, OnTemplate = "reportingType.onTemplate"
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:reportingType.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
	<td style="width: 40%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		<%: Html.TextBox("Reporting", "${row.cell[1]}", new { @class = "hide" })%>
	</td>
	<td style="width: 10%;text-align:center;">
		<%: Html.Span("{{if row.cell[2]}}"+Html.Image("tick.png").ToHtmlString()+"{{/if}}", new { @class = "show" })%>		
		<%: Html.CheckBox("Enabled",false, new { @class = "hide", @val="${row.cell[2]}" })%>
	</td>
	<td style="text-align:right;">
		{{if row.cell[0]==0}}
		<%: Html.Image("Add.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:reportingType.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:reportingType.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:reportingType.edit(this);" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:reportingType.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("ReportingTypeId", "${row.cell[0]}") %>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

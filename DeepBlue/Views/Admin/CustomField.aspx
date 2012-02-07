<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditCustomFieldModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Custom Field
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("CustomField.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
	<%=Html.StylesheetLinkTag("customfield.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">ADMIN</span><span class="arrow"></span><span class="pname">CUSTOM
					FIELD MANAGEMENT</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-content cfcontent">
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" border="0" id="CustomFieldList" class="grid">
				<thead>
					<tr>
						<th sortname="CustomFieldText" style="width: 18%">
							Custom Field
						</th>
						<th sortname="ModuleName" style="width: 18%">
							Module
						</th>
						<th sortname="DataTypeName" style="width: 18%">
							DataType
						</th>
						<th sortname="OptionalText" style="width: 18%">
							Optional Text
						</th>
						<th sortname="Search" style="width: 25%;">
							Search
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
	<%=Html.jQueryFlexiGrid("CustomFieldList", new FlexigridOptions { 
    ActionName = "CustomFieldList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "CustomFieldText", Paging = true 
	, OnSuccess= "customField.onGridSuccess"
	, OnRowClick = "customField.onRowClick"
	, OnInit = "customField.onInit"
	, OnTemplate = "customField.onTemplate"
	, TableName = "CustomField"
	, ExportExcel  = true
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:customField.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
	<td>
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		<%: Html.TextBox("CustomFieldText", "${row.cell[1]}", new { @class = "hide" })%>
	</td>
	<td>
		<%: Html.Span("${row.cell[3]}", new { @class = "show" })%>
		<%: Html.DropDownList("ModuleId", Model.Modules, new { @val="${row.cell[2]}", @class="hide", @refresh="true", @action="Module" })%>
	</td>
	<td>
		<%: Html.Span("${row.cell[5]}", new { @class = "show" })%>
		<%: Html.DropDownList("DataTypeId",Model.DataTypes, new { @val="${row.cell[4]}", @class="hide", @refresh="true", @action="DataType" }) %>
	</td>
	<td>
		<%: Html.Span("${row.cell[6]}", new { @class = "show" })%>
		<%: Html.TextBox("OptionalText", "${row.cell[6]}", new { @class = "hide" }) %>
	</td>
	<td>
		<%: Html.Span("{{if row.cell[7]}}"+Html.Image("tick.png").ToHtmlString()+"{{/if}}", new { @class = "show" })%>		
		<%: Html.CheckBox("Search", new {  @class = "hide", @val="${row.cell[7]}" }) %>
	</td>
	<td style="text-align:right">
		{{if row.cell[0]==0}}
		<%: Html.Image("add_active.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:customField.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:customField.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton editbtn show", @onclick = "javascript:customField.edit(this);" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:customField.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("CustomFieldId", "${row.cell[0]}") %>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

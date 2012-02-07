<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditCommunicationTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Communication Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("CommunicationType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">ADMIN</span><span class="arrow"></span><span class="pname">INVESTOR
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
			<table cellpadding="0" cellspacing="0" border="0" id="InvCommunicationTypeList" class="grid">
				<thead>
					<tr>
						<th sortname="CommunicationTypeName" style="width: 30%">
							Communication Type
						</th>
						<th sortname="CommunicationGroupingName" style="width: 30%">
							Communication Group
						</th>
						<th sortname="Enabled" style="width: 30%;">
							Enable
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
	<%=Html.jQueryFlexiGrid("InvCommunicationTypeList", new FlexigridOptions { 
    ActionName = "CommunicationTypeList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "CommunicationTypeName", Paging = true 
	, OnSuccess= "communicationType.onGridSuccess"
	, OnRowClick = "communicationType.onRowClick"
	, OnInit = "communicationType.onInit"
	, OnTemplate = "communicationType.onTemplate"
	, TableName = "CommunicationType"
	, ExportExcel = true
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:communicationType.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
	<td style="width: 30%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		<%: Html.TextBox("CommunicationTypeName", "${row.cell[1]}", new { @class = "hide" })%>
	</td>
	<td style="width: 30%">
		<%: Html.Span("${row.cell[2]}", new { @class = "show" })%>
		<%: Html.DropDownListFor(model => model.CommunicationGroupId,Model.CommunicationGroupings, new { @class="hide", @val = "${row.cell[4]}", @refresh="true", @action="CommunicationGroup" })%>
	</td>
	<td style="width: 30%;text-align:left;">
		<%: Html.Span("{{if row.cell[3]}}"+Html.Image("tick.png").ToHtmlString()+"{{/if}}", new { @class = "show" })%>		
		<%: Html.CheckBox("Enabled",false, new { @class = "hide", @val="${row.cell[3]}" })%>
	</td>
	<td style="text-align:right;">
		{{if row.cell[0]==0}}
		<%: Html.Image("add_active.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:communicationType.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:communicationType.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton editbtn show", @onclick = "javascript:communicationType.edit(this);" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:communicationType.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("CommunicationTypeId", "${row.cell[0]}") %>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

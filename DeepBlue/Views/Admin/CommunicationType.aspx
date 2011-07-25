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
			<table cellpadding="0" cellspacing="0" border="0" id="InvCommunicationTypeList">
				<thead>
					<tr>
						<th sortname="CommunicationTypeName" style="width: 40%">
							Communication Type
						</th>
						<th sortname="CommunicationGroupingName" style="width: 20%">
							Communication Group
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
	<%=Html.jQueryFlexiGrid("InvCommunicationTypeList", new FlexigridOptions { 
    ActionName = "CommunicationTypeList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "CommunicationTypeName", Paging = true 
	, OnSuccess= "communicationType.onGridSuccess"
	, OnRowClick = "communicationType.onRowClick"
	, OnInit = "communicationType.onInit"
	, OnTemplate = "communicationType.onTemplate"
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:communicationType.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
	<td style="width: 40%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		<%: Html.TextBox("CommunicationTypeName", "${row.cell[1]}", new { @class = "hide" })%>
	</td>
	<td style="width: 20%">
		<%: Html.Span("${row.cell[2]}", new { @class = "show" })%>
		<%: Html.DropDownListFor(model => model.CommunicationGroupId,Model.CommunicationGroupings, new { @class="hide", @val = "${row.cell[4]}" })%>
	</td>
	<td style="width: 10%;text-align:center;">
		<%: Html.Span("{{if row.cell[3]}}"+Html.Image("tick.png").ToHtmlString()+"{{/if}}", new { @class = "show" })%>		
		<%: Html.CheckBox("Enabled",false, new { @class = "hide", @val="${row.cell[3]}" })%>
	</td>
	<td style="text-align:right;">
		{{if row.cell[0]==0}}
		<%: Html.Image("Add.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:communicationType.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:communicationType.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:communicationType.edit(this);" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:communicationType.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("CommunicationTypeId", "${row.cell[0]}") %>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

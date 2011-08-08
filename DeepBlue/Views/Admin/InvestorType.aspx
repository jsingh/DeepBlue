<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Investor Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("InvestorType.js")%>
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
			<table cellpadding="0" cellspacing="0" border="0" id="InvInvestorTypeList">
				<thead>
					<tr>
						<th sortname="InvestorTypeName" style="width: 40%">
							Investor Type
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
	<%=Html.jQueryFlexiGrid("InvInvestorTypeList", new FlexigridOptions { 
    ActionName = "InvestorTypeList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "InvestorTypeName", Paging = true 
	, OnSuccess= "investorType.onGridSuccess"
	, OnRowClick = "investorType.onRowClick"
	, OnInit = "investorType.onInit"
	, OnTemplate = "investorType.onTemplate"
	, TableName = "InvestorType"
	, ExportExcel = true
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:investorType.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
	<td style="width: 40%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
		<%: Html.TextBox("InvestorTypeName", "${row.cell[1]}", new { @class = "hide" })%>
	</td>
	<td style="width: 10%;text-align:center;">
		<%: Html.Span("{{if row.cell[2]}}"+Html.Image("tick.png").ToHtmlString()+"{{/if}}", new { @class = "show" })%>		
		<%: Html.CheckBox("Enabled",false, new { @class = "hide", @val="${row.cell[2]}" })%>
	</td>
	<td style="text-align:right;">
		{{if row.cell[0]==0}}
		<%: Html.Image("Add.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:investorType.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:investorType.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:investorType.edit(this);" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:investorType.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("InvestorTypeId", "${row.cell[0]}") %>
	</td>
</tr>
{{/each}}
	</script>
</asp:Content>

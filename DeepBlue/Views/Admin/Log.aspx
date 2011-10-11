<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Log
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("Log.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">ADMIN</span><span class="arrow"></span><span class="pname">LOG</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main" style="width:95%;padding-left:20px;">
		<div class="admin-content cfcontent">
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" border="0" id="LogList" class="grid">
				<thead>
					<tr>
						<th>
							Controller
						</th>
						<th>
							Action
						</th>
						<th>
							View
						</th>
						<th>
							QueryString
						</th>
						<th>
							LogText
						</th>
						<th>
							Detail
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("LogList", new FlexigridOptions { 
    ActionName = "LogList", ControllerName = "Admin", 
    HttpMethod = "GET", SortName = "LogName", Paging = true 
	, OnSuccess= "log.onGridSuccess"
	, OnRowClick = "log.onRowClick"
	, OnInit = "log.onInit"
	, OnTemplate = "log.onTemplate"
	, TableName = "Log"
	, ExportExcel  = true
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr style='background-image:none;background-color:white;' id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
	{{each(j,ce) row.cell}}
	<td style='white-space:normal;background-image:none;border-bottom:solid 1px #000;padding:10px'>${ce}</td>
	{{/each}}
</tr>
{{/each}}
	</script>
</asp:Content>

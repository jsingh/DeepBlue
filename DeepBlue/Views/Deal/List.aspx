<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.JavascriptInclueTag("DealList.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
	<%= Html.StylesheetLinkTag("deallist.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTMENTS</span><span class="arrow"></span><span class="pname">
					MODIFY DEAL</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="deal-main">
		<div class="section-det" id="DealDetail">
			<% Html.RenderPartial("TBoxTop"); %>
			<table id="DealList" cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th sortname="DealName" style="width: 26%" colspan="4">
							Deal Name
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		<div class="section-det" id="AddNewDeal" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("DealList", new FlexigridOptions { ActionName = "DealList"
															   ,ControllerName = "Deal"
															   ,SortName = "DealName"
															   ,ResizeWidth = false
															   ,Paging = true
															   ,OnSubmit = "dealList.onSubmit"
															   ,OnTemplate = "dealList.onTemplate"
															   ,OnSuccess = "dealList.onGridSuccess"
															   ,BoxStyle = false
															   ,RowsLength = 50
})%>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
		{{each(i,row) rows}}
			{{if i%4==0}}
				<tr>
			{{/if}}
				<td><a href="/Deal/Edit/${row.cell[0]}">${row.cell[1]}</a></td>
			{{if i%4==3}}
				</tr>
			{{/if}}
		{{/each}}
	</script>
</asp:Content>

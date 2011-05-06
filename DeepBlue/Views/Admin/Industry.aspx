<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Industry
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("Industry.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:industry.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Industry</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="IndustryList">
				<thead>
					<tr>
						<th sortname="IndustryID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="Industry1" style="width: 80%">
							Industry
						</th>
						<th datatype="Boolean" sortname="Enabled" align="center" style="width: 10%;">
							Enable
						</th>
						<th align="center" style="width: 5%;">
						</th>
					</tr>
				</thead>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("IndustryList", new FlexigridOptions { ActionName = "IndustryList", ControllerName = "Admin"
	,HttpMethod = "GET"
	,SortName = "Industry1"
	,Paging = true
	,OnSuccess = "industry.onGridSuccess"
	,OnRowClick = "industry.onRowClick"
	,OnRowBound = "industry.onRowBound"
})%>
</asp:Content>

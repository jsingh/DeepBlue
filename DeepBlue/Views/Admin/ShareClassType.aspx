<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Share Class Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("ShareClassType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:shareClassType.add(0);" style="font-weight:bold;">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add ShareClass</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="ShareClassTypeList">
				<thead>
					<tr>
						<th sortname="ShareClassTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="ShareClass" style="width: 80%">
							ShareClass
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
	<%=Html.jQueryFlexiGrid("ShareClassTypeList", new FlexigridOptions { ActionName = "ShareClassTypeList", ControllerName = "Admin"
	,HttpMethod = "GET"
	,SortName = "ShareClass"
	,Paging = true
	,OnSuccess = "shareClassType.onGridSuccess"
	,OnRowClick = "shareClassType.onRowClick"
	,OnRowBound = "shareClassType.onRowBound"
})%>
</asp:Content>

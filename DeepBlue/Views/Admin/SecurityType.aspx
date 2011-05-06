<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("SecurityType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:securityType.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Security Type</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="SecurityTypeList">
				<thead>
					<tr>
						<th sortname="SecurityTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="Name" style="width: 80%">
							Security Type
						</th>
						<th sortname="Enabled" style="width: 10%" align=center>
							Enabled
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
	<%=Html.jQueryFlexiGrid("SecurityTypeList", new FlexigridOptions { ActionName = "SecurityTypeList", ControllerName = "Admin"
	,HttpMethod = "GET"
	,SortName = "Name"
	,Paging = true
	,OnSuccess = "securityType.onGridSuccess"
	,OnRowClick = "securityType.onRowClick"
	,OnRowBound = "securityType.onRowBound"
})%>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UnderlyingFundType
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("UnderlyingFundType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:underlyingFundType.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add UnderlyingFund</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="UnderlyingFundTypeList">
				<thead>
					<tr>
						<th sortname="UnderlyingFundTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="Name" style="width: 90%">
							Name
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
	<%=Html.jQueryFlexiGrid("UnderlyingFundTypeList", new FlexigridOptions { ActionName = "UnderlyingFundTypeList", ControllerName = "Admin"
	, HttpMethod = "GET"
	, SortName = "UnderlyingFundTypeID"
	, Paging = true
	, OnSuccess = "underlyingFundType.onGridSuccess"
	, OnRowClick = "underlyingFundType.onRowClick"
	, OnRowBound = "underlyingFundType.onRowBound"
})%>
</asp:Content>

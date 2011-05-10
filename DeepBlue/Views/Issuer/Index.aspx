<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("Issuer.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:issuer.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Issuer</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="IssuerList">
				<thead>
					<tr>
						<th sortname="IssuerId" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="Name" style="width: 40%">
							Name
						</th>
						<th sortname="ParentName" style="width: 20%">
							Parent Name
						</th>
						<th sortname="Country" style="width: 20%">
							Country
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
<%=Html.jQueryFlexiGrid("IssuerList", new FlexigridOptions { ActionName = "IssuerList", ControllerName = "Issuer"
	, HttpMethod = "GET"
	, SortName = "Name"
	, Paging = true
	, OnRowBound = "issuer.onRowBound"
})%>
</asp:Content>

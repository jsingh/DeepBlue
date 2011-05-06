<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Underlying Funds
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("UnderlyingFund.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:underlyingFund.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Underlying Fund</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="UnderlyingFundList">
				<thead>
					<tr>
						<th sortname="UnderlyingFundId" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="FundName" style="width: 40%">
							Fund Name
						</th>
						<th sortname="FundType" style="width: 25%">
							Fund Type
						</th>
						<th sortname="IssuerName" style="width: 25%">
							Issuer Name
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
	<%=Html.jQueryFlexiGrid("UnderlyingFundList", new FlexigridOptions { ActionName = "UnderlyingFundList", ControllerName = "Deal", HttpMethod = "GET", SortName = "FundName", Paging = true, OnRowBound = "underlyingFund.onRowBound" })%>
</asp:Content>

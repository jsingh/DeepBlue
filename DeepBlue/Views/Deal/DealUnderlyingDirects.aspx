<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal Underlying Directs
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("CreateDealUnderlyingDirect.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:dealUnderlyingDirect.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Deal Underlying Direct</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="DealUnderlyingDirectList">
				<thead>
					<tr>
						<th sortname="DealUnderlyingDirectID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="DealName" style="width: 45%">
							Deal Name
						</th>
						<th sortname="CloseDate" style="width: 15%">
							Deal Close Date
						</th>
						<th sortname="IssuerName" style="width: 15%">
							Issuer Name
						</th>
						<th sortname="SecurityType" style="width: 15%">
							Security Type
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
	<%=Html.jQueryFlexiGrid("DealUnderlyingDirectList", new FlexigridOptions { ActionName = "DealUnderlyingDirectList"
	, ControllerName = "Deal"
	, HttpMethod = "GET"
	, SortName = "DealName"
	, Paging = true
	, OnRowBound = "dealUnderlyingDirect.onRowBound"
	})%>
</asp:Content>

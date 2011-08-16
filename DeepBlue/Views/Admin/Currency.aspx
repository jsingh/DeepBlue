<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Currency
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("Currency.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:currency.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Currency</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="CurrencyList">
				<thead>
					<tr>
						<th sortname="CurrencyID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="Currency1" style="width: 40%">
							Currency
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
	<%=Html.jQueryFlexiGrid("CurrencyList", new FlexigridOptions { ActionName = "CurrencyList", ControllerName = "Admin"
	, HttpMethod = "GET"
	,
																   SortName = "CurrencyID"
	, Paging = true
	, OnSuccess = "currency.onGridSuccess"
	, OnRowClick = "currency.onRowClick"
})%>
</asp:Content>

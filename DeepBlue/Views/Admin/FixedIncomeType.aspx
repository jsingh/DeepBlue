<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fixed Income Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FixedIncomeType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:fixedIncomeType.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Fixed Income Type</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="FixedIncomeTypeList">
				<thead>
					<tr>
						<th sortname="FixedIncomeTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="FixedIncomeType1" style="width: 40%">
							Fixed Income Type
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
	<%=Html.jQueryFlexiGrid("FixedIncomeTypeList", new FlexigridOptions { ActionName = "FixedIncomeTypeList", ControllerName = "Admin"
	, HttpMethod = "GET"
	, SortName = "FixedIncomeType1"
	, Paging = true
	, OnSuccess = "fixedIncomeType.onGridSuccess"
	, OnRowClick = "fixedIncomeType.onRowClick"
})%>
</asp:Content>

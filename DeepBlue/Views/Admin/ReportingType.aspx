<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ReportingType
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("ReportingType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:reportingType.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Reporting</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="ReportingTypeList">
				<thead>
					<tr>
						<th sortname="ReportingTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="Reporting" style="width: 80%">
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
	<%=Html.jQueryFlexiGrid("ReportingTypeList", new FlexigridOptions { ActionName = "ReportingTypeList", ControllerName = "Admin"
	,HttpMethod = "GET"
	,SortName = "Reporting"
	,Paging = true
	,OnSuccess = "reportingType.onGridSuccess"
	,OnRowClick = "reportingType.onRowClick"
	,OnRowBound = "reportingType.onRowBound"
})%>
</asp:Content>

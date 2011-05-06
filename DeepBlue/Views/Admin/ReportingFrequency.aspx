<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ReportingFrequency
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("ReportingFrequency.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:reportingFrequency.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add ReportingFrequency</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="ReportingFrequencyList">
				<thead>
					<tr>
						<th sortname="ReportingFrequencyID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="ReportingFrequency1" style="width: 80%">
							Reporting Frequency
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
	<%=Html.jQueryFlexiGrid("ReportingFrequencyList", new FlexigridOptions { ActionName = "ReportingFrequencyList", ControllerName = "Admin"
	,HttpMethod = "GET"
	,SortName = "ReportingFrequency1"
	,Paging = true
	,OnSuccess = "reportingFrequency.onGridSuccess"
	,OnRowClick = "reportingFrequency.onRowClick"
	,OnRowBound = "reportingFrequency.onRowBound"
})%>
</asp:Content>

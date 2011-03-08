<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.JavascriptInclueTag("Fund.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
	<%= Html.StylesheetLinkTag("fund.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="fund-main">
		<div class="fund-header">
			<%: Html.Image("add_icon.png", new { @style = "cursor:pointer", @onclick = "javascript:fund.add(0);" }) %>
		</div>
		<div class="fund-content">
			<table id="FundList" cellpadding="0" cellspacing="0" border="0" style="width: 100%">
				<thead>
					<tr>
						<th sortname="FundName">
							Fund Name
						</th>
						<th sortname="TaxId" align="center" style="width: 10%">
							Tax ID
						</th>
						<th sortname="FundStartDate" align="center" style="width: 15%">
							Fund Start Date
						</th>
						<th sortname="ScheduleTerminationDate" align="center" style="width: 18%">
							Schedule termination Date
						</th>
						<th align="center" style="width: 5%">
						</th>
					</tr>
				</thead>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("FundList", new FlexigridOptions { ActionName = "Fund", ControllerName = "List", SortName= "FundName", Paging=true })%>
</asp:Content>

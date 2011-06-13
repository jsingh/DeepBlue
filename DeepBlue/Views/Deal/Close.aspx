<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateDealCloseModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Close
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("DealClose.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<div class="editor-label" style="width: auto;margin-left:40%;">
				<%: Html.Label("Deal:") %>&nbsp;<%: Html.TextBox("Deal","", new { @id="Deal", @style = "width:200px" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
				<%: Html.Hidden("DealId",0)%>
			</div>
			<a href="javascript:dealClose.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add Deal Close</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="DealCloseList">
				<thead>
					<tr>
						<th sortname="DealClosingId" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="DealName" style="width: 40%">
							Deal Name
						</th>
						<th sortname="FundName" style="width: 30%">
							Fund Name
						</th>
						<th sortname="CloseDate" style="width: 20%">
							Close Date
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
	<%=Html.jQueryAutoComplete("Deal", new AutoCompleteOptions { Source = "/Deal/FindDeals", MinLength = 1, OnSelect = "function(event, ui) { dealClose.selectDeal(ui.item.id);}" })%>
	<%=Html.jQueryFlexiGrid("DealCloseList", new FlexigridOptions {
	ActionName = "DealClosingList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, SortName = "DealName"
	, Paging = true
	, OnSuccess = "dealClose.onGridSuccess"
	, OnRowClick = "dealClose.onRowClick"
	, Autoload = false
	})%>
</asp:Content>

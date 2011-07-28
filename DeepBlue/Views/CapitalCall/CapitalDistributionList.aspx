<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CapitalDistributionListModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Capital Distribution
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("CapitalDistributionDetail.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="cc-header">
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FundName) %>&nbsp;<%: Html.TextBoxFor(model => model.FundName, new { @style = "width:200px" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
		</div>
		<%: Html.HiddenFor(model => model.FundId) %>
	</div>
	<div class="cc-main" id="CaptialCallDetail" style="display: none">
		<div class="box">
			<div class="box-top">
				<div class="box-left">
				</div>
				<div class="box-center">
					Fund:&nbsp;
					<%:Html.Span("",new { id = "TitleFundName" })%>
				</div>
				<div class="box-right">
				</div>
			</div>
			<div class="box-content">
				<div class="editor-label">
					Fund Name:&nbsp;<b><%:Html.Span("",new { id = "SpnFundName" })%></b>
				</div>
				<div class="list">
					<table cellpadding="0" cellspacing="0" border="0" id="FundDetail">
						<thead>
							<tr>
								<th style="width: 20%; display: none;" align="center" sortname="CapitalDistributionID">
									Capital Distribution Id
								</th>
								<th style="width: 20%" align="center" sortname="CapitalDistributionID">
									Capital Distribution Number
								</th>
								<th style="width: 20%" align="right" sortname="DistributionAmount">
									Capital Distribution Amount
								</th>
								<th style="width: 15%" align="right" sortname="ReturnManagementFees">
									Return Management Fees
								</th>
								<th style="width: 15%" align="right" sortname="ReturnFundExpenses">
									Return Fund Expenses
								</th>
								<th style="width: 15%" align="center" sortname="CapitalDistributionDate">
									Capital Distribution Date
								</th>
								<th style="width: 15%" align="center" sortname="CapitalDistributionDueDate">
									Capital Distribution Due Date
								</th>
							</tr>
						</thead>
					</table>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { capitalDistributionDetail.selectFund(ui.item.id);}" })%>
	<%= Html.jQueryFlexiGrid("FundDetail", new FlexigridOptions {	ActionName = "CapitalDistributionDetailList", ControllerName = "CapitalCall", Autoload = false, SortName = "CapitalDistributionID", SortOrder = "desc",	HttpMethod = "Get", Paging = true, Height = 480, Width=600 })%>
	<script type="text/javascript">
		capitalDistributionDetail.init();
	</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.ListModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Capital Call Detail
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("CapitalCallDetail.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="cc-header">
		<div class="page-title">
			<h2>
				Capital Call Detail</h2>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FundName) %>&nbsp;<%: Html.TextBoxFor(model => model.FundName, new { @style = "width:200px" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
		</div>
		<%: Html.HiddenFor(model => model.FundId) %>
	</div>
	<div class="cc-main" id="CaptialCallDetail">
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
				<div class="editor-label" style="clear: right">
					Capital Committed:&nbsp;<b><%:Html.Span("", new { id = "CapitalCommitted" })%></b>
				</div>
				<div class="editor-label" style="clear: right">
					Unfunded Amount:&nbsp;<b><%:Html.Span("", new { id = "UnfundedAmount" })%></b>
				</div>
				<div class="editor-label" style="clear: right">
					Management Fees:&nbsp;<b><%:Html.Span("", new { id = "ManagementFees" })%></b>
				</div>
				<div class="editor-label" style="clear: right">
					Fund Expenses:&nbsp;<b><%:Html.Span("", new { id = "FundExpenses" })%></b>
				</div>
				<div class="list">
					<table cellpadding="0" cellspacing="0" border="0" id="FundDetail">
						<thead>
							<tr>
								<th style="width: 20%;display:none;" align="center" sortname="CapitalCallID">
									Capital Call Id
								</th>
								<th style="width: 20%" align="center" sortname="CapitalCallID">
									Capital Call Number
								</th>
								<th style="width: 20%" align="right" sortname="CapitalAmountCalled">
									Capital Call Amount
								</th>
								<th style="width: 15%" align="right" sortname="ManagementFees">
									Management Fees
								</th>
								<th style="width: 15%" align="right" sortname="FundExpenses">
									Fund Expenses
								</th>
								<th style="width: 15%" align="center" sortname="CapitalCallDate">
									Capital Call Date
								</th>
								<th style="width: 15%" align="center" sortname="CapitalCallDueDate">
									Capital Call Due Date
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
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { capitalCallDetail.selectFund(ui.item.id);}" })%>
	<%= Html.jQueryFlexiGrid("FundDetail", new FlexigridOptions { 
	ActionName="CapitalCallList", ControllerName="CapitalCall", Autoload=false, SortName="CapitalCallID", SortOrder="desc",
	HttpMethod = "Get", Paging = true, Height = 480, OnRowClick = "capitalCallDetail.selectCapitalCall"
})%>

	<script type="text/javascript">
		capitalCallDetail.init();
	</script>

</asp:Content>

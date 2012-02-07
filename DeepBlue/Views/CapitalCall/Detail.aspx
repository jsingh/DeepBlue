<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.DetailModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Detail
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.StylesheetLinkTag("capitalcalldetail.css")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("CapitalCallDetail.js")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title"> FUNDS</span><span class="arrow"></span><span class="pname">Detail</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="cc-box">
		<div class="header">
			<div class="tabbg">
				<div class="tabinnerbox">
					<%using (Html.Tab(new { @id = "NewCCDetailTab", @class = (Model.DetailType == DeepBlue.Models.CapitalCall.Enums.DetailType.CapitalCall ? "section-tab-sel" : ""), @onclick = "javascript:capitalCallDetail.selectTab('C',this);" })) {%>Capital
					Call Detail
					<%}%>
					<%using (Html.Tab(new { @id = "ManCDetailTab", @class = (Model.DetailType == DeepBlue.Models.CapitalCall.Enums.DetailType.CapitalDistribution ? "section-tab-sel" : ""), @onclick = "javascript:capitalCallDetail.selectTab('M',this);" })) {%>Capital
					Distribution Detail
					<%}%>
					<%using (Html.Div(new { @id = "SerCDTab" })) {%>
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...&nbsp;", new { @id = "SpnLoading", @style = "display:none;float:left;" })%><%: Html.TextBox("Fund","SEARCH  FUND", new { @class="wm", @style = "width:200px" })%>
					<%}%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="cc-main" id="CaptialCallDetail" style="display: none">
		<div class="cc-box">
			<div class="section ccdetail">
				<div class="cell">
					<label>
						<%:Html.Span("",new { id = "TitleFundName" })%></label>
					<%: Html.HiddenFor(model => model.FundId)%>
				</div>
			</div>
		</div>
		<%using (Html.Div(new { @id = "CapitalCallReport", @style = (Model.DetailType == DeepBlue.Models.CapitalCall.Enums.DetailType.CapitalCall ? "display:block" : "display:none") })) {%>
		<%}%>
		<%using (Html.Div(new { @id = "CapitalDistributionReport", @style = (Model.DetailType == DeepBlue.Models.CapitalCall.Enums.DetailType.CapitalDistribution ? "display:block" : "display:none") })) {%>
		<%}%>
		<div class="line">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { capitalCallDetail.selectFund(ui.item.id);}" })%>
	<script type="text/javascript">		capitalCallDetail.init();</script>
	<script id="CapitalCallReportTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CapitalCallDetail"); %>
	</script>
	<script id="CapitalDistributionReportTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CapitalDistributionDetail"); %>
	</script>
	<script id="CCInvestorTemplate" type="text/x-jquery-tmpl">
	<div class="gbox" style="margin:10px 0;">
	<table cellpadding="0" cellspacing="0" border="0" class="grid">
		<thead>
			<tr class="green-hrow">
				<th style="width: 15%; text-align: left;">
					Investor Name
				</th>
				<th style="width:20%;text-align: right">
					Capital Call Amount
				</th>
				<th style="width:15%;text-align: right">
					Management Fees
				</th>
				<th style="width:15%;text-align: right">
					Fund Expenses
				</th>
				<th style="width:15%;text-align: right">
					Capital Call Date
				</th>
				<th style="width:15%;text-align: right">
					Capital Call Due Date
				</th>
				<th style="width:5%;">
				</th>
			</tr>
		</thead>
		<tbody>
		{{each(i,cc) Investors}}
		<tr {{if i%2==0}}class="row"{{else}}class="green-arow"{{/if}}>
			<td>
				${cc.InvestorName}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.CapitalCallAmount)}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.ManagementFees)}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.FundExpenses)}
			</td>
			<td style="text-align: right">
				${formatDate(cc.CapitalCallDate)}
			</td>
			<td style="text-align: right">
				${formatDate(cc.CapitalCallDueDate)}
			</td>
			<td>
			</td>
		</tr>
		{{/each}}
		</tbody>
	</table>
	</div>
	</script>
	<script id="CDInvestorTemplate" type="text/x-jquery-tmpl">
	<div class="gbox" style="margin:10px 0;">
	<table cellpadding="0" cellspacing="0" border="0" class="grid">
		<thead>
			<tr class="green-hrow">
				<th style="text-align:left;width:12%;">
					Investor Name
				</th>
				<th style="text-align: right;width:15%;">
					Capital Distribution Amount
				</th>
				<th style="text-align: right;width:14%;">
					Return Management Fees
				</th>
				<th style="text-align: right;width:12.5%;">
					Return Fund Expenses
				</th>
				<th style=" text-align: right;width:13%;">
					Capital Distribution Date
				</th>
				<th style="text-align: right;width:15%;">
					Capital Distribution Due Date
				</th>
				<th style="text-align: right;width:6.5%;">
					LP Profits
				</th>
				<th style="text-align: right;width:9.5%;">
					Cost Returned
				</th>
				<th style="width:1%">&nbsp;
				</th>
			</tr>
		</thead>
		<tbody>
		{{each(i,cc) Investors}}
		<tr {{if i%2==0}}class="row"{{else}}class="green-arow"{{/if}}>
			<td>
				${cc.InvestorName}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.CapitalDistributed)}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.ReturnManagementFees)}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.ReturnFundExpenses)}
			</td>
			<td style="text-align: right">
				${formatDate(cc.CapitalDistributionDate)}
			</td>
			<td style="text-align: right">
				${formatDate(cc.CapitalDistributionDueDate)}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.LPProfit)}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.CostReturn)}
			</td>
			<td>
			</td>
		</tr>
		{{/each}}
		</tbody>
	</table>
	</div>
	</script>
</asp:Content>

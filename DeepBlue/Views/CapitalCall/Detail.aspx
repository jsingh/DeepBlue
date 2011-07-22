﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.DetailModel>" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">FUNDS</span><span class="arrow"></span><span class="pname">Detail</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="cc-box">
		<div class="header">
			<div class="tabbg">
				<%using (Html.Div(new { @id = "NewCCDetailTab", @class = (Model.DetailType == DeepBlue.Models.CapitalCall.Enums.DetailType.CapitalCall ? "select" : ""), @onclick = "javascript:capitalCallDetail.selectTab('C',this);" })) {%>&nbsp;
				<%}%>
				<%using (Html.Div(new { @id = "ManCDetailTab", @class = (Model.DetailType == DeepBlue.Models.CapitalCall.Enums.DetailType.CapitalDistribution ? "select" : ""), @onclick = "javascript:capitalCallDetail.selectTab('M',this);" })) {%>&nbsp;
				<%}%>
				<%using (Html.Div(new { @id = "SerCDTab" })) {%>&nbsp;
				<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>&nbsp;<%: Html.TextBox("Fund","SEARCH FUND", new { @class="wm", @style = "width:200px" })%>
				<%}%>
			</div>
		</div>
	</div>
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
		<div class="line">
		</div>
		<%using(Html.Div(new { @id = "CapitalCallReport", @style=(Model.DetailType == DeepBlue.Models.CapitalCall.Enums.DetailType.CapitalCall ? "display:block" : "display:none") })){%>
		<%}%>
		<%using (Html.Div(new { @id = "CapitalDistributionReport", @style = (Model.DetailType == DeepBlue.Models.CapitalCall.Enums.DetailType.CapitalDistribution ? "display:block" : "display:none") })) {%>
		<%}%>
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
			<tr>
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
		<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
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
			<tr>
				<th style="text-align:left;width:12%;">
					Investor Name
				</th>
				<th style="text-align: right">
					Capital Distribution Amount
				</th>
				<th style="text-align: right">
					Return Management Fees
				</th>
				<th style="text-align: right">
					Return Fund Expenses
				</th>
				<th style=" text-align: right">
					Capital Distribution Date
				</th>
				<th style="text-align: right">
					Capital Distribution Due Date
				</th>
				<th style="text-align: right">
					Profits (%)
				</th>
				<th style="text-align: right">
					Profits Returned
				</th><th></th>
			</tr>
		</thead>
		<tbody>
		{{each(i,cc) Investors}}
		<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
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
				${formatPercentage(cc.Profit)}
			</td>
			<td style="text-align: right">
				${formatCurrency(cc.ProfitReturn)}
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
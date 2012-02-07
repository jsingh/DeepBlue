<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.FundBreakDownReportDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="ReportContent">
	<table cellpadding="0" cellspacing="0" <%=(Model.IsTemplateDisplay==false ? "border='1'" : string.Empty)%>
		style="width: 100%">
		<tr>
			<td colspan="5" class="title">
				Fund Breakdown
			</td>
			<td rowspan="2" colspan="1">
				<div class="symbol">
					W</div>
			</td>
		</tr>
		<tr class="headingrow">
			<td colspan="6" class="title" style="text-transform: uppercase">
				<%: Html.jQueryTemplateDisplayFor(model => model.FundName, Model.IsTemplateDisplay)%>
			</td>
		</tr>
		<%if (Model.IsTemplateDisplay) {%>
		<tr class="gridline">
			<td colspan="6">
			</td>
		</tr>
		<%}%>
		<tr class="drow">
			<td>
				Total # of Funds:
			</td>
			<td class="lalign">
				<%: Html.jQueryTemplateDisplayFor(model => model.TotalUnderlyingFunds, Model.IsTemplateDisplay)%>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td class="lalign">
				Average Deal Size:
			</td>
			<td class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.AvgDealSize, Model.IsTemplateDisplay, "formatCurrency")%>
			</td>
		</tr>
		<tr class="drow">
			<td>
				Total # of Directs:
			</td>
			<td class="lalign">
				<%: Html.jQueryTemplateDisplayFor(model => model.TotalDirects, Model.IsTemplateDisplay)%>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td class="lalign">
				Avg Fund Age at Purchase:
			</td>
			<td class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.AvgFundAgeAtPurchase, Model.IsTemplateDisplay)%>
			</td>
		</tr>
		<%if (Model.IsTemplateDisplay) {%>
		<tr class="gridline">
			<td colspan="6">
			</td>
		</tr>
		<%}%>
		<tr class="drow">
			<td style="width: 15%">
				&nbsp;
			</td>
			<td class="ralign" style="width: 25%">
				<b>Funds:</b>
			</td>
			<td style="width: 8%">
			</td>
			<td style="width: 8%">
				&nbsp;
			</td>
			<td style="width: 15%">
				<b>Deals:</b>
			</td>
			<td>
				&nbsp;
			</td>
		</tr>
		<tr class="drow">
			<td colspan="2" class="ralign">
				% Venture:
			</td>
			<td class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.Venture, Model.IsTemplateDisplay, "formatPercentage")%>
			</td>
			<td>
			</td>
			<td class="ralign">
				% Partnered:
			</td>
			<td class="lalign">
				<%: Html.jQueryTemplateDisplayFor(model => model.Partnered, Model.IsTemplateDisplay, "formatPercentage")%>
			</td>
		</tr>
		<tr class="drow">
			<td colspan="2" class="ralign">
				% Buyout:
			</td>
			<td class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.Buyout, Model.IsTemplateDisplay, "formatPercentage")%>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
		<tr class="drow">
			<td colspan="2" class="ralign">
				% Mezzanine:
			</td>
			<td class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.Mezzanine, Model.IsTemplateDisplay, "formatPercentage")%>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
		<tr class="drow">
			<td colspan="2" class="ralign">
				% Funds of Funds:
			</td>
			<td class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.FundOfFunds, Model.IsTemplateDisplay, "formatPercentage")%>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
		<tr class="drow">
			<td colspan="2" class="ralign">
				% Buyout-Venture:
			</td>
			<td class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.BuyoutVenture, Model.IsTemplateDisplay, "formatPercentage")%>
			</td>
			<td>
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr> 
		<tr class="frow">
			<td colspan="3" style="font-style: italic">
				<%=EntityHelper.EntityName%> Incorporated
			</td>
			<td colspan="3" style="text-align: right">
				<%=DateTime.Now.ToLongDateString()%>
			</td>
		</tr>
	</table>
</div>

﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.DealDetailReportModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<table cellpadding="0" cellspacing="0" <%=(Model.IsTemplateDisplay==false ? "border='1'" : string.Empty)%> style="width: 100%">
	<tr>
		<td colspan="12" class="title">
			Deal Detail
		</td>
	</tr>
	<tr class="headingrow">
		<td colspan=11>
			<span style="border-bottom:solid 1px #000"><%: Html.jQueryTemplateDisplayFor(model => model.FundName, Model.IsTemplateDisplay)%></span>
		</td>
		<td>
			<div class="symbol">
				W</div>
		</td>
	</tr>
	<tr class="headingrow">
		<td style="width: 5%;">
			Deal
		</td>
		<td>
			<%: Html.jQueryTemplateDisplayFor(model => model.DealNumber, Model.IsTemplateDisplay)%>
		</td>
		<td>
			Deal Name:
		</td>
		<td colspan=5>
			<%: Html.jQueryTemplateDisplayFor(model => model.DealName, Model.IsTemplateDisplay)%>
		</td>
		<td>
			&nbsp;
		</td>
		<td>
			Contact
		</td>
		<td>
			<%: Html.jQueryTemplateDisplayFor(model => model.Contact, Model.IsTemplateDisplay)%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="headingrow bordertop">
		<td colspan="12">
			DEAL PERFORMANCE
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			Date of first fund close:
		</td>
		<td colspan="2" class="ralign" style="width: 20%">
			<%: Html.jQueryTemplateDisplayFor(model => model.FirstFundCloseDate, Model.IsTemplateDisplay, "formatDate")%>
		</td>
		<td colspan="2" style="width: 5%">
			&nbsp;
		</td>
		<td style="width: 20%;">
			Unfunded % at Closing:
		</td>
		<td colspan="2" class="ralign" style="width: 20%;">
			<%: Html.jQueryTemplateDisplayFor(model => model.UnfundedClosingPercentage, Model.IsTemplateDisplay, "formatPercentage")%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			Net Purchase Price:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.NetPurchasePrice, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			Original Discount:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.OrginalDiscount, Model.IsTemplateDisplay, "formatPercentage")%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			Closing Costs:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.ClosingCosts, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			Total Cash Out:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.TotalCashOut, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			Total Initial Investment:
		</td>
		<td colspan="2" class="bordertop ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.TotalInitialInvestment, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			Dist. Received:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.ReceivedDistributions, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			AMB Contributions:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.AMBContributions, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			Realized ROI:
		</td>
		<td colspan="2" class="bordertop ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.RealizedROI, Model.IsTemplateDisplay, "formatROI")%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			Total Cash Out:
		</td>
		<td colspan="2" class="bordertop ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.TotalCashOut, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			&nbsp;
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			Current Unfunded:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.CurrentUnfunded, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			Estimated NAV:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.EstimatedNAV, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			Original Commitment:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.OriginalCommitment, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			Dist. + Estimated NAV:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.DistEstimatedNAV, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow">
		<td>
			&nbsp;
		</td>
		<td colspan="3">
			Unfunded at Closing:
		</td>
		<td colspan="2" class="ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.UnfundedClosing, Model.IsTemplateDisplay, "formatCurrency")%>
		</td>
		<td colspan="2">
			&nbsp;
		</td>
		<td>
			Unrealized ROI:
		</td>
		<td colspan="2" class="bordertop ralign">
			<%: Html.jQueryTemplateDisplayFor(model => model.UnrealizedROI, Model.IsTemplateDisplay, "formatROI")%>
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr class="detailrow bordertop">
		<td colspan="12">
			DEAL DETAIL
		</td>
	</tr>
	<%if (Model.IsTemplateDisplay) {%>
	<tr class="gridline">
		<td colspan=12></td>
	</tr>
	<%}%>
	<%if (Model.IsTemplateDisplay) {%>
	<tr class="hrow">
	<%}else{%>
	<tr class="hrow underline">
	<%}%>
		<td>
			Trans.Date
		</td>
		<td colspan="4">
			Fund
		</td>
		<td colspan="2">
			Amount
		</td>
		<td colspan="2">
			Unused Capital
		</td>
		<td colspan="3">
			Type
		</td>
	</tr>
	<%if (Model.IsTemplateDisplay) {%>
	{{each(i,item) Details}}
	<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
		<td>
			${formatDate(item.Date)}
		</td>
		<td colspan="4">
			${item.FundName}
		</td>
		<td colspan="2">
			${formatCurrency(item.Amount)}
		</td>
		<td colspan="2">
			${formatCurrency(item.UnUsedCapital)}
		</td>
		<td colspan="3">
			${item.Type}
		</td>
	</tr>
	{{/each}}
	<%}
   else {%>
	<%if (Model.Details != null) { %>
	<%foreach (var detail in Model.Details) {%>
	<tr>
		<td>
			<%if (detail.Date.HasValue) {%>
			<%=detail.Date.Value.ToString("MM/dd/yyyy")%>
			<%}%>
		</td>
		<td colspan="4">
			<%=detail.FundName%>
		</td>
		<td colspan="2">
			<%=detail.Amount.ToString()%>
		</td>
		<td colspan="2">
			<%=detail.UnUsedCapital.ToString()%>
		</td>
		<td colspan="3">
			<%=detail.Type%>
		</td>
	</tr>
	<%}%>
	<%}%>
	<%}%> 
	<tr class="frow">
			<td colspan="6" style="font-style: italic">
				<%=EntityHelper.EntityName%> Incorporated
			</td>
			<td colspan="6" style="text-align: right">
				<%=DateTime.Now.ToLongDateString()%>
			</td>
		</tr>
</table>

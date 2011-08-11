<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.DealOriginationReportModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="ReportContent">
	<table cellpadding="0" cellspacing="0" <%=(Model.IsTemplateDisplay==false ? "border='1'" : string.Empty)%>
		style="width: 100%">
		<tr>
			<td colspan="9" class="title">
				Deal Origination (Detailed)
			</td>
			<td rowspan="2" colspan=1>
				<div class="symbol">
					W</div>
			</td>
		</tr>
		<tr class="headingrow">
			<td>
				<div class="borderbottom">
					<%: Html.jQueryTemplateDisplayFor(model => model.FundName, Model.IsTemplateDisplay)%></div>
			</td>
			<td colspan="9">
				&nbsp;
			</td>
		</tr>
		<tr class="headingrow">
			<td colspan="2">
				AMB Contact Name:
			</td>
			<td>
				<%: Html.jQueryTemplateDisplayFor(model => model.Contact, Model.IsTemplateDisplay)%>
			</td>
			<td colspan="7">
				&nbsp;
			</td>
		</tr>
		<tr class="headingrow">
			<td style="width: 5%;" <%=(Model.IsTemplateDisplay==false ? "class=underline" : string.Empty)%>>
				Deal No.
			</td>
			<td style="width: 5%;">
				<%: Html.jQueryTemplateDisplayFor(model => model.DealNumber, Model.IsTemplateDisplay)%>
			</td>
			<td style="width: 1%;" <%=(Model.IsTemplateDisplay==false ? "class=underline" : string.Empty)%>>
				Deal Name:
			</td>
			<td style="width: 15%;">
				<%: Html.jQueryTemplateDisplayFor(model => model.DealName, Model.IsTemplateDisplay)%>
			</td>
			<td>
				&nbsp;
			</td>
			<td style="width: 1%;" <%=(Model.IsTemplateDisplay==false ? "class=underline" : string.Empty)%>>
				Gross Purchase Price:
			</td>
			<td style="width: 10%;" class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.GrossPurchasePrice, Model.IsTemplateDisplay, "formatCurrency")%>
			</td>
			<td>
				&nbsp;
			</td>
			<td style="width: 1%;" <%=(Model.IsTemplateDisplay==false ? "class=underline" : string.Empty)%>>
				Net Purchase Price:
			</td>
			<td style="width: 10%" class="ralign">
				<%: Html.jQueryTemplateDisplayFor(model => model.NetPurchasePrice, Model.IsTemplateDisplay, "formatCurrency")%>
			</td>
		</tr>
		<%if (Model.IsTemplateDisplay) {%>
		<tr class="gridline">
			<td colspan="10">
			</td>
		</tr>
		<%}%>
		<tr class="hrow trow">
			<td colspan="2">
			</td>
			<td colspan="2" style="text-align: left; padding-left: 6%;">
				Dates
			</td>
			<td class="ralign">
				Record Date
			</td>
			<td class="ralign">
				Gross
			</td>
			<td class="ralign">
				Post Record
			</td>
			<td class="ralign">
				Net
			</td>
			<td>
			</td>
			<td class="ralign">
				Unfunded
			</td>
		</tr>
		<tr class="hrow">
			<td>
				No.
			</td>
			<td>
				Fund
			</td>
			<td>
				Record
			</td>
			<td>
				Purchase
			</td>
			<td class="ralign">
				NAV
			</td>
			<td class="ralign">
				Purch. Price
			</td>
			<td class="ralign">
				Adjustment
			</td>
			<td class="ralign">
				Purch. Price
			</td>
			<td class="ralign">
				Commitment
			</td>
			<td class="ralign">
				At Purchase
			</td>
		</tr>
		<%if (Model.IsTemplateDisplay) {%>
	{{each(i,detail) Details}}
	<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
			<td>
				${i+1}
			</td>
			<td>
				${detail.FundName}
			</td>
			<td>
				${formatDate(detail.RecordDate)}
			</td>
			<td>
				${formatDate(detail.PurchaseDate)}
			</td>
			<td class="ralign">
				${formatCurrency(detail.NAV)}
			</td>
			<td class="ralign">
				${formatCurrency(detail.GrossPurchasePrice)}
			</td>
			<td class="ralign">
				${formatCurrency(detail.PostRecordAdjustMent)}
			</td>
			<td class="ralign">
				${formatCurrency(detail.NetPurchasePrice)}
			</td>
			<td class="ralign">
				${formatCurrency(detail.CommitmentAmount)}
			</td>
			<td class="ralign">
				${formatCurrency(detail.UnfundedAmount)}
			</td>
	</tr>
	{{/each}}
	<%}
   else {%>
	<%if (Model.Details != null) { %>
	<% int index = 0;
	 foreach (var detail in Model.Details) {
		 index++;
		 %>
		<tr>
			<td>
				<%=index.ToString()%>
			</td>
			<td>
				<%=detail.FundName%>
			</td>
			<td>
				<%=(detail.RecordDate.HasValue ? detail.RecordDate.Value.ToString("MM/dd/yyyy") : string.Empty)%>
			</td>
			<td>
				<%=(detail.PurchaseDate.HasValue ? detail.PurchaseDate.Value.ToString("MM/dd/yyyy") : string.Empty)%>
			</td>
			<td class="ralign">
				<%=detail.NAV%>
			</td>
			<td class="ralign">
				<%=detail.GrossPurchasePrice%>
			</td>
			<td class="ralign">
				<%=detail.PostRecordAdjustMent%>
			</td>
			<td class="ralign">
				<%=detail.NetPurchasePrice%>
			</td>
			<td class="ralign">
				<%=detail.CommitmentAmount%>
			</td>
			<td class="ralign">
				<%=detail.UnfundedAmount%>
			</td>
		</tr>
	<%}%>
	<%}%>
	<%}%>
		<%if (Model.IsTemplateDisplay == false) {%>
		<tr>
			<td colspan="5" style="font-style: italic">
				Willowridge Incorporated
			</td>
			<td colspan="5" style="text-align: right">
				<%=DateTime.Now.ToLongDateString()%>
			</td>
		</tr>
		<%}%>
	</table>
</div>

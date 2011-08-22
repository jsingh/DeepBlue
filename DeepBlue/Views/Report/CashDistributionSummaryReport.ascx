<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.CashDistributionReportDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="ReportContent">
	<table cellpadding="0" cellspacing="0" <%=(Model.IsTemplateDisplay==false ? "border='1'" : string.Empty)%>
		style="width: 100%">
		<tr>
			<td colspan="3" class="title">
				Cash Distribution Summary
			</td>
			<td rowspan="2" colspan=1>
				<div class="symbol">
					W</div>
			</td>
		</tr>
		<tr class="headingrow">
			<td colspan=4  class="title" style="text-transform:uppercase">
				<%: Html.jQueryTemplateDisplayFor(model => model.FundName, Model.IsTemplateDisplay)%>
			</td>
		</tr>
		<tr class="headingrow">
		<%if (Model.IsTemplateDisplay) {%>
			<td colspan=4  class="title">
				Distribution Of <%: Html.jQueryTemplateDisplayFor(model => model.DistributionDate, Model.IsTemplateDisplay, "formatDate")%> - <%: Html.jQueryTemplateDisplayFor(model => model.TotalDistributionAmount, Model.IsTemplateDisplay, "formatCurrency")%>
			</td>
			<%}else{%>
				<td>Distribution Of</td>
				<td><%: Html.jQueryTemplateDisplayFor(model => model.DistributionDate, Model.IsTemplateDisplay, "formatDate")%></td>
				<td>Total Distribution Amount</td>
				<td><%: Html.jQueryTemplateDisplayFor(model => model.TotalDistributionAmount, Model.IsTemplateDisplay)%></td>
			<%}%>
		</tr>
		<%if (Model.IsTemplateDisplay) {%>
		<tr class="gridline">
			<td colspan="4">
			</td>
		</tr>
		<%}%>
		<tr class="hrow">
			<td>
				Member
			</td>
			<td class="ralign">
				Designation
			</td>
			<td class="ralign">
				Commitment
			</td>
			<td class="ralign">
				Distribution Amount
			</td>
		</tr>
		<%if (Model.IsTemplateDisplay) {%>
			{{each(i,detail) Items}}
			<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>
						${detail.InvestorName}
					</td>
					<td>
						${detail.Designation}
					</td>
					<td class="ralign">
						${formatCurrency(detail.Commitment)}
					</td>
					<td class="ralign">
						${formatCurrency(detail.DistributionAmount)}
					</td>
			</tr>
			{{/each}}
			<tr class="hrow">
				<td class="lalign">Total Distribution:</td>
				<td class="lalign">${formatCurrency(TotalDistributionAmount)}</td>
				<td class="ralign">With Carry Amount:</td>
				<td class="ralign">${formatCurrency(WithCarryAmount)}</td>
			</tr>
			<tr class="hrow">
				<td></td><td></td>
				<td class="ralign">And Repayment of Mgt Fees:
				</td>
				<td class="ralign">${formatCurrency(RepayManFees)}</td>
			</tr>
		<%}else{%>
			<%if (Model.Items != null) { %>
				<% int index = 0;
	  foreach (var detail in Model.Items) {
					 index++;
					 %>
					<tr>
						<td>
							<%=detail.InvestorName%>
						</td>
						<td>
							<%=detail.Designation%>
						</td>
						<td>
							<%=detail.Commitment%>
						</td>
						<td class="ralign">
							<%=detail.DistributionAmount%>
						</td>
					</tr>
				<%}%>
				<tr>
					<td>Total Distribution:</td>
					<td><%=Model.TotalDistributionAmount%></td>
					<td>With Carry Amount:</td>
					<td><%=Model.WithCarryAmount%></td>
				</tr>
				<tr>
					<td></td><td></td>
					<td>And Repayment of Mgt Fees:
					</td>
					<td><%=Model.RepayManFees%></td>
				</tr>
			<%}%>
		<%}%> 
		<tr class="frow">
			<td colspan="2" style="font-style: italic">
				Willowridge Incorporated
			</td>
			<td colspan="2" style="text-align: right">
				<%=DateTime.Now.ToLongDateString()%>
			</td>
		</tr>
	</table>
</div>

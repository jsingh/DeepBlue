<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.CapitalCallReportDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="ReportContent">
	<table cellpadding="0" cellspacing="0" <%=(Model.IsTemplateDisplay==false ? "border='1'" : string.Empty)%>
		style="width: 100%">
		<tr>
			<td colspan="5" class="title">
				Capital Call Summary
			</td>
			<td rowspan="2" colspan=1>
				<div class="symbol">
					W</div>
			</td>
		</tr>
		<tr class="headingrow">
			<td colspan=6  class="title" style="text-transform:uppercase">
				<%: Html.jQueryTemplateDisplayFor(model => model.FundName, Model.IsTemplateDisplay)%>
			</td>
		</tr>
		<tr class="headingrow">
		<%if (Model.IsTemplateDisplay) {%>
			<td colspan=6  class="title">
				Capital Call Due <%: Html.jQueryTemplateDisplayFor(model => model.CapitalCallDueDate, Model.IsTemplateDisplay, "formatDate")%> - <%: Html.jQueryTemplateDisplayFor(model => model.TotalCapitalCall, Model.IsTemplateDisplay, "formatCurrency")%>
			</td>
			<%}else{%>
				<td>Capital Call Due</td>
				<td colspan=2><%: Html.jQueryTemplateDisplayFor(model => model.CapitalCallDueDate, Model.IsTemplateDisplay, "formatDate")%></td>
				<td>Total Capital Call Amount</td>
				<td colspan=2><%: Html.jQueryTemplateDisplayFor(model => model.TotalCapitalCall, Model.IsTemplateDisplay)%></td>
			<%}%>
		</tr>
		<%if (Model.IsTemplateDisplay) {%>
		<tr class="gridline">
			<td colspan="6">
			</td>
		</tr>
		<%}%>
		<tr class="hrow">
			<td>
				Investor
			</td>
			<td class="ralign">
				Commitment
			</td>
			<td class="ralign">
				Investments
			</td>
			<td class="ralign">
				Management Fees
			</td>
			<td class="ralign">
				Expenses
			</td>
			<td class="ralign">
				Total
			</td>
		</tr>
		<%if (Model.IsTemplateDisplay) {%>
			{{each(i,detail) Items}}
			<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
				<td>${InvestorName}</td>
				<td class="ralign">${formatCurrency(Commitment)}</td>
				<td class="ralign">${formatCurrency(Investments)}</td>
				<td class="ralign">${formatCurrency(ManagementFees)}</td>
				<td class="ralign">${formatCurrency(Expenses)}</td>
				<td class="ralign">${formatCurrency(Total)}</td>
			</tr>
			{{/each}}
			<tr class="drow">
				<td>Total CapitalCall:</td><td class="ralign">${formatCurrency(TotalCapitalCall)}</td><td colspan=4>&nbsp;</td>
			</tr>
			<tr class="drow">
				<td>Total Mgt Fees:</td><td class="ralign">${formatCurrency(TotalManagementFees)}</td><td colspan=4>&nbsp;</td>
			</tr>
			<tr class="drow">
				<td>Total Expenses:</td><td class="ralign">${formatCurrency(TotalExpenses)}</td><td colspan=4>&nbsp;</td>
			</tr>
			<tr>
				<td colspan=6>&nbsp;</td>
			</tr>
			<tr class="drow">
				<td>Amount for Investments:</td><td class="ralign">${formatCurrency(AmountForInv)}</td><td colspan=4>&nbsp;</td>
			</tr>
			<tr class="drow">
				<td>New Inv.:</td><td class="ralign">${formatCurrency(NewInv)}</td><td colspan=4>&nbsp;</td>
			</tr>
			<tr class="drow">
				<td>Existing Inv.:</td><td class="ralign">${formatCurrency(ExistingInv)}</td><td colspan=4>&nbsp;</td>
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
							<%=detail.Commitment%>
						</td>
						<td>
							<%=detail.Investments%>
						</td>
						<td>
							<%=detail.ManagementFees%>
						</td>
						<td>
							<%=detail.Expenses%>
						</td>
						<td>
							<%=detail.Total%>
						</td>
					</tr>
				<%}%>
				<tr>
					<td>Total CapitalCall:</td><td><%=Model.TotalCapitalCall%></td><td colspan=4>&nbsp;</td>
				</tr>
				<tr>
					<td>Total Mgt Fees:</td><td><%=Model.TotalManagementFees%></td><td colspan=4>&nbsp;</td>
				</tr>
				<tr>
					<td>Total Expenses:</td><td><%=Model.TotalExpenses%></td><td colspan=4>&nbsp;</td>
				</tr>
				<tr>
					<td colspan=6>&nbsp;</td>
				</tr>
				<tr>
					<td>Amount for Investments:</td><td><%=Model.AmountForInv%></td><td colspan=4>&nbsp;</td>
				</tr>
				<tr>
					<td>New Inv.:</td><td><%=Model.NewInv%></td><td colspan=4>&nbsp;</td>
				</tr>
				<tr>
					<td>Existing Inv.:</td><td><%=Model.ExistingInv%></td><td colspan=4>&nbsp;</td>
				</tr>
				<%}%>
			<%}%>
			 
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

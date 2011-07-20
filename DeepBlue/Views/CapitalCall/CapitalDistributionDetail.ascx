<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="cc-box-main cc-report-main">
	<div class="line">
	</div>
	<div class="cc-box-det dist-detail">
		<div class="editor-label">
			<label>
				Capital Distributed-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(CapitalDistributed)}</b>
		</div>
		<div class="editor-label" style="clear: right;">
			<label>
				Return Management Fees-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(ReturnManagementFees)}</b>
		</div>
		<div class="editor-label">
			<label>
				Return Fund Expense-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(ReturnFundExpenses)}</b>
		</div>
		<div class="editor-label" style="clear: right;">
			<label>
				Fund Expenses-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(FundExpenses)}</b>
		</div>
		<div class="editor-label" style="clear: right;">
			<label>
				Profits Returned-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(ProfitsReturned)}</b>
		</div>
	</div>
	<div class="line">
	</div>
	<div class="cc-box-det cc-det-report">
		{{if CapitalDistributions.length>0}}
		<div class="gbox">
			<table cellpadding="0" cellspacing="0" class="grid">
				<thead>
					<tr>
						<th style="text-align:left;width:12%;">
							Capital Distribution #
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
						</th>
						<th>
						</th>
					</tr>
				</thead>
				<tbody>
					{{each(i,cd) CapitalDistributions}}
					<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
						<td>
							${cd.Number}
						</td>
						<td style="text-align: right">
							${formatCurrency(cd.CapitalDistributed)}
						</td>
						<td style="text-align: right">
							${formatCurrency(cd.ReturnManagementFees)}
						</td>
						<td style="text-align: right">
							${formatCurrency(cd.ReturnFundExpenses)}
						</td>
						<td style="text-align: right">
							${formatDate(cd.CapitalDistributionDate)}
						</td>
						<td style="text-align: right">
							${formatDate(cd.CapitalDistributionDueDate)}
						</td>
						<td style="text-align: right">
							${formatPercentage(cd.Profit)}
						</td>
						<td style="text-align: right">
							${formatCurrency(cd.ProfitReturn)}
						</td>
						<td style="width: 5%" align="center">
							<%: Html.Image("downarrow.png", new { @class = "ccexpandrow", @onclick = "javascript:capitalCallDetail.expandCD(this,${cd.CapitalDistrubutionId});" })%>
						</td>
					</tr>
					{{/each}}
				</tbody>
			</table>
		</div>
		{{/if}}
	</div>
</div>

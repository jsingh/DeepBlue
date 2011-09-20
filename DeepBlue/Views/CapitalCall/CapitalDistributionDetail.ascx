<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="cc-box-main cc-report-main">
	<div class="line">
	</div>
	<div class="cc-box-det dist-detail">
		<div class="editor-label-first">
			<label>
				Capital Distributed-</label>
		</div>
		<div class="editor-field">
			${formatCurrency(CapitalDistributed)}
		</div>
		<div class="editor-label" style="clear: right;">
			<label>
				Return Management Fees-</label>
		</div>
		<div class="editor-field">
			${formatCurrency(ReturnManagementFees)}
		</div>
		<div class="editor-label-first">
			<label>
				Return Fund Expense-</label>
		</div>
		<div class="editor-field">
			${formatCurrency(ReturnFundExpenses)}
		</div>
		<div class="editor-label" style="clear: right;">
			<label>
				Fund Expenses-</label>
		</div>
		<div class="editor-field">
			${formatCurrency(FundExpenses)}
		</div>
		<div class="editor-label" style="clear: right;">
			<label>
				Profits Returned-</label>
		</div>
		<div class="editor-field">
			${formatCurrency(ProfitsReturned)}
		</div>
	</div>
	{{if CapitalDistributions.length>0}}
	<div class="line">
	</div>
	{{/if}}
	<div class="cc-box-det cc-det-report">
		{{if CapitalDistributions.length>0}}
		<div>
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" class="grid">
				<thead>
					<tr>
						<th style="text-align:left;width:12%;">
							Capital Distribution #
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
							Profits (%)
						</th>
						<th style="text-align: right;width:9.5%;">
							Profits Returned
						</th>
						<th style="width:1%">
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
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		{{/if}}
	</div>
</div>

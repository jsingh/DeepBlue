<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="cc-box-main">
	<div class="line">
	</div>
	<div class="cc-box-det dist-detail">
		<div class="editor-label">
			<label>
				Capital Committed-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(CapitalCommitted)}</b>
		</div>
		<div class="editor-label" style="clear: right;">
			<label>
				Unfunded Amount-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(UnfundedAmount)}</b>
		</div>
		<div class="editor-label">
			<label>
				Management Fees-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(ManagementFees)}</b>
		</div>
		<div class="editor-label" style="clear: right;">
			<label>
				Fund Expenses-</label>
		</div>
		<div class="editor-field" style="padding-top: 10px;">
			<b>${formatCurrency(FundExpenses)}</b>
		</div>
	</div>
	<div class="line">
	</div>
	<div class="cc-box-det cc-det-report">
		{{if CapitalCalls.length>1}}
		<div class="gbox">
			<table cellpadding="0" cellspacing="0" class="grid">
				<thead>
					<tr>
						<th style="width: 12%;text-align:left;">
							Capital Call Number
						</th>
						<th style="width: 10%;text-align: right">
							Capital Call Amount
						</th>
						<th style="width: 10%;text-align: right">
							Management Fees
						</th>
						<th style="width: 10%;text-align: right">
							Fund Expenses
						</th>
						<th style="width: 5%; text-align: right">
							Capital Call Date
						</th>
						<th style="width: 5%;text-align: right">
							Capital Call Due Date
						</th>
						<th style="width: 5%" align="center">
						</th>
					</tr>
				</thead>
				<tbody>
					{{each(i,cc) CapitalCalls}}
					<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
						<td>
							${cc.Number}
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
						<td style="width: 5%" align="center">
							<%: Html.Image("downarrow.png", new { @class = "ccexpandrow", @onclick = "javascript:capitalCallDetail.expandCC(this,${cc.CapitalCallId});" })%>
						</td>
					</tr>
					{{/each}}
				</tbody>
			</table>
		</div>
		{{/if}}
	</div>
</div>

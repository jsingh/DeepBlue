<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="cc-box-main cc-report-main">
	<div class="line">
	</div>
	<div class="cc-box-det dist-detail">
		<div class="editor-label">
			<%: Html.Label("Capital Committed")%>
		</div>
		<div class="editor-field" >
			${formatCurrency(CapitalCommitted)}
		</div>
		<div class="editor-label" style="clear: right;">
			<%: Html.Label("Capital Called")%>
		</div>
		<div class="editor-field" >
			${formatCurrency(CapitalCalled)}
		</div>
		<div class="editor-label" style="clear: right;">
			<%: Html.Label("Unfunded Amount")%>
		</div>
		<div class="editor-field" >
			${formatCurrency(UnfundedAmount)}
		</div>
		<div class="editor-label">
			<%: Html.Label("Management Fees")%>
		</div>
		<div class="editor-field" >
			${formatCurrency(ManagementFees)}
		</div>
		<div class="editor-label" style="clear: right;">
			<%: Html.Label("Fund Expenses")%>
		</div>
		<div class="editor-field" >
			${formatCurrency(FundExpenses)}
		</div>
	</div>
	{{if CapitalCalls.length>0}}
		<div class="line">
		</div>
		<div class="cc-det-report">
		<% Html.RenderPartial("TBoxTop"); %>
		<table cellpadding="0" cellspacing="0" class="grid">
			<thead>
				<tr>
					<th style="width: 15%;text-align:left;">
						Capital Call Number
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
					<th style="width:5%">
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
						<%: Html.Image("downarrow.png", new { @class = "ccexpandrow editbtn", @onclick = "javascript:capitalCallDetail.expandCC(this,${cc.CapitalCallId});" })%>
					</td>
				</tr>
				{{/each}}
			</tbody>
		</table>
		<% Html.RenderPartial("TBoxBottom"); %>
		</div>
	{{/if}}
</div>

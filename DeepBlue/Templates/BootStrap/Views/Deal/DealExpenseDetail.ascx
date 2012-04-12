<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="accordion-group">
	<div class="accordion-heading">
		<a href="#DealExpenseBox" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle">Deal Expenses</a>
	</div>
	<div id="DealExpenseBox" class="accordion-body collapse">
		<div class="deal-detail-list">
			<div class="pull-left">
				<h3>
					Total Expenses:&nbsp;<%:Html.Span("",new { @id = "SpnTotalExpenses" })%>
				</h3>
			</div>
			<div class="pull-right">
				<%: Html.Button("Add expense", new { @class = "btn btn-primary", @onclick = "javascript:deal.showMakeNewHeader('MakeNewDEHeader');" })%>
			</div>
		</div>
		<div class="clear">
			&nbsp;</div>
		<br />
		<div class="deal-detail-list">
			<table id="tblDealExpense" class="table table-striped table-bordered">
				<thead>
					<tr>
						<th class="lalign">
							Description
						</th>
						<th class="ralign">
							Amount
						</th>
						<th class="lalign">
							Date
						</th>
						<th class="ralign">
						</th>
					</tr>
				</thead>
				<thead id="MakeNewDEHeader" style="display: none">
					<tr>
						<td class="lalign">
							<%: Html.DropDownList("DealClosingCostTypeId", Model.DealClosingCostTypes, new { @class = "input-large" })%>
						</td>
						<td class="ralign">
							<%: Html.TextBox("Amount", "", new { @class = "input-large", @onkeydown = "return jHelper.isCurrency(event);" })%>
						</td>
						<td class="lalign">
							<%: Html.TextBox("Date", "", new { @class = "datefield input-large", @id = "0_Date" })%>
						</td>
						<td class="ralign">
							<button onclick="javascript:deal.addDealExpense(this);" class="btn btn-info">
								Add</button>
							<%: Html.Hidden("DealClosingCostId","${DealClosingCostId}")%>
						</td>
					</tr>
				</thead>
				<tbody id="tbodyDealExpense">
				</tbody>
				<tfoot id="tfootDealExpense" style="display: none;">
					<tr>
						<td>
							<b>Total</b>
						</td>
						<td class="ralign">
							<%:Html.Span("",new { @id = "SpnFooterTotalExpenses" })%>
						</td>
						<td colspan="2">
							&nbsp;
						</td>
					</tr>
				</tfoot>
			</table>
		</div>
	</div>
</div>

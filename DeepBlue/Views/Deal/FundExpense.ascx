<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.FundExpenseModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="FLE">
	<%using (Html.Form(new { @id = "frmFundExpense", @onsubmit = "return dealActivity.SaveFundLevelExpense(this);" })) {%>
	<div class="cell">
		<%:Html.Span("", new { @id = "FLE_FundName" })%>
	</div>
	<div style="clear: both">
		<table cellpadding="0" cellspacing="0" border="0" id="ExpenseToDealList" class="grid">
			<thead>
				<tr>
					<th style="width: 15%">
						Expense Amount
					</th>
					<th>
						Fund Expense Type
					</th>
					<th>
					</th>
				</tr>
			</thead>
			<tbody>
				<tr class="row">
					<td style="width: 20%;">
						<%: Html.EditorFor(model => model.Amount, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td style="width: 20%;">
						<%: Html.DropDownListFor(model => model.FundExpenseTypeId, Model.FundExpenseTypes)%>
					</td>
					<td>
					</td>
				</tr>
			</tbody>
			<tfoot>
				<tr>
					<td colspan="2" style="text-align: center; background-color: transparent; border: 0px;">
						<%: Html.Span("", new { @id = "SpnFLELoading" }) %>&nbsp;
						<%: Html.ImageButton("Save.png", new { @style = "width:auto;height:auto; " })%>
					</td>
				</tr>
			</tfoot>
		</table>
	</div>
	<%:Html.HiddenFor(model => model.FundExpenseId)%>
	<%:Html.HiddenFor(model => model.FundId, new { @id = "FLE_FundId" })%>
	<%}%>
</div>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div>
	<%: Html.Image("DealExpenses.png", new { @class="expandbtn" })%></div>
<div class="fieldbox">
	<h5>
		Total Expenses -
		<%:Html.Span("",new { @id = "SpnTotalExpenses" })%>
	</h5>
	<br />
	<table id="tblDealExpense" cellpadding="0" cellspacing="0" border="0" class="grid" style="width: 80%">
		<thead>
			<tr>
				<th style="width: 25%">
					Description
				</th>
				<th style="width: 25%;">
					Amount
				</th>
				<th style="width: 25%;">
					Date
				</th>
				<th style="width: 18%;">
				</th>
				<th style="width: 7%;">
				</th>
			</tr>
		</thead>
		<tbody id="tbodyDealExpense">
		</tbody>
		<tfoot>
			<tr>
				<td>
					<%: Html.DropDownList("DealClosingCostTypeId", Model.DealClosingCostTypes)%>
				</td>
				<td>
					<%: Html.TextBox("Amount", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td>
					<%: Html.TextBox("Date", "", new { @class = "datefield", @id = "${Index}_Date" })%>
				</td>
				<td>
					<%: Html.Image("add_btn.png", new { @onclick = "javascript:deal.addDealExpense(this);" })%>
					<%: Html.Hidden("DealClosingCostId","${DealClosingCostId}")%>
				</td>
				<td class="blank">
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
				</td>
			</tr>
		</tfoot>
	</table>
</div>

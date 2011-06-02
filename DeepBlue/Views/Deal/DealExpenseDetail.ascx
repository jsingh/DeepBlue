<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div>
	<%: Html.Image("DealExpenses.png", new { @class="expandbtn" })%></div>
<div class="fieldbox">
	<h5>
		Total Expenses -
		<%:Html.Span("",new { @id = "SpnTotalExpenses" })%>&nbsp;&nbsp;<%:Html.Anchor("Make new deal expense", "javascript:deal.showMakeNewHeader('MakeNewDEHeader');", new { @class = "make" })%>
	</h5>
	<br />
	<table id="tblDealExpense" cellpadding="0" cellspacing="0" border="0" class="grid"
		style="width: 65%">
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
			</tr>
		</thead>
		<thead id="MakeNewDEHeader" style="display:none">
			<tr>
				<td>
					<%: Html.DropDownList("DealClosingCostTypeId", Model.DealClosingCostTypes)%>
				</td>
				<td>
					<%: Html.TextBox("Amount", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td>
					<%: Html.TextBox("Date", "", new { @class = "datefield", @id = "0_Date" })%>
				</td>
				<td style="text-align: right">
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
					<%: Html.Image("tick.png", new { @onclick = "javascript:deal.addDealExpense(this);" })%>
					<%: Html.Hidden("DealClosingCostId","${DealClosingCostId}")%>
				</td>
			</tr>
		</thead>
		<tbody id="tbodyDealExpense">
		</tbody>
	</table>
</div>

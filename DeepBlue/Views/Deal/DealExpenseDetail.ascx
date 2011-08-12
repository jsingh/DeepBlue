<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line" id="DETopLine">
</div>
<div class="expandaddbtn">
	<%using (Html.GreenButton(new { @onclick = "javascript:deal.showMakeNewHeader('MakeNewDEHeader');" })) {%>Add
	expense<%}%>
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			ADD DEAL EXPENSES</div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Deal Expenses</div>
		</div>
	</div>
</div>
<div class="fieldbox">
	<div class="section">
		<div class="dealdetail">
			<h5 style="margin: 10px 0px 20px 0px">
				Total Expenses:
				<%:Html.Span("",new { @id = "SpnTotalExpenses" })%>
			</h5>
			<br />
			<div class="gbox" style="width: 90%;">
				<table id="tblDealExpense" cellpadding="0" cellspacing="0" border="0" class="grid">
					<thead>
						<tr>
							<th class="lalign" style="width: 15%;">
								Description
							</th>
							<th class="ralign" style="width: 15%;">
								Amount
							</th>
							<th class="lalign" style="width: 15%;">
								Date
							</th>
							<th class="ralign">
							</th>
						</tr>
					</thead>
					<thead id="MakeNewDEHeader" style="display: none">
						<tr>
							<td class="lalign">
								<%: Html.DropDownList("DealClosingCostTypeId", Model.DealClosingCostTypes)%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("Amount", "", new { @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="lalign">
								<%: Html.TextBox("Date", "", new { @class = "datefield", @id = "0_Date" })%>
							</td>
							<td class="ralign">
								<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
								<%: Html.Image("add.png", new { @onclick = "javascript:deal.addDealExpense(this);" })%>
								<%: Html.Hidden("DealClosingCostId","${DealClosingCostId}")%>
							</td>
						</tr>
					</thead>
					<tbody id="tbodyDealExpense">
					</tbody>
					<tfoot>
						<tr>
							<td>
								Total
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
</div>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			<%: Html.Image("DealExpenses.png")%></div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Deal Expenses</div>
		</div>
	</div>
	 <div class="expandaddbtn">
   <%: Html.Anchor(Html.Image("addexpense.png").ToHtmlString(), "javascript:deal.showMakeNewHeader('MakeNewDEHeader');")%>
    </div>
</div>
<div class="fieldbox">
	<div class="section">
		<h5>
			Total Expenses: 
			<%:Html.Span("",new { @id = "SpnTotalExpenses" })%>
		</h5>
		<br />
		<table id="tblDealExpense" cellpadding="0" cellspacing="0" border="0" class="grid"
			style="width: 65%">
			<thead>
				<tr class="dealhead_tr">
					<th style="width: 25%;">
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
			<thead id="MakeNewDEHeader" style="display: none">
				<tr>
					<td style="text-align: left">
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
						<%: Html.Image("add.png", new { @onclick = "javascript:deal.addDealExpense(this);" })%>
						<%: Html.Hidden("DealClosingCostId","${DealClosingCostId}")%>
					</td>
				</tr>
			</thead>
			<tbody id="tbodyDealExpense">
			</tbody>
		</table>
	</div>
</div>

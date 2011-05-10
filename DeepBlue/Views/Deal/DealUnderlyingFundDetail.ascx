<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div>
	<%: Html.Image("UnderlyingFunds.png", new { @class="expandbtn" })%></div>
<div class="fieldbox">
	<table id="tblUnderlyingFund" cellpadding="0" cellspacing="0" border="0" class="grid"
		style="width: 100%">
		<thead>
			<tr>
				<th>
					No.
				</th>
				<th>
					Fund Name
				</th>
				<th>
					Fund NAV
				</th>
				<th>
					Percentage
				</th>
				<th>
					Commitment
				</th>
				<th>
					Unfunded Amount
				</th>
				<th>
					Gross Purchase Price
				</th>
				<th>
					Record Date
				</th>
				<th>
				</th>
				<th>
				</th>
			</tr>
		</thead>
		<tbody id="tbodyUnderlyingFund">
		</tbody>
		<tfoot>
			<tr>
				<td style="text-align: center">
					<%: Html.Span("", new { @id = "SpnIndex" }) %>
				</td>
				<td style="text-align: center">
					<%: Html.DropDownList("UnderlyingFundID", Model.UnderlyingFunds)%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("FundNav", "",new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("Percent", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("CommittedAmount", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("UnfundedAmount", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("GrossPurchasePrice", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("RecordDate", "", new { @class = "datefield", @id = "0_RecordDate" })%>
				</td>
				<td style="text-align: center">
					<%: Html.Image("add_btn.png", new { @onclick = "javascript:deal.addUnderlyingFund(this);" })%>
					<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
				</td>
				<td class="blank">
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
				</td>
			</tr>
		</tfoot>
	</table>
</div>

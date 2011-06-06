<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div style="float: left">
	<%: Html.Image("UnderlyingFunds.png", new { @class="expandbtn" })%></div>
<div class="makenew-header">
	<%:Html.Anchor("Make new deal underlying fund", "javascript:deal.showMakeNewHeader('MakeNewDUFund');", new { @class = "make" })%></div>
<div class="fieldbox">
	<table id="tblUnderlyingFund" cellpadding="0" cellspacing="0" border="0" class="grid"
		style="width: 99%">
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
			</tr>
		</thead>
		<thead id="MakeNewDUFund" style="display: none">
			<tr>
				<td style="text-align: center">
					<%: Html.Span("", new { @id = "SpnIndex" }) %>
				</td>
				<td style="text-align: center">
					<%: Html.DropDownList("UnderlyingFundID", Model.UnderlyingFunds, new { @onchange = "javascript:deal.FindFundNAV(this);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("FundNAV", "", new { @id="FundNAV", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("Percent", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("CommittedAmount", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("UnfundedAmount", "", new { @readonly="readonly", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("GrossPurchasePrice", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td style="text-align: center">
					<%: Html.TextBox("RecordDate", "", new { @class = "datefield", @id = "0_RecordDate" })%>
				</td>
				<td style="text-align: right">
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
					<%: Html.Image("tick.png", new { @onclick = "javascript:deal.addUnderlyingFund(this);" })%>
					<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
				</td>
			</tr>
		</thead>
		<tbody id="tbodyUnderlyingFund">
		</tbody>
	</table>
</div>

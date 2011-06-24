<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			<%: Html.Image("UnderlyingFunds.png")%></div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Underlying Funds</div>
		</div>
	</div>
	<div class="expandaddbtn">
		<%: Html.Anchor(Html.Image("add_funds.png").ToHtmlString(), "javascript:deal.showMakeNewHeader('MakeNewDUFund');")%>
	</div>
</div>
<div class="fieldbox">
	<div class="section" style="width: 90%;">
		<table id="tblUnderlyingFund" cellpadding="0" cellspacing="0" border="0" class="grid"
			style="width: 100%">
			<thead>
				<tr class="dealhead_tr">
					<th style="display: none;">
						No.
					</th>
					<th style="width: 5%">
						Close
					</th>
					<th>
						Fund Name
					</th>
					<th style="width: 15%">
						Gross Purchase Price
					</th>
					<th style="width: 10%">
						Fund NAV
					</th>
					<th style="width: 12%">
						Capital Commitment
					</th>
					<th style="width: 12%">
						Amount Unfunded
					</th>
					<th style="width: 12%">
						Record Date
					</th>
					<th>
					</th>
				</tr>
			</thead>
			<thead id="MakeNewDUFund" style="display: none">
				<tr>
					<td style="text-align: center; display: none;">
						<%: Html.Span("", new { @id = "SpnIndex" }) %>
					</td>
					<td style="text-align: center;">
					</td>
					<td style="text-align: left">
						<%: Html.TextBox("UnderlyingFund", "", new { @id = "UnderlyingFund", @style = "width:78%" })%>
						<%: Html.Hidden("UnderlyingFundId", "0")%>
					</td>
					<td style="text-align: center">
						<%: Html.TextBox("GrossPurchasePrice", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td style="text-align: center">
						<%: Html.TextBox("FundNAV", "", new { @id="FundNAV", @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td style="text-align: center">
						<%: Html.TextBox("CommittedAmount", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td style="text-align: center">
						<%: Html.TextBox("UnfundedAmount", "", new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td style="text-align: center">
						<%: Html.TextBox("RecordDate", "", new { @class = "datefield", @id = "0_RecordDate" })%>
					</td>
					<td style="text-align: right">
						<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
						<%: Html.Image("add.png", new { @onclick = "javascript:deal.addUnderlyingFund(this);" })%>
						<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
					</td>
				</tr>
			</thead>
			<tbody id="tbodyUnderlyingFund">
			</tbody>
		</table>
	</div>
</div>

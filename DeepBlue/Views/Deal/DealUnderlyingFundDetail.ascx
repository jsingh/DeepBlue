<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="expandaddbtn">
	<%using (Html.GreenButton(new { @onclick = "javascript:deal.showMakeNewHeader('MakeNewDUFund');" })) {%>Add
	underlying funds<%}%>
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			ADD UNDERLYING FUNDS</div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Underlying Funds</div>
		</div>
	</div>
</div>
<div class="fieldbox">
	<div class="section" style="width: 90%;">
		<div class="dealdetail">
			<div class="gbox">
				<table id="tblUnderlyingFund" cellpadding="0" cellspacing="0" border="0" class="grid"
					style="width: 100%">
					<thead>
						<tr>
							<th style="display: none;">
								No.
							</th>
							<th class="lalign">
								Close
							</th>
							<th class="lalign">
								Fund Name
							</th>
							<th class="ralign" style="width: 15%;">
								Gross Purchase Price
							</th>
							<th class="ralign" style="width: 10%">
								Fund NAV
							</th>
							<th class="ralign">
								Capital Commitment
							</th>
							<th class="ralign">
								Amount Unfunded
							</th>
							<th class="lalign">
								Record Date
							</th>
							<th style="width: 10%">
							</th>
						</tr>
					</thead>
					<thead id="MakeNewDUFund" style="display: none">
						<tr>
							<td class="calign" style="display: none;">
								<%: Html.Span("", new { @id = "SpnIndex" }) %>
							</td>
							<td class="calign">
							</td>
							<td class="lalign">
								<%: Html.TextBox("UnderlyingFund", "", new { @id = "UnderlyingFund", @class = "tooltiptxt", @top = "198", @style = "width:78%" })%>
								<%: Html.Hidden("UnderlyingFundId", "0")%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("GrossPurchasePrice", "", new { @id = "GrossPurchasePrice", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("FundNAV", "", new { @id = "FundNAV", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("CommittedAmount", "", new { @id = "CommittedAmount", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("UnfundedAmount", "", new { @id = "UnfundedAmount", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="lalign">
								<%: Html.TextBox("RecordDate", "", new { @class = "datefield", @id = "0_RecordDate" })%>
							</td>
							<td class="ralign">
								<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
								<%: Html.Image("add.png", new { @onclick = "javascript:deal.addUnderlyingFund(this);" })%>
								<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
							</td>
						</tr>
					</thead>
					<tbody id="tbodyUnderlyingFund">
					</tbody>
					<tfoot>
						<tr>
							<td class="lalign">
								Total
							</td>
							<td>
							</td>
							<td class="ralign">
								<%:Html.Span("", new { @id = "SpnTotalFundGPP" })%>
							</td>
							<td class="ralign">
								<%:Html.Span("", new { @id = "SpnTotalFundNAV" })%>
							</td>
							<td class="ralign">
								<%:Html.Span("", new { @id = "SpnTotalCAmount" })%>
							</td>
							<td class="ralign">
								<%:Html.Span("", new { @id = "SpnTotalUAmount" })%>
							</td>
							<td>
							</td>
							<td>
							</td>
						</tr>
					</tfoot>
				</table>
			</div>
		</div>
	</div>
</div>

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
			UNDERLYING FUNDS</div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Underlying Funds</div>
		</div>
	</div>
</div>
<div class="fieldbox">
	<div class="section" style="width: 95%;">
		<div class="dealdetail">
			<div>
				<% Html.RenderPartial("TBoxTop"); %>
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
							<th class="ralign" style="width: 100px;">
								Gross Purchase Price
							</th>
							<th class="ralign" style="width: 100px">
								Fund NAV
							</th>
							<th class="lalign" style="width: 85px">
								Effective Date
							</th>
							<th class="ralign" style="width: 100px;">
								Capital Commitment
							</th>
							<th class="ralign" style="width: 100px;">
								Amount Unfunded
							</th>
							<th class="lalign" style="width: 85px;">
								Record Date
							</th>
							<th style="width: 100px;">
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
								<%: Html.TextBox("UnderlyingFund", "", new { @id = "UnderlyingFund", @class = "", @top = "198"  })%>
								<%: Html.Hidden("UnderlyingFundId", "0")%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("GrossPurchasePrice", "", new { @id = "GrossPurchasePrice", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("FundNAV", "", new { @id = "FundNAV", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="lalign">
								<%: Html.TextBox("EffectiveDate", "", new { @style="width:85px;", @class = "datefield", @id = "0_EffectiveDate" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("CommittedAmount", "", new { @id = "CommittedAmount", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("UnfundedAmount", "", new { @id = "UnfundedAmount", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="lalign">
								<%: Html.TextBox("RecordDate", "", new { @style = "width:85px;", @class = "datefield", @id = "0_RecordDate" })%>
							</td>
							<td class="ralign">
								<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
								<%: Html.Image("add_active.png", new { @onclick = "javascript:deal.addUnderlyingFund(this);" })%>
								<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
							</td>
						</tr>
					</thead>
					<tbody id="tbodyUnderlyingFund">
					</tbody>
					<tfoot id="tfootUnderlyingFund" style="display:none;">
						<tr>
							<td class="lalign">
								<b>Total</b>
							</td>
							<td>
							</td>
							<td class="ralign">
								<%:Html.Span("", new { @id = "SpnTotalFundGPP" })%>
							</td>
							<td class="ralign">
								<%:Html.Span("", new { @id = "SpnTotalFundNAV" })%>
							</td>
							<td></td>
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
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>

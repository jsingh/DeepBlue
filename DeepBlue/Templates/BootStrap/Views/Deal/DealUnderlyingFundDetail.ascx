<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="accordion-group">
	<div class="accordion-heading">
		<a href="#UnderlyingFundBox" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle">Underlying Funds</a>
	</div>
	<div id="UnderlyingFundBox" class="accordion-body collapse">
		<div class="pull-right">
			<%: Html.Button("Add underlying funds", new { @class = "btn btn-primary", @onclick = "javascript:deal.showMakeNewHeader('MakeNewDUFund');" })%>
		</div>
		<div class="clear">
			&nbsp;</div>
		<br /><div class="deal-detail-list">
		<table id="tblUnderlyingFund" class="table table-striped table-bordered">
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
					<th class="ralign">
						Gross Purchase Price
					</th>
					<th class="ralign">
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
					<th>
					</th>
				</tr>
			</thead>
			<thead id="MakeNewDUFund" style="display: none">
				<tr>
					<td class="calign" colspan="8">
						<%using (Html.Form(new { @class = "form-horizontal", @onsubmit = "return false" })) {%>
						<%: Html.Span("", new { @id = "SpnIndex" })%>
						<div class="control-group pull-left">
							<label class="control-label">
								Fund</label>
							<div class="controls">
								<%: Html.TextBox("UnderlyingFund", "", new { @class = "input-large", @id = "UnderlyingFund", @top = "198" })%><%: Html.Hidden("UnderlyingFundId", "0")%></div>
						</div>
						<div class="control-group pull-left">
							<label class="control-label">
								Gross Purchase Price</label>
							<div class="controls">
								<%: Html.TextBox("GrossPurchasePrice", "", new { @class = "input-large", @id = "GrossPurchasePrice", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</div>
						</div>
						<div class="clear">
							&nbsp;</div>
						<div class="control-group pull-left">
							<label class="control-label">
								Fund NAV</label>
							<div class="controls">
								<%: Html.TextBox("FundNAV", "", new { @class = "input-large", @id = "FundNAV", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</div>
						</div>
						<div class="control-group pull-left">
							<label class="control-label">
								Committed Amount</label>
							<div class="controls">
								<%: Html.TextBox("CommittedAmount", "", new { @class = "input-large", @id = "CommittedAmount", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</div>
						</div>
						<div class="clear">
							&nbsp;</div>
						<div class="control-group pull-left">
							<label class="control-label">
								Unfunded Amount</label>
							<div class="controls">
								<%: Html.TextBox("UnfundedAmount", "", new { @class = "input-large", @id = "UnfundedAmount", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</div>
						</div>
						<div class="control-group pull-left">
							<label class="control-label">
								Record Date</label>
							<div class="controls">
								<%: Html.TextBox("RecordDate", "", new { @class = "datefield input-large", @id = "0_RecordDate" })%>
							</div>
						</div>
						<div class="clear">
							&nbsp;</div>
						<div class="control-group">
							<button id="save" class="btn btn-primary input-small" onclick="javascript:deal.addUnderlyingFund(this);" data-loading-text="Saving...">
								Save</button>
						</div>
						<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
						<%}%>
					</td>
				</tr>
			</thead>
			<tbody id="tbodyUnderlyingFund">
			</tbody>
			<tfoot id="tfootUnderlyingFund" style="display: none;">
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

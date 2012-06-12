<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="accordion-group">
	<div class="accordion-heading">
		<a href="#UnderlyingDirectBox" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle">Underlying Directs</a>
	</div>
	<div id="UnderlyingDirectBox" class="accordion-body collapse">
		<div class="pull-right">
			<%: Html.Button("Add new directs", new { @class = "btn btn-primary", @onclick = "javascript:deal.showMakeNewHeader('MakeNewDUDirect');" })%>
		</div>
		<div class="clear">
			&nbsp;</div>
		<br />
		<div class="deal-detail-list">
			<table id="tblUnderlyingDirect" class="table table-striped table-bordered">
				<thead>
					<tr>
						<th style="display: none;">
							No.
						</th>
						<th class="lalign">
							Close
						</th>
						<th class="lalign">
							Company
						</th>
						<th class="ralign">
							No. of Shares
						</th>
						<th class="ralign">
							Purchase Price
						</th>
						<th class="ralign">
							Fair Market Value
						</th>
						<th class="ralign">
							Tax Cost Basis Per Share
						</th>
						<th class="lalign">
							Tax Cost Date
						</th>
						<th class="lalign">
							Record Date
						</th>
						<th>
						</th>
					</tr>
				</thead>
				<thead id="MakeNewDUDirect" style="display: none">
					<tr>
						<td colspan="9">
							<%using (Html.Form(new { @class = "form-horizontal", @onsubmit = "return false" })) {%>
							<%: Html.Span("", new { @id = "SpnIndex" }) %>
							<div class="control-group pull-left">
								<label class="control-label">
									Company</label>
								<div class="controls">
									<%: Html.TextBox("Issuer", "", new { @id = "Issuer", @class = "input-large", @top = "198" })%>
								</div>
							</div>
							<div class="control-group pull-left">
								<label class="control-label">
									No. of Shares</label>
								<div class="controls">
									<%: Html.TextBox("NumberOfShares", "", new { @class = "input-large", @id = "NumberOfShares", @onkeyup = "javascript:deal.calcDUD();", @onkeydown = "return jHelper.isNumeric(event);" })%>
								</div>
							</div>
							<div class="clear">
								&nbsp;</div>
							<div class="control-group pull-left">
								<label class="control-label">
									Purchase Price</label>
								<div class="controls">
									<%: Html.TextBox("PurchasePrice", "", new { @class = "input-large", @id = "PurchasePrice", @onkeyup = "javascript:deal.calcDUD();", @onkeydown = "return jHelper.isCurrency(event);" })%>
								</div>
							</div>
							<div class="control-group pull-left">
								<label class="control-label">
									Fair Market Value</label>
								<div class="controls">
									<%: Html.TextBox("FMV", "", new { @class = "input-large", @id = "FMV", @onkeyup = "javascript:deal.calcDUD();", @onkeydown = "return jHelper.isCurrency(event);" })%>
								</div>
							</div>
							<div class="clear">
								&nbsp;</div>
							<div class="control-group pull-left">
								<label class="control-label">
									Tax Cost Basis Per Share</label>
								<div class="controls">
									<%: Html.TextBox("TaxCostBase", "", new { @class = "input-large", @onkeydown = "return jHelper.isCurrency(event);" })%>
								</div>
							</div>
							<div class="control-group pull-left">
								<label class="control-label">
									Tax Cost Date</label>
								<div class="controls">
									<%: Html.TextBox("RecordDate", "", new { @class = "datefield input-large", @id = "0_DirectRecordDate" })%>
								</div>
							</div>
							<div class="clear">
								&nbsp;</div>
							<div class="control-group center">
								<button id="save" class="btn btn-primary input-small" onclick="javascript:deal.addUnderlyingDirect(this);" data-loading-text="Saving...">Save</button>
							</div>
							<%: Html.Hidden("DealUnderlyingDirectId", "${DealUnderlyingDirectId}")%>
							<%: Html.Hidden("IssuerId", "0")%>
							<%: Html.Hidden("SecurityTypeId","0")%>
							<%: Html.Hidden("SecurityId", "0")%>
							<%}%>
						</td>
					</tr>
				</thead>
				<tbody id="tbodyUnderlyingDirect">
				</tbody>
				<tfoot id="tfootUnderlyingDirect" style="display: none;">
					<tr>
						<td class="lalign">
							<b>Total</b>
						</td>
						<td class="lalign">
						</td>
						<td class="ralign">
							<%:Html.Span("", new { @id = "SpnTotalNOS" })%>
						</td>
						<td class="ralign">
							<%:Html.Span("", new { @id = "SpnTotalPP" })%>
						</td>
						<td class="ralign">
							<%:Html.Span("", new { @id = "SpnTotalFMV" })%>
						</td>
						<td>
						</td>
						<td>
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

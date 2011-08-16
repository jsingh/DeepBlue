<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="expandaddbtn">
	<%using (Html.GreenButton(new { @onclick = "javascript:deal.showMakeNewHeader('MakeNewDUDirect');" })) {%>Add
	new directs<%}%>
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			ADD UNDERLYING DIRECTS</div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Underlying Direct</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="fieldbox">
	<div class="section" style="width: 95%;">
		<div class="dealdetail">
			<div class="gbox">
				<table id="tblUnderlyingDirect" cellpadding="0" cellspacing="0" border="0" class="grid"
					style="width: 100%">
					<thead>
						<tr>
							<th style="display: none;">
								No.
							</th>
							<th class="lalign" style="width: 5%">
								Close
							</th>
							<th class="lalign">
								Company
							</th>
							<th class="ralign" style="width: 12%">
								No. of Shares
							</th>
							<th class="ralign" style="width: 12%">
								Purchase Price
							</th>
							<th class="ralign" style="width: 12%">
								Fair Market Value
							</th>
							<th class="ralign" style="width: 12%">
								Tax Cost Basic
							</th>
							<th class="lalign" style="width: 12%">
								Tax Cost Date
							</th>
							<th class="lalign" style="width: 12%">
								Record Date
							</th>
							<th style="width: 10%">
							</th>
						</tr>
					</thead>
					<thead id="MakeNewDUDirect" style="display: none">
						<tr>
							<td style="text-align: center; display: none;">
								<%: Html.Span("", new { @id = "SpnIndex" }) %>
							</td>
							<td class="lalign">
							</td>
							<td class="lalign">
								<%: Html.TextBox("Issuer", "", new { @id = "Issuer", @class = "tooltiptxt", @top = "198", @style = "width:78%" })%>
								<%: Html.Hidden("IssuerId", "0")%>
								<%: Html.Hidden("SecurityTypeId","0")%>
								<%: Html.Hidden("SecurityId", "0")%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("NumberOfShares", "", new { @id = "NumberOfShares", @onkeyup = "javascript:deal.calcDUD();", @onkeydown = "return jHelper.isNumeric(event);" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("PurchasePrice", "", new { @id = "PurchasePrice", @onkeyup = "javascript:deal.calcDUD();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("FMV", "", new { @id = "FMV", @onkeyup = "javascript:deal.calcDUD();", @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="ralign">
								<%: Html.TextBox("TaxCostBase", "", new { @onkeydown = "return jHelper.isCurrency(event);" })%>
							</td>
							<td class="lalign">
								<%: Html.TextBox("TaxCostDate", "", new { @class = "datefield", @id = "0_DirectTaxCostDate" })%>
							</td>
							<td class="lalign">
								<%: Html.TextBox("RecordDate", "", new { @class = "datefield", @id = "0_DirectRecordDate" })%>
							</td>
							<td class="ralign">
								<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
								<%: Html.Image("add.png", new { @onclick = "javascript:deal.addUnderlyingDirect(this);" })%>
								<%: Html.Hidden("DealUnderlyingDirectId", "${DealUnderlyingDirectId}")%>
							</td>
						</tr>
					</thead>
					<tbody id="tbodyUnderlyingDirect">
					</tbody>
					<tfoot>
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
</div>

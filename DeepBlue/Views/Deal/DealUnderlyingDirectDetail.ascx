<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			<%: Html.Image("UnderlyingDirects.png")%></div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Underlying Direct</div>
		</div>
	</div>
	<div class="expandaddbtn">
		<%: Html.Anchor(Html.Image("add_new_dir.png").ToHtmlString(), "javascript:deal.showMakeNewHeader('MakeNewDUDirect');")%>
	</div>
</div>
<div class="fieldbox">
	<div class="section" style="width: 90%;">
		<table id="tblUnderlyingDirect" cellpadding="0" cellspacing="0" border="0" class="grid"
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
						Company
					</th>
					<th style="width: 12%">
						No. of Shares
					</th>
					<th style="width: 12%">
						Purchase Price
					</th>
					<th style="width: 12%">
						Tax Cost Basic
					</th>
					<th style="width: 12%">
						Tax Cost Date
					</th>
					<th style="width: 10%">
						Fair Market Value
					</th>
					<th style="width: 12%">
						Record Date
					</th>
					<th style="width: 5%">
					</th>
				</tr>
			</thead>
			<thead id="MakeNewDUDirect" style="display: none">
				<tr>
					<td style="text-align: center; display: none;">
						<%: Html.Span("", new { @id = "SpnIndex" }) %>
					</td>
					<td style="text-align: center;">
					</td>
					<td>
						<%: Html.TextBox("Issuer", "", new { @id = "Issuer", @style = "width:78%" })%>
						<%: Html.Hidden("IssuerId", "0")%>
						<%: Html.Hidden("SecurityTypeId","0")%>
						<%: Html.Hidden("SecurityId", "0")%>
					</td>
					<td>
						<%: Html.TextBox("NumberOfShares", "", new { @id = "NumberOfShares", @onkeyup = "javascript:deal.calcFMV(this);", @onkeypress = "return jHelper.isNumeric(event);" })%>
					</td>
					<td>
						<%: Html.TextBox("PurchasePrice", "", new { @onkeypress = "return jHelper.isCurrency(event);", @onkeyup = "javascript:deal.calcFMV(this);" })%>
					</td>
					<td>
						<%: Html.TextBox("TaxCostBase", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td>
						<%: Html.TextBox("TaxCostDate", "", new { @class = "datefield", @id = "0_DirectTaxCostDate" })%>
					</td>
					<td>
						<%: Html.TextBox("FMV", "", new { @readonly = "readonly", @id = "FMV", @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td>
						<%: Html.TextBox("RecordDate", "", new { @class = "datefield", @id = "0_DirectRecordDate" })%>
					</td>
					<td style="text-align: center">
						<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
						<%: Html.Image("add.png", new { @onclick = "javascript:deal.addUnderlyingDirect(this);" })%>
						<%: Html.Hidden("DealUnderlyingDirectId", "${DealUnderlyingDirectId}")%>
					</td>
				</tr>
			</thead>
			<tbody id="tbodyUnderlyingDirect">
			</tbody>
		</table>
	</div>
</div>

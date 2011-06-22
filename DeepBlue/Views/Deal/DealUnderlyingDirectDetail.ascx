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
		<%:Html.Anchor("Add new directs", "javascript:deal.showMakeNewHeader('MakeNewDUDirect');")%></div>
</div>
<div class="fieldbox">
	<div class="section" style="width:90%;">
		<table id="tblUnderlyingDirect" cellpadding="0" cellspacing="0" border="0" class="grid"
			style="width: 100%">
			<thead>
				<tr class="dealhead_tr">
					<th>
						No.
					</th>
					<th style="width: 10%">
						Issuer
					</th>
					<th style="width: 10%">
						Security Type
					</th>
					<th style="width: 10%">
						Security
					</th>
					<th nowrap>
						No. of Shares
					</th>
					<th nowrap>
						Purchase Price
					</th>
					<th>
						FMV
					</th>
					<th nowrap>
						Tax Cost Base
					</th>
					<th nowrap>
						Tax Cost Date
					</th>
					<th>
						Record Date
					</th>
					<th>
					</th>
				</tr>
			</thead>
			<thead id="MakeNewDUDirect" style="display: none">
				<tr>
					<td style="text-align: center">
						<%: Html.Span("", new { @id = "SpnIndex" }) %>
					</td>
					<td>
						<%: Html.DropDownList("IssuerId", Model.Issuers, new { @id="IssuerId", @class="issuerddl", @onchange = "javascript:deal.changeIssuer(this);" })%>
					</td>
					<td>
						<%: Html.DropDownList("SecurityTypeId", Model.SecurityTypes, new { @id="SecurityTypeId", @onchange = "javascript:deal.changeSecurityType(this);" })%>
					</td>
					<td>
						<%: Html.DropDownList("SecurityId", Model.Securities, new { @id = "SecurityId", @onchange = "javascript:deal.changeSecurity(this);" })%>
					</td>
					<td>
						<%: Html.TextBox("NumberOfShares", "", new { @id = "NumberOfShares", @onkeyup = "javascript:deal.calcFMV(this);", @onkeypress = "return jHelper.isNumeric(event);" })%>
					</td>
					<td>
						<%: Html.TextBox("FMV", "", new { @readonly="readonly", @id="FMV", @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td>
						<%: Html.TextBox("Percent", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td>
						<%: Html.TextBox("TaxCostBase", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</td>
					<td>
						<%: Html.TextBox("TaxCostDate", "", new { @class = "datefield", @id = "0_DirectTaxCostDate" })%>
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

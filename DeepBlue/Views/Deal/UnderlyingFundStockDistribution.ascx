<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFSD_${Index}" {{if UnderlyingFundStockDistributionId>0==false}}class="newrow"{{/if}}>
	<td style="text-align:center;display:none;" class="lalign ismanual">
		<%: Html.Image("treeminus.gif", new { @onclick = "javascript:dealActivity.expandMSDTree(${Index},this);" })%>
	</td>
	<td colspan=12>
		<table cellpadding=0 cellspacing=0 border=0 style="width:100%">
			<tr style="background: transparent">
				<td id="ufsd_controls">
					<div class="editor-label">
					Fund Name
					</div>
						<div class="editor-field">
						<%: Html.Span("${FundName}", new { @class = "show" })%>
					</div>
						<div class="editor-label">
					Issuer Name
					</div>
						<div class="editor-field">
							<%: Html.TextBox("${Index}_IssuerName", "", new { @id = "IssuerName", @style = "width:120px" })%>
					<%: Html.Hidden("${Index}_IssuerId", "", new { @id = "IssuerId" })%>
					</div>
					<div class="editor-label" style="clear:right">
					Equity
					</div>
						<div class="editor-field">
							<%: Html.TextBox("${Index}_Issuer", "", new { @id = "Issuer", @style = "width:120px" })%>
					<%: Html.Hidden("${Index}_SecurityId", "", new { @id = "SecurityId" })%>
					<%: Html.Hidden("${Index}_SecurityTypeId", "", new { @id = "SecurityTypeId" })%>
					</div>
					<div class="editor-label" style="clear:right">
					Broker
					</div>
						<div class="editor-field">
							<%: Html.TextBox("${Index}_Broker", "", new { @id = "Broker", @style = "width:120px" })%>
							<%: Html.Hidden("${Index}_BrokerID", "", new { @id = "BrokerID" })%>
					</div>
					<div class="editor-label">
					Number Of Shares
					</div>
						<div class="editor-field">
						<%: Html.TextBox("${Index}_NumberOfShares", "", new {  @onkeydown = "return jHelper.isNumeric(event);" })%>
					</div>
					<div class="editor-label" style="clear:right">
					Purchase Price
					</div>
						<div class="editor-field">
						<%: Html.TextBox("${Index}_PurchasePrice", "", new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
						<div class="editor-label" style="clear:right">
					Notice Date
					</div>
						<div class="editor-field">
					<%: Html.TextBox("${Index}_NoticeDate", "", new { @id = "${Index}_NoticeDate", @class = "datefield" })%>
					</div>
						<div class="editor-label">
					Distribution Date
					</div>
						<div class="editor-field">
					<%: Html.TextBox("${Index}_DistributionDate", "", new { @id = "${Index}_DistributionDate", @class = "datefield" })%>
					</div>
						<div class="editor-label" style="clear:right">
					Tax Cost Base
					</div>
						<div class="editor-field">
				<%: Html.TextBox("${Index}_TaxCostBase", "", new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
						<div class="editor-label" style="clear:right">
					Tax Cost Date
					</div>
						<div class="editor-field">
				<%: Html.TextBox("${Index}_TaxCostDate", "", new { @id = "${Index}_TaxCostDate", @class = "datefield" })%>
					</div>
						<div class="editor-label">
					Notes
					</div>
						<div class="editor-field">
							<%: Html.TextArea("${Index}_Notes", "", 4, 70, new { @id = "${Index}_Notes" })%>
						</div>
							<%: Html.Hidden("${Index}_UnderlyingFundStockDistributionId", "${UnderlyingFundStockDistributionId}")%>
					<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}", new { @id = "UnderlyingFundId" })%>
					<%: Html.Hidden("${Index}_FundId", "${FundId}", new { @id = "FundId" })%>
					<%: Html.Hidden("${Index}_Index", "${Index}", new { @id = "Index" })%>
				 </td>
			</tr>
		</table>
	</td>
</tr>
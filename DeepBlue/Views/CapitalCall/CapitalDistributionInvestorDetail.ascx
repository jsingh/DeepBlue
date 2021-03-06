﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr class="row">
	<td>
		<%: Html.Hidden("${Index}_InvestorId","0",new { @id = "InvestorId" })%>
		<%: Html.TextBox("Investor", "", new { @id = "Investor", @class = "hide" })%>
	</td>
	<td class="ralign">
		<%:Html.TextBox("${Index}_DistributionAmount", "", new { @id = "txtDistributionAmount", @style = "text-align:right;width:120px;", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualDistribution.calcCDA();" })%>
	</td>
	<td class="ralign">
		<%:Html.TextBox("${Index}_ReturnManagementFees", "", new { @id = "txtReturnManagementFees", @style = "text-align:right;width:120px;", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualDistribution.calcRMFAmt();" })%>
	</td>
	<td class="ralign">
		<%:Html.TextBox("${Index}_ReturnFundExpenses", "", new { @id = "txtReturnFundExpenses", @style = "text-align:right;width:120px;", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualDistribution.calcRFE();" })%>
	</td>
	<td class="ralign">
		<%:Html.TextBox("${Index}_CapitalReturn", "", new { @id = "txtCapitalReturn", @style = "text-align:right;width:120px;", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualDistribution.calcCR();" })%>
	</td>
	<td class="ralign">
		<%:Html.TextBox("${Index}_GPProfits", "", new { @id = "txtGPProfits", @style = "text-align:right;width:120px;", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualDistribution.calcGP(this);" })%>
	</td>
	<td class="ralign">
		<%:Html.TextBox("${Index}_PreferredReturn", "", new { @id = "txtPreferredReturn", @style = "text-align:right;width:120px;", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualDistribution.calcPR();" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Image("largedel.png", new { @onclick = "javascript:manualDistribution.deleteInvestor(this);", @class = "gbutton" })%>
	</td>
</tr>

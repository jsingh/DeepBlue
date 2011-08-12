<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr class="row">
	<td>
		<%: Html.Hidden("${Index}_InvestorId","0",new { @id = "InvestorId" })%>
		<%: Html.TextBox("Investor", "", new { @id = "Investor", @class = "hide" })%>
	</td>
	<td>
		<%:Html.TextBox("${Index}_CapitalAmountCalled","",new { @id="txtCapitalAmountCalled", @style="text-align:right", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup="javascript:manualCapitalCall.calcCCA();" })%>
	</td>
	<td>
		<%:Html.TextBox("${Index}_ManagementFeeInterest", "", new { @id = "txtManagementFeeInterest", @style = "text-align:right", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualCapitalCall.calcMFIAmt();" })%>
	</td>
	<td>
		<%:Html.TextBox("${Index}_InvestedAmountInterest", "", new { @id = "txtInvestedAmountInterest", @style = "text-align:right", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualCapitalCall.calcIAI();" })%>
	</td>
	<td>
		<%:Html.TextBox("${Index}_ManagementFees", "", new { @id = "txtManagementFees", @style = "text-align:right", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualCapitalCall.calcMF();" })%>
	</td>
	<td>
		<%:Html.TextBox("${Index}_FundExpenses", "", new { @id = "txtFundExpenses", @style = "text-align:right", @class = "hide", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:manualCapitalCall.calcFE();" })%>
	</td>
	<td style="text-align:right">
		<%: Html.Image("largedel.png", new { @onclick = "javascript:manualCapitalCall.deleteInvestor(this);", @class = "gbutton" })%>
	</td>
</tr>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div id="accountInfoMain">
	<div class="expandaddbtn">
		<%using (Html.GreenButton(new { @onclick = "{{if InvestorId>0}}javascript:editInvestor.addAccountInfo(this,${InvestorId});{{else}}javascript:investor.createAccount(this,0);{{/if}}" })) {%>Add
		New Account<%}%>
	</div>
	<div class="expandheader">
		<div class="expandbtn">
			<div class="expandimg" id="img">
				ADD BANK INFORMATION</div>
			<div class="expandtitle" id="title">
				<div class="expandtitle">
					BANK INFORMATION</div>
			</div>
		</div>
	</div>
	<div class="fieldbox" id="AccountInfoBox" style="display: none; width: 100%;">
		{{if InvestorId>0}} {{each(i,account) AccountInformations}} {{tmpl(account) "#BankInfoEditTemplate"}}
		{{/each}} {{/if}}
	</div>
</div>

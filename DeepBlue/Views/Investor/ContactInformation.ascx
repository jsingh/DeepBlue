<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div id="contactInfoMain">
	<div class="expandaddbtn">
		<%using (Html.GreenButton(new { @onclick = "{{if InvestorId>0}}javascript:editInvestor.addContactInfo(this, ${InvestorId});{{else}}javascript:investor.createContact();{{/if}}" })) {%>Add
		New Contact<%}%>
	</div>
	<div class="expandheader">
		<div class="expandbtn">
			<div class="expandimg" id="img">
				ADD CONTACT INFORMATION</div>
			<div class="expandtitle" id="title">
				<div class="expandtitle">
					CONTACT INFORMATION</div>
			</div>
		</div>
	</div>
	<div class="fieldbox" id="ContactInfoBox" style="display: none;width:100%;">
		{{if InvestorId>0}} 
		{{each(i,account) ContactInformations}} 
		{{tmpl(account) "#ContactInfoEditTemplate"}}
		{{/each}} 
		{{/if}}
	</div>
</div>
	<div class="line">
	</div>
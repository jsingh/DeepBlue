<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.ContactDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @onsubmit = "return false" })) {%>
<div id="ContactInfoAddNew" style="display: none" class="editor-row">
	<div style="float: right">
		<%: Html.Anchor(Html.Image("add.png", new { @title = "Add Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.addContactInfo(this);" })%>
	</div>
</div>
<div id="contactInfo${InvestorContactId}" class="editinfo contactInfo">
	<div class="editor-row">
		<div class="editor-editbtn">
			<div class="EditInvestorInfo show" style="float: left">
				<%: Html.Anchor(Html.Image("add.png", new { @title = "Add Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.addContactInfo(this, ${InvestorId});" })%>
				&nbsp;&nbsp;
				<%: Html.Anchor(Html.Image("Editbtn.png", new { @title = "Edit Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
				&nbsp;&nbsp;
				<%: Html.Anchor(Html.Image("Delete.png", new { @title = "Delete Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.deleteContact(this,${InvestorContactId});" })%>
			</div>
			<div class="UpdateInvestorInfo hide" style="float: left; display: none;">
				<%: Html.Span("", new { @id = "Loading" })%>
				<%: Html.Image("Update.png", new { @style="cursor:pointer", @onclick = "javascript:editInvestor.saveContactAddressDetail(this);",  @class = "hide" })%>
				<%: Html.Anchor(Html.Image("Cancel.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
			</div>
		</div>
	</div>
	<%: Html.jQueryTemplateHidden("ContactId")%>
	<%: Html.jQueryTemplateHidden("InvestorId")%>
	<%: Html.jQueryTemplateHidden("InvestorContactId")%>
	<%=Html.jQueryTemplateHiddenExpression("AddressId", "${getAddress(AddressInformations).AddressId}")%>
	<%=Html.jQueryTemplateHiddenExpression("ContactAddressId", "${getAddress(AddressInformations).ContactAddressId}")%>
	<div class="editor-label">
		<%: Html.Label("Contact Person") %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("Person", new { @class = "hide" })%>
		<%: Html.jQueryTemplateSpan("Person", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Designation") %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("Designation", new { @class = "hide" })%>
		<%: Html.jQueryTemplateSpan("Designation", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Phone Number") %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("Phone", "${getCommunicationValue(ContactCommunications," + ((int)DeepBlue.Models.Admin.Enums.CommunicationType.HomePhone).ToString() + ")}", new { @class = "comvalue hide" })%>
		<%: Html.jQueryTemplateSpan("Phone", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Fax Number") %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("Fax", "${getCommunicationValue(ContactCommunications," + ((int)DeepBlue.Models.Admin.Enums.CommunicationType.Fax).ToString() + ")}", new { @class = "comvalue hide" })%>
		<%: Html.jQueryTemplateSpan("Fax", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Email") %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("Email", "${getCommunicationValue(ContactCommunications," + ((int)DeepBlue.Models.Admin.Enums.CommunicationType.Email).ToString() + ")}", new { @class = "comvalue hide", @onblur = "javascript:jHelper.checkEmail(this);" })%>
		<%: Html.jQueryTemplateSpan("Email", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Web Address") %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("WebAddress", "${getCommunicationValue(ContactCommunications," + ((int)DeepBlue.Models.Admin.Enums.CommunicationType.WebAddress).ToString() + ")}", new { @class = "comvalue hide", @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
		<%: Html.jQueryTemplateSpan("WebAddress", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Address1") %>
	</div>
	<div class="editor-field">
		<%=Html.jQueryTemplateTextBoxExpression("Address1", "${getAddress(AddressInformations).Address1}", "hide")%>
		<%: Html.jQueryTemplateSpan("${getAddress(AddressInformations).Address1}", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Address2") %>
	</div>
	<div class="editor-field">
		<%=Html.jQueryTemplateTextBoxExpression("Address2", "${getAddress(AddressInformations).Address2}", "hide")%>
		<%: Html.jQueryTemplateSpan("${getAddress(AddressInformations).Address2}", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("City") %>
	</div>
	<div class="editor-field">
		<%=Html.jQueryTemplateTextBoxExpression("City", "${getAddress(AddressInformations).City}", "hide")%>
		<%: Html.jQueryTemplateSpan("${getAddress(AddressInformations).City}", "show")%>
	</div>
	<div class="editor-row" id="AddressStateRow" {{if getAddress(AddressInformations).Country!=225}}style="display:none"{{/if}}>
		<div class="editor-label">
			<%: Html.Label("State") %>
		</div>
		<div class="editor-field dropdown">
			<%=Html.jQueryTemplateTextBoxExpression("StateName", "${getAddress(AddressInformations).StateName}", "hide")%>
			<%: Html.jQueryTemplateSpan("${getAddress(AddressInformations).StateName}", "show")%>
			<%=Html.jQueryTemplateHiddenExpression("State", "${getAddress(AddressInformations).State}")%>
		</div>
	</div>
	<div class="editor-label">
		<%: Html.Label("Zip") %>
	</div>
	<div class="editor-field">
		<%=Html.jQueryTemplateTextBoxExpression("Zip", "${getAddress(AddressInformations).Zip}", "hide")%>
		<%: Html.jQueryTemplateSpan("${getAddress(AddressInformations).Zip}", "show")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Country")%>
	</div>
	<div class="editor-field dropdown">
		<%=Html.jQueryTemplateTextBoxExpression("CountryName", "${getAddress(AddressInformations).CountryName}", "hide")%>
		<%: Html.jQueryTemplateSpan("${getAddress(AddressInformations).CountryName}", "show")%>
		<%=Html.jQueryTemplateHiddenExpression("Country", "${getAddress(AddressInformations).Country}")%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Distribution Notices") %>
	</div>
	<div class="editor-field checkbox">
		<%: Html.CheckBox("DistributionNotices", false, new { @val = "${DistributionNotices}", @class = "hide" })%>
		<%: Html.Span("{{if DistributionNotices}}" + Html.Image("tick.png").ToHtmlString() + "{{/if}}", new { @class = "show" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Financials")%>
	</div>
	<div class="editor-field checkbox">
		<%: Html.CheckBox("Financials", false, new { @val = "${Financials}", @class = "hide" })%>
		<%: Html.Span("{{if Financials}}" + Html.Image("tick.png").ToHtmlString() + "{{/if}}", new { @class = "show" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("K1")%>
	</div>
	<div class="editor-field checkbox">
		<%: Html.CheckBox("K1", false, new { @val = "${K1}", @class = "hide" })%>
		<%: Html.Span("{{if K1}}" + Html.Image("tick.png").ToHtmlString() + "{{/if}}", new { @class = "show" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Investor Letters")%>
	</div>
	<div class="editor-field checkbox">
		<%: Html.CheckBox("InvestorLetters", false, new { @val = "${InvestorLetters}", @class = "hide" })%>
		<%: Html.Span("{{if InvestorLetters}}" + Html.Image("tick.png").ToHtmlString() + "{{/if}}", new { @class = "show" })%>
	</div>
</div>
<%}%>

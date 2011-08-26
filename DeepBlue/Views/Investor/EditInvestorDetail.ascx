<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.EditModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="edit-info" id="investorInfo">
	<div class="box" id="EditInvestorMain">
		<div class="box-top">
			<div class="box-left">
			</div>
			<div class="box-center">
				Investor:&nbsp;
				<%:Html.Span("",new { id = "TitleInvestorName" })%>
			</div>
			<div class="box-right">
			</div>
			<div class="box-content">
				<div class="edit-left">
					<div class="editor-label auto-width">
						<%: Html.Label("Investor Name")%>
					</div>
					<div class="display-field">
						<%: Html.Span("${InvestorName}", new { id = "InvestorName" })%>
					</div>
					<div class="editor-label auto-width">
						<%: Html.Label("Display Name")%>
					</div>
					<div class="display-field">
						<%: Html.Span("${DisplayName}", new { id = "Spn_DisplayName" })%>
					</div>
					<div class="editor-label" style="width: 82px;">
						<%: Html.Label("Notes")%>
					</div>
					<div class="display-field">
						<%: Html.Span("${Notes}", new { id = "Spn_Notes" })%>
					</div>
					<div id="funddetails" class="fund-details">
					</div>
					<div style="clear: both; height: 10px">
						&nbsp;</div>
					<div class="editor-button" style="width: 210px;">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Image("Delete.png", new { @id = "Delete", @style = "cursor:pointer", @onclick = "javascript:editInvestor.deleteInvestor(this);" })%>
						</div>
					</div>
				</div>
				<div id="accordion" class="edit-right">
					<h3>
						<a href="#">Investor Information</a>
					</h3>
					<div class="editinfo">
						<div class="editor-row">
							<div class="editor-editbtn">
								<div class="EditInvestorInfo" style="float: left">
									<%: Html.Anchor(Html.Image("Editbtn.png", new { @title = "Edit" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
								</div>
								<div class="UpdateInvestorInfo" style="float: left; display: none;">
									<%: Html.Anchor(Html.Image("Update.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.updateInvestorInfo(this);" })%>&nbsp;&nbsp;
									<%: Html.Anchor(Html.Image("Cancel.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
								</div>
							</div>
						</div>
						<%using (Html.Form(new { @onsubmit = "return editInvestor.saveInvestorDetail(this);" })) {%>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.SocialSecurityTaxId)%>
						</div>
						<div class="display-field">
							<%: Html.Span("${SocialSecurityTaxId}", new { id = "SocialSecurityTaxId" })%>
						</div>
						<div class="editor-label">
							<%: Html.Label("State of Residency")%>
						</div>
						<div class="editor-field dropdown">
							<%: Html.Span("${StateOfResidencyName}", new { @id = "Disp_StateOfResidency" })%>
							<%: Html.DropDownList("StateOfResidency", Model.SelectList.States, new { @val = "${StateOfResidency}" })%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.DomesticForeign) %>
						</div>
						<div class="editor-field dropdown">
							<%: Html.Span("${DomesticForeignName}", new { @id = "Disp_DomesticForeigns" })%>
							<%: Html.DropDownList("DomesticForeigns", Model.SelectList.DomesticForeigns, new { @val = "${DomesticForeign}" })%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.EntityType) %>
						</div>
						<div class="editor-field dropdown">
							<%: Html.Span("${EntityTypeName}", new { @id = "Disp_EntityType" })%>
							<%: Html.DropDownList("EntityType", Model.SelectList.InvestorEntityTypes, new { @val = "${EntityType}" })%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.DisplayName) %>
						</div>
						<div class="editor-field dropdown">
							<%: Html.Span("${DisplayName}", new { @id = "Disp_DisplayName" })%>
							<%: Html.TextBoxFor(model => model.DisplayName,Model.DisplayName) %>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.Notes) %>
						</div>
						<div class="display-field">
							<%: Html.Span("${Notes}", new { @id = "Disp_Notes" })%>
							<%: Html.TextArea("Notes","${Notes}",4,50,new {})%>
						</div>
						<% Html.RenderPartial("CustomFieldList", Model.CustomField);%>
						<%}%>
					</div>
					<h3>
						<a href="#">Address</a>
					</h3>
					<div id="addressInfoMain">
						{{each(i,address) AddressInformations}} {{tmpl(getItem(i,address)) "#AddressInfoTemplate"}}
						{{/each}}
					</div>
					<h3>
						<a href="#">Contact Person</a>
					</h3>
					<div id="contactInfoMain">
						<div id="ContactInfoAddNew" style="display: none" class="editor-row">
							<div style="float: right">
								<%: Html.Anchor(Html.Image("add.png", new { @title = "Add Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.addContactInfo(this);" })%>
							</div>
						</div>
						{{each(i,contact) ContactInformations}}
						<%using (Html.Form(new { @onsubmit = "return editInvestor.saveContactDetail(this);" })) {%>
						<div id="contactInfo" class="editinfo">
							<div class="editor-row">
								<div class="editor-editbtn">
									<div class="EditInvestorInfo" style="float: left">
										<%: Html.Anchor(Html.Image("add.png", new { @title = "Add Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.addContactInfo(this);" })%>
										&nbsp;&nbsp;
										<%: Html.Anchor(Html.Image("Editbtn.png", new { @title = "Edit Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
										&nbsp;&nbsp;
										<%: Html.Anchor(Html.Image("Delete.png", new { @title = "Delete Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.deleteContact(this,'" + Model.ContactInformations.Count.ToString() + "_ContactId');" })%>
									</div>
									<div class="UpdateInvestorInfo" style="float: left; display: none;">
										<%: Html.Anchor(Html.Image("Update.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.updateInvestorInfo(this);" })%>&nbsp;&nbsp;
										<%: Html.Anchor(Html.Image("Cancel.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
									</div>
								</div>
							</div>
							<%: Html.Hidden( "ContactId")%>
							<%: Html.Hidden( "ContactAddressId")%>
							<div class="editor-label">
								<%: Html.Label("Contact Person")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactPerson", "${ContactPerson}")%>
								<%: Html.Span("${ContactPerson}", new { @id = "Disp_" +  "ContactPerson" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Designation")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "Designation", "${Designation}")%>
								<%: Html.Span("${Designation}", new { @id = "Disp_" +  "Designation" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Phone Number")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactPhoneNumber", "${ContactPhoneNumber}")%>
								<%: Html.Span("${ContactPhoneNumber}", new { @id = "Disp_" +  "ContactPhoneNumber" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Fax Number")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactFaxNumber", "${ContactFaxNumber}")%>
								<%: Html.Span("${ContactFaxNumber}", new { @id = "Disp_" +  "ContactFaxNumber" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Email")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactEmail", "${ContactEmail}", new { @onblur = "javascript:jHelper.checkEmail(this);" })%>
								<%: Html.Span("${ContactEmail}", new { @id = "Disp_" +  "ContactEmail" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Web Address")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactWebAddress", "${ContactWebAddress}", new { @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
								<%: Html.Span("${ContactWebAddress}", new { @id = "Disp_" +  "ContactWebAddress" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Address1")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactAddress1", "${ContactAddress1}", new { maxlength = 40 })%>
								<%: Html.Span("${ContactAddress1}", new { @id = "Disp_" +  "ContactAddress1" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Address2")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactAddress2", "${ContactAddress2}", new { maxlength = 40 })%>
								<%: Html.Span("${ContactAddress2}", new { @id = "Disp_" +  "ContactAddress2" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("City")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactCity", "${ContactCity}", new { maxlength = 30 })%>
								<%: Html.Span("${ContactCity}", new { @id = "Disp_" +  "ContactCity" })%>
							</div>
							<div class="editor-row" id="${i}_ContactStateRow">
								<div class="editor-label">
									<%: Html.Label("State")%>
								</div>
								<div class="editor-field dropdown">
									<%: Html.DropDownList( "ContactState", Model.SelectList.States, new { @val = "${ContactState}" })%>
									<%: Html.Span("${ContactState}", new { @id = "Disp_" +  "ContactState" })%>
								</div>
							</div>
							<div class="editor-label">
								<%: Html.Label("Zip")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ContactPostalCode", "${ContactPostalCode}")%>
								<%: Html.Span("${ContactPostalCode}", new { @id = "Disp_" +  "ContactPostalCode" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Country")%>
							</div>
							<div class="editor-field dropdown">
								<%: Html.DropDownList( "ContactCountry", Model.SelectList.Countries, new {
	@val = "${ContactCountry}",
	@onchange = "javascript:editInvestor.changeCountry('" + Model.ContactInformations.Count.ToString() + "_ContactCountry','" + Model.ContactInformations.Count.ToString() + "_ContactState','" + Model.ContactInformations.Count.ToString() + "_ContactStateRow');"
})%>
								<%: Html.Span("${ContactCountry}", new { @id = "Disp_" +  "ContactCountry" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Distribution Notices")%>
							</div>
							<div class="editor-field checkbox">
								<%: Html.CheckBox( "DistributionNotices", false, new { @val = "${DistributionNotices}" })%>
								<%: Html.Span("${DistributionNotices}", new { @id = "Disp_" +  "DistributionNotices" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Financials")%>
							</div>
							<div class="editor-field checkbox">
								<%: Html.CheckBox( "Financials", false, new { @val = "${Financials}" })%>
								<%: Html.Span("${Financials}", new { @id = "Disp_" +  "Financials" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("K1")%>
							</div>
							<div class="editor-field checkbox">
								<%: Html.CheckBox( "K1", false, new { @val = "${K1}" })%>
								<%: Html.Span("${K1}", new { @id = "Disp_" +  "K1" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Investor Letters")%>
							</div>
							<div class="editor-field checkbox">
								<%: Html.CheckBox( "InvestorLetters", false, new { @val = "${InvestorLetters}" })%>
								<%: Html.Span("${InvestorLetters}", new { @id = "Disp_" +  "InvestorLetters" })%>
							</div>
						</div>
						<%}%>
						{{/each}}
					</div>
					<h3>
						<a href="#">Bank A/C Details</a>
					</h3>
					<div id="accountInfoMain">
						<div id="AccountInfoAddNew" style="display: none" class="editor-row">
							<div style="float: right">
								<%: Html.Anchor(Html.Image("add.png", new { @title = "Add Account" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.addAccountInfo(this);" })%>
							</div>
						</div>
						{{each(i,account) AccountInformations}}
						<%using (Html.Form(new { @onsubmit = "return editInvestor.saveBankDetail(this);" })) {%>
						<div id="accountInfo" class="editinfo">
							<div class="editor-row">
								<div class="editor-editbtn">
									<div class="EditInvestorInfo" style="float: left">
										<%: Html.Anchor(Html.Image("add.png", new { @title = "Add Account" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.addAccountInfo(this);" })%>
										&nbsp;&nbsp;
										<%: Html.Anchor(Html.Image("Editbtn.png", new { @title = "Edit" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
										&nbsp;&nbsp;
										<%: Html.Anchor(Html.Image("Delete.png", new { @title = "Delete Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.deleteAccount(this,'" + Model.ContactInformations.Count.ToString() + "_AccountId');" })%>
									</div>
									<div class="UpdateInvestorInfo" style="float: left; display: none;">
										<%: Html.Anchor(Html.Image("Update.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.updateInvestorInfo(this);" })%>&nbsp;&nbsp;
										<%: Html.Anchor(Html.Image("Cancel.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
									</div>
								</div>
							</div>
							<%: Html.Hidden( "AccountId")%>
							<div class="editor-label">
								<%: Html.Label("Bank Name") %>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "BankName","${BankName}") %>
								<%: Html.Span("${BankName}", new { @id = "Disp_" +  "BankName" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Account Number")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "AccountNumber", "${AccountNumber}")%>
								<%: Html.Span("${AccountNumber}", new { @id = "Disp_" +  "AccountNumber" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("ABA Number")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ABANumber", "${ABANumber}", new { @onkeydown = "return jHelper.isNumeric(event);" })%>
								<%: Html.Span("${ABANumber}", new { @id = "Disp_" +  "ABANumber" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Account Of")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "AccountOf", "${AccountOf}")%>
								<%: Html.Span("${AccountOf}", new { @id = "Disp_" +  "AccountOf" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("FFC")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "FFC", "${FFC}")%>
								<%: Html.Span("${FFC}", new { @id = "Disp_" +  "FFC" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("FFCNO")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "FFCNO", "${FFCNO}")%>
								<%: Html.Span("${FFCNO}", new { @id = "Disp_" +  "FFCNO" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Attention")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "Attention", "${Attention}")%>
								<%: Html.Span("${Attention}", new { @id = "Disp_" +  "Attention" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Swift")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "Swift", "${Swift}")%>
								<%: Html.Span("${Swift}", new { @id = "Disp_" +  "Swift" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("IBAN")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "IBAN", "${IBAN}")%>
								<%: Html.Span("${IBAN}", new { @id = "Disp_" +  "IBAN" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Reference")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "Reference", "${Reference}")%>
								<%: Html.Span("${Reference}", new { @id = "Disp_" +  "Reference" })%>
							</div>
							<div class="editor-label">
								<%: Html.Label("ByOrderOf")%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox( "ByOrderOf", "${ByOrderOf}")%>
								<%: Html.Span("${ByOrderOf}", new { @id = "Disp_" +  "ByOrderOf" })%>
							</div>
						</div>
						<%}%>
						{{/each}}
					</div>
				</div>
			</div>
			<%: Html.Hidden("AddressInfoCount") %>
			<%: Html.Hidden("ContactInfoCount")%>
			<%: Html.Hidden("AccountInfoCount")%>
			<%: Html.Hidden("InvestorId","",new {@id = "InvestorId"}) %>
		</div>
	</div>
</div>

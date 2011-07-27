<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Investor.EditModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Investor
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.JavascriptInclueTag("EditInvestor.js") %>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTORS</span><span class="arrow"></span><span class="pname">Update
					Investor Information</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="edit-investor">
		<div class="search">
			<div class="editor-label auto-width" style="padding: 8px;">
				<%: Html.Label("Investor:") %>
			</div>
			<div class="editor-field auto-width">
				<%: Html.TextBox("Investor")%>&nbsp;<%=Html.Span("",new { id = "Loading" })%>
			</div>
		</div>
		<div id="editinfo">
			<div class="edit-info" id="investorInfo" style="display: none;">
				<div class="box">
					<div class="box-top">
						<div class="box-left">
						</div>
						<div class="box-center">
							Investor:&nbsp;
							<%:Html.Span("",new { id = "TitleInvestorName" })%>
						</div>
						<div class="box-right">
						</div>
					</div>
					<%using (Ajax.BeginForm("Update", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "editInvestor.onBegin", OnSuccess = "editInvestor.onSuccess" }, new { @id = "frmInvestorInfo" })) {%>
					<div class="box-content">
						<div class="edit-left">
							<div class="editor-label auto-width">
								<%: Html.Label("Investor Name:") %>
							</div>
							<div class="display-field">
								<%: Html.Span("",new {id = "InvestorName"})%>
							</div>
							<div class="editor-label auto-width">
								<%: Html.Label("Display Name:") %>
							</div>
							<div class="display-field">
								<%: Html.Span("", new { id = "Spn_DisplayName" })%>
							</div>
							<div class="editor-label" style="width: 82px;">
								<%: Html.Label("Notes:") %>
							</div>
							<div class="display-field">
								<%: Html.Span("", new { id = "Spn_Notes" })%>
							</div>
							<div id="funddetails" class="fund-details">
							</div>
							<div style="clear: both; height: 10px">
								&nbsp;</div>
							<div class="editor-button" style="width: 210px;">
								<div style="float: left; padding: 0 0 10px 5px;">
									<%: Html.Image("Delete.png", new { @id = "Delete", @style = "cursor:pointer", @onclick = "javascript:editInvestor.deleteInvestor(this);" })%>
								</div>
								<div style="float: left; padding: 0 0 10px 5px; display: none;">
									<%: Html.ImageButton("Update.png", new { @id = "Update", @class="default-button" })%>
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
								<div class="editor-label">
									<%: Html.LabelFor(model => model.SocialSecurityTaxId)%>
								</div>
								<div class="display-field">
									<%: Html.Span("", new { id = "SocialSecurityTaxId"})%>
								</div>
								<div class="editor-label">
									<%: Html.Label("State of Residency:")%>
								</div>
								<div class="editor-field dropdown">
									<%: Html.Span("",new { @id = "Disp_StateOfResidency" })%>
									<%: Html.DropDownList("StateOfResidency", Model.SelectList.States)%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.DomesticForeign) %>
								</div>
								<div class="editor-field dropdown">
									<%: Html.Span("", new { @id = "Disp_DomesticForeigns" })%>
									<%: Html.DropDownList("DomesticForeigns", Model.SelectList.DomesticForeigns)%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.EntityType) %>
								</div>
								<div class="editor-field dropdown">
									<%: Html.Span("", new { @id = "Disp_EntityType" })%>
									<%: Html.DropDownList("EntityType", Model.SelectList.InvestorEntityTypes)%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.DisplayName) %>
								</div>
								<div class="editor-field dropdown">
									<%: Html.Span("", new { @id = "Disp_DisplayName" })%>
									<%: Html.TextBoxFor(model => model.DisplayName,Model.DisplayName) %>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.Notes) %>
								</div>
								<div class="display-field">
									<%: Html.Span("", new { @id = "Disp_Notes" })%>
									<%: Html.TextAreaFor(model => model.Notes,4,50,new {})%>
								</div>
								<% Html.RenderPartial("CustomFieldList", Model.CustomField);%>
							</div>
							<h3>
								<a href="#">Address</a>
							</h3>
							<div id="addressInfoMain">
								<div id="addressInfo" class="editinfo" style="display: none">
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
									<%--<h4>
										Address&nbsp;<span id="addressIndex"></span></h4>--%>
									<%: Html.Hidden(Model.AddressInformations.Count.ToString() + "_" + "AddressId")%>
									<div class="editor-label">
										<%: Html.Label("Phone:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Phone")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "Phone" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Fax:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Fax")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "Fax" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Email:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Email", "", new { @onblur = "javascript:jHelper.checkEmail(this);" })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "Email" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Web Address:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "WebAddress", "", new { @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "WebAddress" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Address1:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Address1")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "Address1" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Address2:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Address2", new {  maxlength = 40 })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "Address2" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("City:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "City", new {  maxlength = 30 })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "City" })%>
									</div>
									<div class="editor-row" id="<%=Model.AddressInformations.Count.ToString()%>_AddressStateRow">
										<div class="editor-label">
											<%: Html.Label("State:") %>
										</div>
										<div class="editor-field dropdown">
											<%: Html.DropDownList(Model.AddressInformations.Count.ToString() + "_" + "State", Model.SelectList.States)%>
											<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "State" })%>
										</div>
									</div>
									<div class="editor-label">
										<%: Html.Label("Zip:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "PostalCode")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "PostalCode" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Country:")%>
									</div>
									<div class="editor-field dropdown">
										<%: Html.DropDownList(Model.AddressInformations.Count.ToString() + "_" + "Country", Model.SelectList.Countries, new { @onchange = "javascript:editInvestor.changeCountry('" + Model.AddressInformations.Count.ToString() + "_Country','" + Model.AddressInformations.Count.ToString() + "_State','" + Model.AddressInformations.Count.ToString() + "_AddressStateRow');" })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AddressInformations.Count.ToString() + "_" + "Country" })%>
									</div>
								</div>
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
									<%--	<h4>
										Contact&nbsp;<span id="contactIndex"></span></h4>--%>
									<%: Html.Hidden(Model.ContactInformations.Count.ToString() + "_" + "ContactId")%>
									<%: Html.Hidden(Model.ContactInformations.Count.ToString() + "_" + "ContactAddressId")%>
									<div class="editor-label">
										<%: Html.Label("Contact Person:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactPerson")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactPerson" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Designation:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "Designation")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "Designation" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Phone Number:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactPhoneNumber")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactPhoneNumber" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Fax Number:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactFaxNumber")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactFaxNumber" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Email:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactEmail", "", new { @onblur = "javascript:jHelper.checkEmail(this);" })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactEmail" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Web Address:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactWebAddress", "", new { @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactWebAddress" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Address1:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactAddress1", new { maxlength = 40})%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactAddress1" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Address2:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactAddress2", new { maxlength = 40})%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactAddress2" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("City:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactCity", new { maxlength = 30 } )%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactCity" })%>
									</div>
									<div class="editor-row" id="<%=Model.ContactInformations.Count.ToString()%>_ContactStateRow">
										<div class="editor-label">
											<%: Html.Label("State:") %>
										</div>
										<div class="editor-field dropdown">
											<%: Html.DropDownList(Model.ContactInformations.Count.ToString() + "_" + "ContactState", Model.SelectList.States)%>
											<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactState" })%>
										</div>
									</div>
									<div class="editor-label">
										<%: Html.Label("Zip:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactPostalCode")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactPostalCode" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Country:") %>
									</div>
									<div class="editor-field dropdown">
										<%: Html.DropDownList(Model.ContactInformations.Count.ToString() + "_" + "ContactCountry", Model.SelectList.Countries, new { @onchange = "javascript:editInvestor.changeCountry('" + Model.ContactInformations.Count.ToString() + "_ContactCountry','" + Model.ContactInformations.Count.ToString() + "_ContactState','" + Model.ContactInformations.Count.ToString() + "_ContactStateRow');" })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "ContactCountry" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Distribution Notices:") %>
									</div>
									<div class="editor-field checkbox">
										<%: Html.CheckBox(Model.ContactInformations.Count.ToString() + "_" + "DistributionNotices",false)%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "DistributionNotices" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Financials:")%>
									</div>
									<div class="editor-field checkbox">
										<%: Html.CheckBox(Model.ContactInformations.Count.ToString() + "_" + "Financials",false)%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "Financials" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("K1:")%>
									</div>
									<div class="editor-field checkbox">
										<%: Html.CheckBox(Model.ContactInformations.Count.ToString() + "_" + "K1",false)%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "K1" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Investor Letters:")%>
									</div>
									<div class="editor-field checkbox">
										<%: Html.CheckBox(Model.ContactInformations.Count.ToString() + "_" + "InvestorLetters",false)%>
										<%: Html.Span("", new { @id = "Disp_" + Model.ContactInformations.Count.ToString() + "_" + "InvestorLetters" })%>
									</div>
								</div>
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
									<%--<h4>
										Account&nbsp;<span id="accountIndex"></span></h4>--%>
									<%: Html.Hidden(Model.AccountInformations.Count.ToString() + "_" + "AccountId")%>
									<div class="editor-label">
										<%: Html.Label("Bank Name:") %>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "BankName","") %>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "BankName" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Account Number:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "AccountNumber","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "AccountNumber" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("ABA Number:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "ABANumber", "", new { @onkeypress = "return jHelper.isNumeric(event);" })%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "ABANumber" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Account Of:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "AccountOf","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "AccountOf" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("FFC:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "FFC","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "FFC" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("FFCNO:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "FFCNO","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "FFCNO" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Attention:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "Attention","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "Attention" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Swift:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "Swift","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "Swift" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("IBAN:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "IBAN","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "IBAN" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Reference:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "Reference","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "Reference" })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("ByOrderOf:")%>
									</div>
									<div class="editor-field">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "ByOrderOf","")%>
										<%: Html.Span("", new { @id = "Disp_" + Model.AccountInformations.Count.ToString() + "_" + "ByOrderOf" })%>
									</div>
								</div>
							</div>
						</div>
					</div>
					<%: Html.Hidden("AddressInfoCount") %>
					<%: Html.Hidden("ContactInfoCount")%>
					<%: Html.Hidden("AccountInfoCount")%>
					<%: Html.Hidden("InvestorId","",new {@id = "InvestorId"}) %>
					<%}%>
				</div>
			</div>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.id) %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,OnSelect = "function(event, ui){ editInvestor.selectInvestor(ui.item.id);}"})%>
	<script type="text/javascript">
		editInvestor.init();
	</script>
</asp:Content>

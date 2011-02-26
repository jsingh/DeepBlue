﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Investor.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="TitleCnt" ContentPlaceHolderID="TitleContent" runat="server">
	Create Investor
</asp:Content>
<asp:Content ID="HeaderCnt" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("Investor.js")%><%=Html.StylesheetLinkTag("newInvestor.css")%>
</asp:Content>
<asp:Content ID="MainCnt" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.EnableClientValidation(); %>
	<% using (Html.BeginForm("Create", "Investor", FormMethod.Post, new { onsubmit = "return investor.validation();" })) {%>
	<%: Html.ValidationSummary(true) %>
	<br />
	<div class="new-investor">
		<div class="box">
			<div class="box-top">
				<div class="box-left">
				</div>
				<div class="box-center">
					Investor Information
				</div>
				<div class="box-right">
				</div>
			</div>
			<div class="box-content">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.InvestorName) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.InvestorName, new { maxlength = 100 })%>
					<%: Html.ValidationMessageFor(model => model.InvestorName)%>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Alias) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.Alias, new { maxlength = 50 })%>
					<%: Html.ValidationMessageFor(model => model.Alias) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.SocialSecurityTaxId)%>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.SocialSecurityTaxId)%>
					<%: Html.ValidationMessageFor(model => model.SocialSecurityTaxId)%>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.DomesticForeign) %>
				</div>
				<div class="editor-field dropdown">
					<%: Html.DropDownListFor(model => model.DomesticForeign,Model.SelectList.DomesticForeigns)%>
					<%: Html.ValidationMessageFor(model => model.DomesticForeign) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.StateOfResidency) %>
				</div>
				<div class="editor-field dropdown">
					<%: Html.DropDownListFor(model => model.StateOfResidency,Model.SelectList.States) %>
					<%: Html.ValidationMessageFor(model => model.StateOfResidency) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.EntityType) %>
				</div>
				<div class="editor-field dropdown">
					<%: Html.DropDownListFor(model => model.EntityType, Model.SelectList.InvestorEntityTypes)%>
					<%: Html.ValidationMessageFor(model => model.EntityType) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.FOIA) %>
				</div>
				<div class="editor-field checkbox">
					<%: Html.CheckBoxFor(model => model.FOIA)%>
				</div>
				<div class="editor-label ">
					<%: Html.LabelFor(model => model.ERISA) %>
				</div>
				<div class="editor-field checkbox">
					<%: Html.CheckBoxFor(model => model.ERISA)%>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Source) %>
				</div>
				<div class="editor-field dropdown">
					<%: Html.DropDownListFor(model => model.Source,Model.SelectList.Source)%>
				</div>
			</div>
		</div>
		<br />
		<div class="box">
			<div class="box-top">
				<div class="box-left">
				</div>
				<div class="box-center">
					Address Information
				</div>
				<div class="box-right">
				</div>
			</div>
			<div class="box-content">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Phone) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.Phone)%>
					<%: Html.ValidationMessageFor(model => model.Phone) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Fax) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.Fax)%>
					<%: Html.ValidationMessageFor(model => model.Fax) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Email) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.Email)%>
					<%: Html.ValidationMessageFor(model => model.Email) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.WebAddress) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.WebAddress)%>
					<%: Html.ValidationMessageFor(model => model.WebAddress) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Address1) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.Address1, new { maxlength = 40 })%>
					<%: Html.ValidationMessageFor(model => model.Address1) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Address2) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.Address2, new { maxlength = 40 })%>
					<%: Html.ValidationMessageFor(model => model.Address2) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.City) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.City, new { maxlength = 30 })%>
					<%: Html.ValidationMessageFor(model => model.City) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.State) %>
				</div>
				<div class="editor-field dropdown">
					<%: Html.DropDownListFor(model => model.State,Model.SelectList.States)%>
					<%: Html.ValidationMessageFor(model => model.State) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Zip) %>
				</div>
				<div class="editor-field text">
					<%: Html.TextBoxFor(model => model.Zip)%>
					<%: Html.ValidationMessageFor(model => model.Zip) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Country) %>
				</div>
				<div class="editor-field dropdown">
					<%: Html.DropDownListFor(model => model.Country, Model.SelectList.Countries)%>
					<%: Html.ValidationMessageFor(model => model.Country) %>
				</div>
			</div>
		</div>
		<br />
		<div class="box">
			<div class="box-top">
				<div class="box-left">
				</div>
				<div class="box-center">
					Bank Information
				</div>
				<div class="box-right">
				</div>
			</div>
			<%: Html.HiddenFor(model => Model.AccountLength)%>
			<div class="box-content">
				<div id="AccountInfoBox" class="box-main">
					<div id="AccountInfo" class="accountinfo">
						<div class="title">
							<h2>
								Account
							</h2>
						</div>
						<div class="delete">
							<%=Html.ImageLink("Delete.png", new { onclick = "javascript:investor.deleteAccount(this);" })%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.BankName) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "BankName") %>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.AccountNumber) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "AccountNumber")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.ABANumber) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "ABANumber")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.AccountOf) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "AccountOf")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.FFC) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "FFC")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.FFCNO) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "FFCNO")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.Attention) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "Attention")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.Swift) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "Swift")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.IBAN) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "IBAN")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.Reference) %>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "Reference")%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.ByOrderOf)%>
						</div>
						<div class="editor-field text">
							<%: Html.TextBox(Model.AccountLength + "_" + "ByOrderOf")%>
						</div>
					</div>
					<div class="add">
						<%=Html.ImageLink("add_icon.png", new { title = "Add New Account", onclick = "javascript:investor.createAccount(this);" })%>
					</div>
				</div>
			</div>
		</div>
		<br />
		<div class="box">
			<div class="box-top">
				<div class="box-left">
				</div>
				<div class="box-center">
					Contact Information
				</div>
				<div class="box-right">
				</div>
			</div>
			<%: Html.HiddenFor(model => Model.ContactLength)%>
			<div class="box-content">
				<div id="ContactInfoBox" class="box-main">
					<div id="ContactInfo" class="contactinfo">
						<div>
							<div class="title">
								<h2>
									Contact
								</h2>
							</div>
							<div class="delete">
								<%=Html.ImageLink("Delete.png", new { title = "Delete Contact", onclick = "javascript:investor.deleteContact(this);" })%>
							</div>
							<div   class="add">
								<%=Html.ImageLink("add_icon.png", new { title = "Add New Contact", onclick = "javascript:investor.createContact(this);" })%>
							</div>
						</div>
						<div class="contactinfo-box">
							<div class="contactinfo-left">
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactPerson) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactPerson")%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.Designation) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "Designation") %>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactPhoneNumber) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactPhoneNumber") %>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactFaxNumber) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactFaxNumber") %>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactEmail) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactEmail") %>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactWebAddress) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactWebAddress") %>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactAddress1) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactAddress1", "", new { maxlength = 40 })%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactAddress2) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactAddress2","", new { maxlength = 40 })%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactCity) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactCity", "", new { maxlength = 30 })%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactState) %>
								</div>
								<div class="editor-field dropdown">
									<%: Html.DropDownList(Model.ContactLength.ToString() + "_" + "ContactState",Model.SelectList.States)%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactZip) %>
								</div>
								<div class="editor-field text">
									<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactZip")%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.ContactCountry) %>
								</div>
								<div class="editor-field dropdown">
									<%: Html.DropDownList(Model.ContactLength.ToString() + "_" + "ContactCountry", Model.SelectList.Countries)%>
								</div>
							</div>
							<div class="contactinfo-right">
								<div class="editor-label">
									<%: Html.LabelFor(model => model.DistributionNotices) %>
								</div>
								<div class="editor-field checkbox">
									<%: Html.CheckBox(Model.ContactLength.ToString() + "_" + "DistributionNotices",new {style = "width:auto"})%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.Financials)%>
								</div>
								<div class="editor-field checkbox">
									<%: Html.CheckBox(Model.ContactLength.ToString() + "_" + "Financials", new { style = "width:auto" })%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.K1)%>
								</div>
								<div class="editor-field checkbox">
									<%: Html.CheckBox(Model.ContactLength.ToString() + "_" + "K1", new { style = "width:auto" })%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.InvestorLetters)%>
								</div>
								<div class="editor-field checkbox">
									<%: Html.CheckBox(Model.ContactLength.ToString() + "_" + "InvestorLetters", new { style = "width:auto" })%>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="box">
			<div class="box-top">
				<div class="box-left">
				</div>
				<div class="box-center">
					Notes
				</div>
				<div class="box-right">
				</div>
			</div>
			<div class="box-content">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Notes) %>
				</div>
				<div class="editor-field textarea">
					<%: Html.TextAreaFor(model => model.Notes,6,60,null)%>
				</div>
			</div>
		</div>
		<div class="editor-button">
			<%: Html.ImageButton("submit.png", new { style = "width: 73px; height: 23px;" })%>
		</div>
		<% } %>
	</div>
</asp:Content>

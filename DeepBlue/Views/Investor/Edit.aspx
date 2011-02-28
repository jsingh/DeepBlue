<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Investor.EditModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.JavascriptInclueTag("EditInvestor.js") %>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="edit-investor">
		<div class="search">
			<div class="editor-label auto-width">
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
					<%using (Ajax.BeginForm("Update", null, new AjaxOptions { HttpMethod = "Post", OnBegin = "editInvestor.onBegin", OnSuccess = "editInvestor.onSuccess" }, new { onclick = "return editInvestor.submit(this);" })) {%>
					<div class="box-content">
						<div class="edit-left">
							<div class="editor-label auto-width">
								<%: Html.Label("Investor Name") %>
							</div>
							<div class="display-field">
								<%: Html.Span("",new {id = "InvestorName"})%>
							</div>
							<div class="editor-label auto-width">
								<%: Html.Label("Display Name") %>
							</div>
							<div class="display-field">
								<%: Html.Span("", new { id = "DisplayName" })%>
							</div>
							<div id="funddetails" class="fund-details">
							</div>
						</div>
						<div id="accordion" class="edit-right">
							<h3>
								<a href="#">Investor Information</a></h3>
							<div>
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
									<%: Html.DropDownList("StateOfResidency", Model.SelectList.States)%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.DomesticForeign) %>
								</div>
								<div class="editor-field dropdown">
									<%: Html.DropDownList("DomesticForeigns", Model.SelectList.DomesticForeigns)%>
								</div>
								<div class="editor-label">
									<%: Html.LabelFor(model => model.EntityType) %>
								</div>
								<div class="editor-field dropdown">
									<%: Html.DropDownList("EntityType", Model.SelectList.InvestorEntityTypes)%>
								</div>
							</div>
							<h3>
								<a href="#">Address</a></h3>
							<div id="addressInfoMain">
								<div id="addressInfo" class="address-info" style="display: none">
									<h4>
										Address&nbsp;<span id="addressIndex"></span></h4>
									<%: Html.Hidden(Model.AddressInformations.Count.ToString() + "_" + "AddressId")%>
									<div class="editor-label">
										<%: Html.Label("Phone:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Phone"
																)%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Fax:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox("Fax")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Email:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Email")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Web Address:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "WebAddress")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Address1:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Address1")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Address2:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "Address2", new {  maxlength = 40 })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("City:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "City", new {  maxlength = 30 })%>
									</div>
									<div class="editor-label">
										<%: Html.Label("State:") %>
									</div>
									<div class="editor-field dropdown">
										<%: Html.DropDownList(Model.AddressInformations.Count.ToString() + "_" + "State", Model.SelectList.States)%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Zip:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AddressInformations.Count.ToString() + "_" + "PostalCode")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Country:")%>
									</div>
									<div class="editor-field dropdown">
										<%: Html.DropDownList(Model.AddressInformations.Count.ToString() + "_" + "Country", Model.SelectList.Countries)%>
									</div>
								</div>
							</div>
							<h3>
								<a href="#">Contact Person</a>
							</h3>
							<div id="contactInfoMain">
								<div id="contactInfo" class="contactinfo">
									<h4>
										Contact&nbsp;<span id="contactIndex"></span></h4>
									<%: Html.Hidden(Model.ContactInformations.Count.ToString() + "_" + "ContactId")%>
									<%: Html.Hidden(Model.ContactInformations.Count.ToString() + "_" + "ContactAddressId")%>
									<div class="editor-label">
										<%: Html.Label("Contact Person:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactPerson")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Designation:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "Designation")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Phone Number:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactPhoneNumber")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Fax Number:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactFaxNumber")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Email:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactEmail")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Web Address:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactWebAddress")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Address1:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactAddress1", new { maxlength = 40})%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Address2:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactAddress2", new { maxlength = 40})%>
									</div>
									<div class="editor-label">
										<%: Html.Label("City:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactCity", new { maxlength = 30 } )%>
									</div>
									<div class="editor-label">
										<%: Html.Label("State:") %>
									</div>
									<div class="editor-field dropdown">
										<%: Html.DropDownList(Model.ContactInformations.Count.ToString() + "_" + "ContactState", Model.SelectList.States)%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Zip:") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.ContactInformations.Count.ToString() + "_" + "ContactPostalCode")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Country:") %>
									</div>
									<div class="editor-field dropdown">
										<%: Html.DropDownList(Model.ContactInformations.Count.ToString() + "_" + "ContactCountry", Model.SelectList.Countries)%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Distribution Notices") %>
									</div>
									<div class="editor-field checkbox">
										<%: Html.CheckBox(Model.ContactInformations.Count.ToString() + "_" + "DistributionNotices",false)%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Financials")%>
									</div>
									<div class="editor-field checkbox">
										<%: Html.CheckBox(Model.ContactInformations.Count.ToString() + "_" + "Financials",false)%>
									</div>
									<div class="editor-label">
										<%: Html.Label("K1")%>
									</div>
									<div class="editor-field checkbox">
										<%: Html.CheckBox(Model.ContactInformations.Count.ToString() + "_" + "K1",false)%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Investor Letters")%>
									</div>
									<div class="editor-field checkbox">
										<%: Html.CheckBox(Model.ContactInformations.Count.ToString() + "_" + "InvestorLetters",false)%>
									</div>
								</div>
							</div>
							<h3>
								<a href="#">Bank A/C Details</a>
							</h3>
							<div id="accountInfoMain">
								<div id="accountInfo" class="accountInfo">
									<h4>
										Account&nbsp;<span id="accountIndex"></span></h4>
									<%: Html.Hidden(Model.AccountInformations.Count.ToString() + "_" + "AccountId")%>
									<div class="editor-label">
										<%: Html.Label("Bank Name") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "BankName","") %>
									</div>
									<div class="editor-label">
										<%: Html.Label("Account Number") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "AccountNumber","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("ABA Number") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "ABANumber","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Account Of") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "AccountOf","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("FFC") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "FFC","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("FFCNO") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "FFCNO","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Attention") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "Attention","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Swift") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "Swift","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("IBAN") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "IBAN","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("Reference") %>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "Reference","")%>
									</div>
									<div class="editor-label">
										<%: Html.Label("ByOrderOf")%>
									</div>
									<div class="editor-field small-text-180">
										<%: Html.TextBox(Model.AccountInformations.Count + "_" + "ByOrderOf","")%>
									</div>
								</div>
							</div>
						</div>
						<div style="clear: both; height: 10px">
							&nbsp;</div>
						<div class="editor-button" style="width: 210px;">
							<div style="float: left; padding: 0 0 10px 5px;">
								<%: Html.ImageButton("Update.png", new { style = "width: 73px; height: 23px;" })%>
							</div>
							<div style="float: left; padding: 0 0 10px 5px;">
								<%: Html.Span("",new { id = "UpdateLoading" })%>
							</div>
						</div>
						<%: Html.Hidden("InvestorId","",new {id = "InvestorId"}) %>
					</div>
					<%: Html.Hidden("AddressInfoCount") %>
					<%: Html.Hidden("ContactInfoCount")%>
					<%: Html.Hidden("AccountInfoCount")%>
					<%}%>
				</div>
			</div>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.id) %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoCompleteScript("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,
																			OnSelect = "function(event, ui){ editInvestor.selectInvestor(ui.item.id);}"
})%>

	<script type="text/javascript">
		editInvestor.init();
	</script>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Member.CreateModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Member
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

	<script src="../../Assets/javascripts/Member.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.EnableClientValidation(); %>
	<% using (Html.BeginForm("Create", "Member", FormMethod.Post)) {%>
    <%: Html.ValidationSummary(true) %>
	<br />
	<div class="box">
		<div class="box-top">
			<div class="box-left">
			</div>
			<div class="box-center">
				Member Information
			</div>
			<div class="box-right">
			</div>
		</div>
		<div class="box-content">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.MemberName) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBoxFor(model => model.MemberName) %>
				<%: Html.ValidationMessageFor(model => model.MemberName) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Alias) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBoxFor(model => model.Alias) %>
				<%: Html.ValidationMessageFor(model => model.Alias) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Social) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBoxFor(model => model.Social) %>
				<%: Html.ValidationMessageFor(model => model.Social) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.DomesticForeign) %>
			</div>
			<div class="editor-field checkbox">
				<%: Html.CheckBoxFor(model => model.DomesticForeign,new {style = "width:auto;"}) %>
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
				<%: Html.DropDownListFor(model => model.EntityType, Model.SelectList.MemberEntityTypes)%>
				<%: Html.ValidationMessageFor(model => model.EntityType) %>
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
			<div style="display: none">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.AddressType) %>
				</div>
				<div class="editor-field dropdown">
					<%: Html.DropDownListFor(model => model.AddressType,Model.SelectList.AddressTypes) %>
					<%: Html.ValidationMessageFor(model => model.Phone) %>
				</div>
			</div>
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
				<%: Html.TextBoxFor(model => model.Address1)%>
				<%: Html.ValidationMessageFor(model => model.Address1) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Address2) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBoxFor(model => model.Address2)%>
				<%: Html.ValidationMessageFor(model => model.Address2) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.City) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBoxFor(model => model.City)%>
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
						<img src="../../Assets/images/Delete.png" onclick="javascript:Member.deleteAccount(this);" />
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.BankName) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBox(Model.AccountLength + "_" + "BankName") %>
						<%: Html.ValidationMessageFor(model => model.BankName) %>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.AccountNumber) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBox(Model.AccountLength + "_" + "AccountNumber")%>
						<%: Html.ValidationMessageFor(model => model.AccountNumber) %>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.ABANumber) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBox(Model.AccountLength + "_" + "ABANumber")%>
						<%: Html.ValidationMessageFor(model => model.ABANumber) %>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.AccountOf) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBox(Model.AccountLength + "_" + "AccountOf")%>
						<%: Html.ValidationMessageFor(model => model.AccountOf) %>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Reference) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBox(Model.AccountLength + "_" + "Reference")%>
						<%: Html.ValidationMessageFor(model => model.Reference) %>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Attention) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBox(Model.AccountLength + "_" + "Attention")%>
						<%: Html.ValidationMessageFor(model => model.Attention) %>
					</div>
				</div>
				<div class="add">
					<img src="../../Assets/images/add_icon.png" title="Add New Account" onclick="javascript:Member.createAccount();" />
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
						<div class="title" style="width: 530px; text-align: left">
							<h2>
								Contact
							</h2>
						</div>
						<div style="float: left" class="delete">
							<img src="../../Assets/images/Delete.png" onclick="javascript:Member.deleteContact(this);" />
						</div>
						<div style="float: left; margin-top: -12px;" class="add">
							<img src="../../Assets/images/add_icon.png" title="Add New Contact" onclick="javascript:Member.createContact();" />
						</div>
					</div>
					<div style="clear: both">
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
								<%: Html.ValidationMessageFor(model => model.Designation) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.ContactPhoneNumber) %>
							</div>
							<div class="editor-field text">
								<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactPhoneNumber") %>
								<%: Html.ValidationMessageFor(model => model.ContactPhoneNumber) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.ContactFaxNumber) %>
							</div>
							<div class="editor-field text">
								<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactFaxNumber") %>
								<%: Html.ValidationMessageFor(model => model.ContactFaxNumber) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.ContactEmail) %>
							</div>
							<div class="editor-field text">
								<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactEmail") %>
								<%: Html.ValidationMessageFor(model => model.ContactEmail) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.ContactWebAddress) %>
							</div>
							<div class="editor-field text">
								<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactWebAddress") %>
								<%: Html.ValidationMessageFor(model => model.ContactWebAddress) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.ContactAddress1) %>
							</div>
							<div class="editor-field text">
								<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactAddress1") %>
								<%: Html.ValidationMessageFor(model => model.ContactAddress1) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.ContactAddress2) %>
							</div>
							<div class="editor-field text">
								<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactAddress2") %>
								<%: Html.ValidationMessageFor(model => model.ContactAddress2) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.ContactCity) %>
							</div>
							<div class="editor-field text">
								<%: Html.TextBox(Model.ContactLength.ToString() + "_" + "ContactCity") %>
								<%: Html.ValidationMessageFor(model => model.ContactCity) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.ContactState) %>
							</div>
							<div class="editor-field dropdown">
								<%: Html.DropDownList(Model.ContactLength.ToString() + "_" + "ContactState",Model.SelectList.States)%>
								<%: Html.ValidationMessageFor(model => model.ContactState) %>
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
								<%: Html.ValidationMessageFor(model => model.ContactCountry) %>
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
	<div class="editor-label">
		&nbsp;
	</div>
	<div class="editor-field button">
		<input type="image" src="../../Assets/images/submit.png" style="width: 73px; height: 23px;" />
	</div>
	<% } %>
	<div>
		<%: Html.ActionLink("Back to List", "Index") %>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		Member.init();
	</script>

</asp:Content>

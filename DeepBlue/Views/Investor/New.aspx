<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Investor.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="TitleCnt" ContentPlaceHolderID="TitleContent" runat="server">
	Investor
</asp:Content>
<asp:Content ID="HeaderCnt" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("Investor.js")%>
	<%=Html.StylesheetLinkTag("newinvestor.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTORS</span><span class="arrow"></span><span class="pname">New
					Investor Setup</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="MainCnt" ContentPlaceHolderID="MainContent" runat="server">
	<%using (Html.Form(new { @id = "NewInvestor", @onsubmit = "return false;" })) { %>
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
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.InvestorName) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.InvestorName, new { maxlength = 100 })%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Alias) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.Alias, new { maxlength = 50 })%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.SocialSecurityTaxId)%>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.SocialSecurityTaxId)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.DomesticForeign) %>
					</div>
					<div class="editor-field dropdown">
						<%: Html.DropDownListFor(model => model.DomesticForeign,Model.SelectList.DomesticForeigns)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.StateOfResidency) %>
					</div>
					<div class="editor-field dropdown">
						<%: Html.TextBox("StateOfResidencyName","")%>
						<%: Html.HiddenFor(model => model.StateOfResidency) %>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.EntityType) %>
					</div>
					<div class="editor-field dropdown">
						<%: Html.DropDownListFor(model => model.EntityType, Model.SelectList.InvestorEntityTypes)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.FOIA) %>
					</div>
					<div class="editor-field checkbox">
						<%: Html.CheckBoxFor(model => model.FOIA)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label ">
						<%: Html.LabelFor(model => model.ERISA) %>
					</div>
					<div class="editor-field checkbox">
						<%: Html.CheckBoxFor(model => model.ERISA)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Source) %>
					</div>
					<div class="editor-field dropdown">
						<%: Html.DropDownListFor(model => model.Source,Model.SelectList.Source)%>
					</div>
				</div>
				<% Html.RenderPartial("CustomFieldList", Model.CustomField);%>
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
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Phone) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.Phone)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Fax) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.Fax)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Email) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.Email)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.WebAddress)%>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.WebAddress, new { @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Address1) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.Address1, new { maxlength = 40 })%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Address2) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.Address2, new { maxlength = 40 })%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.City) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.City, new { maxlength = 30 })%>
					</div>
				</div>
				<div class="editor-row" id="StateRow">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.State) %>
					</div>
					<div class="editor-field dropdown">
						<%: Html.TextBox("StateName","")%>
						<%: Html.HiddenFor(model => model.State)%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Zip) %>
					</div>
					<div class="editor-field text">
						<%: Html.TextBoxFor(model => model.Zip, new { @maxlength = "10" })%>
					</div>
				</div>
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Country) %>
					</div>
					<div class="editor-field dropdown">
						<%: Html.TextBoxFor(model => model.CountryName)%>
						<%: Html.HiddenFor(model => model.Country)%>
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
					Bank Information
				</div>
				<div class="box-right">
				</div>
			</div>
			<%: Html.HiddenFor(model => Model.AccountLength)%>
			<div class="box-content">
				<div id="AccountInfoBox" class="box-main">
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
				</div>
			</div>
		</div>
		<br />
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
				<div class="editor-row">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.Notes) %>
					</div>
					<div class="editor-field textarea">
						<%: Html.TextAreaFor(model => model.Notes,6,60,null)%>
					</div>
				</div>
			</div>
		</div>
		<div class="status">
			<%: Html.Span("", new { id = "UpdateLoading" })%></div>
		<div class="editor-button">
			<%: Html.Image("submit.png", new { @class = "default-button", @onclick = "javascript:investor.save($('#NewInvestor')); " })%>
		</div>
		<%: Html.HiddenFor(model => model.InvestorId)%>
		<div id="UpdateTargetId" style="display: none">
		</div>
	</div>
	<% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("StateOfResidencyName", new AutoCompleteOptions {
																	  Source = "/Admin/FindStates", MinLength = 1,
																	  OnSelect = "function(event, ui) { $('#StateOfResidency').val(ui.item.id); }"
	})%>
	<%= Html.jQueryAutoComplete("StateName", new AutoCompleteOptions {
																	  Source = "/Admin/FindStates", MinLength = 1,
																	  OnSelect = "function(event, ui) { $('#State').val(ui.item.id); }"
	})%>
	<%= Html.jQueryAutoComplete("CountryName", new AutoCompleteOptions {
																	  Source = "/Admin/FindCountrys", MinLength = 1,
																	  OnSelect = "function(event, ui) { investor.changeCountry(ui.item); }"
	})%>
	<script type="text/javascript">
		$(document).ready(function () {
			var bdy=$("body");
			jHelper.jqCheckBox(bdy);
			jHelper.jqComboBox(bdy);
			investor.createAccount();
			investor.createContact();
		});
	</script>
	<script id="BankInformationTemplate" type="text/x-jquery-tmpl"> 
		<div id="AccountInfo${i}" class="accountinfo">
			<div style="width: 100%;">	
				{{if i>1}}
				<div class="delete" style="display:block;clear:none;">
					<%=Html.Image("Delete.png", new { @title = "Delete Accout",  @onclick = "javascript:investor.deleteAccount(this);" })%>
				</div>
				{{/if}}
				<div class="add">
					<%=Html.Image("add.png", new { @title = "Add New Bank Information", @onclick = "javascript:investor.createAccount(this);" })%>
				</div>
			</div> 
			<%Html.RenderPartial("NewBankInformation", new DeepBlue.Models.Investor.BankDetail());%>
		</div>
	</script>
	<script id="ContactInformationTemplate" type="text/x-jquery-tmpl"> 
		<div id="ContactInfo${i}" class="contactinfo">
			<div>
				{{if i>1}}
				<div class="delete" style="display:block;clear:none;">
					<%=Html.Image("Delete.png", new { @title = "Delete Contact", @onclick = "javascript:investor.deleteContact(this);" })%>
				</div>
				{{/if}}
				<div class="add">
					<%=Html.Image("add.png", new { @title = "Add New Contact", @onclick = "javascript:investor.createContact(this);" })%>
				</div>
			</div>
			<%DeepBlue.Models.Investor.ContactDetail contactDetail = new DeepBlue.Models.Investor.ContactDetail();
			Html.RenderPartial("NewContactInformation", contactDetail);
			%>
		</div>
	</script>
	<script type="text/javascript">
		investor.newContactDetail  = <%= JsonSerializer.ToJsonObject(contactDetail)%>;
	</script>
</asp:Content>

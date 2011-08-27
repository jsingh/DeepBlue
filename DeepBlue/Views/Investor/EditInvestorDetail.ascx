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
								<div class="EditInvestorInfo show" style="float: left">
									<%: Html.Anchor(Html.Image("Editbtn.png", new { @title = "Edit" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
								</div>
								<div class="UpdateInvestorInfo hide" style="float: left; display: none;">
									<%: Html.Anchor(Html.Image("Update.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>&nbsp;&nbsp;
									<%: Html.Anchor(Html.Image("Cancel.png").ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
								</div>
							</div>
						</div>
						<%using (Html.Form(new { @onsubmit = "return false" })) {%>
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
							<%: Html.Span("${StateOfResidencyName}", new { @id = "Disp_StateOfResidency", @class = "show" })%>
							<%: Html.DropDownList("StateOfResidency", Model.SelectList.States, new { @class="hide", @val = "${StateOfResidency}" })%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.DomesticForeign) %>
						</div>
						<div class="editor-field dropdown">
							<%: Html.Span("${DomesticForeignName}", new { @id = "Disp_DomesticForeigns", @class = "show" })%>
							<%: Html.DropDownList("DomesticForeigns", Model.SelectList.DomesticForeigns, new { @class = "hide", @val = "${DomesticForeign}" })%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.EntityType) %>
						</div>
						<div class="editor-field dropdown">
							<%: Html.Span("${EntityTypeName}", new { @id = "Disp_EntityType", @class = "show" })%>
							<%: Html.DropDownList("EntityType", Model.SelectList.InvestorEntityTypes, new { @class = "hide", @val = "${EntityType}" })%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.DisplayName) %>
						</div>
						<div class="editor-field dropdown">
							<%: Html.Span("${DisplayName}", new { @id = "Disp_DisplayName", @class = "show" })%>
							<%: Html.TextBoxFor(model => model.DisplayName, new { @class = "hide" })%>
						</div>
						<div class="editor-label">
							<%: Html.LabelFor(model => model.Notes) %>
						</div>
						<div class="display-field">
							<%: Html.Span("${Notes}", new { @id = "Disp_Notes" })%>
							<%: Html.TextArea("Notes","${Notes}",4,30,new {})%>
						</div>
						<% Html.RenderPartial("CustomFieldList", Model.CustomField);%>
						<%}%>
					</div>
					<h3>
						<a href="#">Address</a>
					</h3>
					<div id="addressInfoMain">
						{{if AddressInformations.length>0}} {{each(i,address) AddressInformations}} {{tmpl(getItem(i,address))
						"#AddressInfoTemplate"}} {{/each}} {{else}}
						{{tmpl(getNewAddress(id)) "#AddressInfoTemplate"}} {{/if}}
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
						{{if ContactInformations.length>0}} {{each(i,contact) ContactInformations}} {{tmpl(getItem(i,contact))
						"#ContactInfoTemplate"}} {{/each}} {{else}}
						{{tmpl(getNewContact(id)) "#ContactInfoTemplate"}} {{/if}}
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
						{{if AccountInformations.length>0}}
						 {{each(i,account) AccountInformations}} {{tmpl(getItem(i,account))
						"#BankInfoTemplate"}} 
						{{/each}} 
						{{else}}
						{{tmpl(getNewBank(id)) "#BankInfoTemplate"}} 
						{{/if}}
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

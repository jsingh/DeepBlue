<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.EditModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="edit-info" id="investorInfo">
	<div class="box" id="EditInvestorMain">
		<div class="box-top">
			<div class="box-left">
			</div>
			<div class="box-center">
				Investor:&nbsp;<%:Html.Span("${InvestorName}",new { id = "TitleInvestorName" })%>
			</div>
			<div class="box-right">
			</div>
			<div class="box-content">
				<div class="edit-left" id="displayInvestorInfoMain">
					{{tmpl "#DisplayInvestorInfoTemplate"}}
				</div>
				<div id="accordion" class="edit-right">
					<h3>
						<a href="#">Investor Information</a>
					</h3>
					<div id="investorInfoMain" class="editinfo">
						{{tmpl "#InvestorInfoEditTemplate"}}
					</div>
					<h3>
						<a href="#">Address</a>
					</h3>
					<div id="addressInfoMain">
						{{if AddressInformations.length>0}} {{each(i,address) AddressInformations}} {{tmpl(getItem(i,address))
						"#AddressInfoTemplate"}} {{/each}} {{else}} {{tmpl(getNewAddress(InvestorId)) "#AddressInfoTemplate"}}
						{{/if}}
					</div>
					<h3>
						<a href="#">Contact Person</a>
					</h3>
					<div id="contactInfoMain">
						<div id="ContactInfoAddNew" style="display: none" class="editor-row">
							<div style="float: right">
								<%: Html.Anchor(Html.Image("add.png", new { @title = "Add Contact" }).ToHtmlString(), "#", new { @onclick = "javascript:editInvestor.addContactInfo(this,${InvestorId});" })%>
							</div>
						</div>
						{{if ContactInformations.length>0}} {{each(i,contact) ContactInformations}} {{tmpl(getItem(i,contact))
						"#ContactInfoTemplate"}} {{/each}} {{else}} {{tmpl(getNewContact(InvestorId)) "#ContactInfoTemplate"}}
						{{/if}}
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
						{{if AccountInformations.length>0}} {{each(i,account) AccountInformations}} {{tmpl(getItem(i,account))
						"#BankInfoTemplate"}} {{/each}} {{else}} {{tmpl(getNewBank(InvestorId)) "#BankInfoTemplate"}}
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

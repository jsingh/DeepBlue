﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.EditModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @onsubmit = "return false" })) {%>
<div id="addressInfo" class="editinfo">
	<div class="editor-row">
		<div class="editor-editbtn">
			<div class="EditInvestorInfo" style="float: left">
				<%: Html.Anchor(Html.Image("Editbtn.png", new { @title = "Edit" }).ToHtmlString(), "#", new { @class="show", @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
			</div>
			<div class="UpdateInvestorInfo" style="float: left;">
				<%: Html.Span("", new { @id = "Loading" })%>
				<%: Html.Image("Update.png", new { @style="cursor:pointer", @onclick = "javascript:editInvestor.saveInvestorAddressDetail(this);",  @class = "hide" })%>
				<%: Html.Anchor(Html.Image("Cancel.png").ToHtmlString(), "#", new { @class = "hide", @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
			</div>
		</div>
	</div>
	<%: Html.Hidden("AddressId", "${AddressId}")%>
	<%: Html.Hidden("InvestorId", "${InvestorId}")%>
	<div class="editor-label">
		<%: Html.Label("Phone") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Phone", "${getCommunicationValue(InvestorCommunications," + ((int)DeepBlue.Models.Admin.Enums.CommunicationType.HomePhone).ToString() +")}", new { @class = "comvalue hide" })%>
		<%: Html.Span("${Phone}", new { @class = "show", @id = "Phone" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Fax") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Fax", "${getCommunicationValue(InvestorCommunications," + ((int)DeepBlue.Models.Admin.Enums.CommunicationType.Fax).ToString() + ")}", new { @class = "comvalue hide" })%>
		<%: Html.Span("${Fax}", new { @class = "show", @id = "Fax" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Email") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Email", "${getCommunicationValue(InvestorCommunications," + ((int)DeepBlue.Models.Admin.Enums.CommunicationType.Email).ToString() + ")}", new { @class = "comvalue hide", @onblur = "javascript:jHelper.checkEmail(this);" })%>
		<%: Html.Span("${Email}", new { @class = "show", @id = "Email" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Web Address") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("WebAddress", "${getCommunicationValue(InvestorCommunications," + ((int)DeepBlue.Models.Admin.Enums.CommunicationType.WebAddress).ToString() + ")}", new { @class = "comvalue hide", @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
		<%: Html.Span("${WebAddress}", new { @class = "show", @id = "WebAddress" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Address1") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Address1", "${Address1}", new { @class = "hide" })%>
		<%: Html.Span("${Address1}", new { @class = "show", @id = "Address1" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Address2") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Address2", "${Address2}", new { @class = "hide" })%>
		<%: Html.Span("${Address2}", new { @class = "show", @id = "Address2" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("City") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("City", "${City}", new { @class = "hide" })%>
		<%: Html.Span("${City}", new { @class = "show", @id = "City" })%>
	</div>
	<div class="editor-row" id="AddressStateRow" {{if Country!=225}}style="display:none"{{/if}}>
		<div class="editor-label">
			<%: Html.Label("State") %>
		</div>
		<div class="editor-field dropdown">
			<%: Html.TextBox("StateName", "${StateName}", new { @id = "StateName", @class = "hide" })%>
			<%: Html.Hidden("State","${State}")%>
			<%: Html.Span("${StateName}", new { @class = "show", @id = "State" })%>
		</div>
	</div>
	<div class="editor-label">
		<%: Html.Label("Zip") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Zip", "${Zip}", new { @class = "hide" })%>
		<%: Html.Span("${Zip}", new { @class = "show", @id = "Disp_" + "Zip" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Country")%>
	</div>
	<div class="editor-field dropdown">
		<%: Html.TextBox("CountryName", "${CountryName}", new { @id = "CountryName", @class = "hide"})%>
		<%: Html.Hidden("Country", "${Country}")%>
		<%: Html.Span("${CountryName}", new { @class = "show", @id = "Country" })%>
	</div>
</div>
<%}%>

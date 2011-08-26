<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.EditModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @onsubmit = "return false" })) {%>
<div id="addressInfo" class="editinfo">
	<div class="editor-row">
		<div class="editor-editbtn">
			<div class="EditInvestorInfo" style="float: left">
				<%: Html.Anchor(Html.Image("Editbtn.png", new { @title = "Edit" }).ToHtmlString(), "#", new { @class="show", @onclick = "javascript:editInvestor.editInvestorInfo(this);" })%>
			</div>
			<div class="UpdateInvestorInfo" style="float: left;">
				<%: Html.Image("Update.png", new { @style="cursor:pointer", @onclick = "javascript:editInvestor.saveInvestorAddressDetail(this);",  @class = "hide" })%>
				<%: Html.Anchor(Html.Image("Cancel.png").ToHtmlString(), "#", new { @class = "hide", @onclick = "javascript:editInvestor.cancelInvestorInfo(this);" })%>
			</div>
		</div>
	</div>
	<%: Html.Hidden("AddressId", "${AddressId}")%>
	<%: Html.Hidden("InvestorId", "${InvestorId}")%>
	<%: Html.Hidden("InvestorCommunicationId", "${InvestorCommunicationId}")%>
	<div class="editor-label">
		<%: Html.Label("Phone") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Phone", "${Phone}", new { @class = "hide" })%>
		<%: Html.Span("${Phone}", new { @class = "show", @id = "Disp_" +  "Phone" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Fax") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Fax", "${Fax}", new { @class = "hide" })%>
		<%: Html.Span("${Fax}", new { @class = "show", @id = "Disp_" +  "Fax" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Email") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox( "Email", "${Email}", new { @class = "hide", @onblur = "javascript:jHelper.checkEmail(this);" })%>
		<%: Html.Span("${Email}", new { @class = "show", @id = "Disp_" +  "Email" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Web Address") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("WebAddress", "${WebAddress}", new { @class = "hide", @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
		<%: Html.Span("${WebAddress}", new { @class = "show", @id = "Disp_" +  "WebAddress" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Address1") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Address1", "${Address1}", new { @class = "hide" })%>
		<%: Html.Span("${Address1}", new { @class = "show", @id = "Disp_" +  "Address1" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Address2") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Address2", "${Address2}", new { @class = "hide" })%>
		<%: Html.Span("${Address2}", new { @class = "show", @id = "Disp_" +  "Address2" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("City") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("City", "${City}", new { @class = "hide" })%>
		<%: Html.Span("${City}", new { @class = "show", @id = "Disp_" +  "City" })%>
	</div>
	<div class="editor-row" id="${i}_AddressStateRow">
		<div class="editor-label">
			<%: Html.Label("State") %>
		</div>
		<div class="editor-field dropdown">
			<%: Html.TextBox("StateName", "", new { @id = "StateName", @class = "stateac hide", @hiddenid = "State" })%>
			<%: Html.Hidden("State","${State}")%>
			<%: Html.Span("${StateName}", new { @class = "show", @id = "Disp_" +  "State" })%>
		</div>
	</div>
	<div class="editor-label">
		<%: Html.Label("Zip") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("PostalCode", "${PostalCode}", new { @class = "hide" })%>
		<%: Html.Span("${PostalCode}", new { @class = "show", @id = "Disp_" +  "PostalCode" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Country")%>
	</div>
	<div class="editor-field dropdown">
		<%: Html.TextBox("CountryName", "", new { @id = "CountryName", @class = "countryac hide", @hiddenid="Country", @staterowid = "${i}_AddressStateRow" })%>
		<%: Html.Hidden("Country", "${Country}")%>
		<%: Html.Span("${Country}", new { @class = "show", @id = "Disp_" +  "Country" })%>
	</div>
</div>
<%}%>

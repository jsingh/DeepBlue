<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.ContactDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="contactinfo-box" id="ContactInfo${i}">
	<div class="line">
	</div>
	<div class="info-detail" style="padding-left: 74px">
		<%: Html.Hidden("${i}_ContactIndex","${i}")%>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.ContactPerson) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "ContactPerson", "${ContactPerson}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.Designation) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "Designation", "${Designation}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ContactPhoneNumber) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "ContactPhoneNumber", "${ContactPhoneNumber}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ContactFaxNumber) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "ContactFaxNumber", "${ContactFaxNumber}")%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.ContactEmail) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "ContactEmail", "${ContactEmail}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ContactWebAddress) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "ContactWebAddress", "${ContactWebAddress}", new { @webaddress = "true", @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ContactAddress1) %>
		</div>
		<div class="editor-field" style="width: 416px;">
			<%: Html.TextBox("${i}_" + "ContactAddress1", "${ContactAddress1}", new { @style = "width:408px" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.ContactCity) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "ContactCity", "${ContactCity}")%>
		</div>
		<div class="editor-row" id="StateRow" style="clear: right; float: left; width: auto">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactState) %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("${i}_" + "ContactStateName", "${ContactStateName}", new { @id = "ContactStateName" })%>
				<%: Html.Hidden("${i}_" + "ContactState", "${ContactState}", new { @id = "ContactState" })%>
			</div>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ContactZip) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "ContactZip", "${ContactZip}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ContactCountry) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("${i}_" + "ContactCountryName", "${ContactCountryName}", new { @id = "ContactCountryName" })%>
			<%: Html.Hidden("${i}_" + "ContactCountry", "${ContactCountry}", new { @id = "ContactCountry" })%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.DistributionNotices) %>
		</div>
		<div class="editor-field">
			<%: Html.CheckBox("${i}_" + "DistributionNotices",new {style = "width:auto"})%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.Financials)%>
		</div>
		<div class="editor-field">
			<%: Html.CheckBox("${i}_" + "Financials", new { style = "width:auto" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.K1)%>
		</div>
		<div class="editor-field">
			<%: Html.CheckBox("${i}_" + "K1", new { style = "width:auto" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.InvestorLetters)%>
		</div>
		<div class="editor-field">
			<%: Html.CheckBox("${i}_" + "InvestorLetters", new { style = "width:auto" })%>
		</div>
		<div class="editor-row">
			<div class="editor-editbtn" style="padding-right: 115px;">
				<div class="EditInvestorInfo" style="float: left">
					<%=Html.Image("delete_active.png", new { @title = "Delete Contact", @onclick = "javascript:investor.deleteContact(this);" })%>
				</div>
			</div>
		</div>
	</div>
</div>

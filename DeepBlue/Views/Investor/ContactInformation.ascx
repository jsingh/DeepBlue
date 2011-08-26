<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.ContactDetail>" %>
<div class="contactinfo-box">
	<%: Html.Hidden("${i}_ContactIndex","${i}")%>
	<div class="contactinfo-left">
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactPerson) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactPerson", "${ContactPerson}")%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Designation) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "Designation", "${Designation}")%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactPhoneNumber) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactPhoneNumber", "${ContactPhoneNumber}")%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactFaxNumber) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactFaxNumber", "${ContactFaxNumber}")%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactEmail) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactEmail", "${ContactEmail}")%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactWebAddress) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactWebAddress", "${ContactWebAddress}", new { @onblur = "javascript:jHelper.checkWebAddress(this);" })%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactAddress1) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactAddress1", "${ContactAddress1}")%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactAddress2) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactAddress2", "${ContactAddress2}")%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactCity) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactCity", "${ContactCity}")%>
			</div>
		</div>
		<div class="editor-row" id="${i}_StateRow" {{if ContactCountry!=225}}style="display:none"{{/if}}>
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactState) %>
			</div>
			<div class="editor-field dropdown">
				<%: Html.TextBox("${i}_" + "ContactStateName", "${ContactStateName}", new { @id = "${i}_" + "ContactStateName" })%>
				<%: Html.Hidden("${i}_" + "ContactState", "${ContactState}", new { @id = "${i}_" + "ContactState" })%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactZip) %>
			</div>
			<div class="editor-field text">
				<%: Html.TextBox("${i}_" + "ContactZip", "${ContactZip}")%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.ContactCountry) %>
			</div>
			<div class="editor-field dropdown">
				<%: Html.TextBox("${i}_" + "ContactCountryName", "${ContactCountryName}", new { @id = "${i}_" + "ContactCountryName" })%>
				<%: Html.Hidden("${i}_" + "ContactCountry", "${ContactCountry}", new { @id = "${i}_" + "ContactCountry" })%>
			</div>
		</div>
	</div>
	<div class="contactinfo-right">
		<div class="editor-row">
			<div class="editor-label" style="width: 255px;">
				<%: Html.LabelFor(model => model.DistributionNotices) %>
			</div>
			<div class="editor-field checkbox">
				<%: Html.CheckBox("${i}_" + "DistributionNotices",new {style = "width:auto"})%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label" style="width: 255px;">
				<%: Html.LabelFor(model => model.Financials)%>
			</div>
			<div class="editor-field checkbox">
				<%: Html.CheckBox("${i}_" + "Financials", new { style = "width:auto" })%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label" style="width: 255px;">
				<%: Html.LabelFor(model => model.K1)%>
			</div>
			<div class="editor-field checkbox">
				<%: Html.CheckBox("${i}_" + "K1", new { style = "width:auto" })%>
			</div>
		</div>
		<div class="editor-row">
			<div class="editor-label" style="width: 255px;">
				<%: Html.LabelFor(model => model.InvestorLetters)%>
			</div>
			<div class="editor-field checkbox">
				<%: Html.CheckBox("${i}_" + "InvestorLetters", new { style = "width:auto" })%>
			</div>
		</div>
	</div>
</div>

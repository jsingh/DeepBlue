<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Investor.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="TitleCnt" ContentPlaceHolderID="TitleContent" runat="server">
	Investor
</asp:Content>
<asp:Content ID="HeaderCnt" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.scrollTo-min.js")%>
	<%=Html.JavascriptInclueTag("Investor.js")%>
	<%=Html.JavascriptInclueTag("InvestorBankInfo.js")%>
	<%=Html.JavascriptInclueTag("InvestorContactInfo.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("newinvestor.css")%>
	<%=Html.JavascriptInclueTag("jquery.fileuploader.js")%>
	<%=Html.JavascriptInclueTag("ImportInvestorExcel.js")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTORS</span><span class="arrow"></span><span class="pname">New Investor Setup</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="MainCnt" ContentPlaceHolderID="MainContent" runat="server">
	<%using (Html.Form(new { @id = "NewInvestor", @onsubmit = "return false;" })) { %>
	<%: Html.HiddenFor(model => model.InvestorId)%>
	<%: Html.HiddenFor(model => model.ContactLength)%>
	<%: Html.HiddenFor(model => model.AccountLength)%>
	<div id="InvestorContainer">
	</div>
	<div style="margin-top: 50px; clear: both;">
		<div style="float: right; padding-right: 93px;">
			<div style="float: left; font-weight: bold; margin-right: 20px;">
				<%: Html.Span("", new { id = "UpdateLoading" })%>
			</div>
			<div style="float: left; margin-right: 20px;">
				<%: Html.ImageButton("Import-Excel_active.png", new { @id = "btnImportInvestor", onclick = "javascript:$('#ExcelImport').dialog('open');" })%>
			</div>
			<div style="float: left;">
				<%: Html.Image("addinvestor_active.png", new { @class = "default-button", @onclick = "javascript:investor.save($('#NewInvestor')); " })%>
			</div>
			<div class="clear">
				&nbsp;</div>
		</div>
	</div>
	<% } %>
	<div id="ExcelImport">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("StateOfResidencyName", new AutoCompleteOptions {
																	  Source = "/Admin/FindStates", MinLength = 1,
																	  OnSelect = "function(event, ui) { $('#StateOfResidency').val(ui.item.id); }"
	})%>
	<%= Html.jQueryAutoComplete("StateName", new AutoCompleteOptions {
																	  Source = "/Admin/FindStates", MinLength = 1,
																	  OnSelect = "function(event, ui) { $('#State','#AddressInfoMain').val(ui.item.id); }"
	})%>
	<%= Html.jQueryAutoComplete("CountryName", new AutoCompleteOptions {
																	  Source = "/Admin/FindCountrys", MinLength = 1,
																	  OnSelect = "function(event, ui) { investor.changeCountry(ui.item); }"
	})%>
	<script type="text/javascript">
		$(document).ready(function () {
			/*var bdy=$("body");
			jHelper.jqCheckBox(bdy);
			jHelper.jqComboBox(bdy);
			investor.createAccount();
			investor.createContact();
			*/
			investor.init();
		});
	</script>
	<script id="InvestorInformationTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("InvestorInformation", Model);%>
	</script>
	<script id="AddressInformationTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("AddressInformation", Model);%>
	</script>
	<script id="AddressInfoEditTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("AddressInformationEdit");%>
	</script>
	<script id="BankInformationTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("BankInformation");%>
	</script>
	<script id="BankInfoEditTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("NewBankInformation", new DeepBlue.Models.Investor.BankDetail());%>
	</script>
	<script id="ContactInformationTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("ContactInformation");%>
	</script>
	<script id="ContactInfoEditTemplate" type="text/x-jquery-tmpl"> 
		<%DeepBlue.Models.Investor.ContactDetail contactDetail = new DeepBlue.Models.Investor.ContactDetail();
		Html.RenderPartial("NewContactInformation", contactDetail);
		%>
	</script>
	<script type="text/javascript">
		investor.newContactDetail  = <%= JsonSerializer.ToJsonObject(contactDetail)%>;
	</script>
	<%using (Html.jQueryTemplateScript("ExcelImprtTemplate")) {%>
	<div class="import-box">
		<%using (Html.Form(new { @id = "frmUploadExcel", @onsubmit = "return false" })) { %>
		<div class="editor-label" style="width: 110px;">
			&nbsp;</div>
		<div class="editor-field" style="text-align: right; font-size: 11px;">
			<%:Html.Anchor("Sample Excel","/Files/ImportSamples/Investor.xls", new { @target = "_blank", @style = "color:blue" })%>
		</div>
		<div class="editor-label" style="width: 110px;">
			<%: Html.Label("File")%></div>
		<div class="editor-field">
			<%: Html.File("UploadFile", new { @id = "UploadFile" })%>
		</div>
		<div class="editor-label" style="width: 100px">
			<%: Html.Span("", new { @id = "SpnUELoading" })%>
		</div>
		<div class="editor-label" style="clear: right; width: auto;">
			<%: Html.Image("Upload_active.png", new { @onclick = "javascript:importInvestorExcel.uploadExcel();" })%></div>
		<div class="editor-field">
			<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:$('#ExcelImport').dialog('close');" })%></div>
		<%}%>
	</div>
	<div id="ImportExcel">
	</div>
	<div id="ProgressBar">
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ImportExcelTemplate")) {%>
	<br />
	<div class="tabbg">
		<%using (Html.Tab(new { @id = "InvestorDetailTab", @class = "section-tab-sel", @onclick = "javascript:importInvestorExcel.selectTab('INV',this);" })) {%>Investor<%}%>
		<%using (Html.Tab(new { @id = "InvestorBankTab", @class = "section-tab", @onclick = "javascript:importInvestorExcel.selectTab('IVBANK',this);" })) {%>Investor Bank Information<%}%>
		<%using (Html.Tab(new { @id = "InvestorContactTab", @class = "section-tab", @onclick = "javascript:importInvestorExcel.selectTab('IVCONTACT',this);" })) {%>Investor Contact Information<%}%>
	</div>
	<div class="clear">
		&nbsp;</div>
	<div class="investorimportsection" id="InvestorDetailBox">
		<div class="formbox">
			<%using (Html.Form(new { @id = "frm", @onsubmit = "return false" })) { %>
			<div class="editor-label">
				<%: Html.Label("Excel Tab")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("InvestorTableName", new List<SelectListItem>() {
	 new  SelectListItem { Text = "--Select Excel Tab--", Value = " " }
}, new { @exceltabname = "InvestorDetail", @class = "ddltable", @onchange = "javascript:importInvestorExcel.selectExcelTab(this);" })%></div>
			<div class="editor-label">
				<%: Html.Label("InvestorName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("InvestorName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("DisplayName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DisplayName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("SocialSecurityID")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("SocialSecurityID", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("DomesticForeign")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DomesticForeign", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("StateOfResidency")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("StateOfResidency", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("EntityType")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("EntityType", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Source")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Source", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("FOIA")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FOIA", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("ERISA")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ERISA", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Notes")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Notes", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Phone")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Phone", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Fax")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Fax", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Email")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Email", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("WebAddress")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("WebAddress", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Address1")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Address1", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Address2")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Address2", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("City")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("City", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("State")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("State", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Zip")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Zip", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Country")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Country", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="clear">
				&nbsp;</div>
			<%: Html.Hidden("TotalRows", "${TotalRows}")%>
			<%: Html.Hidden("SessionKey", "${SessionKey}")%>
			<%}%>
		</div>
		<div class="statusbox">
		</div>
	</div>
	<div class="investorimportsection" id="InvestorBankBox" style="display: none">
		<div class="formbox">
			<%using (Html.Form(new { @id = "frm", @onsubmit = "return false" })) { %>
			<div class="editor-label">
				<%: Html.Label("Excel Tab")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("InvestorBankTableName", new List<SelectListItem>() {
	 new  SelectListItem { Text = "--Select Excel Tab--", Value = " " }
}, new { @exceltabname = "BankInformation", @class = "ddltable", @onchange = "javascript:importInvestorExcel.selectExcelTab(this);" })%></div>
			<div class="editor-label">
				<%: Html.Label("InvestorName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("InvestorName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("BankName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("BankName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("ABANumber")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ABANumber", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("AccountName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("AccountName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("AccountNumber")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("AccountNumber", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("FFCName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FFCName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("FFCNumber")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FFCNumber", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Reference")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Reference", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Swift")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Swift", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("IBAN")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("IBAN", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Phone")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Phone", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Fax")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Fax", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="clear">
				&nbsp;</div>
			<%: Html.Hidden("TotalRows", "${TotalRows}")%>
			<%: Html.Hidden("SessionKey", "${SessionKey}")%>
			<%}%>
		</div>
		<div class="statusbox">
		</div>
	</div>
	<div class="investorimportsection" id="InvestorContactBox" style="display: none">
		<div class="formbox">
			<%using (Html.Form(new { @id = "frm", @onsubmit = "return false" })) { %>
			<div class="editor-label">
				<%: Html.Label("Excel Tab")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("InvestorContactTableName", new List<SelectListItem>() {
	 new  SelectListItem { Text = "--Select Excel Tab--", Value = " " }
}, new { @exceltabname = "ContactInformation", @class = "ddltable", @onchange = "javascript:importInvestorExcel.selectExcelTab(this);" })%></div>
			<div class="editor-label">
				<%: Html.Label("InvestorName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("InvestorName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("ContactPerson")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ContactPerson", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Designation")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Designation", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Telephone")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Telephone", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Fax")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Fax", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Email")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Email", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("WebAddress")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("WebAddress", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Address")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Address", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("City")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("City", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("State")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("State", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Zip")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Zip", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Country")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Country", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("ReceivesDistributionCapitalCallNotices")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ReceivesDistributionCapitalCallNotices", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Financials")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Financials", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("InvestorLetters")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("InvestorLetters", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="clear">
				&nbsp;</div>
			<%: Html.Hidden("TotalRows", "${TotalRows}")%>
			<%: Html.Hidden("SessionKey", "${SessionKey}")%>
			<%}%>
		</div>
		<div class="statusbox">
		</div>
	</div>
	<div class="save-box">
		<div class="editor-label">
			<%: Html.Image("Save_active.png", new {  @onclick = "javascript:importInvestorExcel.import(this);" })%></div>
		<div class="editor-field">
			<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:$('#ExcelImport').dialog('close');" })%></div>
		<div class="clear">
			&nbsp;</div>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ImportExcelResultTemplate")) {%>
	<div class="editor-label">
		<%: Html.Label("Total")%></div>
	<div class="editor-field">
		<%: Html.Span("${TotalRows}", new { @id = "spntotal" })%></div>
	<div class="editor-label">
		<%: Html.Label("Success")%></div>
	<div class="editor-field">
		<%: Html.Span("${SuccessRows}", new { @id = "spnsuccess" })%></div>
	<div class="editor-label">
		<%: Html.Label("Errors")%></div>
	<div class="editor-field">
		<%: Html.Span("${ErrorRows}", new { @id = "spnerrors" })%></div>
	<div class="editor-field">
		<%: Html.Span("", new { @id = "spnerrorexcel", @style="color:red" })%></div>
	<div class='prs-bar'>
		<div class='total-rows'>
			Rows ${CompletedRows} Of ${TotalRows}</div>
		<div class="status-bar">
			<div class='loading-status' style='width: ${Percent}%;'>
			</div>
		</div>
	</div>
	<%}%>
</asp:Content>

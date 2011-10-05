<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Investor.EditModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="TitleCnt" ContentPlaceHolderID="TitleContent" runat="server">
	Investor
</asp:Content>
<asp:Content ID="HeaderCnt" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.scrollTo-min.js")%>
	<%=Html.JavascriptInclueTag("EditInvestor.js")%>
	<%=Html.JavascriptInclueTag("InvestorBankInfo.js")%>
	<%=Html.JavascriptInclueTag("InvestorContactInfo.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("newinvestor.css")%>
	<%=Html.StylesheetLinkTag("editinvestor.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTORS</span><span class="arrow"></span><span class="pname">Update
					Investor Information</span></div>
			<div class="rightcol">
				<div style="margin: 0; padding: 0 0 0 0; float: left;">
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none;" })%>
				</div>
				<div style="float: left">
					<%: Html.TextBox("Investor", "SEARCH INVESTOR", new { @id = "Investor", @class = "wm", @style = "width:200px" })%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="MainCnt" ContentPlaceHolderID="MainContent" runat="server">
	<%using (Html.Form(new { @id = "EditInvestor", @onsubmit = "return false;" })) { %>
	<%: Html.HiddenFor(model => model.InvestorId)%>
	<div id="InvestorContainer">
	</div>
	<% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,OnSelect = "function(event, ui){ editInvestor.selectInvestor(ui.item.id);}"})%>
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
																	  OnSelect = "function(event, ui) { editInvestor.changeCountry(ui.item); }"
	})%>
	<script type="text/javascript">
		$(document).ready(function () {
			editInvestor.init();
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
		<%Html.RenderPartial("BankInformationEdit", new DeepBlue.Models.Investor.BankDetail());%>
	</script>
	<script id="ContactInformationTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("ContactInformation");%>
		<div style="margin-top: 50px; clear: both;">
		<div class="editor-button" style="float: right; padding-right: 198px;">
			<%: Html.Image("updateinvestor_active.png", new { @class = "default-button", @onclick = "javascript:editInvestor.saveInvestorDetail();" })%>
		</div>
		<div class="editor-label" style="float: right">
			<%: Html.Span("", new { id = "UpdateLoading" })%>
		</div>
	</div>
	</script>
	<script id="ContactInfoEditTemplate" type="text/x-jquery-tmpl"> 
		<%DeepBlue.Models.Investor.ContactDetail contactDetail = new DeepBlue.Models.Investor.ContactDetail();
		Html.RenderPartial("ContactInformationEdit", contactDetail);
		%>
	</script>
</asp:Content>

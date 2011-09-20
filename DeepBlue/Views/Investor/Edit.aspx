<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Investor.EditModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Investor
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.scrollTo-1.4.2-min.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.JavascriptInclueTag("EditInvestor.js") %>
	<%=Html.StylesheetLinkTag("editinvestor.css") %>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTORS</span><span class="arrow"></span><span class="pname">Update
					Investor Information</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="edit-investor">
		<div class="editor-label" style="width: auto;padding-left:0px;">
			<%: Html.Label("Investor") %>
		</div>
		<div class="editor-field" style="width: auto">
			<%: Html.TextBox("Investor", "SEARCH INVESTOR", new { @class = "wm", @style = "width:200px" })%>
		</div>
		<div class="editor-field">
			<%=Html.Span("",new { id = "Loading" })%>
		</div>
		<div id="editinfo">
		</div>
	</div>
	<%: Html.HiddenFor(model => model.id) %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,OnSelect = "function(event, ui){ editInvestor.selectInvestor(ui.item.id);}"})%>
	<script type="text/javascript">
		editInvestor.init();
	</script>
	<script id="EditInvestorTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("EditInvestorDetail", Model);%>
	</script>
	<script id="DisplayInvestorInfoTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("DisplayInvestorInformation");%>
	</script>
	<script id="InvestorInfoEditTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("InvestorInformation", Model);%>
	</script>
	<script id="AddressInfoTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("AddressInformation", Model);%>
	</script>
	<script id="ContactInfoTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("ContactInformation", new DeepBlue.Models.Investor.ContactDetail());%>
	</script>
	<script id="BankInfoTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("BankInformation", new DeepBlue.Models.Investor.BankDetail());%>
	</script>
	<script id="CFValueTemplate" type="text/x-jquery-tmpl"> 
	</script>
</asp:Content>

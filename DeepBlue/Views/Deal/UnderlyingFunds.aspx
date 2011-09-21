<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateUnderlyingFundModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Underlying Fund Library
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.StylesheetLinkTag("deal.css") %>
	<%=Html.StylesheetLinkTag("dealdirect.css")%>
	<%=Html.StylesheetLinkTag("addufund.css")%>
	<%=Html.JavascriptInclueTag("UnderlyingFund.js")%>
	<%=Html.JavascriptInclueTag("DealDirect.js")%>
	<%=Html.JavascriptInclueTag("jquery.fileuploader.js")%>
	<%=Html.JavascriptInclueTag("flexgrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<div class="title">
					INVESTMENTS</div>
				<div class="arrow">
				</div>
				<div class="pname">
					UNDERLYING FUND LIBRARY
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%></div>
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
			<div class="leftcol">
				Underlying Fund
			</div>
			<div class="addbtn" style="display: block;margin-left:67px;">
				<%using (Html.GreenButton(new { @id = "AddGP", @onclick = "javascript:dealDirect.add();" })) {%>Add
				GP<%}%>
			</div>
			<div class="addbtn" style="display: block;margin-left:123px;">
				<%using (Html.GreenButton(new { @id = "lnkAddUnderlyingFund", @onclick = "javascript:underlyingFund.load(0,0);" })) {%>Add
				new underlying fund<%}%>
			</div>
			<div class="rightcol">
				<%: Html.TextBox("S_UnderlyingFund", "SEARCH UNDERLYING FUND", new { @id = "S_UnderlyingFund", @style = "width:200px", @class = "wm" })%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="UnderlyingFundDetail">
		<div id="AddNewIssuer" style="display: none">
			<%using (Html.Form(new { @id = "frmAddNewIssuer", @onsubmit = "return dealDirect.createNewIssuer(this);" })) {%>
			<div id="NewIssuerDetail">
			</div>
			<div class="addissuer" style="width:980px;">
				<div class="btnclose">
					<%: Html.Image("issuerclose.png", new { @onclick = "javascript:dealDirect.close();" })%>
				</div>
				<div class="btn">
					<%: Html.ImageButton("savegp.png")%></div>
				<div class="btn">
					<%: Html.Span("", new { @id = "SpnNewLoading" })%></div>
			</div>
			<%}%>
		</div>
		<%using (Html.Form(new { @id = "frmUnderlyingFund", @onsubmit = "return underlyingFund.save(this);" })) {%>
		<div id="AddUnderlyingFund" style="display: none">
		</div>
		<%}%>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		underlyingFund.init();</script>
	<%= Html.jQueryAutoComplete("S_UnderlyingFund", new AutoCompleteOptions {
																		  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																		  OnSelect = "function(event, ui) { underlyingFund.load(ui.item.id,0);}"
	})%>
	<%= Html.jQueryAutoComplete("Issuer", new AutoCompleteOptions {
	Source = "/Deal/FindIssuers",
	MinLength = 1,
																	  OnSelect = "function(event, ui) { underlyingFund.selectIssuer(ui.item.id);}"
	})%>
	<script id="IssuerDetailTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("IssuerDetail", new DeepBlue.Models.Deal.IssuerDetailModel());%>
	</script>
	<script id="UnderlyingFundTemplate" type="text/x-jquery-tmpl">
	<%Html.RenderPartial("UnderlyingFundDetail", Model);%>
	</script>
</asp:Content>

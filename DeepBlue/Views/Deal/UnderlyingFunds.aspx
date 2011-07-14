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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
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
		<div class="leftcol">
			Underlying Fund
		</div>
		<div class="addbtn" style="display: block">
			<%: Html.Anchor(Html.Image("addgp.png").ToHtmlString(), "javascript:dealDirect.add();")%>
		</div>
		<div class="addbtn" style="display: block">
			<%: Html.Anchor(Html.Image("addnufund.png").ToHtmlString(), "javascript:underlyingFund.load(0,0);")%>
		</div>
		<div style="float: right; padding-right: 50px;">
			<%: Html.TextBox("S_UnderlyingFund", "SEARCH UNDERLYING FUND", new { @id = "S_UnderlyingFund", @style = "width:200px", @class = "wm" })%>
		</div>
	</div>
	<div id="UnderlyingFundDetail">
		<div class="subheader" id="AddNewIssuer" style="display: none">
			<%using (Html.Form(new { @id = "frmAddNewIssuer", @onsubmit = "return dealDirect.createNewIssuer(this);" })) {%>
			<div id="NewIssuerDetail">
			</div>
			<div class="addissuer">
				<div class="btn">
					<%: Html.Span("", new { @id = "SpnNewLoading" })%></div>
				<div class="btn">
					<%: Html.ImageButton("addgp.png")%></div>
				<div class="btnclose">
					<%: Html.Image("issuerclose.png", new { @onclick = "javascript:dealDirect.close();" })%>
				</div>
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

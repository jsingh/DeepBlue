<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateIssuerModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Directs
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealdirect.css")%>
	<%=Html.JavascriptInclueTag("DealDirect.js")%>
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
					DIRECT LIBRARY</div>
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="leftcol">
			Underlying Direct</div>
		<div class="addbtn" style="display: block">
			<%: Html.Anchor(Html.Image("addcompany.png").ToHtmlString(), "javascript:dealDirect.add();")%>
		</div>
		<div style="display: block; float: right; margin-right: 15%;">
			<%: Html.Span("", new { @id = "SpnIssuerLoading" })%>
			<%: Html.TextBox("S_Issuer", "SEARCH ISSUER", new { @class = "wm", @style = "width:150px", @id = "S_Issuer" })%>
		</div>
	</div>
	<div id="DirectDetailBox">
		<div class="subheader" id="AddNewIssuer" style="display: none">
			<%using (Html.Form(new { @id = "frmAddNewIssuer", @onsubmit = "return dealDirect.createNewIssuer(this);" })) {%>
			<div id="NewIssuerDetail">
			</div>
			<div class="addissuer">
				<div class="btn">
					<%: Html.Span("", new { @id = "SpnNewLoading" })%></div>
				<div class="btn">
					<%: Html.ImageButton("addcompany.png")%></div>
				<div class="btnclose">
					<%: Html.Image("issuerclose.png", new { @onclick = "javascript:dealDirect.close();" })%>
				</div>
			</div>
			<%}%>
		</div>
		<div id="DirectMain" style="display: none">
			<%using (Html.Form(new { @id = "frmIssuer", @onsubmit = "return dealDirect.save(this);" })) {%>
			<div id="IssuerDetail">
			</div>
			<div class="editor-label">
				<%: Html.Label("Security Type")%>
			</div>
			<div class="editor-field" style="width: auto;">
				<div id="equitytab" class="sel" onclick="javascript:dealDirect.selectTab('E',this);">
					&nbsp;
				</div>
				<div id="fitab" onclick="javascript:dealDirect.selectTab('F',this);">
					&nbsp;
				</div>
			</div>
			<div class="subdetail">
				<div class="line">
				</div>
				<div id="EQdetail">
				</div>
				<div id="FixedIncome" style="display: none">
				</div>
				<div class="line">
				</div>
			</div>
			<div class="direct">
				<%: Html.Span("", new { @id = "SpnSaveIssuerLoading" } )%>&nbsp;&nbsp;&nbsp;
				<%: Html.ImageButton("add_direct.png")%>
			</div>
			<%}%>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		dealDirect.init();</script>
	<%= Html.jQueryAutoComplete("S_Issuer", new AutoCompleteOptions {
																	  Source = "/Issuer/FindIssuers", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealDirect.load(ui.item.id);}"
	})%>
	<script id="IssuerDetailTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("IssuerDetail", Model.IssuerDetailModel);%>
	</script>
	<script id="EquityDetailTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("DirectEquityDetail", Model.EquityDetailModel);%>
	</script>
	<script id="FixedIncomeDetailTemplate" type="text/x-jquery-tmpl">
		<%Html.RenderPartial("FixedIncomeDetail", Model.FixedIncomeDetailModel);%>
	</script>
</asp:Content>

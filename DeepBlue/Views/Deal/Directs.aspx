﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateIssuerModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Directs
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealdirect.css")%>
	<%=Html.JavascriptInclueTag("DealDirect.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.JavascriptInclueTag("jquery.fileuploader.js")%>
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
					DIRECT LIBRARY</div>
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
		<div class="leftcol">
			Direct</div>
		<div class="addbtn" style="display: block">
			<%using (Html.GreenButton(new { @id = "AddCompany", @onclick = "javascript:dealDirect.add();" })) {%>Add
			Company<%}%>
		</div>
		<div class="rightcol" style="display: block;">
			<%: Html.Span("", new { @id = "SpnIssuerLoading" })%>
			<%: Html.TextBox("S_Issuer", "SEARCH ISSUER", new { @class = "wm", @style = "width:200px", @id = "S_Issuer" })%>
		</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="DirectDetailBox">
		<div id="AddNewIssuer" style="display: none">
			<%using (Html.Form(new { @id = "frmAddNewIssuer", @onsubmit = "return dealDirect.createNewIssuer(this);" })) {%>
			<div id="NewIssuerDetail">
			</div>
			<div class="addissuer">
				<div class="btnclose">
					<%: Html.Image("issuerclose.png", new { @onclick = "javascript:dealDirect.close();" })%>
				</div>
				<div class="btn">
					<%: Html.ImageButton("addcompany.png")%></div>
				<div class="btn">
					<%: Html.Span("", new { @id = "SpnNewLoading" })%></div>
			</div>
			<%}%>
		</div>
		<div id="DirectMain" style="display: none">
			<%using (Html.Form(new { @id = "frmIssuer", @onsubmit = "return dealDirect.save(this);" })) {%>
			<div id="IssuerDetail">
			</div>
			<div class="editor-label" style="width:128px;padding-right:26px;">
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
				<div id="EQdetail">
				</div>
				<div id="FixedIncome" style="display: none">
				</div>
			</div>
			<div class="btnbox">
				<div class="direct">
					<%: Html.Span("", new { @id = "SpnSaveIssuerLoading" } )%>&nbsp;&nbsp;&nbsp;
					<%: Html.ImageButton("add_direct_active.png")%>
				</div>
			</div>
			<%}%>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		dealDirect.init();</script>
	<%= Html.jQueryAutoComplete("S_Issuer", new AutoCompleteOptions {
	Source = "/Deal/FindIssuers",
	MinLength = 1,
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

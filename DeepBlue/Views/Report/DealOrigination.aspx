<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Report.DealOriginationModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal Origination
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("DealOriginationReport.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.StylesheetLinkTag("report.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">REPORTS</span><span class="arrow"></span><span class="pname">Deal
					Origination</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div id="ReportHeader">
		<div class="titlebox">
			<% Html.EnableClientValidation(); %>
			<%using (Html.BeginForm("", "", FormMethod.Get, new { @onsubmit = "return dealOrgReport.onSubmit(this);" })) {%>
			<div class="editor-label" style="width: auto;">
				<div style="float:left"><%: Html.LabelFor(model => model.FundId)%></div>
				<div style="float:left"><%: Html.TextBox("FundName", "SEARCH  FUND", new { @class = "wm", @id = "FundName", @style = "width:200px" })%></div>
			</div>
			<div class="editor-label" style="width: auto; clear: right;">
				<div style="float:left"><%: Html.LabelFor(model => model.DealId)%></div>
				<div style="float:left"><%: Html.TextBox("DealName", "SEARCH DEAL", new { @class = "wm", @id = "DealName", @style = "width:200px" })%></div>
				<%: Html.HiddenFor(model => model.DealId)%>
			</div>
			<div class="editor-label" style="width: auto; clear: right;">
				<%: Html.ImageButton("submit_active.png", new { @class="default-button" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
			</div>
			<%: Html.HiddenFor(model => model.FundId)%>
			<%}%>
			<div class="menu exportlist" style="position: absolute; right: 85px;">
				<ul>
					<li><a href="javascript:jHelper.chooseExpMenu(2,'Pdf');">Pdf</a></li>
					<li><a href="javascript:jHelper.chooseExpMenu(1,'Word');">Word</a></li>
					<li><a href="javascript:jHelper.chooseExpMenu(4,'Excel');">Excel</a></li>
				</ul>
			</div>
			<div class="export">
				<div class="print">
					<%:Html.Image("print.gif", new { @style = "cursor:pointer", @onclick = "javascript:dealOrgReport.print()" })%>
				</div>
				<div class="menu" onclick="javascript:jHelper.expandExpMenu(this);">
					<ul>
						<li><a id="lnkExportName" href="#">Pdf</a></li>
					</ul>
					<%: Html.Hidden("ExportId","")%>
				</div>
				<div class="darrow">
					<%:Html.Image("down_arrow.png", new { @style = "cursor:pointer", @onclick = "javascript:dealOrgReport.exportDeal();" })%></div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
	<div class="line">
	</div>
	<div id="ReportContent">
	</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { dealOrgReport.selectFund(ui.item.id);}" })%>
	<%= Html.jQueryAutoComplete("DealName", new AutoCompleteOptions { SearchFunction = "dealOrgReport.dealSearch", MinLength = 1, OnSelect = "function(event, ui) { dealOrgReport.selectDeal(ui.item.id);}" })%>
	<script type="text/javascript">		dealOrgReport.init();</script>
	<script id="ReportTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("DealOriginationReport", new DeepBlue.Models.Report.DealOriginationReportModel { IsTemplateDisplay = true }); %>
	</script>
</asp:Content>

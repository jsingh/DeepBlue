<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Report.FundBreakDownModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund Break Down
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("FundBreakDown.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.StylesheetLinkTag("report.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">REPORTS</span><span class="arrow"></span><span class="pname">Fund Break Down</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div id="ReportHeader">
		<div class="titlebox">
			<% Html.EnableClientValidation(); %>
			<%using (Html.BeginForm("", "", FormMethod.Get, new { @onsubmit = "return fundBreakDownReport.onSubmit(this);" })) {%>
			<div class="editor-label" style="width: auto;">
				<%: Html.LabelFor(model => model.FundId)%>
				<%: Html.TextBox("FundName", "SEARCH FUND", new { @class = "wm", @id = "FundName", @style = "width:200px" })%>
			</div>
			<div class="editor-label" style="width: auto; clear: right;">
				<%: Html.ImageButton("submit.png", new { @class="default-button" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
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
					<%:Html.Image("print.gif", new { @style = "cursor:pointer", @onclick = "javascript:fundBreakDownReport.print()" })%>
				</div>
				<div class="menu" onclick="javascript:jHelper.expandExpMenu(this);">
					<ul>
						<li><a id="lnkExportName" href="#">Pdf</a></li>
					</ul>
					<%: Html.Hidden("ExportId","")%>
				</div>
				<div class="darrow">
					<%:Html.Image("down_arrow.png", new { @style = "cursor:pointer", @onclick = "javascript:fundBreakDownReport.exportDeal();" })%></div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="line">
	</div>
	<div id="ReportMain">
		<div id="ReportDetail" class="rep-main" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { fundBreakDownReport.selectFund(ui.item.id);}" })%>
	<script type="text/javascript">		fundBreakDownReport.init();</script>
	<script id="FundBreakDownReportTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("FundBreakDownReport", new DeepBlue.Models.Report.FundBreakDownReportDetail { IsTemplateDisplay = true }); %>
	</script>
	<script type="text/javascript">
		$(document).ready(function () {
			jHelper.jqComboBox($("body"));
		});
	</script>
</asp:Content>

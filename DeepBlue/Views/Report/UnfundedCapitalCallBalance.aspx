<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Report.UnfundedCapitalCallBalanceModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Underlying Fund NAV
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("UnfundedCapitalCallBalance.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.StylesheetLinkTag("report.css")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">REPORTS</span><span class="arrow"></span><span class="pname">UnfundedCapitalCallBalance</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div id="ReportHeader">
		<div class="titlebox">
			<% Html.EnableClientValidation(); %>
			<%using (Html.Form(new { @id = "frmUnfundedCapitalCallBalance", @onsubmit = "return unfundedCapitalCallBalanceReport.onSubmit(this);" })) {%>
			<div class="editor-label" style="width: auto;">
				<%: Html.LabelFor(model => model.FundId)%>
				<%: Html.TextBox("FundName", "SEARCH  FUND", new { @class = "wm", @id = "FundName", @style = "width:200px" })%>
			</div>
			<div class="editor-label" style="width: auto; clear: right;">
				<%: Html.LabelFor(model => model.StartDate)%>
				<%: Html.TextBoxFor(model => model.StartDate)%>
			</div>
			<div class="editor-label" style="width: auto; clear: right;">
				<%: Html.LabelFor(model => model.EndDate)%>
				<%: Html.TextBoxFor(model => model.EndDate)%>
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
					<%:Html.Image("print.gif", new { @style = "cursor:pointer", @onclick = "javascript:unfundedCapitalCallBalanceReport.print()" })%>
				</div>
				<div class="menu" onclick="javascript:jHelper.expandExpMenu(this);">
					<ul>
						<li><a id="lnkExportName" href="#">Pdf</a></li>
					</ul>
					<%: Html.Hidden("ExportId","")%>
				</div>
				<div class="darrow">
					<%:Html.Image("down_arrow.png", new { @style = "cursor:pointer", @onclick = "javascript:unfundedCapitalCallBalanceReport.exportData();" })%></div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="line">
	</div>
	<div id="ReportMain">
		<div id="ReportDetail" class="rep-main" style="display: none">
			<div id="ReportContent">
				<% Html.RenderPartial("TBoxTop"); %>
				<%:Html.Table(new TableOptions { 
						ID = "UnfundedCapitalCallBalanceList"
						, HtmlAttributes = new { @class = "grid" }
						, Columns = new List<TableColumnOptions> {
						 new TableColumnOptions { InnerHtml = "DealNo", SortName = "DealNo", HtmlAttributes = new { @style = "width:10%" } },
						 new TableColumnOptions { InnerHtml = "FundName", SortName = "FundName" },
						 new TableColumnOptions { InnerHtml = "Unfunded", SortName = "UnfundedAmount", HtmlAttributes = new { @align = "right", @style="text-align:right;width:25%;" } },
						}
					})%>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { unfundedCapitalCallBalanceReport.selectFund(ui.item.id);}" })%>
	<%= Html.jQueryDatePicker("StartDate")%>
	<%= Html.jQueryDatePicker("EndDate")%>
	<%=Html.jQueryFlexiGrid("UnfundedCapitalCallBalanceList", new FlexigridOptions {
	ActionName = "UnfundedCapitalCallBalanceReport",
	ControllerName = "Report",
	HttpMethod = "POST",
	SortName = "FundName",
	SortOrder = "asc", 
	Paging = true, Autoload = false,
	OnTemplate = "unfundedCapitalCallBalanceReport.onTemplate",
	OnSuccess = "unfundedCapitalCallBalanceReport.onGridSuccess",
	BoxStyle = false
})%>
	<script type="text/javascript">		unfundedCapitalCallBalanceReport.init();</script>
	<%using (Html.jQueryTemplateScript("GridTemplate")) {%>
	{{each(i,row) rows}}
	<%: Html.TableRow(
		new { @id = "Row${i+1}"	, @if = "{{if i%2>0}}class=\"erow\"{{else}}class=\"grow\"{{/if}}" }
		, new List<TableColumnOptions>() {
				 new TableColumnOptions { InnerHtml = "${row.cell[0]}" },
				 new TableColumnOptions { InnerHtml = "${row.cell[1]}" },
				 new TableColumnOptions { InnerHtml = "${formatCurrency(row.cell[2])}", HtmlAttributes = new { @style = "text-align:right" } },
			}
		)%>
	{{/each}}
	<%}%>
</asp:Content>

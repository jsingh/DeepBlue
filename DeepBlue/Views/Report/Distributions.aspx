<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Report.FeesExpenseModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Distributions From Underlying Fund
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("Distributions.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.StylesheetLinkTag("report.css")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">REPORTS</span><span class="arrow"></span><span class="pname">Fund
					Break Down</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div id="ReportHeader">
		<div class="titlebox">
			<% Html.EnableClientValidation(); %>
			<%using (Html.Form(new { @id = "frmFeeExpense", @onsubmit = "return distributionsReport.onSubmit(this);" })) {%>
			<div class="editor-label" style="width: auto;">
				<%: Html.LabelFor(model => model.FundId)%>
				<%: Html.TextBox("FundName", "SEARCH FUND", new { @class = "wm", @id = "FundName", @style = "width:200px" })%>
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
					<%:Html.Image("print.gif", new { @style = "cursor:pointer", @onclick = "javascript:distributionsReport.print()" })%>
				</div>
				<div class="menu" onclick="javascript:jHelper.expandExpMenu(this);">
					<ul>
						<li><a id="lnkExportName" href="#">Pdf</a></li>
					</ul>
					<%: Html.Hidden("ExportId","")%>
				</div>
				<div class="darrow">
					<%:Html.Image("down_arrow.png", new { @style = "cursor:pointer", @onclick = "javascript:distributionsReport.export();" })%></div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="line">
	</div>
	<div id="ReportMain">
		<div id="ReportDetail" class="rep-main" style="display: none">
			<table cellpadding="0" cellspacing="0" border="0" id="DistributionsList">
				<thead>
					<tr>
						<th sortname="Date" style="width: 15%">
							Date
						</th>
						<th sortname="Type" style="width: 20%;">
							Type
						</th>
						<th sortname="Amount" align="right" style="width: 20%; text-align: right;">
							Amount
						</th>
						<th sortname="Note">
							Note
						</th>
					</tr>
				</thead>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { distributionsReport.selectFund(ui.item.id);}" })%>
	<%= Html.jQueryDatePicker("StartDate")%>
	<%= Html.jQueryDatePicker("EndDate")%>
	<%=Html.jQueryFlexiGrid("DistributionsList", new FlexigridOptions {
	ActionName = "DistributionsReport",
	ControllerName = "Report",
	HttpMethod = "POST",
	SortName = "", SortOrder = "desc", 
	Paging = true, Autoload = false,
	OnTemplate = "distributionsReport.onTemplate",
	OnSuccess = "distributionsReport.onGridSuccess",
	BoxStyle = false
})%>
	<script type="text/javascript">		distributionsReport.init();</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
	<td>
	${formatDate(row.cell[0])}
	</td>
	<td>
	${row.cell[1]}
	</td>
	<td style="text-align: right;">
	${formatCurrency(row.cell[2])}
	</td>
	<td>
	${row.cell[3]}
	</td>
</tr>
{{/each}}
	</script>
	<%--<script id="DistributionsReportTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("DistributionsReport", new DeepBlue.Models.Report.DistributionsReportDetail { IsTemplateDisplay = true }); %>
	</script>--%>
</asp:Content>

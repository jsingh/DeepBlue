<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Report.CapitalCallSummaryModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Capital Call Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("CCSummaryReport.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.StylesheetLinkTag("report.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">REPORTS</span><span class="arrow"></span><span class="pname">Capital
					Call Summary</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div id="ReportHeader" class="rep-header">
		<div class="editor-label" style="width: auto">
			<% Html.EnableClientValidation(); %>
			<%using (Html.BeginForm("", "", FormMethod.Get, new { @id = "CapitalCallSummary", @onsubmit = "return ccsummaryReport.onSubmit(this);" })) {%>
			<div style="float: left;">
				<%: Html.LabelFor(model => model.FundId)%>
				<%: Html.TextBox("FundName", "", new { @id = "FundName", @style = "width:200px" })%>
			</div>
			<div style="float: left; margin-left: 20px;">
				<%: Html.LabelFor(model => model.CapitalCallId)%>
				<%: Html.DropDownListFor(model => model.CapitalCallId, Model.CapitalCalls, new { @style = "width:200px" })%>
			</div>
			<div style="float: left; margin-left: 10px;">
				<%: Html.ImageButton("submit.png", new { @class="default-button" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
			</div>
			<%: Html.HiddenFor(model => model.FundId)%>
			<%: Html.ValidationMessageFor(model => model.FundId)%>
			<%: Html.ValidationMessageFor(model => model.CapitalCallId)%>
			<%}%>
		</div>
		<div class="editor-label" style="margin-left: 50px; clear: right">
			<%: Html.Anchor(Html.Image("print.png").ToHtmlString() + "&nbsp;Print",new { @onclick = "javascript:ccsummaryReport.print()" })%>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="ReportMain">
		<div id="ReportDetail" class="rep-main" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { ccsummaryReport.selectFund(ui.item.id);}" })%>
	<script type="text/javascript">		ccsummaryReport.init();</script>
	<script id="CCSummaryReportTemplate" type="text/x-jquery-tmpl">
		<div id='RepTop'>
		<div class='title'>Capital Call Summary</div><div class='fundname detail'>${FundName}</div>
		<div class='detail'>Capital Call Due ${CapitalCallDueDate} - ${TotalCapitalCall}</div>
		</div>
		<br/>
		<div id='RepContent'><div class="gbox">
			<table id='ccsummaryReport_tbl' cellspacing=0 cellpadding=0 border=0 class='grid'>
				<thead>
				<tr>
				<th class="lalign" style='width:30%;'>Investor</th>
				<th class="ralign">Commitment</th>
				<th class="ralign">Investments</th>
				<th class="ralign">Management Fees</th>
				<th class="ralign">Expenses</th>
				<th class="ralign">Total</th>
				</tr>
				</thead>
				<tbody>
				{{each(i,item) Items}}
				<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>${InvestorName}</td>
					<td class="ralign">${formatCurrency(Commitment)}</td>
					<td class="ralign">${formatCurrency(Investments)}</td>
					<td class="ralign" nowrap>${formatCurrency(ManagementFees)}</td>
					<td class="ralign">${formatCurrency(Expenses)}</td>
					<td class="ralign">${formatCurrency(Total)}</td>
				</tr>
				{{/each}}
				</tbody>
			</table></div>
		</div>
		<br/>
		<div class='foot-name'>Total CapitalCall:</div><div class='foot-value'>${TotalCapitalCall}</div>
		<div class='foot-name'>Total Mgt Fees:</div><div class='foot-value'>${TotalManagementFees}</div>
		<div class='foot-name'>Total Expenses:</div><div class='foot-value'>${TotalExpenses}</div>
		<div style='height:10px;clear:both;'>&nbsp;</div>
		<div class='foot-name'>Amount for Investments:</div><div class='foot-value'>${AmountForInv}</div>
		<div class='foot-name'>New Inv.:</div><div class='foot-value'>${NewInv}</div>
		<div class='foot-name'>Existing Inv.:</div><div class='foot-value'>${ExistingInv}</div>
	</script>
</asp:Content>

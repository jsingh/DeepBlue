<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Report.CapitalCallSummaryModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Capital Call Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.layoutengine.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("CCSummaryReport.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.StylesheetLinkTag("report.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="ReportMain">
		<div id="ReportHeader" class="rep-header">
			<div class="editor-label" style="width: auto">
				<% Html.EnableClientValidation(); %>
				<%using (Html.BeginForm("", "", FormMethod.Get, new { @id = "CapitalCallSummary", @onsubmit = "return ccsummaryReport.onSubmit('CapitalCallSummary');" })) {%>
				<div style="float: left">
					<%: Html.LabelFor(model => model.FundId)%>
					<%: Html.TextBox("FundName", "", new { @id = "FundName" })%>
					<%: Html.LabelFor(model => model.CapitalCallId)%>
					<%: Html.DropDownListFor(model => model.CapitalCallId, Model.CapitalCalls, new { @style = "width:150px" })%>
				</div>
				<div style="float: left; margin-left: 10px;">
					<%: Html.ImageButton("submit.png", new { @style = "width: 73px; height: 23px;" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
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
		<div id="ReportDetail" class="rep-main" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { ccsummaryReport.selectFund(ui.item.id);}" })%>
	<script type="text/javascript">		ccsummaryReport.init();</script>
	<script id="ccsummaryReportTemplate" type="text/x-jquery-tmpl">
		<div id='RepTop'>
		<div class='title'>Capital Call Summary</div><div class='fundname detail'>${FundName}</div>
		<div class='detail'>Capital Call Due ${CapitalCallDueDate} - ${TotalCapitalCall}</div>
		</div>
		<div id='RepContent' class='grid'>
			<table id='ccsummaryReport_tbl' cellspacing=0 cellpadding=0 border=0>
				<thead>
				<tr>
				<th style='text-align:left;width:30%;'>Investor</th>
				<th>Commitment</th>
				<th>Investments</th>
				<th>Management Fees</th>
				<th>Expenses</th>
				<th>Total</th>
				</tr>
				</thead>
				<tbody>
				{{each Items}}
				<tr>
					<td>${InvestorName}</td>
					<td style='text-align:right'>${Commitment}</td>
					<td style='text-align:right'>${Investments}</td>
					<td style='text-align:right' nowrap>${ManagementFees}</td>
					<td style='text-align:right'>${Expenses}</td>
					<td style='text-align:right'>${Total}</td>
				</tr>
				{{/each}}
				<tr>
					<td colspan=5 style='vertical-align:top'>
					<div class='foot-name'>Total CapitalCall:</div><div class='foot-value'>${TotalCapitalCall}</div>
					<div class='foot-name'>Total Mgt Fees:</div><div class='foot-value'>${TotalManagementFees}</div>
					<div class='foot-name'>Total Expenses:</div><div class='foot-value'>${TotalExpenses}</div>
					<div style='height:10px;clear:both;'>&nbsp;</div>
					<div class='foot-name'>Amount for Investments:</div><div class='foot-value'>${AmountForInv}</div>
					<div class='foot-name'>New Inv.:</div><div class='foot-value'>${NewInv}</div>
					<div class='foot-name'>Existing Inv.:</div><div class='foot-value'>${ExistingInv}</div>
					</td>
				</tr>
				</tbody>
			</table>
		</div>
	</script>
</asp:Content>

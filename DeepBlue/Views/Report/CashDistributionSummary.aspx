<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Report.CashDistributionSummaryModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Cash Distribution Summary
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("jquery.validate.min.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftAjax.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcAjax.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcValidation.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcCustomValidation.js")%>
	<%=Html.JavascriptInclueTag("jquery.layoutengine.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("DSummaryReport.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.StylesheetLinkTag("report.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">REPORTS</span><span class="arrow"></span><span class="pname">Capital
					Distribution Summary</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div id="ReportHeader" class="rep-header">
		<div class="editor-label" style="width: auto">
			<% Html.EnableClientValidation(); %>
			<%using (Html.BeginForm("", "", FormMethod.Get, new { @id = "DistributionSummary", @onsubmit = "return report.onSubmit('DistributionSummary');" })) {%>
			<div style="float: left">
				<%: Html.LabelFor(model => model.FundId)%>
				<%: Html.TextBox("FundName", "", new { @id = "FundName", @style = "width:200px" })%>
			</div>
			<div style="float: left; margin-left: 20px;">
				<%: Html.LabelFor(model => model.CapitalDistributionId)%>
				<%: Html.DropDownListFor(model => model.CapitalDistributionId, Model.CapitalDistributions, new { @style = "width:200px" })%>
			</div>
			<div style="float: left; margin-left: 10px;">
				<%: Html.ImageButton("submit.png", new { @class="default-button" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
			</div>
			<%: Html.HiddenFor(model => model.FundId)%>
			<%: Html.ValidationMessageFor(model => model.FundId)%>
			<%: Html.ValidationMessageFor(model => model.CapitalDistributionId)%>
			<%}%>
		</div>
		<div class="editor-label" style="margin-left: 50px; clear: right">
			<%: Html.Anchor(Html.Image("print.png").ToHtmlString() + "&nbsp;Print",new { @onclick = "javascript:report.print()" })%>
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
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { report.selectFund(ui.item.id);}" })%>
	<script type="text/javascript">		report.init();</script>
	<script id="reportTemplate" type="text/x-jquery-tmpl">
		<div id='RepTop'>
		<div class='title'>Cash Distribution Summary</div><div class='fundname detail'>${FundName}</div>
		<div class='detail'>Distribution Of ${DistributionDate} - ${TotalDistributionAmount}</div>
		</div><br/>
		<div id='RepContent'>
			<div class="gbox">
			<table id='report_tbl' cellspacing=0 cellpadding=0 border=0 class='grid'>
				<thead>
					<tr>
						<th class="lalign" style='width:40%;'>Investor</th>
						<th class="lalign" style='width:20%'>Designation</th>
						<th class="ralign">Commitment</th>
						<th class="ralign">Distribution Amount</th>
					</tr>
				</thead>
				<tbody>
				{{each(i,item) Items}}
					<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
						<td>${InvestorName}</td>
						<td>${Designation}</td>
						<td style='text-align:right'>${formatCurrency(Commitment)}</td>
						<td style='text-align:right'>${formatCurrency(DistributionAmount)}</td>
					</tr>
				{{/each}}
				</tbody>
				<tfoot>
				<tr>
					<td colspan=2 style='vertical-align:top'><b>Total Distribution:${TotalDistributionAmount}</b></td>
					<td style='text-align:right;' nowrap><b>With Carry Amount:<br/>And Repayment of Mgt Fees:</b></td>
					<td style='text-align:right;' nowrap><b>${WithCarryAmount}<br/>${RepayManFees}</b></td>
				</tr>
				</tfoot>
			</table></div>
		</div>
	</script>
</asp:Content>

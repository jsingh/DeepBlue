<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.DealFundDetail>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.JavascriptInclueTag("DealReport.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealreport.css")%>
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
					DEAL REPORT<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%></div>
			</div>
			<div class="rightcol">
				<%: Html.TextBox("FundName", "SEARCH FUND", new { @id = "FundName", @class = "wm",  @style = "width:200px" })%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="DealReportMain" style="float:left;display:none;width:100%;">
		<div class="menu exportlist" style="position: absolute; right: 85px;">
			<ul>
				<li><a href="javascript:dealReport.chooseExpMenu(2,'Pdf');">Pdf</a></li>
				<li><a href="javascript:dealReport.chooseExpMenu(1,'Word');">Word</a></li>
				<li><a href="javascript:dealReport.chooseExpMenu(4,'Excel');">Excel</a></li>
			</ul>
		</div>
		<div class="titlebox">
			<div class="left_title">
				Deal Report -
				<%:Html.Span("", new { @id= "SpnFundName" }) %>
			</div>
			<div class="export" style="float: right">
				<div class="print">
					<%:Html.Image("print.gif", new { @style = "cursor:pointer", @onclick = "javascript:dealReport.printArea();" })%>
				</div>
				<div class="menu" onclick="javascript:dealReport.expandExpMenu(this);">
					<ul>
						<li><a id="lnkExportName" href="#">Pdf</a></li>
					</ul>
					<%: Html.Hidden("ExportId","")%>
				</div>
				<div class="darrow">
					<%:Html.Image("down_arrow.png", new { @style="cursor:pointer", @onclick = "javascript:dealReport.exportDeal();" })%></div>
			</div>
			<div style="display: none; float: right; margin-right: 38%;" id="ReportLoading">
				<%:Html.Image("ajax.jpg")%>&nbsp;Loading....</div>
		</div>
		<div class="line">
		</div>
		<div id="ReportContent" class="report-content">
			<div class="gbox" id="ReportBox">
				<table cellpadding="0" class="grid" cellspacing="0" border="0" id="ReportList">
					<thead>
						<tr class="report_tr">
							<th sortname="DealId" style="display: none">
								DealId
							</th>
							<th class="sorted sdesc" sortname="DealNumber" align="left" style="width: 5%">
								<span>Deal No.</span>
							</th>
							<th class="sorted sdesc" sortname="DealName" align="left" style="width: 18%">
								<span>Deal Name</span>
							</th>
							<th sortname="DealDate" align="left" style="width: 12%">
								<span>Deal Date</span>
							</th>
							<th sortname="NetPurchasePrice" align="right" style="text-align: right; width: 12%">
								<span>Net Purchase Price</span>
							</th>
							<th sortname="GrossPurchasePrice" align="right" style="text-align: right; width: 12%">
								<span>Gross Purchase Price</span>
							</th>
							<th sortname="CommittedAmount" align="right" style="text-align: right; width: 12%">
								<span>Commitment Amount</span>
							</th>
							<th sortname="UnfundedAmount" align="right" style="text-align: right; width: 12%">
								<span>Unfunded Amount</span>
							</th>
							<th class="sorted sdesc" sortname="TotalAmount" align="right" style="text-align: right;
								width: 12%">
								<span>Total Amount</span>
							</th>
							<th style="width: 2%">
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
			</div>
			<br />
			<br />
			<center>
				<div>
					<table cellpadding="0" cellspacing="0" border="0" id="ViewMoreDetail">
					</table>
				</div>
			</center>
		</div>
		<%: Html.Hidden("FundId","0",new  { @id="FundId"}) %>
		<%: Html.Hidden("SortName", "", new { @id = "SortName" })%>
		<%: Html.Hidden("SortOrder", "", new { @id = "SortOrder" })%>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		dealReport.init(); </script>
	<%if (Model.FundId > 0) {%>
	<script type="text/javascript">$(document).ready(function() { setTimeout( function() {  dealReport.selectFund(<%=Model.FundId%>,'<%=Model.FundName%>'); } ,200 ); }); </script>
	<%}%>
	<%=Html.jQueryAjaxTable("ReportList", new AjaxTableOptions {
	ActionName = "DealReportList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, Paging = true 
	, OnSubmit = "dealReport.onSubmit"
	, OnRowBound = "dealReport.onRowBound"
	, OnRowClick = "dealReport.onRowClick"
	, OnSuccess = "dealReport.onSuccess"
	, OnChangeSort = "dealReport.onChangeSort"
	, AppendExistRows= true
	, Autoload = false
	, RowOptions =  new int[] { 15, 20, 50, 100 }
	, RowsLength  = 10
	})%>
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { dealReport.selectFund(ui.item.id,ui.item.label);}" })%>
	<script id="DealDetailTemplate" type="text/x-jquery-tmpl"> 
		<div class="treerow">
			<div class="gbox">
            <table cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr class="row subdet-title">
						<th>Underlying Funds
                        </th>
                     </tr>
					 </thead>
               </thead>
			</table>
			</div>
			<table id="tblUnderlyingFund" class="grid" cellpadding="0" cellspacing="0" border="0">
				<thead>
					<tr class="tblUnderlyingFund_tr">
						<th style="width:23%">
							Fund Name
						</th>
						<th style="width:15%">
							Record Date
						</th>
						<th style="width:12%">
							Fund NAV
						</th>
                        <th style="text-align:right;width:12%">
                           Gross Purchase Price
                        </th>
						<th style="text-align:right;width:12%">
							Capital Commitment
						</th>
                        <th style="text-align:right;width:12%">
                          Amount Unfunded
                        </th>
                        <th style="text-align:right;width:12%">
                         Fund Size (%)
                        </th><th style="width: 2%"><div style="width:14px">&nbsp;</div>
						</th>
					</tr>
				</thead>
				{{if DealUnderlyingFunds.length>0}}
				<tbody>
					{{each(i,df) DealUnderlyingFunds}}
					<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
						<td>
							${FundName}
						</td>
						<td class="datecell">
							${RecordDate}
						</td>
						<td>
							${FundNAV}
						</td>
                        <td class="dollarcell" style="text-align:right">
							${GrossPurchasePrice}
                        </td>
						<td class="dollarcell" style="text-align:right">
							${CommittedAmount}
						</td>
                        <td class="dollarcell" style="text-align:right">
							${UnfundedAmount}
                        </td>
                        <td style="text-align:right">{{if Percent>0}}${Percent}{{/if}}
                        </td><td></td>
					</tr>
					{{/each}}
				</tbody>
				<tfoot>
					<tr><td>Total
						</td>
                        <td>
                        </td>
						<td style="text-align:left;">${TotalFundNAV}
						</td>
						<td></td>
						<td  style="text-align:right">${TotalCommitted}
						</td>
                        <td style="text-align:right">${TotalUnfunded}
                        </td>
						<td>
						</td><td></td>
					</tr>
				</tfoot>
				{{/if}}
			</table>
			<br/>
			<div class="gbox">
            <table cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr class="row subdet-title">
						<th>Underlying Directs
                        </th>
                     </tr>
					 </thead>
               </thead>
			</table>
			</div>
			<div class="gbox">
			<table id="tblUnderlyingDirect" class="grid" cellpadding="0" cellspacing="0" border="0">
				<thead>
					<tr class="tblUnderlyingDirect_tr">
						<th style="width: 15%">
							Company
						</th>
						<th style="width: 8%">
							Security
						</th>
						<th style="width: 7%">
							Record Date
						</th>
						<th style="width: 9%">
							Tax Cost Basic
						</th>
						<th style="width: 9%">
							Tax Cost Date
						</th>
						<th style="text-align:right;width:9%;">
                           Purchase Price
                        </th>
						<th style="text-align:right;width:9%;">
							FMV
						</th>
						<th style="width:12%;">
							No.of Shares
						</th>
                        <th style="text-align:right;width:9%;">
							Fund Size (%)
						</th>
						<th style="width: 2%"><div style="width:14px">&nbsp;</div>
						</th>
					</tr>
				</thead>
				{{if DealUnderlyingDirects.length>0}}
				<tbody>
					{{each(i,dd) DealUnderlyingDirects}}
					 <tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
						<td>
							${IssuerName}
						</td>
						<td>
							${Security}
						</td>
						<td class="datecell">
							${RecordDate}
						</td>
						<td class="dollarcell">${TaxCostBase}
                        </td>
                        <td class="datecell">${TaxtCostDate}
                        </td>
						  <td class="dollarcell" style="text-align:right">
							${PurchasePrice}
                        </td>
						<td style="text-align:right">
							${formatNumber(FMV)}
						</td>
						<td>
							${NumberOfShares}
						</td>
                        <td style="text-align:right;">{{if Percent>0}}${Percent}{{/if}}
                        </td>
						<td>
						</td>
					</tr>
					{{/each}}
				</tbody>
				<tfoot>
					  <tr class="total">
                       <td>
							Total
						</td>
						<td>
						</td>
						<td>
						</td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
						<td  style="text-align:right">${TotalFMV}
						</td>
						<td>
						</td>
                        <td>
                        </td>
						<td>
						</td>
                    </tr>
				</tfoot>
				{{/if}}
			</table>
			</div><br/><br/><div class="gbox"></div>
		 </div>
	</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("DealReport.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealreport.css")%>
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
					DEAL REPORT<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%></div>
			</div>
			<div class="rightcol">
				<%: Html.TextBox("FundName", "SEARCH FUND", new { @id = "FundName", @class = "wm",  @style = "width:200px" })%>
			</div>
		</div>
	</div>
	<div class="titlebox">
		<div class="left_tile">
			Deal Report
		</div>
		<div class="left-col" style="margin-left: 10px; display: none" id="ReportLoading">
			<%:Html.Image("ajax.jpg")%>&nbsp;Loading....</div>
		<div class="export" style="float: right">
			<div style="height: 20px;">
				<div style="float: left; width: 24px; height: 20px;">
					<%:Html.Image("print.gif")%>
				</div>
				<div style="float: left; width: 50px; height: 20px;">
					<a href="#">
						<%:Html.Image("pdf.gif")%></a></div>
				<div style="float: right; height: 20px;">
					<%:Html.Image("down_arrow.png")%></div>
			</div>
			<div style="height: 25px;">
				<a href="javascript:dealReport.exportDeal(1);">
					<%:Html.Image("word.gif")%>
				</a>
			</div>
			<div style="height: 25px;">
				<%--print--%>
				<a href="javascript:dealReport.exportDeal(3);">
					<%:Html.Image("excel.gif")%>
				</a>
			</div>
		</div>
	</div>
	<div class="line">
	</div>
	<div id="ReportContent" class="report-content">
		<table cellpadding="0" class="grid" cellspacing="0" border="0" id="ReportList">
			<thead>
				<tr class="report_tr">
					<th style="display: none">
						DealId
					</th>
					<th align="center" style="width: 10%">
						<span>Deal No.</span>
					</th>
					<th align="left">
						<span>Deal Name</span>
					</th>
					<th align="left">
						<span>Fund Name (S)</span>
					</th>
					<th align="right">
						<span>Committed Amount</span>
					</th>
					<th align="right">
						<span>Unfunded Amount</span>
					</th>
					<th align="right">
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
	<%: Html.Hidden("FundId","0",new  { @id="FundId"}) %>
	<%: Html.Hidden("SortName", "DealName", new { @id = "SortName" })%>
	<%: Html.Hidden("SortOrder", "asc", new { @id = "SortOrder" })%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		dealReport.init(); </script>
	<%=Html.jQueryAjaxTable("ReportList", new AjaxTableOptions {
	ActionName = "DealReportList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, SortName = "DealNumber"
	, Paging = true 
	, OnSubmit = "dealReport.onSubmit"
	, OnRowBound = "dealReport.onRowBound"
	, OnRowClick = "dealReport.onRowClick"
	, OnSuccess = "dealReport.onSuccess"
	, OnChangeSort = "dealReport.onChangeSort"
	, AppendExistRows= true
	, Autoload = false
	})%>
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { dealReport.selectFund(ui.item.id);}" })%>
	<script id="DealDetailTemplate" type="text/x-jquery-tmpl"> 
		<div class="treerow">
			<table id="tblUnderlyingFund" class="grid" cellpadding="0" cellspacing="0" border="0">
				<thead>
					<tr class="tblUnderlyingFund_tr">
						<th>
							Fund Name
						</th>
                        <th>
                           Gross Purchase Price
                        </th>
						<th>
							Fund NAV
						</th>
						<th>
							Capital Commitment
						</th>
                        <th>
                          Amount Unfunded
                        </th>
						<th>
							Record Date
						</th>
                        <th>
                         Fund Size (%)
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
                        <td class="dollarcell" style="text-align:right;padding-right:20px;">${GrossPurchasePrice}
                        </td>
						<td  style="text-align:left;padding-left:10px;">
							${FundNAV}
						</td>
						<td class="dollarcell" style="text-align:right">
							${CommittedAmount}
						</td>
                        <td class="dollarcell" style="text-align:right">${UnfundedAmount}
                        </td>
						<td class="datecell" style="text-align:left;padding-left:15px;">
							${RecordDate}
						</td>
                        <td>${Percent}
                        </td>
					</tr>
					{{/each}}
				</tbody>
				<tfoot>
					<tr>	<td>Total
						</td>
                        <td>
                        </td>
						<td style="text-align:left;padding-left:10px;">${TotalFundNAV}
						</td>
						<td  style="text-align:right">${TotalCommitted}
						</td>
                        <td style="text-align:right">${TotalUnfunded}
                        </td>
						<td>
						</td>
                        <td>
                        </td>
						</tr>
				</tfoot>
				{{/if}}
			</table>
            <table cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th style="background-color:#D3D4D8;padding:10px;text-align:center;">Underlying Funds
                        </th>
                     </tr>
					 </thead>
               </thead>
			</table><br/><br/>
			<table id="tblUnderlyingDirect" class="grid" cellpadding="0" cellspacing="0" border="0">
				<thead>
					<tr class="tblUnderlyingDirect_tr">
					 
						<th>
							Company
						</th>
						<th>
							Security
						</th>
						<th>
							No.of Shares
						</th>
                        <th>
                           Purchase Price
                        </th>
                        <th>
							Tax Cost Basic
						</th>
                         <th>
							Tax Cost Date
						</th>
						<th>
							FMV
						</th>
						<th>
							Record Date
						</th>
                        <th>
							Fund Size (%)
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
						<td  style="text-align:left;padding-left:15px;">
							${NumberOfShares}
						</td>
                        <td class="dollarcell" style="text-align:right">${PurchasePrice}
                        </td>
                        <td class="dollarcell" style="text-align:left;padding-left:15px;">${TaxCostBase}
                        </td>
                        <td class="datecell" style="text-align:left;padding-left:15px;">${TaxtCostDate}
                        </td>
						<td  style="text-align:right">
							${FormatFMV}
						</td>
						<td class="datecell" style="text-align:left;padding-left:15px;">
							${RecordDate}
						</td>
                        <td style="text-align:left;padding-left:10px;">${Percentage}
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
                        <td style="text-align:right">${TotalPurchasePrice}
                        </td>
                        <td style="text-align:right">
                        </td>
                        <td>
                        </td>
						<td  style="text-align:right">${TotalFMV}
						</td>
						<td class="datecell">
						</td>
                        <td>
                        </td>
                    </tr>
				</tfoot>
				{{/if}}
			</table>
             <table cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th style="background-color:#D3D4D8;padding:10px;text-align:center;">Underlying Directs
                        </th>
                     </tr>
					 </thead>
               </thead>
			</table>
		 </div>
		 
	</script>
</asp:Content>

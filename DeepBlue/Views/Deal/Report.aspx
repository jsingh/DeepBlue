<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("DealReport.js")%>
	<%=Html.StylesheetLinkTag("dealreport.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="report-header">
		<div class="line">
		</div>
		<div class="title">
			<div class="left-col">
				Deal Report</div>
			<div class="left-col" style="margin-left: 10px; display: none" id="ReportLoading">
				<%:Html.Image("ajax.jpg")%>&nbsp;Loading....</div>
			<div class="left-col" style="margin-left: 400px;">
				<%: Html.Label("Fund:") %>&nbsp;<%: Html.TextBox("FundName", "", new { @id="FundName", @style = "width:200px" })%>
			</div>
			<div class="right-col export">
				<a id="lnkExport" style="cursor: pointer">Export to&nbsp;<%:Html.Image("arrow_down.png")%></a></div>
		</div>
		<div class="line">
		</div>
	</div>
	<div id="ReportContent" class="report-content">
		<table cellpadding="0" cellspacing="0" border="0" id="ReportList">
			<thead>
				<tr>
					<th style="display: none">
						DealId
					</th>
					<th align="center" sortname="DealNumber" style="width: 10%">
						<span>Deal No.</span>
					</th>
					<th align="center" sortname="DealName">
						<span>Deal Name</span>
					</th>
					<th align="center" sortname="FundName">
						<span>Fund Name</span>
					</th>
					<th align="center" sortname="SellerName">
						<span>Seller Name</span>
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
	<div id="ExportMenu">
		<ul>
			<li>
				<%:Html.Anchor("Word", "javascript:dealReport.exportDeal(1);")%></li>
			<li>
				<%:Html.Anchor("Pdf", "#")%></li>
			<li>
				<%:Html.Anchor("Print", "javascript:dealReport.exportDeal(3);")%></li>
		</ul>
	</div>
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
	<div class="detail-content">
		<div class="detail-left">
			<table id="tblUnderlyingFund" cellpadding="0" cellspacing="1" border="0">
				<thead>
					<tr>
						<th>
							No.
						</th>
						<th>
							Fund Name
						</th>
						<th>
							Fund NAV
						</th>
						<th>
							Commitment
						</th>
						<th>
							Record Date
						</th>
					</tr>
				</thead>
				{{if DealUnderlyingFunds.length>0}}
				<tbody>
					{{each DealUnderlyingFunds}}
					<tr>
						<td>
						</td>
						<td>
							${FundName}
						</td>
						<td>
							${NAV}
						</td>
						<td class="dollarcell">
							${Commitment}
						</td>
						<td class="datecell">
							${RecordDate}
						</td>
					</tr>
					{{/each}}
				</tbody>
				{{/if}}
			</table><br/>
			Underlying Funds
		</div>
		<div class="detail-right">
			<table id="tblUnderlyingDirect" cellpadding="0" cellspacing="1" border="0">
				<thead>
					<tr>
						<th>
							No.
						</th>
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
							Percentage
						</th>
						<th>
							FMV
						</th>
						<th>
							Record Date
						</th>
					</tr>
				</thead>
				{{if DealUnderlyingDirects.length>0}}
				<tbody>
					{{each DealUnderlyingDirects}}
					<tr>
						<td>
						</td>
						<td>
							${Company}
						</td>
						<td>
							${Security}
						</td>
						<td>
							${NoOfShares}
						</td>
						<td>
							${Percentage}
						</td>
						<td>
							${FMV}
						</td>
						<td class="datecell">
							${RecordDate}
						</td>
					</tr>
					{{/each}}
				</tbody>
				{{/if}}
			</table><br/>
			Underlying Directs
		</div>
	</div>
	</script>
</asp:Content>

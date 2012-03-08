<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View Activities
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.JavascriptInclueTag("ViewActivities.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealactivity.css")%>
	<%=Html.StylesheetLinkTag("dealreport.css")%>
	<%=Html.StylesheetLinkTag("viewactivities.css")%>
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
					View Activities<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%></div>
			</div>
			<div class="rightcol">
				<%: Html.TextBox("FundName", "SEARCH  FUND", new { @id = "FundName", @class = "wm", @style = "width:200px" })%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="TabHeader" style="display:none">
		<div class="header">
			<div id="TabMain" class="section-tab-main" style="padding-left: 0">
				<div class="section-tab-box" style="padding-left: 60px;">
					<%using (Html.Tab(new { @id = "TabDealGrid", @class = "section-tab-sel", @onclick = "javascript:viewActivities.selectDealTab(this,'DealReportMain');" })) {%>Deals<%}%>
				</div>
			</div>
		</div>
	</div>
	<div id="ActivityMain">
		<div id="DealReportMain" class="section-det" style="float: left; display: none; width: 100%;">
			<div class="titlebox">
			<div class="left_title">
				Deals -
				<%:Html.Span("", new { @id= "SpnFundName" }) %>
			</div>
		</div>
			<div class="line">
			</div>
			<div id="ReportContent" class="report-content">
			<div id="ReportBox">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" class="grid" cellspacing="0" border="0" id="ReportList">
					<thead>
						<tr>
							<th sortname="DealId" style="display: none">
								DealId
							</th>
							<th class="sorted asc" sortname="DealNumber" align="left" style="width: 8%">
								<span>Deal No.</span>
							</th>
							<th sortname="DealName" align="left" style="width: 18%">
								<span>Deal Name</span>
							</th>
							<th sortname="DealDate" align="left" style="width: 12%">
								<span>Deal Date</span>
							</th>
							<th sortname="NetPurchasePrice" align="right" style="text-align: right; width: 13%">
								<span>Net Purchase Price</span>
							</th>
							<th sortname="GrossPurchasePrice" align="right" style="text-align: right; width: 13%">
								<span>Gross Purchase Price</span>
							</th>
							<th sortname="CommittedAmount" align="right" style="text-align: right; width: 13%">
								<span>Commitment Amount</span>
							</th>
							<th sortname="NoOfShares" align="right" style="text-align: right; width: 12%">
								<span>No.Of Share</span>
							</th>
							<th sortname="FMV" align="right" style="text-align: right;
								width: 12%">
								<span>FMV</span>
							</th>
							<th style="width: 2%">
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
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
	 </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		viewActivities.init(); </script>
	<%=Html.jQueryAjaxTable("ReportList", new AjaxTableOptions {
	ActionName = "ActivityDealsList",
	ControllerName = "Deal"
	, HttpMethod = "GET"
	, Paging = true 
	, OnSubmit = "viewActivities.onSubmit"
	, OnRowBound = "viewActivities.onRowBound"
	, OnRowClick = "viewActivities.onRowClick"
	, OnSuccess = "viewActivities.onSuccess"
	, OnChangeSort = "viewActivities.onChangeSort"
	, AppendExistRows= true
	, Autoload = false
	})%>
	<%using (Html.jQueryTemplateScript("TabTemplate")) {%>
	<div style="float: left">
		<div id="Tab${id}" onmousemove="javascript:$('#tabdel${id}').show();" onmouseout="javascript:$('#tabdel${id}').hide();"
			class="section-tab section-tab-sel">
			<div class="left">
			</div>
			<div class="center" onclick="javascript:viewActivities.selectDealTab($(this).parent(),'Edit${id}');">
				${DealName}</div>
			<div class="right">
			</div>
			<div class='tab-delete' style='display: none' id="tabdel${id}" onclick="javascript:viewActivities.deleteTab(${id},true);">
			</div>
		</div>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ActivityListTemplate")) {%>
	<div id="Edit${id}" class="section-det un-direct-list">
		<% Html.RenderPartial("ActivityReport"); %> 
	</div>
	<%}%>
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { viewActivities.selectFund(ui.item.id,ui.item.label);}" })%>
	<%using (Html.jQueryTemplateScript("DealDetailTemplate")) {%> 
		<div class="treerow">
			<div class="gbox">
				<table cellpadding="0" cellspacing="0" border="0" class="grid">
					<thead>
						<tr class="row subdet-title">
							<th>Underlying Funds
							</th>
						 </tr>
				   </thead>
				</table>
			</div>
			<div class="gbox">
				<table id="tblUnderlyingFund" class="grid ngrid" cellpadding="0" cellspacing="0" border="0">
				<thead>
					<tr class="tblUnderlyingFund_tr">
						<th style="width:23%">
							Fund Name
						</th>
						<th style="width:8%">
							Record Date
						</th>
						<th style="text-align:right;width:12%">
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
                        </th>
						<th style="width: 9%">
							&nbsp;
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
							<td class="dollarcell" style="text-align:right">
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
							</td>
							<td>
								<%: Html.Image("Activities_active.png", new { @class = "default-button gbutton", @onclick = "javascript:viewActivities.viewActivity(${DealId},${UnderlyingFundId},0,${DealUnderlyingFundId},'${FundName}');" })%>
							</td>
						</tr>
					{{/each}}
				</tbody>
				<tfoot>
					<tr>
						<td><b>Total</b>
						</td>
                        <td>
                        </td>
						<td style="text-align:right;">${TotalFundNAV}
						</td>
						<td style="text-align:right">${TotalGrossPurchasePrice}</td>
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
			</div>
			<br/>
			<div class="gbox">
				<table cellpadding="0" cellspacing="0" border="0" class="grid">
					<thead>
						<tr class="row subdet-title">
							<th>Underlying Directs
							</th>
						 </tr>
				   </thead>
				</table>
			</div>
			<div class="gbox">
				<table id="tblUnderlyingDirect" class="grid ngrid" cellpadding="0" cellspacing="0" border="0">
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
						<th style="width: 9%;text-align:right;">
							Tax Cost Basis Per Share
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
						<th style="width:12%;text-align:right;">
							No.of Shares
						</th>
                        <th style="text-align:right;width:9%;">
							Fund Size (%)
						</th>
						<th style="width: 2%">&nbsp;
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
						<td class="dollarcell" style="text-align:right">${TaxCostBase}
                        </td>
                        <td class="datecell">${TaxtCostDate}
                        </td>
						  <td class="dollarcell" style="text-align:right">
							${PurchasePrice}
                        </td>
						<td style="text-align:right">
							${formatCurrency(FMV)}
						</td>
						<td style="text-align:right">
							${formatNumber(NumberOfShares)}
						</td>
                        <td style="text-align:right;">{{if Percent>0}}${Percent}{{/if}}
                        </td>
						<td>
							<%: Html.Image("Activities_active.png", new { @class = "default-button gbutton", @onclick = "javascript:viewActivities.viewActivity(${DealId},0,${DealUnderlyingDirectId},0,'${IssuerName}');" })%>
						</td>
					</tr>
					{{/each}}
				</tbody>
				<tfoot>
					  <tr class="total">
                       <td>
							<b>Total</b>
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
			</div><br/><br/>
			<div class="gbox"></div>
		 </div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ActivityGridTemplate")) {%>
		<% Html.RenderPartial("ActivityGrid"); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("ActivityGridTemplate")) {%>
		<% Html.RenderPartial("ActivityGrid"); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("CapitalCallListTemplate")){%>
		{{each(i,row) rows}}
			<tr {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>${row.UnderlyingFundName}</td>
				<td>${row.FundName}</td>
				<td style="text-align:right">${formatCurrency(row.Amount)}</td>
				<td>${formatDate(row.NoticeDate)}</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("CashDistributionListTemplate")) {%>
		{{each(i,row) rows}}
			<tr {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>${row.UnderlyingFundName}</td>
				<td>${row.FundName}</td>
				<td style="text-align:right">${formatCurrency(row.Amount)}</td>
				<td>${formatDate(row.NoticeDate)}</td>
				<td>${row.CashDistributionType}</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("PostRecordCapitalCallListTemplate")) {%>
		{{each(i,row) rows}}
			<tr {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>${row.UnderlyingFundName}</td>
				<td>${row.FundName}</td>
				<td>${row.DealName}</td>
				<td style="text-align:right">${formatCurrency(row.Amount)}</td>
				<td>${formatDate(row.CapitalCallDate)}</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("PostRecordCashDistributionListTemplate")) {%>
		{{each(i,row) rows}}
			<tr {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>${row.UnderlyingFundName}</td>
				<td>${row.FundName}</td>
				<td>${row.DealName}</td>
				<td style="text-align:right">${formatCurrency(row.Amount)}</td>
				<td>${formatDate(row.DistributionDate)}</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("StockDistributionListTemplate")) {%>
		{{each(i,row) rows}}
			<tr {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>${row.UnderlyingFundName}</td>
				<td>${row.FundName}</td>
				<td style="text-align:right">${formatNumber(row.NumberOfShares)}</td>
				<td style="text-align:right">${formatCurrency(row.PurchasePrice)}</td>
				<td>${formatDate(row.NoticeDate)}</td>
				<td>${formatDate(row.DistributionDate)}</td>
				<td style="text-align:right">${formatCurrency(row.TaxCostBase)}</td>
				<td>${formatDate(row.TaxCostDate)}</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("UnderlyingFundAdjustmentTemplate")) {%>
		{{each(i,row) rows}}
			<tr {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>${row.UnderlyingFundName}</td>
				<td>${row.FundName}</td>
				<td style="text-align:right">${formatNumber(row.CommitmentAmount)}</td>
				<td style="text-align:right">${formatCurrency(row.UnfundedAmount)}</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("UnderlyingFundValuationTemplate")) {%>
		{{each(i,row) rows}}
			<tr {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>${row.UnderlyingFundName}</td>
				<td>${row.FundName}</td>
				<td style="text-align:right">${formatCurrency(row.FundNAV)}</td>
				<td>${formatDate(row.FundNAVDate)}</td>
				<td style="text-align:right">${formatCurrency(row.CalculateNAV)}</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("UnderlyingFundValuationHistoryTemplate")) {%>
		{{each(i,row) rows}}
			<tr {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>${row.UnderlyingFundName}</td>
				<td>${row.FundName}</td>
				<td style="text-align:right">${formatCurrency(row.FundNAV)}</td>
				<td>${formatDate(row.FundNAVDate)}</td>
			</tr>
		{{/each}}
	<%}%>
</asp:Content>

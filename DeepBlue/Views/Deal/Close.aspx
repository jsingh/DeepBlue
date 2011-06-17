<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateDealCloseModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Close
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("DealClose.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.StylesheetLinkTag("deal.css") %>
	<%=Html.StylesheetLinkTag("dealclose.css") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="left-search">
		<p>
			Investments >>
			<%: Html.Span("Close Deal", new { @class = "blue" })%>&nbsp;&nbsp;<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%></p>
	</div>
	<div class="right-search">
		<%: Html.TextBox("Deal","Search Deal", new { @id="Deal", @class="wm", @style = "width:200px" })%>
	</div>
	<div class="line">
	</div>
	<div class="dc-box">
		<div class="cell">
			<%: Html.Span("", new { @id = "SpnFundName" })%>
		</div>
		<div class="cell">
			Deal No:-<%: Html.Span("", new { @id = "SpnDealNo", @style = "padding-left:10px;" })%></div>
		<div class="cell">
			Deal Name-<%: Html.Span("", new { @id = "SpnDealName", @style = "padding-left:10px;" })%></div>
	</div>
	<div class="line">
	</div>
	<div class="dc-box">
		<div class="cell">
			Existing Deal Closes
		</div>
	</div>
	<div class="dc-box">
		<table id="DealCloseList" class="grid" cellpadding="0" cellspacing="0" border="0"
			style="width: 100%;">
			<thead>
				<tr>
					<th style="display: none;">
					</th>
					<th style="width: 5%" align="center">
						No.
					</th>
					<th style="width: 10%" align="center">
						Deal Close
					</th>
					<th style="width: 10%" align="center">
						Close Date
					</th>
					<th style="width: 15%" align="right">
						Total Net Purchase Price
					</th>
					<th>
					</th>
				</tr>
			</thead>
			<tbody>
			</tbody>
		</table>
	</div>
	<div class="dc-box">
		<div class="cell">
			New Deal Close</div>
		<div class="cell link">
			<%: Html.Anchor("Add Deal Close","javascript:dealClose.add(0);")%>
		</div>
		<div class="fundslist">
			<%using (Html.Form(new { @id = "frmDealClose", @onsubmit = "return false;" })) {%>
			<div id="NewDealClose">
				<div class="editor-label">
					<%: Html.Span("", new { @id = "SpnDealCloseNo" })%>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.CloseDate) %>
				</div>
				<%: Html.HiddenFor(model => model.DealNumber)%>
				<%: Html.HiddenFor(model => model.DealId)%>
				<%: Html.HiddenFor(model => model.DealClosingId)%>
			</div>
			<div class="line">
			</div>
			<div class="cell">
				Underlying Funds</div>
			<div class="dc-box">
				<table id="DealUnderlyingFundList" class="grid" cellpadding="0" cellspacing="0" border="0"
					style="width: 100%;">
					<thead>
						<tr>
							<th style="width: 5%">
							</th>
							<th style="width: 15%">
								Fund Name
							</th>
							<th style="width: 15%">
								Commitment Amount
							</th>
							<th style="width: 15%">
								Gross Purchase Price
							</th>
							<th style="width: 15%">
								Post Record Capital Call
							</th>
							<th style="width: 15%">
								Post Record Distribution
							</th>
							<th style="width: 15%">
								Net Purchase Price
							</th>
							<th>
							</th>
						</tr>
					</thead>
				</table>
			</div>
			<div class="line">
			</div>
			<div class="cell">
				Underlying Directs</div>
			<div class="dc-box">
				<table id="DealUnderlyingDirects" class="grid" cellpadding="0" cellspacing="0" border="0"
					style="width: 100%;">
					<thead>
						<tr>
							<th style="width: 5%">
							</th>
							<th style="width: 15%">
								Direct Name
							</th>
							<th style="width: 15%">
								No. Of Shares
							</th>
							<th style="width: 15%">
								Price
							</th>
							<th style="width: 15%">
								Fair Market Value
							</th>
							<th>
							</th>
						</tr>
					</thead>
				</table>
			</div>
			<div class="savefooter" style="width: 100%">
				<div class="cell">
					<%: Html.ImageButton("CloseDeal.png", new { @style="cursor:pointer", @onclick = "javascript:dealClose.saveDealClose(false,'SpnDCloseLoading');" })%>
				</div>
				<div class="cell">
					<%: Html.Span("", new { @id = "SpnDCloseLoading" } )%>
				</div>
			</div>
			<%}%>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryDatePicker("CloseDate")%>
	<%=Html.jQueryAutoComplete("Deal", new AutoCompleteOptions { Source = "/Deal/FindDeals", MinLength = 1, OnSelect = "function(event, ui) { dealClose.selectDeal(ui.item.id);}" })%>
	<%=Html.jQueryAjaxTable("DealCloseList", new AjaxTableOptions {
		ActionName = "DealClosingList",
		ControllerName = "Deal"
		, HttpMethod = "GET"
		, SortName = "DealName"
		, Paging = true
		, OnSuccess = "dealClose.onGridSuccess"
		, OnRowClick = "dealClose.onRowClick"
		, Autoload = false
	})%>
	<script id="DUFundsTemplate" type="text/x-jquery-tmpl">
			<tbody>
						{{each(i, df) DealUnderlyingFunds}}
						<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
							<td>
							  <%: Html.InputCheckBox("DealUnderlyingFundId", false , new { @id="chk", @value="${DealUnderlyingFundId}" })%>
							</td>
							<td style="text-align:center">
								${FundName}
							</td>
							<td style="text-align: right">
								<%: Html.TextBox("${DealUnderlyingFundId}_CommittedAmount", "${CommittedAmount}", new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
							</td>
							<td style="text-align: right">
								<%: Html.TextBox("${DealUnderlyingFundId}_GrossPurchasePrice", "${GrossPurchasePrice}", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
							</td>
							<td style="text-align: right">
								<%: Html.TextBox("${DealUnderlyingFundId}_PostRecordDateCapitalCall", "${PostRecordDateCapitalCall}", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
							</td>
							<td style="text-align: right">
								<%: Html.TextBox("${DealUnderlyingFundId}_PostRecordDateDistribution", "${PostRecordDateDistribution}", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
							</td>
							<td style="text-align: right">
								${NetPurchasePrice}
							</td>
							<td>
								<%: Html.Hidden("${DealUnderlyingFundId}_DealClosingId","${DealClosingId}",  new { @id="DealClosingId" })%>
							</td>
						</tr>
						{{/each}}
					</tbody>
					<tfoot>
						<tr>
							<td>
							</td>
							<td style="text-align:center">
								Total
							</td>
							<td style="text-align: right">
								<%: Html.Span("${TotalCA}", new { @id="SpnTotalCA" })%>
							</td>
							<td style="text-align: right">
								<%: Html.Span("${TotalGPP}", new { @id = "SpnTotalGPP" })%>
							</td>
							<td style="text-align: right">
								<%: Html.Span("${TotalPRCC}", new { @id = "SpnTotalPRCC" })%>
							</td>
							<td style="text-align: right">
								<%: Html.Span("${TotalPRCD}", new { @id = "SpnTotalPRCD" })%>
							</td>
							<td style="text-align: right">
								<%: Html.Span("${TotalNPP}", new { @id = "SpnTotalNPP" })%>
							</td>
							<td>
							</td>
						</tr>
					</tfoot>
	</script>
	<script id="DUDirectsTemplate" type="text/x-jquery-tmpl">
				<tbody>
						{{each(i, direct)  DealUnderlyingDirects}}
						<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
							<td>
								<%:Html.InputCheckBox("DealUnderlyingDirectId", false, new {  @value="${DealUnderlyingDirectId}" })%>
							</td>
							<td style="text-align:center">
								${IssuerName}
							</td>
							<td>
								<%: Html.TextBox("${DealUnderlyingDirectId}_NumberOfShares", "${NumberOfShares}", new {  @onkeypress = "return jHelper.isNumeric(event);" })%>
							</td>
							<td>
								<%: Html.TextBox("${DealUnderlyingDirectId}_PurchasePrice", "${PurchasePrice}", new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
							</td>
							<td>
								<%: Html.TextBox("${DealUnderlyingDirectId}_FMV",  "${FMV}", new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
							</td>
							<td>
								<%: Html.Hidden("${DealUnderlyingDirectId}_DealClosingId","${DealClosingId}",  new { @id="DealClosingId" })%>
							</td>
						</tr>
						{{/each}}
					</tbody>
					<tfoot>
						<tr>
							<td>
							</td>
							<td style="text-align:center">
								Total
							</td>
							<td style="text-align:right">
								<%: Html.Span("${TotalNoOfShares}", new { @id="SpnTotalNoOfShares" })%>
							</td>
							<td style="text-align:right">
								<%: Html.Span("${TotalPurchasePrice}", new { @id = "SpnTotalPurchasePrice" })%>
							</td>
							<td style="text-align:right">
								<%: Html.Span("${TotalFMV}", new { @id = "SpnTotalFMV" })%>
							</td>
							<td>
							</td>
						</tr>
					</tfoot>
	</script>
	<script type="text/javascript">
		dealClose.init();
	</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateDealCloseModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal Close
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("DealClose.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.StylesheetLinkTag("deal.css") %>
	<%=Html.StylesheetLinkTag("dealclose.css") %>
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
					CLOSE DEAL
				</div>
			</div>
			<div class="rightcol">
				<div style="margin: 0; padding: 0 10px 0 0; float: left;">
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none;" })%>
				</div>
				<div style="float: left">
					<%: Html.TextBox("Deal", "SEARCH DEAL", new { @id = "Deal", @class = "wm", @style = "width:200px" })%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="dc-box cnt-box-main" id="DealCloseMain" style="display: none">
		<div class="dc-box">
			<div class="section">
				<div class="dealdetail">
					<div style="overflow: hidden;" class="cell">
						<label>
							<%: Html.Span("", new { @id = "SpnFundName" })%></label>
					</div>
					<div style="text-align: left;" class="cell">
						<label>
							Deal No:-<%: Html.Span("", new { @id = "SpnDealNo", @style = "padding-left:10px;" })%></label></div>
					<div style="margin-left: 25px;" class="cell auto">
						<label>
							Deal Name-<%: Html.Span("", new { @id = "SpnDealName", @style = "padding-left:10px;" })%></label></div>
					<div id="LoadingDetail" class="cell auto">
						<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...", new { @id = "SpnGridLoading", @style="display:none;" })%>
					</div>
				</div>
			</div>
		</div>
		<div class="line">
		</div>
		<div class="dc-box" id="ExistingDealClosing" style="display: none">
			<div class="dc-box">
				<div class="section">
					<div class="dealdetail">
						<div class="cell">
							Existing Deal Closes
						</div>
					</div>
				</div>
			</div>
			<div class="dc-box">
				<div class="section">
					<div class="gbox" style="width: 90%;">
						<table id="DealCloseList" class="grid" cellpadding="0" cellspacing="0" border="0"
							style="width: 100%;">
							<thead>
								<tr>
									<th style="display: none;">
									</th>
									<th style="width: 5%" align="left">
										No.
									</th>
									<th style="width: 12%; text-align: left;" align="left">
										Deal Close
									</th>
									<th style="width: 12%" align="left">
										Close Date
									</th>
									<th style="width: 20%; text-align: right;" align="right">
										Total Net Purchase Price
									</th>
									<th align="right">
									</th>
								</tr>
							</thead>
							<tbody>
							</tbody>
						</table>
					</div>
				</div>
			</div>
		</div>
		<div class="dc-box">
			<div class="fundslist">
				<%using (Html.Form(new { @id = "frmDealClose", @onsubmit = "return false;" })) {%>
				<div id="NewDealClose" class="act-box" style="display: block">
					<div id="NDHeaderBox" class="headerbox" style="display: none">
						<div class="title">
							<%:Html.Span("New Deal Close", new { @id = "SpnDCTitlelbl" })%>
						</div>
					</div>
					<div id="NDExpandBox" class="expandheader expandsel" style="display: block;">
						<div class="expandtitle" style="display: block;">
							<div class="expandtitle">
								<%: Html.Span("New Deal Close", new { @id = "SpnDCTitle" })%></div>
						</div>
						<div id="NewDealCloseBtn" class="expandaddbtn" style="display: none;">
							<%: Html.Anchor(Html.Image("adddealclose.png").ToHtmlString(),"javascript:dealClose.add(0);")%></div>
						<div class="rightuarrow">
						</div>
					</div>
					<div id="NDDetail" class="detail" style="display: block">
						<div class="closedetail">
							<div class="editor-label">
								<%: Html.Span("", new { @id = "SpnDealCloseNo" })%>
							</div>
							<div class="editor-field">
								<%: Html.TextBox("CloseDate", "", new { @id = "New_CloseDate" })%>
							</div>
						</div>
						<%: Html.HiddenFor(model => model.DealNumber)%>
						<%: Html.HiddenFor(model => model.DealId)%>
						<%: Html.HiddenFor(model => model.DealClosingId)%>
						<%: Html.HiddenFor(model => model.FundId)%>
						<div class="closetitle">
							Underlying Funds
						</div>
						<div class="dc-box tabledetail">
							<div class="gbox" style="width: 90%">
								<table id="DealUnderlyingFundList" class="grid" cellpadding="0" cellspacing="0" border="0"
									style="width: 100%;">
									<thead>
										<tr>
											<th style="width: 5%">
											</th>
											<th class="lalign" style="width: 15%">
												Fund Name
											</th>
											<th class="ralign" style="width: 15%">
												Commitment Amount
											</th>
											<th class="ralign" style="width: 15%">
												Gross Purchase Price
											</th>
											<th class="ralign" style="width: 15%">
												Post Record Capital Call
											</th>
											<th class="ralign" style="width: 15%">
												Post Record Distribution
											</th>
											<th class="ralign" style="width: 15%">
												Net Purchase Price
											</th>
											<th>
											</th>
										</tr>
									</thead>
								</table>
							</div>
						</div>
						<div class="closetitle">
							Underlying Directs
						</div>
						<div class="dc-box tabledetail">
							<div class="gbox" style="width: 90%">
								<table id="DealUnderlyingDirects" class="grid" cellpadding="0" cellspacing="0" border="0"
									style="width: 100%;">
									<thead>
										<tr>
											<th class="lalign" style="width: 5%">
											</th>
											<th class="lalign" style="width: 15%">
												Direct Name
											</th>
											<th class="lalign" style="width: 15%">
												No. Of Shares
											</th>
											<th class="ralign" style="width: 15%">
												Price
											</th>
											<th class="ralign" style="width: 15%">
												Fair Market Value
											</th>
											<th>
											</th>
										</tr>
									</thead>
								</table>
							</div>
						</div>
						<div class="savefooter">
							<div class="cell">
								<%: Html.ImageButton("CloseDeal.png", new { @style="cursor:pointer", @onclick = "javascript:dealClose.saveDealClose('SpnDCloseLoading');" })%>
							</div>
							<div class="cell">
								<%: Html.Span("", new { @id = "SpnDCloseLoading" } )%>
							</div>
						</div>
					</div>
				</div>
				<%}%>
				<%using (Html.Form(new { @id = "frmFinalDealClose", @onsubmit = "return false;" })) {%>
				<div id="FinalDealClose" class="act-box" style="display: none">
					<div id="FDHeaderBox" class="headerbox">
						<div class="title">
							<%:Html.Span("Final Deal Close")%>
						</div>
					</div>
					<div id="FDExpandBox" class="expandheader expandsel" style="display: none">
						<div class="expandtitle" style="display: block;">
							<div class="expandtitle">
								<%: Html.Span("Final Deal Close")%></div>
						</div>
						<div style="display: block; float: left;">
							<%: Html.TextBox("CloseDate", "", new { @id = "Final_CloseDate" })%></div>
						<div class="rightuarrow">
						</div>
					</div>
					<div class="detail" style="display: none">
						<div class="dc-box">
							<div class="closetitle" style="margin-top: 0">
								All Underlying Funds
							</div>
							<div class="dc-box tabledetail">
								<div class="gbox" style="width: 90%">
									<table id="FinalDealUnderlyingFundList" class="grid" cellpadding="0" cellspacing="0"
										border="0" style="width: 100%;">
										<thead>
											<tr>
												<th class="lalign" style="width: 15%">
													Fund Name
												</th>
												<th class="ralign" style="width: 15%">
													Reallocated Gross Purchase
												</th>
												<th class="ralign" style="width: 15%">
													Post Record Capital Call
												</th>
												<th class="ralign" style="width: 15%">
													Post Record Distribution
												</th>
												<th class="ralign" style="width: 15%">
													Adjusted Cost
												</th>
												<th>
												</th>
											</tr>
										</thead>
									</table>
								</div>
							</div>
						</div>
						<div class="dc-box">
							<div class="closetitle">
								All Underlying Directs
							</div>
							<div class="dc-box tabledetail">
								<div class="gbox" style="width: 90%">
									<table id="FinalDealUnderlyingDirects" class="grid" cellpadding="0" cellspacing="0"
										border="0" style="width: 100%;">
										<thead>
											<tr>
												<th class="lalign" style="width: 15%">
													Direct Name
												</th>
												<th class="lalign" style="width: 15%">
													No. Of Shares
												</th>
												<th class="ralign" style="width: 15%">
													Price
												</th>
												<th class="ralign" style="width: 2%; white-space: nowrap;">
													Adjusted Fair Market Value
												</th>
												<th>
												</th>
											</tr>
										</thead>
									</table>
								</div>
							</div>
						</div>
						<div class="savefooter">
							<div class="cell">
								<%: Html.ImageButton("FCloseDeal.png", new { @style = "cursor:pointer", @onclick = "javascript:dealClose.saveFinalDealClose('SpnFinalDCloseLoading');" })%>
							</div>
							<div class="cell">
								<%: Html.Span("", new { @id = "SpnFinalDCloseLoading" } )%>
							</div>
						</div>
					</div>
				</div>
				<%}%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryDatePicker("New_CloseDate")%><%=Html.jQueryDatePicker("Final_CloseDate")%>
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
		, AlternateRowClass = "arow"
		, RowClass = "row"
	})%>
	<script id="DUFundsTemplate" type="text/x-jquery-tmpl"> 
	<tbody>
		{{each(i, df) DealUnderlyingFunds}}
		<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
			<td class="calign">
				<%: Html.InputCheckBox("DealUnderlyingFundId", false , new { @onclick="javascript:dealClose.editChkRow(this);", @id="chk", @value="${DealUnderlyingFundId}" })%>
			</td>
			<td class="lalign">
				${FundName}
			</td>
			<td class="ralign">
				<%: Html.Span("${CommittedAmount}", new { @class="show money" })%>
				<%: Html.TextBox("${DealUnderlyingFundId}_CommittedAmount", "${CommittedAmount}", new { @class="hide", @id="CommittedAmount", @onkeypress = "return jHelper.isCurrency(event);" })%>
			</td>
			<td class="ralign"><%: Html.Span("${GrossPurchasePrice}", new { @class="show money" })%>
				<%: Html.TextBox("${DealUnderlyingFundId}_GrossPurchasePrice", "${GrossPurchasePrice}", new { @class="hide",@id="GrossPurchasePrice",@onkeyup="javascript:dealClose.calcCloseUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
			</td>
			<td class="ralign"><%: Html.Span("${PostRecordDateCapitalCall}", new { @class="show money" })%>
				<%: Html.TextBox("${DealUnderlyingFundId}_PostRecordDateCapitalCall", "${PostRecordDateCapitalCall}", new { @class="hide",@id="PostRecordDateCapitalCall", @onkeyup="javascript:dealClose.calcCloseUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
			</td>
			<td class="ralign"><%: Html.Span("${PostRecordDateDistribution}", new { @class="show money" })%>
				<%: Html.TextBox("${DealUnderlyingFundId}_PostRecordDateDistribution", "${PostRecordDateDistribution}", new { @class="hide", @id="PostRecordDateDistribution",@onkeyup="javascript:dealClose.calcCloseUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
			</td>
			<td class="ralign">
				<%:Html.Span("${NetPurchasePrice}", new { @id="SpnNPP", @class="money" })%> 
			</td>
			<td class="ralign">
				<%: Html.Hidden("${DealUnderlyingFundId}_DealClosingId","${DealClosingId}",  new { @id="DealClosingId" })%>
				<%: Html.Image("Edit.png", new { @class="gbutton", @onclick = "javascript:dealClose.editRow(this);" })%>
			</td>
		</tr>
		{{/each}}
	</tbody>
	<tfoot>
		<tr>
			<td>
			</td>
			<td class="lalign">
				Total
			</td>
			<td class="ralign">
				<%: Html.Span("${TotalCA}", new { @id="SpnTotalCA" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("${TotalGPP}", new { @id = "SpnTotalGPP" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("${TotalPRCC}", new { @id = "SpnTotalPRCC" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("${TotalPRCD}", new { @id = "SpnTotalPRCD" })%>
			</td>
			<td class="ralign">
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
				<td class="calign">
					<%:Html.InputCheckBox("DealUnderlyingDirectId", false, new { @onclick="javascript:dealClose.editChkRow(this);",  @id="chk", @value="${DealUnderlyingDirectId}" })%>
				</td>
				<td class="lalign">
					${IssuerName}
				</td>
				<td class="lalign"><%: Html.Span("${NumberOfShares}", new { @class="show" })%>
					<%: Html.TextBox("${DealUnderlyingDirectId}_NumberOfShares", "${NumberOfShares}", new { @class="hide", @id="NumberOfShares", @onkeyup="javascript:dealClose.calcCloseUD();",  @onkeypress = "return jHelper.isNumeric(event);" })%>
				</td>
				<td class="ralign"><%: Html.Span("${PurchasePrice}", new { @class="show money" })%>
					<%: Html.TextBox("${DealUnderlyingDirectId}_PurchasePrice", "${PurchasePrice}", new { @class="hide",  @id="PurchasePrice", @onkeyup="javascript:dealClose.calcCloseUD();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign">
					<%:Html.Span("${FormatFMV}", new { @class="show", @id="SpnFMV" })%>   
					<%: Html.TextBox("${DealUnderlyingDirectId}_FMV", "${FMV}", new { @class="hide", @id="FMV", @onkeyup="javascript:dealClose.calcCloseUD();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign">
					<%: Html.Hidden("${DealUnderlyingDirectId}_DealClosingId","${DealClosingId}",  new { @id="DealClosingId" })%>
						<%: Html.Image("Edit.png", new {  @class ="gbutton", @onclick = "javascript:dealClose.editRow(this);" })%>
				</td>
			</tr>
			{{/each}}
		</tbody>
		<tfoot>
			<tr>
				<td class="lalign">Total
				</td>
				<td>
				</td>
				<td class="lalign">
					<%: Html.Span("${TotalNoOfShares}", new { @id="SpnTotalNoOfShares" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${TotalPurchasePrice}", new { @id = "SpnTotalPurchasePrice" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${TotalFMV}", new { @id = "SpnTotalFMV" })%>
				</td>
				<td>
				</td>
			</tr>
		</tfoot>
	</script>
	<script id="FinalDUFundsTemplate" type="text/x-jquery-tmpl"> 
		<tbody>
			{{each(i, df) DealUnderlyingFunds}}
			{{if DealClosingId>0}}
			<tr>
				<td class="lalign">
					${FundName}
				</td>
				<td class="ralign"><%: Html.Span("${ReassignedGPP}", new { @class="show money" })%>
					<%: Html.TextBox("${DealUnderlyingFundId}_ReassignedGPP", "{{if ReassignedGPP>0}}${ReassignedGPP}{{/if}}", new { @class="hide", @id="ReassignedGPP",@onkeyup="javascript:dealClose.calcFlinalCloseUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign"><%: Html.Span("${PostRecordDateCapitalCall}", new { @class="show money" })%>
					<%: Html.TextBox("${DealUnderlyingFundId}_PostRecordDateCapitalCall", "{{if PostRecordDateCapitalCall>0}}${PostRecordDateCapitalCall}{{/if}}", new { @class="hide",@id="PostRecordDateCapitalCall", @onkeyup="javascript:dealClose.calcFlinalCloseUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign"><%: Html.Span("${PostRecordDateDistribution}", new { @class="show money" })%>
					<%: Html.TextBox("${DealUnderlyingFundId}_PostRecordDateDistribution", "{{if PostRecordDateDistribution>0}}${PostRecordDateDistribution}{{/if}}", new { @class="hide", @id="PostRecordDateDistribution",@onkeyup="javascript:dealClose.calcFlinalCloseUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign">
					<%:Html.Span("${AdjustedCost}", new { @id="SpnAJC", @class="money" })%> 
				</td>
				<td class="ralign">
					<%: Html.Image("Edit.png", new {  @class ="gbutton", @onclick = "javascript:dealClose.editRow(this);" })%>
				</td>
			</tr>
			{{/if}}
			{{/each}}
		</tbody>
		<tfoot>
			<tr>
				<td class="lalign">
					Total
				</td>
				<td class="ralign">
					<%: Html.Span("${TotalRGPP}", new { @id = "SpnTotalGPP" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${TotalFinalPRCC}", new { @id = "SpnTotalPRCC" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${TotalFinalPRCD}", new { @id = "SpnTotalPRCD" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${TotalAJC}", new { @id = "SpnTotalAJC" })%>
				</td>
				<td>
				</td>
			</tr>
		</tfoot>
	</script>
	<script id="FinalDUDirectsTemplate" type="text/x-jquery-tmpl">
		<tbody>
			{{each(i, direct)  DealUnderlyingDirects}}
			{{if DealClosingId>0}}
			<tr>
				<td class="lalign">
					${IssuerName}
				</td>
				<td class="lalign"><%: Html.Span("${NumberOfShares}", new { @class="show" })%>
					<%: Html.TextBox("${DealUnderlyingDirectId}_NumberOfShares", "{{if NumberOfShares>0}}${NumberOfShares}{{/if}}", new { @class="hide", @id="NumberOfShares", @onkeyup="javascript:dealClose.calcFlinalCloseUD();",  @onkeypress = "return jHelper.isNumeric(event);" })%>
				</td>
				<td class="ralign"><%: Html.Span("${PurchasePrice}", new { @class="show money" })%>
					<%: Html.TextBox("${DealUnderlyingDirectId}_PurchasePrice", "{{if PurchasePrice>0}}${PurchasePrice}{{/if}}", new { @class="hide",  @id="PurchasePrice", @onkeyup="javascript:dealClose.calcFlinalCloseUD();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign">
					<%:Html.Span("${FormatFMV}", new { @id="SpnFMV",@class="show" })%>   
					<%: Html.TextBox("${DealUnderlyingDirectId}_FMV", "${FMV}", new { @class="hide", @id="FMV", @onkeyup="javascript:dealClose.calcFlinalCloseUD();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign">
					<%: Html.Image("Edit.png", new { @class ="gbutton", @onclick = "javascript:dealClose.editRow(this);" })%>
				</td>
			</tr>
			{{/if}}
			{{/each}}
		</tbody>
		<tfoot>
			<tr>
				<td class="lalign">
					Total
				</td>
				<td class="lalign">
					<%: Html.Span("${TotalFinalNoOfShares}", new { @id="SpnTotalNoOfShares" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${TotalFinalPurchasePrice}", new { @id = "SpnTotalPurchasePrice" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${TotalFinalFMV}", new { @id = "SpnTotalFMV" })%>
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

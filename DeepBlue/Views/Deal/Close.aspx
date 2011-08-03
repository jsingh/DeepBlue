﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateDealCloseModel>" %>

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
				<div class="section" style="margin-top: 0px">
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
		<div class="dc-box closing-det">
			<div class="fundslist">
				<%using (Html.Form(new { @id = "frmDealClose", @onsubmit = "return false;" })) {%>
				<div id="NewDealClose" class="act-box" style="display: block">
					<div id="NDHeaderBox" class="headerbox" style="display: none">
						<div class="title">
							<%:Html.Span("New Deal Close", new { @id = "SpnDCTitlelbl" })%>
						</div>
					</div>
					<div id="NewDealCloseBtn" class="expandaddbtn" style="display: block;">
						<%using (Html.GreenButton(new { @onclick = "javascript:dealClose.add(0);" })) {%>Add
						Deal Close<%}%>
					</div>
					<div id="NDExpandBox" class="expandheader expandsel" style="display: block;">
						<div class="expandtitle" style="display: block;">
							<div class="expandtitle">
								<%: Html.Span("New Deal Close", new { @id = "SpnDCTitle" })%></div>
						</div>
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
						<div class="closetitle">
							<div class="title">
								Underlying Funds</div>
							<%using (Html.BlueButton(new { @onclick = "javascript:dealClose.addDUF('DealUnderlyingFundList');" })) {%>Add
							underlying funds<%}%>
						</div>
						<div class="dc-box tabledetail">
							<div class="gbox" style="width: 90%">
								<table id="DealUnderlyingFundList" class="grid" cellpadding="0" cellspacing="0" border="0"
									style="width: 100%;">
									<thead>
										<tr>
											<th style="width: 3%">
											</th>
											<th class="lalign" style="width: 20%">
												Fund Name
											</th>
											<th class="ralign">
												Commitment Amount
											</th>
											<th class="ralign">
												Gross Purchase Price
											</th>
											<th class="ralign">
												Post Record Capital Call
											</th>
											<th class="ralign">
												Post Record Distribution
											</th>
											<th class="ralign">
												Net Purchase Price
											</th>
											<th class="ralign">
												Unfunded Amount
											</th>
											<th>
											</th>
										</tr>
									</thead>
								</table>
							</div>
						</div>
						<div class="closetitle">
							<div class="title">
								Underlying Directs</div>
							<%using (Html.BlueButton(new { @onclick = "javascript:dealClose.addDUD('DealUnderlyingDirects');" })) {%>Add
							underlying directs<%}%>
						</div>
						<div class="dc-box tabledetail">
							<div class="gbox" style="width: 90%">
								<table id="DealUnderlyingDirects" class="grid" cellpadding="0" cellspacing="0" border="0"
									style="width: 100%;">
									<thead>
										<tr>
											<th class="lalign" style="width: 3%">
											</th>
											<th class="lalign" style="width: 20%">
												Direct Name
											</th>
											<th class="ralign">
												No. Of Shares
											</th>
											<th class="ralign">
												Price
											</th>
											<th class="ralign">
												Fair Market Value
											</th>
											<th style="width: 32%">
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
				<%: Html.Hidden("TotalUnderlyingFunds","")%>
				<%: Html.Hidden("TotalUnderlyingDirects","")%>
				<%: Html.HiddenFor(model => model.DealNumber)%>
				<%: Html.HiddenFor(model => model.DealId)%>
				<%: Html.HiddenFor(model => model.DealClosingId)%>
				<%: Html.HiddenFor(model => model.FundId)%>
				<%}%>
				<%using (Html.Form(new { @id = "frmFinalDealClose", @onsubmit = "return false;" })) {%>
				<div id="FinalDealClose" class="act-box" style="display: block">
					<div id="FDHeaderBox" class="headerbox" style="display: none">
						<div class="title">
							<%:Html.Span("Final Deal Close")%>
						</div>
					</div>
					<div id="FDExpandBox" class="expandheader expandsel" style="display: block">
						<div class="expandtitle" style="display: block;">
							<div class="expandtitle">
								<%: Html.Span("Final Deal Close")%></div>
						</div>
						<div style="display: block; float: left;">
							<%: Html.TextBox("CloseDate", "", new { @id = "Final_CloseDate" })%></div>
						<div class="rightuarrow">
						</div>
					</div>
					<div class="detail" style="display: block">
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
												<th class="ralign" style="width: 15%">
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
				<%: Html.Hidden("TotalUnderlyingFunds","")%>
				<%: Html.Hidden("TotalUnderlyingDirects","")%>
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
	<script id="DUFEditTemplate" type="text/x-jquery-tmpl"> 
		<tr id="row${index}" {{if item.DealUnderlyingFundId>0}}{{if index%2==0}}class="arow"{{else}}class="row"{{/if}}{{/if}}>
			{{if IsFinalClose!=true}}
				<td class="calign">
					{{if item.DealUnderlyingFundId>0}}
						<%: Html.CheckBox("${index}_IsClose",false, new { @onclick="javascript:dealClose.editChkRow(this);", @id="chk", @val = "${item.IsClose}"  })%>			
					{{/if}}
				</td>
			{{/if}}
			<td class="lalign">
				{{if IsFinalClose==true}}
					${item.FundName}
				{{else}}
					<%: Html.Span("${item.FundName}", new { @class="show" })%>
					<%: Html.TextBox("${index}_UnderlyingFundName", "${item.FundName}", new {  @class="hide dealuf", @id="${index}_UnderlyingFundName" })%>
				{{/if}}
			</td>
			{{if IsFinalClose==true}}
				<td class="ralign">
					<%: Html.Span("${item.ReassignedGPP}", new { @class="show money" })%>
					<%: Html.TextBox("${index}_ReassignedGPP", "${checkNullOrZero(item.ReassignedGPP)}", new { @class="hide", @id="ReassignedGPP"
					, @onkeyup="javascript:dealClose.calcFinalCloseUF();"
					, @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
			{{else}}
				<td class="ralign">
					<%: Html.Span("${item.CommittedAmount}", new { @class="show money" })%>
					<%: Html.TextBox("${index}_CommittedAmount", "${checkNullOrZero(item.CommittedAmount)}", new { @class="hide", @id="CommittedAmount"
					, @onkeyup="javascript:dealClose.calcCloseUF();"
					, @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${item.GrossPurchasePrice}", new { @class="show money" })%>
					<%: Html.TextBox("${index}_GrossPurchasePrice", "${checkNullOrZero(item.GrossPurchasePrice)}", new { @class="hide",@id="GrossPurchasePrice"
					, @onkeyup="javascript:dealClose.calcCloseUF();"
					, @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
			{{/if}}
				<td class="ralign">
					<%: Html.Span("${item.PostRecordDateCapitalCall}", new { @class="show money" })%>
					<%: Html.TextBox("${index}_PostRecordDateCapitalCall", "${checkNullOrZero(item.PostRecordDateCapitalCall)}", new { @class="hide"
					,@id="PostRecordDateCapitalCall"
					, @onkeyup="{{if IsFinalClose==true}}javascript:dealClose.calcFinalCloseUF();{{else}}javascript:dealClose.calcCloseUF();{{/if}}"
					, @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("${item.PostRecordDateDistribution}", new { @class="show money" })%>
					<%: Html.TextBox("${index}_PostRecordDateDistribution", "${checkNullOrZero(item.PostRecordDateDistribution)}", new { @class="hide"
					, @id="PostRecordDateDistribution"
					, @onkeyup="{{if IsFinalClose==true}}javascript:dealClose.calcFinalCloseUF();{{else}}javascript:dealClose.calcCloseUF();{{/if}}"
					, @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
			{{if IsFinalClose==true}}
				<td class="ralign">
					<%: Html.Span("${item.AdjustedCost}", new { @id="SpnAC", @class="money" })%>
				</td>
			{{else}}
				<td class="ralign">
					<%:Html.Span("${item.NetPurchasePrice}", new { @id="SpnNPP", @class="money" })%> 
				</td>
				<td class="ralign">
					<%:Html.Span("${item.UnfundedAmount}", new { @id="SpnNPP", @class="show money" })%> 
					<%: Html.TextBox("${index}_UnfundedAmount", "${checkNullOrZero(item.UnfundedAmount)}", new { @class="hide", @id="UnfundedAmount",@onkeyup="javascript:dealClose.calcCloseUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
			{{/if}}
				<td class="ralign">
					<%: Html.Hidden("${index}_DealClosingId","${item.DealClosingId}",  new { @id="DealClosingId" })%>
					<%: Html.Hidden("${index}_DealUnderlyingFundId", "${item.DealUnderlyingFundId}", new { @id = "DealUnderlyingFundId" })%>
					<%: Html.Hidden("${index}_DealId","${item.DealId}")%>
					<%: Html.Hidden("${index}_FundId","${item.FundId}")%>
					<%: Html.Hidden("${index}_RecordDate","${formatDate(item.RecordDate)}")%>
					<%: Html.Hidden("${index}_Percent","${formatDate(item.Percent)}")%>
					<%: Html.Hidden("${index}_UnderlyingFundId", "${item.UnderlyingFundId}", new { @id = "UnderlyingFundId" })%>
					{{if IsFinalClose==true}}
						<%: Html.Hidden("${index}_CommittedAmount", "${checkNullOrZero(item.CommittedAmount)}")%>
						<%: Html.Hidden("${index}_GrossPurchasePrice", "${checkNullOrZero(item.GrossPurchasePrice)}")%>
						<%: Html.Hidden("${index}_NetPurchasePrice", "${checkNullOrZero(item.NetPurchasePrice)}")%>
						<%: Html.Hidden("${index}_UnfundedAmount", "${checkNullOrZero(item.UnfundedAmount)}")%>
						<%: Html.Hidden("${index}_IsClose", "${item.IsClose}")%>
					{{else}}
						<%: Html.Hidden("${index}_FundNAV","${checkNullOrZero(item.FundNAV)}")%>
						<%: Html.Hidden("${index}_ReassignedGPP","${checkNullOrZero(item.ReassignedGPP)}")%>
						<%: Html.Hidden("${index}_AdjustedCost","${checkNullOrZero(item.AdjustedCost)}")%>	
					{{/if}}
					{{if item.DealUnderlyingFundId>0}}
						<%: Html.Image("Edit.png", new { @id="Edit", @class="gbutton", @onclick = "javascript:dealClose.editRow(this);" })%>
					{{else}}
						{{if IsFinalClose!=true}}
							<%: Html.Image("Add.png", new { @id="Add", @onclick = "javascript:dealClose.saveDUF(this);" })%>
						{{/if}}
					{{/if}}
				</td>
		</tr>
	</script>
	<script id="DUFundsTemplate" type="text/x-jquery-tmpl"> 
	<tbody>
		{{each(i, df) DealUnderlyingFunds}}	{{tmpl(getRow((i+1),df,false)) "#DUFEditTemplate"}} {{/each}}
	</tbody>
	<tfoot>
		<tr>
			<td>
			</td>
			<td class="lalign">
				Total
			</td>
			<td class="ralign">
				<%: Html.Span("", new { @id="SpnTotalCA" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("", new { @id = "SpnTotalGPP" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("", new { @id = "SpnTotalPRCC" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("", new { @id = "SpnTotalPRCD" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("", new { @id = "SpnTotalNPP" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("", new { @id = "SpnTotalUFA" })%>
			</td>
			<td>
			</td>
		</tr>
	</tfoot>
	</script>
	<script id="DUDEditTemplate" type="text/x-jquery-tmpl">
		<tr id="row${index}" {{if item.DealUnderlyingDirectId>0}}{{if index%2==0}}class="arow"{{else}}class="row"{{/if}}{{/if}}>
			{{if IsFinalClose!=true}}
				<td class="calign">
					{{if item.DealUnderlyingDirectId>0}}
						<%: Html.CheckBox("${index}$IsClose",false, new { @onclick="javascript:dealClose.editChkRow(this);", @id="chk", @val = "${item.IsClose}" })%>	
					{{/if}}
				</td>
			{{/if}}
			<td class="lalign">
				{{if IsFinalClose==true}}
					${item.IssuerName}
				{{else}}
					<%: Html.Span("${item.IssuerName}", new { @class="show" })%>
					<%: Html.TextBox("${index}$IssuerName", "${item.IssuerName}", new {  @class="hide dealud", @id="${index}$IssuerName" })%>
				{{/if}}
			</td>
			<td class="ralign">
				<%: Html.Span("${item.NumberOfShares}", new { @class="show" })%>
				<%: Html.TextBox("${index}$NumberOfShares", "${checkNullOrZero(item.NumberOfShares)}", new { @class="hide", @id="NumberOfShares"
				,  @onkeyup="{{if IsFinalClose==true}}javascript:dealClose.calcFinalCloseUD();{{else}}javascript:dealClose.calcCloseUD();{{/if}}"
				,  @onkeypress = "return jHelper.isNumeric(event);" })%>
			</td>
			<td class="ralign">
				<%: Html.Span("${item.PurchasePrice}", new { @class="show money" })%>
				<%: Html.TextBox("${index}$PurchasePrice", "${checkNullOrZero(item.PurchasePrice)}", new { @class="hide",  @id="PurchasePrice"
				, @onkeyup="{{if IsFinalClose==true}}javascript:dealClose.calcFinalCloseUD();{{else}}javascript:dealClose.calcCloseUD();{{/if}}"
				, @onkeypress = "return jHelper.isCurrency(event);" })%>
			</td>
			{{if IsFinalClose==true}}
				<td class="ralign">
					<%:Html.Span("${checkNullOrZero(item.AdjustedFMV)}", new { @class="show", @id="SpnFMV" })%>   
					<%: Html.TextBox("${index}$AdjustedFMV", "${checkNullOrZero(item.AdjustedFMV)}", new { @class="hide", @id="AdjustedFMV"
					, @onkeyup="javascript:dealClose.calcFinalCloseUD();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
			{{else}}
				<td class="ralign">
					<%:Html.Span("${checkNullOrZero(item.FMV)}", new { @class="show", @id="SpnFMV" })%>   
					<%: Html.TextBox("${index}$FMV", "${checkNullOrZero(item.FMV)}", new { @class="hide", @id="FMV", @onkeyup="javascript:dealClose.calcCloseUD();", @onkeypress = "return jHelper.isCurrency(event);" })%>
				</td>
			{{/if}}
			<td class="ralign">
				<%: Html.Hidden("${index}$DealClosingId","${item.DealClosingId}",  new { @id="DealClosingId" })%>
				<%: Html.Hidden("${index}$DealUnderlyingDirectId","${item.DealUnderlyingDirectId}",  new { @id="DealUnderlyingDirectId" })%>
				<%: Html.Hidden("${index}$DealId","${item.DealId}")%>
				<%: Html.Hidden("${index}$FundId","${item.FundId}")%>
				<%: Html.Hidden("${index}$TaxCostBase","${checkNullOrZero(item.TaxCostBase)}")%>
				<%: Html.Hidden("${index}$TaxCostDate","${formatDate(item.TaxCostDate)}")%>
				<%: Html.Hidden("${index}$RecordDate","${formatDate(item.RecordDate)}")%>
				<%: Html.Hidden("${index}$Percent","${formatDate(item.Percent)}")%>
				<%: Html.Hidden("${index}$IssuerId", "${item.IssuerId}", new { @id = "IssuerId" })%>
				<%: Html.Hidden("${index}$SecurityTypeId", "${item.SecurityTypeId}", new { @id = "SecurityTypeId" })%>
				<%: Html.Hidden("${index}$SecurityId", "${item.SecurityId}", new { @id = "SecurityId" })%>
				{{if IsFinalClose==true}}
					<%: Html.Hidden("${index}$FMV","${checkNullOrZero(item.FMV)}")%>
				{{else}}
					<%: Html.Hidden("${index}$AdjustedFMV","${checkNullOrZero(item.AdjustedFMV)}")%>
				{{/if}}
				{{if item.DealUnderlyingDirectId>0}}
				<%: Html.Image("Edit.png", new { @id="Edit", @class="gbutton", @onclick = "javascript:dealClose.editRow(this);" })%>
				{{else}}
				<%: Html.Image("Add.png", new { @id="Add", @onclick = "javascript:dealClose.saveDUD(this);" })%>
				{{/if}}
			</td>
		</tr>
	</script>
	<script id="DUDirectsTemplate" type="text/x-jquery-tmpl">
		<tbody>
			{{each(i, direct)  DealUnderlyingDirects}}
				{{tmpl(getRow((i+1),direct,false)) "#DUDEditTemplate"}}
			{{/each}}
		</tbody>
		<tfoot>
			<tr>
				<td>
				</td>
				<td class="lalign">Total
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id="SpnTotalNoOfShares" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id = "SpnTotalPurchasePrice" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id = "SpnTotalFMV" })%>
				</td>
				<td>
				</td>
			</tr>
		</tfoot>
	</script>
	<script id="FinalDUFundsTemplate" type="text/x-jquery-tmpl"> 
		<tbody>
			{{each(i, duf) DealUnderlyingFunds}}
			{{if duf.DealClosingId>0}}
				{{tmpl(getRow((i+1),duf,true)) "#DUFEditTemplate"}}
			{{/if}}
			{{/each}}
		</tbody>
		<tfoot>
			<tr>
				<td class="lalign">
					Total
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id = "SpnTotalGPP" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id = "SpnTotalPRCC" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id = "SpnTotalPRCD" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id = "SpnTotalAJC" })%>
				</td>
				<td>
				</td>
			</tr>
		</tfoot>
	</script>
	<script id="FinalDUDirectsTemplate" type="text/x-jquery-tmpl">
		<tbody>
			{{each(i, direct)  DealUnderlyingDirects}}
			{{if direct.DealClosingId>0}}
				{{tmpl(getRow((i+1),direct,true)) "#DUDEditTemplate"}}
			{{/if}}
			{{/each}}
		</tbody>
		<tfoot>
			<tr>
				<td class="lalign">
					Total
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id="SpnTotalNoOfShares" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id = "SpnTotalPurchasePrice" })%>
				</td>
				<td class="ralign">
					<%: Html.Span("", new { @id = "SpnTotalFMV" })%>
				</td>
				<td>
				</td>
			</tr>
		</tfoot>
	</script>
	<script type="text/javascript">
		dealClose.init();
	</script>
	<%if (Model.DealId > 0) {%>
	<script type="text/javascript"> $(document).ready(function() {  dealClose.selectDeal(<%=Model.DealId%>); } );</script>
	<%}%>
</asp:Content>

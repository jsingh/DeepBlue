<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.tooltip.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.fileuploader.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.JavascriptInclueTag("jquery.filedrop.js")%>
	<%=Html.JavascriptInclueTag("FileUploadScript.js")%>
	<!--[if lt IE 9]>
	<%=Html.JavascriptInclueTag("html5.js")%>
	<![endif]-->
	<%=Html.JavascriptInclueTag("Deal.js")%>
	<%=Html.JavascriptInclueTag("Footer.js")%>
	<%=Html.JavascriptInclueTag("DealExpense.js")%>
	<%=Html.JavascriptInclueTag("DealUnderlyingFund.js")%>
	<%=Html.JavascriptInclueTag("DealUnderlyingDirect.js")%>
	<%=Html.JavascriptInclueTag("DealDocument.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<%if (ViewData["PageName"] == "CreateNewDeal") {%>
	<%using (Html.Div(new { @id = "DealFundList", @class = "navigation", @style = (ViewData["PageName"] == "CreateNewDeal" ? "display:block" : "display:none") })) {%>
	<div class="heading">
		<div class="leftcol">
			<span class="title">INVESTMENTS</span><span class="arrow"></span><span class="pname">CREATE
				NEW DEAL</span></div>
		<div class="rightcol">
			<%: Html.TextBox("M_Fund", "SEARCH AMBERBROOK FUND", new { @class = "wm", @style = "width:200px", @id = "M_Fund" })%>
		</div>
	</div>
	<%}%>
	<%}%>
	<%if (ViewData["PageName"] == "DealList") {%>
	<%using (Html.Div(new { @id = "ModifyDealBox", @class = "navigation", @style = (ViewData["PageName"] == "DealList" ? "display:block" : "display:none") })) {%>
	<div id="modifyDealUL" class="heading">
		<div class="leftcol">
			<span class="title">INVESTMENTS</span><span class="arrow"></span><span class="pname">
				MODIFY DEAL</span></div>
		<div class="rightcol">
			<%: Html.TextBox("SearchDealName", "SEARCH DEAL", new { @class="wm", @id = "SearchDealName", @style = "width: 200px" })%>
			<div class="editor-field" style="width: auto">
				<a href="javascript:deal.seeFullDeal();" style="text-decoration: underline">See full
					list</a></div>
		</div>
	</div>
	<%}%>
	<%}%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="DealMain" style="display: none">
		<%using (Html.Form(new { @id = "AddNewDeal", @onsubmit = "return deal.saveDeal();" })) {%>
		<div id="NewDeal" class="content">
		</div>
		<%}%>
		<div id="DealExpenses" class="content">
		</div>
		<div id="DealDocuments" class="content">
		</div>
		<%using (Html.Form(new { @id = "frmSellerInfo", @onsubmit = "return deal.saveSellerInfo(this);" })) {%>
		<div id="DealSellerInfo" class="content">
		</div>
		<%}%>
		<div id="DealUnderlyingFunds" class="content">
		</div>
		<div id="DealUnderlyingDirects" class="content">
		</div>
		<div class="editor-field auto" id="SaveDealBox">
			<div class="cell auto" style="float: right;">
				<%: Html.ImageButton("cnewdeal_active.png", new { @id = "btnDummySaveDeal", onclick = "javascript:deal.saveDeal();" })%></div>
			<div class="cell" style="float: right; text-align: right;">
				<%: Html.Span("", new { id = "UpdateLoading" })%></div>
		</div>
	</div>
	<div id="UpdateTargetId" style="display: none">
	</div>
	<div id="FullDealList" style="display: none; padding-left: 10px;">
		<% Html.RenderPartial("TBoxTop"); %>
		<table cellpadding="0" cellspacing="0" border="0" id="DealList" class="grid">
			<thead>
				<tr>
					<th sortname="DealId" style="width: 10%;" align="center">
						ID
					</th>
					<th sortname="DealName" style="width: 50%">
						Deal Name
					</th>
					<th sortname="FundName" style="width: 40%">
						Fund Name
					</th>
				</tr>
			</thead>
		</table>
		<% Html.RenderPartial("TBoxBottom"); %>
	</div>
	<div id="FullFundList" style="display: none; padding-left: 10px;">
		<% Html.RenderPartial("TBoxTop"); %>
		<table cellpadding="0" cellspacing="0" border="0" id="FundList" class="grid">
			<thead>
				<tr>
					<th sortname="FundId" style="width: 10%;" align="center">
						ID
					</th>
					<th sortname="FundName" style="width: 90%">
						Fund Name
					</th>
				</tr>
			</thead>
		</table>
		<% Html.RenderPartial("TBoxBottom"); %>
	</div>
	<div class="tooltip">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("DealList", new FlexigridOptions { ActionName = "DealFundList", ControllerName = "Deal", HttpMethod = "GET", SortName = "DealName", Paging = true, OnSuccess = "deal.onDealListSuccess", Autoload = false, ResizeWidth = false,   Width = 600, BoxStyle = false })%>
	<%=Html.jQueryFlexiGrid("FundList", new FlexigridOptions { ActionName = "List", ControllerName = "Fund", HttpMethod = "GET", SortName = "FundName", Paging = true, OnSuccess = "deal.onFundListSuccess", Autoload = false, ResizeWidth = false,   Width = 600, BoxStyle = false })%>
	<%= Html.jQueryAutoComplete("M_Fund", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { deal.selectFund(ui.item.id,ui.item.label); }"
	})%>
	<%= Html.jQueryAutoComplete("SearchDealName", new AutoCompleteOptions {
																	  Source = "/Deal/FindDeals", MinLength = 1, 
																	  OnSelect = "function(event, ui) {  deal.loadDeal(ui.item.id); }"
	})%>
	<script type="text/javascript">		deal.init();fileUpload.fileExt=<%=Model.DocumentFileExtensions%>;</script>
	<%if (ViewData["PageName"] == "CreateNewDeal" && Model.FundId > 0) {%>
	<script type="text/javascript">$(document).ready(function() { deal.selectFund(<%=Model.FundId%>,'<%=Model.FundName%>'); });</script>
	<%}%>
	<%if (ViewData["PageName"] == "DealList") {%>
	<script type="text/javascript">$(document).ready(function() {  deal.loadDeal(<%=Model.DealId%>); });</script>
	<%}%>
	<%using (Html.jQueryTemplateScript("DealTemplate")) {%>
	<% Html.RenderPartial("DealDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("DealExpenseTemplate")) {%>
	<% Html.RenderPartial("DealExpenseDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("DealSellerInfoTemplate")) {%>
	<% Html.RenderPartial("SellerDetail", Model.SellerInfo); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("DealDocumentTemplate")) {%>
	<% Html.RenderPartial("DealDocumentDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("DealUnderlyingFundTemplate")) {%>
	<% Html.RenderPartial("DealUnderlyingFundDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("DealUnderlyingDirectTemplate")) {%>
	<% Html.RenderPartial("DealUnderlyingDirectDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("DealExpensesRowTemplate")) {%>
	<tr id="DealExpense_${DealClosingCostId}">
		<td class="lalign">
			<%: Html.DropDownList("DealClosingCostTypeId", Model.DealClosingCostTypes, new { @class="hide", @val = "${DealClosingCostTypeId}" })%>
			<%: Html.Span("${Description}",new { @class = "show" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${Amount}",new { @class = "show" , @id = "SpnAmount" })%>
			<%: Html.TextBox("Amount", "${Amount}", new {  @class="hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="lalign">
			<%: Html.Span("${Date}",new { @class = "show", @id = "SpnDate" })%>
			<%: Html.TextBox("Date", "${Date}", new {  @class="hide datefield", @id = "${DealClosingCostId}_DealExpenseDate" })%>
		</td>
		<td class="ralign">
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new { @style = "display:none;", @id = "spnAjax" })%>
			<%: Html.Image("Edit.png", new { @class="gbutton", @onclick = "javascript:deal.editDealExpense(this);" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new {  @class="gbutton", @onclick = "javascript:deal.deleteDealExpense(${DealClosingCostId},this);" })%>
			<%: Html.Hidden("DealClosingCostId","${DealClosingCostId}")%>
		</td>
	</tr>
	<%}%>
	<%using (Html.jQueryTemplateScript("UnderlyingFundsRowTemplate")) {%>
	<tr id="UnderlyingFund_${DealUnderlyingFundId}">
		<td style="text-align: center; display: none;">
			<%: Html.Span("", new { @id = "SpnIndex" }) %>
		</td>
		<td class="lalign">
			{{if DealClosingId>0}}<%: Html.Image("close.gif")%>{{/if}}
		</td>
		<td class="lalign">
			<%: Html.Span("${FundName}",new { @class = "show" })%>
			<%: Html.TextBox("UnderlyingFund", "${FundName}", new { @class = "hide tooltiptxt", @id="UnderlyingFund",  @top = "198",  @style="width:78%" })%>
			<%: Html.Hidden("UnderlyingFundId","${UnderlyingFundId}")%>
		</td>
		<td class="ralign">
			<%: Html.Span("${GrossPurchasePrice}", new { @class = "show money", @id = "SpnGrossPurchasePrice" })%>
			<%: Html.TextBox("GrossPurchasePrice","${GrossPurchasePrice}",new { @class = "hide",  @id = "GrossPurchasePrice", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${FundNAV}", new { @class = "show", @id = "SpnPercent" })%>
			<%: Html.TextBox("FundNAV", "${FundNAV}",new { @class = "hide", @onkeyup="javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${CommittedAmount}", new { @class = "show money", @id = "SpnCommittedAmount" })%>
			<%: Html.TextBox("CommittedAmount","${CommittedAmount}",new { @class = "hide", @onkeyup="javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${UnfundedAmount}", new { @class = "show money", @id = "SpnUnfundedAmount" })%>
			<%: Html.TextBox("UnfundedAmount","${UnfundedAmount}",new { @class = "hide", @onkeyup="javascript:deal.calcDUF();",  @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="lalign">
			<%: Html.Span("${RecordDate}", new { @class = "show dispdate", @id = "SpnRecordDate" })%>
			<%: Html.TextBox("RecordDate", "${RecordDate}",new { @class = "hide datefield", @id = "${DealUnderlyingFundId}_RecordDate" })%>
		</td>
		<td class="ralign">
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
			<%: Html.Image("Edit.png", new { @class="gbutton", @onclick = "javascript:deal.editUnderlyingFund(this);" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class="gbutton", @onclick = "javascript:deal.deleteUnderlyingFund(${DealUnderlyingFundId},this);" })%>
			<%: Html.Hidden("DealUnderlyingFundId","${DealUnderlyingFundId}")%>
			<%: Html.Hidden("Percent","${Percent}")%>
			<%: Html.Hidden("DealClosingId","${DealClosingId}")%>
			<%: Html.Hidden("ReassignedGPP","${ReassignedGPP}")%>
			<%: Html.Hidden("PostRecordDateCapitalCall","${PostRecordDateCapitalCall}")%>
			<%: Html.Hidden("PostRecordDateDistribution","${PostRecordDateDistribution}")%>
			<%: Html.Hidden("NetPurchasePrice","${NetPurchasePrice}")%>
			<%: Html.Hidden("AdjustedCost","${AdjustedCost}")%>
		</td>
	</tr>
	<%}%>
	<%using (Html.jQueryTemplateScript("UnderlyingDirectsRowTemplate")) {%>
	<tr id="UnderlyingDirect_${DealUnderlyingDirectId}">
		<td style="text-align: center; display: none;">
			<%: Html.Span("", new { @id = "SpnIndex" }) %>
		</td>
		<td class="lalign">
			{{if DealClosingId>0}}<%: Html.Image("close.gif")%>{{/if}}
		</td>
		<td class="lalign">
			<%: Html.Span("${IssuerName}",new { @class = "show" })%>
			<%: Html.TextBox("Issuer", "${IssuerName}", new { @class = "hide tooltiptxt", @id = "Issuer",   @top = "198", @style = "width:78%" })%>
			<%: Html.Hidden("IssuerId", "${IssuerId}")%>
			<%: Html.Hidden("SecurityTypeId","${SecurityTypeId}")%>
			<%: Html.Hidden("SecurityId", "${SecurityId}")%>
		</td>
		<td class="ralign">
			<%: Html.Span("${NumberOfShares}", new { @class = "show", @id = "SpnNumberOfShares" })%>
			<%: Html.TextBox("NumberOfShares", "${NumberOfShares}",new { @class = "hide", @onkeyup="javascript:deal.calcDUD();", @id="NumberOfShares", @onkeydown = "return jHelper.isNumeric(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${PurchasePrice}", new { @class = "show money", @id = "SpnPurchasePrice", @val="${PurchasePrice}" })%>
			<%: Html.TextBox("PurchasePrice","${PurchasePrice}",new { @class = "hide",@id="PurchasePrice", @onkeyup="javascript:deal.calcDUD();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${FMV}", new { @class = "show money", @id = "SpnFMV", @val="${FMV}" })%>
			<%: Html.TextBox("FMV","${FMV}",new { @class = "hide", @id="FMV", @onkeyup="javascript:deal.calcDUD();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${TaxCostBase}", new { @class = "show", @id = "SpnTaxCostBase", @val="${TaxCostBase}" })%>
			<%: Html.TextBox("TaxCostBase","${TaxCostBase}",new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="lalign">
			<%: Html.Span("${TaxCostDate}", new { @class = "show", @id = "SpnTaxCostDate" })%>
			<%: Html.TextBox("TaxCostDate", "",new { @class = "hide datefield", @id = "${DealUnderlyingDirectId}_DirectTaxCostDate" })%>
		</td>
		<td class="lalign">
			<%: Html.Span("${RecordDate}", new { @class = "show", @id = "SpnRecordDate" })%>
			<%: Html.TextBox("RecordDate", "",new { @class = "hide datefield", @id = "${DealUnderlyingDirectId}_DirectRecordDate" })%>
		</td>
		<td class="ralign">
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
			<%: Html.Image("Edit.png", new { @class="gbutton", @onclick = "javascript:deal.editUnderlyingDirect(this);" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class="gbutton", @onclick = "javascript:deal.deleteUnderlyingDirect(${DealUnderlyingDirectId},this);" })%>
			<%: Html.Hidden("DealUnderlyingDirectId","${DealUnderlyingDirectId}")%>
			<%: Html.Hidden("Percent","${Percent}")%>
			<%: Html.Hidden("DealClosingId","${DealClosingId}")%>
			<%: Html.Hidden("AdjustedFMV","${AdjustedFMV}")%>
		</td>
	</tr>
	<%}%>
</asp:Content>

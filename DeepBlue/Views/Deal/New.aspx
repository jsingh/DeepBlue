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
			<span class="title">INVESTMENTS</span><span class="arrow"></span><span class="pname">CREATE NEW DEAL</span></div>
		<div class="rightcol">
			<%: Html.TextBox("M_Fund", "SEARCH FUND", new { @class = "wm", @style = "width:200px", @id = "M_Fund" })%>
		</div>
	</div>
	<%}%>
	<%}%>
	<%if (ViewData["PageName"] == "DealList") {%>
	<%using (Html.Div(new { @id = "ModifyDealBox", @class = "navigation", @style = (ViewData["PageName"] == "DealList" ? "display:block" : "display:none") })) {%>
	<div id="modifyDealUL" class="heading">
		<div class="leftcol">
			<span class="title">INVESTMENTS</span><span class="arrow"></span><span class="pname"> MODIFY DEAL</span></div>
		<div class="rightcol">
			<div style="float: left">
				<%: Html.TextBox("SearchDealName", "SEARCH DEAL", new { @class="wm", @id = "SearchDealName", @style = "width: 200px" })%>
			</div>
			<div class="editor-field" style="width: auto">
				<a href="javascript:deal.seeFullDeal();" style="text-decoration: underline">See full list</a></div>
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
			<div class="cell auto" style="float: right;">
				<%: Html.ImageButton("Import-Excel_active.png", new { @id = "btnImportDeal", onclick = "javascript:$('#ExcelImport').dialog('open');" })%>
			</div>
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
	<div id="ExcelImport">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.JavascriptInclueTag("ImportDealExcel.js")%>
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
			<%: Html.TextBox("Amount", "${formatNumber(Amount)}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="lalign">
			<%: Html.Span("${Date}",new { @class = "show", @id = "SpnDate" })%>
			<%: Html.TextBox("Date", "${Date}", new {  @class="hide datefield", @id = "${DealClosingCostId}_DealExpenseDate" })%>
		</td>
		<td class="ralign">
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new { @style = "display:none;", @id = "spnAjax" })%>
			<%: Html.Image("Edit.png", new { @class = "show gbutton editbtn", @onclick = "javascript:deal.editDealExpense(this);" })%>
			<%: Html.Image("Save_active.png", new { @class="hide", @style="float:right", @onclick = "javascript:deal.saveDE(this);" })%>
			<%: Html.Image("largedel.png", new {  @class="show gbutton", @onclick = "javascript:deal.deleteDealExpense(${DealClosingCostId},this);" })%>
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
			<%: Html.TextBox("UnderlyingFund", "${FundName}", new { @class = "hide", @id="UnderlyingFund",  @top = "198" })%>
			<%: Html.Hidden("UnderlyingFundId","${UnderlyingFundId}")%>
		</td>
		<td class="ralign">
			<%: Html.Span("${GrossPurchasePrice}", new { @class = "show money", @id = "SpnGrossPurchasePrice" })%>
			<%: Html.TextBox("GrossPurchasePrice", "${formatNumber(GrossPurchasePrice)}", new { @class = "hide", @id = "GrossPurchasePrice", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${FundNAV}", new { @class = "show money", @id = "SpnPercent" })%>
			<%: Html.TextBox("FundNAV", "${formatNumber(FundNAV)}",new { @class = "hide", @onkeyup="javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="lalign">
			<%: Html.Span("${EffectiveDate}", new { @class = "show dispdate", @id = "SpnEffectiveDate" })%>
			<%: Html.TextBox("EffectiveDate", "${EffectiveDate}", new { @style = "width:85px;", @class = "hide datefield", @id = "${DealUnderlyingFundId}_EffectiveDate" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${CommittedAmount}", new { @class = "show money", @id = "SpnCommittedAmount" })%>
			<%: Html.TextBox("CommittedAmount", "${formatNumber(CommittedAmount)}", new { @class = "hide", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${UnfundedAmount}", new { @class = "show money", @id = "SpnUnfundedAmount" })%>
			<%: Html.TextBox("UnfundedAmount", "${formatNumber(UnfundedAmount)}", new { @class = "hide", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="lalign">
			<%: Html.Span("${RecordDate}", new { @class = "show dispdate", @id = "SpnRecordDate" })%>
			<%: Html.TextBox("RecordDate", "${RecordDate}", new { @style = "width:85px;", @class = "hide datefield", @id = "${DealUnderlyingFundId}_RecordDate" })%>
		</td>
		<td class="ralign">
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
			<%: Html.Image("Edit.png", new { @class="show gbutton editbtn", @onclick = "javascript:deal.editUnderlyingFund(this);" })%>
			<%: Html.Image("Save_active.png", new { @class = "hide", @onclick = "javascript:deal.saveUF(this);" })%>
			<%: Html.Image("largedel.png", new { @class="show gbutton", @onclick = "javascript:deal.deleteUnderlyingFund(${DealUnderlyingFundId},this);" })%>
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
			<%: Html.TextBox("Issuer", "${IssuerName}", new { @class = "hide", @id = "Issuer",   @top = "198" })%>
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
			<%: Html.TextBox("PurchasePrice", "${formatNumber(PurchasePrice)}", new { @class = "hide", @id = "PurchasePrice", @onkeyup = "javascript:deal.calcDUD();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${FMV}", new { @class = "show money", @id = "SpnFMV", @val="${FMV}" })%>
			<%: Html.TextBox("FMV", "${formatNumber(FMV)}", new { @class = "hide", @id = "FMV", @onkeyup = "javascript:deal.calcDUD();", @onkeydown = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${TaxCostBase}", new { @class = "show money", @id = "SpnTaxCostBase", @val="${TaxCostBase}" })%>
			<%: Html.TextBox("TaxCostBase", "${formatNumber(TaxCostBase)}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
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
			<%: Html.Image("Edit.png", new { @class="show gbutton editbtn", @onclick = "javascript:deal.editUnderlyingDirect(this);" })%>
			<%: Html.Image("Save_active.png", new { @class = "hide", @onclick = "javascript:deal.saveUD(this);" })%>
			<%: Html.Image("largedel.png", new { @class="show gbutton", @onclick = "javascript:deal.deleteUnderlyingDirect(${DealUnderlyingDirectId},this);" })%>
			<%: Html.Hidden("DealUnderlyingDirectId","${DealUnderlyingDirectId}")%>
			<%: Html.Hidden("Percent","${Percent}")%>
			<%: Html.Hidden("DealClosingId","${DealClosingId}")%>
			<%: Html.Hidden("AdjustedFMV","${AdjustedFMV}")%>
		</td>
	</tr>
	<%}%>
	<%using (Html.jQueryTemplateScript("ExcelImprtTemplate")) {%>
	<div class="import-box">
		<%using (Html.Form(new { @id = "frmUploadExcel", @onsubmit = "return false" })) { %>
		<div class="editor-label" style="width: 110px;">
			&nbsp;</div>
		<div class="editor-field" style="text-align: right; font-size: 11px;">
			<%:Html.Anchor("Sample Excel","/Files/ImportSamples/Deal.xls", new { @target = "_blank", @style = "color:blue" })%>
		</div>
		<div class="editor-label" style="width: 110px;">
			<%: Html.Label("File")%></div>
		<div class="editor-field">
			<%: Html.File("UploadFile", new { @id = "UploadFile" })%>
		</div>
		<div class="editor-label" style="width: 100px">
			<%: Html.Span("", new { @id = "SpnUELoading" })%>
		</div>
		<div class="editor-label" style="clear: right; width: auto;">
			<%: Html.Image("Upload_active.png", new { @onclick = "javascript:importDealExcel.uploadExcel();" })%></div>
		<div class="editor-field">
			<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:$('#ExcelImport').dialog('close');" })%></div>
		<%}%>
	</div>
	<div id="ImportExcel">
	</div>
	<div id="ProgressBar">
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ImportExcelTemplate")) {%>
	<br />
	<div class="tabbg">
		<%using (Html.Tab(new { @id = "DealDetailTab", @class = "section-tab-sel", @onclick = "javascript:importDealExcel.selectTab('DD',this);" })) {%>Deal<%}%>
		<%using (Html.Tab(new { @id = "DealExpenseTab", @class = "section-tab", @onclick = "javascript:importDealExcel.selectTab('DE',this);" })) {%>Deal Expense<%}%>
		<%using (Html.Tab(new { @id = "DealUFTab", @class = "section-tab", @onclick = "javascript:importDealExcel.selectTab('DUF',this);" })) {%>Deal Underlying Fund<%}%>
		<%using (Html.Tab(new { @id = "DealUDTab", @class = "section-tab", @onclick = "javascript:importDealExcel.selectTab('DUD',this);" })) {%>Deal Underlying Direct<%}%>
	</div>
	<div class="clear">
		&nbsp;</div>
	<div class="dealimportsection" id="DealDetailBox">
		<div class="formbox">
			<%using (Html.Form(new { @id = "frm", @onsubmit = "return false" })) { %>
			<div class="editor-label">
				<%: Html.Label("Excel Tab")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DealTableName", new List<SelectListItem>() {
	 new  SelectListItem { Text = "--Select Excel Tab--", Value = " " }
}, new { @exceltabname = "DealDetail", @class = "ddltable", @onchange = "javascript:importDealExcel.selectExcelTab(this);" })%></div>
			<div class="editor-label">
				<%: Html.Label("DealName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DealName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("FundName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FundName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("PartnerName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("PartnerName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Purchase Type")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("PurchaseType", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Contact Name")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ContactName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Contact Title")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ContactTitle", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Contact Phone Number")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ContactPhoneNumber", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Contact Email")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ContactEmail", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Contact WebAddress")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ContactWebAddress", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Contact Notes")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ContactNotes", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Seller Type")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("SellerType", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("SellerName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("SellerName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Seller Contact Name")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("SellerContactName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Seller Phone Number")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("SellerPhoneNumber", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Seller Email")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("SellerEmail", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Seller Fax")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("SellerFax", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="clear">
				&nbsp;</div>
			<%: Html.Hidden("TotalRows", "${TotalRows}")%>
			<%: Html.Hidden("SessionKey", "${SessionKey}")%>
			<%}%>
		</div>
		<div class="statusbox">
		</div>
	</div>
	<div class="dealimportsection" id="DealExpenseBox" style="display: none">
		<div class="formbox">
			<%using (Html.Form(new { @id = "frm", @onsubmit = "return false" })) { %>
			<div class="editor-label">
				<%: Html.Label("Excel Tab")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DealExpenseTableName", new List<SelectListItem>() {
			 new  SelectListItem { Text = "--Select Excel Tab--", Value = " " }
		}, new { @exceltabname = "DealExpense", @class = "ddltable", @onchange = "javascript:importDealExcel.selectExcelTab(this);" })%></div>
			<div class="editor-label">
				<%: Html.Label("DealName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DealName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("FundName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FundName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Description")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Description", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Amount")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Amount", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Date")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Date", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="clear">
				&nbsp;</div>
			<%: Html.Hidden("TotalRows", "${TotalRows}")%>
			<%: Html.Hidden("SessionKey", "${SessionKey}")%>
			<%}%>
		</div>
		<div class="statusbox">
		</div>
	</div>
	<div class="dealimportsection" id="DealUFBox" style="display: none">
		<div class="formbox">
			<%using (Html.Form(new { @id = "frm", @onsubmit = "return false" })) { %><div class="editor-label">
				<%: Html.Label("Excel Tab")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DealUnderlyingFundTableName", new List<SelectListItem>() {
			 new  SelectListItem { Text = "--Select Excel Tab--", Value = " " }
		}, new { @exceltabname = "DealUnderlyingFund", @class = "ddltable", @onchange = "javascript:importDealExcel.selectExcelTab(this);" })%></div>
			<div class="editor-label">
				<%: Html.Label("DealName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DealName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("FundName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FundName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("UnderlyingFundName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("UnderlyingFundName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("GrossPurchasePrice")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("GrossPurchasePrice", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("FundNav")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FundNav", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("EffectiveDate")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("EffectiveDate", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("CapitalCommitment")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("CapitalCommitment", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("UnfundedAmount")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("UnfundedAmount", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("RecordDate")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("RecordDate", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="clear">
				&nbsp;</div>
			<%: Html.Hidden("TotalRows", "${TotalRows}")%>
			<%: Html.Hidden("SessionKey", "${SessionKey}")%>
			<%}%></div>
		<div class="statusbox">
		</div>
	</div>
	<div class="dealimportsection" id="DealUDBox" style="display: none">
		<div class="formbox">
			<%using (Html.Form(new { @id = "frm", @onsubmit = "return false" })) { %><div class="editor-label">
				<%: Html.Label("Excel Tab")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DealUnderlyingDirectTableName", new List<SelectListItem>() {
			 new  SelectListItem { Text = "--Select Excel Tab--", Value = " " }
		}, new { @exceltabname = "DealUnderlyingDirect", @class = "ddltable", @onchange = "javascript:importDealExcel.selectExcelTab(this);" })%></div>
			<div class="editor-label">
				<%: Html.Label("DealName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DealName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("FundName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FundName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("CompanyName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("CompanyName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("SecurityType")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("SecurityType", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Symbol")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Symbol", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("NoOfShares")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("NoOfShares", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("PurchasePrice")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("PurchasePrice", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("FairMarketValue")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FairMarketValue", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("TaxCostBasisPerShare")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("TaxCostBasisPerShare", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("TaxCostDate")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("TaxCostDate", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("RecordDate")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("RecordDate", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="clear">
				&nbsp;</div>
			<%: Html.Hidden("TotalRows", "${TotalRows}")%>
			<%: Html.Hidden("SessionKey", "${SessionKey}")%>
			<%}%></div>
		<div class="statusbox">
		</div>
	</div>
	<div class="save-box">
		<div class="editor-label">
			<%: Html.Image("Save_active.png", new {  @onclick = "javascript:importDealExcel.import(this);" })%></div>
		<div class="editor-field">
			<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:$('#ExcelImport').dialog('close');" })%></div>
		<div class="clear">
			&nbsp;</div>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ImportExcelResultTemplate")) {%>
	<div class="editor-label">
		<%: Html.Label("Total")%></div>
	<div class="editor-field">
		<%: Html.Span("${TotalRows}", new { @id = "spntotal" })%></div>
	<div class="editor-label">
		<%: Html.Label("Success")%></div>
	<div class="editor-field">
		<%: Html.Span("${SuccessRows}", new { @id = "spnsuccess" })%></div>
	<div class="editor-label">
		<%: Html.Label("Errors")%></div>
	<div class="editor-field">
		<%: Html.Span("${ErrorRows}", new { @id = "spnerrors" })%></div>
	<div class="editor-field">
		<%: Html.Span("", new { @id = "spnerrorexcel", @style="color:red" })%></div>
	<div class='prs-bar'>
		<div class='total-rows'>
			Rows ${CompletedRows} Of ${TotalRows}</div>
		<div class="status-bar">
			<div class='loading-status' style='width: ${Percent}%;'>
			</div>
		</div>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ProgressBarTemplate")) {%>
	<div class='prs-bar'>
		<div class='total-rows'>
			Rows ${CompletedRows} Of ${TotalRows}</div>
		<div class="status-bar">
			<div class='loading-status' style='width: ${Percent}%;'>
			</div>
		</div>
	</div>
	<%}%>
</asp:Content>

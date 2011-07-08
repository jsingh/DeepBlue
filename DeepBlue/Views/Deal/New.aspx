<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.JavascriptInclueTag("Deal.js")%>
	<%=Html.JavascriptInclueTag("DealExpense.js")%>
	<%=Html.JavascriptInclueTag("DealUnderlyingFund.js")%>
	<%=Html.JavascriptInclueTag("DealUnderlyingDirect.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="DealMain">
		<% Html.EnableClientValidation(); %>
		<%using (Ajax.BeginForm("Create", null, new AjaxOptions {
		UpdateTargetId = "UpdateTargetId",
		HttpMethod = "Post",
		OnBegin = "deal.onCreateDealBegin",
		OnSuccess = "deal.onCreateDealSuccess"
	}, new { @id = "AddNewDeal" })) {%>
		<div id="NewDeal" class="content">
		</div>
		<%: Html.ValidationMessageFor(model => model.FundId) %>
		<%: Html.ValidationMessageFor(model => model.DealName) %>
		<%: Html.ValidationMessageFor(model => model.DealNumber) %>
		<%: Html.ValidationMessageFor(model => model.PurchaseTypeId) %>
		<%}%>
		<div id="DealExpenses" class="content">
		</div>
		<%using (Html.Form(new { @id = "frmDocumentInfo", @name = "frmDocumentInfo", @action = "/Deal/CreateDocument", @method = "POST", @onsubmit = "return deal.uploadDocument();" })) {%>
		<div id="DealDocuments" class="content">
		</div>
		<%}%>
		<%using (Html.Form(new { @id = "frmSellerInfo", @onsubmit = "return deal.saveSellerInfo(this);" })) {%>
		<div id="DealSellerInfo" class="content">
		</div>
		<%}%>
		<div id="DealUnderlyingFunds" class="content">
		</div>
		<div id="DealUnderlyingDirects" class="content">
		</div>
		<div class="editor-field auto" id="SaveDealBox" style="display: none; float: right;
			width: auto;">
			<div class="cell">
				<%: Html.Span("", new { id = "UpdateLoading" })%></div>
			<div class="cell auto">
				<%: Html.ImageButton("cnewdeal.png", new { @id = "btnDummySaveDeal",  onclick = "javascript:deal.saveDeal();" })%></div>
		</div>
	</div>
	<div id="UpdateTargetId" style="display: none">
	</div>
	<div id="modifyDealUL" class="heading">
		<div class="leftcol">
			<span class="title">INVESTMENTS</span><span class="arrow"></span><span class="pname">
				MODIFY DEAL</span></div>
		<div class="rightcol">
			<%: Html.TextBox("SearchDealName", "SEARCH DEAL", new { @class="wm", @id = "SearchDealName", @style = "width: 200px" })%>&nbsp;<a
				href="javascript:deal.seeFullDeal();" style="text-decoration: underline">See full
				list</a>
		</div>
	</div>
	<div id="FullDealList">
		<table cellpadding="0" cellspacing="0" border="0" id="DealList">
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
	</div>
	<div id="FullFundList">
		<table cellpadding="0" cellspacing="0" border="0" id="FundList">
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
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("DealList", new FlexigridOptions { ActionName = "DealList", ControllerName = "Deal", HttpMethod = "GET", SortName = "DealName", Paging = true, OnSuccess = "deal.onDealListSuccess", Autoload = false, ResizeWidth=false, RowsLength=20 })%>
	<%=Html.jQueryFlexiGrid("FundList", new FlexigridOptions { ActionName = "List", ControllerName = "Fund", HttpMethod = "GET", SortName = "FundName", Paging = true, OnSuccess = "deal.onFundListSuccess", Autoload = false, ResizeWidth=false, RowsLength=20 })%>
	<script type="text/javascript">		deal.init();</script>
	<script id="FundListTemplate" type="text/x-jquery-tmpl">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTMENTS</span><span class="arrow"></span><span class="pname">CREATE
					NEW DEAL</span></div>
			<div class="rightcol">
				<%: Html.TextBox("M_Fund","SEARCH FUND", new { @class="wm", @style="width:200px", @id="M_Fund" })%>
			</div>
		</div>
	</script>
	<script id="DealTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("DealDetail", Model); %>
	</script>
	<script id="DealExpenseTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("DealExpenseDetail", Model); %>
	</script>
	<script id="DealSellerInfoTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("SellerDetail", Model.SellerInfo); %>
	</script>
	<script id="DealDocumentTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("DealDocumentDetail",Model); %>
	</script>
	<script id="DealUnderlyingFundTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("DealUnderlyingFundDetail",Model); %>
	</script>
	<script id="DealUnderlyingDirectTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("DealUnderlyingDirectDetail",Model); %><div class="line"></div>
	</script>
	<script id="DealExpensesRowTemplate" type="text/x-jquery-tmpl"> 
		<tr id="DealExpense_${DealClosingCostId}">
		<td class="lalign">
			<%: Html.DropDownList("DealClosingCostTypeId", Model.DealClosingCostTypes, new { @class="hide", @val = "${DealClosingCostTypeId}" })%>
			<%: Html.Span("${Description}",new { @class = "show" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${Amount}",new { @class = "show" , @id = "SpnAmount" })%>
			<%: Html.TextBox("Amount", "${Amount}", new {  @class="hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="calign">
			<%: Html.Span("${Date}",new { @class = "show", @id = "SpnDate" })%>
			<%: Html.TextBox("Date", "${Date}", new {  @class="hide datefield", @id = "${DealClosingCostId}_DealExpenseDate" })%>
		</td>
		<td class="ralign">
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new { @style = "display:none;", @id = "spnAjax" })%>
			<%: Html.Image("Edit.png", new { @class="gbutton", @onclick = "javascript:deal.editDealExpense(this);" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new {  @class="gbutton", @onclick = "javascript:deal.deleteDealExpense(${DealClosingCostId},this);" })%>
			<%: Html.Hidden("DealClosingCostId","${DealClosingCostId}")%>
		</td>
	</tr>
	</script>
	<script id="UnderlyingFundsRowTemplate" type="text/x-jquery-tmpl">
	<tr id="UnderlyingFund_${DealUnderlyingFundId}">
		<td style="text-align:center; display:none;">
			<%: Html.Span("", new { @id = "SpnIndex" }) %>
		</td>
		<td class="lalign">
			{{if DealClosingId>0}}<%: Html.Image("close.gif")%>{{/if}}
		</td>
		<td class="lalign">
			<%: Html.Span("${FundName}",new { @class = "show" })%>
			<%: Html.TextBox("UnderlyingFund", "${FundName}", new { @class = "hide", @id="UnderlyingFund", @style="width:78%" })%>
			<%: Html.Hidden("UnderlyingFundId","${UnderlyingFundId}")%>
		</td>
        <td class="ralign">
			<%: Html.Span("${GrossPurchasePrice}", new { @class = "show money", @id = "SpnGrossPurchasePrice" })%>
			<%: Html.TextBox("GrossPurchasePrice","${GrossPurchasePrice}",new { @class = "hide",  @id = "GrossPurchasePrice", @onkeyup = "javascript:deal.calcDUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>	
		<td class="lalign">
			<%: Html.Span("${FundNAV}", new { @class = "show money", @id = "SpnPercent" })%>
			<%: Html.TextBox("FundNAV", "${FundNAV}",new { @class = "hide", @onkeyup="javascript:deal.calcDUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${CommittedAmount}", new { @class = "show money", @id = "SpnCommittedAmount" })%>
			<%: Html.TextBox("CommittedAmount","${CommittedAmount}",new { @class = "hide", @onkeyup="javascript:deal.calcDUF();", @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>	
		<td class="ralign">
			<%: Html.Span("${UnfundedAmount}", new { @class = "show money", @id = "SpnUnfundedAmount" })%>
			<%: Html.TextBox("UnfundedAmount","${UnfundedAmount}",new { @class = "hide", @onkeyup="javascript:deal.calcDUF();",  @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>
		<td style="text-align:center;">
			<%: Html.Span("${RecordDate}", new { @class = "show dispdate", @id = "SpnRecordDate" })%>
			<%: Html.TextBox("RecordDate", "${RecordDate}",new { @class = "hide datefield", @id = "${DealUnderlyingFundId}_RecordDate" })%>
		</td>
		<td style="text-align: right;" nowrap>
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
			<%: Html.Image("Edit.png", new { @class="gbutton", @onclick = "javascript:deal.editUnderlyingFund(this);" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class="gbutton", @onclick = "javascript:deal.deleteUnderlyingFund(${DealUnderlyingFundId},this);" })%>
			<%: Html.Hidden("DealUnderlyingFundId","${DealUnderlyingFundId}")%>
		</td>
	</tr>
	</script>
	<script id="UnderlyingDirectsRowTemplate" type="text/x-jquery-tmpl"> 
	<tr id="UnderlyingDirect_${DealUnderlyingDirectId}">
		<td style="text-align: center;display:none;">
			<%: Html.Span("", new { @id = "SpnIndex" }) %>
		</td>
		<td class="lalign">
			{{if DealClosingId>0}}<%: Html.Image("close.gif")%>{{/if}}
		</td>
		<td class="lalign">
			<%: Html.Span("${IssuerName}",new { @class = "show" })%>
			<%: Html.TextBox("Issuer", "${IssuerName}", new { @class = "hide", @id = "Issuer", @style = "width:78%" })%>
			<%: Html.Hidden("IssuerId", "${IssuerId}")%>
			<%: Html.Hidden("SecurityTypeId","${SecurityTypeId}")%>
			<%: Html.Hidden("SecurityId", "${SecurityId}")%>
		</td>
		<td class="lalign">
			<%: Html.Span("${NumberOfShares}", new { @class = "show", @id = "SpnNumberOfShares" })%>
			<%: Html.TextBox("NumberOfShares", "${NumberOfShares}",new { @class = "hide", @onkeyup="javascript:deal.calcDUD();", @id="NumberOfShares", @onkeypress = "return jHelper.isNumeric(event);" })%>
		</td>
		<td class="ralign">
			<%: Html.Span("${PurchasePrice}", new { @class = "show money", @id = "SpnPurchasePrice", @val="${PurchasePrice}" })%>
			<%: Html.TextBox("PurchasePrice","${PurchasePrice}",new { @class = "hide",@id="PurchasePrice", @onkeyup="javascript:deal.calcDUD();", @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>	
        <td class="ralign">
			<%: Html.Span("${FMV}", new { @class = "show money", @id = "SpnFMV", @val="${FMV}" })%>
			<%: Html.TextBox("FMV","${FMV}",new { @class = "hide", @id="FMV", @onkeyup="javascript:deal.calcDUD();", @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>	
		<td class="lalign">
			<%: Html.Span("${TaxCostBase}", new { @class = "show", @id = "SpnTaxCostBase", @val="${TaxCostBase}" })%>
			<%: Html.TextBox("TaxCostBase","${TaxCostBase}",new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>
		<td class="calign">
			<%: Html.Span("${TaxCostDate}", new { @class = "show", @id = "SpnTaxCostDate" })%>
			<%: Html.TextBox("TaxCostDate", "",new { @class = "hide datefield", @id = "${DealUnderlyingDirectId}_DirectTaxCostDate" })%>
		</td>
		<td class="calign">
			<%: Html.Span("${RecordDate}", new { @class = "show", @id = "SpnRecordDate" })%>
			<%: Html.TextBox("RecordDate", "",new { @class = "hide datefield", @id = "${DealUnderlyingDirectId}_DirectRecordDate" })%>
		</td>
		<td style="text-align: right" nowrap>
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
			<%: Html.Image("Edit.png", new { @class="gbutton", @onclick = "javascript:deal.editUnderlyingDirect(this);" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class="gbutton", @onclick = "javascript:deal.deleteUnderlyingDirect(${DealUnderlyingDirectId},this);" })%>
			<%: Html.Hidden("DealUnderlyingDirectId","${DealUnderlyingDirectId}")%>
		</td>
	</tr>
	</script>
</asp:Content>

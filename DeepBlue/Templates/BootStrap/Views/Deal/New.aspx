<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/BootStrap/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Models.Admin" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%List<DeepBlue.Models.Admin.EntityMenuModel> _Menus = MenuHelper.GetMenus();
   EntityMenuModel dealMenu = null;
   List<EntityMenuModel> dealSubMenus = null;
   int currentMenuID = 0;
   int.TryParse(Request["menuid"], out currentMenuID);
   dealMenu = (from menu in MenuHelper.GetMenus()
			   where menu.MenuName == "Deals"
			   select menu
													   ).FirstOrDefault();
   dealSubMenus = (from menu in MenuHelper.GetMenus()
				   where menu.ParentMenuID == dealMenu.MenuID
				   select menu
													   ).ToList();
	%><br />
	<div class="container-fluid">
		<div class="row-fluid">
			<div class="span2 omega">
				<%if (dealSubMenus != null) {%>
				<div class="tabbable tabs-left">
					<ul class="nav nav-tabs">
						<%foreach (var menu in dealSubMenus) {%>
						<li <%if(menu.MenuID == currentMenuID){%>class="active" <%}%>>
							<% string url = menu.URL;
		  if (url.Contains("javascript") == false) {
			  if (menu.URL.Contains("?")) {
				  url += "&menuid=" + menu.MenuID;
			  }
			  else {
				  url += "?menuid=" + menu.MenuID;
			  }
		  }
							%>
							<a data-toggle="tab" href="<%=url%>">
								<%=menu.DisplayName%></a> </li>
						<%}%>
					</ul>
				</div>
				<%}%>
			</div>
			<div class="span10 alpha">
				<div class="page-header">
					<div class="pull-left">
						<h3>
							<%if (ViewData["PageName"] == "CreateNewDeal") {%>CREATE NEW DEAL<%}
		 else {%>MODIFY DEAL<%} %></h3>
					</div>
					<div class="pull-right">
						<%if (ViewData["PageName"] == "CreateNewDeal") {%>
						<%: Html.TextBox("M_Fund", Model.FundName, new { @class = "wm search-query input-large", @placeholder = "SEARCH FUND" })%>
						<%}
		else {%>
						<%: Html.TextBox("SearchDealName", "", new { @class = "wm search-query input-large", @placeholder = "SEARCH DEAL", @id = "SearchDealName" })%>
						<a href="javascript:deal.seeFullDeal();">See full list</a>
						<%}%>
					</div>
					<div class="clear">
						&nbsp;</div>
				</div>
				<div id="DealMain">
					<%using (Html.Form(new { @id = "AddNewDeal", @onsubmit = "return deal.saveDeal();" })) {%>
					<div id="NewDeal" class="content">
					</div>
					<%}%>
					<div id="DealExpenses" class="content">
					</div>
					<div id="DealDocuments" class="content">
					</div>
					<div id="DealSellerInfo" class="content">
					</div>
					<div id="DealUnderlyingFunds" class="content">
					</div>
					<div id="DealUnderlyingDirects" class="content">
					</div>
					<div class="clear">
						&nbsp;</div>
					<br />
					<div class="control-group pull-right">
						<div class="controls">
							<button onclick="javascript:deal.saveDeal();" class="btn btn-success">
								Create New Deal</button>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/Deal.js")%>" type="text/javascript"></script>
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/DealExpense.js")%>" type="text/javascript"></script>
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/DealDocument.js")%>" type="text/javascript"></script>
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/DealUnderlyingFund.js")%>" type="text/javascript"></script>
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/DealUnderlyingDirect.js")%>" type="text/javascript"></script>
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/jqTransform.js")%>" type="text/javascript"></script>
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/jquery.filedrop.js")%>" type="text/javascript"></script>
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/FileUploadScript.js")%>" type="text/javascript"></script>
	<%= Html.jQueryAutoComplete("M_Fund", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { deal.selectFund(ui.item.id,ui.item.label); }"
	})%>
	<%= Html.jQueryAutoComplete("SearchDealName", new AutoCompleteOptions {
																	  Source = "/Deal/FindDeals", MinLength = 1, 
																	  OnSelect = "function(event, ui) {  deal.loadDeal(ui.item.id); }"
	})%>
	<%=Html.jQueryFlexiGrid("DealList", new FlexigridOptions { ActionName = "DealFundList", ControllerName = "Deal", HttpMethod = "GET", SortName = "DealName", Paging = true, OnSuccess = "deal.onDealListSuccess", Autoload = false, ResizeWidth = false,   Width = 600, BoxStyle = false })%>
	<%=Html.jQueryFlexiGrid("FundList", new FlexigridOptions { ActionName = "List", ControllerName = "Fund", HttpMethod = "GET", SortName = "FundName", Paging = true, OnSuccess = "deal.onFundListSuccess", Autoload = false, ResizeWidth = false,   Width = 600, BoxStyle = false })%>
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
			<%: Html.TextBox("UnderlyingFund", "${FundName}", new { @class = "hide", @id="UnderlyingFund",  @top = "198"  })%>
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
			<%: Html.TextBox("RecordDate", "${RecordDate}",new { @class = "hide datefield", @id = "${DealUnderlyingFundId}_RecordDate" })%>
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
	<tr id="UnderlyingFund_Edit_${DealUnderlyingFundId}">
		<td class="calign" colspan="8">
			<%using (Html.Form(new { @class = "form-horizontal", @onsubmit = "return false" })) {%>
			<%: Html.Span("", new { @id = "SpnIndex" })%>
			<div class="control-group pull-left">
				<label class="control-label">
					Fund</label>
				<div class="controls">
					<%: Html.TextBox("UnderlyingFund", "${FundName}", new { @class = "hide input-large", @id = "UnderlyingFund", @top = "198"})%>
					<%: Html.Hidden("UnderlyingFundId", "0")%>
				</div>
			</div>
			<div class="control-group pull-left">
				<label class="control-label">
					Gross Purchase Price</label>
				<div class="controls">
					<%: Html.TextBox("GrossPurchasePrice", "${formatNumber(GrossPurchasePrice)}", new { @class = "hide input-large", @id = "GrossPurchasePrice", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
				</div>
			</div>
			<div class="clear">
				&nbsp;</div>
			<div class="control-group pull-left">
				<label class="control-label">
					Fund NAV</label>
				<div class="controls">
					<%: Html.TextBox("FundNAV", "${formatNumber(FundNAV)}",new { @class = "hide input-large", @onkeyup="javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
				</div>
			</div>
			<div class="control-group pull-left">
				<label class="control-label">
					Committed Amount</label>
				<div class="controls">
					<%: Html.TextBox("CommittedAmount", "${formatNumber(CommittedAmount)}", new { @class = "hide input-large", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
				</div>
			</div>
			<div class="clear">
				&nbsp;</div>
			<div class="control-group pull-left">
				<label class="control-label">
					Unfunded Amount</label>
				<div class="controls">
					<%: Html.TextBox("UnfundedAmount", "${formatNumber(UnfundedAmount)}", new { @class = "hide input-large", @onkeyup = "javascript:deal.calcDUF();", @onkeydown = "return jHelper.isCurrency(event);" })%>
				</div>
			</div>
			<div class="control-group pull-left">
				<label class="control-label">
					Record Date</label>
				<div class="controls">
					<%: Html.TextBox("RecordDate", "${RecordDate}",new { @class = "hide datefield input-large", @id = "${DealUnderlyingFundId}_RecordDate" })%>
				</div>
			</div>
			<div class="clear">
				&nbsp;</div>
			<div class="control-group">
				<button id="save" class="btn btn-primary input-small" onclick="javascript:deal.saveUF(this);" data-loading-text="Saving...">
					Save</button>
				<button class="btn btn-danger input-small" onclick="javascript:deal.deleteUnderlyingFund(${DealUnderlyingFundId},this);" data-loading-text="Deleting...">
					Delete</button>
			</div>
			<%: Html.Hidden("DealUnderlyingFundId","${DealUnderlyingFundId}")%>
			<%: Html.Hidden("Percent","${Percent}")%>
			<%: Html.Hidden("DealClosingId","${DealClosingId}")%>
			<%: Html.Hidden("ReassignedGPP","${ReassignedGPP}")%>
			<%: Html.Hidden("PostRecordDateCapitalCall","${PostRecordDateCapitalCall}")%>
			<%: Html.Hidden("PostRecordDateDistribution","${PostRecordDateDistribution}")%>
			<%: Html.Hidden("NetPurchasePrice","${NetPurchasePrice}")%>
			<%: Html.Hidden("AdjustedCost","${AdjustedCost}")%>
			<%}%>
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
			<%: Html.TextBox("Issuer", "${IssuerName}", new { @class = "hide", @id = "Issuer",   @top = "198"  })%>
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
			<%: Html.Span("${TaxCostBase}", new { @class = "show", @id = "SpnTaxCostBase", @val="${TaxCostBase}" })%>
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
</asp:Content>

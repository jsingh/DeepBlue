<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create New Deal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%= Html.JavascriptInclueTag("Deal.js")%><%=Html.StylesheetLinkTag("deal.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="DealMain">
		<div style="clear: both">
			<% Html.EnableClientValidation(); %>
			<%using (Ajax.BeginForm("Create", null, new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "deal.onCreateDealBegin",
		 OnSuccess = "deal.onCreateDealSuccess"
	 }, new { @id = "AddNewDeal" })) {%>
			<div id="NewDeal">
			</div>
			<%: Html.ValidationMessageFor(model => model.FundId) %>
			<%: Html.ValidationMessageFor(model => model.DealName) %>
			<%: Html.ValidationMessageFor(model => model.DealNumber) %>
			<%: Html.ValidationMessageFor(model => model.PurchaseTypeId) %>
			<%}%>
		</div>
		<div id="DealExpenses">
		</div>
		<div id="DealDocuments">
		</div>
		<%using (Html.Form(new { @id = "frmSellerInfo", @onsubmit = "return deal.saveSellerInfo(this);" })) {%>
		<div id="DealSellerInfo">
		</div>
		<%}%>
		<div id="DealUnderlyingFunds">
		</div>
		<div id="DealUnderlyingDirects">
		</div>
		<div id="UpdateTargetId" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		deal.init();</script>
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
	<tr class='emptyrow'><td colspan="5">&nbsp;</td></tr><tr id="DealExpense_${DealClosingCostId}">
		<td>
			<%: Html.DropDownList("DealClosingCostTypeId", Model.DealClosingCostTypes, new { @class="hide", @val = "${DealClosingCostTypeId}" })%>
			<%: Html.Span("${Description}",new { @class = "show" })%>
		</td>
		<td>
			<%: Html.Span("${Amount}",new { @class = "show" , @id = "SpnAmount" })%>
			<%: Html.TextBox("Amount", "${Amount}", new {  @class="hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
		</td>
		<td>
			<%: Html.Span("${Date}",new { @class = "show", @id = "SpnDate" })%>
			<%: Html.TextBox("Date", "${Date}", new {  @class="hide datefield", @id = "${DealClosingCostId}_DealExpenseDate" })%>
		</td>
		<td>
			<%: Html.Image("Editbtn.png", new { @onclick = "javascript:deal.editDealExpense(this);" })%>&nbsp;&nbsp;<%: Html.Image("Delete_Btn.png", new { @onclick = "javascript:deal.deleteDealExpenses(${DealClosingCostId},this);" })%>
			<%: Html.Hidden("DealClosingCostId","${DealClosingCostId}")%>
		</td>
		<td class="blank">
			<%using(Html.Div(new { @style="width:75px" })){%>
		<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new { @style = "display:none;", @id = "spnAjax" })%>
		<%}%>
		</td>
	</tr>
	</script>
</asp:Content>

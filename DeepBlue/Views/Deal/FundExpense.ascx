<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.FundExpenseModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="FLE">
	<%using (Html.Form(new { @id = "frmFundExpense", @onsubmit = "return dealActivity.SaveFundLevelExpense(this);" })) {%>
	<div class="cell">
		<%:Html.Span("", new { @id = "FLE_FundName" })%>
	</div>
	<div class="cell" style="margin-left: 20px">
		<%:Html.LabelFor(model => model.Amount)%>
	</div>
	<div class="cell">
		<%: Html.EditorFor(model => model.Amount, new { @onkeypress = "return jHelper.isCurrency(event);" })%></div>
	<div class="cell">
		<%: Html.LabelFor(model => model.FundExpenseTypeId)%></div>
	<div class="cell">
		<%: Html.DropDownListFor(model => model.FundExpenseTypeId, Model.FundExpenseTypes)%></div>
	<div class="cell">
		<%: Html.ImageButton("tick.png", new { @class = "tickbtn" })%></div>
	<div class="cell">
		<%: Html.Span("", new { @id = "SpnFLELoading" }) %>
	</div>
	<%:Html.HiddenFor(model => model.FundExpenseId)%>
	<%:Html.HiddenFor(model => model.FundId, new { @id = "FLE_FundId" })%>
	<%}%>
</div>

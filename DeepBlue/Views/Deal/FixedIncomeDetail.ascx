<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.FixedIncomeDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="fixincomediv">
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FaceValue) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FaceValue", "${FaceValue}") %>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.ISINO) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("ISINO", "${ISINO}") %>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.Maturity) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Maturity", "${Maturity}", new { @class = "datefield", @id = "FI_Maturity" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.IssuedDate) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("IssuedDate", "${IssuedDate}", new { @class = "datefield", @id = "FI_IssuedDate" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.CouponInformation) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("CouponInformation", "${CouponInformation}")%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.CurrencyId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("FI_CurrencyId", Model.Currencies, new { @val = "${CurrencyId}" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.Frequency) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Frequency", "${Frequency}")%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.FirstCouponDate) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FirstCouponDate", "${FirstCouponDate}", new { @class = "datefield", @id = "FI_FirstCouponDate" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.FirstAccrualDate) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FirstAccrualDate", "${FirstAccrualDate}", new { @class = "datefield", @id = "FI_FirstAccrualDate" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FixedIncomeTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownList("FixedIncomeTypeId", Model.FixedIncomeTypes, new { @val = "${FixedIncomeTypeId}" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.IndustryId) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("FI_Industry", "${Industry}", new { @id = "FI_Industry", @style = "width:157px;" })%>
		<%: Html.Hidden("FI_IndustryId", "${IndustryId}")%>
	</div>
	<%: Html.Hidden("FixedIncomeId","${FixedIncomeId}")%>
</div>

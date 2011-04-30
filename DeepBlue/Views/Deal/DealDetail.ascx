<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="editor-label auto">
	<div class="cell">
		<h4>
			<a href="#" id="lnkFundName">${FundName}</a></h4>
	</div>
	<div class="cell" style="text-align: left">
		<%: Html.LabelFor(model => model.DealNumber) %>&nbsp;${DealNumber}</div>
	<div class="cell auto" style="margin-left: 25px">
		<%: Html.LabelFor(model => model.DealName) %>&nbsp;<%: Html.TextBox("DealName","${DealName}") %></div>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.FundId) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("FundName", "${FundName}", new { @id = "FundName", @onblur="javascript:deal.checkFund(this);" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.ContactId) %>
</div>
<div class="editor-field">
	<%: Html.DropDownListFor(model => model.ContactId, Model.Contacts, new { @val = "${ContactId}" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.PurchaseTypeId) %>
</div>
<div class="editor-field">
	<%: Html.DropDownListFor(model => model.PurchaseTypeId, Model.PurchaseTypes, new { @val = "${PurchaseTypeId}" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.IsPartnered) %>
</div>
<div class="editor-field rdo" style="width: auto">
	<div class="cell" style="width: 224px">
		<%: Html.RadioButton("IsPartnered","true", false, new { @id = "IsPartneredYes", @style = "border:0px;", @onclick = "javascript:deal.selectPartner(!this.checked);" })%>
		&nbsp;Yes&nbsp;
		<%: Html.RadioButton("IsPartnered", "false", true, new { @id = "IsPartneredNo", @style = "border:0px;", @onclick = "javascript:deal.selectPartner(this.checked);" })%>
		&nbsp;No
	</div>
	<div class="cell auto" id="divPartnerName" style="display: none;">
		<%: Html.LabelFor(model => model.PartnerName)%>&nbsp;<%: Html.TextBox("PartnerName","${PartnerName}")%></div>
</div>
<div class="editor-label">
</div>
<div class="editor-field auto">
	<div class="cell auto">
		<%: Html.ImageButton("Save.png", new { @id = "btnSaveDeal", style = "width: 73px; height: 23px;border:0;", onclick = "return deal.onDealSubmit('AddNewDeal');" })%></div>
	<div class="cell auto">
		<%: Html.Span("", new { id = "UpdateLoading" })%>
	</div>
</div>
<%: Html.Hidden("FundId", "${FundId}", new { @id = "FundId" })%>
<%: Html.Hidden("DealId", "${DealId}", new { @id = "DealId" })%>
<%: Html.Hidden("DealNumber", "${DealNumber}", new { @id = "DealNumber" })%>

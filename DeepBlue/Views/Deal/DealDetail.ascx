<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="section">
	<div class="dealdetail" style="width: 90%; padding-left: 5px;">
		<div class="cell" style="overflow: hidden">
			${FundName}
		</div>
		<div class="cell" style="text-align: left">
			<%: Html.LabelFor(model => model.DealNumber) %>&nbsp;${DealNumber}</div>
		<div class="cell auto" style="margin-left: 25px">
			<%: Html.LabelFor(model => model.DealName) %>&nbsp;<%: Html.TextBox("DealName", "${DealName}", new { @style = "width:184px" })%></div>
		{{if IsDealClose==true}}
		<div class="cell auto" style="float: right;">
			<%: Html.Image("greenclosedeal.png")%>
		</div>
		{{/if}}
	</div>
</div>
<div class="line">
</div>
<div class="section" style="padding: 15px 0px;">
	<div class="editor-label">
		<%: Html.LabelFor(model => model.ContactId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownListFor(model => model.ContactId, Model.Contacts, new { @val = "${ContactId}" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.IsPartnered) %>
	</div>
	<div class="editor-field rdo" style="width: auto; clear: right;">
		<div class="cell" style="width: 240px">
			<%: Html.RadioButton("IsPartnered","true", false, new { @id = "IsPartneredYes", @style = "border:0px;", @onclick = "javascript:deal.selectPartner(!this.checked);" })%>
			&nbsp;Yes&nbsp;
			<%: Html.RadioButton("IsPartnered", "false", true, new { @id = "IsPartneredNo", @style = "border:0px;", @onclick = "javascript:deal.selectPartner(this.checked);" })%>
			&nbsp;No
		</div>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.PurchaseTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.DropDownListFor(model => model.PurchaseTypeId, Model.PurchaseTypes, new { @val = "${PurchaseTypeId}" })%>
	</div>
	<div class="editor-label" id="divPartnerName">
		<div class="editor-label">
			<%: Html.LabelFor(model => model.PartnerName)%></div>
		<div class="editor-field">
			<%: Html.TextBox("PartnerName","${PartnerName}")%></div>
	</div>
	<%: Html.Hidden("FundId", "${FundId}", new { @id = "FundId" })%>
	<%: Html.Hidden("DealId", "${DealId}", new { @id = "DealId" })%>
	<%: Html.Hidden("DealNumber", "${DealNumber}", new { @id = "DealNumber" })%>
</div>

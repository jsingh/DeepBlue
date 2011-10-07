<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="section">
	<div class="dealdetail">
		<div class="cell" style="overflow: hidden">
			${FundName}
		</div>
		<div class="cell" style="width:159px;">
			<%: Html.LabelFor(model => model.DealNumber) %>&nbsp;${DealNumber}</div>
		<div class="cell auto">
			<%: Html.LabelFor(model => model.DealName)%><%: Html.TextBox("DealName", "${DealName}", new { @style = "width:184px;margin-left:10px;" })%></div>
		{{if IsDealClose==true}}
		<div class="cell auto" style="float: right;">
			<%: Html.Image("greenclosedeal.png")%>
		</div>
		{{/if}}
	</div>
</div>
<div class="line">
</div>
<div class="section">
	<div class="dealdetail">
		<div class="editor-label-first">
			<%: Html.LabelFor(model => model.ContactId)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Contact","${ContactName}") %>
			<%: Html.HiddenFor(model => model.ContactId, new { @val = "${ContactId}" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.IsPartnered) %>
		</div>
		<div class="editor-field rdo" style="width: auto; clear: right;">
			<%: Html.CheckBoxFor(model => model.IsPartnered, new { @val = "${IsPartnered}", @onclick = "javascript:deal.selectPartner(!this.checked);" })%>
		</div>
		<div class="editor-label-first">
			<%: Html.LabelFor(model => model.PurchaseTypeId) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownListFor(model => model.PurchaseTypeId, Model.PurchaseTypes, new { @val = "${PurchaseTypeId}", @refresh="true", @action = "PurchaseType" })%>
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
</div>

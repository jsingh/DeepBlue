<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="form-horizontal">
	<div class="control-group pull-left">
		<label class="control-label auto">
			${FundName}</label>
	</div>
	<div class="control-group pull-left">
		<label class="control-label">
			Deal Number&nbsp;${DealNumber}</label>
	</div>
	<div class="control-group pull-left">
		<label class="control-label">
			Deal Name</label>
		<div class="controls">
			<%: Html.TextBox("DealName", "${DealName}", new { @class = "input-large" })%>
		</div>
	</div>
	{{if IsDealClose==true}}
	<div class="cell auto" style="float: right;">
		<%: Html.Image("greenclosedeal.png")%>
	</div>
	{{/if}}
	<div class="clear">
		&nbsp;</div>
	<div class="line-break">
		&nbsp;</div>
	<div class="control-group pull-left">
		<label class="control-label">
			Contact</label><div class="controls">
				<%: Html.TextBox("Contact", "${ContactName}", new { @class = "input-large" })%>
			</div>
	</div>
	<div class="control-group pull-left">
		<label class="control-label">
			Partnered</label><div class="controls">
				<%: Html.CheckBoxFor(model => model.IsPartnered, new { @val = "${IsPartnered}", @onclick = "javascript:deal.selectPartner(!this.checked);" })%>
			</div>
	</div>
	<div class="clear">
		&nbsp;</div>
	<div class="control-group pull-left">
		<label class="control-label">
			Purchase Type</label><div class="controls">
				<%: Html.DropDownListFor(model => model.PurchaseTypeId, Model.PurchaseTypes, new { @val = "${PurchaseTypeId}", @refresh="true", @action = "PurchaseType", @class="input-large" })%>
			</div>
	</div>
	<div class="control-group pull-left" id="divPartnerName">
		<label class="control-label">
			Partner Name</label><div class="controls">
				<%: Html.TextBox("PartnerName", "${PartnerName}", new { @class = "input-large" })%>
			</div>
	</div>
	<div class="clear">
		&nbsp;</div>
	<%: Html.Hidden("ContactId", "${ContactId}", new { @id = "ContactId" })%>
	<%: Html.Hidden("FundId", "${FundId}", new { @id = "FundId" })%>
	<%: Html.Hidden("DealId", "${DealId}", new { @id = "DealId" })%>
	<%: Html.Hidden("DealNumber", "${DealNumber}", new { @id = "DealNumber" })%>
</div>
<div class="line-break">
	&nbsp;</div>

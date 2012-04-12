<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.DealSellerDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="accordion-group">
	<div class="accordion-heading">
		<a href="#SellerInformationBox" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle">Seller Information</a>
	</div>
	<div id="SellerInformationBox" class="accordion-body collapse">
		<div id="SellerInfo" class="form-horizontal">
			<div class="control-group pull-left">
				<%: Html.LabelFor(model => model.SellerType, new { @class = "control-label" })%>
				<div class="controls">
					<%: Html.TextBox("SellerType", "${SellerType}", new { @class = "input-large" })%>
					<%: Html.Hidden("SellerTypeId", "${SellerTypeId}")%>
				</div>
			</div>
			<div class="control-group pull-left">
				<%: Html.LabelFor(model => model.SellerName, new { @class = "control-label" })%>
				<div class="controls">
					<%: Html.TextBox("SellerName", "${SellerName}", new { @class = "input-large" })%>
				</div>
			</div>
			<div class="clear">
				&nbsp;</div>
			<div class="control-group pull-left">
				<%: Html.LabelFor(model => model.ContactName, new { @class = "control-label" })%>
				<div class="controls">
					<%: Html.TextBox("ContactName", "${ContactName}", new { @class = "input-large" })%>
				</div>
			</div>
			<div class="control-group pull-left">
				<%: Html.LabelFor(model => model.Phone, new { @class = "control-label" })%>
				<div class="controls">
					<%: Html.TextBox("Phone", "${Phone}", new { @class = "input-large" })%>
				</div>
			</div>
			<div class="clear">
				&nbsp;</div>
			<div class="control-group pull-left">
				<%: Html.LabelFor(model => model.Email, new { @class = "control-label" })%>
				<div class="controls">
					<%: Html.TextBox("Email", "${Email}", new { @class = "input-large" })%>
				</div>
			</div>
			<div class="control-group pull-left">
				<%: Html.LabelFor(model => model.Fax, new { @class = "control-label" })%>
				<div class="controls">
					<%: Html.TextBox("Fax", "${Fax}", new { @class = "input-large" })%>
				</div>
			</div>
			<div class="clear">
				&nbsp;</div>
			<div class="control-group center">
				<button id="save" class="btn btn-primary input-small" data-loading-text="Saving...">
					Save</button>
				<button id="cancel" class="btn input-small" onclick="javascript:deal.sellerInfoReset();">
					Reset</button>
			</div>
			<%: Html.Hidden("DealId", "${DealId}")%>
		</div>
	</div>
</div>

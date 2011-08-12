<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.DealSellerDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div class="expandheader">
	<div class="expandbtn">
		<div class="expandimg" id="img">
			ADD SELLER INFORMATION</div>
		<div class="expandtitle" id="title">
			<div class="expandtitle">
				Seller Information</div>
		</div>
	</div>
</div>
<div id="SellerInfo">
	<div class="fieldbox">
		<div class="section">
			<div class="dealdetail">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.SellerName) %>
				</div>
				<div class="editor-field auto">
					<%: Html.TextBox("SellerName","${SellerName}")%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.ContactName) %>
				</div>
				<div class="editor-field auto">
					<%: Html.TextBox("ContactName","${ContactName}") %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.CompanyName) %>
				</div>
				<div class="editor-field auto">
					<%: Html.TextBox("CompanyName","${CompanyName}")%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.Phone) %>
				</div>
				<div class="editor-field auto">
					<%: Html.TextBox("Phone","${Phone}")%>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Email) %>
				</div>
				<div class="editor-field auto">
					<%: Html.TextBox("Email", "${Email}")%>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.Fax) %>
				</div>
				<div class="editor-field auto">
					<%: Html.TextBox("Fax", "${Fax}")%>
				</div>
				<div class="editor-label">
				</div>
				<%: Html.Hidden("DealId", "${DealId}")%>
				<div class="editor-field auto" style="padding-left: 33%; width: auto;">
					<div class="cell auto">
						<%: Html.ImageButton("save.png", new { style = "border:0;" })%></div>
					<div class="cell auto" style="font-weight: bold;">
						<%: Html.Anchor("Reset", "javascript:deal.sellerInfoReset();")%>
					</div>
					<div class="cell auto">
						<%: Html.Span("", new { id = "SpnSellerUpdateLoading" })%>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>

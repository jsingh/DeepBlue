<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.DealSellerDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div id="SellerInfo">
	<div>
		<%: Html.Image("SellerInfo.png", new { @class="expandbtn" })%>
	</div>
	<div class="fieldbox">
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
		<div class="editor-field auto">
			<div class="cell auto">
				<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 26px;border:0;" })%></div>
			<div class="cell auto">
				<%: Html.Span("", new { id = "SpnSellerUpdateLoading" })%>
			</div>
		</div>
	</div>
</div>

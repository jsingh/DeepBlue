<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.DealUnderlyingDirectModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal Underlying Direct
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%= Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%=Html.JavascriptInclueTag("CreateDealUnderlyingDirect.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("CreateDealUnderlyingDirect", null, new AjaxOptions {
		UpdateTargetId = "UpdateTargetId",
		HttpMethod = "Post",
		OnBegin = "dealUnderlyingDirect.onCreateDealUnderlyingDirectBegin",
		OnSuccess = "dealUnderlyingDirect.onCreateDealUnderlyingDirectSuccess"
	}, new { @id = "AddNewDealUnderlyingDirect" })) {%>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FundName) %>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.DealCloseDate) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.RecordDate) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBox("RecordDate",(Model.RecordDate.HasValue == true ? (Model.RecordDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""))%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.IssuerId) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.DropDownListFor(model => model.IssuerId, Model.Issuers, new { @onchange = "javascript:dealUnderlyingDirect.loadSecurity();" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.SecurityTypeId) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.DropDownListFor(model => model.SecurityTypeId, Model.SecurityTypes, new { @onchange = "javascript:dealUnderlyingDirect.loadSecurity();" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.SecurityId) %>
	</div>
	<div class="editor-field auto-width">
		<%if (Model.SecurityTypeId == (int)DeepBlue.Models.Deal.Enums.SecurityType.Equity) {%>
		<%: Html.DropDownListFor(model => model.SecurityId, Model.Equities)%>
		<%}
	else if (Model.SecurityTypeId == (int)DeepBlue.Models.Deal.Enums.SecurityType.FixedIncome) {%>
		<%: Html.DropDownListFor(model => model.SecurityId, Model.FixedIncomes)%>
		<%}%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.NumberOfShares) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.NumberOfShares, new { @onkeypress = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FMV) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.FMV, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.PurchasePrice) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.PurchasePrice, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.TaxCostBase) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBoxFor(model => model.TaxCostBase, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.TaxCostDate) %>
	</div>
	<div class="editor-field auto-width">
		<%: Html.TextBox("TaxCostDate", (Model.TaxCostDate.HasValue == true ? (Model.TaxCostDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""))%>
	</div>
	<%: Html.HiddenFor(model => model.DealId)%>
	<%: Html.HiddenFor(model => model.DealUnderlyingDirectId)%>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return dealUnderlyingDirect.onSubmit('AddNewDealUnderlyingDirect');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "javascript:parent.dealUnderlyingDirect.closeDialog(false);" })%>
		</div>
	</div>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		dealUnderlyingDirect.init();</script>
	<%= Html.jQueryDatePicker("RecordDate")%>
	<%= Html.jQueryDatePicker("TaxCostDate")%>
</asp:Content>

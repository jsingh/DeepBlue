<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Transaction.CreateTransactionModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("TransactionController.js") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="edit-investor">
		<div id="editinfo" class="transaction-info">
			<div class="search">
				<div class="editor-label auto-width" style="width:auto">
					<%: Html.Label("Investor:") %>
				</div>
				<div class="editor-field auto-width">
					<%: Html.TextBox("Investor", "", new { style = "width:265px" })%>&nbsp;<%=Html.Span("",new { id = "Loading" })%>
				</div>
			</div>
			<% if (Model.InvestorId > 0) { %>
			<div class="edit-info" id="investorInfo">
				<div class="box">
					<div class="box-top">
						<div class="box-left">
						</div>
						<div class="box-center">
							Investor:&nbsp;
							<%:Html.Span("",new { id = "TitleInvestorName" })%>
						</div>
						<div class="box-right">
						</div>
					</div>
					<div class="box-content">
						<% Html.EnableClientValidation(); %>
						<% using (Ajax.BeginForm("CreateInvestorFund", new AjaxOptions { HttpMethod = "Post", OnBegin = "transactionController.onCreateFundBegin", OnSuccess = "transactionController.onCreateFundSuccess" })) {%>
						<%: Html.ValidationSummary(true) %>
						<div class="edit-left">
							<%: Html.HiddenFor(model => model.InvestorId)%>
							<div class="editor-label">
								<%: Html.Label("Investor Name") %>
							</div>
							<div class="display-field">
								<%: Html.DisplayFor(model => model.InvestorName)%>
							</div>
							<div class="editor-label">
								<%: Html.Label("Display Name") %>
							</div>
							<div class="display-field">
								<%: Html.DisplayFor(model => model.DisplayName)%>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.FundId) %>
							</div>
							<div class="editor-field">
								<%: Html.DropDownListFor(model => model.FundId,Model.FundNames) %>
								<%: Html.ValidationMessageFor(model => model.FundId) %>
							</div>
							<div class="editor-label auto-width" style="clear: right; white-space: nowrap">
								<%: Html.LabelFor(model => model.TotalCommitment) %>
							</div>
							<div class="editor-field">
								<%: Html.TextBoxFor(model => model.TotalCommitment) %>
								<%: Html.ValidationMessageFor(model => model.TotalCommitment) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.FundClosingId) %>
							</div>
							<div class="editor-field">
								<%: Html.DropDownListFor(model => model.FundClosingId, Model.FundClosings)%>
								<%: Html.ValidationMessageFor(model => model.FundClosingId) %>
							</div>
							<div class="editor-label" style="clear: right">
								<%: Html.LabelFor(model => model.CommittedDate) %>
							</div>
							<div class="editor-field">
								<%: Html.TextBoxFor(model => model.CommittedDate) %>
								<%: Html.ValidationMessageFor(model => model.CommittedDate) %>
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.InvestorTypeId) %>
							</div>
							<div class="editor-field">
								<%: Html.DropDownListFor(model => model.InvestorTypeId,Model.InvestorTypes) %>
							</div>
							<div class="editor-label" style="clear: right">
								<%: Html.LabelFor(model => model.IsAgreementSigned) %>
							</div>
							<div class="editor-field checkbox">
								<%: Html.CheckBoxFor(model => model.IsAgreementSigned)%>
							</div>
						</div>
						<div class="edit-right" id="accordion">
							<h3>
								<a href="#">Fund Details</a></h3>
							<div id="FundDetails">
							</div>
						</div>
						<div class="editor-button">
							<div style="float: left; padding: 0 0 10px 5px;">
								<%: Html.ImageButton("submit.png", new { style = "width: 73px; height: 23px;" })%>
							</div>
							<div style="float: left; padding: 0 0 10px 5px;">
								<%: Html.Span("",new { id = "UpdateLoading" })%>
							</div>
						</div>
						<% } %>
					</div>
				</div>
			</div>
			<% } %>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoCompleteScript("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,
																			OnSelect = "function(event, ui){ transactionController.selectInvestor(ui.item.id);}"
})%>
	<%= Html.jQueryDatePickerScript("CommittedDate")%>
	<%= Html.jQueryAccordionScript("accordion", new AccordionOptions { Disabled = true, Active = 0 })%>

	<script type="text/javascript">
		transactionController.init();
	</script>

</asp:Content>

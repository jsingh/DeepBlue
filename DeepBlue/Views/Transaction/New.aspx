<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Transaction.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Transaction
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("TransactionController.js") %>
	<%=Html.JavascriptInclueTag("EditTransaction.js") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="edit-investor">
		<div id="editinfo" class="transaction-info">
			<div class="search">
			</div>
			<div class="editor-row">
				<div class="editor-label auto-width" style="width: auto">
					<%: Html.Label("Investor:") %>
				</div>
				<div class="editor-field auto-width">
					<%: Html.TextBox("Investor", "", new { style = "width:200px" })%>&nbsp;<%=Html.Span("",new { id = "Loading" })%>
				</div>
			</div>
			<div class="edit-info" id="investorInfo" style="display: none">
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
						<% using (Ajax.BeginForm("CreateInvestorFund", null, new AjaxOptions { HttpMethod = "Post", OnBegin = "transactionController.onCreateFundBegin", OnSuccess = "transactionController.onCreateFundSuccess" }, new { @id = "NewTransaction" })) {%>
						<%: Html.HiddenFor(model => model.InvestorId)%>
						<div class="edit-left">
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.Label("Investor Name") %>
								</div>
								<div class="display-field">
									<div id="InvestorName">
										<%: Html.DisplayFor(model => model.InvestorName)%></div>
								</div>
							</div>
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.Label("Display Name") %>
								</div>
								<div class="display-field">
									<div id="DisplayName">
										<%: Html.DisplayFor(model => model.DisplayName)%></div>
								</div>
							</div>
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.LabelFor(model => model.FundId) %>
								</div>
								<div class="editor-field">
									<%: Html.DropDownListFor(model => model.FundId,Model.FundNames,new { @onchange = "javascript:transactionController.loadFundClosing(this.value);" } ) %>
									<%: Html.ValidationMessageFor(model => model.FundId) %>
								</div>
								<div class="editor-label auto-width" style="clear: right; white-space: nowrap">
									<%: Html.LabelFor(model => model.TotalCommitment) %>
								</div>
								<div class="editor-field">
									<%: Html.TextBox("TotalCommitment","") %>
									<%: Html.ValidationMessageFor(model => model.TotalCommitment) %>
								</div>
							</div>
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.LabelFor(model => model.FundClosingId) %>
								</div>
								<div class="editor-field">
									<%: Html.DropDownListFor(model => model.FundClosingId,Model.FundClosings)%>
									<%: Html.ValidationMessageFor(model => model.FundClosingId) %>
								</div>
								<div class="editor-label" style="clear: right">
									<%: Html.LabelFor(model => model.CommittedDate) %>
								</div>
								<div class="editor-field">
									<%: Html.TextBox("CommittedDate","", new { @id = "CommittedDate" }) %>
									<%: Html.ValidationMessageFor(model => model.CommittedDate) %>
								</div>
							</div>
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.LabelFor(model => model.InvestorTypeId) %>
								</div>
								<div class="editor-field">
									<%: Html.Span("",new { @id = "disp_InvestorTypeId", @style = "display:none"  }) %>
									<%: Html.DropDownListFor(model => model.InvestorTypeId,Model.InvestorTypes) %>
									<%: Html.ValidationMessageFor(model => model.InvestorTypeId)%>
								</div>
								<div class="editor-button">
									<div style="float: left; padding: 0 0 10px 5px;">
										<%: Html.ImageButton("submit.png", new { @class="default-button", @onclick = "javascript:transactionController.showErrorMessage('NewTransaction');" })%>
									</div>
									<div style="float: left; padding: 0 0 10px 5px;">
										<%: Html.Span("", new { @id = "UpdateLoading" })%>
									</div>
								</div>
							</div>
						</div>
						<div class="edit-right" id="accordion">
							<h3>
								<a href="#">Fund Details</a></h3>
							<div>
								<div id="LoadingFundDetail">
									<%: Html.Image("ajax.jpg")%>&nbsp;Loading...
								</div>
								<div id="FundDetails">
								</div>
							</div>
						</div>
						<% } %>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div id="EditCommitmentAmount" style="display: none">
		<% Html.RenderPartial("EditCommitmentAmount", Model.EditCommitmentAmountModel); %>
	</div>
	<div id="EditTransaction">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,
																			OnSelect = "function(event, ui){ transactionController.selectInvestor(ui.item.id);}"
})%>
	<%= Html.jQueryDatePicker("CommittedDate")%>
	<%= Html.jQueryAccordion("accordion", new AccordionOptions { Disabled = true, Active = 0 })%>

	<script type="text/javascript">
		transactionController.init();
		$("#EditCommitmentAmount").dialog({
			title: "Edit Commitment Amount",
			autoOpen: false,
			width: 430,
			modal: true,
			position: 'middle',
			autoResize: true
		});
		$("#EditTransaction").dialog({
			title: "Transaction",
			autoOpen: false,
			width: 600,
			modal: true,
			position: 'top',
			autoResize: true
		});
	</script>

</asp:Content>

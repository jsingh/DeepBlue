<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Transaction.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Transaction
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("TransactionController.js") %>
	<%=Html.JavascriptInclueTag("EditTransaction.js") %>
	<%=Html.StylesheetLinkTag("transaction.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTORS</span><span class="arrow"></span><span class="pname">Investor
					Commitment</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="edit-investor">
		<div id="editinfo">
			<div class="search">
			</div>
			<div class="editor-row">
				<div class="editor-label" style="width: auto;padding-left:0px;">
					<%: Html.Label("Investor") %>
				</div>
				<div class="editor-field" style="width: auto">
					<%: Html.TextBox("Investor", "SEARCH INVESTOR", new { @class="wm", style = "width:200px" })%></div>
				<div class="editor-field">
					&nbsp;<%=Html.Span("",new { id = "Loading" })%>
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
						<% using (Html.Form(new { @id = "frmTransaction", @onsubmit = "return transactionController.save(this);" })) {%>
						<%: Html.HiddenFor(model => model.InvestorId)%>
						<div class="edit-left">
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.Label("Investor Name") %>
								</div>
								<div class="editor-field">
									<div id="InvestorName">
										<%: Html.DisplayFor(model => model.InvestorName)%></div>
								</div>
							</div>
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.Label("Display Name") %>
								</div>
								<div class="editor-field">
									<div id="DisplayName">
										<%: Html.DisplayFor(model => model.DisplayName)%></div>
								</div>
							</div>
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.LabelFor(model => model.FundId) %>
								</div>
								<div class="editor-field">
									<%: Html.TextBox("FundName", "")%>
									<%: Html.Hidden("FundId", "0")%>
								</div>
								<div class="editor-label" style="clear: right; white-space: nowrap">
									<%: Html.LabelFor(model => model.TotalCommitment) %>
								</div>
								<div class="editor-field">
									<%: Html.TextBox("TotalCommitment","") %>
								</div>
							</div>
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.LabelFor(model => model.FundClosingId) %>
								</div>
								<div class="editor-field">
									<%: Html.DropDownListFor(model => model.FundClosingId, Model.FundClosings, new { @onchange = "javascript:transactionController.checkFundClose(this.value);" })%>
								</div>
							</div>
							<div class="editor-row">
								<div class="editor-label">
									<%: Html.LabelFor(model => model.InvestorTypeId) %>
								</div>
								<div class="editor-field">
									<%: Html.Span("",new { @id = "disp_InvestorTypeId", @style = "display:none"  }) %>
									<%: Html.DropDownListFor(model => model.InvestorTypeId,Model.InvestorTypes) %>
								</div>
								<div class="editor-field" style="clear: both; margin-left: 45%;">
									<%: Html.ImageButton("submit_active.png", new { @class="default-button" })%>
									<%: Html.Span("", new { @id = "UpdateLoading" })%>
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
	<div id="AddFundClose">
		<%using (Html.Form(new { @id = "frmAddFundClose", @onsubmit = "return false" })) {%>
		<div class="editor-label">
			<%: Html.Label("Name")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Name", "")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Fund")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("CloseFundName", "")%>
			<%: Html.Hidden("FundId", "0")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Closing Date")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FundClosingDate", "")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("First Closing")%>
		</div>
		<div class="editor-field">
			<%: Html.CheckBox("IsFirstClosing", false)%>
		</div>
		<div class="editor-label">
		</div>
		<div class="editor-field" style="width: 220px">
			<%: Html.Image("Save_active.png", new { @style = "cursor:pointer", @onclick = "javascript:transactionController.addFundClose();" })%>
			&nbsp;&nbsp;<%: Html.Image("Cancel_active.png", new { @style = "cursor:pointer", @onclick = "javascript:transactionController.cancelFundClose();" })%>
			&nbsp;&nbsp;<%: Html.Span("", new { @id = "Loading" })%>
		</div>
		<%}%>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,
																			OnSelect = "function(event, ui){ transactionController.selectInvestor(ui.item.id);}"
})%>
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength=1,
																	  OnSelect = "function(event, ui){ transactionController.loadFundClosing(ui.item.id);}"
})%>
	<%= Html.jQueryAutoComplete("CloseFundName", new AutoCompleteOptions {	Source = "/Fund/FindFunds",
																			MinLength = 1,
																			OnSelect = "function(event, ui){ $('#FundId','#frmAddFundClose').val(ui.item.id);}"
})%>
	<%= Html.jQueryDatePicker("CommittedDate")%>
	<%= Html.jQueryDatePicker("FundClosingDate")%>
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
		$("#AddFundClose").dialog({
			title: "Fund Close",
			autoOpen: false,
			width: 430,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	</script>
	<script id="TransactionTemplate" type="text/x-jquery-tmpl">
			<% Html.RenderPartial("EditTransaction", Model.EditModel); %>
	</script>
</asp:Content>

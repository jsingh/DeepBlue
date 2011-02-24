<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Entity.InvestorEntityType>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Transaction
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("Transaction.js") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="edit-investor">
		<div class="search">
			<div class="editor-label auto-width">
				<%: Html.Label("Investor:") %>
			</div>
			<div class="editor-field auto-width">
				<%: Html.TextBox("Investor")%>&nbsp;<%=Html.Span("",new { id = "Loading" })%>
			</div>
		</div>
		<div id="editinfo">
			<div class="edit-info" id="investorInfo" style="display: none;">
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
						<div class="edit-left">
							<div class="editor-label auto-width">
								<%: Html.Label("Investor Name") %>
							</div>
							<div class="display-field">
								<%: Html.Span("",new {id = "InvestorName"})%>
							</div>
							<div class="editor-label auto-width">
								<%: Html.Label("Display Name") %>
							</div>
							<div class="display-field">
								<%: Html.Span("", new { id = "DisplayName" })%>
							</div>
							<div class="editor-label auto-width">
								<%: Html.Label("Fund Name:") %>
							</div>
							<div class="display-field" style="width: 30px">
								<%: Html.Span("", new { id = "FundName" })%>
							</div>
							<div class="editor-label auto-width" style="clear: right">
								<%: Html.Label("Committed Amount:") %>
							</div>
							<div class="display-field" style="width: 30px">
								<%: Html.Span("", new { id = "CommittedAmount" })%>
							</div>
							<div class="editor-label auto-width">
								<%: Html.Label("Fund Close:") %>
							</div>
							<div class="display-field" style="width: 30px">
								<%: Html.Span("", new { id = "UnfundedAmount" })%>
							</div>
							<div class="editor-label auto-width" style="clear: right">
								<%: Html.Label("Investor Type:") %>
							</div>
							<div class="display-field" style="width: 40px">
								<%: Html.Span("", new { id = "InvestorType" })%>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoCompleteScript("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,
																			OnSelect = "function(event, ui){ transaction.selectInvestor(ui.item.id);}"
})%>
</asp:Content>

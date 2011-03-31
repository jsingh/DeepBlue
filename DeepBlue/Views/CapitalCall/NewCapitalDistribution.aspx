<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateDistributionModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Capital Distribution
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("capitalcall.css")%><%=Html.JavascriptInclueTag("CapitalCallDistribution.js")%><%=Html.StylesheetLinkTag("distribution.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("CreateDistribution", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "distribution.onCreateCapitalCallBegin", OnSuccess = "distribution.onCreateCapitalCallSuccess" }, new { @id = "Distribution" })) {%>
	<div class="cc-header">
		<div class="page-title">
			<h2>
				New Capital Distribution</h2>
		</div>
		<div class="editor-label" style="width: auto">
			<%: Html.LabelFor(model => model.FundId) %>&nbsp;<%: Html.TextBox("Fund","", new { @style = "width:200px" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
		</div>
	</div>
	<div class="cc-main" id="CCDetail" style="display: none">
		<div class="box">
			<div class="box-top">
				<div class="box-left">
				</div>
				<div class="box-center">
					Fund:&nbsp;
					<%:Html.Span("",new { id = "TitleFundName" })%>
				</div>
				<div class="box-right">
				</div>
			</div>
			<div class="box-content">
				<div class="edit-left">
					<%: Html.HiddenFor(model => model.FundId) %>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.DistributionNumber) %>&nbsp;<%: Html.Span("",new { @id= "SpnDistributionNumber"})%>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.DistributionAmount) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBoxFor(model => model.DistributionAmount, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.DistributionDate) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBoxFor(model => model.DistributionDate) %>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.DistributionDueDate) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBoxFor(model => model.DistributionDueDate) %>
					</div>
					<div class="editor-label" style="width: 207px">
						<%: Html.CheckBoxFor(model => model.AddPreferredReturn, new { @onclick="javascript:distribution.showControl(this,'PreferredAmountBox');" }) %>&nbsp;<%: Html.LabelFor(model => model.AddPreferredReturn) %>
					</div>
					<div class="editor-field" id="PreferredAmountBox" style="display: none">
						<%: Html.TextBoxFor(model => model.PreferredReturn, new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="width: 207px">
						<%: Html.CheckBoxFor(model => model.AddReturnManagementFees, new { @onclick = "javascript:distribution.showControl(this,'ReturnManagementFeesBox');" })%>&nbsp;<%: Html.LabelFor(model => model.AddReturnManagementFees) %>
					</div>
					<div class="editor-field" id="ReturnManagementFeesBox" style="display: none">
						<%: Html.TextBoxFor(model => model.ReturnManagementFees, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="width: 207px">
						<%: Html.CheckBoxFor(model => model.AddReturnFundExpenses, new { @onclick = "javascript:distribution.showControl(this,'ReturnFundExpensesBox');" })%>&nbsp;<%: Html.LabelFor(model => model.AddReturnFundExpenses) %>
					</div>
					<div class="editor-field" id="ReturnFundExpensesBox" style="display: none">
						<%: Html.TextBoxFor(model => model.ReturnFundExpenses, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="width: 207px">
						<%: Html.CheckBoxFor(model => model.AddPreferredCatchUp, new { @onclick = "javascript:distribution.showControl(this,'PreferredCatchUpBox');" })%>&nbsp;<%: Html.LabelFor(model => model.AddPreferredCatchUp) %>
					</div>
					<div class="editor-field" id="PreferredCatchUpBox" style="display: none">
						<%: Html.TextBoxFor(model => model.PreferredCatchUp, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="width: 145px">
						<%: Html.CheckBoxFor(model => model.AddProfits, new { @onclick = "javascript:distribution.showControl(this,'ProfitBox');" })%>&nbsp;<%: Html.LabelFor(model => model.AddProfits) %>
					</div>
					<div id="ProfitBox" style="display: none">
						<div class="editor-label" style="clear: right">
							<%: Html.LabelFor(model => model.GPProfits) %>
						</div>
						<div class="editor-field">
							<%: Html.TextBoxFor(model => model.GPProfits, new { @onkeypress = "return jHelper.isCurrency(event);" }) %>
						</div>
						<div class="editor-label" style="clear: right">
							<%: Html.LabelFor(model => model.LPProfits) %>
						</div>
						<div class="editor-field">
							<%: Html.TextBoxFor(model => model.LPProfits, new { @onkeypress = "return jHelper.isCurrency(event);" })%>
						</div>
					</div>
					<div class="status">
						<%: Html.Span("", new { id = "UpdateLoading" })%></div>
					<div class="editor-button">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.ImageButton("submit.png", new { @style = "width: 73px; height: 23px;", @onclick = "javascript:distribution.onSubmit('Distribution');" })%>
						</div>
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Span("", new { @id = "UpdateLoading" })%>
						</div>
					</div>
				</div>
				<div class="edit-right" id="accordion">
					<h3>
						<a href="#">Fund Details</a></h3>
					<div>
						<div id="FundDetails">
							<div class="editor-label">
								Committed Amount:
							</div>
							<div class="editor-label" style="clear: right">
								<b>
									<%: Html.Span("",new { @id="SpnCommittedAmount"})%></b>
							</div>
							<div class="editor-label" style="width: 109px">
								Unfunded Amount:
							</div>
							<div class="editor-label" style="clear: right">
								<b>
									<%: Html.Span("",new { @id="UnfundedAmount"})%></b>
							</div>
							<div class="editor-label">
								<%: Html.Anchor("Previous Capital Calls","#", new { @id="lnkPCC", @target = "_blank", @style="color:Blue" })%>
							</div>
							<div class="editor-label">
								<%: Html.Anchor("Previous Capital Distributions","#", new { @id="lnkPCD", @target = "_blank", @style="color:Blue" })%>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<%: Html.ValidationMessageFor(model => model.FundId) %>
	<%: Html.ValidationMessageFor(model => model.DistributionAmount) %>
	<%: Html.ValidationMessageFor(model => model.DistributionDate) %>
	<%: Html.ValidationMessageFor(model => model.DistributionDueDate) %>
	<%: Html.ValidationMessageFor(model => model.PreferredReturn) %>
	<%: Html.ValidationMessageFor(model => model.ReturnFundExpenses) %>
	<%: Html.ValidationMessageFor(model => model.ReturnManagementFees) %>
	<%: Html.ValidationMessageFor(model => model.PreferredCatchUp) %>
	<%: Html.HiddenFor(model => model.DistributionNumber) %>
	<%: Html.Hidden("CommittedAmount","0",new { @id = "CommittedAmount" }) %><div id="UpdateTargetId" style="display: none">
	</div>
	<%}%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("DistributionDate")%><%= Html.jQueryDatePicker("FromDate")%><%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryDatePicker("DistributionDueDate")%><%= Html.jQueryAccordion("accordion", new AccordionOptions { Disabled = true, Active = 0 })%>
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { distribution.selectFund(ui.item.id);}"})%>

	<script type="text/javascript">
		distribution.init();
	</script>

</asp:Content>

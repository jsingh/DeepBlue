<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateDistributionModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modify Capital Distribution
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("CapitalCallDistribution.js")%>
	<%=Html.JavascriptInclueTag("CapitalCallDistributionManual.js")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">AMBERBROOK FUNDS</span><span class="arrow"></span><span class="pname">Capital
					Distribution</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="cc-main" id="CCDetail" style="display: none">
		<div class="cc-box">
			<div class="section ccdetail">
				<div class="cell">
					<label>
						<%:Html.Span("",new { id = "TitleFundName" })%></label>
					<%: Html.HiddenFor(model => model.FundId)%>
				</div>
				<div class="cell">
					<label>
						Capital Distributed&nbsp;<%: Html.Span("", new { @id = "SpnDAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						Profits&nbsp;<%: Html.Span("", new { @id = "SpnProfitAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						<%: Html.Anchor("Previous Capital Distributions","#", new { @id="lnkPCD", @target = "_blank", @style="color:Blue" })%>
					</label>
				</div>
			</div>
		</div>
		<%: Html.HiddenFor(model => model.FundId) %><%: Html.HiddenFor(model => model.DistributionNumber) %>
		<div id="NewCapitalDistribution">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "Distribution", @onsubmit = "return flase" })) {%>
				<div class="line">
				</div>
				<div class="cc-box-det dist-detail">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.DistributionNumber) %>
					</div>
					<div class="editor-field">
						<%: Html.Span("",new { @id= "SpnDistributionNumber"})%>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.DistributionAmount) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("DistributionAmount","", new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.CapitalDistributionDate) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("CapitalDistributionDate","")%>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.CapitalDistributionDueDate) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("CapitalDistributionDueDate", "")%>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="cc-box-det dist-detail">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.CapitalReturn)%>
					</div>
					<div class="editor-field amtbox" id="Div1">
						<%: Html.TextBoxFor(model => model.CapitalReturn, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.PreferredReturn)%>
					</div>
					<div class="editor-field amtbox" id="PreferredAmountBox">
						<%: Html.TextBoxFor(model => model.PreferredReturn, new {  @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.PreferredCatchUp)%>
					</div>
					<div class="editor-field amtbox" id="PreferredCatchUpBox">
						<%: Html.TextBoxFor(model => model.PreferredCatchUp, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.ReturnFundExpenses)%>
					</div>
					<div class="editor-field amtbox" id="ReturnFundExpensesBox">
						<%: Html.TextBoxFor(model => model.ReturnFundExpenses, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.ReturnManagementFees)%>
					</div>
					<div class="editor-field amtbox" id="ReturnManagementFeesBox">
						<%: Html.TextBoxFor(model => model.ReturnManagementFees, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.GPProfits) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBoxFor(model => model.GPProfits, new { @onkeydown = "return jHelper.isCurrency(event);" }) %>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.LPProfits) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBoxFor(model => model.LPProfits, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-button" style="width: 300px; padding-left: 240px;">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Image("submit_active.png", new { @class = "default-button", @onclick = "javascript:distribution.save('Distribution');" })%>
						</div>
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Span("", new { @id = "UpdateLoading" })%>
						</div>
					</div>
					<%: Html.Hidden("CommittedAmount","0",new { @id = "CommittedAmount" }) %>
				</div>
				<div class="line">
				</div>
				<%}%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("CapitalDistributionDate")%>
	<%= Html.jQueryDatePicker("CapitalDistributionDueDate")%>
	<%= Html.jQueryDatePicker("ManCapitalDistributionDate")%>
	<%= Html.jQueryDatePicker("ManCapitalDistributionDueDate")%>
	<%= Html.jQueryDatePicker("FromDate")%>
	<%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { distribution.selectFund(ui.item.id);}"})%>
	<script id="CapitalDistributionInvestorTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CapitalDistributionInvestorDetail"); %>
	</script>
	<script type="text/javascript">
		distribution.init();
	</script>
	<%if (Model.FundId > 0) {%>
	<script type="text/javascript">$(document).ready(function(){
		distribution.selectFund(<%=Model.FundId%>);
		});
	</script>
	<%}%>
</asp:Content>

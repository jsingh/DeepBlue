<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateDistributionModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modify Capital Distribution
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.StylesheetLinkTag("modifycapitaldistribution.css")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("ModifyCapitalDistribution.js")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">AMBERBROOK FUNDS</span><span class="arrow"></span><span class="pname">Modify
					Capital Distribution</span></div>
			<div class="rightcol">
				<div style="float: left">
					<%: Html.TextBox("SearchFundName", "SEARCH AMBERBROOK FUND", new { @class = "wm", @id = "SearchFundName", @style = "width: 200px" })%>
				</div>
				<div style="float: left; padding-left: 10px;">
					<%: Html.TextBox("SearchCapitalDistribution", "SEARCH CAPITAL DISTRIBUTION", new { @class = "wm", @id = "SearchCapitalDistribution", @style = "width: 200px" })%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%: Html.Hidden("SearchFundID",Model.FundId)%>
	<%: Html.Hidden("SearchCapitalDistributionID",Model.CapitalDistributionID)%>
	<div id="ModifyCapitalDistribution">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("CapitalDistributionDate")%>
	<%= Html.jQueryDatePicker("CapitalDistributionDueDate")%>
	<%= Html.jQueryDatePicker("ManCapitalDistributionDate")%>
	<%= Html.jQueryDatePicker("ManCapitalDistributionDueDate")%>
	<%= Html.jQueryDatePicker("FromDate")%>
	<%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryAutoComplete("SearchFundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { modifyCapitalDistribution.selectFund(ui.item.id);}" })%>
	<%= Html.jQueryAutoComplete("SearchCapitalDistribution", new AutoCompleteOptions { SearchFunction = "modifyCapitalDistribution.searchCD", MinLength = 1, OnSelect = "function(event, ui) { modifyCapitalDistribution.selectCD(ui.item.id);}" })%>
	<script id="CapitalDistributionInvestorTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CapitalDistributionInvestorDetail"); %>
	</script>
	<script id="ModifyCapitalDistributionTemplate" type="text/x-jquery-tmpl">
	<div class="cc-main" id="CDDetail">
		<div class="cc-box">
			<div class="section ccdetail">
				<div class="cell">
					<label>
						<%:Html.Span("${FundName}",new { id = "TitleFundName" })%></label>
					<%: Html.HiddenFor(model => model.FundId)%>
				</div>
				<div class="cell">
					<label>
						Capital Distributed&nbsp;<%: Html.Span("${formatCurrency(TotalDistribution)}", new { @id = "SpnDAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						Profits&nbsp;<%: Html.Span("${formatCurrency(TotalProfit)}", new { @id = "SpnProfitAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						<%: Html.Anchor("Previous Capital Distributions","/CapitalCall/Detail?fundId=${FundId}&typeId=2", new { @id="lnkPCD", @target = "_blank", @style="color:Blue" })%>
					</label>
				</div>
			</div>
		</div>
		<%: Html.HiddenFor(model => model.FundId) %><%: Html.HiddenFor(model => model.DistributionNumber) %>
		<div id="NewCapitalDistribution">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "Distribution", @onsubmit = "return flase" })) {%>
				<%: Html.jQueryTemplateHiddenFor(model => model.CapitalDistributionLineItemsCount)%>
				<%: Html.jQueryTemplateHiddenFor(model => model.CapitalDistributionID) %>
				<%: Html.jQueryTemplateHiddenFor(model => model.FundId) %>
				<%: Html.jQueryTemplateHiddenFor(model => model.DistributionNumber) %>
				<div class="line">
				</div>
				<div class="cc-box-det dist-detail">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.DistributionNumber) %>
					</div>
					<div class="editor-field">
						<b>
							<%: Html.jQueryTemplateDisplayFor(model => model.DistributionNumber, true)%></b>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.DistributionAmount) %>
					</div>
					<div class="editor-field">
						<%: Html.jQueryTemplateTextBoxFor(model => model.DistributionAmount, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.CapitalDistributionDate) %>
					</div>
					<div class="editor-field">
						<%: Html.jQueryTemplateTextBoxFor(model => model.CapitalDistributionDate, new { @class="datefield" },"formatDate")%>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.CapitalDistributionDueDate) %>
					</div>
					<div class="editor-field">
						<%: Html.jQueryTemplateTextBoxFor(model => model.CapitalDistributionDueDate, new { @class="datefield" },"formatDate")%>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="cc-box-det dist-detail">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.CapitalReturn)%>
					</div>
					<div class="editor-field amtbox" id="Div1">
						<%: Html.jQueryTemplateTextBoxFor(model => model.CapitalReturn, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.PreferredReturn)%>
					</div>
					<div class="editor-field amtbox" id="PreferredAmountBox">
						<%: Html.jQueryTemplateTextBoxFor(model => model.PreferredReturn, new {  @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.PreferredCatchUp)%>
					</div>
					<div class="editor-field amtbox" id="PreferredCatchUpBox">
						<%: Html.jQueryTemplateTextBoxFor(model => model.PreferredCatchUp, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.ReturnFundExpenses)%>
					</div>
					<div class="editor-field amtbox" id="ReturnFundExpensesBox">
						<%: Html.jQueryTemplateTextBoxFor(model => model.ReturnFundExpenses, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.ReturnManagementFees)%>
					</div>
					<div class="editor-field amtbox" id="ReturnManagementFeesBox">
						<%: Html.jQueryTemplateTextBoxFor(model => model.ReturnManagementFees, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.GPProfits) %>
					</div>
					<div class="editor-field">
						<%: Html.jQueryTemplateTextBoxFor(model => model.GPProfits, new { @onkeydown = "return jHelper.isCurrency(event);" }) %>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.LPProfits) %>
					</div>
					<div class="editor-field">
						<%: Html.jQueryTemplateTextBoxFor(model => model.LPProfits, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
					</div>
					<div class="editor-label" style="clear: both; width: 100%;padding-left:38px;">
						<% Html.RenderPartial("TBoxTop"); %>
						<table cellpadding="0" cellspacing="0" border="0" class="grid">
							<thead>
								<tr>
									<th class="lalign">
										Investor
									</th>
									<th class="ralign">
										Distribution Amount
									</th>
									<th class="ralign">
										Cost Return
									</th>
									<th class="ralign">
										Preferred Return
									</th>
									<th class="ralign">
										Return Management Fees
									</th>
									<th class="ralign">
										Return Fund Expenses
									</th>
									<th class="ralign">
										Preferred CatchUp
									</th>
									<th class="ralign">
										Profits
									</th>
									<th class="ralign">
										LPProfits
									</th>
									<th class="ralign">
									</th>
								</tr>
							</thead>
							<tbody>
								{{each(i,item) CapitalDistributionLineItems}}
									<tr id="Row${i+1}" {{if i%2>0}}class="arow"{{else}}class="row"{{/if}}>
										<td class="lalign">
											${item.InvestorName}
										</td>
										<td class="ralign">
											<%: Html.Span("${formatCurrency(item.DistributionAmount)}", new { @class = "show" })%>
											<%: Html.TextBox("${i}_DistributionAmount", "${item.DistributionAmount}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
										</td>
										<td class="ralign">
											<%: Html.Span("${formatCurrency(item.CapitalReturn)}", new { @class = "show" })%>
											<%: Html.TextBox("${i}_CapitalReturn", "${item.CapitalReturn}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
										</td>
										<td class="ralign">
											<%: Html.Span("${formatCurrency(item.PreferredReturn)}", new { @class = "show" })%>
											<%: Html.TextBox("${i}_PreferredReturn", "${item.PreferredReturn}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
										</td>
										<td class="ralign">
											<%: Html.Span("${formatCurrency(item.ReturnManagementFees)}", new { @class = "show" })%>
											<%: Html.TextBox("${i}_ReturnManagementFees", "${item.ReturnManagementFees}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
										</td>
										<td class="ralign">
											<%: Html.Span("${formatCurrency(item.ReturnFundExpenses)}", new { @class = "show" })%>
											<%: Html.TextBox("${i}_ReturnFundExpenses", "${item.ReturnFundExpenses}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
										</td>
										<td class="ralign">
											<%: Html.Span("${formatCurrency(item.PreferredCatchUp)}", new { @class = "show" })%>
											<%: Html.TextBox("${i}_PreferredCatchUp", "${item.PreferredCatchUp}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
										</td>
										<td class="ralign">
											<%: Html.Span("${formatCurrency(item.Profits)}", new { @class = "show" })%>
											<%: Html.TextBox("${i}_Profits", "${item.Profits}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
										</td>
										<td class="ralign">
											<%: Html.Span("${formatCurrency(item.LPProfits)}", new { @class = "show" })%>
											<%: Html.TextBox("${i}_LPProfits", "${item.LPProfits}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
										</td>
										<td class="ralign">
											<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:modifyCapitalDistribution.edit(this);" })%>
											<%: Html.Hidden("${i}_CapitalDistributionID", "${item.CapitalDistributionID}")%>
											<%: Html.Hidden("${i}_CapitalDistributionLineItemID", "${item.CapitalDistributionLineItemID}")%>
										</td>
									</tr>
								{{/each}}
							</tbody>
						</table>
						<% Html.RenderPartial("TBoxBottom"); %>
					</div>
					<div class="editor-button" style="width: 300px; padding-left: 240px;">
						<div style="float: left; padding: 0 0 10px 5px;">
							<%: Html.Image("submit_active.png", new { @class = "default-button", @onclick = "javascript:modifyCapitalDistribution.save('Distribution');" })%>
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
	</script>
	<script type="text/javascript">
		modifyCapitalDistribution.init();
	</script>
</asp:Content>

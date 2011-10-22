<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateCapitalCallModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modify Capital Call
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("ModifyCapitalCall.js") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">AMBERBROOK FUNDS</span><span class="arrow"></span><span class="pname">Modify
					Capital Call</span></div>
			<div class="rightcol">
				<div style="float: left">
					<%: Html.TextBox("SearchFundName", "SEARCH AMBERBROOK FUND", new { @class = "wm", @id = "SearchFundName", @style = "width: 200px" })%>
				</div>
				<div style="float: left; padding-left: 10px;">
					<%: Html.TextBox("SearchCapitalCall", "SEARCH CAPITAL CALL", new { @class = "wm", @id = "SearchCapitalCall", @style = "width: 200px" })%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%: Html.Hidden("SearchFundID",Model.FundId)%>
	<%: Html.Hidden("SearchCapitalCallID",Model.CapitalCallID)%>
	<div id="ModifyCapitalCall">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("CapitalCallDate")%>
	<%= Html.jQueryDatePicker("CapitalCallDueDate")%>
	<%= Html.jQueryDatePicker("ManCapitalCallDate")%>
	<%= Html.jQueryDatePicker("ManCapitalCallDueDate")%>
	<%= Html.jQueryDatePicker("FromDate")%>
	<%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryAutoComplete("SearchFundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { modifyCapitalCall.selectFund(ui.item.id);}" })%>
	<%= Html.jQueryAutoComplete("SearchCapitalCall", new AutoCompleteOptions { SearchFunction = "modifyCapitalCall.searchCC", MinLength = 1, OnSelect = "function(event, ui) { modifyCapitalCall.selectCC(ui.item.id);}" })%>
	<script id="CapitalCallInvestorTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CapitalCallInvestorDetail"); %>
	</script>
	<script id="ModifyCapitalCallTemplate" type="text/x-jquery-tmpl">
	 <div class="cc-main" id="CCDetail">
		<div class="cc-box">
			<div class="section ccdetail">
				<div class="cell">
					<label>
						<%:Html.Span("${FundName}",new { id = "TitleFundName" })%></label>
					<%: Html.jQueryTemplateHiddenFor(model => model.FundId)%>
					<%: Html.jQueryTemplateHiddenFor(model => model.CapitalCallNumber)%>
				</div>
				<div class="cell">
					<label>
						Committed Amount&nbsp;<%: Html.Span("${formatCurrency(TotalCommitment)}", new { @id = "CommittedAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						Unfunded Amount&nbsp;<%: Html.Span("${formatCurrency(UnfundedAmount)}", new { @id = "UnfundedAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						<%: Html.Anchor("Previous Capital Calls","/CapitalCall/Detail?fundId=${FundId}", new { @id="lnkPCC", @target = "_blank", @style="color:Blue" })%>
					</label>
				</div>
			</div>
		</div>
		<div id="NewCapitalCall">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "CapitalCall", @onsubmit = "return false" })) {%>
				<%: Html.jQueryTemplateHiddenFor(model => model.CapitalCallLineItemsCount)%>
				<%: Html.jQueryTemplateHiddenFor(model => model.CapitalCallID) %>
				<div class="line">
				</div>
				<div class="cc-box-det">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.CapitalCallNumber)%>
					</div>
					<div class="editor-field">
						<b>
							<%: Html.jQueryTemplateDisplayFor(model => model.CapitalCallNumber, true)%></b>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.CapitalAmountCalled)%>
					</div>
					<div class="editor-field">
						<%: Html.jQueryTemplateTextBoxFor(model => model.CapitalAmountCalled, new { @onkeydown = "return jHelper.isCurrency(event);", @style = "width:110px", @onkeyup = "javascript:modifyCapitalCall.calcExistingInvestmentAmount();" })%>
					</div>
					<div class="editor-label" style="clear: right; width: auto;">
						<%: Html.LabelFor(model => model.CapitalCallDate) %>
					</div>
					<div class="editor-field">
						<%: Html.jQueryTemplateTextBoxFor(model => model.CapitalCallDate, new { @style = "width:110px", @class="datefield" }, "formatDate")%>
					</div>
					<div class="editor-label" id="ccduedatelbl" style="clear: right; width: auto;">
						<%: Html.LabelFor(model => model.CapitalCallDueDate) %>
					</div>
					<div class="editor-field">
						<%: Html.jQueryTemplateTextBoxFor(model => model.CapitalCallDueDate, new { @style = "width:110px", @class="datefield" }, "formatDate")%>
					</div>
					<div class="editor-label">
						<%: Html.Span("Add Management Fees", new { @id = "SpnAddManagementFee" })%>
					</div>
					<div class="editor-field">
						<%: Html.CheckBox("AddManagementFees",false, new { @val="${AddManagementFees}", @style = "width:auto;", @displaywidth = "118px", @display = "", @onclick = "javascript:modifyCapitalCall.selectMFee(this);" })%>
					</div>
						<div id="ManFeeMain" style="{{if AddManagementFees=="true"}}display: none;{{/if}}float: left;">	
							<div class="editor-label">
								<%: Html.LabelFor(model => model.FromDate) %>
							</div>
							<div class="editor-field">
								<%: Html.jQueryTemplateTextBoxFor(model => model.FromDate, new { @class = "datefield", @id = "FromDate", @style="width:111px" }, "formatDate")%>&nbsp;<%: Html.LabelFor(model => model.ToDate) %>&nbsp;<%: Html.jQueryTemplateTextBoxFor(model => model.ToDate, new { @class = "datefield", @id = "ToDate", @style = "width:111px" }, "formatDate")%>
							</div>
							<div class="editor-label" id="feeamountlbl" style="clear: right;">
								Fee Amount
							</div>
							<div class="editor-field">
								<%: Html.jQueryTemplateTextBoxFor(model => model.ManagementFees, new { @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:modifyCapitalCall.calcExistingInvestmentAmount();" })%>
							</div>
						</div>
						<div class="editor-label">
							<%: Html.Span("Add Fund Expenses", new { @id = "SpnAddFundExpenses" })%>
						</div>
						<div class="editor-field">
							<%: Html.CheckBox("AddFundExpenses", false, new { @val="${AddFundExpenses}", @style = "width:auto;", @displaywidth = "118px", @display = "", @onclick = "javascript:modifyCapitalCall.selectFundExp(this);" })%>
						</div>
							<div id="FunExpAmount" style="{{if AddFundExpenses=="true"}}display: none;{{/if}}float: left;">
								<div class="editor-label">
									Fund Expense Amount
								</div>
								<div class="editor-field">
									<%: Html.jQueryTemplateTextBoxFor(model => model.FundExpenseAmount, new { @class = "datetxt", @style = "width:111px", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:modifyCapitalCall.calcExistingInvestmentAmount();" })%>
								</div>
							</div>
							<div class="editor-label">
								Capital Call Split For
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.NewInvestmentAmount) %>
							</div>
							<div class="editor-field">
								<%: Html.jQueryTemplateTextBoxFor(model => model.NewInvestmentAmount, new { @class = "datetxt", @style = "width:110px;", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:modifyCapitalCall.calcExistingInvestmentAmount();" })%>
							</div>
							<div class="editor-label" style="clear: right">
								<%: Html.LabelFor(model => model.ExistingInvestmentAmount) %>
							</div>
							<div class="editor-field">
								<%: Html.Span("${formatCurrency(ExistingInvestmentAmount)}", new { @id = "SpnExistingInvestmentAmount" })%>
								<%: Html.jQueryTemplateHiddenFor(model => model.ExistingInvestmentAmount)%>
							</div>
							<div class="editor-label" style="clear: both; width: 100%;">
								<% Html.RenderPartial("TBoxTop"); %>
								<table cellpadding="0" cellspacing="0" border="0" class="grid">
									<thead>
										<tr>
											<th class="lalign">
												Investor
											</th>
											<th class="ralign">
												Capital Amount Called
											</th>
											<th class="ralign">
												Investment Amount
											</th>
											<th class="ralign">
												New Investment Amount
											</th>
											<th class="ralign">
												Existing Investment Amount
											</th>
											<th class="ralign">
												Management Fees
											</th>
											<th class="ralign">
												Fund Expenses
											</th>
											<th class="ralign">
											</th>
										</tr>
									</thead>
									<tbody>
										{{each(i,item) CapitalCallLineItems}}
											<tr id="Row${i+1}" {{if i%2>0}}class="arow"{{else}}class="row"{{/if}}>
												<td class="lalign">
													${item.InvestorName}
												</td>
												<td class="ralign">
													<%: Html.Span("${formatCurrency(item.CapitalAmountCalled)}", new { @class = "show" })%>
													<%: Html.TextBox("${i}_CapitalAmountCalled", "${item.CapitalAmountCalled}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
												</td>
												<td class="ralign">
													<%: Html.Span("${formatCurrency(item.InvestmentAmount)}", new { @class = "show" })%>
													<%: Html.TextBox("${i}_InvestmentAmount", "${item.InvestmentAmount}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
												</td>
												<td class="ralign">
													<%: Html.Span("${formatCurrency(item.NewInvestmentAmount)}", new { @class = "show" })%>
													<%: Html.TextBox("${i}_NewInvestmentAmount", "${item.NewInvestmentAmount}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
												</td>
												<td class="ralign">
													<%: Html.Span("${formatCurrency(item.ExistingInvestmentAmount)}", new { @class = "show" })%>
													<%: Html.TextBox("${i}_ExistingInvestmentAmount", "${item.ExistingInvestmentAmount}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
												</td>
												<td class="ralign">
													<%: Html.Span("${formatCurrency(item.ManagementFees)}", new { @class = "show" })%>
													<%: Html.TextBox("${i}_ManagementFees", "${item.ManagementFees}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
												</td>
												<td class="ralign">
													<%: Html.Span("${formatCurrency(item.FundExpenses)}", new { @class = "show" })%>
													<%: Html.TextBox("${i}_FundExpenses", "${item.FundExpenses}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
												</td>
												<td class="ralign">
													<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:modifyCapitalCall.edit(this);" })%>
													<%: Html.Hidden("${i}_CapitalCallID", "${item.CapitalCallID}")%>
													<%: Html.Hidden("${i}_CapitalCallLineItemID", "${item.CapitalCallLineItemID}")%>
												</td>
											</tr>
										{{/each}}
									</tbody>
								</table>
								<% Html.RenderPartial("TBoxBottom"); %>
							</div>
							<div class="editor-button" style="margin: 0 0 0 46%; padding-top: 10px; width: auto;">
								<div style="float: left; padding: 0 0 10px 5px;">
									<%: Html.Image("ModifyCC_active.png", new { @class = "default-button", @onclick = "javascript:modifyCapitalCall.save('CapitalCall');" })%>
								</div>
								<div style="float: left; padding: 0 0 10px 5px;">
									<%: Html.Span("", new { @id = "UpdateLoading" })%>
								</div>
							</div>
						</div>
						<div class="line">
						</div>
						<% } %>
					</div>
					<div id="UpdateTargetId" style="display: none">
					</div>
				</div>
			</div>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl"> 
		{{each(i,row) rows}}
		<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
			<td>
				<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
			</td>
			<td>
				<%: Html.Span("${row.cell[2]}", new { @class = "show" })%>
				<%: Html.TextBox("CapitalAmountCalled", "${row.cell[2]}", new { @class = "hide" })%>
			</td>
			<td style="text-align:right;">
				{{if row.cell[0]==0}}
				<%: Html.Image("add_active.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:modifyCapitalCall.save(this,${row.cell[0]});" })%>
				{{else}}
				<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:modifyCapitalCall.save(this,${row.cell[0]});" })%>
				<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:modifyCapitalCall.edit(this);" })%>
				<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:modifyCapitalCall.deleteRow(this,${row.cell[0]});" })%>
				{{/if}}
				<%: Html.Hidden("ShareClassTypeId", "${row.cell[0]}") %>
			</td>
		</tr>
		{{/each}}
	</script>
	<script type="text/javascript">		modifyCapitalCall.init()</script>
</asp:Content>

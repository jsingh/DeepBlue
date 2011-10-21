<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateCapitalCallModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Capital Call
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
					<%: Html.HiddenFor(model => model.CapitalCallNumber)%>
				</div>
				<div class="cell">
					<label>
						Committed Amount&nbsp;<%: Html.Span("", new { @id = "CommittedAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						Unfunded Amount&nbsp;<%: Html.Span("", new { @id = "UnfundedAmount", @style = "padding-left:10px;" })%></label></div>
				<div class="cell auto">
					<label>
						<%: Html.Anchor("Previous Capital Calls","#", new { @id="lnkPCC", @target = "_blank", @style="color:Blue" })%>
					</label>
				</div>
			</div>
		</div>
		<div id="NewCapitalCall">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "CapitalCall", @onsubmit = "return false" })) {%>
				<%: Html.HiddenFor(model => model.CapitalCallLineItemsCount)%>
				<%: Html.HiddenFor(model => model.CapitalCallID) %>
				<div class="line">
				</div>
				<div class="cc-box-det">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.CapitalCallNumber)%>
					</div>
					<div class="editor-field">
						<b>
							<%: Html.DisplayFor(model => model.CapitalCallNumber)%></b>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.CapitalAmountCalled)%>
					</div>
					<div class="editor-field">
						<%: Html.NumericTextBoxFor(model => model.CapitalAmountCalled, new { @onkeydown = "return jHelper.isCurrency(event);", @style = "width:110px", @onkeyup = "javascript:modifyCapitalCall.calcExistingInvestmentAmount();" })%>
					</div>
					<div class="editor-label" style="clear: right; width: auto;">
						<%: Html.LabelFor(model => model.CapitalCallDate) %>
					</div>
					<div class="editor-field">
						<%: Html.EditorFor(model => model.CapitalCallDate, new { @style = "width:110px" })%>
					</div>
					<div class="editor-label" id="ccduedatelbl" style="clear: right; width: auto;">
						<%: Html.LabelFor(model => model.CapitalCallDueDate) %>
					</div>
					<div class="editor-field">
						<%: Html.EditorFor(model => model.CapitalCallDueDate, new { @style = "width:110px" })%>
					</div>
					<div class="editor-label">
						<%: Html.Span("Add Management Fees", new { @id = "SpnAddManagementFee" })%>
					</div>
					<div class="editor-field">
						<%: Html.CheckBox("AddManagementFees", (Model.AddManagementFees ?? false), new { @style = "width:auto;", @displaywidth = "118px", @display = "", @onclick = "javascript:modifyCapitalCall.selectMFee(this);" })%>
					</div>
					<%if ((Model.AddManagementFees ?? false) == true) { %>
					<div id="ManFeeMain" style="float: left;">
						<%}
	   else {%>
						<div id="ManFeeMain" style="display: none; float: left;">
							<%} %>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.FromDate) %>
							</div>
							<div class="editor-field">
								<%: Html.EditorFor(model => model.FromDate, new { @class = "datetxt", @id = "FromDate", @style="width:111px" })%>&nbsp;<%: Html.LabelFor(model => model.ToDate) %>&nbsp;<%: Html.EditorFor(model => model.ToDate, new { @class = "datetxt", @id = "ToDate", @style = "width:111px" })%>
							</div>
							<div class="editor-label" id="feeamountlbl" style="clear: right;">
								Fee Amount
							</div>
							<div class="editor-field">
								<%: Html.NumericTextBoxFor(model => model.ManagementFees, new { @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:modifyCapitalCall.calcExistingInvestmentAmount();" })%>
							</div>
						</div>
						<div class="editor-label">
							<%: Html.Span("Add Fund Expenses", new { @id = "SpnAddFundExpenses" })%>
						</div>
						<div class="editor-field">
							<%: Html.CheckBox("AddFundExpenses", (Model.AddFundExpenses  ?? false), new { @style = "width:auto;", @displaywidth = "118px", @display = "", @onclick = "javascript:modifyCapitalCall.selectFundExp(this);" })%>
						</div>
						<%if ((Model.AddManagementFees ?? false) == true) { %>
						<div id="FunExpAmount" style="float: left;">
							<%}
		else {%>
							<div id="FunExpAmount" style="display: none; float: left;">
								<%} %>
								<div class="editor-label">
									Fund Expense Amount
								</div>
								<div class="editor-field">
									<%: Html.NumericTextBoxFor(model => model.FundExpenseAmount, new { @class = "datetxt", @style = "width:111px", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:modifyCapitalCall.calcExistingInvestmentAmount();" })%>
								</div>
							</div>
							<div class="editor-label">
								Capital Call Split For
							</div>
							<div class="editor-label">
								<%: Html.LabelFor(model => model.NewInvestmentAmount) %>
							</div>
							<div class="editor-field">
								<%: Html.NumericTextBoxFor(model => model.NewInvestmentAmount, new { @class = "datetxt", @style = "width:110px;", @onkeydown = "return jHelper.isCurrency(event);", @onkeyup = "javascript:modifyCapitalCall.calcExistingInvestmentAmount();" })%>
							</div>
							<div class="editor-label" style="clear: right">
								<%: Html.LabelFor(model => model.ExistingInvestmentAmount) %>
							</div>
							<div class="editor-field">
								<%: Html.Span(FormatHelper.CurrencyFormat(Model.ExistingInvestmentAmount ?? 0), new { @id = "SpnExistingInvestmentAmount" })%>
								<%: Html.HiddenFor(model => model.ExistingInvestmentAmount)%>
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
										<%int rowIndex = 0;%>
										<%foreach (var item in Model.CapitalCallLineItems) {%>
										<%rowIndex++;%>
										<%if ((rowIndex % 2) == 0) {%>
										<tr id="Row<%=rowIndex.ToString()%>" class="arow">
											<%}
			else {%><tr id="Row<%=rowIndex.ToString()%>" class="row">
											<%} %>
											<td class="lalign">
												<%=item.InvestorName%>
											</td>
											<td class="ralign">
												<%: Html.Span(FormatHelper.CurrencyFormat(item.CapitalAmountCalled), new { @class = "show" })%>
												<%: Html.TextBox(rowIndex.ToString() + "_" + "CapitalAmountCalled", item.CapitalAmountCalled, new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
											</td>
											<td class="ralign">
												<%: Html.Span(FormatHelper.CurrencyFormat(item.InvestmentAmount), new { @class = "show" })%>
												<%: Html.TextBox(rowIndex.ToString() + "_" + "InvestmentAmount", item.InvestmentAmount, new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
											</td>
											<td class="ralign">
												<%: Html.Span(FormatHelper.CurrencyFormat(item.NewInvestmentAmount), new { @class = "show" })%>
												<%: Html.TextBox(rowIndex.ToString() + "_" + "NewInvestmentAmount", item.NewInvestmentAmount, new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
											</td>
											<td class="ralign">
												<%: Html.Span(FormatHelper.CurrencyFormat(item.ExistingInvestmentAmount), new { @class = "show" })%>
												<%: Html.TextBox(rowIndex.ToString() + "_" + "ExistingInvestmentAmount", item.ExistingInvestmentAmount, new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
											</td>
											<td class="ralign">
												<%: Html.Span(FormatHelper.CurrencyFormat(item.ManagementFees), new { @class = "show" })%>
												<%: Html.TextBox(rowIndex.ToString() + "_" + "ManagementFees", item.ManagementFees, new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
											</td>
											<td class="ralign">
												<%: Html.Span(FormatHelper.CurrencyFormat(item.FundExpenses), new { @class = "show" })%>
												<%: Html.TextBox(rowIndex.ToString() + "_" + "FundExpenses", item.FundExpenses, new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
											</td>
											<td class="ralign">
												<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:modifyCapitalCall.edit(this);" })%>
												<%: Html.Hidden(rowIndex.ToString() + "_" + "CapitalCallID", item.CapitalCallID)%>
												<%: Html.Hidden(rowIndex.ToString() + "_" + "CapitalCallLineItemID", item.CapitalCallLineItemID)%>
											</td>
										</tr>
											<%}%>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("CapitalCallDate")%>
	<%= Html.jQueryDatePicker("CapitalCallDueDate")%>
	<%= Html.jQueryDatePicker("ManCapitalCallDate")%>
	<%= Html.jQueryDatePicker("ManCapitalCallDueDate")%>
	<%= Html.jQueryDatePicker("FromDate")%>
	<%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { modifyCapitalCall.selectFund(ui.item.id);}"})%>
	<script id="CapitalCallInvestorTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("CapitalCallInvestorDetail"); %>
	</script>
	<script type="text/javascript">
	 
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
	<script type="text/javascript">
		$(document).ready(function () {
			var bdy=$("body");
			jHelper.jqCheckBox(bdy);
			jHelper.jqComboBox(bdy);
		});
	</script>
	<%if (Model.FundId > 0) {%>
	<script type="text/javascript">$(document).ready(function(){modifyCapitalCall.selectFund(<%=Model.FundId%>);});</script>
	<%}%>
</asp:Content>

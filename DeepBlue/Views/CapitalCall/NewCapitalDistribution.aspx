<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateDistributionModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Capital Distribution
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.fileuploader.js")%>
	<%=Html.JavascriptInclueTag("CapitalCallDistribution.js")%>
	<%=Html.JavascriptInclueTag("CapitalCallDistributionManual.js")%>
	<%=Html.JavascriptInclueTag("ImportCapitalDistributionExcel.js")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">FUNDS</span><span class="arrow"></span><span class="pname">Capital Distribution</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="cc-box">
		<div class="header">
			<div class="tabbg">
				<div class="tabinnerbox">
					<%using (Html.Tab(new { @id = "NewCDTab", @class = "section-tab-sel", @onclick = "javascript:distribution.selectTab('C',this);" })) {%>New Capital Distribution
					<%}%>
					<%using (Html.Tab(new { @id = "ManCDTab", @class = "section-tab", @onclick = "javascript:distribution.selectTab('M',this);" })) {%>Manual Capital Distribution
					<%}%>
					<%using (Html.Div(new { @id = "SerCDTab" })) {%>
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...&nbsp;", new { @id = "SpnLoading", @style = "display:none;float:left;" })%><%: Html.TextBox("Fund", "SEARCH  FUND", new { @class = "wm", @style = "width:200px" })%>
					<%}%>
				</div>
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
		<div id="ManualCapitalDistribution" style="display: none">
			<div class="cc-box-main">
				<% using (Html.Form(new { @id = "ManualDistribution", @onsubmit = "return false" })) {%>
				<%: Html.HiddenFor(model => model.DistributionAmount) %>
				<%: Html.HiddenFor(model => model.ReturnManagementFees)%>
				<%: Html.HiddenFor(model => model.ReturnFundExpenses)%>
				<%: Html.HiddenFor(model => model.GPProfits)%>
				<%: Html.HiddenFor(model => model.PreferredReturn)%>
				<%: Html.HiddenFor(model => model.CapitalReturn)%>
				<div class="line">
				</div>
				<div class="cc-box-det dist-detail">
					<div class="editor-label">
						<%: Html.LabelFor(model => model.DistributionNumber) %>
					</div>
					<div class="editor-field">
						<%: Html.Span("",new { @id= "SpnManualDistributionNumber"})%>
					</div>
					<div class="editor-label" style="clear: right">
						<%: Html.LabelFor(model => model.CapitalDistributionDate) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("CapitalDistributionDate", "", new { @id = "ManCapitalDistributionDate" })%>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.CapitalDistributionDueDate) %>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("CapitalDistributionDueDate","", new { @id = "ManCapitalDistributionDueDate" })%>
					</div>
					<div class="editor-label">
						<%: Html.LabelFor(model => model.DistributionAmount) %>
					</div>
					<div class="editor-field">
						<%: Html.Span("", new { @id = "SpnDistributionAmount" })%>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.ReturnManagementFees) %>
					</div>
					<div class="editor-field">
						<%: Html.Span("", new { @id = "SpnReturnManagementFees" })%>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.LabelFor(model => model.ReturnFundExpenses) %>
					</div>
					<div class="editor-field">
						<%: Html.Span("", new { @id = "SpnReturnFundExpenses" })%>
					</div>
					<div class="editor-label">
						<%: Html.Label("Profits Returned")%>
					</div>
					<div class="editor-field">
						<%: Html.Span("", new { @id = "SpnPreferredReturn" })%>
					</div>
					<div class="editor-label" style="clear: right;">
						<%: Html.Label("Cost Returned")%>
					</div>
					<div class="editor-field">
						<%: Html.Span("", new { @id = "SpnCapitalReturn" })%>
					</div>
				</div>
				<div class="line">
				</div>
				<div class="closetitle">
					<div class="title">
						Investors</div>
					<%using (Html.BlueButton(new { @onclick = "javascript:manualDistribution.addInvestor();" })) {%>Add New Investor<%}%>
				</div>
				<div id="InvestorDetail" class="dc-box tabledetail">
					<div class="gbox" style="width: 90%">
						<table cellpadding="0" cellspacing="0" id="InvestorList" class="grid">
							<thead>
								<tr>
									<th style="width: 15%">
										Investor Name
									</th>
									<th style="text-align: right">
										Capital Distribution Amount
									</th>
									<th style="text-align: right">
										Return Management Fees
									</th>
									<th style="text-align: right">
										Return Fund Expenses
									</th>
									<th style="text-align: right">
										Cost Returned
									</th>
									<th style="width: 10%; text-align: right">
										Profits (%)
									</th>
									<th style="text-align: right">
										Profits Returned
									</th>
									<th style="width: 5%" align="center">
									</th>
								</tr>
							</thead>
							<tbody>
							</tbody>
						</table>
					</div>
				</div>
				<%: Html.HiddenFor(model => model.InvestorCount)%>
				<div style="margin: 0 auto; padding-top: 10px; width: 320px;">
					<div style="float: left; padding: 0 0 10px 5px;">
						<%: Html.Span("", new { @id = "ManualUpdateLoading" })%>
					</div>
					<div style="float: left; padding: 0 0 10px 5px;">
						<%: Html.Image("submit_active.png", new { @class = "default-button", @onclick = "javascript:manualDistribution.save('ManualDistribution');" })%>
					</div>
					<div style="float: left; margin-left: 20px;">
						<%: Html.ImageButton("Import-Excel_active.png", new { @id = "btnImportInvestor", onclick = "javascript:$('#ExcelImport').dialog('open');" })%>
					</div>
					<div class="clear">
						&nbsp;</div>
				</div>
				<div class="line">
				</div>
				<%}%>
			</div>
		</div>
	</div><div id="ExcelImport">
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
	<%using (Html.jQueryTemplateScript("ExcelImprtTemplate")) {%>
	<div class="import-box">
		<%using (Html.Form(new { @id = "frmUploadExcel", @onsubmit = "return false" })) { %>
		<div class="editor-label" style="width: 110px;">
			&nbsp;</div>
		<div class="editor-field" style="text-align: right; font-size: 11px;">
			<%:Html.Anchor("Sample Excel", "/Files/ImportSamples/ManualCapitalDistribution.xlsx", new { @target = "_blank", @style = "color:blue" })%>
		</div>
		<div class="editor-label" style="width: 110px;">
			<%: Html.Label("File")%></div>
		<div class="editor-field">
			<%: Html.File("UploadFile", new { @id = "UploadFile" })%>
		</div>
		<div class="editor-label" style="width: 100px">
			<%: Html.Span("", new { @id = "SpnUELoading" })%>
		</div>
		<div class="editor-label" style="clear: right; width: auto;">
			<%: Html.Image("Upload_active.png", new { @onclick = "javascript:importCapitalDistributionExcel.uploadExcel();" })%></div>
		<div class="editor-field">
			<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:$('#ExcelImport').dialog('close');" })%></div>
		<%}%>
	</div>
	<div id="ImportExcel">
	</div>
	<div id="ProgressBar">
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ImportExcelTemplate")) {%>
	<br />
	<div class="clear">
		&nbsp;</div>
	<div class="capitaldistimportsection" id="CapitalDistributionDetailBox">
		<div class="formbox">
			<%using (Html.Form(new { @id = "frm", @onsubmit = "return false" })) { %>
			<div class="editor-label">
				<%: Html.Label("Excel Tab")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ManualCapitalDistributionTableName", new List<SelectListItem>() {
	 new  SelectListItem { Text = "--Select Excel Tab--", Value = " " }
}, new { @exceltabname = "CapitalDistribution", @class = "ddltable", @onchange = "javascript:importCapitalDistributionExcel.selectExcelTab(this);" })%></div>
			<div class="editor-label">
				<%: Html.Label("InvestorName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("InvestorName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("FundName")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("FundName", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("CapitalDistributionAmount")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("CapitalDistributionAmount", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("ReturnManagementFees")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ReturnManagementFees", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("ReturnFundExpenses")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ReturnFundExpenses", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("CostReturned")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("CostReturned", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("Profits")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("Profits", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("ProfitsReturned")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("ProfitsReturned", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label">
				<%: Html.Label("DistributionDate")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DistributionDate", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("DistributionDueDate")%></div>
			<div class="editor-field">
				<%: Html.DropDownList("DistributionDueDate", DeepBlue.Helpers.SelectListFactory.GetEmptySelectList())%></div>
			<div class="clear">
				&nbsp;</div>
			<%: Html.Hidden("TotalRows", "${TotalRows}")%>
			<%: Html.Hidden("SessionKey", "${SessionKey}")%>
			<%}%>
		</div>
		<div class="statusbox">
		</div>
	</div>
	<div class="save-box">
		<div class="editor-label">
			<%: Html.Image("Save_active.png", new {  @onclick = "javascript:importCapitalDistributionExcel.import(this);" })%></div>
		<div class="editor-field">
			<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:$('#ExcelImport').dialog('close');" })%></div>
		<div class="clear">
			&nbsp;</div>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("ImportExcelResultTemplate")) {%>
	<div class="editor-label">
		<%: Html.Label("Total")%></div>
	<div class="editor-field">
		<%: Html.Span("${TotalRows}", new { @id = "spntotal" })%></div>
	<div class="editor-label">
		<%: Html.Label("Success")%></div>
	<div class="editor-field">
		<%: Html.Span("${SuccessRows}", new { @id = "spnsuccess" })%></div>
	<div class="editor-label">
		<%: Html.Label("Errors")%></div>
	<div class="editor-field">
		<%: Html.Span("${ErrorRows}", new { @id = "spnerrors" })%></div>
	<div class="editor-field">
		<%: Html.Span("", new { @id = "spnerrorexcel", @style="color:red" })%></div>
	<div class='prs-bar'>
		<div class='total-rows'>
			Rows ${CompletedRows} Of ${TotalRows}</div>
		<div class="status-bar">
			<div class='loading-status' style='width: ${Percent}%;'>
			</div>
		</div>
	</div>
	<%}%>
</asp:Content>

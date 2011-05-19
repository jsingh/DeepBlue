<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.CapitalCall.CreateReceiveModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Receive Capital Call
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
	<%=Html.StylesheetLinkTag("capitalcall.css")%>
	<%=Html.JavascriptInclueTag("CapitalCallReceive.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("CreateReceiveCapitalCall", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "capitalCallReceive.onCreateCapitalCallBegin", OnSuccess = "capitalCallReceive.onCreateCapitalCallSuccess" }, new { @id = "CapitalCallReceive" })) {%>
	<div class="cc-header">
		<div class="page-title">
			<h2>
				Receive Capital Call</h2>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FundName) %>&nbsp;<%: Html.TextBoxFor(model => model.FundName, new { @style = "width:200px" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
			<%: Html.LabelFor(model => model.CapitalCallNumber)%>&nbsp;<%: Html.DropDownListFor(model => model.CapitalCallId, Model.CapitalCalls, new { @style = "width:150px", @onchange = "javascript:capitalCallReceive.selectCapitalCall(this.value);" })%>
		</div>
	</div>
	<div class="cc-main" id="CaptialCallDetail">
	</div>
	<%: Html.HiddenFor(model => model.ItemCount)%>
	<%: Html.Hidden("CapitalCall",Model.CapitalCallId)%>
	<%: Html.HiddenFor(model => model.FundId)%>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { capitalCallReceive.selectFund(ui.item.id);}" })%>
	<script type="text/javascript">
		capitalCallReceive.init();
	</script>
	<script id="CCLItemTemplate" type="text/x-jquery-tmpl">  
	<div class="box">
		<div class="box-top">
			<div class="box-left">
			</div>
			<div class="box-center">
				Fund:&nbsp;
				<%:Html.Span("${FundName}", new { id = "TitleFundName" })%>
			</div>
			<div class="box-right">
			</div>
		</div>
		<div class="box-content">
			<div class="editor-label">
				<%: Html.LabelFor(model => model.CapitalAmountCalled)%>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("CapitalAmountCalled", "${CapitalAmountCalled}", new { @id = "CapitalAmountCalled" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.LabelFor(model => model.CapitalCallDate)%>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("CapitalCallDate", "${CapitalCallDate}" , new { @id = "CapitalCallDate" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.LabelFor(model => model.CapitalCallDueDate)%>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("CapitalCallDueDate", "${CapitalCallDueDate}" , new { @id = "CapitalCallDueDate" })%>
			</div>
			<div id="CapitalCallItems" class="list">
				<table cellpadding="0" cellspacing="0" border="0" id="InvestorDetail">
					<thead>
						<tr>
							<th style="width: 10%">
								Investor Name
							</th>
							<th style="width: 15%">
								Capital Call Amount
							</th>
							<th style="width: 10%">
								Management Fees
							</th>
							<th style="width: 15%">
								Investment Amount
							</th>
							<th style="width: 15%">
								Management Fee Interest
							</th>
							<th style="width: 15%">
								Invested Amount Interest
							</th>
							<th style="width: 10%">
								Received
							</th>
							<th style="width: 10%">
								Received Date
							</th>
						</tr>
					</thead>
					<tbody>
						{{each Items}}
						<tr>
							<td style="width: 10%">
								<div>
									<%: Html.Span("${InvestorName}",new { @id="InvestorName"})%>
									<%: Html.Hidden("${Index}_CapitalCallLineItemId","${CapitalCallLineItemId}",new { @id="CapitalCallLineItemId"}) %>
								</div>
							</td>
							<td style="width: 15%">
								<div>
									<%: Html.TextBox("${Index}_"+ "CapitalAmountCalled","${CapitalAmountCalled}",new {  @id="CapitalAmountCalled", @onkeypress="return jHelper.isCurrency(event);" })%></div>
							</td>
							<td style="width: 10%">
								<div>
									<%: Html.TextBox("${Index}_" + "ManagementFees", "${ManagementFees}", new {   @id = "ManagementFees", @onkeypress = "return jHelper.isCurrency(event);" })%></div>
							</td>
							<td style="width: 15%">
								<div>
									<%: Html.TextBox("${Index}_" + "InvestmentAmount", "${InvestmentAmount}", new {   @id = "InvestmentAmount", @onkeypress = "return jHelper.isCurrency(event);" })%></div>
							</td>
							<td style="width: 15%">
								<div>
									<%: Html.TextBox("${Index}_" + "ManagementFeeInterest", "${ManagementFeeInterest}", new {   @id = "ManagementFeeInterest", @onkeypress = "return jHelper.isCurrency(event);" })%></div>
							</td>
							<td style="width: 15%">
								<div>
									<%: Html.TextBox("${Index}_" + "InvestedAmountInterest", "${InvestedAmountInterest}", new { @id = "InvestedAmountInterest", @onkeypress = "return jHelper.isCurrency(event);" })%></div>
							</td>
							<td style="width: 10%">
								<div>
									<%: Html.CheckBox("${Index}_" + "Received", new { @checkvalue="${Received}", @id = "Received" ,   @onclick="javascript:capitalCallReceive.selectReceive(this);" })%></div>
							</td>
							<td style="width: 10%">
								<div>
									<%: Html.TextBox("${Index}_" + "ReceivedDate", "${ReceivedDate}", new {   @id = "${Index}_ReceivedDate" , @rdate = "true" })%></div>
							</td>
						</tr>
						{{/each}}
					</tbody>
				</table>
			</div>
			<div class="status">
				<%: Html.Span("", new { id = "UpdateLoading" })%></div>
			<div class="editor-button">
				<div style="float: left; padding: 0 0 10px 5px;">
					<%: Html.ImageButton("submit.png", new { @class="default-button", @onclick = "javascript:capitalCallReceive.onSubmit('CapitalCallReceive');" })%>
				</div>
				<div style="float: left; padding: 0 0 10px 5px;">
					<%: Html.Span("", new { @id = "UpdateLoading" })%>
				</div>
			</div>
		</div>
	</div>
	</script>
</asp:Content>

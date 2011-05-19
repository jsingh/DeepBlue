<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateDealCloseModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal Closing
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%=Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%=Html.JavascriptInclueTag("dealClose.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealclose.css")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateDealClosing", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "dealClose.onCreateBegin",
		 OnSuccess = "dealClose.onCreateSuccess"
	 }, new { @id = "AddNewDealClosing" })) {%>
	<div class="editor-label nowrap auto-width">
		<%: Html.LabelFor(model => model.DealNumber) %>&nbsp;<b><%: Html.Span(Model.DealNumber.ToString()) %></b>
	</div>
	<div class="editor-label rightcol auto-width">
		<%: Html.LabelFor(model => model.CloseDate) %>&nbsp;<%: Html.EditorFor(model => model.CloseDate) %>
	</div>
	<div class="editor-label rightcol auto-width">
		<%: Html.LabelFor(model => model.IsFinalClose) %>&nbsp;
		<%: Html.CheckBoxFor(model => model.IsFinalClose, new { @onclick = "javascript:dealClose.finalClose(this);" })%>
	</div>
	<div id="DealDetail">
		<div class="editor-label nowrap">
			<b>Deal Underlying Funds</b>
		</div>
		<div id="DealUnderlyingFunds" class="closebox">
			<table cellspacing="0" cellpadding="0" border="0" class="grid" id="tblDealUnderlyingFund">
				<thead>
					<tr>
						<th id="thCHK" style="width: 10%; display: none;">
						</th>
						<th style="width: 25%">
							Fund Name
						</th>
						<th id="thCommitmentAmount" style="width: 25%">
							Commitment Amount
						</th>
						<th id="thReassignedGPP" style="width: 25%; display: none;">
							Reassigned GPP
						</th>
						<th id="thGPP" style="width: 12%">
							Gross Purchase Price
						</th>
						<th style="width: 12%">
							Post Record Capital Call
						</th>
						<th style="width: 16%">
							Post Record Distribution
						</th>
					</tr>
				</thead>
				<tbody>
					<%foreach (var dealUnderlyingFund in Model.DealUnderlyingFunds) {%>
					<tr class="emptyrow">
						<td colspan="5">
							&nbsp;
						</td>
					</tr>
					<tr>
						<td style="display: none;">
							<%: Html.InputCheckBox("DealUnderlyingFundId", (dealUnderlyingFund.DealClosingId == Model.DealClosingId), new {  @value=dealUnderlyingFund.DealUnderlyingFundId })%>
						</td>
						<td>
							<%: Html.Literal(dealUnderlyingFund.FundName)%>
						</td>
						<td>
							<%: Html.TextBox(dealUnderlyingFund.DealUnderlyingFundId + "_" + "CommittedAmount", dealUnderlyingFund.CommittedAmount, new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
						</td>
						<td style="display: none;">
							<%: Html.TextBox(dealUnderlyingFund.DealUnderlyingFundId + "_" + "ReassignedGPP", dealUnderlyingFund.ReassignedGPP, new { @id="ReassignedGPP", @onkeyup = "javascript:dealClose.calcAmount();", @onkeypress = "return jHelper.isCurrency(event);" })%>
						</td>
						<td>
							<%: Html.TextBox(dealUnderlyingFund.DealUnderlyingFundId + "_" + "GrossPurchasePrice", dealUnderlyingFund.GrossPurchasePrice, new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
						</td>
						<td>
							<%: Html.TextBox(dealUnderlyingFund.DealUnderlyingFundId + "_" + "PostRecordDateCapitalCall", dealUnderlyingFund.PostRecordDateCapitalCall, new { @id = "PostRecordDateCapitalCall", @onkeyup = "javascript:dealClose.calcAmount();", @onkeypress = "return jHelper.isCurrency(event);" })%>
						</td>
						<td>
							<%: Html.TextBox(dealUnderlyingFund.DealUnderlyingFundId + "_" + "PostRecordDateDistribution", dealUnderlyingFund.PostRecordDateDistribution, new { @id = "PostRecordDateDistribution", @onkeyup = "javascript:dealClose.calcAmount();", @onkeypress = "return jHelper.isCurrency(event);" })%>
						</td>
					</tr>
					<%}%>
				</tbody>
			</table>
		</div>
		<br />
		<div class="editor-label  nowrap">
			<b>Deal Underlying Directs</b>
		</div>
		<div id="DealUnderlyingDirects" class="closebox">
			<table cellspacing="0" cellpadding="0" border="0" class="grid" id="tblDealUnderlyingDirect">
				<thead>
					<tr>
						<th id="thCHK" style="width: 10%; display: none;">
						</th>
						<th style="width: 20%">
							No Of Shares
						</th>
						<th style="width: 20%">
							Purchase Price
						</th>
						<th style="width: 18%">
							Fair Market Value
						</th>
						<th>
							&nbsp;
						</th>
						<th>
							&nbsp;
						</th>
					</tr>
				</thead>
				<tbody>
					<%foreach (var dealUnderlyingDirect in Model.DealUnderlyingDirects) {%>
					<tr class="emptyrow">
						<td colspan="5">
							&nbsp;
						</td>
					</tr>
					<tr>
						<td style="display: none">
							<%:Html.InputCheckBox("DealUnderlyingDirectId", (dealUnderlyingDirect.DealClosingId == Model.DealClosingId), new {  @value = dealUnderlyingDirect.DealUnderlyingDirectId })%>
						</td>
						<td>
							<%: Html.TextBox(dealUnderlyingDirect.DealUnderlyingDirectId + "_" + "NumberOfShares", dealUnderlyingDirect.NumberOfShares, new {  @onkeypress = "return jHelper.isNumeric(event);" })%>
						</td>
						<td>
							<%: Html.TextBox(dealUnderlyingDirect.DealUnderlyingDirectId + "_" + "PurchasePrice", dealUnderlyingDirect.PurchasePrice, new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
						</td>
						<td>
							<%: Html.TextBox(dealUnderlyingDirect.DealUnderlyingDirectId + "_" + "FMV", dealUnderlyingDirect.FMV, new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
						</td>
					</tr>
					<%}%>
				</tbody>
			</table>
		</div>
		<div class="editor-label  nowrap">
			<b>Deal Total</b>
		</div>
		<div id="DealTotal" class="closebox">
			<div class="editor-label auto-width">
				Total Reassigned GPP:&nbsp;<b><span id="SpnRGPP">
					<%if (Model.DealUnderlyingFunds.Count > 0) { %>
					$<%=Model.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.ReassignedGPP)%>
					<%}%></span></b>&nbsp;&nbsp;Total Post Record Capital Call:&nbsp;<b><span id="SpnPRCC">
						<%if (Model.DealUnderlyingFunds.Count > 0) { %>
						$<%=Model.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.PostRecordDateCapitalCall)%>
						<%}%>
					</span></b>&nbsp;&nbsp;Total Post Record Capital Distribution:&nbsp;<b><span id="SpnPRCD">
						<%if (Model.DealUnderlyingFunds.Count > 0) { %>
						$<%=Model.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.PostRecordDateCapitalCall)%>
						<%}%></span></b>
			</div>
		</div>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return dealClose.onSubmit('AddNewDealClosing');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.dealClose.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.ValidationMessageFor(model => model.CloseDate) %>
	<%: Html.ValidationMessageFor(model => model.DealId)%>
	<%: Html.ValidationMessageFor(model => model.DealNumber)%>
	<%: Html.ValidationMessageFor(model => model.DealClosingId)%>
	<%: Html.ValidationMessageFor(model => model.DealUnderlyingFunds)%>
	<%: Html.ValidationMessageFor(model => model.DealUnderlyingDirects)%>
	<%: Html.HiddenFor(model => model.DealId)%>
	<%: Html.HiddenFor(model => model.DealNumber)%>
	<%: Html.HiddenFor(model => model.DealClosingId)%>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryDatePicker("CloseDate")%>
	<script type="text/javascript">
		dealClose.init();
	</script>
</asp:Content>

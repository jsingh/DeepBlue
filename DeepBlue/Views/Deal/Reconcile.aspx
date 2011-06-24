<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.ReconcileModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Reconcile
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("dealreconcile.css")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("DealReconcile.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="search-header">
		<div class="line">
		</div>
		<div class="title">
			<div class="left-col">
				Reconcile</div>
			<%using (Html.Form(new { @id = "frmReconcile", @onsubmit = "return false;" })) {%>
			<div class="left-col" style="margin-left: 10px; display: none" id="ReportLoading">
				<%:Html.Image("ajax.jpg")%>&nbsp;Loading....</div>
			<div class="left-col" style="margin-left: 15%;">
				<%: Html.Label("Date Range:") %>&nbsp;<%: Html.EditorFor(model => model.FromDate)%>&nbsp;To&nbsp;<%:Html.EditorFor(model => model.ToDate)%>&nbsp;&nbsp;<%: Html.TextBox("Fund", "SEARCH FUND", new { @class = "wm", @id = "Fund", @style = "width:180px", @onblur = "javascript:dealReconcile.clearFund(this);" })%>
			</div>
			<div class="left-col" style="margin-left: 20px;">
				<%: Html.ImageButton("search.png", new { @onclick = "javascript:dealReconcile.submit();" })%></div>
			<div class="left-col" style="margin-left: 5px;">
				<%: Html.Span("", new { @id = "SpnLoading" })%></div>
			<%: Html.HiddenFor(model => model.UnderlyingFundId)%>
			<%: Html.HiddenFor(model => model.FundId)%>
			<%: Html.HiddenFor(model => model.PageIndex)%>
			<%: Html.HiddenFor(model => model.PageSize)%>
			<%}%>
		</div>
		<div class="line">
		</div>
	</div>
	<div class="search-content">
		<div class="grid-header">
			<table cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th style="width: 20%;">
							Underlying Fund Name
						</th>
						<th style="width: 20%;">
							Fund Name
						</th>
						<th style="width: 15%;">
							Capital Call Amount
						</th>
						<th style="width: 15%;">
							Distribution Amount
						</th>
						<th style="width: 15%;">
							Payment Date / Received Date
						</th>
						<th style="width: 15%;">
							Paid On / Received On
						</th>
					</tr>
				</thead>
			</table>
		</div>
		<div id="ReconcileBox" class="grid-body">
			<table cellpadding="0" cellspacing="0" border="0" class="grid" id="ReconcileList">
				<tbody>
				</tbody>
				<tfoot style="display: none">
					<tr>
						<td colspan="6">
							<%: Html.Anchor("View More", "javascript:dealReconcile.viewMore();", new { @style = "color: #000;" })%>
						</td>
					</tr>
				</tfoot>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		dealReconcile.init();</script>
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealReconcile.setFund(ui.item.id,ui.item.value);}"
	})%>
	<%= Html.jQueryDatePicker("FromDate")%><%= Html.jQueryDatePicker("ToDate")%>
	<script id="ReconcileTemplate" type="text/x-jquery-tmpl">
	<tr {{if EventType>2}}class="blue"{{else}}class="red"{{/if}}>
		<td style="text-align:center;width:20%;">
			${UnderlyingFundName}
		</td>
		<td style="text-align:center;width:20%;">
			${FundName}
		</td>
		<td style="text-align:right;width:15%;">
			${FormatCCAmount}
		</td>
		<td style="text-align:right;width:15%;">
			${FormatDAmount}
		</td>
		<td style="text-align:center;width:15%;">
			${FormatPRDate}
		</td>
		<td style="width:15%;">&nbsp;
		</td>
	</tr><tr class=emptyrow><td colspan=6>&nbsp;</td></tr>
	</script>
</asp:Content>

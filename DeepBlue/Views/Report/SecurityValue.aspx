<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Report.SecurityValueModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Security Value
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("SecurityValue.js")%>
	<%=Html.JavascriptInclueTag("jquery.PrintArea.js")%>
	<%=Html.StylesheetLinkTag("report.css")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">REPORTS</span><span class="arrow"></span><span class="pname">SecurityValue</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div id="ReportHeader">
		<div class="titlebox">
			<% Html.EnableClientValidation(); %>
			<%using (Html.Form(new { @id = "frmSecurityValue", @onsubmit = "return securityValueReport.onSubmit(this);" })) {%>
			<div class="editor-label" style="width: auto;">
				<%: Html.LabelFor(model => model.FundId)%>
				<%: Html.TextBox("FundName", "SEARCH AMBERBROOK FUND", new { @class = "wm", @id = "FundName", @style = "width:200px" })%>
			</div>
			<div class="editor-label" style="width: auto; clear: right;">
				<%: Html.LabelFor(model => model.StartDate)%>
				<%: Html.TextBoxFor(model => model.StartDate)%>
			</div>
			<div class="editor-label" style="width: auto; clear: right;">
				<%: Html.LabelFor(model => model.EndDate)%>
				<%: Html.TextBoxFor(model => model.EndDate)%>
			</div>
			<div class="editor-label" style="width: auto; clear: right;">
				<%: Html.ImageButton("submit_active.png", new { @class="default-button" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
			</div>
			<%: Html.HiddenFor(model => model.FundId)%>
			<%}%>
			<div class="menu exportlist" style="position: absolute; right: 85px;">
				<ul>
					<li><a href="javascript:jHelper.chooseExpMenu(2,'Pdf');">Pdf</a></li>
					<li><a href="javascript:jHelper.chooseExpMenu(1,'Word');">Word</a></li>
					<li><a href="javascript:jHelper.chooseExpMenu(4,'Excel');">Excel</a></li>
				</ul>
			</div>
			<div class="export">
				<div class="print">
					<%:Html.Image("print.gif", new { @style = "cursor:pointer", @onclick = "javascript:securityValueReport.print()" })%>
				</div>
				<div class="menu" onclick="javascript:jHelper.expandExpMenu(this);">
					<ul>
						<li><a id="lnkExportName" href="#">Pdf</a></li>
					</ul>
					<%: Html.Hidden("ExportId","")%>
				</div>
				<div class="darrow">
					<%:Html.Image("down_arrow.png", new { @style = "cursor:pointer", @onclick = "javascript:securityValueReport.exportData();" })%></div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="line">
	</div>
	<div id="ReportMain">
		<div id="ReportDetail" class="rep-main" style="display: none">
			<div id="ReportContent">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="SecurityValueList" class="grid">
					<thead>
						<tr>
							<th sortname="DealNo" style="width: 10%">
								Deal
							</th>
							<th sortname="Security">
								Security
							</th>
						<%--	<th sortname="SecurityType" style="width: 10%">
								Security Type
							</th>--%>
							<th sortname="NoOfShares" align="right" style="text-align: right;">
								#Shares
							</th>
							<th sortname="Price" align="right" style="text-align: right;">
								Price
							</th>
							<th sortname="Date" style="width: 10%">
								Price Date
							</th>
							<th sortname="Value" align="right" style="text-align: right">
								Total Value
							</th>
						</tr>
					</thead>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions { Source = "/Fund/FindFunds", MinLength = 1, OnSelect = "function(event, ui) { securityValueReport.selectFund(ui.item.id);}" })%>
	<%= Html.jQueryDatePicker("StartDate")%>
	<%= Html.jQueryDatePicker("EndDate")%>
	<%=Html.jQueryFlexiGrid("SecurityValueList", new FlexigridOptions {
	ActionName = "SecurityValueReport",
	ControllerName = "Report",
	HttpMethod = "POST",
	SortName = "", SortOrder = "desc", 
	Paging = true, Autoload = false,
	OnTemplate = "securityValueReport.onTemplate",
	OnSuccess = "securityValueReport.onGridSuccess",
	BoxStyle = false
})%>
	<script type="text/javascript">		securityValueReport.init();</script>
	<%using(Html.jQueryTemplateScript("GridTemplate")){%>
		{{each(i,row) rows}} 
		<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
			<td>
			${row.cell[0]}
			</td>
			<td>
			${row.cell[1]}
			</td>
			<%--<td>
			${row.cell[2]}
			</td>--%>
			<td style="text-align:right">
			${formatNumber(row.cell[3],0)}
			</td>
			<td style="text-align:right">
			${formatCurrency(row.cell[4])}
			</td>
			<td>
			${formatDate(row.cell[6])}
			</td>
			<td style="text-align:right">
			${row.cell[5]}
			</td>
		</tr>
		{{/each}}
	<%}%>
</asp:Content>

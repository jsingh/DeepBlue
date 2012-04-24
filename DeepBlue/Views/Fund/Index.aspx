<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Fund.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.JavascriptInclueTag("Fund.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
	<%= Html.StylesheetLinkTag("fund.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">FUNDS</span><span class="arrow"></span><span class="pname"> Fund
					Library</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
			<div class="leftcol">
				Fund
			</div>
			<div class="addbtn" style="display: block; margin-left: 123px;">
				<%using (Html.GreenButton(new { @id = "lnkAddFund", @onclick = "javascript:fund.edit(0,'');" })) {%>Add
				Fund<%}%>
			</div>
			<div class="addbtn" style="display: block; float: right;">
				<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...&nbsp;", new { @id = "SpnLoading", @style = "display:none;float:left;" })%><%: Html.TextBox("Fund", "SEARCH  FUND", new { @class = "wm", @style = "width:200px" })%>
			</div>
		</div>
	</div>
	<div class="schedule-box">
		<div class="header">
			<div id="TabMain" class="section-tab-main">
				<div class="section-tab-box">
					<%using (Html.Tab(new { @id = "TabFundGrid", @class = "section-tab-sel", @onclick = "javascript:fund.selectTab(this,'FundDetail');" })) {%>Fund
					Library
					<%}%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="schedule-main">
		<div class="section-det" id="FundDetail">
			<% Html.RenderPartial("TBoxTop"); %>
			<table id="FundList" cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th sortname="FundName">
							Fund Name
						</th>
						<th sortname="TaxId" style="width: 15%">
							Tax ID
						</th>
						<th sortname="InceptionDate" style="width: 20%">
							Fund Start Date
						</th>
						<th sortname="ScheduleTerminationDate" style="width: 20%">
							Schedule Termination Date
						</th>
						<th sortname="CommitmentAmount" style="width: 20%">
							Commitment Amount
						</th>
						<th sortname="UnfundedAmount" style="width: 20%">
							Unfunded Amount
						</th>
						<th align="right" style="width: 15%">
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		<div class="section-det" id="AddNewFund" style="display: none">
		</div>
	</div>
	<%: Html.HiddenFor(model => model.FundId, new { @id = "DefaultFundId" })%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds"
																, MinLength = 1
																,
																  OnSelect = "function(event, ui) { $('#DefaultFundId').val(ui.item.id); $('#FundList').flexReload(); }"
})%>
	<%=Html.jQueryFlexiGrid("FundList", new FlexigridOptions { ActionName = "List"
															   ,ControllerName = "Fund"
															   ,SortName = "FundName"
															   ,ResizeWidth = false
															   ,Paging = true
															   ,OnRowBound = "fund.onRowBound"
															   ,OnTemplate = "fund.onTemplate"
															   ,OnSuccess = "fund.onGridSuccess"
															   ,OnSubmit  = "fund.onSubmit"
															   ,BoxStyle = false
})%>
	<%using (Html.JavaScript()) {%>
		$(document).ready(function(){
			fund.newFundData = <%=JsonSerializer.ToJsonObject(Model)%>;
			fund.init();
		});
	<%}%>
	<%using (Html.jQueryTemplateScript("GridTemplate")) {%>
		{{each(i,row) rows}}
			<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>
					${row.cell[1]}
				</td>
				<td>
					${row.cell[2]}
				</td>
				<td>
					${formatDate(row.cell[3])}
				</td>
				<td>
					${formatDate(row.cell[4])}
				</td>
				<td>
					${formatCurrency(row.cell[5])}
				</td>
				<td>
					${formatCurrency(row.cell[6])}
				</td>
				<td style="text-align:right">
					{{if row.cell[0]>0}}
					<%: Html.Image("Edit.png", new { @id = "Edit${row.cell[0]}", @class = "gbutton editbtn show", @onclick = "javascript:fund.edit(${row.cell[0]},'${row.cell[1]}');" })%>
					{{/if}}
				</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("FundAddTemplate")) {%>
		<% Html.RenderPartial("FundDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("FundRateSchduleTemplate")) {%>
		<% Html.RenderPartial("FundRateSchduleDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("FundRateSchduleTierTemplate")) {%>
		<% Html.RenderPartial("FundRateSchduleTierDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("TabTemplate")) {%>
		<div style="float:left">
			<div id="Tab${id}" onmousemove="javascript:$('#tabdel${id}').show();"
			 onmouseout="javascript:$('#tabdel${id}').hide();"
			   class="section-tab section-tab-sel">
				<div class="left"></div>
				<div class="center" onclick="javascript:fund.selectTab($(this).parent(),'Edit${id}');">${FundName}</div>
				<div class="right"></div>
				<div class='tab-delete' style='display:none' id="tabdel${id}" onclick="javascript:fund.deleteTab(${id},true);"></div>
			</div>
		</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("SectionTemplate")) {%>
		<div class="section-det" id="Edit${id}" style="display: none">
		</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("InvestorGridTemplate")) {%>
		{{each(i,row) rows}}
		<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
			<td>${row.cell[0]}</td>
			<td style="text-align:right">${formatCurrency(row.cell[1])}</td>
			<td style="text-align:right">${formatCurrency(row.cell[2])}</td>
			<td>${formatDate(row.cell[3])}</td>
		</tr>
		{{/each}}
	<%}%>
</asp:Content>

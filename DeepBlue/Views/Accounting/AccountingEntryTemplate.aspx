<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Accounting.AccountingEntryTemplateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Virtual Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.JavascriptInclueTag("AccountingEntryTemplate.js")%>
	<%= Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
	<%= Html.StylesheetLinkTag("accountentrytemplate.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">FUNDS</span><span class="arrow"></span><span class="pname">Template
					Library</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
			<div class="leftcol">
				Template
			</div>
			<div class="addbtn" style="display: block; margin-left: 123px;">
				<%using (Html.GreenButton(new { @id = "lnkAddAccountingEntryTemplate", @onclick = "javascript:accountingEntryTemplate.edit(0,'');" })) {%>Add
				Template<%}%>
			</div>
			<div class="addbtn" style="display: block; float: right;">
				<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...&nbsp;", new { @id = "SpnLoading", @style = "display:none;float:left;" })%>
			</div>
		</div>
	</div>
	<div class="schedule-box">
		<div class="header">
			<div id="TabMain" class="section-tab-main">
				<div class="section-tab-box">
					<%using (Html.Tab(new { @id = "TabAccountingEntryTemplateGrid", @class = "section-tab-sel", @onclick = "javascript:accountingEntryTemplate.selectTab(this,'AccountingEntryTemplateDetail');" })) {%>Template
					Library
					<%}%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="schedule-main">
		<div class="section-det" id="AccountingEntryTemplateDetail">
			<% Html.RenderPartial("TBoxTop"); %>
			<table id="AccountingEntryTemplateList" cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr> 
						<th sortname="FundName">
							Fund Name
						</th>
						<th sortname="VirtualAccountName">
							Virtual Account Name
						</th>
						<th sortname="AccountingTransactionTypeName">
							Transaction Type
						</th>
						<th sortname="AccountingEntryAmountTypeName">
							Amount Type
						</th>
						<th sortname="IsCredit">
							Credit
						</th>
						<th align="right" style="width: 15%">
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		<div class="section-det" id="AddNewAccountingEntryTemplate" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("AccountingEntryTemplate", new AutoCompleteOptions { Source = "/AccountingEntryTemplate/FindAccountingEntryTemplates"
																, MinLength = 1
																,
																  OnSelect = "function(event, ui) { $('#DefaultAccountingEntryTemplateId').val(ui.item.id); $('#AccountingEntryTemplateList').flexReload(); }"
})%>
	<%=Html.jQueryFlexiGrid("AccountingEntryTemplateList", new FlexigridOptions {
	ActionName = "AccountingEntryTemplateList"
															   ,ControllerName = "Accounting"
															   ,SortName = "FundName"
															   ,ResizeWidth = false
															   ,Paging = true
															   ,OnRowBound = "accountingEntryTemplate.onRowBound"
															   ,OnTemplate = "accountingEntryTemplate.onTemplate"
															   ,OnSuccess = "accountingEntryTemplate.onGridSuccess"
															   ,BoxStyle = false
})%>
	<%using (Html.JavaScript()) {%>
		$(document).ready(function(){
			accountingEntryTemplate.newAccountingEntryTemplateData = <%=JsonSerializer.ToJsonObject(Model)%>;
			accountingEntryTemplate.init();
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
					${row.cell[3]}
				</td>
				<td>
					${row.cell[4]}
				</td>
				<td>
					<%: Html.Span("{{if row.cell[5]}}"+Html.Image("tick.png").ToHtmlString()+"{{/if}}", new { @class = "show" })%>		
				</td>
				<td style="text-align:right">
					{{if row.cell[0]>0}}
					<%: Html.Image("Edit.png", new { @id = "Edit${row.cell[0]}", @class = "gbutton editbtn show", @onclick = "javascript:accountingEntryTemplate.edit(${row.cell[0]},'${row.cell[1]}');" })%>
					<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:accountingEntryTemplate.deleteAccountingEntryTemplate(this,${row.cell[0]});" })%>
					{{/if}}
				</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("AccountingEntryTemplateAddTemplate")) {%>
		<% Html.RenderPartial("AccountingEntryTemplateDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("TabTemplate")) {%>
		<div style="float:left">
			<div id="Tab${id}" onmousemove="javascript:$('#tabdel${id}').show();"
			 onmouseout="javascript:$('#tabdel${id}').hide();"
			   class="section-tab section-tab-sel">
				<div class="left"></div>
				<div class="center" onclick="javascript:accountingEntryTemplate.selectTab($(this).parent(),'Edit${id}');">${AccountingEntryTemplateName}</div>
				<div class="right"></div>
				<div class='tab-delete' style='display:none' id="tabdel${id}" onclick="javascript:accountingEntryTemplate.deleteTab(${id},true);"></div>
			</div>
		</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("SectionTemplate")) {%>
		<div class="section-det" id="Edit${id}" style="display: none">
		</div>
	<%}%>
</asp:Content>

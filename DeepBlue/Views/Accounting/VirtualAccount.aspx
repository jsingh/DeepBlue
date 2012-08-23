<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Accounting.VirtualAccountModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Virtual Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.JavascriptInclueTag("VirtualAccount.js")%>
	<%= Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
	<%= Html.StylesheetLinkTag("VirtualAccount.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">FUNDS</span><span class="arrow"></span><span class="pname"> VirtualAccount
					Library</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
			<div class="leftcol">
				VirtualAccount
			</div>
			<div class="addbtn" style="display: block; margin-left: 123px;">
				<%using (Html.GreenButton(new { @id = "lnkAddVirtualAccount", @onclick = "javascript:virtualAccount.edit(0,'');" })) {%>Add
				VirtualAccount<%}%>
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
					<%using (Html.Tab(new { @id = "TabVirtualAccountGrid", @class = "section-tab-sel", @onclick = "javascript:virtualAccount.selectTab(this,'VirtualAccountDetail');" })) {%>VirtualAccount
					Library
					<%}%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="schedule-main">
		<div class="section-det" id="VirtualAccountDetail">
			<% Html.RenderPartial("TBoxTop"); %>
			<table id="VirtualAccountList" cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th sortname="AccountName">
							Account Name
						</th>
						<th sortname="FundName">
							Fund Name
						</th>
						<th sortname="ParentVirtualAccountName">
							Parent Account
						</th>
						<th sortname="LedgerBalance" style="width: 20%">
							Balance
						</th>
						<th align="right" style="width: 15%">
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		<div class="section-det" id="AddNewVirtualAccount" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("VirtualAccount", new AutoCompleteOptions { Source = "/VirtualAccount/FindVirtualAccounts"
																, MinLength = 1
																,
																  OnSelect = "function(event, ui) { $('#DefaultVirtualAccountId').val(ui.item.id); $('#VirtualAccountList').flexReload(); }"
})%>
	<%=Html.jQueryFlexiGrid("VirtualAccountList", new FlexigridOptions {
	ActionName = "VirtualAccountList"
															   ,ControllerName = "Accounting"
															   ,SortName = "AccountName"
															   ,ResizeWidth = false
															   ,Paging = true
															   ,OnRowBound = "virtualAccount.onRowBound"
															   ,OnTemplate = "virtualAccount.onTemplate"
															   ,OnSuccess = "virtualAccount.onGridSuccess"
															   ,BoxStyle = false
})%>
	<%using (Html.JavaScript()) {%>
		$(document).ready(function(){
			virtualAccount.newVirtualAccountData = <%=JsonSerializer.ToJsonObject(Model)%>;
			virtualAccount.init();
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
					${formatCurrency(row.cell[4])}
				</td>
				<td style="text-align:right">
					{{if row.cell[0]>0}}
					<%: Html.Image("Edit.png", new { @id = "Edit${row.cell[0]}", @class = "gbutton editbtn show", @onclick = "javascript:virtualAccount.edit(${row.cell[0]},'${row.cell[1]}');" })%>
					<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:virtualAccount.deleteVirtualAccount(this,${row.cell[0]});" })%>
					{{/if}}
				</td>
			</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("VirtualAccountAddTemplate")) {%>
		<% Html.RenderPartial("VirtualAccountDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("TabTemplate")) {%>
		<div style="float:left">
			<div id="Tab${id}" onmousemove="javascript:$('#tabdel${id}').show();"
			 onmouseout="javascript:$('#tabdel${id}').hide();"
			   class="section-tab section-tab-sel">
				<div class="left"></div>
				<div class="center" onclick="javascript:virtualAccount.selectTab($(this).parent(),'Edit${id}');">${VirtualAccountName}</div>
				<div class="right"></div>
				<div class='tab-delete' style='display:none' id="tabdel${id}" onclick="javascript:virtualAccount.deleteTab(${id},true);"></div>
			</div>
		</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("SectionTemplate")) {%>
		<div class="section-det" id="Edit${id}" style="display: none">
		</div>
	<%}%>
</asp:Content>

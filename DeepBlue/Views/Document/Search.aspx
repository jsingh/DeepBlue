<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Document.SearchModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Document Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("DocumentSearch.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("document.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<div class="title">
					DOCUMENT MANAGEMENT</div>
				<div class="arrow">
				</div>
				<div class="pname">
					DOCUMENT SEARCH</div>
			</div>
		</div>
		<div class="doc-main">
			<% using (Html.BeginForm("", "", FormMethod.Get, new { @id = "SearchDocument", @onsubmit = "return false;" })) {%>
			<%: Html.HiddenFor(model => model.InvestorId)%>
			<%: Html.HiddenFor(model => model.FundId)%>
			<div class="doc-header">
				<div class="editor-label">
					<%: Html.LabelFor(model => model.FromDate) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("FromDate","", new { @id = "FromDate" }) %>
				</div>
				<div class="editor-label" style="clear: right; width: auto;">
					<%: Html.LabelFor(model => model.ToDate) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBox("ToDate", "", new { @id = "ToDate" })%>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.DocumentTypeId) %>
				</div>
				<div class="editor-field">
					<%: Html.DropDownListFor(model => model.DocumentTypeId, Model.DocumentTypes, new { @style = "width:190px" })%>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.DocumentStatus)%>
				</div>
				<div class="editor-field">
					<div id="InvestorRow" style="float: left;">
						<%: Html.TextBoxFor(model => model.InvestorName, new { @style = "width:164px", @onblur = "javascript:documentSearch.InvestorBlur(this);" })%>
						<%: Html.ValidationMessageFor(model => model.InvestorId) %>
					</div>
					<div id="FundRow" style="display: none; float: left;">
						<%: Html.TextBoxFor(model => model.FundName, new { @style = "width:164px", @onblur = "javascript:documentSearch.FundBlur(this);" })%>
						<%: Html.ValidationMessageFor(model => model.FundId) %>
					</div>
					<div style="float: left; margin-left: 2px;">
						<%: Html.DropDownListFor(model => model.DocumentStatus,Model.DocumentStatusTypes, new { @style="width:80px;", @onchange = "javascript:documentSearch.changeType(this);" })%>
					</div>
				</div>
				<div class="editor-button">
					<div style="float: left; padding: 0 0 10px 5px;">
						<%: Html.ImageButton("Search.png", new { @class="default-button",@onclick="return documentSearch.onSubmit('SearchDocument');" })%>
					</div>
				</div>
			</div>
			<% } %>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="doc-content">
		<table cellpadding="0" cellspacing="0" border="0" id="SearchDocumentList" style="width: 100%">
			<thead>
				<tr>
					<th sortname="DocumentDate" style="width: 10%;">
						Date
					</th>
					<th sortname="FileName" style="width: 30%;">
						File Name
					</th>
					<th sortname="DocumentType" style="width: 20%;">
						Document Type
					</th>
					<th sortname="InvestorName" style="width: 30%" id="InvestorNameColumn">
						Investor Name
					</th>
					<th sortname="FundName" style="display: none; width: 25%;" id="FundNameCloumn">
						Fund Name
					</th>
					<th sortname="FileTypeName" align="right" style="width: 10%;">
					</th>
				</tr>
			</thead>
		</table>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("FromDate")%>
	<%= Html.jQueryDatePicker("ToDate")%>
	<%= Html.jQueryAutoComplete("InvestorName", new AutoCompleteOptions {
																	  Source = "/Investor/FindInvestors", MinLength = 1,
																	  OnSelect = "function(event, ui) { documentSearch.selectInvestor(ui.item.id);}"})%>
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { documentSearch.selectFund(ui.item.id);}"})%>
	<%=Html.jQueryFlexiGrid("SearchDocumentList", new FlexigridOptions {
	ActionName = "List",
	ControllerName = "Document",
	HttpMethod = "GET",
	SortName = "DocumentDate",
	SortOrder = "desc",
	Paging = true,
	Autoload = false,
	Height = 300,
	OnInit = "documentSearch.onInit",
	OnSuccess = "documentSearch.onGridSuccess",
	BoxStyle=false
	})%>
	<script type="text/javascript">
		documentSearch.init();
	</script>
</asp:Content>

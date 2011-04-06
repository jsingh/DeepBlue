<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Document.SearchModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("document.css")%>
	<%=Html.JavascriptInclueTag("DocumentSearch.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="doc-search">
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
				<%: Html.DropDownListFor(model => model.DocumentTypeId, Model.DocumentTypes, new { @style = "width:242px" })%>
			</div>
			<div class="editor-label">
				<%: Html.DropDownListFor(model => model.DocumentStatus,Model.DocumentStatusTypes, new { @onchange = "javascript:documentSearch.changeType(this);" })%>
			</div>
			<div class="editor-field">
				<div id="InvestorRow">
					<%: Html.TextBoxFor(model => model.InvestorName, new { @style = "width:238px", @onblur = "javascript:documentSearch.InvestorBlur(this);" })%>
					<%: Html.ValidationMessageFor(model => model.InvestorId) %>
				</div>
				<div id="FundRow" style="display: none">
					<%: Html.TextBoxFor(model => model.FundName, new { @style = "width:238px", @onblur = "javascript:documentSearch.FundBlur(this);" })%>
					<%: Html.ValidationMessageFor(model => model.FundId) %>
				</div>
			</div>
			<div class="editor-button">
				<div style="float: left; padding: 0 0 10px 5px;">
					<%: Html.ImageButton("Search.png", new { @style = "width: 73px; height: 23px;",@onclick="return documentSearch.onSubmit('SearchDocument');" })%>
				</div>
			</div>
		</div>
		<div class="doc-result">
			<table cellpadding="0" cellspacing="0" border="0" id="SearchDocumentList" style="width: 100%">
				<thead>
					<tr>
						<th sortname="DocumentDate" style="width: 10%;" align="center">
							Date
						</th>
						<th sortname="FileName" style="width: 20%;">
							File Name
						</th>
						<th sortname="DocumentType" style="width: 35%;">
							Document Type
						</th>
						<th sortname="InvestorName" style="width: 25%" id="InvestorNameColumn">
							Investor Name
						</th>
						<th sortname="FundName" style="display: none; width: 25%;" id="FundNameCloumn">
							Fund Name
						</th>
						<th sortname="FileTypeName" align="center" style="width: 10%;">
						</th>
					</tr>
				</thead>
			</table>
		</div>
		<% } %>
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
	<%=Html.jQueryFlexiGrid("SearchDocumentList", new FlexigridOptions { ActionName = "List", ControllerName = "Document", HttpMethod = "GET", SortName = "DocumentDate", SortOrder = "desc", Paging = true, Autoload = false, Height = 300 })%>
	<script type="text/javascript">
		documentSearch.init();
	</script>
</asp:Content>

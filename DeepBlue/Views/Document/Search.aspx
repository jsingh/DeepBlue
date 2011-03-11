<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Document.SearchModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("document.css")%>
	<%=Html.JavascriptInclueTag("DocumentSearch.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="document-search">
		<% using (Html.BeginForm()) {%>
		<%: Html.ValidationSummary(true) %>
		<div class="editor-label" style="width: auto">
			<%: Html.LabelFor(model => model.FromDate) %>
			<%: Html.TextBox("FromDate","", new { @id = "FromDate" }) %>
			<%: Html.LabelFor(model => model.ToDate) %>
			<%: Html.TextBox("ToDate", "", new { @id = "ToDate" })%>
			<%: Html.ValidationMessageFor(model => model.ToDate) %>
		</div>
		<div class="editor-label">
			<%: Html.DropDownListFor(model => model.DocumentStatus,Model.DocumentStatusTypes, new { @onchange = "javascript:documentSearch.changeType(this);" })%>
		</div>
		<div class="editor-field">
			<div id="InvestorRow">
				<%: Html.TextBoxFor(model => model.InvestorName, new { @onblur="javascript:documentSearch.InvestorBlur(this);" }) %>
				<%: Html.ValidationMessageFor(model => model.InvestorId) %>
			</div>
			<div id="FundRow" style="display: none">
				<%: Html.TextBoxFor(model => model.FundName, new { @onblur = "javascript:documentSearch.FundBlur(this);" })%>
				<%: Html.ValidationMessageFor(model => model.FundId) %>
			</div>
		</div>
		<div class="editor-button" style="width: 165px">
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.ImageButton("Search.png", new { style = "width: 73px; height: 23px;", onclick = "return documentSearch.onSubmit('AddNewDocument');" })%>
			</div>
		</div>
		<% } %>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
</asp:Content>

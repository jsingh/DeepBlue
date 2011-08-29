<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="editor-label">
	<%: Html.Label("Investor Name")%>
</div>
<div class="editor-field">
	<%: Html.Span("${InvestorName}", new { id = "InvestorName" })%>
</div>
<div class="editor-label">
	<%: Html.Label("Display Name")%>
</div>
<div class="editor-field">
	<%: Html.Span("${DisplayName}", new { id = "Spn_DisplayName" })%>
</div>
<div class="editor-label">
	<%: Html.Label("Notes")%>
</div>
<div class="editor-field" style="width:70%">
	<%: Html.Span("${formatEditor(Notes)}", new { @class="notes", id = "Spn_Notes" })%>
</div>
<div id="funddetails" class="fund-details">
</div>
<div style="clear: both; height: 10px">
	&nbsp;</div>
<div class="editor-button" style="width: 210px;">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Image("Delete.png", new { @id = "Delete", @style = "cursor:pointer", @onclick = "javascript:editInvestor.deleteInvestor(this,${InvestorId});" })%>
		<%: Html.Span("", new { @id = "DeleteLoading" })%>
	</div>
</div>

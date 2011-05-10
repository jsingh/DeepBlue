<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateDealCloseModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Close
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="search-header">
		<div class="editor-label" style="width: auto">
			<%: Html.LabelFor(model => model.DealId) %>&nbsp;<%: Html.TextBox("Deal","", new { @style = "width:200px" })%>&nbsp;<%: Html.Span( Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Deal", new AutoCompleteOptions { Source = "/Deal/FindDeals", MinLength = 1, OnSelect = "function(event, ui) { dealClose.selectDeal(ui.item.id);}" })%>
	<script type="text/javascript">
		dealClose.init();
	</script>
</asp:Content>

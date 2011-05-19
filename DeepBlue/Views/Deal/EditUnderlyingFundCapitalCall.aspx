<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.UnderlyingFundCapitalCallModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Underlying Fund Capital Call
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%=Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%=Html.JavascriptInclueTag("DealActivity.js") %>
	<%=Html.StylesheetLinkTag("dealactivity.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("CreateUnderlyingFundCapitalCall", Model); %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.selectFund(ui.item.id);}"})%>
	<%= Html.jQueryAutoComplete("UnderlyingFundName", new AutoCompleteOptions {
																	  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { dealActivity.selectUnderlyingFund(ui.item.id);}"})%>
	<%=Html.jQueryDatePicker("NoticeDate")%>
	<%=Html.jQueryDatePicker("ReceivedDate")%>
	<script type="text/javascript">		dealActivity.init();</script>
</asp:Content>

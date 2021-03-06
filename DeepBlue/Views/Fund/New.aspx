﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Fund.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("jquery.validate.min.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftAjax.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcAjax.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcValidation.js")%>
	<%= Html.JavascriptInclueTag("MicrosoftMvcCustomValidation.js")%>
	<%= Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%= Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%= Html.JavascriptInclueTag("Fund.js")%>
	<%= Html.StylesheetLinkTag("fund.css")%>
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("Create", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "fund.onCreateFundBegin", OnSuccess = "fund.onCreateFundSuccess" }, new { @id = "AddNewFund" })) {%>
	<% Html.RenderPartial("FundDetail", Model); %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("InceptionDate")%>
	<%= Html.jQueryDatePicker("ScheduleTerminationDate")%>
	<%= Html.jQueryDatePicker("FinalTerminationDate")%>
	<%= Html.jQueryDatePicker("DateClawbackTriggered")%>
	<%= Html.jQueryDatePicker("MgmtFeesCatchUpDate")%>
</asp:Content>

﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%switch (Convert.ToString(ViewData["SubmenuName"])) {%>
<%case "InvestorManagement":%>
<%using (Html.LeftMenu()) {%>
<div class="menubox">
	<ul>
		<li class="<%=(ViewData["PageName"] == "InvestorEntityType" ? "sel" : "")%>">
			<%: Html.ActionLink("Investor Entity Type", "EntityType", "Admin",null, new {} )%></li>
		<li class="<%=(ViewData["PageName"] == "InvestorType" ? "sel" : "")%>">
			<%: Html.ActionLink("Investor Type", "InvestorType", "Admin",null, new { @class = (ViewData["PageName"] == "InvestorType" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "CommunicationType" ? "sel" : "")%>">
			<%: Html.ActionLink("Communication Type", "CommunicationType", "Admin",null, new { @class = (ViewData["PageName"] == "CommunicationType" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "CommunicationGrouping" ? "sel" : "")%>">
			<%: Html.ActionLink("Communication Grouping", "CommunicationGrouping", "Admin",null, new { @class = (ViewData["PageName"] == "CommunicationGrouping" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "FundClosing" ? "sel" : "")%>">
			<%: Html.ActionLink("Fund Closing", "FundClosing", "Admin", null, new { @class = (ViewData["PageName"] == "FundClosing" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "Source" ? "sel" : "")%>">
			<%: Html.Anchor("Source")%></li>
		<li class="<%=(ViewData["PageName"] == "InvestorAccountingCategories" ? "sel" : "")%>">
			<%: Html.Anchor("Investor Accounting Categories")%></li>
	</ul>
</div>
<%} %>
<%break;%>
<%case "CustomFieldManagement":%>
<%using (Html.LeftMenu()) {%>
<div class="menubox">
	<ul>
		<li class="<%=(ViewData["PageName"] == "CustomField" ? "sel" : "")%>">
			<%: Html.ActionLink("Custom Field", "CustomField", "Admin",null, new { @class = (ViewData["PageName"] == "CustomField" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "DataType" ? "sel" : "")%>">
			<%: Html.ActionLink("Data Type", "DataType", "Admin",null, new { @class = (ViewData["PageName"] == "DataType" ? "sel" : "") })%></li>
	</ul>
</div>
<%} %>
<%break;%>
<%case "DealManagement":%>
<%using (Html.LeftMenu()) {%>
<div class="menubox">
	<ul>
		<li class="<%=(ViewData["PageName"] == "PurchaseType" ? "sel" : "")%>">
			<%: Html.ActionLink("Purchase Type", "PurchaseType", "Admin", null, new { @class = (ViewData["PageName"] == "PurchaseType" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "DealClosingCostType" ? "sel" : "")%>">
			<%: Html.ActionLink("Deal Closing Cost Type", "DealClosingCostType", "Admin", null, new { @class = (ViewData["PageName"] == "DealClosingCostType" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "UnderlyingFundType" ? "sel" : "")%>">
			<%: Html.ActionLink("Underlying Fund Type", "UnderlyingFundType", "Admin", null, new { @class = (ViewData["PageName"] == "UnderlyingFundType" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "ShareClassType" ? "sel" : "")%>">
			<%: Html.ActionLink("Share Class Type", "ShareClassType", "Admin", null, new { @class = (ViewData["PageName"] == "ShareClassType" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "CashDistributionType" ? "sel" : "")%>">
			<%: Html.ActionLink("Cash Distribution Type", "CashDistributionType", "Admin", null, new { @class = (ViewData["PageName"] == "CashDistributionType" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "FundExpenseType" ? "sel" : "")%>">
			<%: Html.ActionLink("Fund Expense Type", "FundExpenseType", "Admin", null, new { @class = (ViewData["PageName"] == "FundExpenseType" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "ReportingFrequency" ? "sel" : "")%>">
			<%: Html.ActionLink("Reporting", "ReportingFrequency", "Admin", null, new { @class = (ViewData["PageName"] == "ReportingFrequency" ? "sel" : "") })%></li>
		<li class="<%=(ViewData["PageName"] == "ReportingType" ? "sel" : "")%>">
			<%: Html.ActionLink("Reporting Type", "ReportingType", "Admin", null, new { @class = (ViewData["PageName"] == "ReportingType" ? "sel" : "") })%></li>
	</ul>
</div>
<%} %>
<%break;%>
<%}%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.ListModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FlexGrid.js")%><%=Html.StylesheetLinkTag("flexigrid.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="admin_accordion">
		<h3>
			<a href="#">Entity Type </a>
		</h3>
		<div>
			<table cellpadding="0" cellspacing="0" border="0" id="EntityTypes">
			</table>
		</div>
		<h3>
			<a href="#">Source</a>
		</h3>
		<div>
		</div>
		<h3>
			<a href="#">Investor Type</a>
		</h3>
		<div>
		</div>
		<h3>
			<a href="#">Fund Close</a>
		</h3>
		<div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryAccordion("admin_accordion", new AccordionOptions { Active = 0 })%>
	<%--	<%=Html.jQueryFlexiGrid("EntityTypes", new FlexigridOptions { ActionName = "Admin", ControllerName = "EntityType", HttpMethod="GET" })%>--%>
</asp:Content>

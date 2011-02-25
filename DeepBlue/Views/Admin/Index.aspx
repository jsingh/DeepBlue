<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.ListModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="tabs" style="border:0px">
		<ul>
			<li><a href="#tabs-1">Entity Type </a></li>
			<li><a href="#tabs-2">Subject to FOIA</a></li>
			<li><a href="#tabs-3">Subject to ERISA</a></li>
			<li><a href="#tabs-3">Restricted Person</a></li>
			<li><a href="#tabs-3">Source</a></li>
			<li><a href="#tabs-3">Investor Type</a></li>
			<li><a href="#tabs-3">Fund Close</a></li>
		</ul>
		<div id="tabs-1">
			<% Html.RenderPartial("EntityType", Model.EntityTypes); %>
		</div>
		<div id="tabs-2">
		</div>
		<div id="tabs-3">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryTab("tabs",new jQueryTabOptions{ }) %>
</asp:Content>

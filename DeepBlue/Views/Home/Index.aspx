<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="TitleCnt" ContentPlaceHolderID="TitleContent" runat="server">
	<%=EntityHelper.EntityName%>
</asp:Content>
<asp:Content ID="MainCnt" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		deepBlue.indexPage=true;</script>
</asp:Content>

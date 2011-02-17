<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content><asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

	<script src="../../Assets/javascripts/Home.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div style="height: 100px">
		&nbsp;
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		Home.init();
	</script>

</asp:Content>

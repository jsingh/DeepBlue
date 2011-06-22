<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateIssuerModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Directs
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealdirect.css")%>
	<%=Html.JavascriptInclueTag("DealDirect.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<div class="title">
					INVESTMENTS</div>
				<div class="arrow">
				</div>
				<div class="pname">
					ADD DIRECTS</div>
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="leftcol">
			Underlying Direct</div>
		<div class="leftcol expandaddbtn" style="display: block">
			<%: Html.Anchor("Add new issuer", "javascript:dealDirect.add(0);")%>
		</div>
	</div>
	<div>
		<%using (Html.Form(new { @onsubmit = "javascript:dealDirect.save(this);" })) {%>
		<div class="editor-field">
			<%: Html.HiddenFor(model => model.IssuerId)%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.Name)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Name", "${Name}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ParentName)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("ParentName", "${ParentName}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.CountryId)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Country", "${Country}")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Security Type")%>
		</div>
		<div class="editor-field" style="width: auto;">
			<div class="smalltab tabsel">
				Security Type
			</div>
			<div class="smalltab">
				Fixed Income
			</div>
			<div class="smalltab last">
			</div>
		</div>
		<div id="EquityDetail" class="subdetail">
			<%Html.RenderPartial("DirectEquityDetail", Model.EquityDetailModel);%>
		</div>
		<div id="FixedIncome" class="subdetail">
		</div>
		<%: Html.HiddenFor(model => model.CountryId)%>
		<%}%>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%--<script id="DirectTemplate" type="text/x-jquery-tmpl">
	</script>--%>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateIssuerModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Directs
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealdirect.css")%>
	<%=Html.JavascriptInclueTag("DealDirect.js")%>
    <%=Html.JavascriptInclueTag("DealActivity.js")%>
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
        <div class="rightcol">
				<%: Html.TextBox("M_Fund","Search Issuer", new { @class="wm", @style="width:150px", @id="M_Fund" })%>
			</div>
	</div>
    <div class="subheader">
   
		<div class="editor-label">
			Issuer Name:
		</div>
         <div class="editor-field">
			<%: Html.TextBox("Name", "Enter Name")%>
		</div>
        <div class="editor-label" style="clear: right">
			Parent Name
		</div>
         <div class="editor-field">
			<%: Html.TextBox("Name", "Enter Name")%>
		</div>
        <div class="editor-label" style="clear: right">
			Country
		</div>
         <div class="editor-field">
			<%: Html.TextBox("Name", "Enter Name")%>
		</div>
        <div class="issuerbtn" style="display: block">
			<%: Html.Anchor("Add issuer", "javascript:void(0);")%>
         
		</div>
        <div class="close">
          <%: Html.Image("close_icon.png")%>  Close
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
			<%: Html.TextBox("Name", "Enter Name")%>
		</div>
		
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.CountryId)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Country", "Enter Country")%>
		</div>
        <div class="editor-label" style="clear: right">
			Issuer rating
		</div>
		<div class="editor-field">
			<%: Html.TextBox("rating")%>
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
        <br />
        <div class="tab"  >
             <div class="tabselected">
				<%: Html.Anchor("Equity", new { @class = "select tablnk", @onclick = "javascript:dealActivity.selectDirTab('E',this);" })%>
              </div>
              <div class="tabUnselected">
                <%: Html.Anchor("FixedIncome", new { @class = "tablnk", @onclick = "javascript:dealActivity.selectDirTab('F',this);" })%>
           </div>
        </div>
       	    

		<div id="EquityDetail" class="subdetail">
        <div id="EQdetail">
			<%Html.RenderPartial("DirectEquityDetail", Model.EquityDetailModel);%>
		</div>
		<div id="FixedIncome" class="subdetail" style="display: none">
        <%Html.RenderPartial("FixedIncomeDetail", Model.FixedIncomeDetailModel);%>
		</div>
		<%: Html.HiddenFor(model => model.CountryId)%>
		<%}%>
        </div>
        <div class="direct">
      <%: Html.Image("add_direct.png")%>
    </div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%--<script id="DirectTemplate" type="text/x-jquery-tmpl">
	</script>--%>
</asp:Content>

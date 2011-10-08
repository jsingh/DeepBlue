<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateUnderlyingFundModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Underlying Fund Library
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.StylesheetLinkTag("deal.css") %>
	<%=Html.StylesheetLinkTag("dealdirect.css")%>
	<%=Html.StylesheetLinkTag("addufund.css")%>
	<%=Html.JavascriptInclueTag("UnderlyingFund.js")%>
	<%=Html.JavascriptInclueTag("UnderlyingFundContact.js")%>
	<%=Html.JavascriptInclueTag("DealDirect.js")%>
	<%=Html.JavascriptInclueTag("jquery.fileuploader.js")%>
	<%=Html.JavascriptInclueTag("flexgrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<div class="title">
					INVESTMENTS</div>
				<div class="arrow">
				</div>
				<div class="pname">
					UNDERLYING FUND LIBRARY
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%></div>
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
			<div class="leftcol">
				Underlying Fund
			</div>
			<div class="addbtn" style="display: block; margin-left: 67px;">
				<%using (Html.GreenButton(new { @id = "AddGP", @onclick = "javascript:dealDirect.add();" })) {%>Add
				GP<%}%>
			</div>
			<div class="addbtn" style="display: block; margin-left: 123px;">
				<%using (Html.GreenButton(new { @id = "lnkAddUnderlyingFund", @onclick = "javascript:underlyingFund.load(0,0,'Add New Underlying Fund');" })) {%>Add
				new underlying fund<%}%>
			</div>
			<div class="rightcol">
				<%: Html.TextBox("S_GP", "SEARCH GP", new { @id = "S_GP", @style = "width:200px", @class = "wm" })%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="AddNewIssuer" style="display: none">
		<%using (Html.Form(new { @id = "frmAddNewIssuer", @onsubmit = "return dealDirect.createNewIssuer(this);" })) {%>
		<div id="NewIssuerDetail">
		</div>
		<div class="addissuer" style="width: 985px;">
			<div class="btnclose">
				<%: Html.Image("issuerclose.png", new { @onclick = "javascript:dealDirect.close();" })%>
			</div>
			<div class="btn">
				<%: Html.ImageButton("savegp.png")%></div>
			<div class="btn">
				<%: Html.Span("", new { @id = "SpnNewLoading" })%></div>
		</div>
		<%}%>
	</div>
	<div id="TabHeader">
		<div class="header">
			<div id="TabMain" class="section-tab-main">
				<div class="section-tab-box">
					<%using (Html.Tab(new { @id = "TabFundGrid", @class = "section-tab-sel", @onclick = "javascript:underlyingFund.selectTab(this,'UnderlyingFundListBox');" })) {%>Underlying
					Fund setup
					<%}%>
				</div>
			</div>
		</div>
	</div>
	<div id="UnderlyingFundListBox" class="section-det">
		<% Html.RenderPartial("TBoxTop"); %>
		<table id="UnderlyingFundList" cellpadding="0" cellspacing="0" border="0" class="grid">
			<thead>
				<tr>
					<th colspan="4">
						Underlying Funds
					</th>
				</tr>
			</thead>
		</table>
		<% Html.RenderPartial("TBoxBottom"); %>
	</div>
	<div id="UnderlyingFundDetail">
	</div>
	<div id="AnnualMeetingDateList" style="display: none">
		<table cellpadding="0" cellspacing="0" id="MeetingDateList" class="grid">
			<thead>
				<tr>
					<th sortname="AnnualMeetingDate">
						Date
					</th>
				</tr>
			</thead>
		</table>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("S_GP", new AutoCompleteOptions {
																		  Source = "/Deal/FindGPs", MinLength = 1,
																		  OnSelect = "function(event, ui) {  underlyingFund.searchGP(ui.item.id); }"
	})%>
	<%=Html.jQueryFlexiGrid("UnderlyingFundList", new FlexigridOptions {
	ActionName = "UnderlyingFundList",
	ControllerName = "Deal",
	HttpMethod = "GET",
	SortName = "FundName",
	Paging = true 
	, OnSuccess= "underlyingFund.onGridSuccess"
	, OnRowClick = "underlyingFund.onRowClick"
	, OnInit = "underlyingFund.onInit"
	, OnTemplate = "underlyingFund.onTemplate"
	, BoxStyle = false 
	, RowsLength = 50
	})%>
	<%=Html.jQueryFlexiGrid("MeetingDateList", new FlexigridOptions {
	ActionName = "AnnualMeetingDateList",
	ControllerName = "Deal",
	HttpMethod = "GET", Autoload=false,
	SortName = "AnnualMeetingDate",
	Paging = true,
	})%>
	<script type="text/javascript">
		$(document).ready(function(){
			underlyingFund.newFundData = <%=JsonSerializer.ToJsonObject(Model)%>;
			underlyingFund.init();
		});
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
		{{each(i,row) rows}}
			{{if i%4==0}}
				<tr>
			{{/if}}
				<td><a href="javascript:underlyingFund.load(${row.cell[0]},0,'${row.cell[1]}')">${row.cell[1]}</a></td>
			{{if i%4==3}}
				</tr>
			{{/if}}
		{{/each}}
	</script>
	<script id="IssuerDetailTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("IssuerDetail", new DeepBlue.Models.Deal.IssuerDetailModel());%>
	</script>
	<script id="UnderlyingFundTemplate" type="text/x-jquery-tmpl">
		<%Html.RenderPartial("UnderlyingFundDetail", Model);%>
	</script>
	<script id="TabTemplate" type="text/x-jquery-tmpl">
		<div style="float:left">
			<div id="Tab${id}" onmousemove="javascript:$('#tabdel${id}').show();"
			 onmouseout="javascript:$('#tabdel${id}').hide();"
			   class="section-tab section-tab-sel">
				<div class="left"></div>
				<div class="center" onclick="javascript:underlyingFund.selectTab($(this).parent(),'Edit${id}');">${FundName}</div>
				<div class="right"></div>
				<div class='tab-delete' style='display:none' id="tabdel${id}" onclick="javascript:underlyingFund.deleteTab(${id},true);"></div>
			</div>
		</div>
	</script>
	<script id="SectionTemplate" type="text/x-jquery-tmpl">
		<div class="section-det" id="Edit${id}" style="display: none">
			<%using (Html.Form(new { @id = "frmUnderlyingFund", @onsubmit = "return underlyingFund.save(this);" })) {%>
				<div id="AddUnderlyingFund" style="display: none">
				</div>
			<%}%>
		</div>
	</script>
	<script id="AddressTemplate" type="text/x-jquery-tmpl">
		<%Html.RenderPartial("UnderlyingFundAddressInformationEdit");%>
	</script>
</asp:Content>

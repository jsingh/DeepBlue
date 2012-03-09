<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.JavascriptInclueTag("DealList.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
	<%= Html.StylesheetLinkTag("deal.css")%>
	<%= Html.StylesheetLinkTag("deallist.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTMENTS</span><span class="arrow"></span><span class="pname">
					<%if(Convert.ToString(ViewData["CloseDeal"])=="True"){%>
					CLOSE DEAL
					<%}else{%>
					MODIFY DEAL
					<%}%>
					</span></div>
			<div class="rightcol">
				<div style="float:left">
				<%: Html.TextBox("M_Fund", "SEARCH FUND", new { @class = "wm", @style="width:300px", @id = "M_Fund" })%>
				</div>
				<div style="float:left; margin-left:20px;">
				<%: Html.TextBox("M_Deal", "SEARCH DEAL", new { @class = "wm", @style = "width:300px", @id = "M_Deal" })%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="deal-main">
		<div class="section-det" id="DealDetail">
			<% Html.RenderPartial("TBoxTop"); %>
			<table id="DealList" cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th sortname="FundName">
							Fund Name
						</th>
						<th></th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		<div class="section-det" id="AddNewDeal" style="display: none">
		</div>
	</div>
	<%:Html.Hidden("FundID", "0")%>
	<%:Html.Hidden("DealID", "0")%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("DealList", new FlexigridOptions {
	ActionName = "DealCloseList"
															   ,ControllerName = "Deal"
															   ,SortName = "FundName"
															   ,ResizeWidth = false
															   ,Paging = true
															   ,OnSubmit = "dealList.onSubmit"
															   ,OnTemplate = "dealList.onTemplate"
															   ,OnSuccess = "dealList.onGridSuccess"
															   ,BoxStyle = false
															   ,RowsLength = 50
})%>
<%= Html.jQueryAutoComplete("M_Fund", new AutoCompleteOptions {
																 Source = "/Fund/FindFunds", MinLength = 1,
																 OnSelect = "function(event, ui) { $('#FundID').val(ui.item.id); $('#DealID').val(0); $('#M_Deal').val('SEARCH DEAL');  dealList.search(); }"
	})%>
	<%= Html.jQueryAutoComplete("M_Deal", new AutoCompleteOptions {
																	SearchFunction = "dealList.searchDeal",
																	MinLength = 1,
																	  OnSelect = "function(event, ui) { $('#DealID').val(ui.item.id); dealList.search();  }"
	})%>
	<%using(Html.jQueryTemplateScript("GridTemplate")){ %>
		{{each(i,row) rows}}
		<tr {{if i%2==0}}class="row"{{else}}class="erow"{{/if}} style="cursor:pointer" onclick="javascript:dealList.expandFund(${row.FundID});">
		<td>${row.FundName}</td><td style="text-align:right"><%: Html.Image("downarrow.png", new { @id = "ExpandImg_${row.FundID}" })%></td>
		</tr>
			 
				<tr id="ExpandRow_${row.FundID}" style="background-color:#E9E9E9;background:none;display:none;">
					<td colspan=2 style="padding:0;">
						<div class="heading">DEALS</div>   
						<div class="line"></div>
						<div style="width:700px;margin:0 0 0 20px;padding:10px 0">
						<% Html.RenderPartial("TBoxTop"); %>
							<table cellpadding=0 cellspacing=0 border=0 class="grid deal-list" id="FundDealList" fundid="${row.FundID}">
								<thead>
									<tr>
										<th sortname="DealNumber" style="width:10%">Deal Number</th>
										<th sortname="DealName">Deal Name</th>
										<th style="width:15%"></th>
									</tr>
								</thead>
							</table>
						<% Html.RenderPartial("TBoxBottom"); %>
						</div>
					</td>
				</tr>
			 
		{{/each}}
	<%}%>
	<%using(Html.jQueryTemplateScript("DealGridTemplate")){ %>
	{{each(j,deal) rows}}
	<tr onclick="<%if(Convert.ToString(ViewData["CloseDeal"])=="True"){%>
	location.href='/Deal/Close/${deal.cell[0]}?menuid=<%=Request["menuid"]%>'
	<%}else{%>location.href='/Deal/Edit/${deal.cell[0]}?menuid=<%=Request["menuid"]%>'<%}%>" {{if j%2==0}}class="row"{{else}}class="erow"{{/if}}>
		<td>${deal.cell[2]}</td>
		<td>${deal.cell[1]}</td>
		<td style="text-align:right">
			<%if(Convert.ToString(ViewData["CloseDeal"])=="True"){%>
				<%: Html.Anchor(Html.Image("CloseDeal_active.png", new { @class = "gbutton editbtn" }).ToHtmlString(), "/Deal/Close/${deal.cell[0]}")%>
			<%}else{%>
				<%: Html.Anchor(Html.Image("Editbtn_active.png", new { @class = "gbutton editbtn" }).ToHtmlString(), "/Deal/Edit/${deal.cell[0]}")%>
			<%}%>
		</td>
	</tr>
	{{/each}}
	<%}%>
</asp:Content>

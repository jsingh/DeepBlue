<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Investor.InvestorLibraryModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="TitleCnt" ContentPlaceHolderID="TitleContent" runat="server">
	Library
</asp:Content>
<asp:Content ID="HeaderCnt" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("InvestorLibrary.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("InvestorLibrary.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTORS</span><span class="arrow"></span><span class="pname">Investor
					Library</span></div>
			<div class="rightcol">
				<%using (Html.Form(new { @onsubmit = "return investorLibrary.search();" })) {%>
				<div style="margin: 0; padding: 5px 0 0 5px; float: left;">
					<%: Html.Span("", new { @id = "SpnLoading" })%>
				</div>
				<div style="float: left">
					<%: Html.TextBox("Investor", "SEARCH INVESTOR", new { @id = "Investor", @class = "wm", @style = "width:200px" })%>
				</div>
				<div style="float: left; padding-left: 20px;">
					<%: Html.TextBox("Fund", "SEARCH  FUND", new { @id = "Fund", @class = "wm", @style = "width:200px" })%>
				</div>
				<%: Html.HiddenFor(model => model.InvestorID)%>
				<%: Html.HiddenFor(model => model.FundID)%>
				<%}%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="MainCnt" ContentPlaceHolderID="MainContent" runat="server">
	<div id="LibraryContainer">
		<div id="InvestorLibraryList">
		</div>
	</div>
	<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions {
																	  Source = "/Investor/FindInvestorFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { $('#FundID').val(ui.item.id); investorLibrary.search(); }"
	})%>
	<%= Html.jQueryAutoComplete("Investor", new AutoCompleteOptions {
																	Source = "/Investor/FindFundInvestors",
																	MinLength = 1,
																	OnSelect = "function(event, ui) { $('#InvestorID').val(ui.item.id); investorLibrary.search(); }"
	})%>
	<%=Html.jQueryFlexiGrid("InvestorLibraryList", new FlexigridOptions {
	ActionName = "InvestorLibraryList",
	ControllerName = "Investor", 
    HttpMethod = "GET"
	, SortName = "FundName"
	, Paging = true 
	, OnSuccess= "investorLibrary.onGridSuccess"
	, OnRowClick = "investorLibrary.onRowClick"
	, OnInit = "investorLibrary.onInit"
	, OnTemplate = "investorLibrary.onTemplate"
	, RowsLength = 5
})%>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
	<div class="container fundtitle-row">
		<div class="middle-content">
			<div class="container-row">
				<div class="inv-box ">
					<div class="left-nav left-paging">{{if LeftPageIndex>0}}
					<div class="paging" onclick = "javascript:investorLibrary.paging('${LeftPageIndex}',this);">
					<%: Html.Image("left_arrow.png")%>
					</div>
					{{/if}}</div>
					{{each(i,row) FlexGridData.rows}}
						<div class="fund-title"><a href='/Fund?id=${row.cell[0].FundID}' style='color:#000;' target=_blank>${row.cell[0].FundName}</a></div>
					{{/each}}
					<div class="right-nav">{{if RightPageIndex>0}}
					<div class="paging" onclick = "javascript:investorLibrary.paging('${RightPageIndex}',this);">
					<%: Html.Image("right_arrow.png")%>
					</div>
					{{/if}}</div>
				</div>
			</div>
		</div>
	</div>
	<div class="line"></div>
	<div class="container">
		<div class="middle-content">
			<div class="container-row">
				<div class="inv-box  tc-row">
					<div class="left-nav left-padding"><b>Total Committed</b></div>
					{{each(i,row) FlexGridData.rows}}
						<div class="fund-title">${formatCurrency(row.cell[0].TotalCommitted)}</div>
					{{/each}}
					
				</div>
			</div><div class="line"></div>
			{{each(i,investor) Investors}}
				<div class="container-row">
					<div class="inv-box inv-name-row {{if i%2>0}}tc-row{{else}}blue{{/if}}" style="cursor:pointer"  onclick = "javascript:investorLibrary.expand('Investor${investor.InvestorID}',this);">
						<div class="left-nav">&nbsp;&nbsp;
						<div class="expand" style="cursor:pointer">
						<%: Html.Image("Pluss.png", new { @id="expandimg" })%>&nbsp;&nbsp;</div>
						<div class="inv-name"><b>${investor.InvestorName}</b></div></div>
					</div>
				</div><div class="line"></div>
				<div class="container-row inv-det-main" id="Investor${investor.InvestorID}" style="display: none;">
						<div class="inv-box inv-det {{if i%2>0}}inv-row{{else}}inv-arow{{/if}}">
							<div class="container-row">
									<div class="left-nav left-padding">Committed Amount</div>
									{{each(i,row) FlexGridData.rows}}
										<div class="fund-title">
										${getCommimentAmount(row.cell[0].FundID,investor.InvestorID)}
										</div>
									{{/each}}
							</div>
							<div class="container-row">
									<div class="left-nav left-padding">Unfunded Amount</div>
									{{each(i,row) FlexGridData.rows}}
										<div class="fund-title">
										${getUnfundedAmount(row.cell[0].FundID,investor.InvestorID)}
										</div>
									{{/each}}
							</div>
							<div class="container-row">
									<div class="left-nav left-padding">Fund Close</div>
									{{each(i,row) FlexGridData.rows}}
										<div class="fund-title">
										${getFundClose(row.cell[0].FundID,investor.InvestorID)}
										</div>
									{{/each}}
							</div>
							<div class="container-row">
								<div class="left-nav left-padding">Close Date</div>
								{{each(i,row) FlexGridData.rows}}
									<div class="fund-title">
									${getCloseDate(row.cell[0].FundID,investor.InvestorID)}
									</div>
								{{/each}}
							</div>
							<div class="line"></div>
						</div>
				</div>
			{{/each}}
		</div>
	</div>
	</script>
</asp:Content>

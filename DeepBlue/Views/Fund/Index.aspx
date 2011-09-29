<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Fund.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FlexGrid.js")%>
	<%= Html.JavascriptInclueTag("Fund.js")%>
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%= Html.StylesheetLinkTag("flexigrid.css")%>
	<%= Html.StylesheetLinkTag("fund.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">FUNDS</span><span class="arrow"></span><span class="pname">Fund
					Setup</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="fund-box">
		<div class="header">
			<div id="TabMain" class="section-tab-main">
				<div class="section-tab-box">
					<%using (Html.Tab(new { @id = "TabFundGrid", @class = "section-tab-sel", @onclick = "javascript:fund.selectTab(this,'FundDetail');" })) {%>Fund
					Setup
					<%}%>
					<%using (Html.Tab(new { @id = "TabAddNewFund", @onclick = "javascript:fund.selectTab(this,'AddNewFund');" })) {%>Add
					New Fund
					<%}%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="fund-main">
		<div class="section-det" id="FundDetail">
			<% Html.RenderPartial("TBoxTop"); %>
			<table id="FundList" cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<%--<th sortname="FundID" style="width: 1%; display: none;">
							Fund Id
						</th>--%>
						<th sortname="FundName" style="width: 26%" colspan=4>
							Fund Name
						</th>
						<%--<th sortname="TaxId" style="width: 12%">
							Tax ID
						</th>
						<th sortname="CommitmentAmount" style="width: 14%">
							Committed Amount
						</th>
						<th sortname="UnfundedAmount" style="width: 14%">
							Unfunded amount
						</th>
						<th sortname="InceptionDate" style="width: 14%">
							Fund Start Date
						</th>
						<th sortname="ScheduleTerminationDate" style="width: 14%">
							Schedule Termination Date
						</th>
						<th align="right" style="width: 5%">
						</th>--%>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		<div class="section-det" id="AddNewFund" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("FundList", new FlexigridOptions { ActionName = "List"
															   ,ControllerName = "Fund"
															   ,SortName = "FundName"
															   ,ResizeWidth = false
															   ,Paging = true
															   ,OnRowBound = "fund.onRowBound"
															   ,OnTemplate = "fund.onTemplate"
															   ,OnSuccess = "fund.onGridSuccess"
															   ,BoxStyle = false
															   , RowsLength = 50
})%>
	<script type="text/javascript">
		$(document).ready(function(){
			fund.newFundData = <%=JsonSerializer.ToJsonObject(Model)%>;
			fund.init();
		});
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
		{{each(i,row) rows}}
			{{if i%4==0}}
				<tr>
			{{/if}}
				<td><a href="javascript:fund.edit(${row.cell[0]},'${row.cell[1]}')">${row.cell[1]}</a></td>
			{{if i%4==3}}
				</tr>
			{{/if}}
		{{/each}}
	</script>
	<script id="FundAddTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("FundDetail", Model); %>
	</script>
	<script id="FundRateSchduleTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("FundRateSchduleDetail", Model); %>
	</script>
	<script id="FundRateSchduleTierTemplate" type="text/x-jquery-tmpl">
		<% Html.RenderPartial("FundRateSchduleTierDetail", Model); %>
	</script>
	<script id="TabTemplate" type="text/x-jquery-tmpl">
		<div style="float:left">
			<div id="Tab${id}" onmousemove="javascript:$('#tabdel${id}').show();"
			 onmouseout="javascript:$('#tabdel${id}').hide();"
			   class="section-tab section-tab-sel">
				<div class="left"></div>
				<div class="center" onclick="javascript:fund.selectTab($(this).parent(),'Edit${id}');">${FundName}</div>
				<div class="right"></div>
				<div class='tab-delete' style='display:none' id="tabdel${id}" onclick="javascript:fund.deleteTab(${id},true);"></div>
			</div>
		</div>
	</script>
	<script id="SectionTemplate" type="text/x-jquery-tmpl">
		<div class="section-det" id="Edit${id}" style="display: none">
		</div>
	</script>
</asp:Content>

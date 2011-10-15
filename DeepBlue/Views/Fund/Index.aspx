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
				<span class="title">AMBERBROOK FUNDS</span><span class="arrow"></span><span class="pname">Amberbrook Fund
					Setup</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
			<div class="leftcol">
				Amberbrook Fund
			</div>
			<div class="addbtn" style="display: block; margin-left: 123px;">
				<%using (Html.GreenButton(new { @id = "lnkAddFund", @onclick = "javascript:fund.edit(0,'');" })) {%>Add Amberbrook
				Fund<%}%>
			</div>
		</div>
	</div>
	<div class="fund-box">
		<div class="header">
			<div id="TabMain" class="section-tab-main">
				<div class="section-tab-box">
					<%using (Html.Tab(new { @id = "TabFundGrid", @class = "section-tab-sel", @onclick = "javascript:fund.selectTab(this,'FundDetail');" })) {%>Amberbrook Fund
					Setup
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
						<th sortname="FundName">
							Fund Name
						</th>
						<th sortname="TaxId" style="width: 15%">
							Tax ID
						</th>
						<th sortname="InceptionDate" style="width: 20%">
							Fund Start Date
						</th>
						<th sortname="ScheduleTerminationDate" style="width: 20%">
							Schedule Termination Date
						</th>
						<th align="right" style="width: 15%">
						</th>
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
})%>
	<script type="text/javascript">
		$(document).ready(function(){
			fund.newFundData = <%=JsonSerializer.ToJsonObject(Model)%>;
			fund.init();
		});
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
		{{each(i,row) rows}}
			<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
				<td>
					${row.cell[1]}
				</td>
				<td>
					${row.cell[2]}
				</td>
				<td>
					${formatDate(row.cell[3])}
				</td>
				<td>
					${formatDate(row.cell[4])}
				</td>
				<td style="text-align:right">
					{{if row.cell[0]>0}}
					<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:fund.edit(${row.cell[0]},'${row.cell[1]}');" })%>
					{{/if}}
				</td>
			</tr>
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

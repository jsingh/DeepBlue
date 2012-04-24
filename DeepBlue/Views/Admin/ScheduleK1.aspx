<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.ScheduleK1Model>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Schedule K-1
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("ScheduleK1.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("schedulek1.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">SCHEDULES</span><span class="arrow"></span><span class="pname"> Schedule
					Library</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
			<div class="leftcol">
				Fund
			</div>
			<div class="addbtn" style="display: block; margin-left: 123px;">
				<%using (Html.GreenButton(new { @id = "lnkAddScheduleK1", @onclick = "javascript:schedule.edit(0,'');" })) {%>Add Schedule K-1<%}%>
			</div>
			<div class="addbtn" style="display: block; float: right;">
				<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...&nbsp;", new { @id = "SpnLoading", @style = "display:none;float:left;" })%>
				<%: Html.TextBox("SearchFund", "SEARCH FUND", new { @class = "wm", @style = "width:200px" })%>
			</div>
		</div>
	</div>
	<div class="schedule-box">
		<div class="header">
			<div id="TabMain" class="section-tab-main">
				<div class="section-tab-box">
					<%using (Html.Tab(new { @id = "TabScheduleGrid", @class = "section-tab-sel", @onclick = "javascript:schedule.selectTab(this,'ScheduleDetail');" })) {%>Schedule K-1
					<%}%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="schedule-main">
		<div class="section-det" id="ScheduleDetail">
			<% Html.RenderPartial("TBoxTop"); %>
			<table id="ScheduleList" cellpadding="0" cellspacing="0" border="0" class="grid">
				<thead>
					<tr>
						<th sortname="UnderlyingFundName">
							Underlying FundName
						</th>
						<th sortname="PartnershipEIN">
							Partnership's employer identification number
						</th>
						<th sortname="FundName">
							Fund Name
						</th>
						<th sortname="PartnerEIN">
							Partner employer identification number
						</th>
						<th align="right" style="width: 15%">
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
			<%: Html.Hidden("SearchFundID","0", new { @id = "SearchFundID" })%>
			<%: Html.Hidden("SearchUnderlyingFundID","0", new { @id = "SearchUnderlyingFundID" })%>
		</div>
		<div class="section-det" id="AddNewSchedule" style="display: none">
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("SearchFund", new AutoCompleteOptions {
	Source = "/Fund/FindFunds"
																, MinLength = 1
																,
																  OnSelect = "function(event, ui) { $('#SearchFundID').val(ui.item.id); $('#ScheduleList').flexReload(); }"
})%>
<%=Html.jQueryFlexiGrid("ScheduleList", new FlexigridOptions {
	ActionName = "ScheduleK1List"
															   ,ControllerName = "Admin"
															   ,SortName = "FundName"
															   ,ResizeWidth = false
															   ,Paging = true
															   ,OnRowBound = "schedule.onRowBound"
															   ,OnTemplate = "schedule.onTemplate"
															   ,OnSuccess = "schedule.onGridSuccess"
															   ,OnSubmit  = "schedule.onSubmit"
															   ,BoxStyle = false
})%>
<%using (Html.jQueryTemplateScript("ScheduleAddTemplate")) {%>
	<%Html.RenderPartial("ScheduleK1Detail", Model);%>
<%}%>
<%using (Html.jQueryTemplateScript("TabTemplate")) {%>
<%}%>
<%using (Html.jQueryTemplateScript("SectionTemplate")) {%>
<%}%>
<%using (Html.jQueryTemplateScript("GridTemplate")) {%>
		{{each(i,row) rows}}
			<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
				<td>
					${row.cell[1]}
				</td>
				<td>
					${row.cell[2]}
				</td>
				<td>
					${row.cell[3]}
				</td>
				<td>
					${row.cell[4]}
				</td>
				<td style="text-align:right">	
					{{if row.cell[0]>0}}
					<%: Html.Image("pdf.png", new { @class = "gbutton show", @onclick = "javascript:schedule.export(${row.cell[0]});" })%>&nbsp;&nbsp;
					<%: Html.Image("Edit.png", new { @id = "Edit${row.cell[0]}", @class = "gbutton editbtn show", @onclick = "javascript:schedule.edit(${row.cell[0]},'${row.cell[1]}');" })%>&nbsp;&nbsp;
					<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:schedule.deleteScheduleK1(this,${row.cell[0]});" })%>
					{{/if}}
				</td>
			</tr>
		{{/each}}
	<%}%>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/BootStrap/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Fund.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fund
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/Fund.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="container-fluid">
		<div class="row-fluid">
			<div id="myCarousel" class="carousel">
				<!-- Carousel items -->
				<div class="carousel-inner">
					<div class="active item">
						<div class="page-header">
							<div class="pull-left">
								<h3>
								Fund Library</h3>
							</div>
							<div class="pull-right">
								&nbsp;&nbsp;<a href="javascript:fund.open(0);" class="btn btn-primary">Add Fund</a>
							</div>
							<div class="pull-right">
  								<%: Html.TextBox("Fund", "", new { @class = "wm search-query input-large", @placeholder = "SEARCH FUND" })%>
							</div>
							<div class="clear">&nbsp;</div>
						</div>
						<table id="FundList" class="table table-striped table-bordered">
							<thead>
								<tr>
									<th sortname="FundName">
										Fund Name
									</th>
									<th sortname="TaxId">
										Tax ID
									</th>
									<th sortname="InceptionDate">
										Fund Start Date
									</th>
									<th sortname="ScheduleTerminationDate">
										Schedule Termination Date
									</th>
									<th sortname="CommitmentAmount" class="right">
										Commitment Amount
									</th>
									<th sortname="UnfundedAmount" class="right">
										Unfunded Amount
									</th>
								</tr>
							</thead>
						</table>
					</div>
					<div class="item" id="fundeditbox">
					</div>
				</div>
				<%:Html.Hidden("DefaultFundId","0")%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			$("#FundList")
			.flexigrid({ usepager: true
					,url: "/Fund/List"
					,method: "GET"
					,sortname: "FundName"
					,sortorder: "asc"
					,height: 0
					,useBoxStyle: false
					,onSubmit: function (p) {
						p.params=null;
						p.params=new Array();
						p.params[p.params.length]={ "name": "fundId","value": $("#DefaultFundId").val() };
						return true;
					}
					,onTemplate: function (tbody,data) {
						$("#GridTemplate").tmpl(data).appendTo(tbody);
					}
			});
		});
	</script>
	<%using (Html.jQueryTemplateScript("GridTemplate")) {%>
	{{each(i,row) rows}}
	<tr>
		<td>
			<a href="javascript:fund.open(${row.cell[0]});">${row.cell[1]}</a>
		</td>
		<td>
			<a href="javascript:fund.open(${row.cell[0]});">${row.cell[2]}</a>
		</td>
		<td>
			<a href="javascript:fund.open(${row.cell[0]});">${formatDate(row.cell[3])}</a>
		</td>
		<td>
			<a href="javascript:fund.open(${row.cell[0]});">${formatDate(row.cell[4])}</a>
		</td>
		<td style="text-align: right">
			<a href="javascript:fund.open(${row.cell[0]});">${formatCurrency(row.cell[5])}</a>
		</td>
		<td style="text-align: right">
			<a href="javascript:fund.open(${row.cell[0]});">${formatCurrency(row.cell[6])}</a>
		</td>
	</tr>
	{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("FundAddTemplate")) {%>
	<% Html.RenderPartial("FundDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("FundRateSchduleTemplate")) {%>
	<% Html.RenderPartial("FundRateSchduleDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("FundRateSchduleTierTemplate")) {%>
	<% Html.RenderPartial("FundRateSchduleTierDetail", Model); %>
	<%}%>
	<%using (Html.jQueryTemplateScript("TabTemplate")) {%>
	<div style="float: left">
		<div id="Tab${id}" onmousemove="javascript:$('#tabdel${id}').show();" onmouseout="javascript:$('#tabdel${id}').hide();" class="section-tab section-tab-sel">
			<div class="left">
			</div>
			<div class="center" onclick="javascript:fund.selectTab($(this).parent(),'Edit${id}');">
				${FundName}</div>
			<div class="right">
			</div>
			<div class='tab-delete' style='display: none' id="tabdel${id}" onclick="javascript:fund.deleteTab(${id},true);">
			</div>
		</div>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("SectionTemplate")) {%>
	<div class="section-det" id="Edit${id}" style="display: none">
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("InvestorGridTemplate")) {%>
	{{each(i,row) rows}}
	<tr>
		<td>
			${row.cell[0]}
		</td>
		<td style="text-align: right">
			${formatCurrency(row.cell[1])}
		</td>
		<td style="text-align: right">
			${formatCurrency(row.cell[2])}
		</td>
		<td>
			${formatDate(row.cell[3])}
		</td>
	</tr>
	{{/each}}
	<%}%>
	<%using (Html.JavaScript()) {%>
	$(document).ready(function(){ fund.newFundData =
	<%=JsonSerializer.ToJsonObject(Model)%>; fund.init(); });
	<%}%>
	<%using (Html.jQueryTemplateScript("alertTemplate")) {%>
	<div class="span6">
		{{if iswarning==true}}
		<div class="alert">
			<a class="close" data-dismiss="alert">×</a>{{html message}}
		</div>
		{{else}}
		<div class="alert alert-error">
			<a data-dismiss="alert" class="close">×</a>{{html message}}
		</div>
		{{/if}}
	</div>
	<%}%>
	<%= Html.jQueryAutoComplete("Fund", new AutoCompleteOptions { Source = "/Fund/FindFunds"
																, MinLength = 1
																, OnSelect = "function(event, ui) { $('#DefaultFundId').val(ui.item.id); $('#FundList').flexReload(); }"
})%>

</asp:Content>

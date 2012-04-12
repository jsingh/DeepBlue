<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Fund.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="rate-detail control-group rate-schedule-list">
	<%: Html.jQueryTemplateHidden("${index}_IsDelete", "", new { @id = "IsDelete" })%>
	<%: Html.jQueryTemplateHidden("${index}_FundRateScheduleId", "${FRS.FundRateScheduleId}", new { @id = "FundRateScheduleId" })%>
	<div class="control-group pull-left">
		Investor Type&nbsp;<%: Html.DropDownList("${index}_InvestorTypeId", Model.InvestorTypes, new { @id = "InvestorTypeId", @onchange="javascript:fund.changeInvestorType(this);", @class="input-large" , @val = "${FRS.InvestorTypeId}" } )%>
	</div>
	<div class="control-group pull-right">
		<button onclick="javascript:fund.addRateSchedule(this);" class="btn btn-primary">Add Investor Type</button>&nbsp;&nbsp;
		<button onclick="javascript:fund.deleteInvestorType(this);" class="btn btn-danger">Delete Rate Schedule</button>
	</div>
	<%: Html.jQueryTemplateHidden("${index}_IsScheduleChange", "" , new { @id = "IsScheduleChange" })%>
	<%: Html.jQueryTemplateHidden("ScheduleIndex", "${index}", new { @id = "ScheduleIndex" })%>
	<%: Html.jQueryTemplateHidden("${index}_Tiers", "${FRS.FundRateScheduleTiers.length}", new { @id = "TiersCount" })%>
	<table id="RateScheduleList" class="table  table-striped table-bordered">
	<thead>
		<tr>
			<th style="width: 8%">
				Year
			</th>
			<th style="width: 8%">
				From Date
			</th>
			<th style="width: 8%">
				To Date
			</th>
			<th style="width: 15%">
				Fee Calculation Type
			</th>
			<th style="width: 8%">
				Rate %
			</th>
			<th style="width: 8%">
				Flat Fee
			</th>
			<th>
				Comments
			</th>
		</tr>
	</thead>
	<tbody>
		{{each(rowIndex,tier) FRS.FundRateScheduleTiers}} {{tmpl(getTier(index,(rowIndex+1),tier))
		"#FundRateSchduleTierTemplate"}} {{/each}}
	</tbody>
</table>
</div>
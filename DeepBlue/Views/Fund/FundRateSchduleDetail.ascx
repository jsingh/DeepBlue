<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Fund.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="rate-detail">
	<%: Html.jQueryTemplateHidden("${index}_IsDelete", "", new { @id = "IsDelete" })%>
	<%: Html.jQueryTemplateHidden("${index}_FundRateScheduleId", "${FRS.FundRateScheduleId}", new { @id = "FundRateScheduleId" })%>
	<div style="float: left; width: 100%; margin: 10px 0;">
		<div class="editor-label" style="float: left; width: auto;padding-left:0px;">
			<%: Html.Label("Investor") %></div>
		<div class="editor-field" style="float: left; width: auto;">
			&nbsp;<%: Html.DropDownList("${index}_InvestorTypeId", Model.InvestorTypes, new { @id = "InvestorTypeId", @style="width:180px", @onchange="javascript:fund.changeInvestorType(this);", @class="investortype ddlist" , @val = "${FRS.InvestorTypeId}" } )%>
		</div>
		<div style="float: left; margin-left: 15px;">
			<%using (Html.GreenButton(new { @onclick = "javascript:fund.addRateSchedule(this);" })) {%>Add
			Investor Type<%}%>
		</div>
		<div id="DeleteRateSchedule" style="float: right;">
			<%using (Html.GreenButton(new { @class = "green-del-btn", @onclick = "javascript:fund.deleteInvestorType(this);" })) {%>Delete
			Rate Schedule<%}%>
		</div>
	</div>
	<div class="rate-grid">
		<div>
			<%: Html.jQueryTemplateHidden("${index}_IsScheduleChange", "" , new { @id = "IsScheduleChange" })%>
			<%: Html.jQueryTemplateHidden("ScheduleIndex", "${index}", new { @id = "ScheduleIndex" })%>
			<%: Html.jQueryTemplateHidden("${index}_Tiers", "${FRS.FundRateScheduleTiers.length}", new { @id = "TiersCount" })%>
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" border="0" class="grid" id="RateScheduleList">
				<thead>
					<tr>
						<th style="width: 8%; text-align: left;">
							Year
						</th>
						<th style="width: 12%; text-align: left;">
							From Date
						</th>
						<th style="width: 12%; text-align: left;">
							To Date
						</th>
						<th style="width: 20%; text-align: left;">
							Fee Calculation Type
						</th>
						<th style="width: 12%; text-align: left;">
							Rate %
						</th>
						<th style="width: 12%; text-align: left;">
							Flat Fee
						</th>
						<th style="text-align: left;">
							Comments
						</th>
					</tr>
				</thead>
				<tbody>
					{{each(rowIndex,tier) FRS.FundRateScheduleTiers}} {{tmpl(getTier(index,(rowIndex+1),tier))
					"#FundRateSchduleTierTemplate"}} {{/each}}
				</tbody>
				<tfoot>
					<tr>
						<td colspan="7" style="text-align: right; padding-right: 10px;">
							<%using (Html.GreenButton(new { @onclick = "javascript:fund.addNewRow(this);", @style = "float:right" })) {%>Add
							Year<%}%>
						</td>
					</tr>
				</tfoot>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
	</div>
</div>

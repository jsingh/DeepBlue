<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Fund.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr>
	<td>
		<%: Html.Span("Year " + "${rowIndex}", new { @id = "SpnName" })%>
	</td>
	<td>
		{{if rowIndex==1}}
		<%: Html.jQueryTemplateTextBox("${index}_${rowIndex}_StartDate", "${formatDate(tier.StartDate)}" , new { @class="datefield input-small", @inputname="StartDate",  @onchange = "javascript:fund.dateChecking(this);fund.checkChange(this);" })%>
		{{else}}
		<%: Html.jQueryTemplateHidden("${index}_${rowIndex}_StartDate", "${formatDate(tier.StartDate)}", new { @id = "StartDate" })%>
		<%: Html.Span("${formatDate(tier.EndDate)}", new { @id="SpnStartDate" })%>
		{{/if}}
	</td>
	<td>
		<%: Html.jQueryTemplateHidden("${index}_${rowIndex}_EndDate","${formatDate(tier.EndDate)}", new { @id = "EndDate" })%>
		<%: Html.Span("${formatDate(tier.EndDate)}", new { @id="SpnEndDate" })%>
	</td>
	<td>
		<%: Html.DropDownList("${index}_${rowIndex}_MultiplierTypeId", Model.MultiplierTypes, new { @class = "input-medium", @id = "MultiplierTypeId",  @val = "${tier.MultiplierTypeId}", @onchange = "return fund.changeRS(this);" })%>
	</td>
	<td>
		<%: Html.jQueryTemplateTextBox("${index}_${rowIndex}_Rate", "${checkNullOrZero(tier.Rate)}", new { @class = "input-small", @id = "Rate", @readonly = "readonly", @onkeydown = "return jHelper.isCurrency(event);", @onchange = "javascript:fund.changeRate(this);fund.checkChange(this);" })%>
	</td>
	<td>
		<%: Html.jQueryTemplateTextBox("${index}_${rowIndex}_FlatFee", "${checkNullOrZero(tier.FlatFee)}", new { @class = "input-small", @id = "FlatFee", @readonly = "readonly", @onkeydown = "return jHelper.isCurrency(event);", @onchange = "javascript:fund.checkChange(this);" })%>
	</td>
	<td>
		<%: Html.jQueryTemplateTextBox("${index}_${rowIndex}_Notes", "${tier.Notes}", new { @class = "span4", @id = "Notes", @onchange = "javascript:fund.checkChange(this);" })%>
		<%: Html.jQueryTemplateHidden("${index}_${rowIndex}_ManagementFeeRateScheduleId", "${tier.ManagementFeeRateScheduleId}", new { @id = "ManagementFeeRateScheduleId" })%>
		<%: Html.jQueryTemplateHidden("${index}_${rowIndex}_ManagementFeeRateScheduleTierId", "${tier.ManagementFeeRateScheduleTierId}", new { @id = "ManagementFeeRateScheduleTierId" })%>
		<button onclick="javascript:fund.addNewRow(this);" class="btn btn-info">
			Add Row</button>
	</td>
</tr>

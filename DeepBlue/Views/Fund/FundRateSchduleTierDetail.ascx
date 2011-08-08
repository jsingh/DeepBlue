<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Fund.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr {{if rowIndex%2==0}}class="row"{{else}}class="arow"{{/if}}>
<td>
		<%: Html.Span("Year " + "${rowIndex}", new { @id = "SpnName" })%>
</td>
<td>
		{{if rowIndex==1}}
		<%: Html.jQueryTemplateTextBox("${index}_${rowIndex}_StartDate", "${formatDate(tier.StartDate)}" , new { @class="datefield", @inputname="StartDate",  @onchange = "javascript:fund.dateChecking(this);fund.checkChange(this);" })%>
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
		<%: Html.DropDownList("${index}_${rowIndex}_MultiplierTypeId", Model.MultiplierTypes, new { @id = "MultiplierTypeId", @class = "ddlist", @val = "${tier.MultiplierTypeId}", @onchange = "return fund.changeRS(this);" })%>
</td>
<td>
		<%: Html.jQueryTemplateTextBox("${index}_${rowIndex}_Rate", "${checkNullOrZero(tier.Rate)}", new { @id = "Rate", @disabled = "disabled", @onkeypress = "return jHelper.isCurrency(event);", @onchange = "javascript:fund.changeRate(this);fund.checkChange(this);" })%>
</td>
<td>
		<%: Html.jQueryTemplateTextBox("${index}_${rowIndex}_FlatFee", "${checkNullOrZero(tier.FlatFee)}", new { @id = "FlatFee", @disabled = "disabled", @onkeypress = "return jHelper.isCurrency(event);", @onchange = "javascript:fund.checkChange(this);" })%>
</td>
<td>
	<%: Html.jQueryTemplateTextBox("${index}_${rowIndex}_Notes", "${tier.Notes}", new { @id = "Notes", @onchange = "javascript:fund.checkChange(this);" })%>
	<%: Html.jQueryTemplateHidden("${index}_${rowIndex}_ManagementFeeRateScheduleId", "${tier.ManagementFeeRateScheduleId}", new { @id = "ManagementFeeRateScheduleId" })%>
	<%: Html.jQueryTemplateHidden("${index}_${rowIndex}_ManagementFeeRateScheduleTierId", "${tier.ManagementFeeRateScheduleTierId}", new { @id = "ManagementFeeRateScheduleTierId" })%>
</td>
</tr>
			 
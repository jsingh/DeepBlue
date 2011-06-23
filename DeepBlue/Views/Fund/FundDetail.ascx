<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Fund.FundDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="editor-label">
	<%: Html.LabelFor(model => model.FundName) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.FundName) %>
	<%: Html.ValidationMessageFor(model => model.FundName) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.TaxId) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.TaxId) %>
	<%: Html.ValidationMessageFor(model => model.TaxId) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.InceptionDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("InceptionDate",(Model.InceptionDate.Year > 1900 ? Model.InceptionDate.ToString("MM/dd/yyyy") : ""),new { @id = "InceptionDate" }) %>
	<%: Html.ValidationMessageFor(model => model.InceptionDate) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.ScheduleTerminationDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("ScheduleTerminationDate", ((Model.ScheduleTerminationDate ?? Convert.ToDateTime("01/01/1900")).Year > 1900 ? (Model.ScheduleTerminationDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "ScheduleTerminationDate" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.FinalTerminationDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("FinalTerminationDate", ((Model.FinalTerminationDate ?? Convert.ToDateTime("01/01/1900")).Year > 1900 ? (Model.FinalTerminationDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "FinalTerminationDate" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.NumofAutoExtensions) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("NumofAutoExtensions", ((Model.NumofAutoExtensions ?? 0) > 0 ? (Model.NumofAutoExtensions ?? 0).ToString() : ""), new { @id = "NumofAutoExtensions", @onkeypress = "return jHelper.isNumeric(event);" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.DateClawbackTriggered) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("DateClawbackTriggered", ((Model.DateClawbackTriggered ?? Convert.ToDateTime("01/01/1900")).Year > 1900 ? (Model.DateClawbackTriggered ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "DateClawbackTriggered" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.RecycleProvision) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("RecycleProvision", ((Model.RecycleProvision ?? 0) > 0 ? (Model.RecycleProvision ?? 0).ToString() : ""), new { @id = "RecycleProvision", @onkeypress = "return jHelper.isNumeric(event);" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.MgmtFeesCatchUpDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("MgmtFeesCatchUpDate", ((Model.MgmtFeesCatchUpDate ?? Convert.ToDateTime("01/01/1900")).Year > 1900 ? (Model.MgmtFeesCatchUpDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "MgmtFeesCatchUpDate" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Carry) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("Carry", ((Model.Carry ?? 0) > 0 ? (Model.Carry ?? 0).ToString() : ""), new { @id = "Carry", @onkeypress = "return jHelper.isNumeric(event);" })%>
</div>
<% Html.RenderPartial("CustomFieldList", Model.CustomField);%>
<%: Html.Hidden("FundRateSchedulesCount", Model.FundRateSchedules.Count, new { @id = "FundRateSchedulesCount" })%>
<div class="rate-sch-main">
	<div class="rate-header">
		<div class="left">
			<b>Rate Schedules</b></div>
		<div class="right">
			<%:Html.Anchor(Html.Image("add_icon.png").ToHtmlString()+"&nbsp;Add Investor Type","javascript:fund.addRateSchedule(this);",new { @style = "font-size:11px" })%></div>
	</div>
	<div class="rate-schedules">
		<% int index = 1;
	 foreach (DeepBlue.Models.Fund.FundRateScheduleDetail rateSchedule in Model.FundRateSchedules) {%>
		<div class="rate-detail">
			<%: Html.Hidden(index.ToString() + "_Tiers",rateSchedule.FundRateScheduleTiers.Count, new { @id = "TiersCount" })%>
			<%: Html.Hidden(index.ToString() + "_IsDelete",Model.FundRateSchedules.Count,new { @id = "IsDelete" })%>
			<%: Html.Hidden(index.ToString() + "_FundRateScheduleId", rateSchedule.FundRateScheduleId, new { @id = "FundRateScheduleId" })%>
			<div class="editor-label" style="width: 97%">
				<div style="float: left">
					<%: Html.Label("Investor:") %>&nbsp;<%: Html.DropDownList(index.ToString() + "_" + "InvestorTypeId", Model.InvestorTypes,new { @id = "InvestorTypeId", @onchange="javascript:fund.changeInvestorType(this);", @class="investortype ddlist" , @val = rateSchedule.InvestorTypeId.ToString() } )%>
				</div>
				<div id="DeleteRateSchedule" style="float: right;">
					<%:Html.Anchor(Html.Image("largedel.png").ToHtmlString() + "&nbsp;Delete Rate Schedule", "#", new { @onclick = "javascript:fund.deleteInvestorType(this);" })%>
				</div>
			</div>
			<div class="rate-grid" style="width: 100%">
				<%: Html.Hidden(index.ToString() + "_IsScheduleChange", "", new { @id = "IsScheduleChange" })%>
				<table cellpadding="0" cellspacing="0" border="0" class="tblrateschedule" id="RateScheduleList">
					<thead>
						<tr>
							<th style="width: 8%" align="center">
								Name
							</th>
							<th style="width: 12%">
								From Date
							</th>
							<th style="width: 12%">
								To Date
							</th>
							<th style="width: 17%">
								Fee Calculation Type
							</th>
							<th style="width: 15%">
								Rate %
							</th>
							<th style="width: 18%">
								Flat Fee
							</th>
							<th style="width: 18%">
								Comments
							</th>
						</tr>
					</thead>
					<tbody>
						<% int rowIndex = 1;
		 foreach (DeepBlue.Models.Fund.FundRateScheduleTier tier in rateSchedule.FundRateScheduleTiers) {%>
						<tr>
							<td style="width: 8%">
								<div>
									<%: Html.Span("Year " + rowIndex.ToString(), new { @id = "SpnName" })%></div>
							</td>
							<td style="width: 12%">
								<div>
									<%if (rowIndex == 1) {%>
									<%: Html.TextBox(index.ToString() + "_$" + rowIndex.ToString() +"$StartDate", (tier.StartDate.Year > 1900 ? tier.StartDate.ToString("MM/dd/yyyy") : string.Empty),new { @id="StartDate", @inputname="StartDate",  @onchange = "javascript:fund.dateChecking(this);fund.checkChange(this);" })%>
									<%}
		   else {%>
									<%: Html.Hidden(index.ToString() + "_$" + rowIndex.ToString() + "$StartDate", (tier.StartDate.Year > 1900 ? tier.StartDate.ToString("MM/dd/yyyy") : string.Empty), new { @id = "StartDate" })%>
									<%: Html.Span( (tier.EndDate.Year > 1900 ? tier.EndDate.ToString("MM/dd/yyyy") : string.Empty),new { @id="SpnStartDate" })%>
									<%}%>
								</div>
							</td>
							<td style="width: 12%">
								<div>
									<%: Html.Hidden(index.ToString() + "_$" + rowIndex.ToString() + "$EndDate", (tier.EndDate.Year > 1900 ? tier.EndDate.ToString("MM/dd/yyyy") : string.Empty), new { @id = "EndDate" })%><%: Html.Span( (tier.EndDate.Year > 1900 ? tier.EndDate.ToString("MM/dd/yyyy") : string.Empty),new { @id="SpnEndDate" })%></div>
							</td>
							<td style="width: 17%">
								<div>
									<%: Html.DropDownList(index.ToString() + "_$" + rowIndex.ToString() + "$MultiplierTypeId", Model.MultiplierTypes, new { @id = "MultiplierTypeId", @class = "ddlist", @val = tier.MultiplierTypeId.ToString(), @onchange = "return fund.changeRS(this);" })%></div>
							</td>
							<td style="width: 15%">
								<div>
									<%: Html.TextBox(index.ToString() + "_$" + rowIndex.ToString() + "$Rate", (tier.Rate > 0 ? tier.Rate.ToString("0.00") : string.Empty), new { @id = "Rate", @onkeypress = "return jHelper.isCurrency(event);", @onchange = "javascript:fund.changeRate(this);fund.checkChange(this);" })%></div>
							</td>
							<td style="width: 18%">
								<div>
									<%: Html.TextBox(index.ToString() + "_$" + rowIndex.ToString() + "$FlatFee", (tier.FlatFee > 0 ? tier.FlatFee.ToString("0.00") : string.Empty), new { @id = "FlatFee", @onkeypress = "return jHelper.isCurrency(event);",@onchange="javascript:fund.checkChange(this);" })%></div>
							</td>
							<td style="width: 18%">
								<div>
									<%: Html.TextBox(index.ToString() + "_$" + rowIndex.ToString() + "$Notes", tier.Notes, new { @id="Notes",@onchange="javascript:fund.checkChange(this);" })%>
									<%: Html.Hidden(index.ToString() + "_$" + rowIndex.ToString() + "$ManagementFeeRateScheduleId", tier.ManagementFeeRateScheduleId.ToString(), new { @id = "ManagementFeeRateScheduleId" })%>
									<%: Html.Hidden(index.ToString() + "_$" + rowIndex.ToString() + "$ManagementFeeRateScheduleTierId", tier.ManagementFeeRateScheduleTierId.ToString(), new { @id = "ManagementFeeRateScheduleTierId" })%>
								</div>
							</td>
						</tr>
						<% rowIndex++;
		 }%>
					</tbody>
					<tfoot>
						<tr>
							<td colspan="7" style="text-align: right; padding-right: 10px;">
								<%:Html.Anchor(Html.Image("add_icon.png").ToHtmlString()+"&nbsp;Add Year", "#",new { @onclick="javascript:fund.addNewRow(this);" } )%>
							</td>
						</tr>
					</tfoot>
				</table>
			</div>
		</div>
		<% index++;
	 }%>
	</div>
</div>
<div class="editor-label">
	<b>Bank Details</b>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.BankName) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.BankName)%>
	<%: Html.ValidationMessageFor(model => model.BankName) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Account) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Account) %>
	<%: Html.ValidationMessageFor(model => model.Account) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.ABANumber) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("ABANumber",((Model.ABANumber ?? 0) > 0 ? (Model.ABANumber ?? 0).ToString() :  string.Empty), new { @onkeypress = "return jHelper.isNumeric(event);" })%>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Swift) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Swift) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.AccountNumberCash) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.AccountNumberCash) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.FFCNumber) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.FFCNumber) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model
=> model.IBAN) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.IBAN) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Reference) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Reference) %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.AccountOf) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.AccountOf) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Attention) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Attention)%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.Telephone) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Telephone) %>
</div>
<div class="editor-label" style="clear: right">
	<%: Html.LabelFor(model => model.Fax) %>
</div>
<div class="editor-field">
	<%: Html.TextBoxFor(model => model.Fax) %>
</div>
<div class="editor-label" style="height: 10px;">
</div>
<div class="editor-button" style="width: 300px">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateLoading" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return fund.onSubmit('AddNewFund');" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.fund.closeDialog(false);" })%>
	</div>
</div>
<%: Html.HiddenFor(model => model.FundId)%>
<%: Html.HiddenFor(model => model.AccountId)%>
<script type="text/javascript">	$(document).ready(function () { fund.init(); }); </script>

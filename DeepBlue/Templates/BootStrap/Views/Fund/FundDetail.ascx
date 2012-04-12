<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Fund.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<form id="funddetail" class="form-horizontal" onsubmit="return false;">
<fieldset>
	<legend>Fund Details</legend>
	<div class="control-group">
		<%: Html.LabelFor(model => model.FundName, new { @class = "control-label" }) %>
		<div class="controls">
			<%: Html.jQueryTemplateTextBoxFor(model => model.FundName, new { @class="input-xlarge" }) %>
		</div>
	</div>
	<div class="control-group">
		<%: Html.LabelFor(model => model.TaxId, new { @class = "control-label" } ) %>
		<div class="controls">
			<%: Html.jQueryTemplateTextBoxFor(model => model.TaxId, new { @class="input-large" }) %>
		</div>
	</div>
	<div class="control-group">
		<%: Html.LabelFor(model => model.InceptionDate, new { @class = "control-label" })%>
		<div class="controls">
			<div class="input-prepend">
				<span class="add-on"><i class="icon-calendar"></i></span><%: Html.jQueryTemplateTextBox("InceptionDate", "${formatDate(InceptionDate)}", new { @class = "datefield input-medium", @id = "InceptionDate" })%>
			</div>
		</div>
	</div>
	<div class="control-group">
		<%: Html.LabelFor(model => model.ScheduleTerminationDate, new { @class = "control-label" })%>
		<div class="controls">
			<div class="input-prepend">
				<span class="add-on"><i class="icon-calendar"></i></span><%: Html.jQueryTemplateTextBox("ScheduleTerminationDate", "${formatDate(ScheduleTerminationDate)}", new { @class = "datefield input-medium", @id = "ScheduleTerminationDate" })%>
			</div>
		</div>
	</div>
	<div class="control-group">
		<%: Html.LabelFor(model => model.FinalTerminationDate, new { @class = "control-label" })%>
		<div class="controls">
			<div class="input-prepend">
				<span class="add-on"><i class="icon-calendar"></i></span><%: Html.jQueryTemplateTextBox("FinalTerminationDate", "${formatDate(FinalTerminationDate)}", new { @class = "datefield input-medium", @id = "FinalTerminationDate" })%>
			</div>
		</div>
	</div>
	<div class="control-group">
		<%: Html.LabelFor(model => model.NumofAutoExtensions, new { @class = "control-label" })%>
		<div class="controls">
			<%: Html.jQueryTemplateTextBox("NumofAutoExtensions", "${checkNullOrZero(NumofAutoExtensions)}", new { @class = "input-medium", @id = "NumofAutoExtensions", @onkeydown = "return jHelper.isNumeric(event);" })%>
		</div>
	</div>
	<div class="control-group">
		<%: Html.LabelFor(model => model.DateClawbackTriggered, new { @class = "control-label" })%>
		<div class="controls">
			<div class="input-prepend">
				<span class="add-on"><i class="icon-calendar"></i></span><%: Html.jQueryTemplateTextBox("DateClawbackTriggered", "${formatDate(DateClawbackTriggered)}", new { @class = "datefield input-medium", @id = "DateClawbackTriggered" })%>
			</div>
		</div>
	</div>
	<div class="control-group">
		<label for="RecycleProvision" class="control-label">
			Recycle Provision</label>
		<div class="controls">
			<div class="input-prepend">
				<span class="add-on">%</span><%: Html.jQueryTemplateTextBox("RecycleProvision", "${checkNullOrZero(RecycleProvision)}", new { @class = "input-medium", @id = "RecycleProvision", @onkeydown = "return jHelper.isCurrency(event);" })%>
			</div>
		</div>
	</div>
	<div class="control-group">
		<%: Html.LabelFor(model => model.MgmtFeesCatchUpDate, new { @class = "control-label" })%>
		<div class="controls">
			<div class="input-prepend">
				<span class="add-on"><i class="icon-calendar"></i></span><%: Html.jQueryTemplateTextBox("MgmtFeesCatchUpDate", "${formatDate(MgmtFeesCatchUpDate)}", new { @class = "datefield input-medium", @id = "MgmtFeesCatchUpDate" })%>
			</div>
		</div>
	</div>
	<div class="control-group">
		<label for="RecycleProvision" class="control-label">
			Carry</label>
		<div class="controls">
			<div class="input-prepend">
				<span class="add-on">%</span><%: Html.jQueryTemplateTextBox("Carry", "${checkNullOrZero(Carry)}", new { @class = "input-medium", @id = "Carry", @onkeydown = "return jHelper.isCurrency(event);" })%>
			</div>
		</div>
	</div>
	<% Html.RenderPartial("JQueryTemplateCustomFieldList", Model.CustomField);%>
	<div id="accordion" class="accordion">
		<div class="accordion-group">
			<div class="accordion-heading">
				<a href="#InvestorListBox" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle">Investors</a>
			</div>
			<div id="InvestorListBox" class="accordion-body collapse">
				<div class="investor-list control-group">
					<table id="InvestorList" class="table table-striped table-bordered">
						<thead>
							<tr>
								<th sortname="InvestorName">
									Investor Name
								</th>
								<th sortname="CommittedAmount" align="right">
									Committed Amount
								</th>
								<th sortname="UnfundedAmount" align="right">
									Unfunded Amount
								</th>
								<th sortname="CloseDate">
									Close Date
								</th>
							</tr>
						</thead>
					</table>
				</div>
			</div>
		</div>
		<div class="accordion-group">
			<div class="accordion-heading">
				<a href="#RateSchdules" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle">Rate Schedules</a>
			</div>
			<div id="RateSchdules" class="accordion-body collapse">
				<%: Html.jQueryTemplateHidden("FundRateSchedulesCount", "${FundRateSchedules.length}", new { })%>
				<div class="control-group" id="AddNewIVType"  {{if FundRateSchedules.length>0}}style="display:none"{{/if}}>
					<div class="controls">
						<button onclick="javascript:fund.addRateSchedule(this);" class="btn btn-primary pull-right" style="margin-right:10px">Add Investor Type</button>
					</div>
				</div>
				{{each(index,FRS) FundRateSchedules}} {{tmpl(getFundRate(index,FRS)) "#FundRateSchduleTemplate"}}{{/each}}
			</div>
		</div>
		<div class="accordion-group">
			<div class="accordion-heading">
				<a href="#collapse3" data-parent="#accordion" data-toggle="collapse" class="accordion-toggle">Bank Information</a>
			</div>
			<div id="collapse3" class="accordion-body collapse">
				<div class="control-group">
					<label for="BankName" class="control-label">
						Bank Name</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("BankName", "${BankDetail[0].BankName}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="ABANumber" class="control-label">
						ABA Number
					</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("ABANumber", "${BankDetail[0].ABANumber}", new { @class = "input-medium" , @onkeydown = "return jHelper.isNumeric(event);" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="Account" class="control-label">
						Account Name
					</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("Account", "${BankDetail[0].Account}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="AccountNumber" class="control-label">
						Account Number</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("AccountNumber", "${BankDetail[0].AccountNumber}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="FFC" class="control-label">
						FFC</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("FFC", "${BankDetail[0].FFC}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="FFC" class="control-label">
						FFCNumber</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("FFCNumber", "${BankDetail[0].FFCNumber}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="Reference" class="control-label">
						Reference</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("Reference", "${BankDetail[0].Reference}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="Swift" class="control-label">
						Swift Code</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("Swift", "${BankDetail[0].Swift}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="IBAN" class="control-label">
						IBAN
					</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("IBAN", "${BankDetail[0].IBAN}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="AccountPhone" class="control-label">
						Phone
					</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("AccountPhone", "${BankDetail[0].AccountPhone}", new { @class = "input-medium" })%>
					</div>
				</div>
				<div class="control-group">
					<label for="AccountFax" class="control-label">
						Fax
					</label>
					<div class="controls">
						<%: Html.jQueryTemplateTextBox("AccountFax", "${BankDetail[0].AccountFax}", new { @class = "input-medium" })%>
					</div>
				</div>
				<%: Html.jQueryTemplateHidden("AccountId", "${BankDetail[0].AccountId}")%>
			</div>
		</div>
	</div>
	<%: Html.jQueryTemplateHiddenFor(model => model.FundId)%>
	<div class="row-fluid" id="AlertRow">
	</div>
	<div class="form-actions">
		<button id="save" class="btn btn-primary" onclick="return fund.save(this);" data-loading-text="Saving...">
			Save</button>
		<button id="cancel" class="btn" onclick="javascript:fund.cancel(${FundId});">
			Cancel</button>
	</div>
</fieldset>
</form>

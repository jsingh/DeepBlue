<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Fund.FundDetail>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @id = "${getFormIndex()}", @onsubmit = "return false;" })) {%>
<div class="fund-box">
	<div class="fund-box-titie">
		Fund Details</div>
</div>
<div class="line">
</div>
<div class="fund-box-det">
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FundName) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.FundName, new { @style = "width:220px;" }) %>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.TaxId) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.TaxId) %>
	</div>
</div>
<div class="line">
</div>
<div class="fund-box-det">
	<div class="editor-label">
		<%: Html.LabelFor(model => model.InceptionDate) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("InceptionDate", "${formatDate(InceptionDate)}", new { @class = "datefield", @id = "InceptionDate" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.ScheduleTerminationDate) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("ScheduleTerminationDate", "${formatDate(ScheduleTerminationDate)}", new { @class = "datefield", @id = "ScheduleTerminationDate" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.FinalTerminationDate) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("FinalTerminationDate", "${formatDate(FinalTerminationDate)}", new { @class = "datefield", @id = "FinalTerminationDate" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.NumofAutoExtensions) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("NumofAutoExtensions", "${checkNullOrZero(NumofAutoExtensions)}", new { @id = "NumofAutoExtensions", @onkeypress = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.DateClawbackTriggered) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("DateClawbackTriggered", "${formatDate(DateClawbackTriggered)}", new { @class = "datefield", @id = "DateClawbackTriggered" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.RecycleProvision) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("RecycleProvision", "${checkNullOrZero(RecycleProvision)}", new { @id = "RecycleProvision", @onkeypress = "return jHelper.isNumeric(event);" })%>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.MgmtFeesCatchUpDate) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("MgmtFeesCatchUpDate", "${formatDate(MgmtFeesCatchUpDate)}", new { @class = "datefield", @id = "MgmtFeesCatchUpDate" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.Carry) %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBox("Carry", "${checkNullOrZero(Carry)}", new { @id = "Carry", @onkeypress = "return jHelper.isNumeric(event);" })%>
	</div>
	<% Html.RenderPartial("JQueryTemplateCustomFieldList", Model.CustomField);%>
</div>
<div class="line">
</div>
<div>
	<div class="headerbox">
		<div class="title">
			<%: Html.Span("RATE SCHEDULES")%>
		</div>
		<div class="rightdarrow">
			<%: Html.ImageButton("downarrow.png")%>
		</div>
	</div>
	<div class="expandheader expandsel" style="display: none">
		<div class="expandbtn">
			<div class="expandtitle">
				Rate Schedules
			</div>
		</div>
		<div class="rightuarrow">
		</div>
	</div>
	<div class="detail" style="display: none" id="RateSchdules">
		<%: Html.jQueryTemplateHidden("FundRateSchedulesCount", "${FundRateSchedules.length}", new { })%>
		<div style="float: left; width: 100%; margin: 10px 0; display: none;" id="AddNewIVType">
			<div style="float: left">
			</div>
			<div style="float: right; margin-right: 30px;">
				<%using (Html.GreenButton(new { @onclick = "javascript:fund.addRateSchedule(this);" })) {%>Add
				Investor Type<%}%>
			</div>
		</div>
		{{each(index,FRS) FundRateSchedules}} {{tmpl(getFundRate(index,FRS)) "#FundRateSchduleTemplate"}}
		{{/each}}
	</div>
</div>
<div class="line">
</div>
<div>
	<div class="headerbox">
		<div class="title">
			<%: Html.Span("BANK INFORMATION")%>
		</div>
		<div class="rightdarrow">
			<%: Html.ImageButton("downarrow.png")%>
		</div>
	</div>
	<div class="expandheader expandsel" style="display: none">
		<div class="expandbtn">
			<div class="expandtitle">
				Bank Information
			</div>
		</div>
		<div class="rightuarrow">
		</div>
	</div>
	<div class="detail" style="display: none" id="BankInformation">
		<div>
			<div class="editor-label">
				<%: Html.Label("Bank Name-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("BankName", "${BankDetail[0].BankName}")%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Account Number-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("AccountNo", "${BankDetail[0].AccountNo}")%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("ABA Number-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("ABANumber", "${BankDetail[0].ABANumber}", new { @onkeypress = "return jHelper.isNumeric(event);" })%>
			</div>
			<div class="editor-label">
				<%: Html.Label("Swift Code-")%>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("Swift", "${BankDetail[0].Swift}") %>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Account Number Cash-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("AccountNumberCash", "${BankDetail[0].AccountNumberCash}")%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("FFC Number-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("FFCNumber", "${BankDetail[0].FFCNumber}")%>
			</div>
			<div class="editor-label">
				<%: Html.Label("IBAN-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("IBAN", "${BankDetail[0].IBAN}")%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Reference-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("Reference", "${BankDetail[0].Reference}")%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Account Of-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("AccountOf", "${BankDetail[0].AccountOf}")%>
			</div>
			<div class="editor-label">
				<%: Html.Label("Attention-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("Attention", "${BankDetail[0].Attention}")%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Telephone-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("Telephone", "${BankDetail[0].Telephone}")%>
			</div>
			<div class="editor-label" style="clear: right">
				<%: Html.Label("Fax-") %>
			</div>
			<div class="editor-field">
				<%: Html.jQueryTemplateTextBox("Fax", "${BankDetail[0].Fax}")%>
				<%: Html.jQueryTemplateHidden("AccountId", "${BankDetail[0].AccountId}")%>
			</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="editor-button" style="width: 300px; padding: 20px 0 0 0">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("{{if FundId>0}}modifyfund.png{{else}}addfund.png{{/if}}", new { @class = "default-button", onclick = "return fund.save(this);" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateLoading" })%>
	</div>
</div>
<%: Html.jQueryTemplateHiddenFor(model => model.FundId)%>
<%}%>
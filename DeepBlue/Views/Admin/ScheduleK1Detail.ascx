<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Admin.ScheduleK1Model>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @id = "${getFormIndex()}", @onsubmit = "return false;" })) {%>
<%: Html.jQueryTemplateHiddenFor(model => model.PartnersShareFormID)%>
<%: Html.jQueryTemplateHiddenFor(model => model.PartnerAddressID)%>
<div>
	<div class="schedule-box headerbox" style="display:none">
		<div class="schedule-box-title">
			Part - I<span style="padding-left: 10px">Information About the Partnership</span></div>
	</div>
	<div class="schedule-box expandheader expandsel">
		<div class="schedule-box-title">
			Part - I<span style="padding-left: 10px">Information About the Partnership</span></div>
	</div>
	<div class="line">
	</div>
	<div class="schedule-box-det detail">
		<div class="editor-label">
			Underlying Fund
		</div>
		<div class="editor-field" style="width:auto">
			<%: Html.jQueryTemplateTextBoxFor(model => model.UnderlyingFundName, new { @style = "width:300px" })%>
			<%: Html.jQueryTemplateHiddenFor(model => model.UnderlyingFundID) %>
		</div>
		<div class="editor-label">
			Partnership's employer identification number
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.PartnershipEIN) %>
		</div>
		<div class="editor-label">
			IRS Center where partnership filed return
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.IRSCenter) %>
		</div>
		<div class="right-col editor-label" style="clear: right">
			Check if this is a publicly traded partnership (PTP)
		</div>
		<div class="editor-field">
			<%: Html.CheckBoxFor(model => model.IsPTP, new { @val = "${IsPTP}" })%>
		</div>
	</div>
</div>
<div>
	<div class="schedule-box headerbox" style="display:none">
		<div class="schedule-box-title">
			Part - II<span style="padding-left: 10px">Information About the Partner</span></div>
	</div>
	<div class="schedule-box expandheader expandsel">
		<div class="schedule-box-title">
			Part - II<span style="padding-left: 10px">Information About the Partner</span></div>
	</div>
	<div class="line">
	</div>
	<div class="schedule-box-det detail">
	<div class="editor-label">
		Fund
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.FundName) %>
		<%: Html.jQueryTemplateHiddenFor(model => model.FundID) %>
	</div>
	<div class="editor-label">
		Partner's identifying number
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.PartnerEIN) %>
	</div>
	<div class="right-col editor-label" style="clear:right">
		What type of entity is this partner?
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.PartnerType) %>
	</div>
    <div class="editor-label">
		<%: Html.Label("Address1") %>
	</div>
	<div class="editor-field address">
		<%: Html.jQueryTemplateTextBoxFor(model => model.PartnerAddress1)%>
	</div>
	<div class="right-col editor-label" style="clear:right;">
		<%: Html.Label("Address2") %>
	</div>
	<div class="editor-field address">
		<%: Html.jQueryTemplateTextBoxFor(model => model.PartnerAddress2)%>
	</div>
	<div class="editor-label">
		<%: Html.Label("City") %>
	</div>
	<div class="editor-field" style="clear:right;">
		<%: Html.jQueryTemplateTextBoxFor(model => model.PartnerCity)%>
	</div>
	<div class="right-col editor-label" style="clear:right;">
		<%: Html.Label("Zip") %>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.PartnerZip)%>
	</div>
	<div class="editor-label" style="clear:right;">
		<%: Html.Label("Country")%>
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.PartnerCountryName)%>
		<%: Html.Hidden("PartnerCountry", "${PartnerCountry}")%>
	</div>
	<div class="editor-row" id="AddressStateRow" style=clear:right;width:auto;{{if PartnerCountry!=225}}display:none;{{/if}}>
		<div class="right-col editor-label">
			<%: Html.Label("State") %>
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.PartnerStateName)%>
			<%: Html.Hidden("PartnerState", "${PartnerState}")%>
		</div>
	</div>
	<div class="editor-label">
		General partner or LLC member-manager
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsGeneralPartner, new { @val = "${IsGeneralPartner}" })%>
	</div>
	<div class="right-col editor-label" style="clear: right">
		Limited partner or other LLC member
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsLimitedPartner, new { @val = "${IsLimitedPartner}" })%>
	</div>
	<div class="editor-label">
		Domestic partner
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsDomesticPartner, new { @val = "${IsDomesticPartner}" })%>
	</div>
	<div class="right-col editor-label" style="clear: right">
		Foreign partner
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsForeignPartner, new { @val = "${IsForeignPartner}" })%>
	</div>
	<div class="schedule-box">
		<div class="schedule-box-title">
			Partner's share of profit, loss, and capital (see instructions)
		</div>
	</div>
	<div class="clear">
		&nbsp;</div>
	<div class="pull-left">
		<div class="editor-label">
			Begining
		</div>
		<div class="editor-label">
			Profit
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.BeginingProfit, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Loss
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.BeginingLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Capital
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.BeginingCapital, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
	</div>
	<div class="pull-left">
		<div class="right-col editor-label">
			Ending
		</div>
		<div class="clear">
			&nbsp;</div>
		<div class="right-col editor-label">
			Profit
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.EndingProfit, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="right-col editor-label">
			Loss
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.EndingLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="right-col editor-label">
			Capital
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.EndingCapital, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
	</div>
	<div class="clear">
		&nbsp;</div>
	<div class="editor-label">
		Partner's share of liabilities at year end:
	</div>
	<div class="editor-label">
		Nonrecourse
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.NonRecourse, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="right-col editor-label" style="clear:right">
		Qualified nonrecourse financing
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.QualifiedNonRecourseFinancing, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		Recourse
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.Recourse, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="clear">
		&nbsp;</div>
	<div class="editor-label">
		Partner's capital account analysis:
	</div>
	<div class="editor-label">
		Beginning capital account
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.BeginningCapitalAccount, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="right-col editor-label" style="clear:right">
		Capital contributed during the year
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.CapitalContributed, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		Current year increase (decrease)
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.CurrentYearIncrease, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="right-col editor-label" style="clear:right">
		Withdrawals and distributions
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.WithdrawalsAndDistributions, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		Ending capital account
	</div>
	<div class="editor-field">
		<%: Html.jQueryTemplateTextBoxFor(model => model.EndingCapitalAccount, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
	</div>
	<div class="editor-label">
		Tax basis
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsTaxBasis, new { @val = "${IsTaxBasis}" })%>
	</div>
	<div class="right-col editor-label" style="clear: right;">
		GAAP
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsGAAP, new { @val = "${IsGAAP}" })%>
	</div>
	<div class="editor-label">
		Section 704(b) book
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsSection704, new { @val = "${IsSection704}" })%>
	</div>
	<div class="right-col editor-label" style="clear: right;">
		Other (explain)
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsOther, new { @val = "${IsOther}" })%>
	</div>
	<div class="editor-label">
		Did the partner contribute property with a built-in gain or loss?
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.IsGain, new { @val = "${IsGain}" })%>
	</div>
	<div class="clear">
		&nbsp;</div> 
</div>
</div>
<div>
	<div class="schedule-box headerbox" style="display:none">
		<div class="schedule-box-title">
			Part - III<span style="padding-left: 10px">Partner's Share of Current Year Income, Deductions, Credits, and Other Items</span></div>
	</div>
	<div class="schedule-box expandheader expandsel">
		<div class="schedule-box-title">
			Part - III<span style="padding-left: 10px">Partner's Share of Current Year Income, Deductions, Credits, and Other Items</span></div>
	</div>
	<div class="line">
	</div>
	<div class="schedule-box-det detail">
	<div class="pull-left">
		<div class="editor-label">
			Ordinary business income (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.OrdinaryBusinessIncome, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Net rental real estate income (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.NetRentalRealEstateIncome, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Other net rental income (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.OtherNetRentalIncomeLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Guaranteed payments
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.GuaranteedPayment, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Interest income
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.InterestIncome, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Ordinary dividends
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.OrdinaryDividends, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Qualified dividends
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.QualifiedDividends, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Royalties
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.Royalties, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Net short-term capital gain (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.NetShortTermCapitalGainLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Net long-term capital gain (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.NetLongTermCapitalGainLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Collectibles (28%) gain (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.Collectibles28GainLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Unrecaptured section 1250 gain
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.UnrecapturedSection1250Gain, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Net section 1231 gain (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.NetSection1231GainLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Other income (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.OtherIncomeLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Section 179 deduction
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.Section179Deduction, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Other deductions
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.OtherDeduction, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="editor-label">
			Self-employment earnings (loss)
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.SelfEmploymentEarningLoss, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
	</div>
	<div class="pull-left">
		<div class="right-col editor-label">
			Credits
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.Credits, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="right-col editor-label">
			Foreign transactions
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.ForeignTransaction, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="right-col editor-label">
			Alternative minimum tax (AMT) items
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.AlternativeMinimumTax, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="right-col editor-label">
			Tax-exempt income and nondeductible expenses
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.TaxExemptIncome, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="right-col editor-label">
			Distributions
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.Distribution, new { @onkeydown = "return jHelper.isCurrency(event);" })%>
		</div>
		<div class="right-col editor-label">
			Other information
		</div>
		<div class="editor-field">
			<%=Html.jQueryTemplateTextArea("OtherInformation", "${OtherInformation}", 5, 32, null)%>
		</div>
	</div>
</div>
</div>
<div class="editor-button" style="width: 480px; padding: 20px 0 0 0">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateLoading" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("{{if PartnersShareFormID>0}}Modify-Schedule-K-1_active.png{{else}}Add-Schedule-K-1_active.png{{/if}}", new { @class = "default-button", onclick = "return schedule.save(this);" })%>
	</div><div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("cancel_active.png", new { @class = "default-button", onclick = "javascript:schedule.cancel(${PartnersShareFormID});" })%>
	</div>
	{{if PartnersShareFormID>0}}
    <div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("Export-Pdf_active.png", new { @class = "default-button", onclick = "return schedule.export(${PartnersShareFormID});" })%>
	</div>
	{{/if}}
</div>
<% } %>

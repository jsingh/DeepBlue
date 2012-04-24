<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.ScheduleK1Model>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title></title>
	<style type="text/css">
		* {
			padding: 0;
			margin: 0;
		}
		body {
			font-family: Arial;
			font-size: 11px;
		}
		.container {
			width: 1040px;
			margin-left: 0px;
			margin-right: 0px;
			background-color: #fff;
		}
		.pull-left {
			float: left;
		}
		.pull-right {
			float: right;
		}
		.code-number {
			font-size: 22px;
		}
		.clear {
			clear: both;
		}
		.clear-right {
			clear: right;
		}
		.clear-left {
			clear: left;
		}
		.check-box {
			width: 18px;
			border: 1px solid #000;
			margin-right: 5px;
			text-align: center;
			font-size: 15px;
			padding-top: 3px;
			padding-bottom: 1px;
		}
		.check-box-label {
			margin-top: 5px;
		}
		.check-box-group {
			margin-left: 20px;
		}
		.border {
			border: 1px solid #000;
		}
		.border-left {
			border-left: 1px solid #000;
		}
		.border-right {
			border-right: 1px solid #000;
		}
		.border-top {
			border-top: 1px solid #000;
		}
		.border-bottom {
			border-bottom: 1px solid #000;
		}
		p.small-para {
			line-height: 14px;
		}
		.input-line {
			border-bottom: 1px solid #000;
			margin-left: 5px;
			padding-bottom: 3px;
			width: 100px;
			line-height: 8px;
		}
		.font-xlarge {
			font-size: x-large;
		}
		.font-11 {
			font-size: 11px;
		}
		.font-12 {
			font-size: 12px;
		}
		.font-13 {
			font-size: 13px;
		}
		.font-14 {
			font-size: 14px;
		}
		.font-15 {
			font-size: 15px;
		}
		.font-16 {
			font-size: 16px;
		}
		.font-18 {
			font-size: 18px;
		}
		.bold {
			font-weight: bold;
		}
		h1 {
			font-size: 24px;
		}
		.cutter {
			margin: 10px;
		}
		.cutter-left {
			margin-left: 10px;
		}
		.cutter-right {
			margin-right: 10px;
		}
		.cutter-top {
			margin-top: 10px;
		}
		.cutter-bottom {
			margin-bottom: 10px;
		}
		.input-font-family {
			font-family: Times New Roman;
			text-transform: uppercase;
		}
		td {
			vertical-align: top;
		}
		.align-left {
			text-align: left;
		}
		.align-center {
			text-align: center;
		}
		.align-right {
			text-align: right;
		}
		.valign-top {
			vertical-align: top;
		}
		.valign-middle {
			vertical-align: middle;
		}
		.valign-bottom {
			vertical-align: bottom;
		}
		/* page-1 start */
		.page-1 .left-col {
			width: 520px;
		}
		.page-1 .right-col {
			width: 520px;
		}
		.page-1 .box-content {
		}
		.page-1 .box-row {
			padding-left: 10px;
			padding-top: 3px;
			padding-bottom: 3px;
		}
		.page-no-box {
			font-size: 16px;
			padding-left: 25px;
			padding-right: 25px;
			padding-top: 3px;
			padding-bottom: 3px;
		}
		.page-1 .first-cell {
			padding-right: 15px;
		}
		.left-padding-cell {
			padding-left: 10px;
		}
		.top-padding-cell {
			padding-left: 10px;
		}
		.bottom-padding-cell {
			padding-left: 10px;
		}
		.part-3-tbl td {
			padding: 4px 3px;
			height: 38px;
		}
		
		/* page-1 end */
		/* page-2 start */
		.page-2 td {
			vertical-align: middle;
		}
		/* page-2 end */
	</style>
</head>
<body>
	<div class="container page-1">
		<div class="pull-right code-number clear">
			65110
		</div>
		<div class="pull-right clear" style="width: 500px;">
			<div class="pull-left">
				<div class="pull-left check-box">
					&nbsp;</div>
				<div class="pull-left check-box-label">
					Final K-1</div>
			</div>
			<div class="pull-left check-box-group">
				<div class="pull-left check-box">
					&nbsp;</div>
				<div class="pull-left check-box-label">
					Amended K-1</div>
			</div>
			<div class="pull-right check-box-group">
				<div class="pull-left check-box-label">
					OMB No. 1545-0099</div>
			</div>
		</div>
		<div class="clear">
			<div class="pull-left left-col">
				<div class="pull-left" style="margin-top: 10px;">
					<h4>
						Schedule K-l</h4>
				</div>
				<div class="pull-right font-xlarge bold" style="margin-right: 50px">
					2010
				</div>
				<div class="clear">
					<div class="pull-left">
						(Form 1065)</div>
					<div class="pull-right" style="margin-right: 50px">
						For calendar year 2010, or tax</div>
				</div>
				<div class="clear" style="margin-top: 10px;">
					<div class="pull-left">
						<p class="small-para">
							Department of the Treasury<br />
							Internal Revenue Service</p>
					</div>
					<div class="pull-right" style="margin-right: 50px">
						<div class="pull-left">
							year beginning</div>
						<div class="pull-left input-line">
							&nbsp;</div>
						<div class="pull-left">
							&nbsp;,&nbsp;&nbsp;2010</div>
						<div class="clear" style="margin-left: 38px">
							<div class="pull-left clear">
								ending</div>
							<div class="pull-left input-line">
							</div>
							<div class="pull-left">
								&nbsp;,&nbsp;&nbsp;</div>
							<div class="pull-left input-line" style="width: 40px;">
							</div>
						</div>
					</div>
				</div>
				<div class="clear">
					<div class="pull-left">
						<h1>
							Partner's Share of Income, Deductions,<br />
							Credits, etc.<span style="margin-left: 140px; margin-right: 10px;" class="font-16">></span><span class="font-16">See separate instructions.</span></h1>
					</div>
				</div>
				<div class="clear border-left border-top box-content">
					<div class="pull-left clear box-row" style="padding-top: 10px; padding-bottom: 10px;">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="page-no-box border">
									Part I
								</td>
								<td class="font-18 bold" style="padding-left: 50px;">
									Information About the Partnership
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 bold first-cell">
									A
								</td>
								<td class="font-16" style="vertical-align: top">
									Partnership's employer identification number<br />
									<span class="input-font-family font-18">
										<div style="height: 25px;">
											<%=Model.PartnershipEIN%></div>
									</span>
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row" style="padding-top: 5px; padding-bottom: 5px;">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 bold first-cell">
									B
								</td>
								<td class="font-16" style="vertical-align: top">
									Partnership's name, address, city, state, and ZIP code<br />
									<br />
									<span class="input-font-family font-18">
										<div style="height: 60px">
											<%=Model.UnderlyingFundName%><br />
											<%if (String.IsNullOrEmpty(Model.PartnershipAddress1) == false) {%>
											<%=Model.PartnershipAddress1%><br />
											<%}%>
											<%if (String.IsNullOrEmpty(Model.PartnershipAddress2) == false) {%>
											<%=Model.PartnershipAddress2%><br />
											<%}%>
											<%=Model.PartnershipCity%>,&nbsp;<%=Model.PartnershipStateName%>&nbsp;<%=Model.PartnershipZip%>
										</div>
									</span>
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row" style="padding-top: 3px; padding-bottom: 3px;">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 bold first-cell">
									C
								</td>
								<td class="font-16">
									IRS Center where partnership filed return<br />
									<span class="input-font-family font-18">
										<div style="height: 25px">
											<%=Model.IRSCenter%></div>
									</span>
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row" style="padding-top: 3px; padding-bottom: 3px;">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 bold first-cell">
									D
								</td>
								<td class="font-16 check-box">
									<%if (Model.IsPTP == true) {%>X<%} %>
								</td>
								<td class="font-16">
									&nbsp;&nbsp;Check if this is a publicly traded partnership (PTP)
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row" style="padding-top: 10px; padding-bottom: 10px;">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="page-no-box border">
									Part II
								</td>
								<td class="font-18 bold" style="padding-left: 50px;">
									Information About the Partner
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row" style="padding-top: 5px; padding-bottom: 5px;">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 bold first-cell">
									E
								</td>
								<td class="font-16">
									Partner's identifying number<br />
									<span class="input-font-family font-18">
										<div style="height: 25px">
											<%=Model.PartnerEIN%></div>
									</span>
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row" style="padding-top: 10px;">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 bold first-cell">
									F
								</td>
								<td class="font-16">
									Partner's name, address, city, state, and ZIP code<br />
									<br />
									<span class="input-font-family font-18">
										<div style="height: 80px">
											<%=Model.FundName%><br />
											<%if (string.IsNullOrEmpty(Model.PartnerAddress1) == false) {%>
											<%=Model.PartnerAddress1%><br />
											<%}%>
											<%if (string.IsNullOrEmpty(Model.PartnerAddress2) == false) {%>
											<%=Model.PartnerAddress2%><br />
											<%}%>
											<%=Model.PartnerCity%>,&nbsp;<%=Model.PartnerStateName%>&nbsp;<%=Model.PartnerZip%>
										</div>
									</span>
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row" style="margin-top: 10px;">
						<div class="pull-left">
							<table cellpadding="0" cellspacing="0" border="0">
								<tr>
									<td class="font-16 bold first-cell">
										G
									</td>
									<td class="font-16">
										<div class="check-box">
											<%if (Model.IsGeneralPartner == true) {%>X<%}%></div>
									</td>
									<td class="font-16">
										&nbsp;&nbsp;General partner or LLC<br />
										&nbsp;&nbsp;member-manager
									</td>
								</tr>
							</table>
						</div>
						<div class="pull-right">
							<table cellpadding="0" cellspacing="0" border="0">
								<tr>
									<td class="font-16">
										<div class="check-box">
											<%if (Model.IsLimitedPartner == true) {%>X<%}%></div>
									</td>
									<td class="font-16">
										&nbsp;&nbsp;Limited partner or other&nbsp;&nbsp;<br />
										&nbsp;&nbsp;LLC member
									</td>
								</tr>
							</table>
						</div>
					</div>
					<div class="clear box-row" style="margin-top: 20px; height: 30px;">
						<div class="pull-left">
							<table cellpadding="0" cellspacing="0" border="0">
								<tr>
									<td class="font-16 bold first-cell">
										H
									</td>
									<td class="font-16">
										<div class="check-box">
											<%if (Model.IsDomesticPartner == true) {%>X<%} %></div>
									</td>
									<td class="font-16">
										&nbsp;&nbsp;Domestic partner
									</td>
								</tr>
							</table>
						</div>
						<div class="pull-right" style="margin-right: 62px;">
							<table cellpadding="0" cellspacing="0" border="0">
								<tr>
									<td class="font-16">
										<div class="check-box">
											<%if (Model.IsForeignPartner) { %>X<%} %></div>
									</td>
									<td class="font-16">
										&nbsp;&nbsp;Foreign partner
									</td>
								</tr>
							</table>
						</div>
					</div>
					<div class="clear box-row" style="margin-top: 20px">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 bold first-cell">
									I
								</td>
								<td class="font-16 first-cell">
									&nbsp;&nbsp;What type of entity is this partner?
								</td>
								<td class="font-16 input-line" style="width: 200px">
									<%=Model.PartnerType%>
								</td>
							</tr>
						</table>
					</div>
					<div class="clear box-row" style="margin-top: 20px">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 bold first-cell">
									J
								</td>
								<td class="font-16 first-cell">
									&nbsp;Partner's share of profit, loss, and capital (see instructions):
								</td>
							</tr>
						</table>
					</div>
					<div class="clear box-row" style="margin-top: 20px">
						<table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16 align-center border-right" colspan="2" style="padding-top: 3px; padding-bottom: 3px">
									Begining
								</td>
								<td class="font-16 align-center" style="padding-top: 3px; padding-bottom: 3px">
									Ending
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
								<td class="font-16 align-left border-bottom" style="padding-top: 3px; padding-bottom: 3px">
									Profit
								</td>
								<td class="font-16 align-right border-bottom border-right" style="padding-top: 3px; padding-bottom: 3px">
									<%=FormatHelper.NumberFormat(Model.BeginingProfit)%>&nbsp;%&nbsp;
								</td>
								<td class="font-16 align-right border-bottom" style="padding-top: 3px; padding-bottom: 3px">
									<%=FormatHelper.NumberFormat(Model.EndingProfit)%>&nbsp;%&nbsp;
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
								<td class="font-16 align-left border-bottom" style="padding-top: 3px; padding-bottom: 3px">
									Loss
								</td>
								<td class="font-16 align-right border-bottom border-right" style="padding-top: 3px; padding-bottom: 3px">
									<%=FormatHelper.NumberFormat(Model.BeginingLoss)%>&nbsp;%&nbsp;
								</td>
								<td class="font-16 align-right border-bottom" style="padding-top: 3px; padding-bottom: 3px">
									<%=FormatHelper.NumberFormat(Model.EndingLoss)%>&nbsp;%&nbsp;
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
								<td class="font-16 align-left border-bottom" style="padding-top: 3px; padding-bottom: 3px">
									Capital
								</td>
								<td class="font-16 align-right border-bottom border-right" style="padding-top: 3px; padding-bottom: 3px">
									<%=FormatHelper.NumberFormat(Model.BeginingCapital)%>&nbsp;%&nbsp;
								</td>
								<td class="font-16 align-right border-bottom" style="padding-top: 3px; padding-bottom: 3px">
									<%=FormatHelper.NumberFormat(Model.EndingCapital)%>&nbsp;%&nbsp;
								</td>
							</tr>
						</table>
					</div>
					<div class="clear box-row" style="margin-top: 20px">
						<table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
							<tr>
								<td class="font-16 bold first-cell" style="padding-top: 3px; padding-bottom: 3px">
									K
								</td>
								<td class="font-16 first-cell" style="padding-top: 3px; padding-bottom: 3px" colspan="2">
									&nbsp;Partner's share of liabilities at year end:
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									&nbsp;Nonrecourse&nbsp;......................................................&nbsp;$
								</td>
								<td class="font-16 align-right" style="padding-top: 3px; padding-bottom: 3px; width: 150px;">
									<div class="border-bottom align-right">
										<%=FormatHelper.NumberFormat(Model.NonRecourse)%>&nbsp;&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									&nbsp;Qualified nonrecourse financing&nbsp;..................&nbsp;$
								</td>
								<td class="font-16 align-right" style="padding-top: 3px; padding-bottom: 3px; width: 150px;">
									<div class="border-bottom align-right">
										<%=FormatHelper.NumberFormat(Model.QualifiedNonRecourseFinancing)%>&nbsp;&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									&nbsp;Recourse&nbsp;........................................................&nbsp;$
								</td>
								<td class="font-16 align-right" style="padding-top: 3px; padding-bottom: 3px; width: 150px;">
									<div class="border-bottom align-right">
										<%=FormatHelper.NumberFormat(Model.Recourse)%>&nbsp;&nbsp;</div>
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-top box-row" style="margin-top: 20px">
						<table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
							<tr>
								<td class="font-16 bold first-cell" style="padding-top: 3px; padding-bottom: 3px">
									L
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									Partner's capital account analysis:
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									Beginning capital account&nbsp;.................................&nbsp;$
								</td>
								<td class="font-16 align-right" style="padding-top: 3px; padding-bottom: 3px; width: 150px;">
									<div class="border-bottom align-right">
										<%=FormatHelper.NumberFormat(Model.BeginningCapitalAccount)%>&nbsp;&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									Capital contributed during the year&nbsp;................&nbsp;$
								</td>
								<td class="font-16 align-right" style="padding-top: 3px; padding-bottom: 3px; width: 150px;">
									<div class="border-bottom align-right">
										<%=FormatHelper.NumberFormat(Model.CapitalContributed)%>&nbsp;&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									Current year increase (decrease)&nbsp;..................&nbsp;$
								</td>
								<td class="font-16 align-right" style="padding-top: 3px; padding-bottom: 3px; width: 150px;">
									<div class="border-bottom align-right">
										<%=FormatHelper.NumberFormat(Model.CurrentYearIncrease)%>&nbsp;&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									Withdrawals and distributions&nbsp;.........................&nbsp;$
								</td>
								<td class="font-16 align-right" style="padding-top: 3px; padding-bottom: 3px; width: 150px;">
									<div class="border-bottom align-right">
										<%=FormatHelper.NumberFormat(Model.WithdrawalsAndDistributions)%>&nbsp;&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16" style="padding-top: 3px; padding-bottom: 3px">
									Ending capital account&nbsp;....................................&nbsp;$
								</td>
								<td class="font-16 align-right" style="padding-top: 3px; padding-bottom: 3px; width: 150px;">
									<div class="border-bottom align-right">
										<%=FormatHelper.NumberFormat(Model.EndingCapitalAccount)%>&nbsp;&nbsp;</div>
								</td>
							</tr>
						</table>
					</div>
					<div class="clear box-row" style="margin-top: 20px">
						<table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16">
									<div class="check-box">
										<%if (Model.IsTaxBasis == true) {%>X<%}%></div>
								</td>
								<td class="font-16" style="width: 120px">
									Tax basis
								</td>
								<td class="font-16">
									<div class="check-box">
										<%if (Model.IsGAAP == true) {%>X<%}%></div>
								</td>
								<td class="font-16" style="width: 120px">
									GAAP
								</td>
								<td class="font-16">
									<div class="check-box">
										<%if (Model.IsSection704 == true) {%>X<%} %></div>
								</td>
								<td class="font-16">
									Section 704(b) book
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16">
									<div class="check-box">
										<%if (Model.IsOther == true) {%>X<%} %></div>
								</td>
								<td class="font-16">
									Other (explain)
								</td>
							</tr>
						</table>
					</div>
					<div class="clear border-bottom box-row" style="margin-top: 22px;">
						<table cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td class="font-16 first-cell">
									M
								</td>
								<td class="font-16" colspan="4">
									Did the partner contribute property with a built-in gain or loss?
								</td>
							</tr>
							<tr>
								<td style="width: 30px; padding-top: 3px; padding-bottom: 3px">
									&nbsp;
								</td>
								<td class="font-16 first-cell" style="padding-top: 3px; padding-bottom: 3px">
									<div class="check-box pull-left">
										<%if (Model.IsGain == true) {%>X<%} %></div>
									<div class="pull-left" style="margin-top: 3px">
										Yes</div>
									<div class="check-box pull-left" style="margin-left: 45px;">
										<%if (Model.IsGain == false) {%>X<%} %></div>
									<div class="pull-left" style="margin-top: 3px">
										No</div>
								</td>
							</tr>
						</table>
					</div>
				</div>
			</div>
			<div class="pull-right right-col">
				<div class="clear  border-top box-content" style="margin-top: 5px;">
					<div class="pull-left clear border-left border-right box-row" style="padding-top: 10px; padding-bottom: 10px;">
						<table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
							<tr>
								<td>
									<div class="page-no-box border">
										Part III</div>
								</td>
								<td class="font-18 bold" style="padding-left: 50px;">
									Partner's Share of Current Year Income,<br />
									Deductions, Credits, and Other Items
								</td>
							</tr>
						</table>
					</div>
					<div class="clear">
						<table class="part-3-tbl border" cellpadding="0" cellspacing="0" border="0" style="width: 100%">
							<tr>
								<td class="font-16 align-center border-right">
									1
								</td>
								<td class="font-16 border-right">
									Ordinary business income (loss)
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.OrdinaryBusinessIncome)%>
									</div>
								</td>
								<td class="font-16 align-center border-right">
									15
								</td>
								<td class="font-16">
									Credits
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.Credits)%></div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									2
								</td>
								<td class="font-16 border-right border-top">
									Net rental real estate income (loss)
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.NetRentalRealEstateIncome)%></div>
								</td>
								<td class="font-16 align-center border-right border-top">
									&nbsp;
								</td>
								<td class="font-16 border-top">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									3
								</td>
								<td class="font-16 border-right border-top">
									Other net rental income (loss)
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.OtherNetRentalIncomeLoss)%></div>
								</td>
								<td class="font-16 align-center border-right border-top">
									16
								</td>
								<td class="font-16 border-top">
									Foreign transactions
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.ForeignTransaction)%></div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									4
								</td>
								<td class="font-16 border-right border-top">
									Guaranteed payments
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.GuaranteedPayment)%></div>
								</td>
								<td class="font-16 align-center border-right">
									&nbsp;
								</td>
								<td class="font-16">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									5
								</td>
								<td class="font-16 border-right border-top">
									Interest income
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.InterestIncome)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									6a
								</td>
								<td class="font-16 border-right border-top">
									Ordinary dividends
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.OrdinaryDividends)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									6b
								</td>
								<td class="font-16 border-right border-top">
									Qualified dividends
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.QualifiedDividends)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									7
								</td>
								<td class="font-16 border-right border-top">
									Royalties
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.Royalties)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									8
								</td>
								<td class="font-16 border-right border-top">
									Net short-term capital gain (loss)
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.NetShortTermCapitalGainLoss)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									9a
								</td>
								<td class="font-16 border-right border-top">
									Net long-term capital gain (loss)
									<div class="pull-right input-font-family" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.NetLongTermCapitalGainLoss)%></div>
								</td>
								<td class="font-16 align-center border-right border-top">
									17
								</td>
								<td class="font-16 border-top">
									Alternative minimum tax (AMT) items<div class="pull-right input-font-family font-18">
										<%=FormatHelper.NumberFormat(Model.AlternativeMinimumTax)%></div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									9b
								</td>
								<td class="font-16 border-right border-top">
									Collectibles (28%) gain (loss)
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.Collectibles28GainLoss)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									9c
								</td>
								<td class="font-16 border-right border-top">
									Unrecaptured section 1250 gain
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.UnrecapturedSection1250Gain)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									10
								</td>
								<td class="font-16 border-right border-top">
									Net section 1231 gain (loss)
									<div class="pull-right input-font-family font-18">
										<%=FormatHelper.NumberFormat(Model.NetSection1231GainLoss)%></div>
								</td>
								<td class="font-16 align-center border-right border-top">
									18
								</td>
								<td class="font-16 border-top">
									Tax-exempt income and nondeductible expenses<div class="pull-right input-font-family font-18">
										<%=FormatHelper.NumberFormat(Model.TaxExemptIncome)%></div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									11
								</td>
								<td class="font-16 border-right border-top">
									Other income (loss)
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.OtherIncomeLoss)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16 border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									12
								</td>
								<td class="font-16 border-right border-top">
									Section 179 deduction
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.Section179Deduction)%></div>
								</td>
								<td class="font-16 align-center border-right border-top">
									19
								</td>
								<td class="font-16 border-top">
									Distributions<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.Distribution)%></div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									<div style="width: 30px;">
										13</div>
								</td>
								<td class="font-16 border-right border-top">
									<div style="width: 222px;">
										Other deductions<div class="pull-right input-font-family font-18" style="padding: 0; margin: 0;">
											<%=FormatHelper.NumberFormat(Model.OtherDeduction)%></div>
									</div>
								</td>
								<td class="font-16 align-center border-right border-top">
									20
								</td>
								<td class="font-16 border-top" rowspan="5">
									Other information<br />
									<p class="font-13">
										<%=Model.OtherInformation%></p>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16 border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16 border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right border-top">
									14
								</td>
								<td class="font-16 border-right border-top">
									Self-employment earnings (loss)
									<div class="pull-right input-font-family font-18" style="height: 25px;">
										<%=FormatHelper.NumberFormat(Model.SelfEmploymentEarningLoss)%></div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16 border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
								<td class="font-16 align-center border-right">
									<div class="pull-right input-font-family font-18">
										&nbsp;</div>
								</td>
							</tr>
							<tr>
								<td class="font-18 align-center border-top" colspan="4" style="vertical-align: middle">
									*See attached statement for additional information.
								</td>
							</tr>
							<tr>
								<td class="font-11 align-left border-top" colspan="4" style="vertical-align: top">
									<div style="height: 229px;">
										<p>
											F<br />
											O<br />
											R<br />
											<br />
											I<br />
											R<br />
											S<br />
											<br />
											U<br />
											S<br />
											E<br />
											<br />
											O<br />
											N<br />
											L<br />
											Y
										</p>
									</div>
								</td>
							</tr>
						</table>
					</div>
				</div>
			</div>
		</div>
		<div class="clear">
			<div class="pull-left bold font-14" style="margin-top: 2px;">
				BAA For Paperwork Reduction Act Notice, see Instructions for Form 1065.
			</div>
			<div class="pull-right" style="margin-top: 2px;">
				Schedule K-l (Form 1065) 2010
			</div>
		</div>
		<div class="clear">
			<div class="pull-left font-16" style="margin-top: 2px">
				PARTNER 17</div>
			<div class="pull-right" style="margin-top: 2px">
				PTPA0312L 01125111</div>
		</div>
	</div>
	<div class="container page-2 clear">
		<div class="clear border-bottom" style="height: 15px;">
			<div class="pull-left font-14">
				Schedule K-1 (Form 1065) 2010
				<%=Model.UnderlyingFundName%>&nbsp;<%=Model.PartnershipEIN%></div>
			<div class="pull-right font-14">
				Page 2</div>
		</div>
		<div class="clear" style="padding-top: 5px;">
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td class="font-13">
						This list identifies the codes used on Schedule K-1 for all partners and provides summarized reporting information for partners who file Form 1040. For detailed reporting and filing information, see the separate Partner's Instructions for Schedule K-1 and the instructions for your income tax return.
					</td>
				</tr>
			</table>
		</div>
		<div class="clear" style="padding-top: 10px">
			<table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
				<tr>
					<td style="width: 50%; vertical-align: top;">
						<table cellpadding="0" cellspacing="2" border="0" style="width: 100%">
							<tr>
								<td class="font-14 align-center" style="width: 40px;">
									1
								</td>
								<td class="font-12" colspan="3">
									<b>Ordinary business income (loss).</b> Determine whether the income (loss) is passive
									<br />
									or non passive and enter on your return as follows.
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
								<td class="font-14">
									Report on
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
								<td>
									Passive loss
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
								<td>
									Passive income
								</td>
								<td>
									Schedule E, line 28, column (g)
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
								<td>
									Nonpassive loss
								</td>
								<td>
									Schedule E, line 28, column (h)
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
								<td>
									Nonpassive income
								</td>
								<td>
									Schedule E, line 28, column (i)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									2
								</td>
								<td class="font-12">
									<b>Net rental real estate income (loss)</b>
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									3
								</td>
								<td class="font-12">
									<b>Other net rental income (loss)</b>
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									Net income
								</td>
								<td>
									Schedule E, line 28, column (g)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									Net loss
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									4
								</td>
								<td class="font-12">
									<b>Guaranteed payments</b>
								</td>
								<td>
									Schedule E, line 28, column (j)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									5
								</td>
								<td class="font-12">
									<b>Interest income</b>
								</td>
								<td>
									Form 1040, line 8a
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									6a
								</td>
								<td class="font-12">
									<b>Ordinary dividends</b>
								</td>
								<td>
									Form 1040, line 9a
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									6b
								</td>
								<td class="font-12">
									<b>Qualified dividends</b>
								</td>
								<td>
									Form 1040, line 9b
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									7
								</td>
								<td class="font-12">
									<b>Royalties</b>
								</td>
								<td>
									Schedule E, line 4
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									8
								</td>
								<td class="font-12">
									<b>Net short-tenn capital gain (loss)</b>
								</td>
								<td>
									Schedule D, line 5, column (I)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									9a
								</td>
								<td class="font-12">
									<b>Net long-term capital gain (loss)</b>
								</td>
								<td>
									Schedule D, line 12, column (I)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									9b
								</td>
								<td class="font-12">
									<b>Collectibles (28%) gain (loss)</b>
								</td>
								<td>
									28% Rate Gain Worksheet, line<br />
									4 (Schedule D Instructions)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									10
								</td>
								<td class="font-12">
									<b>Net section 1231 gain (loss)</b>
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									11
								</td>
								<td class="font-12">
									<b>Other income (loss)</b>
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-16">
									<i>Code</i>
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Other portfolio income (loss)
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Involuntary conversions
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Section 1256 contracts and straddles
								</td>
								<td>
									Form 6781, line 1
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">D</b>&nbsp;&nbsp;Mining exploration costs recapture
								</td>
								<td>
									See Pub 535
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">E</b>&nbsp;&nbsp;Cancellation of debt
								</td>
								<td>
									Form 1040, line 21 or Form 982
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">F</b>&nbsp;&nbsp;Other income (loss)
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									12
								</td>
								<td class="font-12">
									Section 179 deduction
								</td>
								<td>
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									13
								</td>
								<td class="font-12">
									Other deductions
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Cash contributions (50%)
								</td>
								<td rowspan="7" style="vertical-align: top">
									<div class="pull-left">
										<div class="border-bottom" style="width: 20px; height: 1px;">
											&nbsp;</div>
										<div class="border-right" style="width: 20px; height: 160px;">
											&nbsp;</div>
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px; margin-top: 80px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div style="margin-top: 70px; padding-left: 10px;">
											See the Partner's<br />
											Instructions</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Cash contributions (30%)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Noncash contributions (50%)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">D</b>&nbsp;&nbsp;Noncash contributions (30%)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">E</b>&nbsp;&nbsp;Capital ga in property to a 50%<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;organization (30%)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">F</b>&nbsp;&nbsp;Capital gain property (20%)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">G</b>&nbsp;&nbsp;Contributions (100%)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">H</b>&nbsp;&nbsp;Contributions (100%)
								</td>
								<td class="font-12">
									Form 4952, line 1
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">I</b>&nbsp;&nbsp;Deductions -royalty income
								</td>
								<td class="font-12">
									Schedule E, line 18
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">J</b>&nbsp;&nbsp;Section 59(e)(2) expenditures
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">K</b>&nbsp;&nbsp;Deductions -portfolio (2% floor)
								</td>
								<td class="font-12">
									Schedule A, line 23
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">L</b>&nbsp;&nbsp;Deductions -portfolio (other)
								</td>
								<td class="font-12">
									Schedule A, line 28
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">M</b>&nbsp;&nbsp;Amounts paid for medical<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;insurance
								</td>
								<td class="font-12">
									Schedule A, line 1 or<br />
									Form 1040, line 29
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">N</b>&nbsp;&nbsp;Educational assistance benefits
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">O</b>&nbsp;&nbsp;Dependent care benefits
								</td>
								<td class="font-12">
									Form 2441, line 12
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">P</b>&nbsp;&nbsp;Preproductive period expenses
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">Q</b>&nbsp;&nbsp;Commercial revitalization deduction from<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;rental real estate activities
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">R</b>&nbsp;&nbsp;Pensions and IRAs
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">S</b>&nbsp;&nbsp;Reforestation expense deduction
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">T</b>&nbsp;&nbsp;Domestic production activities information
								</td>
								<td class="font-12">
									See Form 8903 Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">U</b>&nbsp;&nbsp;Qualified production activities income
								</td>
								<td class="font-12">
									Form 8903, line 7b
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">V</b>&nbsp;&nbsp;Employer's Form W-2 wages
								</td>
								<td class="font-12">
									Form 8903, line 17
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">W</b>&nbsp;&nbsp;Other deductions
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									14
								</td>
								<td class="font-12">
									Self-employment earnings (loss)
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td colspan="3" class="font-12">
									<b>Note.</b> IflOU have a secfion 179 deducfion or any partner-level deductions, see the<br />
									<i>Partner 5 Instructions before complefing Schedule SE.</i>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Net earnings (loss) from self-employment
								</td>
								<td class="font-12">
									Schedule SE, Section A or B
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Gross farming or fishing income
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Gross non -farm income
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									15
								</td>
								<td class="font-12">
									Credits
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Low-income housing credit (section 42U)(5)
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Low-income housing credit (other) from
								</td>
								<td class="font-12">
									pre-2008 buildings
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Low-income housing credit (other) from<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;pre-2008 buildings
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Low-income housing credit (section 42U)(5)<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;from post-2007 buildings
								</td>
								<td class="font-12">
									Form 8586, line 11
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">D</b>&nbsp;&nbsp;Low-income housing credit (other) from<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;post-2007 buildings
								</td>
								<td class="font-12">
									Form 8586, line 11
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">E</b>&nbsp;&nbsp;Qualified rehabilitation expenditures (rental<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;real estate)
								</td>
								<td class="font-12" rowspan="3">
									<div class="pull-left">
										<div class="border-bottom" style="width: 20px; height: 1px;">
											&nbsp;</div>
										<div class="border-right" style="width: 20px; height: 70px;">
											&nbsp;</div>
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px; margin-top: 35px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div style="margin-top: 25px; margin-left: 10px;">
											See the Partner's<br />
											Instructions</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">F</b>&nbsp;&nbsp;Other rental real estate credits
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">G</b>&nbsp;&nbsp;Other rental credits
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">H</b>&nbsp;&nbsp;Undistributed capital gains credit
								</td>
								<td class="font-12">
									Form 1040, line 71; check box a
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">I</b>&nbsp;&nbsp;Alcohol and cellulosic biofuel fuels credit
								</td>
								<td class="font-12">
									Form 6478, line 8
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">J</b>&nbsp;&nbsp;Work opportunity credit
								</td>
								<td class="font-12">
									Form 5884, line 3
								</td>
							</tr>
						</table>
					</td>
					<td style="width: 50%; vertical-align: top; text-align: right;">
						<table align="left" cellpadding="0" cellspacing="2" border="0" style="width: 100%; text-align: left;">
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-14">
									<i>Code</i>
								</td>
								<td class="font-14">
									<i>Report On</i>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">K</b>&nbsp;&nbsp;Disabled access credit
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">L</b>&nbsp;&nbsp;Empowerment zone and renewal community<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;employment credit
								</td>
								<td class="font-12">
									Form 8844, line 3
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">L</b>&nbsp;&nbsp;Empowerment zone and renewal community<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;employment credit
								</td>
								<td class="font-12">
									Form 8844, line 3
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">M</b>&nbsp;&nbsp;Credit for increasing research activities
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">N</b>&nbsp;&nbsp;Credit for employer social security and<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Medicare taxes
								</td>
								<td class="font-12">
									Form 8846, line 5
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">O</b>&nbsp;&nbsp;Backup withholding
								</td>
								<td class="font-12">
									Form 1040, line 61
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">P</b>&nbsp;&nbsp;Other credits
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									16
								</td>
								<td class="font-12">
									Foreign transactions
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Name of country or U.S. possession
								</td>
								<td class="font-12" rowspan="3">
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
										<div class="border-right" style="width: 20px; height: 40px;">
											&nbsp;</div>
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px; margin-top: 20px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div style="margin-top: 15px; margin-left: 10px;">
											Form 1116, Part I</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Gross income from all sources
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Gross income sourced at partner level
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-16" colspan="2">
									<i>Foreign gross income sourced at partnership level</i>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">D</b>&nbsp;&nbsp;Passive category
								</td>
								<td class="font-12" rowspan="3">
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
										<div class="border-right" style="width: 20px; height: 40px;">
											&nbsp;</div>
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px; margin-top: 20px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div style="margin-top: 15px; margin-left: 10px;">
											Form 1116, Part I</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">E</b>&nbsp;&nbsp;General category
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">F</b>&nbsp;&nbsp;Other
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-16" colspan="2">
									<i>Deductions allocated and apportioned at partner level</i>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">G</b>&nbsp;&nbsp;Interest expense
								</td>
								<td class="font-12">
									Form 1116, Part I
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">H</b>&nbsp;&nbsp;Other
								</td>
								<td class="font-12">
									Form 1116, Part I
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-16" colspan="2">
									<i>Deductions allocated and apportioned at partnership level to<br />
										foreign source income</i>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">I</b>&nbsp;&nbsp;Passive category
								</td>
								<td class="font-12" rowspan="3">
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
										<div class="border-right" style="width: 20px; height: 40px;">
											&nbsp;</div>
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px; margin-top: 20px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div style="margin-top: 15px; margin-left: 10px;">
											Form 1116, Part I</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">J</b>&nbsp;&nbsp;General category
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">K</b>&nbsp;&nbsp;Other
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-16" colspan="2">
									<i>Other information</i>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">L</b>&nbsp;&nbsp;Total foreign taxes paid
								</td>
								<td class="font-12">
									Form 1116, Part II
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">M</b>&nbsp;&nbsp;Total foreign taxes accrued
								</td>
								<td class="font-12">
									Form 1116, Part II
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">N</b>&nbsp;&nbsp;Reduction in taxes available for credit
								</td>
								<td class="font-12">
									Form 1116, line 12
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">O</b>&nbsp;&nbsp;Foreign trading gross receipts
								</td>
								<td class="font-12">
									Form 8873
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">P</b>&nbsp;&nbsp;Extraterritorial income exclusion
								</td>
								<td class="font-12">
									Form 8873
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">Q</b>&nbsp;&nbsp;Other foreign transactions
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									17
								</td>
								<td class="font-12">
									Alternative minimum tax (AMT) items
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Post-1986 depreciation adjustment
								</td>
								<td class="font-12" rowspan="6">
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
										<div class="border-right" style="width: 20px; height: 110px;">
											&nbsp;</div>
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px; margin-top: 60px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div style="margin-top: 35px; margin-left: 10px;">
											See the Partner's<br />
											Instructions and<br />
											the Instructions for<br />
											Form 6251</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Adjusted gain or loss
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Depletion (other than oil & gas)
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">D</b>&nbsp;&nbsp;Oil, gas, & geothermal -gross income
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">E</b>&nbsp;&nbsp;Oil, gas, & geothermal -deductions
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">F</b>&nbsp;&nbsp;Other AMT items
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									18
								</td>
								<td class="font-12">
									Tax-exempt income and nondeductible expenses
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Tax-exempt interest income
								</td>
								<td class="font-12">
									Form 1040. line 8b
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Other tax-exempt income
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Nondeductible expenses
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									19
								</td>
								<td class="font-12">
									Distributions
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Cash and marketable securities
								</td>
								<td class="font-12" rowspan="3">
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
										<div class="border-right" style="width: 20px; height: 40px;">
											&nbsp;</div>
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px; margin-top: 20px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div style="margin-top: 15px; margin-left: 10px;">
											See the Partner's<br />
											Instructions</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Distribution subject to section 737
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Other property
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									20
								</td>
								<td class="font-12">
									Other infonnation
								</td>
								<td class="font-12">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">A</b>&nbsp;&nbsp;Investment income
								</td>
								<td class="font-12">
									Form 4952, line 4a
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">B</b>&nbsp;&nbsp;Investment expenses
								</td>
								<td class="font-12">
									Form 4952, line 5
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">C</b>&nbsp;&nbsp;Fuel tax credit information
								</td>
								<td class="font-12">
									Form 4136
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">D</b>&nbsp;&nbsp;Qualified rehabilitation expenditures (other than<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;rental real estate)
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">E</b>&nbsp;&nbsp;Basis of energy property
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">F</b>&nbsp;&nbsp;Recapture of low-income housing credit (section<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;42U)(5))
								</td>
								<td class="font-12">
									Form 8611, line 8
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">G</b>&nbsp;&nbsp;Recapture of low-income housing credit (other)
								</td>
								<td class="font-12">
									Form 8611, line 8
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">H</b>&nbsp;&nbsp;Recapture of investment credit
								</td>
								<td class="font-12">
									Form 4255
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">I</b>&nbsp;&nbsp;Recapture of other credits
								</td>
								<td class="font-12">
									See the Partner's Instructions
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">J</b>&nbsp;&nbsp;Look-back interest -completed<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;long-term contracts
								</td>
								<td class="font-12">
									See Form 8697
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">K</b>&nbsp;&nbsp;Look-back interest -income<br />
									&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;forecast method
								</td>
								<td class="font-12">
									See Form 8866
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">L</b>&nbsp;&nbsp;Dispositions of property with<br />
									&nbsp;&nbsp;&nbsp;&nbsp;section 179 deductions
								</td>
								<td class="font-12" rowspan="14" style="vertical-align: top">
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
										<div class="border-right" style="width: 20px; height: 250px;">
											&nbsp;</div>
										<div class="border-top" style="width: 20px; height: 1px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div class="border-top" style="width: 20px; height: 1px; margin-top: 120px;">
											&nbsp;</div>
									</div>
									<div class="pull-left">
										<div style="margin-top: 105px; margin-left: 10px;">
											See the Partner's<br />
											Instructions</div>
									</div>
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">M</b>&nbsp;&nbsp;Recapture of section 179 deduction
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">N</b>&nbsp;&nbsp;Interest expense for corporate partners
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">O</b>&nbsp;&nbsp;Section 453(1)(3) information
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">P</b>&nbsp;&nbsp;Section 453A(c) information
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">Q</b>&nbsp;&nbsp;Section 1260(b) information
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">R</b>&nbsp;&nbsp;Interest allocable to production expenditures
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">S</b>&nbsp;&nbsp;CCF nonqua/ified withdrawals
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">T</b>&nbsp;&nbsp;Depletion information -oil and gas
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">U</b>&nbsp;&nbsp;Amortization of reforestation costs
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">V</b>&nbsp;&nbsp;Unrelated business taxable income
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">W</b>&nbsp;&nbsp;Precontribution gain (loss)
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">X</b>&nbsp;&nbsp;Section 108(i) information
								</td>
							</tr>
							<tr>
								<td class="font-14 align-center">
									&nbsp;
								</td>
								<td class="font-12">
									<b class="font-14">Y</b>&nbsp;&nbsp;Other information
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</div>
		<div class="clear" style="padding-top: 20px;">
			<div class="pull-left font-16">
				PARTNER 17:&nbsp;<%=Model.FundName%>&nbsp;<%=Model.PartnerEIN%></div>
			<div class="pull-right font-14">
				PTPA0312L 01/25/11 Schedule K-1 (Form 1065) 2010
			</div>
		</div>
	</div>
	<div class="clear page-3">
		<div class="font-14" style="font-family: Times New Roman">
			<%=Model.UnderlyingFundName%>&nbsp;&nbsp;&nbsp;&nbsp;<%=Model.PartnershipEIN%></div>
		<div class="clear" style="padding-top: 10px">
			<div class="pull-left font-11">
				SCHEDULE K-1 (FORM 1065) 2010</div>
			<div class="pull-left font-13">
				&nbsp;&nbsp;SUPPLEMENTAL INFORMATION</div>
			<div class="pull-right font-12">
				PAGE 3</div>
		</div>
		<div class="clear border" style="padding: 10px; margin-top: 10px; height: 1430px;">
			<div class="clear font-16 bold">
				SUPPLEMENTAL INFORMATION</div>
			<br />
			<br />
			<br />
			<div class="clear">
				<p class="font-14" style="font-family: Times New Roman; line-height: 25px;">
					AS A RESULT OF THE 1986 TAX REFORM ACT, EXPENSES RELATED TO PORTFOLIO INCOME INCLUDED ON LINE 10 OF THE K-1 GENERALLY ARE MISCELLANEOUS ITEMIZED DEDUCTIONS FOR AN INDIVIDUAL AND ARE SUBJECT TO THE 2% OF ADJUSTED GROSS INCOME LIMITATION. YOU SHOULD CONSULT YOUR TAX ADVISOR REGARDING THE POSSIBILITY OF TREATING THESE DEDUCTIONS AS ORDINARY BUSINESS EXPENSES INCURRED IN CONNECTION WITH THE OPERATION OF A VENTURE CAPITAL PARTNERSHIP.</p>
			</div>
		</div>
		<div class="clear font-14" style="margin-top: 10px; font-family: Times New Roman;">
			PARTNER 17:&nbsp;<%=Model.FundName%>&nbsp;<%=Model.PartnerEIN%>
		</div>
		<div class="clear font-11" style="width: 100%; text-align: center; height: 20px;">
			SPSL1201L 07/31/03</div>
	</div>
	<div class="clear page-4 border-top border-left border-right" style="padding: 30px; font-family: Times New Roman;">
		<div class="clear border" style="width: 100%; margin-bottom: 2px;">
			<br />
			<br />
			<div class="font-18" style="text-align: center; padding-left: 40px;">
				<table cellpadding="0" cellspacing="2" border="0" style="text-align: center">
					<tr>
						<td>
							<%=Model.UnderlyingFundName%>
						</td>
					</tr>
					<tr>
						<td>
							<%=Model.PartnershipAddress1%>
						</td>
					</tr>
					<tr>
						<td>
							<%=Model.PartnershipCity%>&nbsp;<%=Model.PartnershipStateName%>&nbsp;<%=Model.PartnershipZip%>
						</td>
					</tr>
					<tr>
						<td>
							<%=Model.PartnershipEIN%>
						</td>
					</tr>
				</table>
			</div>
			<br />
			<div class="clear" style="padding-right: 40px">
				<div class="pull-right font-13">
					<%=DateTime.Now.ToString("MMMM dd,yyyy")%></div>
			</div>
			<br />
			<div class="clear font-18" style="padding-left: 40px;">
				<table cellpadding="0" cellspacing="2" border="0">
					<tr>
						<td>
							<p>
								<%=Model.FundName%><br />
								<%if (string.IsNullOrEmpty(Model.PartnerAddress1) == false) {%>
								<%=Model.PartnerAddress1%><br />
								<%}%>
								<%if (string.IsNullOrEmpty(Model.PartnerAddress2) == false) {%>
								<%=Model.PartnerAddress2%><br />
								<%}%>
								<%=Model.PartnerCity%>&nbsp;<%=Model.PartnerStateName%>&nbsp;<%=Model.PartnerZip%>
							</p>
						</td>
					</tr>
				</table>
			</div>
			<br />
			<br />
			<br />
			<div class="clear font-18" style="padding-left: 40px;">
				<table cellpadding="0" cellspacing="2" border="0">
					<tr>
						<td>
							<p>
								RE:<br />
								<%=Model.UnderlyingFundName%><br />
								<%=Model.PartnershipEIN%><br />
								Schedule K-1 from Partnership's 2010 Return of Income</p>
						</td>
					</tr>
				</table>
			</div>
			<br />
			<br />
			<br />
			<div class="clear font-18" style="padding-left: 40px;">
				Dear&nbsp;<%=Model.FundName%>:
			</div>
			<br />
			<br />
			<br />
			<div class="clear font-18" style="padding-left: 40px; padding-right: 40px;">
				<p>
					Enclosed is your 2010 Schedule K-1 (Form 1065) Partner's Share of Income, Deductions, Credits, Etc. from
					<%=Model.UnderlyingFundName%>. This information reflects the amounts you need to complete your income tax return. The amounts shown are your distributive share of partnership tax items to be reported on your tax return, and may not correspond to actual distributions you have received during the year. This information is included in the Partnership's 2010 Federal Return of Partnership Income that was filed with the Internal Revenue Service.</p>
			</div>
			<br />
			<br />
			<div class="clear font-18" style="padding-left: 40px;">
				If you have any questions concerning this information, please contact us immediately.
			</div>
			<br />
			<br />
			<div class="clear font-18" style="padding-left: 40px;">
				Sincerely,
			</div>
			<br />
			<br />
			<br />
			<br />
			<div class="clear font-18" style="padding-left: 40px;">
				<%=Model.UnderlyingFundName%>
			</div>
			<br />
			<br />
			<div class="clear font-18" style="padding-left: 40px;">
				Enclosure(s)
			</div>
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
			<br />
		</div>
	</div>
	<div class="clear border-top" style="width: 100%;">
		&nbsp;</div>
</body>
</html>

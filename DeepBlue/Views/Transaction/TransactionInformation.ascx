<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="section investor-titlebox">
	<div class="info">
		<div class="info-title" style="overflow: hidden">
			${InvestorName}
		</div>
		<div class="editor-field" style="margin-left: 150px; text-align: right;">
			Display Name
		</div>
		<div class="editor-field">
			${DisplayName}
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="section">
	<div class="info">
		<div class="info-title" style="overflow: hidden; margin-bottom: 10px;">
			Investment Details
		</div>
	</div>
	<div class="info" style="clear: both; width: 90%;">
		<% Html.RenderPartial("TBoxTop"); %>
		<table cellpadding="0" cellspacing="0" border="0" id="InvestmentList" class="grid">
			<thead>
				<tr>
					<th sortname="FundName" style="width: 15%">
						Fund Name
					</th>
					<th sortname="InvestorType" style="width: 15%">
						Investor Type
					</th>
					<th sortname="TotalCommitment" style="width: 15%;text-align:right;" align=right>
						Total Commitment
					</th>
					<th sortname="UnfundedAmount" style="width: 15%;text-align:right;" align=right>
						Unfunded Amount
					</th>
					<th sortname="FundClose" style="width: 15%">
						Fund Close
					</th>
					<th style="width: 15%">
					</th>
				</tr>
			</thead>
		</table>
		<% Html.RenderPartial("TBoxBottom"); %>
	</div>
	<br />
</div>
<div class="line">
</div>

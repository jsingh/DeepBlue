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
	<div class="inv-info-title" style="overflow: hidden; margin-bottom: 0px;">
		<div class="title">
			Investment Details</div>
		<div id="InvListLoading" class="title">
		</div>
		<%using (Html.BlueButton(new { @onclick = "javascript:investorCommitment.add(this);" })) {%>Add
		Commitment<%}%>
	</div>
	<div class="info" style="clear: both; width: 90%;">
		<table cellpadding="0" cellspacing="0" border="0" id="InvestmentList" class="grid">
			<thead>
				<tr>
					<th sortname="FundName" style="width: 25%">
						Fund Name
					</th>
					<th sortname="InvestorType"  style="width: 18%">
						Investor Type
					</th>
					<th sortname="TotalCommitment"  align="right">
						Total Commitment
					</th>
					<th sortname="UnfundedAmount"  align="right">
						Unfunded Amount
					</th>
					<th sortname="FundClose" style="width: 18%">
						Fund Close
					</th>
					<th style="width: 15%">
					</th>
				</tr>
			</thead>
		</table>
	</div>
	<br />
</div>
<div class="line"> </div> 
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Capital Calls From Investments</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Capital Calls From Investments
				</div>
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			<div style="width: 90%; margin: 0 auto; clear: both;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="tblCC" class="grid" sortname="UnderlyingFundName" sortorder="asc" url="/Deal/GetUnderlyingFundCapitalCalls"
					templatename="CapitalCallListTemplate">
					<thead>
						<tr>
							<th style="text-align: left" sortname="UnderlyingFundName">
								Underlying FundName
							</th>
							<th style="text-align: left" sortname="FundName">
								Fund Name
							</th>
							<th style="text-align: right;" sortname="Amount">
								Amount
							</th>
							<th style="text-align: left; width: 100px;" sortname="DueDate">
								Due Date
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Distributions/Sales Of Investment</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Distributions/Sales Of Investment
				</div>
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			<div style="width: 90%; margin: 0 auto; clear: both;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="tblCD" class="grid" sortname="UnderlyingFundName" sortorder="asc" url="/Deal/GetUnderlyingFundCashDistributions"
					templatename="CashDistributionListTemplate">
					<thead>
						<tr>
							<th style="text-align: left" sortname="UnderlyingFundName">
								Underlying FundName
							</th>
							<th style="text-align: left" sortname="FundName">
								Fund Name
							</th>
							<th style="text-align: right;" sortname="Amount">
								Amount
							</th>
							<th style="text-align: left; width: 100px;" sortname="NoticeDate">
								Due Date
							</th>
							<th style="text-align: left; width: 100px;" sortname="CashDistributionType">
								Distribution Type
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Post Record Capital Calls From Investments</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Post Record Capital Calls From Investments
				</div>
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			<div style="width: 90%; margin: 0 auto; clear: both;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="tblPRCC" class="grid" sortname="UnderlyingFundName" sortorder="asc" url="/Deal/GetUnderlyingFundPostRecordCapitalCalls"
					templatename="PostRecordCapitalCallListTemplate">
					<thead>
						<tr>
							<th style="text-align: left" sortname="UnderlyingFundName">
								Underlying FundName
							</th>
							<th style="text-align: left" sortname="FundName">
								Fund Name
							</th>
							<th style="text-align: left" sortname="DealName">
								Deal Name
							</th>
							<th style="text-align: right;" sortname="Amount">
								Amount
							</th>
							<th style="text-align: left; width: 100px;" sortname="CapitalCallDate">
								Capital Call Date
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Post Record Distributions/Sales Of Investment</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Post Record Distributions/Sales Of Investment
				</div>
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			<div style="width: 90%; margin: 0 auto; clear: both;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="tblPRCD" class="grid" sortname="UnderlyingFundName" sortorder="asc" url="/Deal/GetUnderlyingFundPostRecordCashDistributions"
					templatename="PostRecordCashDistributionListTemplate">
					<thead>
						<tr>
							<th style="text-align: left" sortname="UnderlyingFundName">
								Underlying FundName
							</th>
							<th style="text-align: left" sortname="FundName">
								Fund Name
							</th>
							<th style="text-align: left" sortname="DealName">
								Deal Name
							</th>
							<th style="text-align: right;" sortname="Amount">
								Amount
							</th>
							<th style="text-align: left; width: 100px;" sortname="DistributionDate">
								Distribution Date
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Stock Distributions</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Stock Distributions
				</div>
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			<div style="width: 90%; margin: 0 auto; clear: both;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="tblSD" class="grid" sortname="UnderlyingFundName" sortorder="asc" url="/Deal/GetUnderlyingFundStockDistributions"
					templatename="StockDistributionListTemplate">
					<thead>
						<tr>
							<th style="text-align: left" sortname="UnderlyingFundName">
								Underlying FundName
							</th>
							<th style="text-align: left" sortname="FundName">
								Fund Name
							</th>
							<th style="text-align: right;" sortname="NumberOfShares">
								Number Of Shares
							</th>
							<th style="text-align: right" sortname="PurchasePrice">
								Purchase Price
							</th>
							<th style="text-align: left" sortname="NoticeDate">
								Notice Date
							</th>
							<th style="text-align: left" sortname="DistributionDate">
								Distribution Date
							</th>
							<th style="text-align: left" sortname="TaxCostBase">
								Tax Cost Basis Per Share
							</th>
							<th style="text-align: left" sortname="TaxCostDate">
								Tax Cost Date
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Unfunded Adjustments</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Unfunded Adjustments
				</div>
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			<div style="width: 90%; margin: 0 auto; clear: both;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="tblUA" class="grid" sortname="UnderlyingFundName" sortorder="asc" url="/Deal/GetUnderlyingFundAdjustments"
					templatename="UnderlyingFundAdjustmentTemplate">
					<thead>
						<tr>
							<th style="text-align: left" sortname="UnderlyingFundName">
								Underlying FundName
							</th>
							<th style="text-align: left" sortname="FundName">
								Fund Name
							</th>
							<th style="text-align: right" sortname="CommitmentAmount">
								Commitment Amount
							</th>
							<th style="text-align: right" sortname="UnfundedAmount">
								Unfunded Amount
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Underlying Fund Valuations</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Underlying Fund Valuations
				</div>
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			<div style="width: 90%; margin: 0 auto; clear: both;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="tblUFV" class="grid" sortname="UnderlyingFundName" sortorder="asc" url="/Deal/GetUnderlyingFundValuations"
					templatename="UnderlyingFundValuationTemplate">
					<thead>
						<tr>
							<th style="text-align: left" sortname="UnderlyingFundName">
								Underlying FundName
							</th>
							<th style="text-align: left" sortname="FundName">
								Fund Name
							</th>
							<th style="text-align: right" sortname="FundNAV">
								Reported NAV
							</th>
							<th style="text-align: left" sortname="FundNAVDate">
								Reporting Date
							</th>
							<th style="text-align: right">
								Calculated NAV
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Underlying Fund Valuation History</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Underlying Fund Valuation History
				</div>
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			<div style="width: 90%; margin: 0 auto; clear: both;">
				<% Html.RenderPartial("TBoxTop"); %>
				<table cellpadding="0" cellspacing="0" border="0" id="tblUFVH" class="grid" sortname="UnderlyingFundName" sortorder="asc" url="/Deal/GetUnderlyingFundValuationHistories"
					templatename="UnderlyingFundValuationHistoryTemplate">
					<thead>
						<tr>
							<th style="text-align: left" sortname="UnderlyingFundName">
								Underlying FundName
							</th>
							<th style="text-align: left" sortname="FundName">
								Fund Name
							</th>
							<th style="text-align: right" sortname="FundNAV">
								Fund NAV
							</th>
							<th style="text-align: left" sortname="FundNAVDate">
								Fund NAV Date
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
				<% Html.RenderPartial("TBoxBottom"); %>
			</div>
		</div>
	</div>
</div>

﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if Deals.length>1}}
<tr id="ManualUFCD_Deal_${Index}" class="mcc">
	<td class="ismanual"></td>
	<td class="childrowcell ismanual" colspan=6>
		<div class="gbox"><table cellpadding=0 cellspacing=0 border=0 style="width:60%" class="grid">
			<thead>
				<tr>
					<th style="width:5%">Deal Number</th>
					<th style="width:25%">Deal Name</th>
					<th style="width:19.7%;">Commitment Amount</th>
					<th>Call Amount</th>
				</tr>
			</thead>
			{{each(i,deal) Deals}}
				<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>Deal ${deal.DealNumber}</td>
					<td class="dealname">${deal.DealName}</td>
					<td style="text-align:right"><%: Html.Span("${deal.CommitmentAmount}", new { @class = "money" })%></td>
					<td style="text-align:right"><%: Html.TextBox("${deal.FundId}_${deal.DealId}_CallAmount", "", new { @class = "manualcamount" })%></td>
				</tr>
			{{/each}}
		</table></div>
	</td>
</tr>
{{/if}}
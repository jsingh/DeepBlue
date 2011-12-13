<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if Deals.length>0}}
<tr id="ManualDD_Deal_${Index}" class="mcc">
	<td class="ismanual" style="background-image:none"></td>
	<td class="childrowcell ismanual" colspan=5 style="background-image:none">
		<div style="width:70%"><table cellpadding=0 cellspacing=0 border=0  class="grid">
			<thead>
				<tr>
					<th style="width:20%">Deal Number</th>
					<th style="width:25%">Deal Name</th>
					<th class="ralign" style="width:25%">No Of Shares</th>
					<th class="ralign">Distribution Amount</th>
				</tr>
			</thead>
			{{each(i,deal) Deals}}
				<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>Deal ${deal.DealNumber}</td>
					<td class="dealname">${deal.DealName}</td>
					<td  class="ralign"><%: Html.Span("${deal.NoOfShares}")%></td>
					<td  class="ralign"><%: Html.TextBox("${deal.FundId}_${deal.DealId}_CallAmount", "", new { @class = "manualcamount" })%></td>
				</tr>
			{{/each}}
		</table></div>
	</td>
</tr>
{{/if}}
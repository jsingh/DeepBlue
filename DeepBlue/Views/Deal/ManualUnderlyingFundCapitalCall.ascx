<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if Deals.length>0}}
<tr id="ManualUFCC_Deal_${Index}" class="mcc">
	<td class="ismanual"></td>
	<td class="childrowcell ismanual" colspan=6 style="background-image:none">
		<div class="gbox" style="width:70%"><table cellpadding=0 cellspacing=0 border=0  class="grid">
			<thead>
				<tr>
					<th style="width:20%">Deal Number</th>
					<th style="width:25%">Deal Name</th>
					<th class="lalign" style="width:25%">Commitment Amount</th>
					<th class="lalign">Call Amount</th>
				</tr>
			</thead>
			{{each(i,deal) Deals}}
				<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>Deal ${deal.DealNumber}</td>
					<td class="dealname">${deal.DealName}</td>
					<td  class="lalign"><%: Html.Span("${deal.CommitmentAmount}", new { @class = "money" })%></td>
					<td  class="lalign"><%: Html.TextBox("${deal.FundId}_${deal.DealId}_CallAmount", "", new { @class = "manualcamount" })%></td>
				</tr>
			{{/each}}
		</table></div>
	</td>
</tr>
{{/if}}
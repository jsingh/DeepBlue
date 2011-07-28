<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if Deals.length>0}}
<tr id="ManualUFCC_Deal_${Index}" class="mcc">
	<td class="ismanual"></td>
	<td class="childrowcell ismanual" colspan=6 style="background-image:none">
		<div class="gbox" style="width:60%"><table cellpadding=0 cellspacing=0 border=0  class="grid">
			<thead>
				<tr>
					<th style="width:5%">Deal Number</th>
					<th style="width:25%">Deal Name</th>
					<th class="ralign" style="width:19.7%;">Commitment Amount</th>
					<th class="ralign">Call Amount</th>
				</tr>
			</thead>
			{{each(i,deal) Deals}}
				<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>Deal ${deal.DealNumber}</td>
					<td class="dealname">${deal.DealName}</td>
					<td  class="ralign"><%: Html.Span("${deal.CommitmentAmount}", new { @class = "money" })%></td>
					<td  class="ralign"><%: Html.TextBox("${deal.FundId}_${deal.DealId}_CallAmount", "", new { @class = "manualcamount" })%></td>
				</tr>
			{{/each}}
		</table></div>
	</td>
</tr>
{{/if}}
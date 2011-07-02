<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="ManualUFCD_Deal_${Index}" class="mcc">
	<td></td>
	<td class="childrowcell" colspan=6>
		<table cellpadding=0 cellspacing=0 border=0 style="width:60%" class="grid">
			<thead>
				<tr>
					<th>Deal Number</th>
					<th>Deal Name</th>
					<th>Commitment Amount</th>
					<th>Call Amount</th>
				</tr>
			</thead>
			{{each(i,deal) Deals}}
				<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>Deal ${deal.DealNumber}</td>
					<td>${deal.DealName}</td>
					<td style="text-align:right"><%: Html.Span("${deal.CommitmentAmount}", new { @class = "money" })%></td>
					<td style="text-align:right"><%: Html.TextBox("${deal.FundId}_${deal.DealId}_CallAmount", "")%></td>
				</tr>
			{{/each}}
		</table>
	</td>
</tr>
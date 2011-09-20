<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if Deals.length>0}}
<tr id="ManualUFSD_Deal_${Index}" class="mcc">
	<td class="ismanual" style="display:none;background-image:none"></td>
	<td class="childrowcell ismanual"  style="display:none;background-image:none" colspan=9 >
		<div style="width:70%">
			<table cellpadding=0 cellspacing=0 border=0 class="grid">
				<thead>
					<tr>
						<th style="width:20%">Deal Number</th>
						<th style="width:25%">Deal Name</th>
						<th class="lalign" style="width:25%">NumberOfShares</th>
						<th class="lalign" style="width:25%">FMV</th>
					</tr>
				</thead>
				<tbody>
				{{each(i,direct) Deals}}
					<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
						<td>Deal ${direct.DealNumber}</td>
						<td class="dealname">${direct.DealName}</td>
						<td class="lalign"><%: Html.TextBox("${direct.FundId}_${direct.DealId}_NumberOfShares", "")%></td>
						<td class="lalign"><%: Html.TextBox("${direct.FundId}_${direct.DealId}_FMV", "")%></td>
					</tr>
				{{/each}}
				</tbody>
			</table>
		</div>
	</td>
</tr>
{{/if}}
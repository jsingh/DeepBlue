<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="ManualUFSD_Deal_${Index}" class="mcc">
	<td class="ismanual" style="display:none"></td>
	<td class="childrowcell ismanual"  style="display:none" colspan=6>
		<div class="gbox"><table cellpadding=0 cellspacing=0 border=0 style="width:100%" class="grid">
			<thead>
				<tr>
					<th>Deal Number</th>
					<th>Deal Name</th>
					<th style="width:18%">NumberOfShares</th>
					<th style="width:18%">FMV</th>
				</tr>
			</thead>
			<tbody>
			{{each(i,direct) Directs}}
				<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>Deal ${direct.DealNumber}</td>
					<td class="dealname">${direct.DealName}</td>
					<td style="text-align:center"><%: Html.TextBox("${direct.FundId}_${direct.DealId}_NumberOfShares", "")%></td>
					<td style="text-align:center"><%: Html.TextBox("${direct.FundId}_${direct.DealId}_FMV", "")%></td>
				</tr>
			{{/each}}
			</tbody>
		</table></div>
	</td>
</tr>
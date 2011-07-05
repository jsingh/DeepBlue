<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="ManualUFSD_Deal_${Index}" class="mcc">
	<td class="ismanual" style="display:none"></td>
	<td class="childrowcell ismanual"  style="display:none" colspan=6>
		<table cellpadding=0 cellspacing=0 border=0 style="width:100%" class="grid">
			<thead>
				<tr>
					<th>Deal Number</th>
					<th>Deal Name</th>
					<th>Issuer Name</th>
					<th>NumberOfShares</th>
					<th>PurchasePrice</th>
					<th>FMV</th>
					<th>TaxCostBase</th>
					<th>TaxCostDate</th>
					<th>NoticeDate</th>
					<th>DistributionDate</th>
				</tr>
			</thead>
			<tbody>
			{{each(i,direct) Directs}}
				<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
					<td>Deal ${direct.DealNumber}</td>
					<td class="dealname">${direct.DealName}</td>
					<td>${direct.DirectName}</td>
					<td><%: Html.TextBox("NumberOfShares","${direct.NumberOfShares}")%> </td>
					<td><%: Html.TextBox("PurchasePrice", "${direct.PurchasePrice}")%> </td>
					<td><%: Html.TextBox("FMV", "${direct.FMV}")%> </td>
					<td><%: Html.TextBox("TaxCostBase", "${direct.TaxCostBase}")%> </td>
					<td><%: Html.TextBox("TaxCostDate", "${direct.TaxCostDate}", new { @class = "datefield" })%> </td>
					<td><%: Html.TextBox("NoticeDate", "${direct.NoticeDate}", new { @class = "datefield" })%> </td>
					<td><%: Html.TextBox("DistributionDate", "${direct.DistributionDate}", new { @class = "datefield" })%> </td>
				</tr>
			{{/each}}
			</tbody>
		</table>
	</td>
</tr>
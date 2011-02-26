<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<DeepBlue.Models.Investor.FundInformation>>" %>
<table cellpadding="0" cellspacing="2" border="0" style="width: 98%" class="grid-list">
	<tr class="grid-header">
		<th>
			Fund Name
		</th>
		<th>
			Committed Amount
		</th>
		<th>
			Unfunded Amount
		</th>
		<th>
			Investror Type
		</th>
	</tr>
	<% int rowIndex = 0;
	foreach (var item in Model) { %>
	<%if (rowIndex % 2 == 0) { %>
	<tr class="row">
		<%} else {%>
		<tr class="alter-row">
			<%}%>
			<td style="text-align: center; white-space: nowrap;">
				<%: item.FundName %>
			</td>
			<td style="text-align: right">
				$<%: item.TotalCommitment.ToString("0.00") %>
			</td>
			<td style="text-align: right">
				$<%: Convert.ToDecimal(item.UnfundedAmount).ToString("0.00") %>
			</td>
			<td>
			</td>
		</tr>
		<% rowIndex++;
	} %>
		<%if (Model.Count() == 0) {  %>
		<tr>
			<td colspan="4" style="text-align: center">
				No Records Found
			</td>
		</tr>
		<%} %>
</table>

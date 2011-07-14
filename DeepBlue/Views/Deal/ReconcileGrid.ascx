<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if Items.length>1}}
<div class="gridbox">
<div class="gbox">
<table cellpadding="0" cellspacing="0" border="0" class="grid">
<thead>
<tr>
	<th style="text-align:left">
		Counter Party
	</th>
	<th style="text-align:left">
		Fund Name
	</th>
	<th style="text-align:left;width:5%;">
		Type
	</th>
	<th style="text-align:right;width:15%;">
		Amount
	</th>
	<th style="text-align:center;width:10%;">
		Payment Date
	</th>
	<th style="text-align:center;width:12%;">
		Paid On
	</th>
	<th style="text-align:center;width:5%;">
		Reconcile
	</th>
	<th style="text-align:left;width:15%;">
		Cheque Number
	</th>
	<th><div style="width:65px">&nbsp;</div>
	</th>
</tr>
</thead>
<tbody>
{{each(i,item) Items}}
<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
	<td style="text-align:left">
		${CounterParty}
	</td>
	<td style="text-align:left">
		${FundName}
	</td>
	<td style="text-align:left">
		${Type}
	</td>
	<td style="text-align:right">
		${formatCurrency(Amount)}
	</td>
	<td style="text-align:center">
		${formatDate(PaymentDate)}
	</td>
	<td style="text-align:center">
		<%: Html.TextBox("PaidOn", "", new { @id="${ReconcileTypeId}_${i}_PaidOn", @class="datefield" })%>
	</td>
	<td style="text-align:center">
		<%: Html.CheckBox("IsReconciled")%>
	</td>
	<td style="text-align:left">
		<%: Html.TextBox("ChequeNumber")%>
	</td>
	<td style="text-align:right">
		<%: Html.Hidden("ReconcileTypeId","${ReconcileTypeId}")%>
		<%: Html.Hidden("Id","${id}")%>
		<%: Html.Image("save.png", new { @class="gbutton", @onclick = "javascript:dealReconcile.save(this);" })%>
	</td>
</tr>
{{/each}}
</tbody>
</table></div>
</div>
{{/if}}
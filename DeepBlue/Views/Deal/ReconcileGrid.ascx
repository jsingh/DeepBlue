<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if Items.length>0}}
<div class="gridbox">
<%using (Html.Form(new { @id="frm${ReconcileTypeId}", @onsubmit = "return false" })) {%>
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
</tr>
</thead>
<tbody id="ReconcileBdy">
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
		<%: Html.TextBox("${i}_PaidOn", "", new { @id = "${ReconcileTypeId}_${i}_PaidOn", @class = "datefield" })%>
	</td>
	<td style="text-align:center">
		<%: Html.CheckBox("${i}_IsReconciled", false, new { @onclick = "javascript:dealReconcile.checkReconcile(this,'${ReconcileTypeId}_${i}_PaidOn','${formatDate(PaymentDate)}');" })%>
	</td>
	<td style="text-align:left">
		<%: Html.TextBox("${i}_ChequeNumber")%>
		<%: Html.Hidden("${i}_ReconcileTypeId", "${ReconcileTypeId}")%>
		<%: Html.Hidden("${i}_Id", "${id}")%>
	</td>
</tr>
{{/each}}
</tbody>
</table>
</div>
<center>
<div style="margin:10px 0;">
<%: Html.Image("save.png", new { @style="cursor:pointer", @onclick = "javascript:dealReconcile.save('frm${ReconcileTypeId}','Spn${ReconcileTypeId}Loading',${ReconcileTypeId});" })%></span><span id="Spn${ReconcileTypeId}Loading"></span>
</div>
</center>
<%}%>
</div>
{{/if}}
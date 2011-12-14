<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if ReconcileTypeId<=5}}
		{{if Items.length>0}}
		<div class="gridbox">
		<%using (Html.Form(new { @id="frm${ReconcileTypeId}", @onsubmit = "return false" })) {%>
		<div>
			<% Html.RenderPartial("TBoxTop"); %>
				<table type="regrid" cellpadding="0" cellspacing="0" border="0" id="tbl${ReconcileTypeId}" class="grid">
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
			<th style="text-align:right;">
				Amount
			</th>
			<th style="text-align:left;width:100px;">
				Payment Date
			</th>                          
			<th style="text-align:left;width:100px;">
				Paid On
			</th>
			<th style="text-align:left;width:50px;">
				Reconcile
			</th>
			<th style="text-align:left;width:120px;">
				Cheque Number
			</th>
		</tr>
		</thead>
		<tbody id="ReconcileBdy">
		</tbody>
		</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		<center>
		<div style="margin:10px 0;">
		<%: Html.Image("Save_active.png", new { @style = "cursor:pointer", @onclick = "javascript:dealReconcile.save('frm${ReconcileTypeId}','Spn${ReconcileTypeId}Loading',${ReconcileTypeId});" })%></span><span id="Spn${ReconcileTypeId}Loading"></span>
		</div>
		</center>
		<%}%>
		</div>
		{{/if}}
{{else}}
	{{if Items.length>0}}
		<div class="grid-box">
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" border="0" class="grid" id="REFundExpenseList">
			<thead>
			<tr>
				<th style="text-align:left;">
					Fund Name
				</th>
				<th style="text-align:left;">
					Expense Type
				</th>
				<th style="text-align:right;">
					Amount
				</th>
				<th style="text-align:left;">
					Date
				</th>
			</tr>
			</thead>
			<tbody id="Tbody1">
			</tbody>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
	{{/if}}
{{/if}}
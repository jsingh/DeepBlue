<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if ActivityTypeId<=8}}
		{{if Items.length>0}}
		<div class="gridbox">
		<%using (Html.Form(new { @id="frm${ActivityTypeId}", @onsubmit = "return false" })) {%>
		<div>
			<% Html.RenderPartial("TBoxTop"); %>
				<table type="regrid" cellpadding="0" cellspacing="0" border="0" id="tbl${ActivityTypeId}" class="grid">
		<thead>
		<tr>
			<th style="text-align:left" sortname="CounterParty">
				Counter Party
			</th>
			<th style="text-align:left" sortname="FundName">
				Fund Name
			</th>
			<th style="text-align:left;width:5%;" sortname="Type">
				Type
			</th>
			<th style="text-align:right;" sortname="Amount">
				Amount
			</th>
			<th style="text-align:left;width:100px;" sortname="PaymentDate">
				Payment Date
			</th>                          
			<th style="text-align:left;width:100px;" sortname="PaidOn">
				Paid On
			</th>
			<th style="text-align:left;width:50px;">
				Activity
			</th>
			<th style="text-align:left;width:120px;" sortname="ChequeNumber">
				Cheque Number
			</th>
		</tr>
		</thead>
		<tbody id="ActivityBdy">
		</tbody>
		</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
		<center>
		<div style="margin:10px 0;">
		<%: Html.Image("Save_active.png", new { @style = "cursor:pointer", @onclick = "javascript:dealActivity.save('frm${ActivityTypeId}','Spn${ActivityTypeId}Loading',${ActivityTypeId});" })%></span><span id="Spn${ActivityTypeId}Loading"></span>
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
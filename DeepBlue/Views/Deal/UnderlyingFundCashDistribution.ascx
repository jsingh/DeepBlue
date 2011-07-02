﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCashDistributionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFCD_${Index}" {{if UnderlyingFundCashDistributionId>0==false}}class="newrow"{{/if}}>
	{{if IsManualCapitalCall==true}}
	<td style="text-align:center;">
		<%: Html.Image("treeminus.gif", new { @onclick = "javascript:dealActivity.expandMCDTree(${Index},this);" })%>
	</td>
	{{/if}}
	<td style="text-align: left">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class = "", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_NoticeDate", "{{if UnderlyingFundCashDistributionId>0}}${NoticeDate}{{/if}}", new { @class = "datefield", @id = "${Index}_CD_NoticeDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_ReceivedDate", "{{if UnderlyingFundCashDistributionId>0}}${ReceivedDate}{{/if}}", new { @class = "datefield", @id = "${Index}_CD_ReceivedDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.DropDownList("${Index}_CashDistributionTypeId", Model.CashDistributionTypes, new { @style = "width:160px", @val = "${CashDistributionTypeId}" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("${Index}_UnderlyingFundCashDistributionId", "${UnderlyingFundCashDistributionId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}")%>
	</td>
</tr>
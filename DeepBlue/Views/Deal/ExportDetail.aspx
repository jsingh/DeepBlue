<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.DealExportModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Export</title>
	<%=Html.JavascriptInclueTag("jquery-1.4.1.min.js")%>
	<style type="text/css">
		body {
			font-family: Calibri, Arial;
			font-size: 14px;
		}
		table {
			width: 100%;
		}
		tbody tr {
			background-color: #FFF;
		}
		th {
			white-space: nowrap;
		}
		td {
			padding: 8px 0px;
		}
		.erow {
			background-color: #fff;
		}
		.erow td {
			padding: 0px;
		}
		.arow {
			background-color: #fff;
		}
		#tblUnderlyingFund, #tblUnderlyingDirect {
			width: 95%;
			background-color: #000;
		}
		#tblUnderlyingFund tr th, #ReportList #tblUnderlyingFund tr td, #tblUnderlyingDirect tr th, #ReportList #tblUnderlyingDirect tr td {
			text-align: center;
		}
	</style>
</head>
<body>
	<table cellpadding="0" cellspacing="1" border="0" id="ReportList">
		<thead>
			<tr>
				<th style="width: 10%">
					Deal No.
				</th>
				<th>
					Deal Name
				</th>
				<th>
					Fund Name
				</th>
				<th>
					Seller Name
				</th>
			</tr>
		</thead>
		<tbody>
			<% int index = 0;
	  foreach (var item in Model.Deals) { %>
			<tr class="erow">
				<td colspan="4">
					&nbsp;
				</td>
			</tr>
			<% index++;
	  if ((index % 2) == 0) {%>
			<tr class="arow">
				<%}
	  else {%><tr>
		  <%}%>
		  <td style="text-align: center">
			  <%: item.DealNumber %>&nbsp;.
		  </td>
		  <td style="text-align: center">
			  <%: item.DealName %>
		  </td>
		  <td style="text-align: center">
			  <%: item.FundName %>
		  </td>
		  <td style="text-align: center">
			  <%: item.SellerName %>
		  </td>
	  </tr>
				<tr>
					<td colspan="2" style="padding-left: 20px; text-align: center; vertical-align: top;">
						<table cellspacing="1" cellpadding="0" border="0" id="tblUnderlyingFund">
							<thead>
								<tr>
									<th>
										No.
									</th>
									<th>
										Fund Name
									</th>
									<th>
										Fund NAV
									</th>
									<th>
										Commitment
									</th>
									<th>
										Record Date
									</th>
								</tr>
							</thead>
							<%if (item.DealUnderlyingFunds.Count > 0) {%>
							<tbody>
								<%int fundIndex = 0;
		  foreach (var fund in item.DealUnderlyingFunds) {
			  fundIndex++; %>
								<tr>
									<td>
										<%: fundIndex%>&nbsp;.
									</td>
									<td>
										<%: fund.FundName%>
									</td>
									<td>
										<%: fund.FundNAV%>
									</td>
									<td class="dollarcell">
										<%: FormatHelper.CurrencyFormat(fund.CommittedAmount)%>
									</td>
									<td class="datecell">
										<%: (fund.RecordDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")%>
									</td>
								</tr>
								<%}%>
							</tbody>
							<%}%>
						</table>
						<br />
						Underlying Funds
					</td>
					<td colspan="2" style="padding-left: 20px; text-align: center; vertical-align: top;">
						<table cellspacing="1" cellpadding="0" border="0" id="tblUnderlyingDirect">
							<thead>
								<tr>
									<th>
										No.
									</th>
									<th>
										Company
									</th>
									<th>
										Security
									</th>
									<th>
										No.of Shares
									</th>
									<th>
										Percentage
									</th>
									<th>
										FMV
									</th>
									<th>
										Record Date
									</th>
								</tr>
							</thead>
							<%if (item.DealUnderlyingDirects.Count > 0) {%>
							<tbody>
								<%int directIndex = 0;
		  foreach (var direct in item.DealUnderlyingDirects) {
			  directIndex++; %>
								<tr>
									<td>
										<%: directIndex%>&nbsp;.
									</td>
									<td>
										<%: direct.IssuerId%>
									</td>
									<td>
										<%: direct.Security%>
									</td>
									<td>
										<%: direct.NumberOfShares%>
									</td>
									<td>
										<%: direct.Percent%>
									</td>
									<td>
										<%: direct.FMV%>
									</td>
									<td class="datecell">
										<%: (direct.RecordDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") %>
									</td>
								</tr>
								<%}%>
							</tbody>
							<%}%>
						</table>
						<br />
						Underlying Directs
					</td>
				</tr>
				<% } %>
		</tbody>
	</table>
	<%: Html.HiddenFor(model => model.IsPrint)%>
</body>
<script type="text/javascript">
	$(document).ready(function () {
		var isprint=$("#IsPrint").val();
		if(isprint.toLowerCase()=="true") {
			window.print();
		}
		window.close();
	});
</script>
</html>

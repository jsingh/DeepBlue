<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.DealExportModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Export</title>
	<style type="text/css">
		body {
			font-family: Calibri;
			font-size: 14px;
			color: #A6A6A6;
		}
		table {
			width: 100%;
		}
		tbody tr {
			background-color: #F2F2F2;
		}
		th {  white-space:nowrap; }
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
			background-color: #D9D9D9;
		}
		#tblUnderlyingFund, #tblUnderlyingDirect {
			background-color: #000000;
			color: #000000;
			width: 95%;
		}
		#tblUnderlyingFund tr th, #ReportList #tblUnderlyingFund tr td, #tblUnderlyingDirect tr th, #ReportList #tblUnderlyingDirect tr td {
			background-color: #FFFFFF;
			color: #000000; text-align:center;
		}
	</style>
</head>
<body>
	<table cellpadding="0" cellspacing="0" border="0" id="ReportList">
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
			<tr class="erow" colspan="4">
				<td>
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
					<td colspan="2" style="padding-left: 20px; text-align: center;vertical-align:top;">
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
										<%: fund.NAV%>
									</td>
									<td class="dollarcell">
										<%: string.Format("{0:C}",fund.Commitment)%>
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
					<td colspan="2" style="paddin-left: 20px; text-align: center;vertical-align:top;">
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
										<%: direct.Company%>
									</td>
									<td>
										<%: direct.Security%>
									</td>
									<td>
										<%: direct.NoOfShares%>
									</td>
									<td>
										<%: direct.Percentage%>
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
</body>
</html>

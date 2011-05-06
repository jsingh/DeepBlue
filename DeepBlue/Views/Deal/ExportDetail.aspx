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
		}
		#tblUnderlyingFund tr th, #ReportList #tblUnderlyingFund tr td, #tblUnderlyingDirect tr th, #ReportList #tblUnderlyingDirect tr td {
			text-align: center;
			border: solid 1px #000;
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
					<td colspan="2" style="padding-left: 20px; text-align: center; vertical-align: top;
						border-bottom: solid 1px #000;">
						<table cellspacing="0" cellpadding="0" border="0" id="tblUnderlyingFund">
							<thead>
								<tr>
									<th style="border-right: 0px">
										No.
									</th>
									<th style="border-right: 0px">
										Fund Name
									</th>
									<th style="border-right: 0px">
										Fund NAV
									</th>
									<th style="border-right: 0px">
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
									<td style="border-right: 0px">
										<%: fundIndex%>&nbsp;.
									</td>
									<td style="border-right: 0px">
										<%: fund.FundName%>
									</td>
									<td style="border-right: 0px">
										<%: fund.NAV%>
									</td>
									<td class="dollarcell" style="border-right: 0px">
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
					<td colspan="2" style="paddin-left: 20px; text-align: center; vertical-align: top;
						border-bottom: solid 1px #000;">
						<table cellspacing="0" cellpadding="0" border="0" id="tblUnderlyingDirect">
							<thead>
								<tr>
									<th style="border-right: 0px">
										No.
									</th>
									<th style="border-right: 0px">
										Company
									</th>
									<th style="border-right: 0px">
										Security
									</th>
									<th style="border-right: 0px">
										No.of Shares
									</th>
									<th style="border-right: 0px">
										Percentage
									</th>
									<th style="border-right: 0px">
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
									<td style="border-right: 0px">
										<%: directIndex%>&nbsp;.
									</td>
									<td style="border-right: 0px">
										<%: direct.Company%>
									</td>
									<td style="border-right: 0px">
										<%: direct.Security%>
									</td>
									<td style="border-right: 0px">
										<%: direct.NoOfShares%>
									</td>
									<td style="border-right: 0px">
										<%: direct.Percentage%>
									</td>
									<td style="border-right: 0px">
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

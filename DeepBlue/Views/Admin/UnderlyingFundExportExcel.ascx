<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Admin.UnderlyingFundExportExcelModel>" %>
<%
	GridView grd;
	System.IO.StringWriter swr;
	HtmlTextWriter tw;
%>
<table cellpadding="0" cellspacing="0" border="1" style="width: 100%">
	<tbody>
		<tr>
			<td>
				<b>Underlying Funds</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.UnderlyingFunds;
					grd.DataBind();
					swr = new System.IO.StringWriter();
					tw = new HtmlTextWriter(swr);
					grd.RenderControl(tw);
					Response.Write(swr.ToString());
				%>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<b>Underlying Fund Contacts</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.UnderlyingFundContacts;
					grd.DataBind();
					swr = new System.IO.StringWriter();
					tw = new HtmlTextWriter(swr);
					grd.RenderControl(tw);
					Response.Write(swr.ToString());
				%>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<b>Underlying Fund Capital Calls</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.UnderlyingFundCapitalCalls;
					grd.DataBind();
					swr = new System.IO.StringWriter();
					tw = new HtmlTextWriter(swr);
					grd.RenderControl(tw);
					Response.Write(swr.ToString());
				%>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<b>Underlying Fund Capital Call Line Items</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.UnderlyingFundCapitalCallLineItems;
					grd.DataBind();
					swr = new System.IO.StringWriter();
					tw = new HtmlTextWriter(swr);
					grd.RenderControl(tw);
					Response.Write(swr.ToString());
				%>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<b>Underlying Fund Cash Distributions</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.UnderlyingFundCashDistributions;
					grd.DataBind();
					swr = new System.IO.StringWriter();
					tw = new HtmlTextWriter(swr);
					grd.RenderControl(tw);
					Response.Write(swr.ToString());
				%>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<b>Underlying Fund Cash Distribution Line Items</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.UnderlyingFundCashDistributionLineItems;
					grd.DataBind();
					swr = new System.IO.StringWriter();
					tw = new HtmlTextWriter(swr);
					grd.RenderControl(tw);
					Response.Write(swr.ToString());
				%>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<b>Underlying Fund Stock Distributions</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.UnderlyingFundStockDistributions;
					grd.DataBind();
					swr = new System.IO.StringWriter();
					tw = new HtmlTextWriter(swr);
					grd.RenderControl(tw);
					Response.Write(swr.ToString());
				%>
			</td>
		</tr>
		<tr>
			<td>
			</td>
		</tr>
		<tr>
			<td>
				<b>Underlying Fund Stock Distribution LineItems</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.UnderlyingFundStockDistributionLineItems;
					grd.DataBind();
					swr = new System.IO.StringWriter();
					tw = new HtmlTextWriter(swr);
					grd.RenderControl(tw);
					Response.Write(swr.ToString());
				%>
			</td>
		</tr>
	</tbody>
</table>
<%
	Response.Clear();
	Response.AddHeader("content-disposition", "attachment;filename=UnderlyingFunds.xls");
	Response.ContentType = "application/excel";
	swr = new System.IO.StringWriter();
	tw = new HtmlTextWriter(swr);
	Response.Write(swr.ToString());
	Response.End();
%>
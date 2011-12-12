<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Admin.FundExportExcelModel>" %>
<%
	GridView grd;
	System.IO.StringWriter swr;
	HtmlTextWriter tw;
%>
<table cellpadding="0" cellspacing="0" border="1" style="width: 100%">
	<tbody>
		<tr>
			<td>
				<b>Amberbrook Funds</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.AmberbrookFunds;
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
				<b>Investor Informations</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.Investors;
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
				<b>Amberbrook Fund Rate Schdules</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.RateSchdules;
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
				<b>Investor Bank Informations</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.BankInformations;
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
	Response.AddHeader("content-disposition", "attachment;filename=AmberbrookFunds.xls");
	Response.ContentType = "application/excel";
	swr = new System.IO.StringWriter();
	tw = new HtmlTextWriter(swr);
	Response.Write(swr.ToString());
	Response.End();
%>
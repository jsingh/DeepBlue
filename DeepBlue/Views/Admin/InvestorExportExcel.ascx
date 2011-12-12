<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Admin.InvestorExportExcelModel>" %>
<%
	GridView grd;
	System.IO.StringWriter swr;
	HtmlTextWriter tw;
%>
<table cellpadding="0" cellspacing="0" border="1" style="width: 100%">
	<tbody>
		<tr>
			<td>
				<b>Investors</b>
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
				<b>Investor Investment Informations</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.InvestorInvestments;
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
				<b>Investor Address Informations</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.InvestorAddresses;
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
					grd.DataSource = Model.InvestorBanks;
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
				<b>Investor Contact Informations</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.InvestorContacts;
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
	Response.AddHeader("content-disposition", "attachment;filename=Investor.xls");
	Response.ContentType = "application/excel";
	swr = new System.IO.StringWriter();
	tw = new HtmlTextWriter(swr);
	Response.Write(swr.ToString());
	Response.End();
%>
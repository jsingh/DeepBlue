<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Admin.UnderlyingDirectExportExcelModel>" %>
<%
	GridView grd;
	System.IO.StringWriter swr;
	HtmlTextWriter tw;
%>
<table cellpadding="0" cellspacing="0" border="1" style="width: 100%">
	<tbody>
		<tr>
			<td>
				<b>Directs</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.Directs;
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
				<b>Equities</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.Equities;
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
				<b>FixedIncomes</b>
			</td>
		</tr>
		<tr>
			<td style="width: 100%">
				<%
					grd = new GridView();
					grd.DataSource = Model.FixedIncomes;
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
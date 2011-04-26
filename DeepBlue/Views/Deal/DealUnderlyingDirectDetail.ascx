<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.CreateModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line">
</div>
<div>
	<%: Html.Image("UnderlyingDirects.png", new { @class="expandbtn" })%></div>
<div class="fieldbox">
	<table cellpadding="0" cellspacing="0" border="0" class="grid" style="width: 80%">
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
					No. of Shares
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
				<th>
				</th>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td style="text-align: center">
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
				</td>
				<td>
					<%: Html.Image("Editbtn.png")%><%: Html.Image("Delete_Btn.png")%>
				</td>
			</tr>
		</tbody>
	</table>
</div>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Document.DocumentDetail>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add((item.DocumentDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"));
	   row.cell.Add(item.FileName);
	   row.cell.Add(item.InvestorName);
	   row.cell.Add(item.FundName);
	   string imgname = string.Empty;
	   switch (item.FileTypeName.ToLower()) {
		   case "pdf":
			   imgname = "pdf.png";
			   break;
		   case "word":
			   imgname = "word.png";
			   break;
		   case "excel":
			   imgname = "xls.png";
			   break;
	   }
	   row.cell.Add(Html.Image(imgname, new { @onclick = "javascript:documentSearch.downloadFile('" + Url.Encode(item.FilePath.Replace("\\","/")) + "','" + Url.Encode(item.FileName) + "');" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>

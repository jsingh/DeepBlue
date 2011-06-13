<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DeepBlue.Models.Document.DocumentDetail>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add((item.DocumentDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"));
	   row.cell.Add(item.FileName);
	   row.cell.Add(item.DocumentType);
	   row.cell.Add(item.InvestorName);
	   row.cell.Add(item.FundName);
	   string imgname = string.Empty;
	   switch (item.FileTypeName.ToLower()) {
		   case "pdf":
			   imgname = "pdf.png";
			   break;
		   case "word":
			   imgname = "doc.png";
			   break;
		   case "excel":
			   imgname = "xls.png";
			   break;
		   default:
			   imgname = "attach.gif";
			   break;
	   }
	   string href = string.Empty;
	   if(item.FilePath.ToLower().StartsWith("http://")){
			href = (item.FilePath.EndsWith("/") == false ? item.FilePath + "/" + item.FilePath : item.FilePath + item.FileName);
	   }else{
			href = "/" + item.FilePath.Replace("\\","/") + "/" + item.FileName;
	   }
	   row.cell.Add(Html.Anchor(Html.Image(imgname).ToHtmlString(),href,new { @target = "_blank" }).ToHtmlString()); // new { @onclick = "javascript:documentSearch.downloadFile('" + Url.Encode(item.FilePath.Replace("\\","/")) + "','" + Url.Encode(item.FileName) + "');" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>

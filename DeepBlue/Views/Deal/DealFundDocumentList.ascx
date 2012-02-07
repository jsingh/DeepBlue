<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DeepBlue.Models.Deal.DealFundDocumentList>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   //row.cell.Add(item.DealFundDocumentId);
	   row.cell.Add(item.DocumentType);
	   row.cell.Add((item.DocumentDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"));
	   row.cell.Add(item.FundName);
	   row.cell.Add(item.FileName);
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
	   string fileName = string.Empty;
	   if (item.FilePath.ToLower().StartsWith("http://")) {
		   href = "javascript:window.open('" + (item.FilePath.EndsWith("/") == false ? item.FilePath + "/" + item.FilePath : item.FilePath + item.FileName) + "');";
	   }
	   else {
		   fileName = item.FilePath.Replace("\\", "/") + "/" + item.FileName;
		   if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/"), fileName))) {
			   href = "/" + fileName;
		   }
		   else {
			   href = "javascript:void(0);";
		   }
	   }
	   row.cell.Add(Html.Anchor(Html.Image(imgname).ToHtmlString(), href, new { @class = "gbutton", @target = "_blank" }).ToHtmlString() + "&nbsp;&nbsp;" +
		   Html.Image("largedel.png", new { @style = "cursor:pointer;", @class = "gbutton", @onclick = "javascript:deal.deleteDealDocument(" + item.DealFundDocumentId + ",this);" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>

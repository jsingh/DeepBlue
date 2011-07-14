﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DeepBlue.Models.Deal.UnderlyingFundDocumentList>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.UnderlyingFundDocumentId);
	   row.cell.Add(item.DocumentType);
	   row.cell.Add((item.DocumentDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"));
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
		   if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("/"), fileName))) {
			   href = "/" + fileName;
		   }
		   else {
			   href = "javascript:void(0);";
		   }
	   }
	   row.cell.Add(Html.Anchor(Html.Image(imgname).ToHtmlString(), href, new { @target = "_blank" }).ToHtmlString());
	   row.cell.Add(Html.Image("largedel.png", new { @style = "cursor:pointer;", @onclick = "javascript:underlyingFund.deleteDocument(" + item.UnderlyingFundDocumentId + ",this);" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
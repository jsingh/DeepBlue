<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Admin.ResultModel>" %>
<%=DeepBlue.Helpers.JsonSerializer.ToJsonObject(Model)%>
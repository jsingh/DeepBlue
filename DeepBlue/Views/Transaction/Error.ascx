<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Transaction.ErrorModel>" %>
<% foreach (var item in Model.ErrorInfo) { %>
<%=item.PropertyName + " : " + item.ErrorMessage%><br />
<% } %>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<footer class="footer"><p>Copyright <%=DateTime.Now.Year.ToString()%>&nbsp;<%=EntityHelper.EntityName%></p>
</footer>

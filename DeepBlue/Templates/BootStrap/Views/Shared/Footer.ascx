<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<footer class="footer"><p>Copyright 2011 <%=EntityHelper.EntityName%>&nbsp;<%=ConfigurationManager.AppSettings["CurrentVersion"]%></p>
</footer>

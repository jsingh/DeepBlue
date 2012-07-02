<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="footer">
		<div class="footercontent">
			Copyright <%=DateTime.Now.Year.ToString()%>&nbsp;<%=EntityHelper.EntityName%></div>
	</div>

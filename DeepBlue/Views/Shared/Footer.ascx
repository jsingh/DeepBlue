<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div id="footer">
		<div class="footercontent">
			Copyright 2011 <%=EntityHelper.EntityName%>&nbsp;<%=ConfigurationManager.AppSettings["CurrentVersion"]%></div>
	</div>

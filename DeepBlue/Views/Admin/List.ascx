<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Admin.ListModel>" %>
<% switch (Model.ControllerType) {%>
<%case DeepBlue.Models.Admin.Enums.ControllerType.EntityType:%>
<% Html.RenderPartial("EntityType",Model.EntityTypes); %>
<%break;%>
<%case DeepBlue.Models.Admin.Enums.ControllerType.FOIA:%>
<%break;%>
<%case DeepBlue.Models.Admin.Enums.ControllerType.ERISA:%>
<%break;%>
<%}%>

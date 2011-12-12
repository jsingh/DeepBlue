<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% Html.RenderPartial("UnderlyingFundCapitalCall", new DeepBlue.Models.Deal.UnderlyingFundCapitalCallModel()); %>
<% Html.RenderPartial("ManualUnderlyingFundCapitalCall"); %>

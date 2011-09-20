<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.IssuerDetailModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%: Html.Hidden("IssuerId", "${IssuerId}")%>
<%: Html.Hidden("CountryId", "${CountryId}")%>
<%: Html.Hidden("IsUnderlyingFundModel", "${IsUnderlyingFundModel}")%>
<div id="DetailBox" style="float:left;clear:both;">
	<div class="editor-label" style="{{if IsUnderlyingFundModel==true}}width:157px{{else}}width:128px;padding-right:26px;{{/if}}" >
		{{if IsUnderlyingFundModel==true}}GP{{else}}Company{{/if}}
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Name", "${Name}", new { @class = "wm", @onkeyup = "javascript:dealDirect.copyName(this);" })%>
	</div>
	<div class="editor-label" style="clear: right;{{if IsUnderlyingFundModel==true}}{{/if}}">
		{{if IsUnderlyingFundModel==true}}GP Parent{{else}}Company Parent{{/if}}
	</div>
	<div class="editor-field">
		<%: Html.TextBox("ParentName", "${ParentName}", new { @class = "wm" })%>
	</div>
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.CountryId)%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Country", "${Country}", new { @id="I_Country", @class = "wm" })%>
	</div>
</div>

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
		<%: Html.TextBox("Name", "${Name}", new { @class = "wm"})%>
	</div>
	{{if IsUnderlyingFundModel==true}}
	<div class="editor-label" style="clear: right;">
		Annual Meeting
	</div>
	<div class="editor-field">
		<%: Html.TextBox("AnnualMeetingDate", "${AnnualMeetingDate}", new { @class = "datefield", @style="width:102px" })%>&nbsp;<%: Html.Anchor("See Full List", "javascript:dealDirect.openAMD();") %>
	</div>
	{{/if}}
	<div class="editor-label" style="clear: right">
		<%: Html.LabelFor(model => model.CountryId)%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Country", "${Country}", new { @id="I_Country", @class = "wm" })%>
	</div>
</div>

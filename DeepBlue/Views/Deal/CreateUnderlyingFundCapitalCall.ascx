<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%using (Html.Form(new { @id = "AddNewCapitalCall", @onsubmit = "return dealActivity.onCCSubmit(this);" })) {%>
<div class="editor-label">
	<%: Html.LabelFor(model => model.FundId) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("FundName","${FundName}") %>
</div>
<div class="editor-label rightcol">
	<%: Html.LabelFor(model => model.UnderlyingFundId) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("UnderlyingFundName","${UnderlyingFundName}") %>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.Amount) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("Amount", "${Amount}", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
</div>
<div class="editor-label rightcol">
	<%: Html.LabelFor(model => model.NoticeDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("NoticeDate", "${NoticeDate}", new { @class = "datefield", @id = "${UnderlyingFundCapitalCallId}_CC_NoticeDate" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.ReceivedDate) %>
</div>
<div class="editor-field">
	<%: Html.TextBox("ReceivedDate", "${ReceivedDate}", new { @class = "datefield", @id = "${UnderlyingFundCapitalCallId}_CC_ReceivedDate" })%>
</div>
<div class="editor-label rightcol">
	<%: Html.LabelFor(model => model.IsPostRecordDateTransaction) %>
</div>
<div class="editor-field checkbox ">
	<%: Html.CheckBox("IsPostRecordDateTransaction", false, new { @val = "${IsPostRecordDateTransaction}" })%>
</div>
<div class="editor-label">
	<%: Html.LabelFor(model => model.IsDeemedCapitalCall) %>
</div>
<div class="editor-field checkbox">
	<%: Html.CheckBox("IsDeemedCapitalCall", false, new { @val = "${IsDeemedCapitalCall}" })%>
</div>
<div id="UpdateTargetId" style="display: none">
</div>
<%: Html.Hidden("UnderlyingFundCapitalCallId","${UnderlyingFundCapitalCallId}")%>
<%: Html.Hidden("FundId", "${FundId}")%>
<%: Html.Hidden("UnderlyingFundId","${UnderlyingFundId}")%>
<div class="status">
	<%: Html.Span("", new { id = "UpdateLoading" })%></div>
<div class="editor-button" style="width: 200px; margin: 0px 0px 0px 30%;">
	<div style="float: left; padding: 0 0 10px 5px;">
		{{if UnderlyingFundCapitalCallId>0}}
		<%: Html.ImageButton("save.png", new { @class = "default-button" })%>
		{{else}}
		<%: Html.ImageButton("add_btn.png", new { @class = "default-button"  })%>
		{{/if}}
	</div>
</div>
<% } %>

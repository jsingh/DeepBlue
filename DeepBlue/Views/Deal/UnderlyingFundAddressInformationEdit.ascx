<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="info-detail">
	<div class="editor-label">
		<%: Html.Label("Address1") %>
	</div>
	<div class="editor-field address">
		<%: Html.TextBox("Address1", "${Address1}", new { @class="hide", @style = "width:100%;" })%>
		<%: Html.Span("${Address1}", new { @class="show" })%>
	</div>
	<div class="editor-label" style="clear:right;">
		<%: Html.Label("Address2") %>
	</div>
	<div class="editor-field address">
		<%: Html.TextBox("Address2", "${Address2}", new { @class="hide", @style = "width:100%" })%>
		<%: Html.Span("${Address1}", new { @class="show" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("City") %>
	</div>
	<div class="editor-field" style="clear:right;">
		<%: Html.TextBox("City", "${City}", new { @class = "hide" })%>
		<%: Html.Span("${City}", new { @class = "show" })%>
	</div>
	<div class="editor-row" id="AddressStateRow" style=clear:right;width:auto;{{if Country!=225}}display:none;{{/if}}>
		<div class="editor-label">
			<%: Html.Label("State") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("StateName", "${StateName}", new { @class="hide", @id = "StateName" })%>
			<%: Html.Hidden("State","${State}")%>
			<%: Html.Span("${StateName}", new { @class = "show" })%>
		</div>
	</div>
	<div class="editor-label" style="clear:right;">
		<%: Html.Label("Zip") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Zip", "${Zip}", new { @class = "hide" })%>
		<%: Html.Span("${StateName}", new { @class = "show" })%>
	</div>
	<div class="editor-label" style="clear:right;">
		<%: Html.Label("Country")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("CountryName", "${CountryName}", new { @class="hide", @id = "CountryName"})%>
		<%: Html.Hidden("Country", "${Country}")%>
		<%: Html.Span("${CountryName}", new { @class = "show" })%>
	</div>
	<div>
		{{if UnderlyingFundId>0}}
		<div class="editor-label" style="float: right; width: auto;">
			<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:underlyingFund.cancelEdit(this,'.info-detail');" })%>
		</div>
		<div class="editor-label show" style="float: right; width: auto;">
			<%: Html.Image("Editbtn_active.png", new { @onclick = "javascript:underlyingFund.edit(this,'.info-detail');" })%>
		</div>
		<div class="editor-label hide" style="float: right; width: auto;">
			<%: Html.Image("Save_active.png", new { @onclick = "javascript:underlyingFund.saveAddress(this,${UnderlyingFundId})" })%>
		</div>
		<div class="editor-label" style="float: right; width: auto;">
			<%: Html.Span("", new { @id = "AILoading" })%>
		</div>
		{{/if}}
	</div>
</div>
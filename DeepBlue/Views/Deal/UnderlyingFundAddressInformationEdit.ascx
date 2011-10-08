<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="info-detail">
	<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
	<div class="editor-label">
		<%: Html.Label("Address1") %>
	</div>
	<div class="editor-field" style="width:442px">
		<%: Html.TextBox("Address1", "${Address1}", new { @style = "width:430px" })%>
	</div>
	<div class="editor-label" style="clear:right">
		<%: Html.Label("Address2") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Address2", "${Address2}", new { @style = "width:430px" })%>
	</div>
	<div class="editor-label">
		<%: Html.Label("City") %>
	</div>
	<div class="editor-field" style="clear:right">
		<%: Html.TextBox("City", "${City}")%>
	</div>
	<div class="editor-row" id="AddressStateRow" style=clear:right;width:auto;{{if Country!=225}}display:none;{{/if}}>
		<div class="editor-label">
			<%: Html.Label("State") %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("StateName", "${StateName}", new { @id = "StateName" })%>
			<%: Html.Hidden("State","${State}")%>
		</div>
	</div>
	<div class="editor-label" style="clear:right">
		<%: Html.Label("Zip") %>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("Zip", "${Zip}")%>
	</div>
	<div class="editor-label" style="clear:right">
		<%: Html.Label("Country")%>
	</div>
	<div class="editor-field">
		<%: Html.TextBox("CountryName", "${CountryName}", new { @id = "CountryName"})%>
		<%: Html.Hidden("Country", "${Country}")%>
	</div>
	<div class="savebox">
		<div class="resetbtn">
			<%: Html.Span("Reset", new { @onclick = "javascript:underlyingFund.reset('AddressInformation');" })%></div>
		<div class="btn">
			<%: Html.Image("Save_active.png", new { @onclick = "javascript:underlyingFund.saveAddress(this,${UnderlyingFundId})" })%></div>
		<div class="btn" id="AILoading">
		</div>
	</div>
</div>
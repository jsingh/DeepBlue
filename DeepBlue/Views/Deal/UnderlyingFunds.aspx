<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateUnderlyingFundModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Underlying Fund Library
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("jAjaxTable.js")%>
	<%=Html.StylesheetLinkTag("deal.css") %>
	<%=Html.StylesheetLinkTag("dealdirect.css")%>
	<%=Html.StylesheetLinkTag("addufund.css")%>
	<%=Html.JavascriptInclueTag("UnderlyingFund.js")%>
	<%=Html.JavascriptInclueTag("DealDirect.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<div class="title">
					INVESTMENTS</div>
				<div class="arrow">
				</div>
				<div class="pname">
					UNDERLYING FUND LIBRARY
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none" })%></div>
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="leftcol">
			Underlying Fund
		</div>
		<div class="addbtn" style="display: block">
			<%: Html.Anchor(Html.Image("addnewissuer.png").ToHtmlString(), "javascript:dealDirect.add();")%>
		</div>
		<div class="addbtn" style="display: block">
			<%: Html.Anchor(Html.Image("addnufund.png").ToHtmlString(), "javascript:underlyingFund.load(0,0);")%>
		</div>
		<div style="float: right; padding-right: 50px;">
			<%: Html.TextBox("S_UnderlyingFund", "SEARCH UNDERLYING FUND", new { @id = "S_UnderlyingFund", @style = "width:200px", @class = "wm" })%>
		</div>
	</div>
	<div id="UnderlyingFundDetail">
		<div class="subheader" id="AddNewIssuer" style="display: none">
			<%using (Html.Form(new { @id = "frmAddNewIssuer", @onsubmit = "return dealDirect.createNewIssuer(this);" })) {%>
			<div id="NewIssuerDetail">
			</div>
			<div class="addissuer">
				<div class="btn">
					<%: Html.Span("", new { @id = "SpnNewLoading" })%></div>
				<div class="btn">
					<%: Html.ImageButton("addissuer.png")%></div>
				<div class="btnclose">
					<%: Html.Image("issuerclose.png", new { @onclick = "javascript:dealDirect.close();" })%>
				</div>
			</div>
			<%}%>
		</div>
		<%using (Html.Form(new { @id = "frmUnderlyingFund", @onsubmit = "return underlyingFund.save(this);" })) {%>
		<div id="AddUnderlyingFund" style="display: none">
		</div>
		<%}%>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		underlyingFund.init();</script>
	<%= Html.jQueryAutoComplete("S_UnderlyingFund", new AutoCompleteOptions {
																		  Source = "/Deal/FindUnderlyingFunds", MinLength = 1,
																		  OnSelect = "function(event, ui) { underlyingFund.load(ui.item.id,0);}"
	})%>
	<%= Html.jQueryAutoComplete("Issuer", new AutoCompleteOptions {
																	  Source = "/Issuer/FindIssuers",	MinLength = 1,
																	  OnSelect = "function(event, ui) { underlyingFund.selectIssuer(ui.item.id);}"
	})%>
	<script id="IssuerDetailTemplate" type="text/x-jquery-tmpl"> 
		<%Html.RenderPartial("IssuerDetail", new DeepBlue.Models.Deal.IssuerDetailModel());%>
	</script>
	<script id="UnderlyingFundTemplate" type="text/x-jquery-tmpl">
		<div class="content">
			<%: Html.Hidden("IssuerId", "${IssuerId}")%>
			<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
			<div class="editor-label">
				<label>
					Issuer Name</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Issuer", "${IssuerName}", new { @id = "Issuer", @style="width:157px", @onblur = "javascript:underlyingFund.checkIssuer(this);" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Legal Fund Name</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("FundName", "${FundName}", new { @class = "wm" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Fund Type</label>
			</div>
			<div class="editor-field">
				<%: Html.DropDownList("FundTypeId", Model.UnderlyingFundTypes, new { @val = "${FundTypeId}" })%>
			</div>
			<div class="editor-label">
				<label>
					Vintage Year</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("VintageYear", "${VintageYear}", new { @onkeypress = "return jHelper.isNumeric(event);" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Fund Size</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("TotalSize", "${TotalSize}", new { @onkeypress = "return jHelper.isNumeric(event);" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Termination Year</label>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("TerminationYear", "${TerminationYear}", new { @class = "wm", @onkeypress = "return jHelper.isNumeric(event);" })%>
			</div>
			<div class="editor-label">
				<label>
					Reporting</label>
			</div>
			<div class="editor-field">
				<%: Html.DropDownList("ReportingFrequencyId", Model.Reportings, new { @val = "${ReportingFrequencyId}" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Reporting Type</label>
			</div>
			<div class="editor-field">
				<%: Html.DropDownList("ReportingTypeId", Model.ReportingTypes, new { @val = "${ReportingTypeId}" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Fees Included</label>
			</div>
			<div class="editor-field" style="width: auto;padding: 3px 0 0;">
				<%: Html.CheckBox("IsFeesIncluded", false, new { @val = "${IsFeesIncluded}" })%>
			</div>
			<div class="editor-label">
				<label>
					Industry</label>
			</div>
			<div class="editor-field">
				<%: Html.DropDownList("IndustryId", Model.Industries, new { @val = "${IndustryId}" })%>
			</div>
			<div class="editor-label" style="clear: right">
				<label>
					Geography</label>
			</div>
			<div class="editor-field">
				<%: Html.DropDownList("GeographyId", Model.Geographyes, new { @val = "${GeographyId}" })%>
			</div>
			<div class="editor-label">
				<label>
					Description</label>
			</div>
			<div class="editor-field">
				<%: Html.TextArea("Description", "${Description}", new { @style = "width:582px;height:140px;" })%>
			</div>
		</div>
		<div id="ExpandUnderlying">
			<div class="line">
			</div>
			<div id="contatctInformation">
				<div class="headerbox">
					<div class="title">
						<%: Html.Span("CONTACT INFORMATION")%>
					</div>
					<div style="float: right; padding-right: 200px; vertical-align: middle; padding-top: 10px;">
						<%: Html.ImageButton("downarrow.png")%>
					</div>
				</div>
				<div class="expandheader expandsel" style="display: none">
					<div class="expandbtn">
						<div class="expandtitle">
							Contact Information
						</div>
					</div>
				</div>
				<div class="detail" style="display: none" id="ContactInformation">
					<div class="editor-label">
						<label>
							Contact Name</label>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("ContactName", "${ContactName}", new { @class = "wm" })%>
					</div>
					<div class="editor-label" style="clear: right">
						<label>
							Phone Number</label>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("Phone", "${Phone}", new { @class = "wm" })%>
					</div>
					<div class="editor-label">
						<label>
							Email</label>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("Email", "${Email}", new { @class = "wm" })%>
					</div>
					<div class="editor-label" style="clear: right">
						<label>
							Web Address</label>
					</div>
					<div class="editor-field">
						<%: Html.TextBox("WebAddress", "${WebAddress}", new { @class = "wm" })%>
					</div>
					<div class="editor-label">
						<label>
							Registered Address</label>
					</div>
					<div class="editor-field">
						<%: Html.TextArea("Address", "${Address}", new { @style = "width:582px;height:140px;" })%>
					</div>
					<div class="savebox">
						<div class="btn" id="CILoading">
						</div>
						<div class="btn">
							<%: Html.Image("Save.png", new { @onclick = "javascript:underlyingFund.saveTemp('CILoading')" })%></div>
						<div class="resetbtn">
							<%: Html.Span("Reset", new { @onclick = "javascript:underlyingFund.reset('ContactInformation');" })%></div>
					</div>
				</div>
			</div>
			<div class="line">
			</div>
			<div id="BankInformation">
				<div class="headerbox">
					<div class="title">
						<%: Html.Span("BANK INFORMATION")%>
					</div>
					<div style="float: right; padding-right: 200px; vertical-align: middle; padding-top: 10px;">
						<%: Html.ImageButton("downarrow.png")%>
					</div>
				</div>
				<div class="expandheader expandsel" style="display: none">
					<div class="expandbtn">
						<div class="expandtitle">
							Bank Information
						</div>
					</div>
				</div>
				<div class="detail" style="display: none" id="BankInformation">
					<div>
						<div class="editor-label">
							<label>
								Bank Name</label>
						</div>
						<div class="editor-field">
							<%: Html.TextBox("BankName", "${BankName}", new { @class = "wm" })%>
						</div>
						<div class="editor-label" style="clear: right">
							<label>
								ABA No.</label>
						</div>
						<div class="editor-field">
							<%: Html.TextBox("Routing", "${Routing}", new { @class = "wm", @onkeypress = "return jHelper.isNumeric(event);" })%>
						</div>
						<div class="editor-label">
							<label>
								Account Of</label>
						</div>
						<div class="editor-field">
							<%: Html.TextBox("AccountOf", "${AccountOf}", new { @class = "wm" })%>
						</div>
						<div class="editor-label" style="clear: right">
							<label>
								Account No.</label>
						</div>
						<div class="editor-field">
							<%: Html.TextBox("Account", "${Account}", new { @class = "wm" })%>
						</div>
						<div class="editor-label">
							<label>
								Attention</label>
						</div>
						<div class="editor-field">
							<%: Html.TextBox("Attention", "${Attention}", new { @class = "wm" })%>
						</div>
						<div class="editor-label" style="clear: right">
							<label>
								Reference</label>
						</div>
						<div class="editor-field">
							<%: Html.TextBox("Reference", "${Reference}", new { @class = "wm" })%>
						</div>
					</div>
					<div class="savebox">
						<div class="btn" id="BILoading">
						</div>
						<div class="btn">
							<%: Html.Image("Save.png", new { @onclick = "javascript:underlyingFund.saveTemp('BILoading')" })%></div>
						<div class="resetbtn">
							<%: Html.Span("Reset", new { @onclick = "javascript:underlyingFund.reset('BankInformation');" })%></div>
					</div>
				</div>
			</div>
			<div class="line">
			</div>
			<div style="padding-top: 50px; float: right; padding-right: 200px;">
				<span id="SpnSaveLoading">	
				</span>
				<span>
					<%: Html.ImageButton("Add-Underlying-Fund.png", new { @id = "btnSave" })%>
				</span>
			</div>
		</div>
	</script>
</asp:Content>

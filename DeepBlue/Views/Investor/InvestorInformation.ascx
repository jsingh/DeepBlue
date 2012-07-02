<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Investor.InvestorInformation>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
{{if InvestorId>0}}
<div class="section investor-titlebox">
	<div class="info">
		<div class="info-title" style="overflow: hidden">
			${InvestorName}
		</div>
		<div class="editor-field" style="margin-left: 150px; text-align: right;">
			Display Name
		</div>
		<div class="editor-field">
			${Alias}
		</div>
		<div class="editor-field" style="float: right; width: 191px; padding: 5px 5px 0;">
			<%: Html.Span("&nbsp;", new { @id = "DeleteLoading", @style = "float:left;padding: 2px 5px 0;width:66px;" })%>
			<%: Html.Image("delete_active.png", new { @id = "Delete", @style = "cursor:pointer", @onclick = "javascript:editInvestor.deleteInvestor(this);" })%>
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="section">
	<div class="inv-info-title" style="overflow: hidden; margin-bottom: 0px;">
		<span class="title">Investment Details</span>&nbsp;&nbsp;<span id="InvListLoading"></span>
	</div>
	<div class="info inv-grid">
		<table cellpadding="0" cellspacing="0" border="0" id="InvestmentList" class="grid">
			<thead>
				<tr>
					<th sortname="FundName">
						Fund Name
					</th>
					<th sortname="InvestorType" style="width: 10%;">
						Investor Type
					</th>
					<th sortname="TotalCommitment" style="width: 20%;" align="right">
						Total Commitment
					</th>
					<th sortname="UnfundedAmount"  style="width: 20%;" align="right">
						Unfunded Amount
					</th>
					<th sortname="FundClose" style="width: 20%;">
						Fund Close
					</th>
				</tr>
			</thead>
		</table>
	</div>
	<br />
</div>
<div class="line">
</div>
{{/if}}
<div class="section">
	<div class="info">
		<div class="info-title" style="overflow: hidden">
			Investor Information
		</div>
	</div>
	<div class="info investorinfo-box">
		<div class="editor-label firstcol">
			<%: Html.LabelFor(model => model.InvestorName)%>
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.InvestorName, new { @style = "width:238px" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.Alias) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Alias", "${Alias}", new { @style = "width:238px" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.SocialSecurityTaxId)%>
		</div>
		<div class="editor-field">
			<%: Html.jQueryTemplateTextBoxFor(model => model.SocialSecurityTaxId, new { @style = "width:238px" })%>
		</div>
		<div class="editor-label firstcol">
			<%: Html.LabelFor(model => model.DomesticForeign) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("DomesticForeign", Model.DomesticForeigns, new { @val = "${DomesticForeign}" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.Label("State of Residency")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("StateOfResidencyName", "${StateOfResidencyName}", new {  })%>
			<%: Html.Hidden("StateOfResidency", "${StateOfResidency}")%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.EntityType) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownList("EntityType", Model.InvestorEntityTypes, new { @val = "${EntityType}" })%>
		</div>
		<div class="editor-label firstcol">
			<%: Html.LabelFor(model => model.Source) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownListFor(model => model.Source, Model.Sources, new { @val = "${Source}" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.FOIA) %>
		</div>
		<div class="editor-field">
			<%: Html.CheckBoxFor(model => model.FOIA, new { @val = "${FOIA}" })%>
		</div>
		<div class="editor-label" style="clear: right">
			<%: Html.LabelFor(model => model.ERISA) %>
		</div>
		<div class="editor-field">
			<%: Html.CheckBoxFor(model => model.ERISA, new { @val = "${ERISA}" })%>
		</div>
		<% Html.RenderPartial("JQueryTemplateCustomFieldList", Model.CustomField);%>
		<div class="editor-label">
		</div>
		<div class="editor-field">
			<%=Html.jQueryTemplateTextArea("Notes", "${Notes}", 4, 73, new {  })%>
		</div>
	</div>
</div>

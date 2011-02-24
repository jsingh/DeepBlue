<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Entity.InvestorFund>" %>
<% using (Html.BeginForm()) {%>
<%: Html.ValidationSummary(true) %>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.InvestorFundID) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.InvestorFundID) %>
		<%: Html.ValidationMessageFor(model => model.InvestorFundID) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.InvestorID) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.InvestorID) %>
		<%: Html.ValidationMessageFor(model => model.InvestorID) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.FundID) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.FundID) %>
		<%: Html.ValidationMessageFor(model => model.FundID) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.TotalCommitment) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.TotalCommitment) %>
		<%: Html.ValidationMessageFor(model => model.TotalCommitment) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.UnfundedAmount) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.UnfundedAmount) %>
		<%: Html.ValidationMessageFor(model => model.UnfundedAmount) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.CreatedDate) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.CreatedDate) %>
		<%: Html.ValidationMessageFor(model => model.CreatedDate) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.CreatedBy) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.CreatedBy) %>
		<%: Html.ValidationMessageFor(model => model.CreatedBy) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.LastUpdatedDate) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.LastUpdatedDate) %>
		<%: Html.ValidationMessageFor(model => model.LastUpdatedDate) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.LastUpdatedBy) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.LastUpdatedBy) %>
		<%: Html.ValidationMessageFor(model => model.LastUpdatedBy) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.InvestorTypeId) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.InvestorTypeId) %>
		<%: Html.ValidationMessageFor(model => model.InvestorTypeId) %>
	</div>
	<div class="editor-label">
		<%: Html.LabelFor(model => model.CommittedDate) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.CommittedDate) %>
		<%: Html.ValidationMessageFor(model => model.CommittedDate) %>
	</div>
		<input type="submit" value="Save" />
<% } %>
<div>
	<%: Html.ActionLink("Back to List", "Index") %>
</div>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.FixedIncomeDetailModel>" %>
<div id="fixincomediv">
	<table cellpadding="0" border="0" cellspacing="0" width="100%">
		<tr>
			<td>
				<div class="line">
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.FaceValue) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.FaceValue) %>
					<%: Html.ValidationMessageFor(model => model.FaceValue) %>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.ISINO) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.ISINO) %>
					<%: Html.ValidationMessageFor(model => model.ISINO) %>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.Maturity) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.Maturity) %>
					<%: Html.ValidationMessageFor(model => model.Maturity) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.IssuedDate) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.IssuedDate) %>
					<%: Html.ValidationMessageFor(model => model.IssuedDate) %>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.CouponInformation) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.CouponInformation) %>
					<%: Html.ValidationMessageFor(model => model.CouponInformation) %>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.CurrencyId) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.CurrencyId) %>
					<%: Html.ValidationMessageFor(model => model.CurrencyId) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(model => model.Frequency) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.Frequency) %>
					<%: Html.ValidationMessageFor(model => model.Frequency) %>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.FirstCouponDate) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.FirstCouponDate) %>
					<%: Html.ValidationMessageFor(model => model.FirstCouponDate) %>
				</div>
				<div class="editor-label" style="clear: right">
					<%: Html.LabelFor(model => model.FirstAccrualDate) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(model => model.FirstAccrualDate) %>
					<%: Html.ValidationMessageFor(model => model.FirstAccrualDate) %>
				</div>
				<div class="line">
				</div>
			</td>
		</tr>
	</table>
</div>

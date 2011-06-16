<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Issuer.EditIssuerModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Communication Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%= Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%= Html.JavascriptInclueTag("Issuer.js")%>
	<%= Html.JavascriptInclueTag("IssuerEquity.js")%>
	<%= Html.JavascriptInclueTag("IssuerFixedIncome.js")%>
	<%= Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
	<%= Html.StylesheetLinkTag("Issuer.css")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div style="float: left;">
		<%Html.EnableClientValidation(); %>
		<% using (Ajax.BeginForm("UpdateIssuer", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId",
		 HttpMethod = "Post",
		 OnBegin = "issuer.onCreateIssuerBegin",
		 OnSuccess = "issuer.onCreateIssuerSuccess"
	 }, new { @id = "AddNewIssuer" })) {%>
		<div class="editor-label" style="width: 145px">
			<%: Html.LabelFor(model => model.Name) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBoxFor(model => model.Name)%>
		</div>
		<div class="editor-label" style="width: 145px">
			<%: Html.LabelFor(model => model.ParentName)%>
		</div>
		<div class="editor-field">
			<%: Html.TextBoxFor(model => model.ParentName)%>
		</div>
		<div class="editor-label" style="width: 145px">
			<%: Html.LabelFor(model => model.CountryId)%>
		</div>
		<div class="editor-field">
			<%: Html.DropDownListFor(model => model.CountryId, Model.Countries)%>
		</div>
		<div class="status" style="margin: 0 0 0 137px; text-align: left;">
			<%: Html.Span("", new { id = "UpdateLoading" })%></div>
		<div class="editor-button" style="width: 258px; margin: 0 0 0 137px;">
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.ImageButton("SaveExit.png", new { @id = "SaveExit", style = "width: 78px; height: 26px;", onclick = "return issuer.onSubmit('AddNewIssuer',true);" })%>&nbsp;&nbsp;
				<%: Html.ImageButton("Save.png", new { @id = "Save", style = "width: 78px; height: 26px;", onclick = "return issuer.onSubmit('AddNewIssuer',false);" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.issuer.closeDialog(false,0,'',true);" })%>
			</div>
		</div>
		<div id="EquityDetail" style="clear: both">
			<div>
				<%: Html.Image("S_Equities.png", new { @class="expandbtn" })%></div>
			<br />
			<div id="EquityList" class="fieldbox">
				<table id="tblEquity" cellpadding="0" cellspacing="0" border="0" class="grid" style="width: 100%">
					<thead>
						<tr>
							<th>
								No
							</th>
							<th nowrap style="width: 15%">
								Equity Type
							</th>
							<th style="width: 10%">
								Symbol
							</th>
							<th style="width: 10%">
								Public
							</th>
							<th style="width: 15%">
								Share Class Type
							</th>
							<th style="width: 15%">
								Industry
							</th>
							<th style="width: 15%">
								Currency
							</th>
							<th>
							</th>
							<th>
							</th>
						</tr>
					</thead>
					<tbody id="tbodyEquity">
					</tbody>
					<tfoot>
						<tr>
							<td style="text-align: center">
								<%: Html.Span("1.", new { @id = "SpnIndex" }) %>
							</td>
							<td nowrap>
								<%: Html.DropDownList("EquityTypeId", Model.EquityTypes)%>
							</td>
							<td>
								<%: Html.TextBox("Symbol")%>
							</td>
							<td>
								<%: Html.CheckBox("Public")%>
							</td>
							<td>
								<%: Html.DropDownList("ShareClassTypeId", Model.ShareClassTypes)%>
							</td>
							<td>
								<%: Html.DropDownList("IndustryId", Model.Industries)%>
							</td>
							<td>
								<%: Html.DropDownList("CurrencyId", Model.Currencies)%>
							</td>
							<td style="text-align: center; width: 100px;" nowrap>
								<%: Html.Image("add_btn.png", new { @onclick = "javascript:issuer.addEquity(this);" })%>
								<%: Html.Hidden("EquityId", "0")%>
							</td>
							<td class="blank" style="width: 100px;" nowrap>
								<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
							</td>
						</tr>
					</tfoot>
				</table>
			</div>
		</div>
		<div style="height: 10px; clear: both;">
			&nbsp;</div>
		<div id="FixedIncomeDetail" style="clear: both">
			<div>
				<%: Html.Image("S_Fixed-Incomes.png", new { @class = "expandbtn" })%></div>
			<br />
			<div id="FixedIncomeList" class="fieldbox">
				<div id="tblFixedIncome">
					<div class="head">
						<div class="cell" style="width: 20px; min-width: 20px;">
							No
						</div>
						<div class="cell" style="width: 126px">
							Fixed Income Type
						</div>
						<div class="cell" style="width: 136px">
							Symbol
						</div>
						<div class="cell" style="width: 128px">
							FaceValue
						</div>
						<div class="cell" style="width: 142px">
							Maturity
						</div>
						<div class="cell" style="width: 142px">
							IssuedDate
						</div>
						<div class="cell" style="width: 142px">
							Coupon
						</div>
						<div class="cell" style="width: 145px">
						</div>
						<div class="cell" style="width: 40px">
						</div>
					</div>
					<div class="body" id="tbodyFixedIncome">
					</div>
					<div class="footer">
						<div class="row">
							<div class="cell" style="width: 20px; min-width: 20px;">
								<%: Html.Span("1.", new { @id = "SpnIndex" }) %>
							</div>
							<div class="cell" style="width: 136px">
								<%: Html.DropDownList("FixedIncomeTypeId", Model.FixedIncomeTypes)%>
							</div>
							<div class="cell" style="width: 128px">
								<%: Html.TextBox("Symbol")%>
							</div>
							<div class="cell" style="width: 142px">
								<%: Html.TextBox("FaceValue", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
							</div>
							<div class="cell" style="width: 142px">
								<%: Html.TextBox("Maturity", "", new { @class = "datefield", @id = "0_Maturity" })%>
							</div>
							<div class="cell" style="width: 142px">
								<%: Html.TextBox("IssuedDate", "", new { @class = "datefield", @id = "0_IssuedDate" })%>
							</div>
							<div class="cell" style="width: 142px">
								<%: Html.TextBox("CouponInformation")%>
							</div>
							<div class="cell" style="width: 145px">
								<%: Html.Image("add_btn.png", new { @onclick = "javascript:issuer.addFixedIncome(this);" })%>
								<%: Html.Hidden("FixedIncomeId", "0")%>
							</div>
							<div class="cell" style="width: 40px">
								<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
							</div>
							<div style="clear: both; float: left; height: 60px;">
								<div class="cell" style="width: 20px; min-width: 20px;">
									&nbsp;</div>
								<div class="cell" style="width: 126px">
									<div class="celltitle">
										<%: Html.Span("Frequency")%></div>
									<%: Html.TextBox("Frequency", "", new { @onkeypress = "return jHelper.isNumeric(event);" })%>
								</div>
								<div class="cell" style="width: 136px">
									<div class="celltitle">
										<%: Html.Span("First Coupon Date")%></div>
									<%: Html.TextBox("FirstCouponDate", "", new { @class = "datefield", @id = "0_FirstCouponDate" })%>
								</div>
								<div class="cell" style="width: 128px">
									<div class="celltitle">
										<%: Html.Span("First Accrual Date")%></div>
									<%: Html.TextBox("FirstAccrualDate", "", new { @class = "datefield", @id = "0_FirstAccrualDate" })%>
								</div>
								<div class="cell" style="width: 142px">
									<div class="celltitle">
										<%: Html.Span("Industry")%></div>
									<%: Html.DropDownList("IndustryId", Model.Industries)%>
								</div>
								<div class="cell" style="width: ">
									<div class="celltitle">
										<%: Html.Span("Currency")%></div>
									<%: Html.DropDownList("CurrencyId", Model.Currencies)%>
								</div>
								<div class="cell" style="width: 145px">
									&nbsp;</div>
								<div class="cell" style="width: 40px">
									&nbsp;</div>
							</div>
						</div>
					</div>
				</div>
			</div>
			<%: Html.HiddenFor(model => model.IssuerId) %>
			<%: Html.ValidationMessageFor(model => model.Name)%>
			<%: Html.ValidationMessageFor(model => model.ParentName)%>
			<%: Html.ValidationMessageFor(model => model.CountryId)%>
			<% } %>
			<div id="UpdateTargetId" style="display: none">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		issuer.init();
	</script>
	<script id="EquityRowTemplate" type="text/x-jquery-tmpl">  
	<tr class='emptyrow'><td colspan="9">&nbsp;</td></tr>
	<tr id="Equity_${EquityId}">
		<td style="text-align: center">
		 <%: Html.Span("", new { @id = "SpnIndex" }) %>
		</td>
		<td style="text-align: center"  nowrap>
			<%: Html.DropDownList("EquityTypeId", Model.EquityTypes, new {  @class = "hide", @val = "${EquityTypeId}" })%>
			<%: Html.Span("${EquityType}",new { @class = "show" })%>
		</td>
		<td style="text-align: center">
			<%: Html.TextBox("Symbol","${Symbol}", new {  @class = "hide" })%>
			<%: Html.Span("${Symbol}",new { @class = "show" })%>
		</td>
		<td style="text-align: center">
			<%: Html.CheckBox("Public",false, new { @class = "hide", @val = "${Public}" })%>
			<%: Html.Span("{{if Public==true}}<img src='/Assets/images/tick.gif'/>{{/if}}",new { @class = "show" })%>
		</td>
		<td style="text-align: center">
			<%: Html.DropDownList("ShareClassTypeId", Model.ShareClassTypes, new { @class = "hide", @val = "${ShareClassTypeId}" })%>
			<%: Html.Span("${ShareClassType}",new { @class = "show" })%>
		</td>
		<td style="text-align: center">
			<%: Html.DropDownList("IndustryId", Model.Industries, new { @class = "hide", @val = "${IndustryId}" })%>
			<%: Html.Span("${Industry}",new { @class = "show" })%>
		</td>
		<td style="text-align: center">
			<%: Html.DropDownList("CurrencyId", Model.Currencies, new { @class = "hide", @val = "${IndustryId}" })%>
			<%: Html.Span("${Currency}",new { @class = "show" })%>
		</td>
		<td style="text-align: center;width:200px;" nowrap>
			<%: Html.Image("Editbtn.png", new { @onclick = "javascript:issuer.editEquity(this);" })%>&nbsp;&nbsp;<%: Html.Image("Delete_Btn.png", new { @onclick = "javascript:issuer.deleteEquity(${EquityId},this);" })%>
			<%: Html.Hidden("EquityId","${EquityId}")%>
		</td>
		<td class="blank" nowrap style="width:100px;" nowrap>
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
		</td>
	</tr>
	</script>
	<script id="FixedIncomeRowTemplate" type="text/x-jquery-tmpl">  
	<div class="row" id="FixedIncome_${FixedIncomeId}" style="background-color: #F2F2F2">
		<div class="cell" style="width: 20px; min-width: 20px;">
			<%: Html.Span("", new { @id = "SpnIndex" }) %>
		</div>
		<div class="cell" style="width: 126px">
			<%: Html.DropDownList("FixedIncomeTypeId", Model.FixedIncomeTypes, new { @class = "hide", @val = "${FixedIncomeTypeId}" })%>
			<%: Html.Span("${FixedIncomeType}", new { @class = "show" })%>
		</div>
		<div class="cell" style="width: 136px">
			<%: Html.TextBox("Symbol","${Symbol}", new { @class = "hide" })%>
			<%: Html.Span("${Symbol}", new { @class = "show" })%>
		</div>
		<div class="cell" style="width: 128px">
			<%: Html.TextBox("FaceValue", "${FaceValue}", new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
			<%: Html.Span("${FaceValue}", new { @class = "show" })%>
		</div>
		<div class="cell" style="width: 142px">
			<%: Html.TextBox("Maturity", "${Maturity}", new { @class = "datefield hide",  @id = "${FixedIncomeId}_Maturity"})%>
			<%: Html.Span("${Maturity}", new { @id="SpnMaturity", @class = "show" })%>
		</div>
		<div class="cell" style="width: 142px">
			<%: Html.TextBox("IssuedDate", "${IssuedDate}", new { @class = "datefield hide", @id = "${FixedIncomeId}_IssuedDate" })%>
			<%: Html.Span("${IssuedDate}", new { @id="SpnIssuedDate", @class = "show" })%>
		</div>
		<div class="cell" style="width: 142px">
			<%: Html.TextBox("CouponInformation", "${CouponInformation}", new { @class = "hide" })%>
			<%: Html.Span("${CouponInformation}", new { @class = "show" })%>
		</div>
		<div class="cell" style="width: 145px">
			<%: Html.Image("Editbtn.png", new { @onclick = "javascript:issuer.editFixedIncome(this);" })%>&nbsp;&nbsp;<%: Html.Image("Delete_Btn.png", new { @onclick = "javascript:issuer.deleteFixedIncome(${FixedIncomeId},this);" })%>
			<%: Html.Hidden("FixedIncomeId", "${FixedIncomeId}")%>
		</div>
		<div class="cell" style="width: 40px">
			<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Saving...", new {  @style = "display:none;", @id = "spnAjax" })%>
		</div>
		<div style="clear: both; float: left; height: 60px;" class="hide">
			<div class="cell" style="width: 20px; min-width: 20px;">
				&nbsp;</div>
			<div class="cell" style="width: 126px">
				<div class="celltitle">
					<%: Html.Span("Frequency")%></div>
				<%: Html.TextBox("Frequency", "${Frequency}", new { @class = "hide", @onkeypress = "return jHelper.isNumeric(event);" })%>
			</div>
			<div class="cell" style="width: 136px">
				<div class="celltitle">
					<%: Html.Span("First Coupon Date")%></div>
				<%: Html.TextBox("FirstCouponDate", "${FirstCouponDate}", new { @class = "datefield hide", @id = "${FixedIncomeId}_FirstCouponDate" })%>
			</div>
			<div class="cell" style="width: 128px">
				<div class="celltitle">
					<%: Html.Span("First Accrual Date")%></div>
				<%: Html.TextBox("FirstAccrualDate", "${FirstAccrualDate}", new { @class = "datefield hide", @id = "${FixedIncomeId}_FirstAccrualDate" })%>
			</div>
			<div class="cell" style="width: 142px">
				<div class="celltitle">
					<%: Html.Span("Industry")%></div>
				<%: Html.DropDownList("IndustryId", Model.Industries, new { @class = "hide", @val = "${IndustryId}" })%>
			</div>
			<div class="cell" style="width: 142px">
				<div class="celltitle">
					<%: Html.Span("Currency")%></div>
				<%: Html.DropDownList("CurrencyId", Model.Currencies, new { @class = "hide", @val = "${CurrencyId}" })%>
			</div>
			<div class="cell" style="width: 145px">
				&nbsp;</div>
			<div class="cell" style="width: 40px">
				&nbsp;</div>
		</div>
	</div>
	</script>
</asp:Content>

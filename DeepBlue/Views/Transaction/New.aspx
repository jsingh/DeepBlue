<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Transaction.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New Transaction
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("InvestorCommitment.js") %>
	<%=Html.JavascriptInclueTag("EditTransaction.js") %>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("transaction.css")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">INVESTORS</span><span class="arrow"></span><span class="pname">Investor
					Commitment</span></div>
			<div class="rightcol">
				<div style="margin: 0; padding: 0 0 0 0; float: left;">
					<%: Html.Span(Html.Image("ajax.jpg").ToHtmlString() + "&nbsp;Loading...",new { @id = "SpnLoading",@style="display:none;" })%>
				</div>
				<div style="float: left">
					<%: Html.TextBox("Investor", "SEARCH INVESTOR", new { @id = "Investor", @class = "wm", @style = "width:200px" })%>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="TransactionMain">
		<div id="TransactionContainer">
		</div>
		<%: Html.HiddenFor(model => model.InvestorId)%>
	</div>
	<div id="EditTransaction">
	</div>
	<div id="AddFundClose">
		<%using (Html.Form(new { @id = "frmAddFundClose", @onsubmit = "return false" })) {%>
		<div class="editor-label">
			<%: Html.Label("Name")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("Name", "")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Fund")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("CloseFundName", "")%>
			<%: Html.Hidden("FundId", "0")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("Closing Date")%>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FundClosingDate", "")%>
		</div>
		<div class="editor-label">
			<%: Html.Label("First Closing")%>
		</div>
		<div class="editor-field">
			<%: Html.CheckBox("IsFirstClosing", false)%>
		</div>
		<div class="editor-label">
		</div>
		<div class="editor-field" style="width: 250px">
			<%: Html.Image("Save_active.png", new { @style = "cursor:pointer", @onclick = "javascript:investorCommitment.addFundClose();" })%>
			&nbsp;&nbsp;<%: Html.Image("Cancel_active.png", new { @style = "cursor:pointer", @onclick = "javascript:investorCommitment.cancelFundClose();" })%>
			&nbsp;&nbsp;<%: Html.Span("", new { @id = "Loading" })%>
		</div>
		<%}%>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryAutoComplete("Investor", new AutoCompleteOptions { Source = "/Investor/FindInvestors", MinLength=1,
																	  OnSelect = "function(event, ui){ investorCommitment.selectInvestor(ui.item.id);}"
})%>
	<%= Html.jQueryDatePicker("CommittedDate")%>
	<%= Html.jQueryDatePicker("FundClosingDate")%>
	<script type="text/javascript">
		investorCommitment.init();
		$("#EditTransaction").dialog({
			title: "Transaction",
			autoOpen: false,
			width: 900,
			modal: true,
			position: 'top',
			autoResize: true
		});
		$("#AddFundClose").dialog({
			title: "Fund Close",
			autoOpen: false,
			width: 430,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	</script>
	<script id="TransactionTemplate" type="text/x-jquery-tmpl">
			<% Html.RenderPartial("EditTransaction", Model.EditModel); %>
	</script>
	<script id="TransactionInformationTemplate" type="text/x-jquery-tmpl"> 
		<% Html.RenderPartial("TransactionInformation");%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
		{{each(i,row) rows}}
		<tr id="Row${row.cell[9]}" {{if i%2>0}}class="erow"{{/if}}>
			<td>
				{{if row.cell[9]==0}}
				<%: Html.TextBox("FundName", "${row.cell[2]}", new { })%>
				<%: Html.Hidden("FundId","${row.cell[1]}") %>
				{{else}}
				<%: Html.Span("${row.cell[2]}", new {  })%>
				{{/if}}
			</td>
			<td>
				<%: Html.Span("${row.cell[4]}", new { id="SpnInvestorType"  })%>
				{{if row.cell[9]==0}}
				<%: Html.DropDownListFor(model => model.InvestorTypeId, Model.InvestorTypes, new { @val="${row.cell[3]}", @refresh="true", @action = "InvestorType" })%>
				{{/if}}
			</td>
			<td style="text-align:right">
				<%: Html.Span("${formatCurrency(row.cell[5])}", new { @id="SpnCA", @class = "show" })%>
				<%: Html.TextBox("TotalCommitment", "${row.cell[5]}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
			</td>
			<td style="text-align:right">
				<%: Html.Span("${formatCurrency(row.cell[6])}", new { @id="SpnUFA", @class = "show" })%>
				{{if row.cell[9]>0}}
				<%: Html.TextBox("UnfundedAmount", "${row.cell[6]}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
				{{/if}}
			</td>
			<td>
				{{if row.cell[9]==0}}
				<%: Html.TextBox("FundClose", "${row.cell[8]}", new {  })%>
				<%: Html.Hidden("FundClosingId","${row.cell[7]}") %>
				{{else}}
				<%: Html.Span("${row.cell[8]}", new { })%>
				{{/if}}
			</td>
			<td style="text-align:right;">
				<%: Html.Hidden("InvestorFundId","${row.cell[9]}") %>
				<%: Html.Hidden("InvestorId","${row.cell[10]}") %>
				{{if row.cell[9]==0}}
				<%: Html.Image("add_active.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:investorCommitment.addTransaction(this,${row.cell[9]});" })%>
				{{else}}
				<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:investorCommitment.save(this,${row.cell[9]});" })%>
				<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:investorCommitment.edit(this);" })%>
				<%: Html.Image("trans.png", new { @class = "gbutton show", @title = "Transaction", @onclick = "javascript:investorCommitment.editTS(${row.cell[9]});" })%>
				{{/if}}
			</td>
		</tr>
		{{/each}}
	</script>
</asp:Content>

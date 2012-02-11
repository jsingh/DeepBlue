<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.CreateIssuerModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Directs
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.StylesheetLinkTag("deal.css")%>
	<%=Html.StylesheetLinkTag("dealdirect.css")%>
	<%=Html.StylesheetLinkTag("addufund.css")%>
	<%=Html.StylesheetLinkTag("adddirect.css")%>
	<%=Html.JavascriptInclueTag("DealDirect.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.JavascriptInclueTag("jquery.fileuploader.js")%>
	<%=Html.JavascriptInclueTag("jquery.html5filedrop.js")%>
	<!--[if lt IE 9]>
	<%=Html.JavascriptInclueTag("html5.js")%>
	<![endif]-->
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<div class="title">
					INVESTMENTS</div>
				<div class="arrow">
				</div>
				<div class="pname">
					DIRECT LIBRARY</div>
			</div>
		</div>
	</div>
	<div class="headerbar">
		<div class="breadcrumb">
			<div class="leftcol">
				Direct</div>
			<div class="addbtn" style="display: block" id="AddCBtn">
				<%using (Html.GreenButton(new { @id = "AddCompany", @onclick = "javascript:dealDirect.add();" })) {%>Add
				Company<%}%>
			</div>	
			<div class="addbtn" id="AddUDBtn" style="display: none">
				<%using (Html.GreenButton(new { @id = "AddUD" })) {%>Add Direct<%}%>
			</div>
			<div class="rightcol" id="SearchCBox">
				<%: Html.Span("", new { @id = "SpnIssuerLoading" })%>
				<%: Html.TextBox("S_Issuer", "SEARCH COMPANY", new { @class = "wm", @style = "width:200px;", @id = "S_Issuer" })%>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id="AddNewIssuer" style="display: none">
		<%using (Html.Form(new { @id = "frmAddNewIssuer", @onsubmit = "return dealDirect.createNewIssuer(this);" })) {%>
		<div id="NewIssuerDetail">
		</div>
		<div class="addissuer">
			<div class="btnclose">
				<%: Html.Image("issuerclose.png", new { @onclick = "javascript:dealDirect.close();" })%>
			</div>
			<div class="btn">
				<%: Html.ImageButton("savecompany.png")%></div>
			<div class="btn">
				<%: Html.Span("", new { @id = "SpnNewLoading" })%></div>
		</div>
		<%}%>
	</div>
	<div id="TabHeader">
		<div class="header">
			<div id="TabMain" class="section-tab-main" style="padding-left: 0">
				<div class="section-tab-box" style="padding-left: 60px;">
					<%using (Html.Tab(new { @id = "TabDirectGrid", @class = "section-tab-sel", @onclick = "javascript:dealDirect.selectDirectTab(this,'DirectListBox');" })) {%>Direct
					Setup<%}%>
				</div>
			</div>
		</div>
	</div>
	<div id="DirectListBox" class="section-det">
		<% Html.RenderPartial("TBoxTop"); %>
		<table id="DirectList" cellpadding="0" cellspacing="0" border="0" class="grid">
			<thead>
				<tr>
					<th colspan="4" sortname="DirectName">
						Company
					</th>
				</tr>
			</thead>
		</table>
		<% Html.RenderPartial("TBoxBottom"); %>
	</div>
	<div id="DirectDetailBox">
	</div>
	<%: Html.Hidden("SearchCompanyID","0") %>
	<%: Html.Hidden("Mode",ViewData["mode"]) %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">		dealDirect.init();</script>
	<%= Html.jQueryAutoComplete("S_Issuer", new AutoCompleteOptions {
	Source = "/Deal/FindCompanys",
	MinLength = 1,
	OnSelect = "function(event, ui) { dealDirect.searchCompany(ui.item.id);}"
	})%>
	<%=Html.jQueryFlexiGrid("DirectList", new FlexigridOptions {
	ActionName = "DirectList",
	ControllerName = "Deal",
	HttpMethod = "GET",
	SortName = "DirectName",
	Paging = true 
	, OnSubmit = "dealDirect.onSubmit"
	, OnSuccess= "dealDirect.onGridSuccess"
	, OnRowClick = "dealDirect.onRowClick"
	, OnInit = "dealDirect.onInit"
	, OnTemplate = "dealDirect.onTemplate"
	, BoxStyle = false 
	, RowsLength = 50
	})%>
	<%using (Html.jQueryTemplateScript("GridTemplate")) {%>
	{{each(i,row) rows}} {{if i%4==0}}
	<tr>
		{{/if}}
		<td style="width:25%;"><div class="hidden-cell">
			<a href="javascript:dealDirect.load(${row.cell[0]},'${row.cell[1]}')">${row.cell[1]}</a>
			</div>
		</td>
		{{if i%4==3}}
	</tr>
	{{/if}} {{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("IssuerDetailTemplate")) {%>
	<%Html.RenderPartial("IssuerDetail", Model.IssuerDetailModel);%>
	<%}%>
	<%using (Html.jQueryTemplateScript("EquityDetailTemplate")) {%>
	<%Html.RenderPartial("DirectEquityDetail", Model.EquityDetailModel);%>
	<%}%>
	<%using (Html.jQueryTemplateScript("FixedIncomeDetailTemplate")) {%>
	<%Html.RenderPartial("FixedIncomeDetail", Model.FixedIncomeDetailModel);%>
	<%}%>
	<%using (Html.jQueryTemplateScript("DirectListTemplate")) {%>
	<div id="Edit${id}" class="section-det un-direct-list">
		<% Html.RenderPartial("TBoxTop"); %>
		<table cellpadding="0" cellspacing="0" border="0" class="grid" id="UnderlyingDirectList">
			<thead>
				<tr>
					<th style="width:15%" sortname="SecurityType">
						Security Type
					</th>
					<th sortname="Symbol">
						Symbol
					</th>
					<th style="width:25%" sortname="Industry">
						Industry
					</th>
					<th sortname="Security">
						Security
					</th>
					<th>
					</th>
				</tr>
			</thead>
		</table>
		<% Html.RenderPartial("TBoxBottom"); %>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("UnderlyingDirectListRowTemplate")) {%>
		{{each(i,row) rows}}
	   <tr id="Row${row.ID}" {{if i%2>0}}class="erow"{{else}}class="grow"{{/if}}>
			<td>
				${row.SecurityType}
			</td>
			<td>
				${row.Symbol}
			</td>
			<td>
				${row.Industry}
			</td>
			<td>
				${row.Security}
			</td>
			<td style="text-align:right;">
				<%: Html.Hidden("ID", "${row.ID}")%>
				<%: Html.Hidden("SecurityType", "${row.SecurityType}")%>
				<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;" })%>
				<%: Html.Image("Edit.png", new { @class = "gbutton editbtn", @id = "Edit"  })%>
				<%: Html.Image("largedel.png", new { @class = "gbutton", @onclick="javascript:dealDirect.deleteDirect(this,${row.ID},'${row.SecurityType}');"  })%>
			</td>
		</tr>
		{{/each}}
	<%}%>
	<%using (Html.jQueryTemplateScript("TabTemplate")) {%>
	<div style="float: left">
		<div id="Tab${id}" onmousemove="javascript:$('#tabdel${id}').show();" onmouseout="javascript:$('#tabdel${id}').hide();"
			class="section-tab section-tab-sel">
			<div class="left">
			</div>
			<div class="center" onclick="javascript:dealDirect.selectDirectTab($(this).parent(),'Edit${id}');">
				${DirectName}</div>
			<div class="right">
			</div>
			<div class='tab-delete' style='display: none' id="tabdel${id}" onclick="javascript:dealDirect.deleteTab(${id},true);">
			</div>
		</div>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("SectionTemplate")) {%>
	<div id="Edit${id}" style="background-color:#CBD2DA;clear:both;float:left;width:100%;">
		<%using (Html.Form(new { @id = "frmIssuer${id}", @onsubmit = "return dealDirect.save(this);" })) {%>
		<div id="IssuerDetail">
			{{tmpl(IssuerDetailModel) "#IssuerDetailTemplate"}}
		</div>
		<div style="clear:both;padding: 10px 0px;float:left;">
		<div class="editor-label">
			<%: Html.Label("Security Type")%>
		</div>
		<div class="editor-field" style="width: auto;padding:1px 0 0;">
			<div id="equitytab" class="sel" onclick="javascript:dealDirect.selectTab('E',this);">
				&nbsp;
			</div>
			<div id="fitab" onclick="javascript:dealDirect.selectTab('F',this);">
				&nbsp;
			</div>
		</div>
		</div>
		<div class="line"></div>
		<div class="subdetail">
			<div id="EQdetail">
				{{tmpl(EquityDetailModel) "#EquityDetailTemplate"}}
			</div>
			<div id="FixedIncome" style="display: none">
				{{tmpl(FixedIncomeDetailModel) "#FixedIncomeDetailTemplate"}}
			</div>
		</div>
		<div class="line"></div>
		<div class="subdetail" style="clear:both;">
			<div style="float:right;clear:both;margin-right:135px;">
			<div class="editor-label" style="float: right;width:auto;padding-top:3px;">
				<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:dealDirect.cancel(this);" })%>
			</div>
			<div class="editor-label" style="float:right;width:auto;">
				<%: Html.ImageButton("add_direct_active.png")%>
			</div><div class="editor-label" style="float:right;width:auto;">
				<%: Html.Span("", new { @id = "SpnSaveIssuerLoading" } )%>&nbsp;&nbsp;&nbsp;
			</div>
			</div>
		</div>
		<%}%>
	</div>
	<%}%>
	<%using (Html.jQueryTemplateScript("UnderlyingDirectDocumentTemplate")) {%>
		{{each(f,file) rows}}
		<tr id="Row${file.DocumentID}" {{if f%2>0}}class="erow"{{/if}}>
			<td>
				${file.DocumentTypeName}
			</td>
			<td>
				${file.DocumentName}
			</td>
			<td>
			</td>
		</tr>
		{{/each}}
	<%}%>
	<script type="text/javascript">
		dealDirect.newEquityData = <%=JsonSerializer.ToJsonObject(new DeepBlue.Models.Deal.EquityDetailModel())%>;
		dealDirect.newFixedIncomeData = <%=JsonSerializer.ToJsonObject(new DeepBlue.Models.Deal.FixedIncomeDetailModel())%>;
	</script>
	<script type="text/javascript">_fileExt=<%=Model.DocumentFileExtensions%>;</script>
</asp:Content>

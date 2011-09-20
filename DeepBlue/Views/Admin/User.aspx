<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<%=Html.JavascriptInclueTag("User.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<span class="title">ADMIN</span><span class="arrow"></span><span class="pname">USER
					MANAGEMENT</span></div>
			<div class="rightcol">
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-content">
			<% Html.RenderPartial("TBoxTop"); %>
			<table cellpadding="0" cellspacing="0" border="0" id="UserList" class="grid">
				<thead>
					<tr>
						<th sortname="FirstName" style="width: 20%">
							First Name
						</th>
						<th sortname="LastName" style="width: 20%">
							Last Name
						</th>
						<th sortname="Login" style="width: 20%">
							UserName
						</th>
						<th sortname="Email" style="width: 20%">
							Email
						</th>
						<th sortname="Enabled" style="width: 10%">
							Enabled
						</th>
						<th style="width: 10%">
						</th>
					</tr>
				</thead>
			</table>
			<% Html.RenderPartial("TBoxBottom"); %>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("UserList", new FlexigridOptions { 
    ActionName = "UserList", ControllerName = "Admin",
	HttpMethod = "GET",
	SortName = "Login",
	Paging = true 
	, OnSuccess= "user.onGridSuccess"
	, OnRowClick = "user.onRowClick"
	, OnInit = "user.onInit"
	, OnTemplate = "user.onTemplate"
	, TableName = "User"
	, ExportExcel = true
})%>
	<script id="AddButtonTemplate" type="text/x-jquery-tmpl">
<%using (Html.GreenButton(new { @onclick = "javascript:user.add(this);" })) {%>${name}<%}%>
	</script>
	<script id="GridTemplate" type="text/x-jquery-tmpl">
{{each(i,row) rows}}
<tr id="Row${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}}>
	<td style="width: 20%">
		<%: Html.Span("${row.cell[1]}", new { @class = "show" })%>
	</td>
	<td style="width: 20%">
		<%: Html.Span("${row.cell[2]}", new { @class = "show" })%>
	</td>
	<td style="width: 20%">
		<%: Html.Span("${row.cell[3]}", new { @class = "show" })%>
	</td>
	<td style="width: 20%">
		<%: Html.Span("${row.cell[4]}", new { @class = "show" })%>
	</td>
	<td style="width: 10%">
		<%: Html.Span("{{if row.cell[5]}}"+Html.Image("tick.png").ToHtmlString()+"{{/if}}", new { @class = "show" })%>		
	</td>
	<td style="text-align:right;width:10%;">
		{{if row.cell[0]==0}}
		<%: Html.Image("add_active.png", new { @id = "Add", @style="display:none;cursor:pointer;" , @onclick = "javascript:user.save(this,${row.cell[0]});" })%>
		{{else}}
		<%: Html.Image("Save_active.png", new { @id = "Save", @style="display:none;cursor:pointer;", @onclick = "javascript:user.save(this,${row.cell[0]});" })%>
		<%: Html.Image("Edit.png", new { @class = "gbutton show", @onclick = "javascript:user.edit(this,${row.cell[0]});" })%>
		<%: Html.Image("largedel.png", new { @class = "gbutton show", @onclick = "javascript:user.deleteRow(this,${row.cell[0]});" })%>
		{{/if}}
		<%: Html.Hidden("UserId", "${row.cell[0]}") %>
	</td>
</tr>
<tr id="EditRow${row.cell[0]}" {{if i%2>0}}class="erow"{{/if}} style="background-image:none;">
		<td colspan=6 style="width: 100%;display:none;">
			<%using(Html.Form(new { @id="frm${row.cell[0]}", @onsubmit = "return false;" })){%>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("First Name") %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("FirstName", "${row.cell[1]}") %>
			</div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Last Name") %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("LastName", "${row.cell[2]}") %>
			</div>
			<div class="editor-label">
				<%: Html.Label("Middle Name") %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("MiddleName", "${row.cell[6]}") %>
			</div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Email") %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Email","${row.cell[4]}") %>
			</div>
			<div class="editor-label">
				<%: Html.Label("PhoneNumber") %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("PhoneNumber","${row.cell[8]}") %>
			</div>
			<div class="editor-label">
				Login Details
			</div>
			<div class="editor-label">
				<%: Html.Label("User Name") %>
			</div>
			<div class="editor-field">
				<%: Html.TextBox("Login", "${row.cell[3]}") %>
			</div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Password") %>
			</div>
			<div class="editor-field">
				{{if row.cell[0]>0}}
					<%: Html.Password("Password","", new { @disabled = "disabled" }) %>
					<%: Html.Hidden("ChangePassword","false")%>
				{{else}}
					<%: Html.Password("Password","") %>
					<%: Html.Hidden("ChangePassword","true")%>
				{{/if}}
			</div>
			{{if row.cell[0]>0}}
				<div class="editor-label" style="clear:right;margin:0; padding: 2px 0 0 7px;">
					<%: Html.Image("Editbtn_active.png", new { @onclick = "javascript:user.editPassword(${row.cell[0]});" }) %>
					<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:user.cancelPassword(${row.cell[0]});" }) %>
				</div>
			{{/if}}
			<div class="editor-label">
				<%: Html.Label("Enabled") %>
			</div>
			<div class="editor-field">
				<%: Html.CheckBox("Enabled", false, new { @val = "${row.cell[5]}" }) %>
			</div>
			<div class="editor-label" style="clear:right">
				<%: Html.Label("Admin") %> 
			</div>
			<div class="editor-field">
				<%: Html.CheckBox("IsAdmin", false, new { @val = "${row.cell[7]}" }) %>
			</div>
			<%: Html.Hidden("UserId", "${row.cell[0]}")%>
			<div class="editor-label" style="margin-left:35%;margin-top:10px;width:200px;text-align:left;">
				<%: Html.Image("Save_active.png", new { @class="submitbtn", @onclick = "javscript:user.save(${row.cell[0]});" } )%>
				&nbsp;&nbsp;<%: Html.Image("Cancel_active.png", new { @onclick = "javascript:user.cacelEdit(${row.cell[0]});" }) %>
				&nbsp;&nbsp;<%:Html.Span("", new { @id = "Loading" })%>
			</div>
			<%}%>
		</td>
	</tr>
{{/each}}
	</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.JavascriptInclueTag("FileType.js")%>
	<%=Html.JavascriptInclueTag("FlexGrid.js")%>
	<%=Html.StylesheetLinkTag("flexigrid.css") %>
	<%=Html.StylesheetLinkTag("adminbackend.css") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="admin-main">
		<div class="admin-header">
			<a href="javascript:fileType.add(0);">
				<%: Html.Image("add_icon.png") %>
				&nbsp;Add FileType</a>
		</div>
		<div class="admin-content">
			<table cellpadding="0" cellspacing="0" border="0" id="FileTypeList">
				<thead>
					<tr>
						<th sortname="FileTypeID" style="width: 5%;" align="center">
							ID
						</th>
						<th sortname="FileTypeName" style="width: 50%">
							FileType
						</th>
						<th sortname="FileExtension" style="width: 40%">
							FileType
						</th>
						<th align="center" style="width: 5%;">
						</th>
					</tr>
				</thead>
			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%=Html.jQueryFlexiGrid("FileTypeList", new FlexigridOptions { ActionName = "FileTypeList", ControllerName = "Admin"
	, HttpMethod = "GET"
	, SortName = "FileTypeName"
	, Paging = true
	, OnRowBound = "fileType.onRowBound"
})%>
</asp:Content>

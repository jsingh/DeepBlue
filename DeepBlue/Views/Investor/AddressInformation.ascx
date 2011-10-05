<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="line"></div>
<div id="addressInfoMain">
	<div class="expandheader">
		<div class="expandbtn">
			<div class="expandimg" id="img">
				ADD ADDRESS INFORMATION</div>
			<div class="expandtitle" id="title">
				<div class="expandtitle">
					ADDRESS INFORMATION</div>
			</div>
		</div>
	</div>
	<div class="fieldbox addressinfo-box editinfo" style="display:none;">
		{{each(i,address) AddressInformations}}
			{{tmpl(address) "#AddressInfoEditTemplate"}}
		{{/each}}
	</div>
</div>


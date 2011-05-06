var underlyingFundType={
	pageLoad: false
	,init: function () {
		$("document").ready(function () {
			underlyingFundType.resizeIframe();
			underlyingFundType.pageLoad=true;
			$("body").css("overflow","hidden");
		});
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditUnderlyingFundType/"+id+"?t="+dt.getTime();
		$("#addUnderlyingFundTypeDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addUnderlyingFundTypeDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").css("overflow","hidden").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Underlying Fund Type",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteUnderlyingFundType: function (id,img) {
		if(confirm("Are you sure you want to delete this UnderlyingFundType?")) {
			var dt=new Date();
			var url="/Admin/DeleteUnderlyingFundType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#UnderlyingFundTypeList").flexReload();
				}
			});
		}
	}
	,resizeIframe: function (h) {
		var theFrame=$("#iframe_modal",parent.document.body);
		if(theFrame) {
			var bdyHeight=$("body").height();
			if(parseInt(h)>0&&this.pageLoad) {
				bdyHeight=bdyHeight+h;
			}
			theFrame.height(bdyHeight);
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' src='/Assets/images/Edit.gif'/>");
		});
	}
	,onRowClick: function (row) {
		underlyingFundType.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addUnderlyingFundTypeDialog").dialog('close');
		if(reload==true) {
			$("#UnderlyingFundTypeList").flexReload();
		}
	}
	,onUnderlyingFundTypeBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onUnderlyingFundTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.underlyingFundType.closeDialog(true);
		}
	}
}
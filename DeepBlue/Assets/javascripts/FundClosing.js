var fundClosing={
	pageLoad: false
	,init: function () {
		$("document").ready(function () {
			fundClosing.resizeIframe();
			fundClosing.pageLoad=true;
			$("body").css("overflow","hidden");
		});
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditFundClosing/"+id+"?t="+dt.getTime();
		$("#addFundClosingDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addFundClosingDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").css("overflow","hidden").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Fund Closing",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteFundClosing: function (id,img) {
		if(confirm("Are you sure you want to delete this fund closing?")) {
			var dt=new Date();
			var url="/Admin/DeleteFundClosing/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#FundClosingList").flexReload();
				}
			});
		}
	}
	,showDate : function(dateText, inst){
		fundClosing.resizeIframe(130);
	}
	,closeDate : function(){
		fundClosing.resizeIframe(-130);
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
		var frm=document.getElementById(formId);
		var message='';
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		if(message!="") {
			alert(message);
			return false;
		}
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		if(message!="") {
			alert(message);
			return false;
		} else {
			return true;
		}
		return true;
	}
	,closeDialog: function (reload) {
		$("#addFundClosingDialog").dialog('close');
		if(reload==true) {
			$("#FundClosingList").flexReload();
		}
	}
	,onCreateFundClosingBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateFundClosingSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.fundClosing.closeDialog(true);
		}
	}
}
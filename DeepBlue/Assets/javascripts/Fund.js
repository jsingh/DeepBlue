var fund={
	init: function () {
		this.resizeIframe();
	}
	,add: function () {
		var dt=new Date();
		var url="/Fund/New/?t="+dt.getTime();
		this.open(url);
	}
	,edit: function (id) {
		var dt=new Date();
		var url="/Fund/Edit/"+id+"?t="+dt.getTime();
		this.open(url);
	}
	,open: function (url) {
		$("#addFundDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addFundDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Fund",
			autoOpen: true,
			width: 850,
			modal: true,
			position: 'top',
			autoResize: true
		});
	}
	,resizeIframe: function () {
		$("document").ready(function () {
			var theFrame=$("#iframe_modal",parent.document.body);
			if(theFrame) {
				theFrame.height($("body").height()+10);
			}
		});
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
		var UnfundedAmount=parseFloat($("#UnfundedAmount",frm).val());
		var CommitmentAmount=parseFloat($("#CommitmentAmount",frm).val());
		if(isNaN(UnfundedAmount)) {
			UnfundedAmount=0;
		}
		if(isNaN(CommitmentAmount)) {
			CommitmentAmount=0;
		}
		if(CommitmentAmount>UnfundedAmount) {
			message+="Transaction Amount should be less than Unfunded Commitment Amount\n";
		}
		if(message!="") {
			alert(message);
			return false;
		} else {
			return true;
		}
		return true;
	}
	,onTaxIdAvailable: function (message) {
		if(message!='')
			alert(message);
	}
	,closeDialog: function (reload) {
		$("#addFundDialog").dialog('close');
		if(reload==true) {
			$("#FundList").flexReload();
		}
	}
	,onCreateFundBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateFundSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(UpdateTargetId.html()!="")
			alert(UpdateTargetId.html());
		else
			parent.fund.closeDialog(true);
	}
}
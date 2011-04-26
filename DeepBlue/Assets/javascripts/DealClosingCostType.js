var dealClosingCostType={
	init: function () {
		this.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditDealClosingCostType/"+id+"?t="+dt.getTime();
		$("#addDealClosingCostTypeDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addDealClosingCostTypeDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Deal Closing Cost Type",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteType: function (id,img) {
		if(confirm("Are you sure you want to delete this deal closing cost type?")) {
			var dt=new Date();
			var url="/Admin/DeleteDealClosingCostType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#DealClosingCostTypeList").flexReload();
				}
			});
		}
	}
	,resizeIframe: function () {
		$("document").ready(function () {
			var theFrame=$("#iframe_modal",parent.document.body);
			if(theFrame) {
				theFrame.height($("body").height());
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
		if(message!="") {
			alert(message);
			return false;
		} else {
			return true;
		}
		return true;
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' src='/Assets/images/Edit.gif'/>");
		});
	}
	,onRowClick: function (row) {
		dealClosingCostType.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDealClosingCostTypeDialog").dialog('close');
		if(reload==true) {
			$("#DealClosingCostTypeList").flexReload();
		}
	}
	,onCreateDealClosingCostTypeBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateDealClosingCostTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.dealClosingCostType.closeDialog(true);
		}
	}
}
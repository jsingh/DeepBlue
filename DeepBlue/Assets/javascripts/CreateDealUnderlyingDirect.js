var dealUnderlyingDirect={
	init: function () {
		this.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Deal/EditDealUnderlyingDirect/"+id+"?t="+dt.getTime();
		$("#addDealUDirectDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addDealUDirectDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Deal Underlying Direct",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'top',
			autoResize: true
		});
	}
	,deleteDealUnderlyingDirect: function (id,img) {
		if(confirm("Are you sure you want to delete this deal underlying direct?")) {
			var dt=new Date();
			var url="/Deal/DeleteDealUnderlyingDirect/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#DealUnderlyingDirectList").flexReload();
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
		return jHelper.formSubmit(formId);
	}
	,onRowBound: function (tr,row) {
		$("td:last div",tr).html("<img id='Edit' src='/Assets/images/Edit.gif'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/Delete.png'/>");
		$("td:not(:last)",tr).click(function () { dealUnderlyingDirect.add(row.cell[0]); });
		$("#Edit",tr).click(function () { dealUnderlyingDirect.add(row.cell[0]); });
		$("#Delete",tr).click(function () { dealUnderlyingDirect.deleteDealUnderlyingDirect(row.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addDealUDirectDialog").dialog('close');
		if(reload==true) {
			$("#DealUnderlyingDirectList").flexReload();
		}
	}
	,onCreateDealUnderlyingDirectBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateDealUnderlyingDirectSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html()).indexOf("True|")<= -1) {
			alert(UpdateTargetId.html())
		} else {
			parent.dealUnderlyingDirect.closeDialog(true);
		}
	}
	,loadSecurity: function () {
		var issuerId=$("#IssuerId").val();
		var securityTypeId=$("#SecurityTypeId").val();
		var ddl=$("#SecurityId").get(0);
		ddl.options.length=null;
		var listItem;
		listItem=new Option("--Select One--","0",false,false);
		ddl.options[0]=listItem;
		if(parseInt(issuerId)>0&&parseInt(securityTypeId)>0) {
			ddl.options.length=null;
			listItem=new Option("Loading...","",false,false);
			ddl.options[0]=listItem;
			var dt=new Date();
			var url="/Deal/GetSecurity?issuerId="+issuerId+"&securityTypeId="+securityTypeId+"&t="+dt.getTime();
			$.getJSON(url,function (data) {
				ddl.options.length=null;
				if(data.length>0) {
					for(i=0;i<data.length;i++) {
						listItem=new Option(data[i].Text,data[i].Value,false,false);
						ddl.options[ddl.options.length]=listItem;
					}
				} else {
					listItem=new Option("--Select One--","0",false,false);
					ddl.options[0]=listItem;
				}
			});
		}
	}
}
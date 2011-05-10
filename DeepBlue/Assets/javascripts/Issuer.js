var issuer={
	isCreateDealPage: false
	,onIssuerSuccess: null
	,onCloseDialog: null
	,isSaveExit: false
	,init: function () {
		this.resizeIframe();
		this.loadIssuer();
		$(".expandbtn").toggle(function () {
			var parent=$(this).parent().parent();
			var fname=this.src.substring(this.src.lastIndexOf('/')+1);
			var src=this.src.replace("/"+fname,"");
			fname=fname.replace("S_","");
			this.src=src+"/"+fname;
			$(".fieldbox",parent).hide();
			issuer.resizeIframe();
		},function () {
			var parent=$(this).parent().parent();
			var fname=this.src.substring(this.src.lastIndexOf('/')+1);
			var src=this.src.replace("/"+fname,"");
			fname="S_"+fname.replace("S_","");
			this.src=src+"/"+fname;
			$(".fieldbox",parent).show();
			issuer.resizeIframe();
		});
	}
	,loadIssuer: function () {
		var issuerId=parseInt($("#IssuerId").val());
		var dt=new Date();
		var url="/Issuer/FindIssuer?id="+issuerId+"&t="+dt.getTime();
		$.getJSON(url,function (data) {
			$("#Name").val(data.Name);
			$("#ParentName").val(data.ParentName);
			$("#CountryId").val(data.CountryId);
			$.each(data.Equities,function (index,item) { issuer.loadEquityData(item); });
			$.each(data.FixedIncomes,function (index,item) { issuer.loadFixedIncomeData(item); });
			issuer.resizeIframe();
			issuer.applyDatePicker($("#FixedIncomeDetail"));
		});
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Issuer/EditIssuer/"+id+"?t="+dt.getTime();
		$("#addDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Issuer",
			autoOpen: true,
			width: 1000,
			modal: true,
			position: 'top',
			autoResize: true
		});
	}
	,deleteIssuer: function (id,img) {
		if(confirm("Are you sure you want to delete this issuer?")) {
			var dt=new Date();
			var url="/Issuer/DeleteIssuer/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#IssuerList").flexReload();
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
	,onSubmit: function (formId,saveExit) {
		issuer.onIssuerSuccess=null;
		issuer.isSaveExit=saveExit;
		return jHelper.formSubmit(formId);
	}
	,onRowBound: function (tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' src='/Assets/images/Edit.gif'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/Delete.png'/>");
		$("#Edit",lastcell).click(function () { issuer.add(data.cell[0]); });
		$("#Delete",lastcell).click(function () { issuer.deleteIssuer(data.cell[0],this); });
		$("td:not(:last)",tr).click(function () { issuer.add(data.cell[0]); });
	}
	,closeDialog: function (reload,issuerId,issuerName) {
		if(issuer.isSaveExit) {
			$("#addDialog").dialog('close');
			if(reload==true) { if(issuer.isCreateDealPage) { if(issuerId>0) { deal.loadIssuers(issuerName,issuerId); } } else { $("#IssuerList").flexReload(); } }
		}
	}
	,onCreateIssuerBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateIssuerSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		var data=jQuery.trim(UpdateTargetId.html());
		if(data.indexOf("True")<0) {
			alert(UpdateTargetId.html())
		} else {
			var arr=data.split("||");
			var issuerId=arr[1];
			var issuerName=arr[2];
			issuer.setIssuerId(issuerId);
			parent.issuer.isSaveExit=issuer.isSaveExit;
			if(issuer.onIssuerSuccess) {
				issuer.onIssuerSuccess();
				issuer.onCloseDialog=function () { parent.issuer.closeDialog(true,issuerId,issuerName); }
			} else {
				parent.issuer.closeDialog(true,issuerId,issuerName);
			}
		}
	}
	,getIssuerId: function () { return $("#IssuerId").val(); }
	,setIssuerId: function (id) { $("#IssuerId").val(id); }
	/* common functions */
	,selectValue: function (target) {
		$("select",target).each(function () { var id=parseInt($(this).attr("val"));if(isNaN(id)) { id=0; } this.value=id; });
	}
	,checkboxValue: function (target) {
		$("input:checkbox",target).each(function () { var bool=$(this).attr("val");if(bool.toLowerCase()=="true") { this.checked=true; } });
	}
	,applyDatePicker: function (target) {
		$(".datefield",target).each(function () {
			$(this).datepicker({ changeMonth: true,changeYear: true });
		});
	}
	,setIndex: function (target) {
		var index=0;
		$("tbody tr",target).each(function () { index=issuer.putIndex(this,index); });
		$("tfoot tr",target).each(function () { index=issuer.putIndex(this,index); });
	}
	,putIndex: function (tr,index) {
		var spnindex=$("#SpnIndex",tr).get(0);if(spnindex) { index++;spnindex.innerHTML=index+"."; } return index;
	}
	,showElements: function (tr) {
		$(".hide",tr).css("display","block");
		$(".show",tr).css("display","none");
	}
}
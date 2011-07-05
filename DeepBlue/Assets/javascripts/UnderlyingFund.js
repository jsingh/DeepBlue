var underlyingFund={
	tempSave: false
	,onAfterUnderlyingFundSave: null
	,init: function () {
		jHelper.resizeIframe();
		jHelper.waterMark();
		underlyingFund.setUp();
		dealDirect.isUnderlyingFundModel=true;
		dealDirect.onCreateNewIssuer=function (id) {
			underlyingFund.load(0,id);
		}
		dealDirect.onAddIssuer=function () {
			$("#Name","#NewIssuerDetail").focus();
			$("#UnderlyingFundDetail").scrollTop(0);
		}
	}
	,setUp: function () {
		$(document).ready(function () {
			underlyingFund.formatPercent("TaxRate");
			underlyingFund.formatPercent("ManagementFee");
			underlyingFund.formatPercent("IncentiveFee");
			underlyingFund.expand();
			$("#Issuer").autocomplete({ source: "/Deal/FindIssuers",minLength: 1
			,select: function (event,ui) { $("#IssuerId").val(ui.item.id); },appendTo: "body",delay: 300
			});
		});
	}
	,formatPercent: function (txtid) {
		var txt=$("#"+txtid);
		if($.trim(txt.val())!="") {
			txt.val(txt.val()+"%");
		}
	}
	,deleteUnderlyingFund: function (id,img) {
		if(confirm("Are you sure you want to delete this underlying fund?")) {
			var dt=new Date();
			var url="/Admin/DeleteUnderlyingFund/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#UnderlyingFundList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId,false);
	}
	,onRowBound: function (tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' src='/Assets/images/Edit.png'/>");
		$("#Edit",lastcell).click(function () { underlyingFund.add(data.cell[0]); });
		$("td:not(:last)",tr).click(function () { underlyingFund.add(data.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#UnderlyingFundList").flexReload();
		}
	}
	,onCreateUnderlyingFundBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateUnderlyingFundSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			alert(UpdateTargetId.html())
		} else {
			parent.underlyingFund.closeDialog(true);
		}
	}
	,selectIssuer: function (id) {
		$("#IssuerId").val(id);
	}
	,checkIssuer: function (txt) {
		if($.trim(txt.value)=="") {
			$("#IssuerId").val(0);
		}
	}
	,expand: function () {
		$(".headerbox").click(function () {
			$(".headerbox").show();
			$(".expandheader").hide();
			$(".detail").hide();
			$(this).hide();
			var parent=$(this).parent();
			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
			var display=detail.attr("issearch");
			detail.show();
		});
		$(".expandtitle",".expandheader").click(function () {
			var expandheader=$(this).parents(".expandheader:first");
			var parent=$(expandheader).parent();
			expandheader.hide();
			var detail=$(".detail",parent);
			detail.hide();
			$(".headerbox",parent).show();
		});
	}
	,load: function (id,issuerId) {
		var addUnderlyingfund=$("#AddUnderlyingFund");
		addUnderlyingfund.css("text-align","center").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		addUnderlyingfund.show();
		$.getJSON("/Deal/FindUnderlyingFund",{ "_": (new Date).getTime(),"underlyingFundId": id,"issuerId": issuerId },function (data) {
			addUnderlyingfund.empty();
			addUnderlyingfund.css("text-align","left");
			$("#UnderlyingFundTemplate").tmpl(data).appendTo(addUnderlyingfund);
			underlyingFund.setUp();
			jHelper.checkValAttr(addUnderlyingfund);
			jHelper.formatDateTxt(addUnderlyingfund);
			$("#Description").val($.trim($("#Description").val()));
			$("#Address").val($.trim($("#Address").val()));
			if(id>0) {
				$("#btnSave").attr("src","/Assets/images/muf.png");
			} else {
				$("#btnSave").attr("src","/Assets/images/adduf.png");
			}
			$("#Doc_DocumentDate").datepicker({ changeMonth: true,changeYear: true });
		});
	}
	,save: function (frm) {
		try {
			var loading=$("#SpnSaveLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Deal/UpdateUnderlyingFund",$(frm).serializeArray(),function (data) {
				loading.empty();
				$("#SpnDocLoading").empty();
				$("#BILoading").empty();
				$("#CILoading").empty();
				var arr=data.split("||");
				if(arr[0]=="True") {
					if(underlyingFund.onAfterUnderlyingFundSave) {
						underlyingFund.onAfterUnderlyingFundSave();
					}
					if(underlyingFund.tempSave==false) {
						alert("Underlying Fund Added.");
						$("#AddUnderlyingFund").hide();
						$("#S_UnderlyingFund").val("");
					}
					$("#UnderlyingFundId").val(arr[1]);
				} else { alert(data); }
				underlyingFund.tempSave=false;
			});
		} catch(e) { alert(e); }
		return false;
	}
	,saveTemp: function (loadingId) {
		var loading=$("#"+loadingId);
		underlyingFund.tempSave=true;
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		$("#btnSave").click();
	}
	,reset: function (targetId) {
		var target=$("#"+targetId);
		$(":input[type='text']",target).val("");
		$("textarea",target).val("");
	}
	,saveDocument: function (frm) {
		try {
			var loading=$("#SpnDocLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			var ufid=parseInt($("#UnderlyingFundId").val());
			underlyingFund.tempSave=false;
			underlyingFund.onAfterUnderlyingFundSave=null;
			if(ufid>0) {
				var param=$(frm).serializeArray();
				param[param.length]={ "UnderlyingFundId": ufid };
			} else {
				underlyingFund.tempSave=true;
				underlyingFund.onAfterUnderlyingFundSave=function () { underlyingFund.saveDocument(frm); }
				$("#btnSave").click();
			}
		} catch(e) { alert(e); }
		return false;
	}
	,changeUploadType: function (uploadType) {
		var FileRow=document.getElementById("FileRow");
		var LinkRow=document.getElementById("LinkRow");
		FileRow.style.display="none";
		LinkRow.style.display="none";
		if(uploadType.value=="1")
			FileRow.style.display="";
		else
			LinkRow.style.display="";
	}
}
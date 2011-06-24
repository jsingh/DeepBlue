var underlyingFund={
	tempSave: false
	,init: function () {
		jHelper.resizeIframe();
		jHelper.waterMark();
		underlyingFund.setUp();
		dealDirect.onCreateNewIssuer=function (id) {
			underlyingFund.load(id);
		}
	}
	,setUp: function () {
		$(document).ready(function () {
			underlyingFund.formatPercent("TaxRate");
			underlyingFund.formatPercent("ManagementFee");
			underlyingFund.formatPercent("IncentiveFee");
			underlyingFund.expand();
			$("#Issuer").autocomplete({ source: "/Issuer/FindIssuers",minLength: 1
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
	,add: function (id) {
		var dt=new Date();
		var url="/Deal/EditUnderlyingFund/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Underlying Fund",
			autoOpen: true,
			width: 800,
			modal: true,
			position: 'top',
			autoResize: true
		});
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
	,load: function (id) {
		var addUnderlyingfund=$("#AddUnderlyingFund");
		addUnderlyingfund.empty();
		if($("#AddNewIssuer").css("display")=="block") {
			addUnderlyingfund.css("top","245px");
		}
		addUnderlyingfund.show();
		$.getJSON("/Deal/FindUnderlyingFund",{ "_": (new Date).getTime(),"issuerId": id },function (data) {
			$("#UnderlyingFundTemplate").tmpl(data).appendTo(addUnderlyingfund);
			underlyingFund.setUp();
			jHelper.checkValAttr(addUnderlyingfund);
			jHelper.formatDateTxt(addUnderlyingfund);
			$("#Description").val($.trim($("#Description").val()));
			$("#Address").val($.trim($("#Address").val()));
		});
	}
	,save: function (frm) {
		try {
			var loading=$("#SpnSaveLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Deal/UpdateUnderlyingFund",$(frm).serializeArray(),function (data) {
				loading.empty();
				$("#BILoading").empty();
				$("#CILoading").empty();
				var arr=data.split("||");
				if(arr[0]=="True") {
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
}
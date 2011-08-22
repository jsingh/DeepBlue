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
		lastcell.html("<img id='Edit' class='gbutton' src='/Assets/images/Edit.png'/>");
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
		$(".expandheader").click(function () {
			var expandheader=$(this);
			var parent=$(expandheader).parent();
			expandheader.hide();
			var detail=$(".detail",parent);
			detail.hide();
			$(".headerbox",parent).show();
		});
	}
	,getUnderlyingFundId: function () {
		return parseInt($("#UnderlyingFundId").val());
	}
	,load: function (id,issuerId) {
		var lnkAddUnderlyingFund=$("#lnkAddUnderlyingFund");
		var addUnderlyingfund=$("#AddUnderlyingFund");
		addUnderlyingfund.empty();
		addUnderlyingfund.css("text-align","center").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		addUnderlyingfund.show();
		$.getJSON("/Deal/FindUnderlyingFund",{ "_": (new Date).getTime(),"underlyingFundId": id,"issuerId": issuerId },function (data) {
			addUnderlyingfund.empty();
			addUnderlyingfund.css("text-align","left");
			$("#UnderlyingFundTemplate").tmpl(data).appendTo(addUnderlyingfund);
			$("#btnSave").attr("src","/Assets/images/muf.png");
			$("#lnkAddUnderlyingFund").removeClass("green-btn-sel");
			if(id==0) {
				//src=lnkAddUnderlyingFund.attr("src").replace("addnufund.png","addnufundselect.png");
				$("#S_UnderlyingFund").val("");
				$("#btnSave").attr("src","/Assets/images/adduf.png");
				$("#lnkAddUnderlyingFund").addClass("green-btn-sel");
			}
			underlyingFund.setUp();
			jHelper.checkValAttr(addUnderlyingfund);
			jHelper.formatDateTxt(addUnderlyingfund);
			jHelper.jqComboBox(addUnderlyingfund);
			jHelper.jqCheckBox(addUnderlyingfund);
			$("#Description").val($.trim($("#Description").val()));
			$("#Address").val($.trim($("#Address").val()));
			$("#Doc_DocumentDate").datepicker({ changeMonth: true,changeYear: true });
			var p=new Array();
			p[p.length]={ "name": "UnderlyingFundId","value": underlyingFund.getUnderlyingFundId() };
			$("#DocumentList").flexigrid({
				usepager: true
				,url: "/Deal/UnderlyingFundDocumentList"
				,params: p
				,resizeWidth: true
				,method: "GET"
				,sortname: "DocumentDate"
				,sortorder: "desc"
				,autoload: true
				,height: 200
				,resizeWidth: false
				,useBoxStyle: false
			});
		});
	}
	,documentRefresh: function () {
		var grid=$("#DocumentList");
		var p=new Array();
		p[p.length]={ "name": "UnderlyingFundId","value": underlyingFund.getUnderlyingFundId() };
		grid.flexOptions({ params: p });
		grid.flexReload();
	}
	,save: function (frm) {
		try {
			var loading=$("#SpnSaveLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Deal/UpdateUnderlyingFund",$(frm).serializeForm(),function (data) {
				loading.empty();
				$("#SpnDocLoading").empty();
				$("#BILoading").empty();
				$("#CILoading").empty();
				var arr=data.split("||");
				if(arr[0]=="True") {
					$("#UnderlyingFundId").val(arr[1]);
					if(underlyingFund.onAfterUnderlyingFundSave) {
						underlyingFund.onAfterUnderlyingFundSave();
					} else {
						if(underlyingFund.tempSave==false) {
							alert("Underlying Fund Added.");
							$("#lnkAddUnderlyingFund").removeClass("green-btn-sel");
							$("#AddUnderlyingFund").hide();
							$("#S_UnderlyingFund").val("");
						}
					}
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
			var ufid=underlyingFund.getUnderlyingFundId();
			underlyingFund.tempSave=false;
			underlyingFund.onAfterUnderlyingFundSave=null;
			if(ufid>0) {
				var p=new Array();
				p[p.length]={ "name": "UnderlyingFundId","value": ufid };
				$.ajaxFileUpload(
				{
					url: '/Deal/CreateUnderlyingFundDocument',
					secureuri: false,
					formId: 'frmDocumentInfo',
					param: p,
					dataType: 'json',
					success: function (data,status) {
						loading.empty();
						if($.trim(data.data)!="") {
							alert(data.data);
						} else {
							alert("Document Saved");
							underlyingFund.documentRefresh();
							jHelper.resetFields($("#frmDocumentInfo"));
						}
					}
					,error: function (data,status,e) {
						loading.empty();
						alert(data.msg+","+status+","+e);
					}
				});
			} else {
				underlyingFund.tempSave=true;
				underlyingFund.onAfterUnderlyingFundSave=function () { underlyingFund.saveDocument(frm); }
				$("#btnSave").click();
			}
		} catch(e) { alert(e); }
		return false;
	}
	,uploadDocument: function () {
		return false;
		try {
			$.ajaxFileUpload(
			{
				url: '/Deal/CreateDocument',
				secureuri: false,
				formId: 'frmDocumentInfo',
				dataType: 'json',
				success: function (data,status) {
					if(typeof (data.error)!='undefined') {
						if(data.error!='') {
							alert(data.error);
						} else {
							alert(data.msg);
						}
					}
				},
				error: function (data,status,e) {
					alert(data.msg+","+status+","+e);
				}
			}
		);
		} catch(e) {
			alert(e);
		}
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
	,deleteDocument: function (id,img) {
		if(confirm("Are you sure you want to delete this document?")) {
			img.src="/Assets/images/ajax.jpg";
			$.get("/Deal/DeleteUnderlyingFundDocumentFile/"+id,function (data) {
				if($.trim(data)!="") {
					alert(data);
				} else {
					underlyingFund.documentRefresh();
				}
			});
		}
	}
}
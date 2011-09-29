var underlyingFund={
	tempSave: false
	,onAfterUnderlyingFundSave: null
	,newFundData: null
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
	,setUp: function (box) {
		$(document).ready(function () {
			underlyingFund.formatPercent("TaxRate",box);
			underlyingFund.formatPercent("ManagementFee",box);
			underlyingFund.formatPercent("IncentiveFee",box);
			underlyingFund.expand(box);
			$("#Issuer",box).autocomplete({ source: "/Deal/FindGPs",minLength: 1
			,select: function (event,ui) { $("#IssuerId",box).val(ui.item.id); },appendTo: "body",delay: 300
			});
		});
	}
	,formatPercent: function (txtid,box) {
		var txt=$("#"+txtid,box);
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
					jAlert(data);
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
			jAlert(UpdateTargetId.html())
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
	,expand: function (box) {
		$(".headerbox",box).click(function () {
			//$(".headerbox").show();
			//$(".expandheader").hide();
			//$(".detail").hide();
			$(this).hide();
			var parent=$(this).parent();
			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
			var display=detail.attr("issearch");
			detail.show();
		});
		$(".expandheader",box).click(function () {
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
	,selectTab: function (that,detailid) {
		$(".section-tab").removeClass("section-tab-sel");
		$(".section-det").hide();
		$(that).addClass("section-tab-sel");
		$("#"+detailid).show();
	}
	,load: function (id,issuerId,fundName) {
		var addNewFund=$("#UnderlyingFundDetail");
		var addNewFundTab;
		addNewFundTab=$("#TabFundGrid");
		var data={ id: id,FundName: fundName };
		var tab=$("#Tab"+id);
		var editbox=$("#Edit"+id);
		if(tab.get(0)) {
			addNewFundTab.after(tab);
			addNewFund.after(editbox);
		} else {
			$("#SectionTemplate").tmpl(data).insertAfter(addNewFund);
			$("#TabTemplate").tmpl(data).insertAfter(addNewFundTab);
			editbox=$("#Edit"+id);
			underlyingFund.open(id,issuerId,editbox);
		}
		$(".center",$("#Tab"+id)).click();
	}
	,open: function (id,issuerId,box) {
		var lnkAddUnderlyingFund=$("#lnkAddUnderlyingFund");
		var addUnderlyingfund=$("#AddUnderlyingFund",box);
		addUnderlyingfund.empty();
		addUnderlyingfund.css("text-align","center").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		addUnderlyingfund.show();
		$.getJSON("/Deal/FindUnderlyingFund",{ "_": (new Date).getTime(),"underlyingFundId": id,"issuerId": issuerId },function (data) {
			addUnderlyingfund.empty();
			addUnderlyingfund.css("text-align","left");
			$("#UnderlyingFundTemplate").tmpl(data).appendTo(addUnderlyingfund);
			$("#btnSave",addUnderlyingfund).attr("src","/Assets/images/muf_active.png");
			$("#lnkAddUnderlyingFund").removeClass("green-btn-sel");
			if(id==0) {
				//src=lnkAddUnderlyingFund.attr("src").replace("addnufund.png","addnufundselect.png");
				$("#btnSave",addUnderlyingfund).attr("src","/Assets/images/adduf_active.png");
				$("#lnkAddUnderlyingFund").addClass("green-btn-sel");
			}
			underlyingFund.setUp(box);
			jHelper.checkValAttr(addUnderlyingfund);
			jHelper.formatDateTxt(addUnderlyingfund);
			jHelper.jqComboBox(addUnderlyingfund);
			jHelper.jqCheckBox(addUnderlyingfund);
			$("#Description",addUnderlyingfund).val($.trim($("#Description",addUnderlyingfund).val()));
			$("#Address",addUnderlyingfund).val($.trim($("#Address",addUnderlyingfund).val()));
			$("#ContactNotes",addUnderlyingfund).val($.trim($("#ContactNotes",addUnderlyingfund).val()));
			$("#Doc_DocumentDate",addUnderlyingfund).datepicker({ changeMonth: true,changeYear: true });
			var p=new Array();
			p[p.length]={ "name": "UnderlyingFundId","value": id };
			$("#DocumentList",addUnderlyingfund).flexigrid({
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

			//load contact list
			underlyingFundContact.load(id);
		});
	}
	,documentRefresh: function () {
		var grid=$("#DocumentList");
		var p=new Array();
		p[p.length]={ "name": "UnderlyingFundId","value": underlyingFund.getUnderlyingFundId() };
		grid.flexOptions({ params: p });
		grid.flexReload();
	}
	,deleteTab: function (id,isConfirm) {
		var isRemove=true;
		if(isConfirm) {
			isRemove=confirm("Are you sure you want to remove this underlying fund?");
		}
		if(isRemove) {
			$("#Tab"+id).remove();
			$("#Edit"+id).remove();
			$("#tabdel"+id).remove();
			$("#TabFundGrid").click();
		}
	}
	,save: function (frm) {
		try {
			var loading=$("#SpnSaveLoading",frm);
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Deal/UpdateUnderlyingFund",$(frm).serializeForm(),function (data) {
				loading.empty();
				$("#SpnDocLoading",frm).empty();
				$("#BILoading",frm).empty();
				$("#CILoading",frm).empty();
				var arr=data.split("||");
				if(arr[0]=="True") {
					var ufund=$("#UnderlyingFundId",frm);
					var isNew=false;
					if(ufund.val()==0) {
						isNew=true;
					}
					ufund.val(arr[1]);
					if(underlyingFund.onAfterUnderlyingFundSave) {
						underlyingFund.onAfterUnderlyingFundSave(arr[1]);
					} else {
						if(underlyingFund.tempSave==false) {
							jAlert("Underlying Fund Added.");
							$("#lnkAddUnderlyingFund").removeClass("green-btn-sel");
							//$("#AddUnderlyingFund").hide();
							if(isNew) {
								underlyingFund.deleteTab(0,false);
								underlyingFund.load(arr[1],0,$("#FundName",frm).val());
							}
						}
						underlyingFund.cancelWebPassword();
					}
				} else { jAlert(data); }
				underlyingFund.tempSave=false;
			});
		} catch(e) { jAlert(e); }
		return false;
	}
	,searchGP: function (gpId) {
		var grid=$("#UnderlyingFundList");
		var param=[{ name: "gpId",value: gpId}];
		grid.flexOptions({ params: param });
		grid.flexReload();
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
		if(targetId=="ContactInformation") {
			$(":input[type='password']",target).val("");
			underlyingFund.editWebPassword();
		}
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
							jAlert(data.data);
						} else {
							jAlert("Document Saved");
							underlyingFund.documentRefresh();
							jHelper.resetFields($("#frmDocumentInfo"));
						}
					}
					,error: function (data,status,e) {
						loading.empty();
						jAlert(data.msg+","+status+","+e);
					}
				});
			} else {
				underlyingFund.tempSave=true;
				underlyingFund.onAfterUnderlyingFundSave=function () { underlyingFund.saveDocument(frm); }
				$("#btnSave").click();
			}
		} catch(e) { jAlert(e); }
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
							jAlert(data.error);
						} else {
							jAlert(data.msg);
						}
					}
				},
				error: function (data,status,e) {
					jAlert(data.msg+","+status+","+e);
				}
			}
		);
		} catch(e) {
			jAlert(e);
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
					jAlert(data);
				} else {
					underlyingFund.documentRefresh();
				}
			});
		}
	}
	,editWebPassword: function () {
		$("#WebPassword").removeAttr("disabled");
		$("#ChangeWebPassword").val("true");
	}
	,cancelWebPassword: function () {
		$("#WebPassword").val("").attr("disabled","disabled");
		$("#ChangeWebPassword").val("false");
		$("#EditWebPassword").show();
		$("#CancelWebPassword").show();
	}
	,onGridSuccess: function (t,g) {
		//jHelper.checkValAttr(t);
		//jHelper.jqCheckBox(t);
		//$(window).resize();
		$("tbody tr",t).each(function () {
			var tdlen=$("td",this).length;
			if(tdlen<4) {
				$("td:last",this).attr("colspan",(5-$("td",this).length));
			}
		});
	}
	,onInit: function (g) {
		//var data={ name: "Add Cash Distribution Type" };
		//$("#AddButtonTemplate").tmpl(data).prependTo(g.pDiv);
	}
	,onTemplate: function (tbody,data) {
		$("#GridTemplate").tmpl(data).appendTo(tbody);
	}
}
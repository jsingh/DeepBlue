var underlyingFund={
	tempSave: false
	,onAfterUnderlyingFundSave: null
	,newFundData: null
	,currentDetailBox: null
	,init: function () {
		$(document).ready(function () {
			jHelper.resizeIframe();
			jHelper.waterMark();
			underlyingFund.setUp();
			dealDirect.isUnderlyingFundModel=true;
			dealDirect.onCreateNewIssuer=function (id) {
				//underlyingFund.load(0,id);
				$("#DirectList").flexReload();
			}
			dealDirect.onAddIssuer=function () {
				$("#Name","#NewIssuerDetail").focus();
				$("#UnderlyingFundDetail").scrollTop(0);
			}
			if($("#Mode").val()=="direct") {
				dealDirect.add();
			}
		});
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
	,setupCountryState: function (addInfo) {
		$("#StateName",addInfo)
				.autocomplete({ source: "/Admin/FindStates",minLength: 1
				,select: function (event,ui) {
					$("#State",addInfo).val(ui.item.id);
				}
				,appendTo: "body",delay: 300
				});

		$("#CountryName",addInfo)
		.autocomplete({ source: "/Admin/FindCountrys",minLength: 1
		,select: function (event,ui) {
			$("#Country",addInfo).val(ui.item.id);
			var stateRow=$("#AddressStateRow",addInfo);
			var stateName=$("#StateName",stateRow);
			var state=$("#State",stateRow);
			state.val(52);
			if(ui.item.label!="United States") {
				stateRow.hide();
			} else {
				stateRow.show();
				stateName.val("");
				state.val(0);
			}
		}
		,appendTo: "body",delay: 300
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
					$("#DirectList").flexReload();
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
			$("#DirectList").flexReload();
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
		var AddGPBtn=$("#AddGPBtn");
		var AddUFBtn=$("#AddUFBtn");
		var SearchGP=$("#SearchGP");
		AddGPBtn.hide();
		AddUFBtn.hide();
		SearchGP.hide();
		var add=$("#lnkAddUnderlyingFund",AddUFBtn);
		add.unbind("click");
		underlyingFund.currentDetailBox=$("#"+detailid);
		if(that.id!="TabFundGrid") {
			AddUFBtn.show();
			dealDirect.close();
			add.click(function () {
				//dealDirect.addUD(detailbox,detailid);
				underlyingFund.addUF(underlyingFund.currentDetailBox,detailid.replace("Edit",""));
			});
		} else {
			AddGPBtn.show();
			SearchGP.show();
		}
		underlyingFund.currentDetailBox.show();
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
			$("#UnderlyingFundListTemplate").tmpl(data).insertAfter(addNewFund);
			$("#TabTemplate").tmpl(data).insertAfter(addNewFundTab);
			editbox=$("#Edit"+id);
			underlyingFund.ufList(id,editbox);
		}
		$(".center",$("#Tab"+id)).click();
	}
	,ufList: function (id,box) {
		$("#UnderlyingFundList",box)
		.flexigrid({
			usepager: true
			,useBoxStyle: false
			,url: "/Deal/UnderlyingFundList"
			,onSubmit: function (p) {
				p.params=null;
				p.params=new Array();
				p.params[p.params.length]={ "name": "gpId","value": id };
				return true;
			}
			,onTemplate: function (tbody,data) {
				$("#UnderlyingFundListRowTemplate").tmpl(data).appendTo(tbody);
				$("tr",tbody).each(function () {
					var tr=this;
					$("#Edit",this).click(function () {
						underlyingFund.editUF(tr,$("#ID",tr).val(),$("#IssuerID",tr).val())
					});
				});
			}
			,rpOptions: [25,50,100,200]
			,rp: 25
			,resizeWidth: false
			,method: "GET"
			,sortname: "FundName"
			,sortorder: ""
			,autoload: true
			,height: 0
		});
	}
	,editUF: function (tr,id,issuerId) {
		var trid="Edit_"+id;
		var t=$(tr).parents("table:first");
		$("#"+trid).remove();
		var editTR=document.createElement("tr");
		var td=document.createElement("td");
		td.colSpan=$("thead tr:first th",t).length;
		td.className="edit-cell";
		$(td).html("<center>"+jHelper.loadingHTML()+"</center>");
		editTR.className="un-directedit-box";
		editTR.id=trid;
		$(editTR).append(td);
		$(tr).after(editTR);
		var url="";
		var templateName="SectionTemplate";
		var data={ id: id };
		$(td).empty();
		$("#SectionTemplate").tmpl(data).appendTo(td);
		underlyingFund.open(id,issuerId,td);
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
				//$("#lnkAddUnderlyingFund").addClass("green-btn-sel");
			}
			underlyingFund.setUp(box);
			underlyingFund.setupCountryState(box);
			jHelper.checkValAttr(addUnderlyingfund);
			jHelper.formatDateTxt(addUnderlyingfund);
			jHelper.jqComboBox(addUnderlyingfund);
			jHelper.jqCheckBox(addUnderlyingfund);
			$("#Description",addUnderlyingfund).val($.trim($("#Description",addUnderlyingfund).val()));
			$("#Address",addUnderlyingfund).val($.trim($("#Address",addUnderlyingfund).val()));
			$("#ContactNotes",addUnderlyingfund).val($.trim($("#ContactNotes",addUnderlyingfund).val()));
			$("#Doc_DocumentDate",addUnderlyingfund).datepicker({ changeMonth: true,changeYear: true });
			if(id==0) {
				$(".show",addUnderlyingfund).hide();
				$(".hide",addUnderlyingfund).show();
			}
			/*
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
			*/
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
			$("#lnkAddUnderlyingFund").removeClass("green-btn-sel");
			$("#TabFundGrid").click();
		}
	}
	,save: function (id) {
		try {
			var frm=$("#frmUnderlyingFund"+id,underlyingFund.currentDetailBox);
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
					var tr=$(frm).parents("tr:first");
					var t=$(frm).parents("#UnderlyingFundList:first");
					if(ufund.val()==0) {
						isNew=true;
						$(t).flexReload();
					}
					ufund.val(arr[1]);
					$("#DirectList").flexReload();
					if(underlyingFund.onAfterUnderlyingFundSave) {
						underlyingFund.onAfterUnderlyingFundSave(arr[1]);
					} else {
						if(underlyingFund.tempSave==false) {
							jAlert("Underlying Fund Saved.");
							$(tr).remove();
							$(t).flexReload();
							/*
							$("#lnkAddUnderlyingFund").removeClass("green-btn-sel");
							//$("#AddUnderlyingFund").hide();
							if(isNew) {
							underlyingFund.deleteTab(0,false);
							underlyingFund.load(arr[1],0,$("#FundName",frm).val());
							}
							*/
						}
						//underlyingFund.cancelWebPassword();
					}
				} else { jAlert(data); }
				underlyingFund.tempSave=false;
			});
		} catch(e) { jAlert(e); }
		return false;
	}
	,saveAddress: function (img,id) {
		try {
			var frm=$(img).parents("form:first");
			var frmUnderlyingFund=$(frm).parents(".section-det:first");
			var ufid=$("#UnderlyingFundId",frm).val();
			underlyingFund.tempSave=false;
			underlyingFund.onAfterUnderlyingFundSave=null;
			if(ufid>0) {
				var param=$(frm).serializeForm();
				var url="/Deal/CreateUnderlyingFundAddress";
				var loading=$("#AILoading",frm);
				loading.html(jHelper.savingHTML());

				$.post(url,param,function (data) {
					loading.empty();
					if($.trim(data)!="") {
						jAlert(data);
					} else {
						jAlert("Underlying Fund Address Information Saved");
					}
				});
			} else {
				underlyingFund.tempSave=true;
				underlyingFund.onAfterUnderlyingFundSave=function (ufid) {
					$("#UnderlyingFundId",frm).val(ufid);
					underlyingFund.saveAddress(img,id);
				}
				$("#btnSave",frmUnderlyingFund).click();
			}
		} catch(e) { jAlert(e); }
		return false;
	}
	,searchGP: function (gpId) {
		$("#SearchCompanyID").val(gpId);
		var grid=$("#DirectList");
		grid.flexReload();
	}
	,saveTemp: function (img,id) {
		var frmUnderlyingFund=$(img).parents(".section-det:first");
		var loading=$("#BILoading",frmUnderlyingFund);
		underlyingFund.tempSave=true;
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		underlyingFund.onAfterUnderlyingFundSave=function (ufid) {
			loading.empty();
			jAlert("Underlying Fund Bank Information Saved.");
		}
		underlyingFund.save(id);
	}
	,cancelInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".show",cntdiv).show();
		$(".hide",cntdiv).hide();
		//editInvestor.scroll(cntdiv);
	}
	,editInfo: function (that) {
		var cntdiv=$(that).parents(".editinfo:first").get(0);
		$(".show",cntdiv).hide();
		$(".hide",cntdiv).show();
		//editInvestor.scroll(cntdiv);;
	}
	,reset: function (targetId) {
		var target=$("#"+targetId);
		$(":input[type='text']",target).val("");
		$(":input[type='hidden']",target).val("0");
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
	,onDLSubmit: function (p) {
		p.params=new Array();
		p.params[p.params.length]={ "name": "isGP","value": true };
		p.params[p.params.length]={ "name": "companyId","value": $("#SearchCompanyID").val() };
		return true;
	}
	,edit: function (img,className,className2) {
		var box=$(img).parents(".section-det:first");
		var editbox=$(className+":first",box);
		$(".show",editbox).hide();
		$(".hide",editbox).show();
		editbox=$(className2+":first",box);
		$(".show",editbox).hide();
		$(".hide",editbox).show();
	}
	,cancelEdit: function (img,className,className2) {
		var box=$(img).parents(".section-det:first");
		var editbox=$(className+":first",box);
		$(".show",editbox).show();
		$(".hide",editbox).hide();
		editbox=$(className2+":first",box);
		$(".show",editbox).show();
		$(".hide",editbox).hide();
	}
	,cancel: function (img) {
		$(img).parents("tr:first").remove();
	}
	,addUF: function (box,issuerId) {
		var t=$("#UnderlyingFundList",box);
		var trid="Edit_0";
		$("#"+trid,t).remove();
		var firstRow=$("tbody tr:first",t);
		var editTR=document.createElement("tr");
		var td=document.createElement("td");
		td.colSpan=$("thead tr:first th",t).length;
		td.className="edit-cell";
		$(td).html("<center>"+jHelper.loadingHTML()+"</center>");
		editTR.className="un-directedit-box";
		editTR.id=trid;
		$(editTR).append(td);
		if(firstRow.get(0)) {
			$(firstRow).before(editTR);
		} else {
			$("tbody",t).before(editTR);
		}
		var data={ id: 0 };
		$(td).empty();
		$("#SectionTemplate").tmpl(data).appendTo(td);
		underlyingFund.open(0,issuerId,td);
	}
}
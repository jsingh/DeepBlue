﻿var dealDirect={
	newEquityData: null
	,newFixedIncomeData: null
	,init: function () {
		$(document).ready(function () {
			jHelper.waterMark();
			dealDirect.onCreateNewIssuer=function (id) {
				//dealDirect.load(id);
				$("#DirectList").flexReload();
			}
			dealDirect.onAddIssuer=function () {
				$("#Name","#NewIssuerDetail").focus();
			}
			if($("#Mode").val()=="direct") {
				dealDirect.add();
			}
		});
	}
	,onCreateNewIssuer: null
	,onAddIssuer: null
	,isUnderlyingFundModel: false
	,setUpNewIssuer: function () {
		$("#I_Country","#frmAddNewIssuer")
			.blur(function () { if($.trim(this.value)=="") { $("#CountryId","#frmAddNewIssuer").val(0); } })
			.autocomplete({ source: "/Admin/FindCountrys",minLength: 1
			,select: function (event,ui) {
				$("#CountryId","#frmAddNewIssuer").val(ui.item.id);
			},appendTo: "body",delay: 300
			});
	}
	,setUpIssuer: function (box) {
		$("#I_Country",box)
		.blur(function () { if($.trim(this.value)=="") { $("#CountryId",box).val(0); } })
		.autocomplete({ source: "/Admin/FindCountrys",minLength: 1
			,select: function (event,ui) {
				$("#CountryId",box).val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$("#EquityIndustry",box)
		.blur(function () { if($.trim(this.value)=="") { $("#EquityIndustryId",box).val(0); } })
		.autocomplete({ source: "/Admin/FindIndustrys",minLength: 1
			,select: function (event,ui) {
				$("#EquityIndustryId",box).val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$("#FixedIncomeIndustry",box)
		.blur(function () { if($.trim(this.value)=="") { $("#FixedIncomeIndustryId",box).val(0); } })
		.autocomplete({ source: "/Admin/FindIndustrys",minLength: 1
			,select: function (event,ui) {
				$("#FixedIncomeIndustryId",box).val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$(".datefield",box).each(function () {
			$(this).datepicker({ changeMonth: true,changeYear: true });
		});
	}
	,save: function (frm) {
		try {
			var loading=$("#SpnSaveIssuerLoading",frm);
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.ajaxFileUpload(
				{
					url: '/Deal/UpdateIssuer',
					secureuri: false,
					formId: frm.id,
					dataType: 'json',
					success: function (data,status) {
						loading.empty();
						var arr=data.data.split("||");
						if(arr[0]=="True") {
							jAlert("Underlying Direct Added.");
							var tr=$(frm).parents("tr:first");
							var t=$(frm).parents("#UnderlyingDirectList:first");
							$(tr).remove();
							$(t).flexReload();
						} else {
							jAlert(data.data);
						}
					}
					,error: function (data,status,e) {
						loading.empty();
						jAlert(data.msg+","+status+","+e);
					}
				});
		} catch(e) { jAlert(e); }
		return false;
	}
	,loadSelectImages: function (isSelect) {
		var addCompany=$("#AddCompany");
		var addGP=$("#AddGP");
		var src="/Assets/images/";
		var addCSRC=src+"addcompany.png";
		var addGPSRC=src+"addgp.png";
		if(isSelect) {
			addCSRC=src+"addcompanyselect.png";
			addGPSRC=src+"addgpselect.png";
		}
		addCompany.attr("src",addCSRC);
		addGP.attr("src",addGPSRC);
	}
	,createNewIssuer: function (frm) {
		try {
			var loading=$("#SpnNewLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Deal/CreateIssuer",$(frm).serializeForm(),function (data) {
				loading.empty();
				var arr=data.split("||");
				if(arr[0]=="True") {
					if(dealDirect.isUnderlyingFundModel) {
						jAlert("GP saved");
					} else {
						jAlert("Company Saved.");
					}
					$("#AddNewIssuer").hide();
					$("#AddGP").removeClass("green-btn-sel");
					dealDirect.loadSelectImages(false);
					if(dealDirect.onCreateNewIssuer) {
						dealDirect.onCreateNewIssuer(arr[1]);
					}
				} else { jAlert(data); }
			});
		} catch(e) { jAlert(e); }
		return false;
	}
	,add: function () {
		dealDirect.loadSelectImages(true);
		var addIssuer=$("#AddNewIssuer");
		addIssuer.show();
		var newIssuerDetail=$("#NewIssuerDetail");newIssuerDetail.empty();
		var data={ "IssuerId": 0,"CountryId": 225,"Country": "United States" };
		data.IsUnderlyingFundModel=dealDirect.isUnderlyingFundModel;
		$("#IssuerDetailTemplate").tmpl(data).appendTo(newIssuerDetail);
		jHelper.applyDatePicker(newIssuerDetail);
		dealDirect.setUpNewIssuer();
		$("#AddGP").addClass("green-btn-sel");
		if(dealDirect.onAddIssuer)
			dealDirect.onAddIssuer();
	}
	,close: function () {
		$('#AddNewIssuer').hide();
		$("#AddGP").removeClass("green-btn-sel");
		dealDirect.loadSelectImages(false);
	}
	,searchCompany: function (id) {
		$("#SearchCompanyID").val(id);
		var grid=$("#DirectList");
		grid.flexReload();
	}
	,load: function (id,directName) {
		var addNewFund=$("#DirectDetailBox");
		var addNewFundTab;
		addNewFundTab=$("#TabDirectGrid");
		var data={ id: id,DirectName: directName };
		var tab=$("#Tab"+id);
		var editbox=$("#Edit"+id);
		if(tab.get(0)) {
			addNewFundTab.after(tab);
			addNewFund.after(editbox);
		} else {
			$("#DirectListTemplate").tmpl(data).insertAfter(addNewFund);
			$("#TabTemplate").tmpl(data).insertAfter(addNewFundTab);
			editbox=$("#Edit"+id);
			dealDirect.undirectList(id,editbox);
		}
		$(".center",$("#Tab"+id)).click();
	}
	,undirectList: function (id,box) {
		$("#UnderlyingDirectList",box)
		.flexigrid({
			usepager: true
			,useBoxStyle: false
			,url: "/Deal/UnderlyingDirectList"
			,onSubmit: function (p) {
				p.params=null;
				p.params=new Array();
				p.params[p.params.length]={ "name": "companyId","value": id };
				return true;
			}
			,onTemplate: function (tbody,data) {
				$("#UnderlyingDirectListRowTemplate").tmpl(data).appendTo(tbody);
				$("tr",tbody).each(function () {
					var tr=this;
					$("#Edit",this).click(function () {
						dealDirect.editUD(tr,$("#ID",tr).val(),$("#SecurityType",tr).val())
					});
				});
			}
			,rpOptions: [25,50,100,200]
			,rp: 25
			,resizeWidth: false
			,method: "GET"
			,sortname: "Symbol"
			,sortorder: ""
			,autoload: true
			,height: 0
		});
	}
	,open: function (id,box) {
		var issuerDetail=$("#IssuerDetail",box);
		var eqDetail=$("#EQdetail",box);
		var fixedIncome=$("#FixedIncome",box);
		var loading=$("#SpnIssuerLoading",box);
		issuerDetail.empty();
		eqDetail.empty();
		fixedIncome.empty();
		if(id>0) {
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
			$.getJSON("/Deal/FindIssuer",{ "_": (new Date).getTime(),"id": id },function (data) {
				loading.empty();
				$("#IssuerDetailTemplate").tmpl(data.IssuerDetailModel).appendTo(issuerDetail);
				$("#EquityDetailTemplate").tmpl(data.EquityDetailModel).appendTo(eqDetail);
				$("#FixedIncomeDetailTemplate").tmpl(data.FixedIncomeDetailModel).appendTo(fixedIncome);
				jHelper.checkValAttr(eqDetail);
				jHelper.checkValAttr(fixedIncome);
				jHelper.formatDateTxt(fixedIncome);
				jHelper.formatDateTxt(eqDetail);
				jHelper.trimTextArea(fixedIncome);
				jHelper.trimTextArea(eqDetail);
				jHelper.jqComboBox(eqDetail);
				jHelper.jqComboBox(fixedIncome);
				jHelper.applyDatePicker(issuerDetail);
				dealDirect.setUpIssuer(box);
				$("#tblExistingEquity",box).flexigrid({ usepager: true
					,url: "/Deal/DirectEquityList"
					,onRowBound: dealDirect.onEquityRowBound
					,method: "GET"
					,sortname: "EquityID"
					,sortorder: "desc"
					,autoload: false
					,height: 0
					,useBoxStyle: false
				});
			});
		}
	}
	,selectTab: function (type,lnk) {
		var box=$(lnk).parents(".section-det:first");
		var equitytab=$("#equitytab",box);
		var fitab=$("#fitab",box);
		equitytab.removeClass("sel");
		fitab.removeClass("sel");
		var EQ=$("#EQdetail",box);
		var FI=$("#FixedIncome",box);
		EQ.hide();FI.hide();
		switch(type) {
			case "E": EQ.show();equitytab.addClass("sel");
				break;
			case "F": FI.show();fitab.addClass("sel");
				break;
		}
	}
	,deleteTab: function (id,isConfirm) {
		var isRemove=true;
		if(isConfirm) {
			isRemove=confirm("Are you sure you want to remove this company?");
		}
		if(isRemove) {
			$("#Tab"+id).remove();
			$("#Edit"+id).remove();
			$("#tabdel"+id).remove();
			$("#TabDirectGrid").click();
		}
	}
	,selectDirectTab: function (that,detailid) {
		$(".section-tab").removeClass("section-tab-sel");
		$(".section-det").hide();
		var detailbox=$("#"+detailid);
		var AddUDBtn=$("#AddUDBtn");
		var AddCBtn=$("#AddCBtn");
		var SearchCBox=$("#SearchCBox");
		AddUDBtn.hide();
		AddCBtn.hide();
		SearchCBox.hide();
		var add=$("#AddUD",AddUDBtn);
		add.unbind("click");
		if(that.id!="TabDirectGrid") {
			AddUDBtn.show();
			dealDirect.close();
			add.click(function () {
				dealDirect.addUD(detailbox,detailid);
			});
		} else {
			AddCBtn.show();
			SearchCBox.show();
		}
		$(that).addClass("section-tab-sel");
		detailbox.show();
	}
	,tabEquitySelect: function (type) {
		$("#NewEqTab").removeClass("tabselect");
		$("#ExistingEqTab").removeClass("tabselect");
		$("#equitysymboldiv").hide();
		$("#existingEquity").hide();
		switch(type) {
			case "N": $("#NewEqTab").addClass("tabselect");$("#equitysymboldiv").show();break;
			case "E": $("#ExistingEqTab").addClass("tabselect");$("#existingEquity").show();dealDirect.existingEQRefresh();break;
		}
	}
	,existingEQRefresh: function () {
		$("#tblExistingEquity").flexReload();
	}
	,changeUploadType: function (uploadType,target) {
		var FileRow=$("#FileRow","#"+target).get(0);
		var LinkRow=$("#LinkRow","#"+target).get(0);
		if(FileRow&&LinkRow) {
			FileRow.style.display="none";
			LinkRow.style.display="none";
			if(uploadType.value=="1")
				FileRow.style.display="";
			else
				LinkRow.style.display="";
		}
	}
	,copyName: function (txt) {
		var parent=$(txt).parents("#DetailBox:first");
		$("#ParentName",parent).val(txt.value);
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
	,openAMD: function () {
		$("#AnnualMeetingDateList")
		.dialog({ title: "Annual Meeting Dates"
		,modal: true
		,position: 'middle'
		,autoResize: false
		,open: function () {
			$("#MeetingDateList").flexReload();
		}
		});
	}
	,addUD: function (box,id) {
		var t=$("#UnderlyingDirectList",box);
		var trid="Edit_0_"+box.attr("id");
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
		$.getJSON("/Deal/FindIssuer",{ "_": (new Date).getTime(),"id": id.replace("Edit","") },function (data) {
			$(td).empty();
			data.FixedIncomeDetailModel.Documents={};
			$("#SectionTemplate").tmpl(data).appendTo(td);
			jHelper.checkValAttr(td);
			jHelper.jqComboBox(td);
			jHelper.jqCheckBox(td);
			dealDirect.setUpIssuer(td);
			jHelper.applyDatePicker(td);
			$(".show",td).hide()
			$(".hide",td).show();
		});
	}
	,editUD: function (tr,id,type) {
		var trid="Edit_"+id+"_"+type;
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
		var templateName="";
		if(type=="Equity") {
			url="/Deal/FindEquity/"+id;
			templateName="EquityDetailTemplate";
		} else {
			url="/Deal/FindFixedIncome/"+id;
			templateName="FixedIncomeDetailTemplate";
		}
		$.getJSON(url,function (data) {
			$(td).empty();
			$("#"+templateName).tmpl(data).appendTo(td);
			jHelper.checkValAttr(td);
			jHelper.jqComboBox(td);
			jHelper.jqCheckBox(td);
			dealDirect.setUpIssuer(td);
			jHelper.applyDatePicker(td);
		});
	}
	,modifyEquity: function (img) {
		var frm=$(img).parents("#frmEquity:first");
		var tr=$(frm).parents("tr:first");
		var t=$(frm).parents("#UnderlyingDirectList:first");
		var loading=$("#SpnLoading",frm);
		loading.html(jHelper.savingHTML());
		$.post("/Deal/UpdateEquity",$(frm).serializeForm(),function (data) {
			loading.empty();
			if($.trim(data)!="") {
				jAlert(data);
			} else {
				jAlert("Direct Saved");
				$(tr).remove();
				$(t).flexReload();
			}
		});
	}
	,modifyFixedIncome: function (img) {
		var frm=$(img).parents(".frm-fixedincome:first").get(0);
		var tr=$(frm).parents("tr:first");
		var t=$(frm).parents("#UnderlyingDirectList:first");
		var loading=$("#SpnLoading",frm);
		loading.html(jHelper.savingHTML());
		$.ajaxFileUpload(
		{
			url: '/Deal/UpdateFixedIncome',
			secureuri: false,
			formId: frm.id,
			dataType: 'json',
			success: function (data,status) {
				loading.empty();
				if($.trim(data.data)!="") {
					jAlert(data.data);
				} else {
					jAlert("Direct Saved");
					$(tr).remove();
					$(t).flexReload();
				}
			}
			,error: function (data,status,e) {
				loading.empty();
				//jAlert(data.msg+","+status+","+e);
			}
		});
	}
	,edit: function (img) {
		var box=$(img).parents(".section-det:first");
		$(".show",box).hide();
		$(".hide",box).show();
	}
	,cancel: function (img) {
		$(img).parents("tr:first").remove();
	}
	,onSubmit: function (p) {
		p.params=new Array();
		p.params[p.params.length]={ "name": "isGP","value": false };
		p.params[p.params.length]={ "name": "companyId","value": $("#SearchCompanyID").val() };
		return true;
	}
	,deleteDirect: function (img,id,type) {
		var url="";
		if(type=="Equity") {
			url="/Deal/DeleteEquity/";
		} else {
			url="/Deal/DeleteFixedIncome/";
		}
		url+=id;
		if(confirm("Are you sure you want to delete this "+(type)+" ?")) {
			var tr=$(img).parents("tr:first");
			var imgsrc=img.src;
			$(img).attr("src","/Assets/images/ajax.jpg");
			img.src=imgsrc;
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					var t=$(tr).parents("table:first");
					tr.remove();
					jHelper.applyFlexGridClass(t);
				}
			});
		}
	}
}



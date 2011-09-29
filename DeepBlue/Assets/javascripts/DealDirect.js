var dealDirect={
	init: function () {
		jHelper.waterMark();
		dealDirect.onCreateNewIssuer=function (id) {
			dealDirect.load(id);
		}
		dealDirect.onAddIssuer=function () {
			$("#Name","#NewIssuerDetail").focus();
		}
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
							$("#AddNewIssuer").hide();
							$("#S_Issuer").val("");
							dealDirect.loadSelectImages(false);
							dealDirect.deleteTab(arr[1],false);
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
					//if(dealDirect.onCreateNewIssuer) {
					//	dealDirect.onCreateNewIssuer(arr[1]);
					//}
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
			$("#SectionTemplate").tmpl(data).insertAfter(addNewFund);
			$("#TabTemplate").tmpl(data).insertAfter(addNewFundTab);
			editbox=$("#Edit"+id);
			dealDirect.open(id,editbox);
		}
		$(".center",$("#Tab"+id)).click();
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
		var equitytab=$("#equitytab");
		var fitab=$("#fitab");
		equitytab.removeClass("sel");
		fitab.removeClass("sel");
		var EQ=$("#EQdetail");
		var FI=$("#FixedIncome");
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
			isRemove=confirm("Are you sure you want to remove this direct?");
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
		$(that).addClass("section-tab-sel");
		$("#"+detailid).show();
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
}



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
	,setUpIssuer: function () {
		$("#I_Country","#frmIssuer")
		.blur(function () { if($.trim(this.value)=="") { $("#CountryId","#frmIssuer").val(0); } })
		.autocomplete({ source: "/Admin/FindCountrys",minLength: 1
			,select: function (event,ui) {
				$("#CountryId","#frmIssuer").val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$("#EquityIndustry","#frmIssuer")
		.blur(function () { if($.trim(this.value)=="") { $("#EquityIndustryId","#frmIssuer").val(0); } })
		.autocomplete({ source: "/Admin/FindIndustrys",minLength: 1
			,select: function (event,ui) {
				$("#EquityIndustryId","#frmIssuer").val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$("#FixedIncomeIndustry","#frmIssuer")
		.blur(function () { if($.trim(this.value)=="") { $("#FixedIncomeIndustryId","#frmIssuer").val(0); } })
		.autocomplete({ source: "/Admin/FindIndustrys",minLength: 1
			,select: function (event,ui) {
				$("#FixedIncomeIndustryId","#frmIssuer").val(ui.item.id);
			},appendTo: "body",delay: 300
		});
		$(".datefield","#DirectMain").each(function () {
			$(this).datepicker({ changeMonth: true,changeYear: true });
		});
	}
	,save: function (frm) {
		try {
			var loading=$("#SpnSaveIssuerLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.ajaxFileUpload(
				{
					url: '/Deal/UpdateIssuer',
					secureuri: false,
					formId: 'frmIssuer',
					dataType: 'json',
					success: function (data,status) {
						loading.empty();
						var arr=data.data.split("||");
						if(arr[0]=="True") {
							alert("Underlying Direct Added.");
							$("#DirectMain").hide();
							$("#AddNewIssuer").hide();
							$("#S_Issuer").val("");
							dealDirect.loadSelectImages(false);
						} else {
							alert(data.data);
						}
					}
					,error: function (data,status,e) {
						loading.empty();
						alert(data.msg+","+status+","+e);
					}
				});

			/*$.post("/Deal/UpdateIssuer",$(frm).serializeArray(),function (data) {
			loading.empty();
			var arr=data.split("||");
			if(arr[0]=="True") {
			alert("Underlying Direct Added.");
			$("#DirectMain").hide();
			$("#AddNewIssuer").hide();
			$("#S_Issuer").val("");
			} else { alert(data); }
			}); */
		} catch(e) { alert(e); }
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
			$.post("/Deal/CreateIssuer",$(frm).serializeArray(),function (data) {
				loading.empty();
				var arr=data.split("||");
				if(arr[0]=="True") {
					if(dealDirect.isUnderlyingFundModel) {
						alert("GP saved");
					} else {
						alert("Company Saved.");
					}
					$("#AddNewIssuer").hide();
					dealDirect.loadSelectImages(false);
					if(dealDirect.onCreateNewIssuer) {
						dealDirect.onCreateNewIssuer(arr[1]);
					}
				} else { alert(data); }
			});
		} catch(e) { alert(e); }
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
		dealDirect.setUpNewIssuer();
		if(dealDirect.onAddIssuer)
			dealDirect.onAddIssuer();
	}
	,close: function () {
		$('#AddNewIssuer').hide();
		dealDirect.loadSelectImages(false);
	}
	,load: function (id) {
		var issuerDetail=$("#IssuerDetail");
		var eqDetail=$("#EQdetail");
		var fixedIncome=$("#FixedIncome");
		var loading=$("#SpnIssuerLoading");
		$("#DirectMain").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		issuerDetail.empty();eqDetail.empty();fixedIncome.empty();
		$.getJSON("/Deal/FindIssuer",{ "_": (new Date).getTime(),"id": id },function (data) {
			$("#DirectMain").show();
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
			dealDirect.setUpIssuer();
			$("#tblExistingEquity").flexigrid({ usepager: true
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
		FileRow.style.display="none";
		LinkRow.style.display="none";
		if(uploadType.value=="1")
			FileRow.style.display="";
		else
			LinkRow.style.display="";
	}
	,copyName: function (txt) {
		$("#ParentName").val(txt.value);
	}
}



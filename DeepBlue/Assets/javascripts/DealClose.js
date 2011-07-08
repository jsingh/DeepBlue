var dealClose={
	init: function () {
		jHelper.resizeIframe();
		$("document").ready(function () {
			jHelper.waterMark();
			dealClose.expand();
		});
	}
	,checkInputBox: function (table) {
		$("tr",table).each(function () {
			var chk=$("input:checkbox",this).get(0);
			if(chk) {
				if(chk.checked) {
					$(":input:text",this).removeAttr("disabled");
				}
			}
		});
	}
	,checkDeal: function (chk) {
		var tr=$(chk).parents("tr:first");
		$(":input:text",tr).each(function () {
			this.disabled=!chk.checked;
		});
	}
	,selectDeal: function (id) {
		dealClose.setDealId(id);
		dealClose.loadDeal();
	}
	,loadDeal: function () {
		var id=dealClose.getDealId();
		$("#SpnLoading").show();
		$("#DealCloseMain").hide();
		$("#LoadingDetail").show();
		$("#FinalDealClose").hide();
		$("#NewDealClose").hide();
		$("#NewDealCloseBtn").hide();
		$("tbody","#DealUnderlyingFundList").empty();
		$("tbody","#DealUnderlyingDirects").empty();
		$("tbody","#FinalDealUnderlyingFundList").empty();
		$("tbody","#FinalDealUnderlyingDirects").empty();
		$.getJSON("/Deal/GetDealDetail/"+id+"?_"+(new Date()).getTime(),function (data) {
			$("#LoadingDetail").hide();
			$("#DealCloseMain").show();
			$("#SpnLoading").hide();

			$("#SpnFundName").html(data.FundName);
			$("#SpnDealNo").html(data.DealNumber);
			$("#SpnDealName").html(data.DealName);
			$("#FundId").val(data.FundId);
			$("#ExistingDealClosing").hide();
			$("#SpnDCTitle").html("New Deal Close");
			$("#SpnDCTitlelbl").html("New Deal Close");
			$("#NDDetail").hide();

			dealClose.onGridSubmit();
			var totalNotClosing=data.TotalUnderlyingFundNotClosing+data.TotalUnderlyingDirectNotClosing;
			var totalClosing=data.TotalUnderlyingFundClosing+data.TotalUnderlyingDirectClosing;
			if(data.TotalDealClosing>0) {
				$("#ExistingDealClosing").show();
			}
			if(totalNotClosing>0) {
				$("#NewDealClose").show();
				$("#NewDealCloseBtn").show();
				$("#NDHeaderBox").click();
				$("#NDDetail").hide();
			}
			else if(totalNotClosing==0&&totalClosing>0) {
				$("#FDHeaderBox").click();
				dealClose.loadFinalDealClose();
			}
		});
	}
	,getDealId: function () { return $("#DealId").val(); }
	,setDealId: function (id) { $("#DealId").val(id); }
	,onGridSubmit: function () {
		var param=[{ name: "dealId",value: dealClose.getDealId()}];
		var grid=$("#DealCloseList");
		grid.ajaxTableOptions({ params: param });
		grid.ajaxTableReload();
	}
	,add: function (id) {
		$("#SpnGridLoading").show();
		var dealId=parseInt(dealClose.getDealId());
		var newDealClose=$("#NewDealClose");
		var finalDealClose=$("#FinalDealClose");
		newDealClose.hide();
		finalDealClose.hide();
		newDealClose.show();
		$("#LoadingDetail").show();
		if(dealId>0) {
			$.getJSON("/Deal/GetDealCloseDetails",
			{ "_": (new Date).getTime(),"id": id,"dealId": dealId }
			,function (data) {
				$("#LoadingDetail").hide();
				$("#SpnGridLoading").hide();
				if(id>0) {
					finalDealClose.show();
					$("#SpnDCTitle").html("Edit Deal Close");
					$("#SpnDCTitlelbl").html("Edit Deal Close");
				} else {
					$("#NDDetail").show();
					$("#SpnDCTitle").html("New Deal Close");
					$("#SpnDCTitlelbl").html("New Deal Close");
				}
				$("#NDHeaderBox").click();
				$("#DealClosingId").val(data.DealClosingId);
				$("#DealNumber").val(data.DealNumber);

				$("#SpnDealCloseNo").html("Deal Close "+data.DealNumber);
				if(data.CloseDate!=null) {
					var d=jHelper.formatDate(jHelper.parseJSONDate(data.CloseDate));
					if(d=="01/01/1") { d=""; }
					$("#New_CloseDate","#frmDealClose").val(d);
					$("#Final_CloseDate","#frmFinalDealClose").val(d);
				} else {
					$("#New_CloseDate","#frmDealClose").val("");
					$("#Final_CloseDate","#frmFinalDealClose").val("");
				}
				var tblduflist=$("#DealUnderlyingFundList");
				dealClose.clearTable(tblduflist);
				$("#DUFundsTemplate").tmpl(data).appendTo(tblduflist);
				jHelper.formatDollar(tblduflist);

				var tbldirectlist=$("#DealUnderlyingDirects");
				dealClose.clearTable(tbldirectlist);
				$("#DUDirectsTemplate").tmpl(data).appendTo(tbldirectlist);
				jHelper.formatDollar(tbldirectlist);

				dealClose.checkDealCloseId(tblduflist);
				dealClose.checkDealCloseId(tbldirectlist);

			});
		} else {
			alert("Deal is required");
			$("#Deal").focus();
		}
	}
	,addRowClass: function (tbl) {
		$("tbody tr:odd",tbl).addClass("arow");
		$("tbody tr:even",tbl).addClass("row");
	}
	,clearTable: function (tbl) {
		$("tbody",tbl).empty();$("tfoot",tbl).empty();
	}
	,checkDealCloseId: function (tbl) {
		$("tbody tr",tbl).each(function () {
			var dcid=parseInt($("#DealClosingId",this).val());
			if(dcid>0) {
				var chk=$("#chk",this).get(0);if(chk) { chk.checked=true; }
			}
		});
	}
	,calcCloseUF: function () {
		var tbl=$("#DealUnderlyingFundList");
		var totalGPP=0;var totalPRCC=0;var totalPRCD=0;var totalNPP=0;
		$("tbody tr",tbl).each(function () {
			var gpp=parseFloat($("#GrossPurchasePrice",this).val());
			var prcc=parseFloat($("#PostRecordDateCapitalCall",this).val());
			var prcd=parseFloat($("#PostRecordDateDistribution",this).val());
			if(isNaN(gpp)) { gpp=0; } if(isNaN(prcc)) { prcc=0; } if(isNaN(prcd)) { prcd=0; }
			var npp=gpp+(prcc-prcd);
			totalGPP=totalGPP+gpp;
			totalPRCC=totalPRCC+prcc;
			totalPRCD=totalPRCD+prcd;
			$("#SpnNPP",this).html(jHelper.dollarAmount(npp.toString()));
		});
		totalNPP=totalGPP+(totalPRCC-totalPRCD);
		$("tfoot tr:first",tbl).each(function () {
			$("#SpnTotalGPP",this).html(jHelper.dollarAmount(totalGPP.toString()));
			$("#SpnTotalPRCC",this).html(jHelper.dollarAmount(totalPRCC.toString()));
			$("#SpnTotalPRCD",this).html(jHelper.dollarAmount(totalPRCD.toString()));
			$("#SpnTotalNPP",this).html(jHelper.dollarAmount(totalNPP.toString()));
		});
	}
	,calcCloseUD: function () {
		var tbl=$("#DealUnderlyingDirects");
		var totalNOS=0;var totalPrice=0;var totalFMV=0;
		$("tbody tr",tbl).each(function () {
			var nos=parseInt($("#NumberOfShares",this).val());
			var price=parseFloat($("#PurchasePrice",this).val());
			var fmv=parseFloat($("#FMV",this).val());
			if(isNaN(nos)) { nos=0; } if(isNaN(price)) { price=0; } if(isNaN(fmv)) { fmv=0; }
			totalNOS+=nos;
			totalPrice+=price;
			totalFMV+=fmv;
		});
		$("tfoot tr:first",tbl).each(function () {
			$("#SpnTotalNoOfShares",this).html(jHelper.dollarAmount(totalNOS.toString()));
			$("#SpnTotalPurchasePrice",this).html(jHelper.dollarAmount(totalPrice.toString()));
			$("#SpnTotalFMV",this).html(jHelper.dollarAmount(totalFMV.toString()));
		});
	}
	,calcFlinalCloseUF: function () {
		var tbl=$("#FinalDealUnderlyingFundList");
		var totalGPP=0;var totalPRCC=0;var totalPRCD=0;var totalNPP=0;
		$("tbody tr",tbl).each(function () {
			var gpp=parseFloat($("#ReassignedGPP",this).val());
			var prcc=parseFloat($("#PostRecordDateCapitalCall",this).val());
			var prcd=parseFloat($("#PostRecordDateDistribution",this).val());
			if(isNaN(gpp)) { gpp=0; } if(isNaN(prcc)) { prcc=0; } if(isNaN(prcd)) { prcd=0; }
			var ajc=gpp+(prcc-prcd);
			totalGPP=totalGPP+gpp;
			totalPRCC=totalPRCC+prcc;
			totalPRCD=totalPRCD+prcd;
			$("#SpnAJC",this).html(jHelper.dollarAmount(ajc.toString()));
		});
		totalNPP=totalGPP+(totalPRCC-totalPRCD);
		$("tfoot tr:first",tbl).each(function () {
			$("#SpnTotalGPP",this).html(jHelper.dollarAmount(totalGPP.toString()));
			$("#SpnTotalPRCC",this).html(jHelper.dollarAmount(totalPRCC.toString()));
			$("#SpnTotalPRCD",this).html(jHelper.dollarAmount(totalPRCD.toString()));
			$("#SpnTotalAJC",this).html(jHelper.dollarAmount(totalNPP.toString()));
		});
	}
	,calcFlinalCloseUD: function () {
		var tbl=$("#FinalDealUnderlyingDirects");
		var totalNOS=0;var totalPrice=0;var totalFMV=0;
		$("tbody tr",tbl).each(function () {
			var nos=parseInt($("#NumberOfShares",this).val());
			var price=parseFloat($("#PurchasePrice",this).val());
			var fmv=parseFloat($("#FMV",this).val());
			if(isNaN(nos)) { nos=0; } if(isNaN(price)) { price=0; } if(isNaN(fmv)) { fmv=0; }
			totalNOS+=nos;
			totalPrice+=price;
			totalFMV+=fmv;
		});
		$("tfoot tr:first",tbl).each(function () {
			$("#SpnTotalNoOfShares",this).html(jHelper.dollarAmount(totalNOS.toString()));
			$("#SpnTotalPurchasePrice",this).html(jHelper.dollarAmount(totalPrice.toString()));
			$("#SpnTotalFMV",this).html(jHelper.dollarAmount(totalFMV.toString()));
		});
	}
	,editRow: function (img) {
		var tr=$(img).parents("tr:first");var isShow=false;
		var chk=$(":input[type='checkbox']",tr).get(0);
		if(chk) { chk.checked=true; }
		if(img.src.indexOf('add.png')> -1) {
			isShow=true;img.src="/Assets/images/Edit.png";
		} else {
			// img.src="/Assets/images/tick.png";
		}
		this.showElements(tr,isShow);
	}
	,editChkRow: function (chk) {
		this.showElements($(chk).parents("tr:first"),!chk.checked);
	}
	,showElements: function (tr,isShow) {
		if(isShow==false) {
			$(".hide",tr).css("display","block");
			$(".show",tr).css("display","none");
		} else {
			$(".hide",tr).css("display","none");
			$(".show",tr).css("display","block");
		}
	}
	,saveDealClose: function (loadingId) {
		var loading=$("#"+loadingId);
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		var param=$("#frmDealClose").serializeArray();
		$.post("/Deal/UpdateDealClosing",param,function (data) {
			loading.empty();
			var arr=data.split("||");
			if(arr[0]=="True") {
				var dcid=parseInt($("#DealClosingId").val());
				if(isNaN(dcid)) { dcid=0; }
				if(dcid>0) {
					alert("Deal Close Saved");
				} else {
					alert("New Deal Close Saved");
				}
				$("#ExistingDealClosing").show();
				dealClose.loadDeal();
			} else { alert(data); }
		});
	}
	,saveFinalDealClose: function (loadingId) {
		var loading=$("#"+loadingId);
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		var param=$("#frmFinalDealClose").serializeArray();
		param[param.length]={ name: "IsFinalClose",value: "true" };
		param[param.length]={ name: "DealId",value: dealClose.getDealId() };
		param[param.length]={ name: "DealNumber",value: $("#DealNumber").val() };
		param[param.length]={ name: "DealClosingId",value: $("#DealClosingId").val() };
		param[param.length]={ name: "FundId",value: $("#FundId").val() };
		$.post("/Deal/UpdateFinalDealClosing",param,function (data) {
			loading.empty();
			if($.trim(data)!="") {
				alert(data);
			} else {
				alert("Final Deal Close Saved");
				$("#ExistingDealClosing").show();
				dealClose.loadDeal();
			}
		});
	}
	,resetForm: function () {
		$("#NewDealClose").hide();
		$("#FinalDealClose").hide();
		$("#SpnDCTitle").html("New Deal Close");
		$("#SpnDCTitlelbl").html("New Deal Close");
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId,false);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#DealCloseList").ajaxTableReload();
		}
	}
	,onCreateBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onGridSuccess: function (t) {
		$("tbody tr",t).each(function () {
			$("td:last",this).html("<img id='Edit' src='/Assets/images/Edit.png'/>");
		});
	}
	,onRowClick: function (row) {
		dealClose.add(row.cell[0]);
	}
	,onCreateSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="") {
			alert(UpdateTargetId.html())
		} else {
			parent.dealClose.closeDialog(true);
		}
	}
	,finalClose: function (chk) {
		var tbl=$("#tblDealUnderlyingFund");
		var thCommitmentAmount=$("#thCommitmentAmount",tbl);
		var thGPP=$("#thGPP",tbl);
		var thReassignedGPP=$("#thReassignedGPP",tbl);
		var display="";var disabled=!chk.checked;
		if(chk.checked) { display="none"; } else { display=""; }
		this.hideColumn(thCommitmentAmount,tbl,display);
		this.hideColumn(thGPP,tbl,display);
		if(display=="none") { display=""; } else { display="none"; }
		this.hideColumn(thReassignedGPP,tbl,display);
	}
	,hideColumn: function (th,tbl,display) {
		th.css("display",display);
		var colindex=$("th",tbl).index(th);
		$("tbody tr",tbl).each(function () {
			$("td:eq("+colindex+")",this).css("display",display);
		});
	}
	,calcAmount: function () {
		var totRGPP=0;
		var totPRCC=0;
		var totPRD=0;
		$("tbody tr","#tblDealUnderlyingFund").each(function () {
			var rgpp=parseFloat($("#ReassignedGPP",this).val());
			var prcc=parseFloat($("#PostRecordDateCapitalCall",this).val());
			var prd=parseFloat($("#PostRecordDateDistribution",this).val());
			if(isNaN(rgpp)) { rgpp=0; } if(isNaN(prcc)) { prcc=0; } if(isNaN(prd)) { prd=0; }
			totRGPP+=rgpp;
			totPRCC+=prcc;
			totPRD+=prd;
		});
		$("#SpnRGPP").html("$"+totRGPP);$("#SpnPRCC").html("$"+totPRCC);$("#SpnPRCD").html("$"+totPRD);
	}
	,expand: function () {
		$(".headerbox").click(function () {
			$(".headerbox").show();
			$(".expandheader").hide();
			$(".detail").hide();
			$(this).hide();
			var actbox=$(this).parents(".act-box:first");
			actbox.show();
			var parent=$(this).parent();

			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
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
	,loadFinalDealClose: function () {
		$("#LoadingDetail").show();
		$("#FinalDealClose").show();
		$.getJSON("/Deal/GetFianlDealCloseDetails?_="+(new Date()).getTime()+"&dealClosingId=0&dealId="+dealClose.getDealId(),function (data) {
			$("#LoadingDetail").hide();

			var finaltblduflist=$("#FinalDealUnderlyingFundList");
			dealClose.clearTable(finaltblduflist);
			$("#FinalDUFundsTemplate").tmpl(data).appendTo(finaltblduflist);
			jHelper.formatDollar(finaltblduflist);

			var finaltbldirectlist=$("#FinalDealUnderlyingDirects");
			dealClose.clearTable(finaltbldirectlist);
			$("#FinalDUDirectsTemplate").tmpl(data).appendTo(finaltbldirectlist);
			jHelper.formatDollar(finaltbldirectlist);

			dealClose.addRowClass(finaltblduflist);
			dealClose.addRowClass(finaltbldirectlist);
		});
	}
}
var dealClose={
	init: function () {
		jHelper.resizeIframe();
		$("document").ready(function () {
			jHelper.waterMark();
			dealClose.expand();
		});
	}
	,selectDeal: function (id) {
		dealClose.setDealId(id);
		dealClose.loadDeal();
	}
	,loadDeal: function () {
		var id=dealClose.getDealId();
		var dealCloseMain=$("#DealCloseMain");
		dealCloseMain.hide();
		if(id>0) {
			dealCloseMain.show();
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
				$("#Deal").val(data.DealName);
				$("#FundId").val(data.FundId);
				$("#ExistingDealClosing").hide();
				$("#SpnDCTitle").html("New Deal Close");
				$("#SpnDCTitlelbl").html("New Deal Close");
				$("#NDDetail").hide();

				dealClose.loadDealCloseList();

				var totalNotClosing=data.TotalUnderlyingFundNotClosing+data.TotalUnderlyingDirectNotClosing;
				var totalClosing=data.TotalUnderlyingFundClosing+data.TotalUnderlyingDirectClosing;
				if(data.TotalDealClosing>0) {
					$("#ExistingDealClosing").show();
				}
				dealClose.add(0);
				dealClose.loadFinalDealClose();
			});
		}
	}
	,getDealId: function () { return $("#DealId").val(); }
	,setDealId: function (id) { $("#DealId").val(id); }
	,loadDealCloseList: function () {
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
		var tblduflist=$("#DealUnderlyingFundList");
		var tbldirectlist=$("#DealUnderlyingDirects");
		//finalDealClose.show();
		newDealClose.show();
		$("#LoadingDetail").show();
		dealClose.clearTable(tblduflist);
		dealClose.clearTable(tbldirectlist);
		if(dealId>0) {
			$.getJSON("/Deal/GetDealCloseDetails",
			{ "_": (new Date).getTime(),"id": id,"dealId": dealId }
			,function (data) {
				$("#LoadingDetail").hide();
				$("#SpnGridLoading").hide();
				if(id>0) {
					$("#SpnDCTitle").html("Edit Deal Close");
					$("#SpnDCTitlelbl").html("Edit Deal Close");
				} else {
					$("#NDDetail").show();
					$("#SpnDCTitle").html("New Deal Close");
					$("#SpnDCTitlelbl").html("New Deal Close");
				}

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

				dealClose.clearTable(tblduflist);
				$("#DUFundsTemplate").tmpl(data).appendTo(tblduflist);
				jHelper.formatDollar(tblduflist,true);

				dealClose.clearTable(tbldirectlist);
				$("#DUDirectsTemplate").tmpl(data).appendTo(tbldirectlist);
				jHelper.formatDollar(tbldirectlist,true);

				jHelper.checkValAttr(tblduflist);
				jHelper.checkValAttr(tbldirectlist);
				dealClose.setAutoComplete(tblduflist);
				dealClose.setAutoComplete(tbldirectlist);
				dealClose.calcCloseUF();
				dealClose.calcCloseUD();
			});
		} else {
			jAlert("Deal is required");
			$("#Deal").focus();
		}
	}
	,clearTable: function (tbl) {
		$("tbody",tbl).empty();
		$("tfoot",tbl).empty();
	}
	,calcCloseUF: function () {
		var tbl=$("#DealUnderlyingFundList");
		var totalCA=0;var totalGPP=0;var totalPRCC=0;var totalPRCD=0;var totalNPP=0;var totalUFA=0;
		$("tbody tr",tbl).each(function () {
			var gpp=parseFloat($("#GrossPurchasePrice",this).val());
			var prcc=parseFloat($("#PostRecordDateCapitalCall",this).val());
			var prcd=parseFloat($("#PostRecordDateDistribution",this).val());
			var ufa=parseFloat($("#UnfundedAmount",this).val());
			var ca=parseFloat($("#CommittedAmount",this).val());
			if(isNaN(gpp)) { gpp=0; } if(isNaN(prcc)) { prcc=0; } if(isNaN(prcd)) { prcd=0; }
			if(isNaN(ufa)) { ufa=0; } if(isNaN(ca)) { ca=0; }
			var npp=(gpp+(prcc-prcd));
			$("#SpnNPP",this).html(jHelper.dollarAmount(npp.toString()));
			$("#SpnDCUFA",this).html(jHelper.dollarAmount(ufa.toString()));
			totalGPP=totalGPP+gpp;
			totalPRCC=totalPRCC+prcc;
			totalPRCD=totalPRCD+prcd;
			totalNPP=totalNPP+npp;
			totalUFA=totalUFA+ufa;
			totalCA=totalCA+ca;
		});
		$("tfoot tr:first",tbl).each(function () {
			$("#SpnTotalGPP",this).html(jHelper.dollarAmount(totalGPP.toString()));
			$("#SpnTotalPRCC",this).html(jHelper.dollarAmount(totalPRCC.toString()));
			$("#SpnTotalPRCD",this).html(jHelper.dollarAmount(totalPRCD.toString()));
			$("#SpnTotalNPP",this).html(jHelper.dollarAmount(totalNPP.toString()));
			$("#SpnTotalUFA",this).html(jHelper.dollarAmount(totalUFA.toString()));
			$("#SpnTotalCA",this).html(jHelper.dollarAmount(totalCA.toString()));
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
	,calcFinalCloseUF: function () {
		var tbl=$("#FinalDealUnderlyingFundList");
		var totalGPP=0;var totalPRCC=0;var totalPRCD=0;var totalAC=0;
		$("tbody tr",tbl).each(function () {
			var gpp=parseFloat($("#ReassignedGPP",this).val());
			var prcc=parseFloat($("#PostRecordDateCapitalCall",this).val());
			var prcd=parseFloat($("#PostRecordDateDistribution",this).val());
			if(isNaN(gpp)) { gpp=0; } if(isNaN(prcc)) { prcc=0; } if(isNaN(prcd)) { prcd=0; }
			var ac=(gpp+(prcc-prcd));
			$("#SpnAC",this).html(jHelper.dollarAmount(ac.toString()));
			totalGPP=totalGPP+gpp;
			totalPRCC=totalPRCC+prcc;
			totalPRCD=totalPRCD+prcd;
			totalAC=totalAC+ac;
		});
		$("tfoot tr:first",tbl).each(function () {
			$("#SpnTotalGPP",this).html(jHelper.dollarAmount(totalGPP.toString()));
			$("#SpnTotalPRCC",this).html(jHelper.dollarAmount(totalPRCC.toString()));
			$("#SpnTotalPRCD",this).html(jHelper.dollarAmount(totalPRCD.toString()));
			$("#SpnTotalAJC",this).html(jHelper.dollarAmount(totalAC.toString()));
		});
	}
	,calcFinalCloseUD: function () {
		var tbl=$("#FinalDealUnderlyingDirects");
		var totalNOS=0;var totalPrice=0;var totalFMV=0;
		$("tbody tr",tbl).each(function () {
			var nos=parseInt($("#NumberOfShares",this).val());
			var price=parseFloat($("#PurchasePrice",this).val());
			var fmv=parseFloat($("#AdjustedFMV",this).val());
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
		dealClose.showElements(tr,isShow);
	}
	,editChkRow: function (chk) {
		dealClose.showElements($(chk).parents("tr:first"),!chk.checked);
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
		var frm=$("#frmDealClose");
		$("#TotalUnderlyingFunds",frm).val($("tbody tr","#DealUnderlyingFundList").length);
		$("#TotalUnderlyingDirects",frm).val($("tbody tr","#DealUnderlyingDirects").length);
		var param=frm.serializeForm();
		$.post("/Deal/UpdateDealClosing",param,function (data) {
			loading.empty();
			var arr=data.split("||");
			if(arr[0]=="True") {
				var dcid=parseInt($("#DealClosingId").val());
				if(isNaN(dcid)) { dcid=0; }
				if(dcid>0) {
					jAlert("Deal Close Saved");
				} else {
					jAlert("New Deal Close Saved");
				}
				$("#ExistingDealClosing").show();
				dealClose.clearTable($("#DealUnderlyingFundList"));
				dealClose.clearTable($("#DealUnderlyingDirects"));
				dealClose.loadDeal();
				dealClose.loadFinalDealClose();
			} else { jAlert(data); }
		});
	}
	,saveFinalDealClose: function (loadingId) {
		var loading=$("#"+loadingId);
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		var frm=$("#frmFinalDealClose");
		$("#TotalUnderlyingFunds",frm).val($("tbody tr","#FinalDealUnderlyingFundList").length);
		$("#TotalUnderlyingDirects",frm).val($("tbody tr","#FinalDealUnderlyingDirects").length);
		var param=frm.serializeForm();
		$.post("/Deal/UpdateFinalDealClosing",param,function (data) {
			loading.empty();
			if($.trim(data)!="") {
				jAlert(data);
			} else {
				jAlert("Final Deal Close Saved");
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
	,onGridSuccess: function (t) {
		$("tbody tr",t).each(function () {
			$("td:last",this).html("<img id='Edit' src='/Assets/images/Edit.png'/>");
		});
	}
	,onRowClick: function (row) {
		dealClose.add(row.cell[0]);
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
			$(".expandaddbtn").hide();
			$(this).hide();
			var actbox=$(this).parents(".act-box:first");
			actbox.show();
			$(".expandaddbtn",actbox).show();
			var parent=$(this).parent();
			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
			detail.show();
		});
		$(".expandheader").click(function () {
			var expandheader=$(this);
			$(".expandaddbtn").hide();
			var parent=$(expandheader).parent();
			expandheader.hide();
			var detail=$(".detail",parent);
			detail.hide();
			$(".headerbox",parent).show();
		});
	}
	,loadFinalDealClose: function () {
		try {
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

				jHelper.applyGridClass($("tbody",finaltblduflist));
				jHelper.applyGridClass($("tbody",finaltbldirectlist));
				dealClose.calcFinalCloseUF();
				dealClose.calcFinalCloseUD();
			});
		} catch(e) { jAlert(e); }
	}
	/* Deal Underlying Fund */
	,addDUF: function (tblid) {
		var item={ "DealUnderlyingFundId": 0,"DealClosingId": 0,"DealId": dealClose.getDealId(),"FundId": $("#FundId").val() }
		var data={ "index": 0,"item": item,"IsFinalClose": false };
		var target=$("#"+tblid);
		dealClose.loadDUF(data,target,data.index);
	}
	,loadDUF: function (data,target,index) {
		var row=$("#row"+index,target);
		if(!row.get(0)) {
			$("#DUFEditTemplate").tmpl(data).prependTo(target);
			row=$("#row"+index,target);
			dealClose.setAutoComplete(row);
			if(index==0) {
				dealClose.showElements(row,false);
			}
			var chk=$("#chk",row).get(0);
			if(chk)
				chk.checked=true;
		}
	}
	,saveDUF: function (img) {
		var oldSrc=img.src;
		img.src="/Assets/images/ajax.jpg";
		var tr=$(img).parents("tr:first");
		var param=jHelper.serialize(tr);
		$.post("/Deal/CreateClosingDealUnderlyingFund",param,function (data) {
			var arr=data.split("||");
			if($.trim(arr[0])!="True") {
				jAlert(arr[0]);
				img.src=oldSrc;
			} else {
				$.getJSON("/Deal/FindDealUnderlyingFund?_"+(new Date).getTime()+"&dealUnderlyingFundId="+arr[1],function (dufitem) {
					var target=$("#DealUnderlyingFundList");
					$("#row0",target).remove();
					var data={ "index": $("tbody tr","#DealUnderlyingFundList").length,"item": dufitem,"IsFinalClose": false };
					data.index++;
					dealClose.loadDUF(data,target,data.index);
				});
			}
		});
	}
	/* End Deal Underlying Fund */
	/* Deal Underlying Direct */
	,addDUD: function (tblid) {
		var item={ "DealUnderlyingDirectId": 0,"DealClosingId": 0,"DealId": dealClose.getDealId(),"FundId": $("#FundId").val() }
		var data={ "index": 0,"item": item,"IsFinalClose": false };
		var target=$("#"+tblid);
		dealClose.loadDUD(data,target,data.index);
	}
	,loadDUD: function (data,target,index) {
		var row=$("#row"+index,target);
		if(!row.get(0)) {
			$("#DUDEditTemplate").tmpl(data).prependTo(target);
			row=$("#row"+index,target);
			dealClose.setAutoComplete(row);
			if(index==0) {
				dealClose.showElements(row,false);
			}
			var chk=$("#chk",row).get(0);
			if(chk)
				chk.checked=true;
		}
	}
	,saveDUD: function (img) {
		var oldSrc=img.src;
		img.src="/Assets/images/ajax.jpg";
		var tr=$(img).parents("tr:first");
		var param=jHelper.serialize(tr);
		$.post("/Deal/CreateClosingDealUnderlyingDirect",param,function (data) {
			var arr=data.split("||");
			if($.trim(arr[0])!="True") {
				jAlert(arr[0]);
				img.src=oldSrc;
			} else {
				$.getJSON("/Deal/FindDealUnderlyingDirect?_"+(new Date).getTime()+"&dealUnderlyingDirectId="+arr[1],function (duditem) {
					var target=$("#DealUnderlyingDirects");
					$("#row0",target).remove();
					var data={ "index": $("tbody tr","#DealUnderlyingDirects").length,"item": duditem,"IsFinalClose": false };
					data.index++;
					dealClose.loadDUD(data,target,data.index);
				});
			}
		});
	}
	/* End Deal Underlying Direct */
	,setAutoComplete: function (target) {
		$(".dealuf",target).each(function () {
			var tr=$(this).parents("tr:first");
			var txt=this;
			$(txt)
			.unbind("blur")
			.blur(function () { if($.trim(txt.value)=="") { $("#UnderlyingFundId",tr).val(0); } })
			.autocomplete({ source: "/Deal/FindUnderlyingFunds"
									,minLength: 1
									,select: function (event,ui) {
										$("#UnderlyingFundId",tr).val(ui.item.id);
									}
									,appendTo: "body",delay: 300
			});
		});
		$(".dealud",target).each(function () {
			var tr=$(this).parents("tr:first");
			var txt=this;
			var issuerId=$("#IssuerId",tr);
			var securityTypeId=$("#SecurityTypeId",tr);
			var securityId=$("#SecurityId",tr);
			$(txt)
				.autocomplete({ source: "/Deal/FindEquityFixedIncomeIssuers",minLength: 1,
					select: function (event,ui) {
						issuerId.val(ui.item.id);
						securityTypeId.val(ui.item.otherid);
						securityId.val(ui.item.otherid2);
					},
					appendTo: "#content",delay: 300
				});
		});
	}
}
$.extend(window,{
	getRow: function (index,item,isFinalClose) { var data={ "index": index,"item": item,"IsFinalClose": isFinalClose };return data; }
	,checkFinalClose: function (isCheck) { var display="none";if(isCheck) { display="block"; } var style="style:display:"+display;return style; }
});
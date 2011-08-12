dealActivity.makeNewSD=function () {
	var ufid=dealActivity.getSDUnderlyingFundId();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadSD(false);
	}
};
dealActivity.expandMSDTree=function (index,img) {
	var display="";
	if(img.src.indexOf('treeplus')>0) {
		display="";
		img.src=img.src.replace("treeplus.gif","treeminus.gif");
	} else {
		display="none";
		img.src=img.src.replace("treeminus.gif","treeplus.gif");
	}
	$("#ManualUFSD_Deal_"+index).css("display",display);
};
dealActivity.setSDUnderlyingFund=function (id,name) {
	$("#SDUnderlyingFundId").val(id);
	$("#SpnSDUFName").html(name);
	$("tbody","#StockDistributionList").empty();
	$("tbody","#PRStockDistributionList").empty();
	$("#SDDetail").attr("issearch","true").show();
	dealActivity.loadSD(true);
};
dealActivity.loadSD=function (isRefresh) {
	var loading=$("#SDLoading");
	var tbl=$("#StockDistributionList");
	$("#StockDistribution").show();
	$("#PRSDListBox").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#StockDistribution").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Deal/UnderlyingFundStockDistributionList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getSDUnderlyingFundId() },function (data) {
			$("#PRStockDistribution").show();
			loading.empty();
			$.each(data,function (i,item) { item["Index"]=i;$("#StockDistributionAddTemplate").tmpl(item).appendTo(target); });
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#StockDistribution").show(); }
			$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
			$(".mcc",target).removeAttr("class");
			$("tr",target).each(function () {
				var issuer=$("#Issuer",this).get(0);
				var underlyingFundId=dealActivity.getSDUnderlyingFundId();
				var fundId=$("#FundId",this).val();
				var index=$("#Index",this).val();
				var row=this;
				if(issuer) {
					$(issuer).autocomplete(
					{
						source: "/Deal/FindStockIssuers?underlyingFundId="+underlyingFundId+"&fundId="+fundId
					,minLength: 1
					,select: function (event,ui) {
						$("#SecurityTypeId",row).val(ui.item.otherid);
						$("#SecurityId",row).val(ui.item.id);
					}
					,appendTo: "body",delay: 300
					}).blur(function () {
						if($.trim(this.value)=="") {
							$("#SecurityTypeId",row).val(0);
							$("#SecurityId",row).val(0);
						}
					});
				}
			});
			dealActivity.showManualSDCtl();
		});
	}
};
dealActivity.getSDUnderlyingFundId=function (id) {
	return parseInt($("#SDUnderlyingFundId").val());
};
dealActivity.submitUFStockDistribution=function (frm) {
	try {
		var param=$(frm).serializeForm();
		var loading=$("#SpnSDSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		var chk=$("#IsManualStockDistribution").get(0);
		var totalRows=($("tbody tr","#StockDistributionList").length);
		var isManual=chk.checked;
		var isError=false;
		/*if(totalRows>0&&isManual) {
		var msg="";var isFocus=false;
		$(".manualcamount").each(function () {
		var amount=parseFloat(this.value);if(isNaN(amount)) { amount=0; }
		if(amount>0) {
		var parentRow=$(this).parents("tr:first");
		var dealname=$(".dealname",parentRow).html();
		var date=$(".manualdate",parentRow).val();
		if($.trim(date)=="") {
		msg+=dealname+" Distribution Date is required\n";
		if(isFocus==false) {
		$(".manualdate",parentRow).focus();
		isFocus=true;
		}
		}
		}
		});
		if(msg!="") {
		alert(msg);
		isError=true;
		loading.empty();
		}
		}*/
		if(isError==false) {
			param[param.length]={ name: "TotalRows",value: ($("tbody tr","#StockDistributionList").length) };
			param[param.length]={ name: "IsManualStockDistribution",value: isManual };
			$.post("/Deal/CreateUnderlyingFundStockDistribution",param,function (data) {
				loading.empty();
				if($.trim(data)!="") { dealActivity.processErrMsg(data,frm); } else {
					alert("Stock Distributions Saved");
					dealActivity.resetStockDistribution();
				}
			});
		}
	} catch(e) { alert(e); }
	return false;
};
dealActivity.resetStockDistribution=function () {
	$("#StockDistribution").hide();
	$("#PRStockDistribution").hide();
	$("tbody","#StockDistributionList").empty();
	$("#SD_UnderlyingFund").val("");
	$("#SpnSDUFName").empty();
	$("#SDUnderlyingFundId").val(0);
	$("#SD_UnderlyingFund").focus();
};
dealActivity.showManualSDCtl=function (parentId) {
	var chk=$("#IsManualStockDistribution").get(0);
	var display="";
	if(chk.checked) { display=""; } else { display="none"; }
	$(".ismanual","#StockDistributionList").css("display",display);
};
deal.loadUnderlyingFundData=function (data) {
	var tbody=$("#tbodyUnderlyingFund");
	var tr=$("#UnderlyingFund_"+data.DealUnderlyingFundId,tbody);
	if(!(tr.get(0))) {
		$("#UnderlyingFundsRowTemplate").tmpl(data).prependTo("#tbodyUnderlyingFund");
	} else {
		$("#UnderlyingFundsRowTemplate").tmpl(data).insertAfter(tr);
		tr.remove();
	}
	tr=$("#UnderlyingFund_"+data.DealUnderlyingFundId,tbody);
	jHelper.formatDollar(tr);
	jHelper.formatDateTxt(tr);
	jHelper.formatDateHtml(tr);
	deal.selectValue(tr);
	jHelper.applyDatePicker(tr);
	jHelper.setUpToolTip(tr);
	deal.setIndex($("#tblUnderlyingFund"));
	$("#MakeNewDUFund").hide();
	deal.applyUFAutocomplete(tr);
	$("tr:odd","#tbodyUnderlyingFund").removeClass("row").removeClass("arow").addClass("arow");
	$("tr:even","#tbodyUnderlyingFund").removeClass("row").removeClass("arow").addClass("row");
};
deal.calcDUF=function () {
	var tbl=$("#tblUnderlyingFund");
	var totalGPP=0;var totalNAV=0;var totalCA=0;var totalUFA=0;
	$("tr",tbl).each(function () {
		var fundNAV=$("#FundNAV",this);
		var ca=$("#CommittedAmount",this);
		var ufa=$("#UnfundedAmount",this);
		var gpp=$("#GrossPurchasePrice",this);
		if(fundNAV.get(0)) {
			var a=parseFloat(fundNAV.val());
			if(isNaN(a)) { a=0; }
			totalNAV+=a;
		}
		if(ca.get(0)) {
			var a=parseFloat(ca.val());
			if(isNaN(a)) { a=0; }
			totalCA+=a;
		}
		if(ufa.get(0)) {
			var a=parseFloat(ufa.val());
			if(isNaN(a)) { a=0; }
			totalUFA+=a;
		}
		if(gpp.get(0)) {
			var a=parseFloat(gpp.val());
			if(isNaN(a)) { a=0; }
			totalGPP+=a;
		}
	});
	$("#SpnTotalFundGPP",tbl).html(jHelper.dollarAmount(totalGPP.toString()));
	$("#SpnTotalFundNAV",tbl).html(jHelper.numberFormat(totalNAV.toString()));
	$("#SpnTotalCAmount",tbl).html(jHelper.dollarAmount(totalCA.toString()));
	$("#SpnTotalUAmount",tbl).html(jHelper.dollarAmount(totalUFA.toString()));
};
deal.applyUFAutocomplete=function (tr) {
	var underlyingFund=$("#UnderlyingFund",tr);
	var underlyingFundId=$("#UnderlyingFundId",tr);
	underlyingFund
	.blur(function () { if($.trim(this.value)=="") { underlyingFundId.val(0); } })
	.autocomplete({ source: "/Deal/FindUnderlyingFunds",minLength: 1,
		select: function (event,ui) {
			underlyingFundId.val(ui.item.id);
			deal.FindFundNAV(ui.item.id,tr);
		}
	,appendTo: "body",delay: 300
	});
};
deal.deleteUnderlyingFund=function (id,img) {
	if(confirm("Are you sure you want to delete this deal underlying fund?")) {
		var tr=$(img).parents("tr:first");
		var url="/Deal/DeleteDealUnderlyingFund/"+id;
		$.get(url,function (data) {
			if($.trim(data)!="") {
				alert(data);
			} else {
				tr.remove();
				deal.setIndex($("#tblUnderlyingFund"));
			}
		});
	}
};
deal.editUnderlyingFund=function (img) {
	var tr=$(img).parents("tr:first");
	if(img.src.indexOf('save.png')> -1) {
		deal.saveUnderlyingFund(tr);
	} else {
		img.src="/Assets/images/save.png";
		deal.showElements(tr);
	}
};
deal.addUnderlyingFund=function (img) {
	var tr=$(img).parents("tr:first");
	deal.saveUnderlyingFund(tr);
};
deal.saveUnderlyingFund=function (tr) {
	var dealId=deal.getDealId();
	var spnAjax=$("#spnAjax",tr).show();
	spnAjax.show();
	if(dealId>0) {
		var param=jHelper.serialize(tr);
		param[param.length]={ name: "DealId",value: deal.getDealId() };
		param[param.length]={ name: "FundId",value: deal.getFundId() };
		var url="/Deal/CreateDealUnderlyingFund";
		$.post(url,param,function (data) {
			spnAjax.hide();
			if(data.indexOf("True||")> -1) {
				var id=data.split("||")[1];
				deal.loadUnderlyingFund(id);
			} else {
				alert(data);
			}
		});
	} else {
		spnAjax.hide();
		deal.onDealSuccess=null;
		deal.onDealSuccess=function () { deal.saveUnderlyingFund(tr); }
		deal.saveDeal();
	}
};
deal.loadUnderlyingFund=function (id) {
	var dt=new Date();
	var url="/Deal/FindDealUnderlyingFund?dealUnderlyingFundId="+id+"&t="+dt.getTime();
	$.getJSON(url,function (data) {
		deal.loadUnderlyingFundData(data);
	});
};
deal.FindFundNAV=function (ufid,tr) {
	var FundNAV=$("#FundNAV",tr);
	FundNAV.attr("readonly","readonly").val("Loading FundNAV...");
	$.get("/Deal/FindFundNAV?_"+(new Date()).getTime()+"&underlyingFundId="+ufid+"&fundId="+deal.getFundId(),function (data) {
		var fundNAV="";
		data=parseFloat(data);
		if(data>0) { fundNAV=data.toFixed(2); }
		FundNAV.removeAttr("readonly","readonly").val(fundNAV);
	});
};
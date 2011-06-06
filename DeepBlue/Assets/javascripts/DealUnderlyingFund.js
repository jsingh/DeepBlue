deal.loadUnderlyingFundData=function (data) {
	var tbody=$("#tbodyUnderlyingFund");
	var tr=$("#UnderlyingFund_"+data.DealUnderlyingFundId,tbody);
	if(!(tr.get(0))) {
		$("#UnderlyingFundsRowTemplate").tmpl(data).prependTo("#tbodyUnderlyingFund");
	} else {
		tr.prev().remove();
		$("#UnderlyingFundsRowTemplate").tmpl(data).insertAfter(tr);
		tr.remove();
	}
	tr=$("#UnderlyingFund_"+data.DealUnderlyingFundId,tbody);
	jHelper.formatDollar(tr);
	jHelper.formatDateTxt(tr);
	jHelper.formatDateHtml(tr);
	deal.selectValue(tr);
	jHelper.applyDatePicker(tr);
	deal.setIndex($("#tblUnderlyingFund"));
	$("#MakeNewDUFund").hide();
};
deal.deleteUnderlyingFund=function (id,img) {
	if(confirm("Are you sure you want to delete this deal underlying fund?")) {
		var tr=$(img).parents("tr:first");
		var url="/Deal/DeleteDealUnderlyingFund/"+id;
		$.get(url,function (data) { tr.prev().remove();tr.remove();deal.setIndex($("#tblUnderlyingFund")); });
	}
};
deal.editUnderlyingFund=function (img) {
	var tr=$(img).parents("tr:first");
	if(img.src.indexOf('tick.png')> -1) {
		deal.saveUnderlyingFund(tr);
	} else {
		img.src="/Assets/images/tick.png";
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
		$("#btnSaveDeal").click();
	}
};
deal.loadUnderlyingFund=function (id) {
	var dt=new Date();
	var url="/Deal/FindDealUnderlyingFund?dealUnderlyingFundId="+id+"&t="+dt.getTime();
	$.getJSON(url,function (data) {
		deal.loadUnderlyingFundData(data);
	});
};
deal.FindFundNAV=function (ddl) {
	var tr=$(ddl).parents("tr:first");
	var FundNAV=$("#FundNAV",tr);
	FundNAV.attr("readonly","readonly").val("Loading FundNAV...");
	$.get("/Deal/FindFundNAV?_"+(new Date()).getTime()+"&underlyingFundId="+ddl.value+"&fundId="+deal.getFundId(),function (data) {
		var fundNAV="";if(parseInt(data)>0) { fundNAV=data; }
		FundNAV.removeAttr("readonly","readonly").val(fundNAV);
	});
};
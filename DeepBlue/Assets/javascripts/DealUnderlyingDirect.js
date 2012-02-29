deal.loadUnderlyingDirectData=function (data) {
	var tbody=$("#tbodyUnderlyingDirect");
	var tr=$("#UnderlyingDirect_"+data.DealUnderlyingDirectId,tbody);
	if(!(tr.get(0))) {
		$("#UnderlyingDirectsRowTemplate").tmpl(data).prependTo("#tbodyUnderlyingDirect");
	} else {
		$("#UnderlyingDirectsRowTemplate").tmpl(data).insertAfter(tr);
		tr.remove();
	}
	tr=$("#UnderlyingDirect_"+data.DealUnderlyingDirectId,tbody);
	var date;
	jHelper.formatDollar(tr);
	date=jHelper.formatDate(jHelper.parseJSONDate(data.RecordDate));
	$("#SpnRecordDate",tr).html(date);
	$(":input[name='RecordDate']",tr).val(date);
	if(data.TaxCostDate!=null) {
		date=jHelper.formatDate(jHelper.parseJSONDate(data.TaxCostDate));
		$("#SpnTaxCostDate",tr).html(date);
		$(":input[name='TaxCostDate']",tr).val(date);
	}
	deal.selectValue(tr);
	jHelper.applyDatePicker(tr);
	deal.setIndex($("#tblUnderlyingDirect"));
	deal.applyUDAutocomplete(tr);
	jHelper.setUpToolTip(tr);
	jHelper.gridEditRow(tr);
	$("#MakeNewDUDirect").hide();
	$("tr:odd","#tbodyUnderlyingDirect").removeClass("row").removeClass("arow").addClass("arow");
	$("tr:even","#tbodyUnderlyingDirect").removeClass("row").removeClass("arow").addClass("row");
	footer.show("tbodyUnderlyingDirect","tfootUnderlyingDirect");
};
deal.calcDUD=function () {
	var tbl=$("#tblUnderlyingDirect");
	var totalNOS=0;var totalPP=0;var totalFMV=0;
	$("tr",tbl).each(function () {
		var nos=$("#NumberOfShares",this);
		var pp=$("#PurchasePrice",this);
		var fmv=$("#FMV",this);
		if(nos.get(0)) {
			var a=jHelper.cFloat(nos.val());
			if(isNaN(a)) { a=0; }
			totalNOS+=a;
		}
		if(pp.get(0)) {
			var a=jHelper.cFloat(pp.val());
			if(isNaN(a)) { a=0; }
			totalPP+=a;
		}
		if(fmv.get(0)) {
			var a=jHelper.cFloat(fmv.val());
			if(isNaN(a)) { a=0; }
			totalFMV+=a;
		}
	});
	$("#SpnTotalNOS",tbl).html(jHelper.numberFormat(totalNOS.toString()));
	$("#SpnTotalPP",tbl).html(jHelper.dollarAmount(totalPP.toString()));
	$("#SpnTotalFMV",tbl).html(jHelper.dollarAmount(totalFMV.toString()));
};
deal.applyUDAutocomplete=function (tr) {
	var issuer=$("#Issuer",tr);
	var issuerId=$("#IssuerId",tr);
	var securityTypeId=$("#SecurityTypeId",tr);
	var securityId=$("#SecurityId",tr);
	issuer.unbind('blur')
	.autocomplete({ source: "/Deal/FindEquityFixedIncomeIssuers",minLength: 1,
		autoFocus: true,
		select: function (event,ui) {
			issuerId.val(ui.item.id);
			securityTypeId.val(ui.item.otherid);
			securityId.val(ui.item.otherid2);
			deal.loadPurchasePrice(tr);
		},
		appendTo: "#content",delay: 300
	});
};
deal.deleteUnderlyingDirect=function (id,img) {
	if(confirm("Are you sure you want to delete this deal underlying direct?")) {
		var tr=$(img).parents("tr:first");
		var url="/Deal/DeleteDealUnderlyingDirect/"+id;
		$.get(url,function (data) {
			if($.trim(data)!="") {
				jAlert(data);
			} else {
				tr.remove();
				deal.setIndex($("#tblUnderlyingDirect"));
				footer.show("tbodyUnderlyingDirect","tfootUnderlyingDirect");
			}
		});
	}
};
deal.editUnderlyingDirect=function (img) {
	var tr=$(img).parents("tr:first");
	deal.showElements(tr);
};
deal.saveUD=function (img) {
	var tr=$(img).parents("tr:first");
	deal.saveUnderlyingDirect(tr);
};
deal.addUnderlyingDirect=function (img) {
	var tr=$(img).parents("tr:first");
	deal.saveUnderlyingDirect(tr);
};
deal.saveUnderlyingDirect=function (tr) {
	var dealId=deal.getDealId();
	var spnAjax=$("#spnAjax",tr).show();
	spnAjax.show();
	if(dealId>0) {
		var param=jHelper.serialize(tr);
		param[param.length]={ name: "DealId",value: deal.getDealId() };
		param[param.length]={ name: "FundId",value: deal.getFundId() };
		var url="/Deal/CreateDealUnderlyingDirect";
		$.post(url,param,function (data) {
			spnAjax.hide();
			if(data.indexOf("True||")> -1) {
				var id=data.split("||")[1];
				deal.loadUnderlyingDirect(id);
			} else {
				jAlert(data);
			}
		});
	} else {
		spnAjax.hide();
		deal.onDealSuccess=null;
		deal.onDealSuccess=function () { deal.saveUnderlyingDirect(tr); }
		deal.saveDeal();
	}
};
deal.loadUnderlyingDirect=function (id) {
	var dt=new Date();
	var url="/Deal/FindDealUnderlyingDirect?dealUnderlyingDirectId="+id+"&t="+dt.getTime();
	$.getJSON(url,function (data) {
		deal.loadUnderlyingDirectData(data);
	});
};
deal.changeIssuer=function (ddl) {
	if(ddl.value=="-1") {
		deal.currentIssuerDDL=ddl;
	} else {
		deal.loadSecurity($(ddl).parents("tr:first"));
	}
};
deal.changeSecurityType=function (ddl) {
	deal.loadSecurity($(ddl).parents("tr:first"));
};
deal.loadSecurity=function (tr) {
	var issuerId=$("#IssuerId",tr).val();
	var securityTypeId=$("#SecurityTypeId",tr).val();
	var ddl=$("#SecurityId",tr).get(0);
	ddl.options.length=null;
	var listItem;
	listItem=new Option("--Select One--","0",false,false);
	ddl.options[0]=listItem;
	if(parseInt(issuerId)>0&&parseInt(securityTypeId)>0) {
		ddl.options.length=null;
		listItem=new Option("Loading...","",false,false);
		ddl.options[0]=listItem;
		var dt=new Date();
		var url="/Deal/GetSecurity?issuerId="+issuerId+"&securityTypeId="+securityTypeId+"&t="+dt.getTime();
		$.getJSON(url,function (data) {
			ddl.options.length=null;
			if(data.length>0) {
				for(i=0;i<data.length;i++) {
					listItem=new Option(data[i].Text,data[i].Value,false,false);
					ddl.options[ddl.options.length]=listItem;
				}
			} else {
				listItem=new Option("--Select One--","0",false,false);
				ddl.options[0]=listItem;
			}
		});
	}
};
deal.changeSecurity=function (ddl) {
	deal.loadPurchasePrice($(ddl).parents("tr:first"));
};
deal.currentIssuerDDL=null;
deal.loadIssuers=function (issuerName,issuerId) {
	$(".issuerddl").each(function () {
		var listItem=new Option(issuerName,issuerId,false,false);
		var i;
		for(i=0;i<this.options.length;i++) {
			if(this.options[i].value=="-1") {
				this.options[i]=null;
			}
		}
		this.options[this.options.length]=listItem;
	});
	if(deal.currentIssuerDDL) {
		deal.currentIssuerDDL.value=issuerId;
	}
};
deal.calcFMV=function (txt) {
	/*var tr=$(txt).parents("tr:first");
	var noofsha=jHelper.cFloat($("#NumberOfShares",tr).val());
	var price=jHelper.cFloat($("#PurchasePrice",tr).val());
	var FMV=$("#FMV",tr);
	if(isNaN(noofsha)) { noofsha=0; }
	if(isNaN(price)) { price=0; }
	FMV.val(noofsha*price);
	deal.calcDUD();*/
};
deal.loadPurchasePrice=function (tr) {
	var PurchasePrice=$("#PurchasePrice",tr);
	var securityTypeId=$("#SecurityTypeId",tr).val();
	var securityId=$("#SecurityId",tr).val();
	PurchasePrice.val("Loading Purchase Price...");
	$.get("/Deal/FindLastPurchasePrice?_"+(new Date()).getTime()+"&fundId="+deal.getFundId()+"&securityId="+securityId+"&securityTypeId="+securityTypeId,function (data) {
		data=jHelper.cFloat(data);
		if(data>0) { PurchasePrice.val(data.toFixed(2)); } else { PurchasePrice.val(""); }
		deal.calcFMV(PurchasePrice);
	});
};
deal.loadUnderlyingDirectData=function (data) {
	var tbody=$("#tbodyUnderlyingDirect");
	var tr=$("#UnderlyingDirect_"+data.DealUnderlyingDirectId,tbody);
	if(!(tr.get(0))) {
		$("#UnderlyingDirectsRowTemplate").tmpl(data).appendTo("#tbodyUnderlyingDirect");
	} else {
		tr.prev().remove();
		$("#UnderlyingDirectsRowTemplate").tmpl(data).insertAfter(tr);
		tr.remove();
	}
	tr=$("#UnderlyingDirect_"+data.DealUnderlyingDirectId,tbody);
	var Security=$("#SecurityId",tr).get(0);
	if(Security) {
		if(data.SecurityTypeId==1) {
			jHelper.loadDropDown(Security,data.Equities);
		} else if(data.SecurityTypeId==2) {
			jHelper.loadDropDown(Security,data.FixedIncomes);
		}
	}
	var date;
	var amount;
	amount=parseFloat(data.PurchasePrice);if(isNaN(amount)) { data.PurchasePrice=0; }
	amount=parseFloat(data.TaxCostBase);if(isNaN(amount)) { data.TaxCostBase=0; }
	$("#SpnPurchasePrice",tr).html(jHelper.dollarAmount(data.PurchasePrice.toString()));
	$("#SpnTaxCostBase",tr).html(jHelper.dollarAmount(data.TaxCostBase.toString()));
	date=jHelper.formatDate(jHelper.parseJSONDate(data.RecordDate));
	$("#SpnRecordDate",tr).html(date);
	$(":input[name='RecordDate']",tr).val(date);
	if(data.TaxCostDate!=null) {
		date=jHelper.formatDate(jHelper.parseJSONDate(data.TaxCostDate));
		$("#SpnTaxCostDate",tr).html(date);
		$(":input[name='TaxCostDate']",tr).val(date);
	}
	deal.selectValue(tr);
	deal.applyDatePicker(tr);
	deal.setIndex($("#tblUnderlyingDirect"));
};
deal.deleteUnderlyingDirect=function (id,img) {
	if(confirm("Are you sure you want to delete this deal underlying direct?")) {
		var tr=$(img).parents("tr:first");
		var url="/Deal/DeleteDealUnderlyingDirect/"+id;
		$.get(url,function (data) { tr.prev().remove();tr.remove();deal.setIndex($("#tblUnderlyingDirect")); });
	}
};
deal.editUnderlyingDirect=function (img) {
	var tr=$(img).parents("tr:first");
	if(img.src.indexOf('save.png')> -1) {
		deal.saveUnderlyingDirect(tr);
	} else {
		img.src="/Assets/images/save.png";
		deal.showElements(tr);
	}
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
		var url="/Deal/CreateDealUnderlyingDirect";
		$.post(url,param,function (data) {
			spnAjax.hide();
			if(data.indexOf("True||")> -1) {
				var id=data.split("||")[1];
				deal.loadUnderlyingDirect(id);
			} else {
				alert(data);
			}
		});
	} else {
		spnAjax.hide();
		deal.onDealSuccess=null;
		deal.onDealSuccess=function () { deal.saveUnderlyingDirect(tr); }
		$("#btnSaveDeal").click();
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
		issuer.add(0,true); // Call add issuer
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
		listItem=new Option("--Add Issuer--","-1",false,false);
		this.options[this.options.length]=listItem;
	});
	if(deal.currentIssuerDDL) {
		deal.currentIssuerDDL.value=issuerId;
	}
};
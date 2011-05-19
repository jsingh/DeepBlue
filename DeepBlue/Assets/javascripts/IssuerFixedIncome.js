issuer.loadFixedIncomeData=function (data) {
	try {
		var tbody=$("#tbodyFixedIncome");
		var tr=$("#FixedIncome_"+data.FixedIncomeId,tbody);
		if(!(tr.get(0))) {
			$("#FixedIncomeRowTemplate").tmpl(data).appendTo("#tbodyFixedIncome");
		} else {
			$("#FixedIncomeRowTemplate").tmpl(data).insertAfter(tr);
			tr.remove();
		}
		tr=$("#FixedIncome_"+data.FixedIncomeId,tbody);
		var date;
		if(data.Maturity!=null) {
			date=jHelper.formatDate(jHelper.parseJSONDate(data.Maturity));
			$("#SpnMaturity",tr).html(date);
			$(":input[name='Maturity']",tr).val(date);
		}
		if(data.IssuedDate!=null) {
			date=jHelper.formatDate(jHelper.parseJSONDate(data.IssuedDate));
			$("#SpnIssuedDate",tr).html(date);
			$(":input[name='IssuedDate']",tr).val(date);
		}
		if(data.FirstCouponDate!=null) {
			date=jHelper.formatDate(jHelper.parseJSONDate(data.FirstCouponDate));
			$(":input[name='FirstCouponDate']",tr).val(date);
		}
		if(data.FirstAccrualDate!=null) {
			date=jHelper.formatDate(jHelper.parseJSONDate(data.FirstAccrualDate));
			$(":input[name='FirstAccrualDate']",tr).val(date);
		}
		issuer.selectValue(tr);
		issuer.checkboxValue(tr);
		issuer.fixedIncomeSetIndex($("#tblFixedIncome"));
		jHelper.applyDatePicker(tr);
		jHelper.resizeIframe();
	} catch(e) { alert(e); }
};
issuer.loadFixedIncome=function (id) {
	var dt=new Date();
	var url="/Issuer/FindFixedIncome?fixedIncomeId="+id+"&t="+dt.getTime();
	$.getJSON(url,function (data) {
		issuer.loadFixedIncomeData(data);
	});
};
issuer.addFixedIncome=function (img) {
	var tr=$(img).parents(".row:first");
	issuer.saveFixedIncome(tr);
	jHelper.resizeIframe();
};
issuer.editFixedIncome=function (img) {
	var tr=$(img).parents(".row:first");
	if(img.src.indexOf('save.png')> -1) {
		issuer.saveFixedIncome(tr);
	} else {
		img.src="/Assets/images/save.png";
		issuer.showElements(tr);
	}
	jHelper.resizeIframe();
};
issuer.saveFixedIncome=function (tr) {
	var issuerId=issuer.getIssuerId();
	var spnAjax=$("#spnAjax",tr).show();
	spnAjax.show();
	if(issuerId>0) {
		var param=jHelper.serialize(tr);
		param[param.length]={ name: "IssuerId",value: issuer.getIssuerId() };
		var url="/Issuer/CreateFixedIncome";
		$.post(url,param,function (data) {
			spnAjax.hide();
			if(data.indexOf("True||")> -1) {
				var id=data.split("||")[1];
				issuer.loadFixedIncome(id);
				if(issuer.onCloseDialog) {
					issuer.onCloseDialog();
				}
			} else {
				alert(data);
			}
		});
	} else {
		spnAjax.hide();
		issuer.onIssuerSuccess=null;
		issuer.onIssuerSuccess=function () { issuer.saveFixedIncome(tr); }
		$("#Save").click();
	}
};
issuer.fixedIncomeSetIndex=function (target) {
	var index=0;
	$(".row",target).each(function () { index=issuer.putIndex(this,index); });
};
issuer.deleteFixedIncome=function (id,img) {
	if(confirm("Are you sure you want to delete this fixed income?")) {
		var tr=$(img).parents(".row:first");
		var url="/Issuer/DeleteFixedIncome/"+id;
		$.get(url,function (data) { tr.remove();issuer.fixedIncomeSetIndex($("#tblFixedIncome")); });
	}
};

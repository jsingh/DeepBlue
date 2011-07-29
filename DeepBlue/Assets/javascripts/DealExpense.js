deal.loadDealExpenseData=function (data) {
	var tbody=$("#tbodyDealExpense");
	var tr=$("#DealExpense_"+data.DealClosingCostId,tbody);
	if(!(tr.get(0))) {
		$("#DealExpensesRowTemplate").tmpl(data).prependTo("#tbodyDealExpense");
	} else {
		$("#DealExpensesRowTemplate").tmpl(data).insertAfter(tr);
		tr.remove();
	}
	tr=$("#DealExpense_"+data.DealClosingCostId,tbody);
	$("#SpnAmount",tr).html(jHelper.dollarAmount(data.Amount.toString()));
	var date=jHelper.formatDate(jHelper.parseJSONDate(data.Date));
	$("#SpnDate",tr).html(date);
	$(":input[name='Date']",tr).val(date);
	deal.selectValue(tr);
	jHelper.applyDatePicker(tr);
	deal.calcTotalExpense();
	$("#MakeNewDEHeader").hide();
	$("tr:odd","#tbodyDealExpense").removeClass("row").removeClass("arow").addClass("arow");
	$("tr:even","#tbodyDealExpense").removeClass("row").removeClass("arow").addClass("row");
};
deal.deleteDealExpense=function (id,img) {
	if(confirm("Are you sure you want to delete this deal expense?")) {
		var tr=$(img).parents("tr:first");
		var url="/Deal/DeleteDealClosingCost/"+id;
		$.get(url,function (data) {
			tr.remove();
			deal.calcTotalExpense();
		});
	}
};
deal.editDealExpense=function (img) {
	var tr=$(img).parents("tr:first");
	if(img.src.indexOf('save.png')> -1) {
		deal.saveExpense(tr);
	} else {
		img.src="/Assets/images/save.png";
		deal.showElements(tr);
	}
};
deal.addDealExpense=function (img) {
	var tr=$(img).parents("tr:first");
	deal.saveExpense(tr);
};
deal.saveExpense=function (tr) {
	var dealId=deal.getDealId();
	var spnAjax=$("#spnAjax",tr).show();
	spnAjax.show();
	if(dealId>0) {
		var param=[{ name: "DealClosingCostId",value: $(":input[name='DealClosingCostId']",tr).val() }
						,{ name: "DealClosingCostTypeId",value: $(":input[name='DealClosingCostTypeId']",tr).val() }
						,{ name: "Amount",value: $(":input[name='Amount']",tr).val() }
						,{ name: "Date",value: $(":input[name='Date']",tr).val() }
						,{ name: "DealId",value: dealId }
						];
		var url="/Deal/CreateDealExpense";
		$.post(url,param,function (data) {
			spnAjax.hide();
			if(data.indexOf("True||")> -1) {
				var id=data.split("||")[1];
				deal.loadDealExpense(id);
			} else {
				alert(data);
			}
		});
	} else {
		spnAjax.hide();
		deal.onDealSuccess=null;
		deal.onDealSuccess=function () { deal.saveExpense(tr); }
		deal.saveDeal();
	}
};
deal.loadDealExpense=function (id) {
	var dt=new Date();
	var url="/Deal/FindDealClosingCost?dealClosingCostId="+id+"&t="+dt.getTime();
	$.getJSON(url,function (data) {
		deal.loadDealExpenseData(data);
	});
};
deal.calcTotalExpense=function () {
	var total=0;
	$("tbody tr","#tblDealExpense").each(function () { var amt=parseFloat($("#Amount",this).val());if(isNaN(amt)) { amt=0; } total+=amt; });
	$("#SpnTotalExpenses").html(jHelper.dollarAmount(total.toString()));
	$("#SpnFooterTotalExpenses").html(jHelper.dollarAmount(total.toString()));
};
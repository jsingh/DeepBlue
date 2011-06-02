dealActivity.setFLEFund=function (id,name) {
	$("#FLE_FundName").html(name);
	$("#FLE_FundId").val(id);
	var sl=$("#SpnFLEDetLoading");
	jHelper.loading(sl);
	var frm=$("#frmFundExpense");
	$.getJSON("/Deal/FindFundExpense/?_"+(new Date()).getTime()+"&fundId="+id,function (data) {
		sl.empty();var fetypeid=0;var amt="";if(data!=null) {
			fetypeid=data.FundExpenseTypeId;amt=data.Amount;
			var bdy=$("tbody","#ExpenseToDealList");
			bdy.empty();
			$.each(data.ExpenseToDeals,function (index,item) { dealActivity.loadExpenseDealData(item,bdy); });
		}
		$("#FundExpenseTypeId").val(fetypeid);
		$("#Amount").val(amt);
	});
};
dealActivity.loadExpenseDealData=function (data,bdy) {
	var tr=$("#ETD_"+data.DealId,bdy);
	var emptyRow=$("#EmptyETD_"+data.DealId,bdy);
	tr.remove();emptyRow.remove();
	$("#ExpenseToDealTemplate").tmpl(data).prependTo(bdy);
	tr=$("#ETD_"+data.DealId,bdy);
	dealActivity.setUpRow(tr);
};
dealActivity.makeNewETD=function () {
};
dealActivity.addETD=function (img,id) {
};
dealActivity.editETD=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.SaveFundLevelExpense=function (frm) {
	var sl=$("#SpnFLELoading");
	jHelper.loading(sl);
	$.post("/Deal/CreateFundExpense",$(frm).serializeArray(),function (data) {
		sl.empty();
		var arr=data.split("||");
		if(arr[0]!="True") {
			alert(data);
		} else {
			$("#FundExpenseTypeId",frm).val(arr[1]);
		}
	});
	return false;
};
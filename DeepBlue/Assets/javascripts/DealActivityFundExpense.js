dealActivity.setFLEFund=function (id,name) {
	$("#FLE_FundName").html(name);
	$("#FLE_FundId").val(id);
	$("#FLEDetail").show();
	$("#FLEDetail").attr("issearch","true").show();
	var sl=$("#SpnFLEDetLoading");
	jHelper.loading(sl);
	var frm=$("#frmFundExpense");
	$.getJSON("/Deal/FindFundExpense/?_"+(new Date()).getTime()+"&fundId="+id,function (data) {
		sl.empty();var fetypeid=0;var amt="";
		if(data!=null) {
			fetypeid=data.FundExpenseTypeId;amt=data.Amount;
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
	sl.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	$.post("/Deal/CreateFundExpense",$(frm).serializeArray(),function (data) {
		sl.empty();
		var arr=data.split("||");
		if(arr[0]!="True") {
			alert(data);
		} else {
			alert("Fund Expense Saved.");
			$("#FLE_Fund").val("");
			$("#FLEDetail").hide();
		}
	});
	return false;
};
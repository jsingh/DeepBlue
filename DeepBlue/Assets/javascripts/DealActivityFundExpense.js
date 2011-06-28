dealActivity.setFLEFund=function (id,name) {
	$("#FLE_FundName").html(name);
	$("#FLE_FundId").val(id);
	$("#FLEDetail").show();
	$("#FLEDetail").attr("issearch","true").show();
	dealActivity.loadFLE();
};
dealActivity.getFLEFundId=function () {
	return $("#FLE_FundId").val();
};
dealActivity.loadFLE=function () {
	var tbl=$("#FundExpenseList");
	var loading=$("#FLELoading");
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	target.empty();
	$("#FLE").hide();
	$("#FLEDetail").attr("issearch","true").show();
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	$.getJSON("/Deal/FundExpenseList",{ "_": (new Date).getTime(),"fundId": dealActivity.getFLEFundId() },function (data) {
		loading.empty();
		$.each(data,function (i,item) {
			$("#FLEAddTemplate").tmpl(item).appendTo(target);
		});
		dealActivity.setUpRow($("tr",target));
		var rowsLength=$("tr",target).length;
		if(rowsLength>0) { $("#FLE").show(); }
		$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
		$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
	});
};
dealActivity.makeNewFLE=function () {
	var tbl=$("#FundExpenseList");
	var fundId=parseInt(dealActivity.getFLEFundId());
	$("#FLE").show();
	if(isNaN(fundId)) { fundId=0; }
	if(fundId==0) {
		alert("Fund is required");
		$("#FLE_Fund").focus();
	} else {
		var target=$("tbody",tbl);
		var item=dealActivity.newFLEData;
		item["FundId"]=fundId;
		$("#FLEAddTemplate").tmpl(item).prependTo(target);
		var tr=$("#FLE_0",target);
		dealActivity.setUpRow(tr);
		dealActivity.editRow(tr);
	}
};
dealActivity.editFLE=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addFLE=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateFundExpense";
	var param=jHelper.serialize(tr);
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findFLE(arr[1]);
		} else { alert(data); }
	});
};
dealActivity.findFLE=function (feid) {
	var url="/Deal/FindFundExpense/?_"+(new Date()).getTime()+"&fundExpenseId="+feid;
	$("#FLELoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#FLELoading").empty();
		var tbl=$("#FundExpenseList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#FLE_"+feid);
		if(row.get(0)) {
			$("#FLEAddTemplate").tmpl(data).insertAfter(row);
			row.remove();
		} else {
			row=$("tr:first",bdy);
			$("#FLEAddTemplate").tmpl(data).insertBefore(row);
		}
		$("#FLE_0","#FundExpenseList").remove();
		row=$("#FLE_"+feid);
		dealActivity.setUpRow(row);
		$("tr:odd",bdy).removeClass("row").removeClass("arow").addClass("arow");
		$("tr:even",bdy).removeClass("row").removeClass("arow").addClass("row");
	});
};
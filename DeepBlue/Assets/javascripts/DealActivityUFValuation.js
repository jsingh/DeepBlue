dealActivity.findUFV=function (fundid,id) {
	var ufid=dealActivity.getUFVUnderlyingFund();
	var dt=new Date();
	var url="/Deal/FindUnderlyingFundValuation/?underlyingFundId="+ufid+"&fundId="+fundid+"&t="+dt.getTime();
	$("#UFVLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#UFVLoading").empty();
		var tbl=$("#UnderlyingFundValuationList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UFV_"+fundid);
		var emptyRow=$("#EmptyUFV_"+fundid);
		if(row.get(0)) {
			$("#UFValuationAddTemplate").tmpl(data).insertAfter(row);
			row.remove();emptyRow.remove();
		} else {
			row=$("tr:first",bdy);
			$("#UFValuationAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UFV_"+fundid);
		dealActivity.setUpRow(row);
		$("tr:odd",bdy).removeClass("row").removeClass("arow").addClass("arow");
		$("tr:even",bdy).removeClass("row").removeClass("arow").addClass("row");
	});
};
dealActivity.makeNewUFV=function () {
};
dealActivity.editUFV=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addUFV=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateUnderlyingFundValuation";
	var param=jHelper.serialize(tr);
	var ufid=dealActivity.getUFVUnderlyingFund();
	param[param.length]={ name: "UnderlyingFundId",value: ufid };
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findUFV(arr[2],arr[1]);
		} else { alert(data); }
	});
};
dealActivity.deleteUFV=function (fundId,id,img) {
	if(confirm("Are you sure you want to delete this underlying fund valuation?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundValuation/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFV_"+fundId;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				dealActivity.findUFV(fundId,id);
			}
		});
	}
};
dealActivity.setUFVUnderlyingFund=function (id,name) {
	$("#UFVUnderlyingFundId").val(id);
	var loading=$("#UFVLoading");
	var tbl=$("#UnderlyingFundValuationList");
	$("#SpnUFVName").html(name);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	$("#PRValuation").hide();
	$("tbody",tbl).empty();
	$.getJSON("/Deal/UnderlyingFundValuationList",{ "_": (new Date).getTime(),"underlyingFundId": id },function (data) {
		$("#PRValuation").show();
		loading.empty();
		var target=$("tbody",tbl);
		target.empty();
		$.each(data,function (i,item) { $("#UFValuationAddTemplate").tmpl(item).appendTo(target); });
		dealActivity.setUpRow($("tr",target));
		$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
		$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
	});
};
dealActivity.getUFVUnderlyingFund=function (id) {
	return parseInt($("#UFVUnderlyingFundId").val());
};
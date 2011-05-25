dealActivity.findCD=function (id,isNew) {
	var dt=new Date();
	var url="/Deal/FindUnderlyingFundCashDistribution/"+id+"?t="+dt.getTime();
	$("#CDLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#CDLoading").empty();
		var tbl=$("#CashDistributionList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UFCD_"+id);
		var emptyRow=$("#EmptyUFCD_"+id);
		if(row.get(0)) {
			$("#CashDistributionAddTemplate").tmpl(data).insertAfter(row);
			row.remove();emptyRow.remove();
		} else {
			row=$("tr:first",bdy);
			$("#CashDistributionAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UFCD_"+id);
		dealActivity.setUpRow(row);
		if(isNew) { $("#UFCD_0",tbl).remove();$("#EmptyUFCD_0",tbl).remove(); }
	});
};
dealActivity.makeNewCD=function () {
	var ufid=dealActivity.getCDUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		var tbl=$("#CashDistributionList");
		var bdy=$("tbody",tbl);
		var newRow=$("#UFCD_0",bdy);
		var emptyNewRow=$("#EmptyUFCD_0",bdy);
		newRow.remove();
		emptyNewRow.remove();
		var rowsLength=$("tr",bdy).length;
		var data=dealActivity.newCDData;
		if(rowsLength>0) {
			var row=$("tr:first",bdy);
			$("#CashDistributionAddTemplate").tmpl(data).insertBefore(row);
		} else {
			$("#CashDistributionAddTemplate").tmpl(data).appendTo(bdy);
		}
		newRow=$("#UFCD_0",bdy);
		dealActivity.editRow(newRow);
		dealActivity.setUpRow(newRow);
	}
};
dealActivity.editCD=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addCD=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	var isNew=false;
	if(id==0) { isNew=true; }
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateUnderlyingFundCashDistribution";
	var param=jHelper.serialize(tr);
	param[param.length]={ name: "UnderlyingFundId",value: dealActivity.getCDUnderlyingFund() };
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findCD(arr[1],isNew);
		} else { alert(data); }
	});
};
dealActivity.deleteCD=function (id,img) {
	if(confirm("Are you sure you want to delete this underlying fund cash distribution?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundCashDistribution/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFCD_"+id;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				$("#"+trid).remove();
				$("#Empty"+trid).remove();
			}
		});
	}
};
dealActivity.setCDUnderlyingFund=function (id,name) {
	$("#CDUnderlyingFundId").val(id);
	var loading=$("#CDLoading");
	var tbl=$("#CashDistributionList");
	$("#SpnCDUFName").html(name);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	$("#PRCashDistribution").hide();
	$.getJSON("/Deal/UnderlyingFundCashDistributionList",{ "_": (new Date).getTime(),"underlyingFundId": id },function (data) {
		$("#PRCashDistribution").show();
		loading.empty();
		var target=$("tbody",tbl);
		target.empty();
		$.each(data,function (i,item) { $("#CashDistributionAddTemplate").tmpl(item).appendTo(target); });
		dealActivity.setUpRow($("tr",target));
		dealActivity.loadPRCD();
	});
};
dealActivity.getCDUnderlyingFund=function (id) {
	return parseInt($("#CDUnderlyingFundId").val());
};
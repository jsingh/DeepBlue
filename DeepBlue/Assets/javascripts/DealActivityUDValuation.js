dealActivity.findUDV=function (id) {
	var dt=new Date();
	var url="/Deal/FindUnderlyingDirectValuation/"+id+"?t="+dt.getTime();
	$("#UDVLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#UDVLoading").empty();
		var tbl=$("#UDValuationList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UDV_"+id);
		var emptyRow=$("#EmptyUDV_"+id);
		if(row.get(0)) {
			$("#UDVAddTemplate").tmpl(data).insertAfter(row);
			row.remove();emptyRow.remove();
		} else {
			row=$("tr:first",bdy);
			$("#UDVAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UDV_"+id);
		dealActivity.setUpRow(row);
		$("#UDV_0").remove();$("#EmptyUDV_0").remove();
	});
};
dealActivity.makeNewUDV=function () {
};
dealActivity.editUDV=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addUDV=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateUnderlyingDirectValuation";
	var param=jHelper.serialize(tr);
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findUDV(arr[1]);
		} else { alert(data); }
	});
};
dealActivity.deleteUDV=function (id,img) {
	if(confirm("Are you sure you want to delete this underlying direct valuation?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingDirectValuation/"+id+"?t="+dt.getTime();
		var trid="UDV_"+id;
		var tr=$("#UDV_"+id);
		var emptyRow=$("#EmptyUDV_"+id);
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				tr.addClass("newrow");
				$("#LastPrice",tr).html("");
				$("#LastPriceDate",tr).html("");
				$("#editbtn",tr).remove();
				$("#deletebtn",tr).remove();
				$("#add",tr).show();
				tr.attr("id","UDV_0");
				emptyRow.attr("id","EmptyUDV_0");
				dealActivity.editRow(tr);
			}
		});
	}
};
dealActivity.loadUDV=function (id) {
	var loading=$("#UDVLoading");
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	var tbl=$("#UDValuationList");
	$("tbody",tbl).empty();
	$.getJSON("/Deal/UnderlyingDirectValuationList",{ "_": (new Date).getTime(),"issuerId": id },function (data) {
		loading.empty();
		var target=$("tbody",tbl);
		target.empty();
		$.each(data,function (i,item) { $("#UDVAddTemplate").tmpl(item).appendTo(target); });
		dealActivity.setUpRow($("tr",target));
	});
};
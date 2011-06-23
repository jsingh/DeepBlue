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
				$("input[type='text']",tr).val("");
				$("#LastPrice",tr).html("");
				$("#LastPriceDate",tr).html("");
			}
		});
	}
};
dealActivity.setUDV=function (id,name) {
	$("#UDVIssuerId").val(id);
	$("#SpnUDVName").html(name);
	$("#UDVDetail").attr("issearch","true").show();
	dealActivity.loadUDV();
};
dealActivity.getUDVIssuerId=function () {
	return parseInt($("#UDVIssuerId").val());
};
dealActivity.loadUDV=function () {
	var loading=$("#UDVLoading");
	var tbl=$("#UDValuationList");
	var target=$("tbody",tbl);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	target.empty();
	$("#UDValuation").hide();
	$.getJSON("/Deal/UnderlyingDirectValuationList",{ "_": (new Date).getTime(),"issuerId": dealActivity.getUDVIssuerId() },function (data) {
		loading.empty();
		$.each(data,function (i,item) { $("#UDVAddTemplate").tmpl(item).appendTo(target); });
		dealActivity.setUpRow($("tr",target));
		if($("tr",target).length>0) {
			$("#UDValuation").show();
		}$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
				$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
	});
};
dealActivity.submitUDV=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#SpnUDVSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#UDValuationList").length)};
		$.post("/Deal/CreateUnderlyingDirectValuation",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { alert(data); } else {
				alert("Underlying Direct Valuations Saved");
				var tbl=$("#UDValuationList");var target=$("tbody",tbl);
				target.empty();$("#UDValuation").hide();
				$("#UDVIssuerId").val(0);
				$("#SpnUDVName").html("");
				$("#UDV_UnderlyingDirect").val("");
				$("#UDV_UnderlyingDirect").focus();
				
			}
		});
	} catch(e) { alert(e); }
	return false;
};
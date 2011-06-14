dealActivity.setUFAUnderlyingFund=function (id,name) {
	$("#UFAUnderlyingFundId").val(id);
	$("#SpnUFAUFName").html(name);
	dealActivity.loadUFA();
};
dealActivity.getUFAUnderlyingFund=function () {
	return $("#UFAUnderlyingFundId").val();
};
dealActivity.loadUFA=function () {
	var tbl=$("#UnfundedAdjustmentList");
	var loading=$("#UFALoading");
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	target.empty();
	$("#UFAdjustment").hide();
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	$.getJSON("/Deal/UnfundedAdjustmentList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getUFAUnderlyingFund() },function (data) {
		loading.empty();
		$.each(data,function (i,item) {
			item["Index"]=i;
			$("#UFAAddTemplate").tmpl(item).appendTo(target);
		});
		dealActivity.setUpRow($("tr",target));
		rowsLength=$("tr",target).length;
		if(rowsLength>0) { $("#UFAdjustment").show(); }
	});
};
dealActivity.submitUFA=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#SpnUFASaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#UnfundedAdjustmentList").length)/2 };
		$.post("/Deal/CreateUnfundedAdjustment",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { alert(data); } else {
				alert("Unfunded Adjustments Saved");
				var tbl=$("#UnfundedAdjustmentList");var target=$("tbody",tbl);
				target.empty();$("#UFAdjustment").hide();
				$("#UFAUnderlyingFundId").val(0);
				$("#SpnUFAUFName").html("");$("#UFA_UnderlyingFund").val("");
				$("#UFA_UnderlyingFund").focus();
			}
		});
	} catch(e) { alert(e); }
	return false;
};
dealActivity.deleteUFA=function (id,img) {
	if(confirm("Are you sure you want to delete this unfunded adjustment?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnfundedAdjustment/"+id+"?t="+dt.getTime();
		var trid="UFA_"+id;
		var tr=$("#UFA_"+id);
		var emptyRow=$("#EmptyUFA_"+id);
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				tr.addClass("newrow");
				$("input[type='text']",tr).val("");
			}
		});
	}
};
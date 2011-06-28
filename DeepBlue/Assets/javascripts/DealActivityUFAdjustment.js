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
	$("#UFADetail").attr("issearch","true").show();
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
		$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
		$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
	});
};
dealActivity.editUFA=function (img,id) {
	var tr=$(img).parents("tr:first");
	$("#UFA_NCA").html("New Commitment Amount");
	$("#UFA_NUA").html("New Unfunded Amount");
	dealActivity.editRow(tr);
};
dealActivity.addUFA=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/UpdateUnfundedAdjustment";
	var param=jHelper.serialize(tr);
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findUFA(arr[1]);
		} else { alert(data); }
	});
};
dealActivity.findUFA=function (dufid) {
	var url="/Deal/FindUnfundedAdjustment/?_"+(new Date()).getTime()+"&dealUnderlyingFundId="+dufid;
	$("#UFALoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#UFALoading").empty();
		var tbl=$("#UnfundedAdjustmentList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UFA_"+dufid);
		if(row.get(0)) {
			$("#UFAAddTemplate").tmpl(data).insertAfter(row);
			row.remove();
		} else {
			row=$("tr:first",bdy);
			$("#UFAAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UFA_"+dufid);
		dealActivity.setUpRow(row);
		$("tr:odd",bdy).removeClass("row").removeClass("arow").addClass("arow");
		$("tr:even",bdy).removeClass("row").removeClass("arow").addClass("row");
	});
};
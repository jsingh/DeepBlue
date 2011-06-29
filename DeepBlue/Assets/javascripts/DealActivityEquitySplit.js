dealActivity.setESDirect=function (id,name) {
	$("#EquityId").val(id);
	dealActivity.loadEquitySymbol(id,$("#SpnSymbol"));
	//dealActivity.loadNHP(1,dealActivity.getEquityId());
};
dealActivity.onESDirectSearch=function (event,ui) {
	var msg=dealActivity.directSearchValid();
	if(msg!="") { alert(msg);return false; }
	return dealActivity.checkUD();
};
dealActivity.directSearchValid=function () {
	var atypeId=dealActivity.getActivityTypeId();
	var msg="";
	if(atypeId==0) { msg+="Corporate Action is required\n"; }
	/*var udid=dealActivity.getUnderlyingDirectId();
	if(udid==0) { msg+="Underlying Direct is required\n"; }*/
	return msg;
};
dealActivity.getActivityTypeId=function () {
	return parseInt($("#ActivityTypeId").val());
};
dealActivity.checkUD=function () {
	/*var udid=dealActivity.getUnderlyingDirectId();
	if(udid==0) {
	alert("Underlying Direct is required");
	return false;
	}*/
	return true;
};
dealActivity.changeAType=function (ddl) {
	var SplitDetail=$("#SplitDetail");
	var ConversionDetail=$("#ConversionDetail");
	SplitDetail.hide();
	$("#NHPList").hide();
	ConversionDetail.hide();
	switch(ddl.value) {
		case "1":
			var securityType=$("#SecurityTypeId",SplitDetail).get(0);
			securityType.value=1;
			$("#SpnSecurityType",SplitDetail).html(securityType.options[securityType.selectedIndex].text);
			SplitDetail.show();
			break;
		case "2":
			ConversionDetail.show();
			break;
	}
};
dealActivity.setESAutoComplete=function () {
	if(dealActivity.checkUD()) {
		var url="/Deal/FindEquityDirects?dealUnderlyingDirectId="+dealActivity.getUnderlyingDirectId();
		$("#SplitEquityName").autocomplete("option","source",url);
	}
};
dealActivity.getEquityId=function (id) {
	var id=parseInt($("#EquityId").val());
	if(isNaN(id)) { id=0; }
	return id;
};
dealActivity.selectUD=function (id,name) {
	$("#DealUnderlyingDirectId").val(id);
	$("#SpnUDirectName").html(name);
	$("#SATitle").show();
	$("#SADetailBox").show();
	$("#NHPList").show();
};
dealActivity.getUnderlyingDirectId=function () {
	var id=parseInt($("#DealUnderlyingDirectId").val());
	if(isNaN(id)) { id=0; }
	return id;
};
dealActivity.onNHPSubmit=function (p) {
	$("#NHPLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	return true;
};
dealActivity.onNHPSuccess=function (t,p) {
	$("#NHPLoading").empty();
};
dealActivity.loadEquitySymbol=function (id,target) {
	$.get("/Deal/FindEquitySymbol/"+id,function (data) { target.html(data); });
};
dealActivity.loadNHP=function (securityTypeId,securityId,activityId) {
	var atypeId=dealActivity.getActivityTypeId();
	var grid=$("#NewHoldingPatternList");
	var param=[{ name: "dealUnderlyingDirectId",value: dealActivity.getUnderlyingDirectId() }
					 ,{ name: "activityTypeId",value: atypeId }
					 ,{ name: "activityId",value: activityId }
					 ,{ name: "securityTypeId",value: securityTypeId }
					 ,{ name: "securityId",value: securityId}];
	grid.ajaxTableOptions({ params: param });
	grid.ajaxTableReload();
};
dealActivity.onNHPRowBound=function (tr,row,t) {
};
dealActivity.createSA=function (frm) {
	var param=$(frm).serializeArray();
	param[param.length]={ name: "ActivityTypeId",value: dealActivity.getActivityTypeId() };
	param[param.length]={ name: "DealUnderlyingDirectId",value: dealActivity.getUnderlyingDirectId() };
	$("#SpnEquitySplitLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	$.post("/Deal/CreateSplitActivity?_"+(new Date()).getTime(),param,function (data) {
		$("#SpnEquitySplitLoading").empty();
		if($.trim(data)!="") {
			alert(data);
		} else {
			//dealActivity.loadNHP(1,dealActivity.getEquityId());
			//jHelper.resetFields(frm);
		}
	});
	return false;
};
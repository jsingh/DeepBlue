dealActivity.onNewSecuritySearch=function (event,ui) {
	var msg=dealActivity.directSearchValid();
	var stypeId=parseInt($("#NewSecurityTypeId").val());
	if(stypeId==0) {
		msg+="New Direct Type is required";
	}
	if(msg!="") { alert(msg);return false; }
};
dealActivity.onOldSecuritySearch=function (event,ui) {
	var msg=dealActivity.directSearchValid();
	var stypeId=parseInt($("#OldSecurityTypeId").val());
	if(stypeId==0) {
		msg+="Old Direct Type is required";
	}
	if(msg!="") { alert(msg);return false; }
};
dealActivity.changeNewSecurityType=function (ddl) {
	dealActivity.setSecConvAutoComplete(ddl.value,$("#NewSecurity"));
};
dealActivity.changeOldSecurityType=function (ddl) {
	dealActivity.setSecConvAutoComplete(ddl.value,$("#OldSecurity"));
};
dealActivity.setNewSecurity=function (id,name) {
	$("#NewSecurityId").val(id);
};
dealActivity.setOldSecurity=function (id,name) {
	$("#OldSecurityId").val(id);
	dealActivity.loadOldSecurityData();
};
dealActivity.loadOldSecurityData=function () {
	var id=$("#OldSecurityId").val();
	var stypeId=$("#OldSecurityTypeId").val();
	var url="";
	switch(stypeId.toString()) {
		case "1":
			url="/Deal/FindEquitySecurityConversionModel?_"+(new Date()).getTime()+"&equityId="+id;
			break;
		case "2":
			url="/Deal/FindFixedIncomeSecurityConversionModel?_"+(new Date()).getTime()+"&fixedIncomeId="+id;
			break;
	}
	var newSymbol=$("#SpnNewSymbol");
	newSymbol.html("");
	if(url!="") {
		$.getJSON(url,function (data) {
			newSymbol.html(data.Symbol);
			dealActivity.loadNHP($("#OldSecurityTypeId").val(),$("#OldSecurityId").val());
		});
	}
};
dealActivity.setSecConvAutoComplete=function (stypeId,target) {
	if(dealActivity.checkUD()) {
		var url="";
		stypeId=parseInt(stypeId);
		if(isNaN(stypeId)) { stypeId=0; }
		switch(stypeId.toString()) {
			case "1":
				url="/Deal/FindEquityDirects?dealUnderlyingDirectId="+dealActivity.getUnderlyingDirectId();
				break;
			case "2":
				url="/Deal/FindFixedIncomeDirects?dealUnderlyingDirectId="+dealActivity.getUnderlyingDirectId();
				break;
		}
		if(stypeId>0) {
			target.autocomplete("option","source",url);
		}
	}
};
dealActivity.createSecConversion=function (frm) {
	var param=$(frm).serializeArray();
	param[param.length]={ name: "ActivityTypeId",value: dealActivity.getActivityTypeId() };
	param[param.length]={ name: "DealUnderlyingDirectId",value: dealActivity.getUnderlyingDirectId() };
	$("#SpnSecCoversionLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	$.post("/Deal/CreateConversionActivity?_"+(new Date()).getTime(),param,function (data) {
		$("#SpnSecCoversionLoading").empty();
		if($.trim(data)!="") {
			alert(data);
		} else {
			dealActivity.loadOldSecurityData();
		}
	});
	return false;
};
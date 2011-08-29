var fundBreakDownReport={
	init: function () {
		jHelper.waterMark();
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
	}
	,onSubmit: function (frm) {
		try {
			var loading=$("#SpnLoading",frm);
			loading.show();
			var target=$("#ReportDetail");
			target.show();
			$.post("/Report/FundBreakDownReport",$(frm).serializeForm(),function (data) {
				loading.hide();
				if($.trim(data.Error)!="") {
					jAlert(data.Error);
				} else {
					target.empty();
					$("#FundBreakDownReportTemplate").tmpl(data.Data).appendTo(target);
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,print: function () {
		$("#ReportDetail").printArea();
	}
	,exportDeal: function () {
		var exportTypeId=$("#ExportId").val();
		var url="";
		var features="width="+1+",height="+1;
		if(exportTypeId=="1"||exportTypeId=="4") {
			url="/Report/ExportFundBreakDownDetail?FundId="+$("#FundId").val()+"&ExportTypeId="+exportTypeId;
			window.open(url,"exportdeal",features);
		}
	}
}
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
		var width=300;var height=200;var left=(screen.availWidth/2)-(width/2);var top=(screen.availHeight/2)-(height/2);var features="width="+width+",height="+height+",left="+left+",top="+top+",location=no,menubar=no,toobar=no,scrollbars=yes,resizable=yes,status=yes";
		if(exportTypeId=="1"||exportTypeId=="4") {
			url="/Report/ExportFundBreakDownDetail?FundId="+$("#FundId").val()+"&ExportTypeId="+exportTypeId;
			window.open(url,"exportexcel",features);
		}
	}
}
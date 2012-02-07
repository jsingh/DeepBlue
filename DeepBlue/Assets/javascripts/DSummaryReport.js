var report={
	init: function () {
		jHelper.waterMark();
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
		var dt=new Date();
		var url="/Report/GetCapitalDistributions?fundId="+id+"&t="+dt.getTime();
		var txt=$("#jqCBSTextBox_CapitalDistributionId");
		txt.val("Loading...");
		var ddl=document.getElementById("CapitalDistributionId");
		ddl.options.length=null;
		var listItem;
		$.getJSON(url,function (data) {
			ddl.options.length=null;
			listItem=new Option("--Select One--","0",false,false);
			ddl.options[0]=listItem;
			for(i=0;i<data.length;i++) {
				listItem=new Option(data[i].Text,data[i].Value,false,false);
				ddl.options[ddl.options.length]=listItem;
			}
			txt.val("--Select One--");
		});
	}
	,onSubmit: function (frm) {
		try {
			var loading=$("#SpnLoading",frm);
			loading.show();
			var target=$("#ReportDetail");
			target.show();
			$.post("/Report/DistributionSummaryList",$(frm).serializeForm(),function (data) {
				loading.hide();
				if($.trim(data.Error)!="") {
					jAlert(data.Error);
				} else {
					target.empty();
					$("#ReportTemplate").tmpl(data.Data).appendTo(target);
				}
			},"JSON");
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
			url="/Report/ExportCashDistributionDetail?CapitalDistributionId="+$("#CapitalDistributionId").val()+"&FundId="+$("#FundId").val()+"&ExportTypeId="+exportTypeId;
			window.open(deepBlue.rootUrl+url,"exportexcel",features);
		}
	}
}
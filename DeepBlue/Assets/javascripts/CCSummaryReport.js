var ccsummaryReport={
	init: function () {
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
		var dt=new Date();
		var url="/Report/GetCapitalCalls?fundId="+id+"&t="+dt.getTime();
		var ddl=document.getElementById("CapitalCallId");
		ddl.options.length=null;
		var listItem=new Option("Loading...","",false,false);
		ddl.options[0]=listItem;
		$.getJSON(url,function (data) {
			ddl.options.length=null;
			listItem=new Option("--Select One--","0",false,false);
			ddl.options[0]=listItem;
			for(i=0;i<data.length;i++) {
				listItem=new Option(data[i].Text,data[i].Value,false,false);
				ddl.options[ddl.options.length]=listItem;
			}
		});
	}
	,onSubmit: function (frm) {
		try {
			var loading=$("#SpnLoading",frm);
			loading.show();
			var target=$("#ReportDetail");
			target.show();
			$.post("/Report/CapitalCallSummaryList",$(frm).serializeArray(),function (data) {
				loading.hide();
				if($.trim(data.Error)!="") {
					alert(data.Error);
				} else {
					target.empty();
					$("#CCSummaryReportTemplate").tmpl(data.Data).appendTo(target);
				}
			});
		} catch(e) {
			alert(e);
		}
		return false;
	}
	,print: function () {
		$("#ReportDetail").printArea();
	}
}
var report={
	init: function () {
		$(document).ready(function () {
			var layoutSettings=
		{
			Name: "Main",
			Dock: $.layoutEngine.DOCK.NONE,
			EleID: "content",
			Children: [
		{
			Name: "Main",
			Dock: $.layoutEngine.DOCK.FILL,
			EleID: "ReportMain",
			Children: [
		{
			Name: "Top",
			Dock: $.layoutEngine.DOCK.TOP,
			EleID: "ReportHeader",
			Margin: 0,
			Height: 40
		}
		,{
			Name: "Fill",
			Dock: $.layoutEngine.DOCK.FILL,
			EleID: "ReportDetail",Margin: 0,
			MarginLeft: 200,
			MarginRight: 200
		}
		]
		}
		]
		};
			//$.layoutEngine(layoutSettings);
		});
	}
	,reportReinit: function () {
		var layoutSettings=
		{
			Name: "Main",
			Dock: $.layoutEngine.DOCK.NONE,
			EleID: "ReportDetail",
			Margin: 0,
			Children: [
		{
			Name: "Top",
			Dock: $.layoutEngine.DOCK.TOP,
			EleID: "RepTop",
			Height: 100
		}
		,{
			Name: "Fill",
			Dock: $.layoutEngine.DOCK.FILL,
			EleID: "RepContent"
		}
		]
		};
		//$.layoutEngine(layoutSettings);
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
		var dt=new Date();
		var url="/Report/GetCapitalDistributions?fundId="+id+"&t="+dt.getTime();
		var ddl=document.getElementById("CapitalDistributionId");
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
	,onSubmit: function (formId) {
		var frm=document.getElementById(formId);
		var message='';
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		if(message!="") {
			alert(message);
		} else {
			$("#SpnLoading",frm).hide();
			try {
				this.loadReport(formId);
			} catch(e) {
				alert(e);
			}
		}
		return false;
	}
	,loadReport: function (formId) {
		var frm=document.getElementById(formId);
		var fundId=$("#FundId",frm).val();
		var capitalDistributionlId=$("#CapitalDistributionId",frm).val();
		var dt=new Date();
		var url="/Report/DistributionSummaryList/?fundId="+fundId+"&capitalDistributionlId="+capitalDistributionlId+"&t="+dt.getTime();
		$("#SpnLoading",frm).show();
		var reportDetail=$("#ReportDetail");
		reportDetail.show();
		$.getJSON(url,function (data) {
			reportDetail.html("");
			try {
				$("#reportTemplate").tmpl(data).appendTo("#ReportDetail");
				$("#SpnLoading",frm).hide();
				var report_tbl=$("#report_tbl",reportDetail);
				$("tbody tr:not(:last)",report_tbl).each(function () {
					var commitment=$("td:eq(2)",this);
					var distAmount=$("td:eq(3)",this);
					commitment.html(jHelper.dollarAmount(commitment.html()));
					distAmount.html(jHelper.dollarAmount(distAmount.html()));
				});
			} catch(e) {
				alert(e);
			}
			report.reportReinit();
		});
	}
	,print: function () {
		$("#ReportDetail").printArea();
	}
}
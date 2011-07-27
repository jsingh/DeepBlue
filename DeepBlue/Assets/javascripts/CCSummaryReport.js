var ccsummaryReport={
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
										EleID: "ReportDetail",Margin:0,
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
	,ccsummaryReportReinit: function () {
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
		var capitalCallId=$("#CapitalCallId",frm).val();
		var dt=new Date();
		var url="/Report/CapitalCallSummaryList/?fundId="+fundId+"&capitalCallId="+capitalCallId+"&t="+dt.getTime();
		$("#SpnLoading",frm).show();
		var ccsummaryReportDetail=$("#ReportDetail");
		ccsummaryReportDetail.show();
		$.getJSON(url,function (data) {
			ccsummaryReportDetail.html("");
			try {
				$("#ccsummaryReportTemplate").tmpl(data).appendTo("#ReportDetail");
				$("#SpnLoading",frm).hide();
				var ccsummaryReport_tbl=$("#ccsummaryReport_tbl",ccsummaryReportDetail);
				$("tbody tr:not(:last)",ccsummaryReport_tbl).each(function () {
					var commitment=$("td:eq(1)",this);
					var investment=$("td:eq(2)",this);
					var managementFees=$("td:eq(3)",this);
					var expenses=$("td:eq(4)",this);
					var total=$("td:eq(5)",this);
					commitment.html(jHelper.dollarAmount(commitment.html()));
					investment.html(jHelper.dollarAmount(investment.html()));
					managementFees.html(jHelper.dollarAmount(managementFees.html()));
					expenses.html(jHelper.dollarAmount(expenses.html()));
					total.html(jHelper.dollarAmount(total.html()));
				});
			} catch(e) {
				alert(e);
			}
			ccsummaryReport.ccsummaryReportReinit();
		});
	}
	,print : function(){	
		$("#ReportDetail").printArea();
	}
}
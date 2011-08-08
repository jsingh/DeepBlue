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
			$("#ccsummaryReportTemplate").tmpl(data).appendTo("#ReportDetail");
			$("#SpnLoading",frm).hide();
		});
	}
	,print: function () {
		$("#ReportDetail").printArea();
	}
}
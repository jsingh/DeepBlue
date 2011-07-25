var documentSearch={
	init: function () {
		$(document).ready(function () {
			//$("#FundNameCloumn").width($("#InvestorNameColumn").width());
			//$("#FundNameCloumn div").width($("#InvestorNameColumn").width());
		});
	}
	,changeType: function (select) {
		var InvestorRow=document.getElementById("InvestorRow");
		var FundRow=document.getElementById("FundRow");
		var InvestorNameColumn=document.getElementById("InvestorNameColumn");
		var FundNameCloumn=document.getElementById("FundNameCloumn");
		InvestorRow.style.display="none";
		FundRow.style.display="none";
		$("#FundId").val(0);
		$("#InvestorId").val(0);
		InvestorNameColumn.style.display="none";
		FundNameCloumn.style.display="none";
		if(select.value=="1") {
			InvestorRow.style.display="";
			InvestorNameColumn.style.display="";
		} else if(select.value=="2") {
			FundRow.style.display="";
			FundNameCloumn.style.display="";
		}
		this.onSubmit("SearchDocument");
	}
	,selectInvestor: function (id) {
		$("#InvestorId").val(id);
	}
	,InvestorBlur: function (txt) {
		if(txt.value=="") {
			$("#InvestorId").val(0);
		}
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
	}
	,FundBlur: function (txt) {
		if(txt.value=="") {
			$("#FundId").val(0);
		}
	}
	,onGridSuccess: function (t,g) {
		$(window).resize();
	}
	,onInit: function (g) {
		$(window).resize(function () {
			documentSearch.resizeGV(g);
		});
	}
	,resizeGV: function (g) {
		var admain=$(".doc-main");
		var bDivBox=$(g.bDivBox);
		bDivBox.css("height","auto");
		var ah=admain.height()-320;
		var h=bDivBox.height();
		if(h>ah) {
			bDivBox.height(ah);
		}
	}
	,onSubmit: function (formId) {
		var FromDate=document.getElementById("FromDate").value;
		var ToDate=document.getElementById("ToDate").value;
		var DocumentTypeId=document.getElementById("DocumentTypeId").value;
		var DocumentStatus=document.getElementById("DocumentStatus").value;
		var InvestorId=document.getElementById("InvestorId").value;
		var FundId=document.getElementById("FundId").value;
		var grid=$("#SearchDocumentList");
		var param=[{ name: "fromDate",value: FromDate }
					,{ name: "toDate",value: ToDate }
					 ,{ name: "investorId",value: InvestorId }
					 ,{ name: "fundId",value: FundId }
					 ,{ name: "documentTypeId",value: DocumentTypeId }
					 ,{ name: "documentStatusId",value: DocumentStatus }
					];
		//grid.flexOptions({ params: null });
		grid.flexOptions({ params: param });
		grid.flexReload();
		return false;
	}
	,downloadFile: function (filePath,fileName) {
		//window.open("/Document/DownloadDocument?filePath="+filePath+"&fileName="+fileName);
		var url="/"+filePath+"/"+fileName;
		window.open(url);
	}
}
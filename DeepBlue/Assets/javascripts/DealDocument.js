deal.addDealDocument=function () {
	$("#AddDealDocument").show();
};
deal.saveDealDocument=function (frm) {
	try {
		var loading=$("#SpnDealDocLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		var dealId=deal.getDealId();
		if(dealId>0) {
			var p=$(frm).serializeArray();
			p[p.length]={ "name": "DealId","value": dealId };
			$.ajaxFileUpload(
				{
					url: '/Deal/CreateUnderlyingFundDocument',
					secureuri: false,
					formId: 'frmDocumentInfo',
					param: p,
					dataType: 'json',
					success: function (data,status) {
						loading.empty();
						if($.trim(data.data)!="") {
							alert(data.data);
						} else {
							alert("Document Saved");
							underlyingFund.documentRefresh();
							jHelper.resetFields($("#frmDocumentInfo"));
						}
					}
					,error: function (data,status,e) {
						loading.empty();
						alert(data.msg+","+status+","+e);
					}
				});
		}
		else {
			deal.onDealSuccess=null;
			deal.onDealSuccess=function () { deal.saveDealDocument(frm); }
			$("#btnSaveDeal").click();
		}
	} catch(e) { alert(e); }
	return false;
};
deal.documentSelectInvestor=function (id) {
	$("#DocumentInvestorId").val(id);
};
deal.documentInvestorBlur=function (txt) {
	if(txt.value=="") {
		$("#DocumentInvestorId").val(0);
	}
};
deal.documentSelectFund=function (id) {
	$("#DocumentFundId").val(id);
};
deal.documentFundBlur=function (txt) {
	if(txt.value=="") {
		$("#DocumentFundId").val(0);
	}
};
deal.documentChangeType=function (select) {
	var InvestorRow=document.getElementById("InvestorRow");
	var FundRow=document.getElementById("FundRow");
	InvestorRow.style.display="none";
	FundRow.style.display="none";
	$("#DocumentFundId").val(0);
	$("#DocumentInvestorId").val(0);
	if(select.value=="1")
		InvestorRow.style.display="";
	else if(select.value=="2")
		FundRow.style.display="";
};
deal.documentChangeUploadType=function (uploadType) {
	var FileRow=document.getElementById("FileRow");
	var LinkRow=document.getElementById("LinkRow");
	FileRow.style.display="none";
	LinkRow.style.display="none";
	if(uploadType.value=="1")
		FileRow.style.display="";
	else
		LinkRow.style.display="";
};
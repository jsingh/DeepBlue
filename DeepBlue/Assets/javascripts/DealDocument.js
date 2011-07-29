deal.addDealDocument=function () {
	$("#AddDealDocument").show();
};
deal.saveDealDocument=function (frm) {
	try {
		var loading=$("#SpnDealDocLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Uploading...");
		var dealId=deal.getDealId();
		if(dealId>0) {
			var p=$(frm).serializeArray();
			p[p.length]={ "name": "DealId","value": dealId };
			$.ajaxFileUpload(
				{
					url: '/Deal/CreateDealFundDocument',
					secureuri: false,
					formId: 'frmDealDocument',
					param: p,
					dataType: 'json',
					success: function (data,status) {
						loading.empty();
						if($.trim(data.data)!="") {
							alert(data.data);
						} else {
							alert("Document Saved");
							deal.documentRefresh();
							jHelper.resetFields(frm);
						}
					}
					,error: function (data,status,e) {
						loading.empty();
						alert(data.msg+","+status+","+e);
					}
				});
		}
		else {
			loading.empty();
			deal.onDealSuccess=null;
			deal.onDealSuccess=function () { deal.saveDealDocument(frm); }
			deal.saveDeal();
		}
	} catch(e) { alert(e); }
	return false;
};
deal.deleteDealDocument=function (id,img) {
	if(confirm("Are you sure you want to delete this document?")) {
		img.src="/Assets/images/ajax.jpg";
		$.get("/Deal/DeleteDealFundDocumentFile/"+id,function (data) {
			if($.trim(data)!="") {
				alert(data);
			} else {
				deal.documentRefresh();
			}
		});
	}
};
deal.documentRefresh=function () {
	$("#DealDocumentList").flexReload();
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
deal.documentInfoReset=function () {
	jHelper.resetFields($("#AddDealDocument"));
};
deal.documentSetUp=function () {
	try {
		$("#DealDocumentList").flexigrid({
			usepager: false
				,url: "/Deal/DealFundDocumentList"
				,onSubmit: function (p) {
					p.params=null;
					p.params=new Array();
					p.params[p.params.length]={ "name": "dealId","value": deal.getDealId() };
					return true;
				}
				,rpOptions: [10,15,20,50,100]
				,rp: 10
				,resizeWidth: true
				,method: "GET"
				,sortname: "DocumentDate"
				,sortorder: "desc"
				,autoload: true
				,height: 200
				,resizeWidth: false
				,useBoxStyle: false
		});
		$("#DocumentFundName").autocomplete({ source: "/Fund/FindFunds",minLength: 1,select: function (event,ui) { $("#DocumentFundId").val(ui.item.id); },appendTo: "body",delay: 300 });
	} catch(e) { alert(e); }
};
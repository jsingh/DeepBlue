$(document).ready(function () {
	fileUpload.url="/Deal/CreateDealFundDocument";
	fileUpload.onValid=function () {
		var frm=$("#frmDealDocument");
		var dtypeid=parseInt($("#DocumentTypeId",frm).val());
		if(isNaN(dtypeid)) { dtypeid=0; }
		if(dtypeid<=0) {
			jAlert("Document Type is required");
			return false;
		}
		var dealId=deal.getDealId();
		if(dealId==0) {
			deal.onDealSuccess=null;
			if(fileUpload.onUploadStart) {
				deal.onDealSuccess=function () { fileUpload.onUploadStart(); }
			}
			deal.saveDeal();
			return false;
		}
	};
	fileUpload.onCreateBox=function (preview) {
		$("#FilesList",fileUpload.form).append(preview);
	};
	fileUpload.onBeforeSend=function () {
		var loading=$("#SpnDealDocLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Uploading...");
		var frm=$("#frmDealDocument");
		var p=$(frm).serializeForm();
		p[p.length]={ "name": "DealId","value": deal.getDealId() };
		return p;
	};
	fileUpload.onUploadFinished=function (index,file,jsonData,timeDiff) {
		$("#preview_"+index).remove();
		if($.trim(jsonData.data)!="") {
			jAlert(jsonData.data);
			return false;
		}
	};
	fileUpload.onProgress=function (i,file,progress) {
		$("#prsbar_"+i).css("width",progress+"%");
	};
	fileUpload.onAfterAll=function () {
		fileUpload.onUploadStart=null;
		var loading=$("#SpnDealDocLoading");
		loading.empty();
		deal.documentRefresh();
	};
});
deal.addDealDocument=function () {
	$("#AddDealDocument").show();
};
deal.saveDealDocument=function (frm) {
	try {
		if(fileUpload.onUploadStart) {
			fileUpload.onUploadStart();
			return false;
		}
		var loading=$("#SpnDealDocLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Uploading...");
		var dealId=deal.getDealId();
		if(dealId>0) {
			var p=$(frm).serializeForm();
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
							jAlert(data.data);
						} else {
							jAlert("Document Saved");
							deal.documentRefresh();
							jHelper.resetFields(frm);
							$("#DocumentFundName",frm).val("SEARCH AMBERBROOK FUND");
							$("#DocumentType",frm).val("Search");
						}
					}
					,error: function (data,status,e) {
						loading.empty();
						jAlert(data.msg+","+status+","+e);
					}
				});
		}
		else {
			loading.empty();
			deal.onDealSuccess=null;
			deal.onDealSuccess=function () { deal.saveDealDocument(frm); }
			deal.saveDeal();
		}
	} catch(e) { jAlert(e); }
	return false;
};
deal.deleteDealDocument=function (id,img) {
	if(confirm("Are you sure you want to delete this document?")) {
		img.src="/Assets/images/ajax.jpg";
		$.get("/Deal/DeleteDealFundDocumentFile/"+id,function (data) {
			if($.trim(data)!="") {
				jAlert(data);
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
	var target=$("#AddDealDocument");
	$(".preview",target).remove();
	jHelper.resetFields(target);
};
deal.documentSetUp=function () {
	try {
		$("#DealDocumentList").flexigrid({
			usepager: true
				,url: "/Deal/DealFundDocumentList"
				,onSubmit: function (p) {
					p.params=null;
					p.params=new Array();
					p.params[p.params.length]={ "name": "dealId","value": deal.getDealId() };
					return true;
				}
				,resizeWidth: true
				,method: "GET"
				,sortname: "DocumentDate"
				,sortorder: "desc"
				,autoload: true
				,height: 200
				,resizeWidth: false
				,useBoxStyle: false
				,onSuccess: function (t,p) {
					if($("tbody tr",t).length<=0) {
						deal.addDealDocument();
					}
				}
		});
		$("#DocumentFundName").autocomplete({ source: "/Fund/FindFunds",minLength: 1,select: function (event,ui) { $("#DocumentFundId").val(ui.item.id); },appendTo: "body",delay: 300 });
	} catch(e) { jAlert(e); }
};
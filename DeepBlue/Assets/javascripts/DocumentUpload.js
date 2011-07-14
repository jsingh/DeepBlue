var documentUpload={
	pageInit: false
	,init: function () {
		var frm=document.getElementById("AddNewDocument");
		this.showErrorMessage(frm);
		this.pageInit=true;
	}
	,changeType: function (select) {
		var InvestorRow=document.getElementById("InvestorRow");
		var FundRow=document.getElementById("FundRow");
		InvestorRow.style.display="none";
		FundRow.style.display="none";
		if(this.pageInit) {
			$("#FundId").val(0);
			$("#InvestorId").val(0);
		}
		if(select.value=="1")
			InvestorRow.style.display="";
		else if(select.value=="2")
			FundRow.style.display="";
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
	,showErrorMessage: function (frm) {
		var message='';
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		if(documentUpload.pageInit) {
			var DocumentStatus=document.getElementById("DocumentStatus");
			var InvestorId=document.getElementById("InvestorId").value;
			var FundId=document.getElementById("FundId").value;
			if(DocumentStatus.value=="1") {
				if(!parseInt(InvestorId)>0) {
					message+="Investor is required"+"\n";
				}
			} else {
				if(!parseInt(FundId)>0) {
					message+="Fund is required"+"\n";
				}
			}
			var UploadType=document.getElementById("UploadType");
			var FilePath=document.getElementById("FilePath");
			var File=document.getElementById("File");
			if(UploadType.value=="2") {
				if(FilePath.value=="") {
					message+="Link is required"+"\n";
				} else {
					if(FilePath.value.toLowerCase().indexOf("http://")<0) {
						FilePath.value="http://"+FilePath.value;
					}
				}
			} else {
				if(File.value=="")
					message+="Upload file is required"+"\n";
			}
		}
		if(message!="") {
			alert(message);
			return false;
		} else {
			return true;
		}
		return false;
	}
	,changeUploadType: function (uploadType) {
		var FileRow=document.getElementById("FileRow");
		var LinkRow=document.getElementById("LinkRow");
		FileRow.style.display="none";
		LinkRow.style.display="none";
		if(uploadType.value=="1")
			FileRow.style.display="";
		else
			LinkRow.style.display="";
	}
	,onSubmit: function (formId) {
		var frm=document.getElementById(formId);
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
		return this.showErrorMessage(frm);
	}
	,save: function (frm) {
		try {
			var loading=$("#SpnDocLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.ajaxFileUpload(
				{
					url: '/Document/Create',
					secureuri: false,
					formId: 'AddNewDocument',
					dataType: 'json',
					success: function (data,status) {
						loading.empty();
						if($.trim(data.data)!="") {
							alert(data.data);
						} else {
							alert("Document Saved");
							jHelper.resetFields($("#AddNewDocument"));
							$("#FundId").val(0);
							$("#InvestorId").val(0);
						}
					}
					,error: function (data,status,e) {
						loading.empty();
						alert(data.msg+","+status+","+e);
					}
				});
		} catch(e) {
			alert(e);
		}
		return false;
	}
}
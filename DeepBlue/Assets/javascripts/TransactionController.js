var transactionController={
	selectFundCloseId: 0
	,init: function () {
		//	transactionController.loadFundDetails();
		$(document).ready(function () {
			$("#NewTransaction").css("height","auto");
			jHelper.jqComboBox($("body"));
			jHelper.waterMark();


		});
	}
	,save: function (frm) {
		try {
			var loading=$("#UpdateLoading");
			loading.html(jHelper.savingHTML());
			$.post("/Transaction/CreateInvestorFund",$(frm).serializeForm(),function (data) {
				loading.empty();
				if($.trim(data)!="") {
					jAlert(data);
				} else {
					jAlert("Transaction Saved");
					var investorId=$("#InvestorId").val();
					jHelper.resetFields(frm);
					$("#InvestorId").val(investorId);
					transactionController.loadFundDetails();
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,selectInvestor: function (id) {
		var $investorInfo=$("#investorInfo");
		$investorInfo.hide();
		$("#FundId").val(0);
		$("#FundClosingId").val(0);
		$("#InvestorTypeId").val(0);
		$("#TotalCommitment").val("");
		$("#CommittedDate").val("");
		if(id>0) {
			var dt=new Date();
			var loading=$("#Loading");
			loading.html(jHelper.loadingHTML());
			$.getJSON("/Investor/InvestorDetail/"+id+"?t="+dt.getTime(),function (data) {
				$investorInfo.show();
				loading.empty();
				$("#InvestorName",$investorInfo).html(data.InvestorName);
				$("#DisplayName",$investorInfo).html(data.DisplayName);
				$("#TitleInvestorName",$investorInfo).html(data.InvestorName);
				$("#InvestorId",$investorInfo).val(data.InvestorId);
				transactionController.loadFundDetails();
			});
		}
	}
	,loadFundDetails: function () {
		$("#LoadingFundDetail").show();
		var investorId=$("#InvestorId").val();
		var dt=new Date();
		if(parseInt(investorId)>0) {
			$.get("/Transaction/List/"+investorId+"?t="+dt.getTime(),function (data) {
				$("#LoadingFundDetail").hide();
				$("#FundDetails").html(data);
			});
		}
	}
	,onCreateFundBegin: function () {
		$("#UpdateLoading").html(jHelper.savingHTML());
	}
	,onCreateFundSuccess: function () {
		$("#UpdateLoading").html("");
		$("input[type!='hidden'][type!='checkbox']","#investorInfo").val("");
		$("select","#investorInfo").val("0");
		transactionController.loadFundDetails();
	}
	,cloneInvestorInfo: function () {
		var $investorInfo=$("#investorInfo").clone();
		$("#addressInfo",$investorInfo).remove();
		$("#contactInfo",$investorInfo).remove();
		$("#accountInfo",$investorInfo).remove();
		transactionController.assignId($investorInfo);
		return $investorInfo;
	}
	,assignId: function ($target) {
		$("input",$target).each(function () {
			if($(this).attr("name")!="") {
				this.id=$(this).attr("name");
			}
		});
		$("select",$target).each(function () {
			if($(this).attr("name")!="") {
				$(this).attr("id",$(this).attr("name"));
			}
		});
	}
	,loadInvestorInfo: function ($investorInfo,investor) {
		$("#TitleInvestorName",$investorInfo).html(investor.InvestorName);
		$("#InvestorName",$investorInfo).html(investor.InvestorName);
		$("#DisplayName",$investorInfo).html(investor.DisplayName);
		$("#SocialSecurityTaxId",$investorInfo).html(investor.SocialSecurityTaxId);
		if(investor.DomesticForeigns)
			$("#DomesticForeigns",$investorInfo).val("true");
		else
			$("#DomesticForeigns",$investorInfo).val("false");
		$("#StateOfResidency",$investorInfo).val(investor.StateOfResidency);
		$("#EntityType",$investorInfo).val(investor.EntityType);
		return $investorInfo;
	}
	,closeEditTransactionDialog: function (reload) {
		$("#editTransactionDialog").dialog('close');
		$("#EditCommitmentAmount").dialog('close');
		if(reload)
			transactionController.loadFundDetails();
	}
	,editTS: function (transactionId) {
		if(transactionId>0) {
			var editTransaction=$("#EditTransaction");
			editTransaction.empty();
			editTransaction.html(jHelper.loadingHTML());
			$.getJSON("/Transaction/FindInvestorFundDetail/"+transactionId,function (data) {
				editTransaction.empty();
				$("#TransactionTemplate").tmpl(data).appendTo(editTransaction);
				jHelper.jqComboBox(editTransaction);

				$("#CounterPartyInvestor",editTransaction).autocomplete(
				{ source: "/Investor/FindOtherInvestors?investorId="+$("#InvestorId",editTransaction).val()
				,minLength: 1
				,autoFocus: true
				,select: function (event,ui) {
					$("#CounterPartyInvestorId",editTransaction).val(ui.item.id);
					transactionController.loadInvestorType(ui.item.id);
				}
				 ,appendTo: "body"
				 ,delay: 300
				});

			});
			$("#EditTransaction").dialog("open");
		}
	}
	,loadInvestorType: function (investorId) {
		var editTransaction=$("#EditTransaction");
		var FundId=$("#FundId",editTransaction).val();
		var url="/Transaction/InvestorType/?investorId="+investorId+"&fundId="+FundId;
		var disp_InvestorTypeId=$("#disp_InvestorTypeId",editTransaction).get(0);
		var InvestorTypeId=$("#InvestorTypeId",editTransaction).get(0);
		var InvestorTypeRow=$("#InvestorTypeRow",editTransaction).get(0);
		jHelper.jqComboBox(editTransaction);
		InvestorTypeRow.style.display="";
		InvestorTypeId.value=0;
		InvestorTypeId.style.display="none";
		disp_InvestorTypeId.style.display="none";
		disp_InvestorTypeId.innerHTML="";
		$(InvestorTypeId).combobox('destroy');
		$(InvestorTypeId).combobox('remove');
		if(investorId>0) {
			$.getJSON(url,function (data) {
				if(data.InvestorTypeId>0) {
					InvestorTypeId.value=data.InvestorTypeId;
					InvestorTypeId.style.display="none";
					disp_InvestorTypeId.innerHTML=InvestorTypeId.options[InvestorTypeId.selectedIndex].text;
					disp_InvestorTypeId.style.display="";
				}else{
					$(InvestorTypeId).combobox();
				}
			});
		}
	}
	,saveTransaction: function (img) {
		try {
			var frm=$(img).parents("form:first");
			var loading=$("#UpdateLoading","#EditTransaction");
			loading.html(jHelper.savingHTML());
			$.post("/Transaction/CreateFundTransaction",$(frm).serializeForm(),function (data) {
				loading.empty();
				if($.trim(data)!="") {
					jAlert(data);
				} else {
					jAlert("Transaction Saved");
					$("#EditTransaction").dialog("close");
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,editCommitmentAmount: function (investorFundId) {
		$("#Investor").focus();
		$("#CommitmentAmount","#EditCommitmentAmount").val("");
		$("#EditCommitmentAmount").dialog('open');
		$("#UpdateEditCmtLoading","#EditCommitmentAmount").html("");
		$("#EditCommitAmtLoading").html(jHelper.loadingHTML());
		var dt=new Date();
		$.getJSON("/Transaction/FindCommitmentAmount/"+investorFundId+"?t="+dt.getTime(),function (data) {
			$("#EditCommitAmtLoading").html("");
			$("#InvestorFundId","#EditCommitmentAmount").val(data.InvestorFundId);
			$("#CommitmentAmount","#EditCommitmentAmount").val(data.CommitmentAmount);
			$("#UnfundedAmount","#EditCommitmentAmount").val(data.UnfundedAmount);
		});
	}
	,loadFundClosing: function (fundId) {
		$("#FundId","#frmTransaction").val(fundId);
		var dt=new Date();
		var investorId=$("#InvestorId").val();
		var url="/Transaction/FundClosingList/?fundId="+fundId+"&investorId="+investorId+"&t="+dt.getTime();
		var ddl=document.getElementById("FundClosingId");
		ddl.options.length=null;
		var listItem=new Option("Loading...","",false,false);
		ddl.options[0]=listItem;
		var InvestorTypeId=document.getElementById("InvestorTypeId");
		var disp_InvestorTypeId=document.getElementById("disp_InvestorTypeId");
		disp_InvestorTypeId.style.display="none";
		$(InvestorTypeId).combobox('show');
		InvestorTypeId.value=0;
		disp_InvestorTypeId.innerHTML="";
		$.getJSON(url,function (data) {
			if(data.InvestorTypeId>0) {
				InvestorTypeId.value=data.InvestorTypeId;
				$(InvestorTypeId).combobox('hide');
				disp_InvestorTypeId.innerHTML=InvestorTypeId.options[InvestorTypeId.selectedIndex].text;
				disp_InvestorTypeId.style.display="";
			}
			var i;
			ddl.options.length=null;
			listItem=new Option("--Select One--","0",false,false);
			ddl.options[ddl.options.length]=listItem;
			for(i=0;i<data.FundClosingDetails.length;i++) {
				listItem=new Option(data.FundClosingDetails[i].Name,data.FundClosingDetails[i].FundClosingId,false,false);
				ddl.options[ddl.options.length]=listItem;
			}
			listItem=new Option("Add Fund Close","-1",false,false);
			ddl.options[ddl.options.length]=listItem;
			ddl.value=transactionController.selectFundCloseId;
			transactionController.selectFundCloseId=0;
			$(ddl).combobox('destroy');
			$(ddl).combobox('remove');
			jHelper.jqComboBox($("#frmTransaction"));
		});
	}
	,checkFundClose: function (id) {
		if(id== -1) {
			var frm=$("#frmAddFundClose");
			var frmTransaction=$("#frmTransaction");

			jHelper.resetFields(frm);
			jHelper.removejqCheckBox(frm);
			jHelper.jqCheckBox(frm);

			$("#FundId",frm).val($("#FundId",frmTransaction).val());
			$("#CloseFundName",frm).val($("#FundName",frmTransaction).val());
			$("input[type='hidden'][name='IsFirstClosing']",frm).val(false);

			var loading=$("#Loading",frm);
			loading.empty();
			$("#AddFundClose").dialog("open");
		}
	}
	,addFundClose: function () {
		try {
			var frm=$("#frmAddFundClose");
			var frmTransaction=$("#frmTransaction");
			var loading=$("#Loading",frm);
			var param=$(frm).serializeForm();
			var url="/Admin/UpdateFundClosing";
			loading.html(jHelper.savingHTML());
			$.post(url,param,function (data) {
				loading.empty();
				var arr=data.split("||");
				if(arr[0]!="True") {
					jAlert(data);
				} else {
					jAlert("Fund Close Added");

					$("#AddFundClose").dialog("close");
					transactionController.selectFundCloseId=arr[1];

					$("#FundId",frmTransaction).val($("#FundId",frm).val());
					$("#FundName",frmTransaction).val($("#CloseFundName",frm).val());

					transactionController.loadFundClosing($("#FundId",frm).val());
					jHelper.resetFields(frm);
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,cancelFundClose: function () {
		$("#AddFundClose").dialog("close");
	}
}
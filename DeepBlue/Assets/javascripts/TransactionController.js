var transactionController={
	selectFundCloseId: 0
	,init: function () {
		//	transactionController.loadFundDetails();
		$(document).ready(function () {
			$("#NewTransaction").css("height","auto");
			jHelper.jqComboBox($("body"));
		});
	}
	,save: function (frm) {
		try {
			var loading=$("#UpdateLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Transaction/CreateInvestorFund",$(frm).serializeArray(),function (data) {
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
		//location.href="/Transaction/New/"+id;
		$("#Loading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		var $investorInfo=$("#investorInfo");
		$investorInfo.show();
		var dt=new Date();
		$("#FundId").val(0);
		$("#FundClosingId").val(0);
		$("#InvestorTypeId").val(0);
		$("#TotalCommitment").val("");
		$("#CommittedDate").val("");
		$.getJSON("/Investor/InvestorDetail/"+id+"?t="+dt.getTime(),function (data) {
			$("#Loading").html("");
			$("#InvestorName",$investorInfo).html(data.InvestorName);
			$("#DisplayName",$investorInfo).html(data.DisplayName);
			$("#TitleInvestorName",$investorInfo).html(data.InvestorName);
			$("#InvestorId",$investorInfo).val(data.InvestorId);
			transactionController.loadFundDetails();
		});
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
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
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
	,editTransaction: function (transactionId) {
		$("#editTransactionDialog").remove();
		var dt=new Date();
		var url="/Transaction/Edit/"+transactionId+"?t="+dt.getTime();
		var iframe=document.createElement("div");
		iframe.id="editTransactionDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Transaction",
			autoOpen: true,
			width: 600,
			modal: true,
			position: 'top',
			autoResize: true
		});
	}
	,editCommitmentAmount: function (investorFundId) {
		$("#Investor").focus();
		$("#CommitmentAmount","#EditCommitmentAmount").val("");
		$("#EditCommitmentAmount").dialog('open');
		$("#UpdateEditCmtLoading","#EditCommitmentAmount").html("");
		$("#EditCommitAmtLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
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

			$("#FundId",frm).val($("#FundId",frmTransaction).val());
			$("#CloseFundName",frm).val($("#FundName",frmTransaction).val());
			$("#IsFirstClosing",frm).get(0).checked=false;
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
			var param=$(frm).serializeArray();
			var url="/Admin/UpdateFundClosing";
			loading.html(jHelper.savingHTML());
			$.post(url,param,function (data) {
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
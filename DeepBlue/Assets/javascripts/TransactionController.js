var transactionController={
	init: function () {
		//	transactionController.loadFundDetails();
	}
	,selectInvestor: function (id) {
		//location.href="/Transaction/New/"+id;
		$("#Loading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		var $investorInfo=$("#investorInfo");
		$investorInfo.show();
		var dt = new Date();
		$.getJSON("/Investor/InvestorDetail/"+id+"?t="+dt.getTime(),function (data) {
			$("#Loading").html("");
			$("#InvestorName",$investorInfo).html(data.InvestorName);
			$("#DisplayName",$investorInfo).html(data.DisplayName);
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
		$("select","#investorInfo").val("");
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
		if(reload)
			transactionController.loadFundDetails();
	}
	,editTransaction: function (transactionId) {
		$("#editTransactionDialog").remove();
		var dt=new Date();
		var url="/Transaction/Edit/"+transactionId;
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
			width: 630,
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
		var dt = new Date();
		$.getJSON("/Transaction/FindCommitmentAmount/"+investorFundId+"?t="+dt.getTime(),function (data) {
			$("#InvestorFundId","#EditCommitmentAmount").val(data.InvestorFundId);
			$("#CommitmentAmount","#EditCommitmentAmount").val(data.CommitmentAmount);
		});
	}
}
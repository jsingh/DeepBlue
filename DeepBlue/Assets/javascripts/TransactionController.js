var transactionController={
	init: function () {
		transactionController.loadFundDetails();
	}
	,selectInvestor: function (id) {
		location.href="/Transaction/New/"+id;
		/*$("#Loading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Investor/FindInvestor/"+id,function (data) {
		$("#Loading").html("");
		var $investorInfo=$("#investor_"+id);
		if(!($investorInfo.get(0))) {
		var $investorInfo=transactionController.cloneInvestorInfo();
		$("#editinfo").append($investorInfo);
		$investorInfo.css("display","");
		$investorInfo.attr("id","investor_"+id);
		$("#InvestorId",$investorInfo).val(id);
		transactionController.loadInvestorInfo($investorInfo,data);
		}
		});*/
	}
	,loadFundDetails: function () {
		$("#FundDetails").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.get("/Transaction/List/?id="+$("#InvestorId").val(),function (data) {
			$("#FundDetails").html(data);
		});
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
	,closeEditTransactionDialog : function(){
		$("#editTransactionDialog").dialog('close');
	}
	,editTransaction: function (transactionId) {
		var dt = new Date();
		var url="/Transaction/Edit/"+transactionId;
		$("#editTransactionDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="editTransactionDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () {
			$("#loading",iframe).remove();
		});
		ifrm.src=url;
		$(iframe).dialog({
			title: "Transaction",
			autoOpen: true,
			width: 630,
			modal: true,
			position: 'top',
			resizable: true,
			autoResize: true,
			open: function () {
				$("body").css("overflow","hidden");
			},
			close: function () {
				$("body").css("overflow","");
			},
			overlay: {
				opacity: 0.5,
				background: "black"
			}
		});
	}
}
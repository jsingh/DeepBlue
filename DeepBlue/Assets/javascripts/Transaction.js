var transaction={
	selectInvestor: function (id) {
		$("#Loading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Investor/FindInvestor/"+id,function (data) {
			$("#Loading").html("");
			var $investorInfo=$("#investor_"+id);
			if(!($investorInfo.get(0))) {
				var $investorInfo=transaction.cloneInvestorInfo();
				$("#editinfo").append($investorInfo);
				$investorInfo.css("display","");
				$investorInfo.attr("id","investor_"+id);
				$("#InvestorId",$investorInfo).val(id);
				transaction.loadInvestorInfo($investorInfo,data);
			}
		});
	}
	,cloneInvestorInfo: function () {
		var $investorInfo=$("#investorInfo").clone();
		$("#addressInfo",$investorInfo).remove();
		$("#contactInfo",$investorInfo).remove();
		$("#accountInfo",$investorInfo).remove();
		transaction.assignId($investorInfo);
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
}
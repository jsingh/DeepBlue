﻿var capitalCallDetail={
	init: function () {
		$(document).ready(function () {
			var fundId=$("#FundId").val();
			if(parseInt(fundId)>0) {
				capitalCallDetail.selectFund(fundId);
			} else {
				setTimeout(function () {
					$("#CaptialCallDetail").hide();
				},200);
			}
		});
	}
	,selectFund: function (id) {
		$("#SpnLoading").show();
		var dt=new Date();
		var url="/CapitalCall/CapitalCallDetail?id="+id+"&t="+dt.getTime();
		$.getJSON(url,function (data) {
			$("#SpnLoading").hide();
			$("#CaptialCallDetail").show();
			$("#TitleFundName").html(data.FundName);
			$("#SpnFundName").html(data.FundName);
			$("#CapitalCommitted").html(data.CapitalCommitted);
			$("#UnfundedAmount").html(data.UnfundedAmount);
			$("#ManagementFees").html(data.ManagementFees);
			$("#FundExpenses").html(data.FundExpenses);
			var param=[{ name: "fundId",value: id}];
			var grid=$("#FundDetail");
			grid.flexOptions({ params: param });
			grid.flexReload();
		});
	}
}
var capitalDistributionDetail={
	init: function () {
		$(document).ready(function () {
			var fundId=$("#FundId").val();
			if(parseInt(fundId)>0) {
				capitalDistributionDetail.selectFund(fundId);
			} else {
				/*setTimeout(function () {
					$("#CaptialCallDetail").hide();
				},200);*/
			}
		});
	}
	,selectFund: function (id) {
		$("#SpnLoading").show();
		$("#FundId").val(id);
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
	},selectCapitalCall: function (row) {
		location.href="/CapitalCall/Receive/?id="+row.cell[0]+"&fundId="+$("#FundId").val();
	}
}
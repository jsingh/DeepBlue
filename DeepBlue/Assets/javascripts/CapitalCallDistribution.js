var distribution={
	init: function () {
		$(document).ready(function () {
			jHelper.waterMark();
		});
	}
	,selectFund: function (id) {
		$("#SpnLoading").show();
		var dt=new Date();
		var url="/CapitalCall/FundDetail?id="+id+"&t="+dt.getTime();
		$("#lnkPCC").attr("href","#");
		$.getJSON(url,function (data) {
			$("#lnkPCC").attr("href",deepBlue.rootUrl+"/CapitalCall/Detail?fundId="+id+"&typeId=1");
			$("#lnkPCD").attr("href",deepBlue.rootUrl+"/CapitalCall/Detail?fundId="+id+"&typeId=2");
			$("#SpnLoading").hide();
			$("#CCDetail").show();
			$("#FundId").val(data.FundId);

			$("#TitleFundName").html(data.FundName);
			$("#Fund").val(data.FundName);
			$("#SpnDAmount").html(jHelper.dollarAmount(data.TotalDistribution));
			$("#SpnProfitAmount").html(jHelper.dollarAmount(data.TotalProfit));
			$("#SpnDistributionNumber").html(data.DistributionNumber);
			$("#SpnManualDistributionNumber").html(data.DistributionNumber);

			$("#CommittedAmount").val(data.TotalCommitment);
			$("#DistributionNumber").val(data.DistributionNumber);
		});
	}
	,showControl: function (chk,boxId) {
		var box=document.getElementById(boxId);
		if(box) {
			if(chk.checked)
				box.style.display="";
			else
				box.style.display="none";
		}
	}
	,calcProfit: function () {
		var DistributionAmount=jHelper.cFloat($("#DistributionAmount").val());
		var PreferredReturn=jHelper.cFloat($("#PreferredReturn").val());
		var ReturnManagementFees=jHelper.cFloat($("#ReturnManagementFees").val());
		var ReturnFundExpenses=jHelper.cFloat($("#ReturnFundExpenses").val());
		var PreferredCatchUp=jHelper.cFloat($("#PreferredCatchUp").val());
		var profit=(DistributionAmount-PreferredReturn-ReturnManagementFees-ReturnFundExpenses-PreferredCatchUp);
	}
	,selectTab: function (type,lnk) {
		var CD=$("#NewCapitalDistribution");
		var MCD=$("#ManualCapitalDistribution");
		$("#NewCDTab").removeClass("section-tab-sel");
		$("#ManCDTab").removeClass("section-tab-sel");
		CD.hide();MCD.hide();
		$(lnk).addClass("section-tab-sel");
		switch(type) {
			case "C": CD.show();break;
			case "M": MCD.show();break;
		}
	}
	,save: function (frmid) {
		try {
			var frm=$("#"+frmid);
			var loading=$("#UpdateLoading");
			loading.html(jHelper.savingHTML());
			var param=$(frm).serializeForm();
			param[param.length]={ name: "FundId",value: $("#FundId").val() };
			param[param.length]={ name: "DistributionNumber",value: $("#DistributionNumber").val() };
			$.post("/CapitalCall/CreateDistribution",param,function (data) {
				loading.empty();
				var arr=data.split("||");
				if($.trim(arr[0])!="True") {
					jAlert(data);
				} else {
					jAlert("Capital Distribution Saved.");
					location.href = deepBlue.rootUrl+"/CapitalCall/ModifyCapitalDistribution/"+arr[2];
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
}
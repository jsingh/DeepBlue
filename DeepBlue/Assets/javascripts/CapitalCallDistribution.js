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
			$("#lnkPCC").attr("href","/CapitalCall/Detail?fundId="+id+"&typeId=1");
			$("#lnkPCD").attr("href","/CapitalCall/Detail?fundId="+id+"&typeId=2");
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
		var DistributionAmount=jHelper.cfloat($("#DistributionAmount").val());
		var PreferredReturn=jHelper.cfloat($("#PreferredReturn").val());
		var ReturnManagementFees=jHelper.cfloat($("#ReturnManagementFees").val());
		var ReturnFundExpenses=jHelper.cfloat($("#ReturnFundExpenses").val());
		var PreferredCatchUp=jHelper.cfloat($("#PreferredCatchUp").val());
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
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
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
					$("#SpnDistributionNumber").html(arr[1]);
					$("#DistributionNumber").val(arr[1]);
					$("#SpnManualDistributionNumber").html(arr[1]);
					jHelper.resetFields(frm);
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
}
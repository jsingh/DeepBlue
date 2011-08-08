var capitalCallDetail={
	init: function () {
		$(document).ready(function () {
			jHelper.waterMark();
			var fundId=$("#FundId").val();
			if(parseInt(fundId)>0) {
				capitalCallDetail.selectFund(fundId);
			}
		});
	}
	,selectFund: function (id) {
		$("#SpnLoading").show();
		$("#FundId").val(id);
		$("#CaptialCallDetail").hide();
		var ccReportTarget=$("#CapitalCallReport");
		var cdReportTarget=$("#CapitalDistributionReport");
		ccReportTarget.empty();
		cdReportTarget.empty();
		$.getJSON("/CapitalCall/FindDetail?_"+(new Date).getTime()+"&fundId="+id,function (data) {
			$("#SpnLoading").hide();
			$("#CaptialCallDetail").show();
			$("#TitleFundName").html(data.FundName);
			$("#Fund").val(data.FundName);
			$("#CapitalCallReportTemplate").tmpl(data).prependTo(ccReportTarget);
			$("#CapitalDistributionReportTemplate").tmpl(data).prependTo(cdReportTarget);
		});
	}
	,selectTab: function (type,lnk) {
		var CD=$("#CapitalCallReport");
		var MCD=$("#CapitalDistributionReport");
		$("#NewCCDetailTab").removeClass("section-tab-sel");
		$("#ManCDetailTab").removeClass("section-tab-sel");
		CD.hide();MCD.hide();
		$(lnk).addClass("section-tab-sel");
		switch(type) {
			case "C": CD.show();break;
			case "M": MCD.show();break;
		}
	}
	,selectCapitalCall: function (row) {
		location.href="/CapitalCall/Receive/?id="+row.cell[0]+"&fundId="+$("#FundId").val();
	}
	,expandCC: function (img,id) {
		var tr=$(img).parents("tr:first");
		var expandTR=$("#CC_"+id).get(0);
		var display="";
		var imgsrc="";
		$(".expandrow").css("display","none");
		if(img.src.indexOf("downarrow.png")>0) {
			imgsrc="/Assets/images/rightuarrow.png";
		} else {
			imgsrc="/Assets/images/downarrow.png";
			display="none";
		}
		$(".ccexpandrow").attr("src","/Assets/images/downarrow.png");
		img.src=imgsrc;
		if(!expandTR) {
			expandTR=document.createElement("tr");
			expandTR.id="CC_"+id;
			expandTR.className="expandrow";
			var expandTD=document.createElement("td");
			expandTD.colSpan=7;
			$(expandTD).css("padding","0px");
			$(expandTR).append(expandTD);
			var div=document.createElement("div");
			div.className="exploading";
			div.innerHTML="Loading...";
			$(expandTD).append(div);
			$.getJSON("/CapitalCall/GetCapitalCallInvestors?_"+(new Date).getTime()+"&capitalCallId="+id,function (data) {
				$(expandTD).empty();
				$("#CCInvestorTemplate").tmpl(data).appendTo(expandTD);
			});
			$(tr).after(expandTR);
		}
		$(expandTR).css("display",display);
	}
	,expandCD: function (img,id) {
		var tr=$(img).parents("tr:first");
		var expandTR=$("#CD_"+id).get(0);
		var display="";
		var imgsrc="";
		$(".expandrow").css("display","none");
		if(img.src.indexOf("downarrow.png")>0) {
			imgsrc="/Assets/images/rightuarrow.png";
		} else {
			imgsrc="/Assets/images/downarrow.png";
			display="none";
		}
		$(".ccexpandrow").attr("src","/Assets/images/downarrow.png");
		img.src=imgsrc;
		if(!expandTR) {
			expandTR=document.createElement("tr");
			expandTR.id="CD_"+id;
			expandTR.className="expandrow";
			var expandTD=document.createElement("td");
			expandTD.colSpan=9;
			$(expandTD).css("padding","0px");
			$(expandTR).append(expandTD);
			var div=document.createElement("div");
			div.className="exploading";
			div.innerHTML="Loading...";
			$(expandTD).append(div);
			$.getJSON("/CapitalCall/GetCapitalDistributionInvestors?_"+(new Date).getTime()+"&capitalDistributionId="+id,function (data) {
				$(expandTD).empty();
				$("#CDInvestorTemplate").tmpl(data).appendTo(expandTD);
			});
			$(tr).after(expandTR);
		}
		$(expandTR).css("display",display);
	}
}
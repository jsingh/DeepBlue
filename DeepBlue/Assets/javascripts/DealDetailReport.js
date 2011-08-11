var dealDetailReport={
	init: function () {
		jHelper.waterMark();
	}
	,onSubmit: function (frm) {
		try {
			var target=$("#ReportContent");
			target.html("<center>"+jHelper.loadingHTML()+"</center>");
			$.post("/Report/DealDetailReport",$(frm).serializeArray(),function (data) {
				target.empty();
				if($.trim(data.Error)!="") {
					alert(data.Error);
				} else {
					$("#ReportTemplate").tmpl(data.Data).appendTo(target);
				}
			});
		} catch(e) { alert(e); }
		return false;
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
	}
	,selectDeal: function (id) {
		$("#DealId").val(id);
	}
	,dealSearch: function (request,response) {
		$.getJSON("/Deal/FindDeals?term="+request.term+"&fundId="+$("#FundId").val(),function (data) {
			response(data);
		});
	}
	,print: function () {
		$("#ReportContent").printArea();
	}
	,exportDeal: function () {
		var exportTypeId=$("#ExportId").val();
		var url="";
		var features="width="+1+",height="+1;
		if(exportTypeId=="1"||exportTypeId=="4") {
			url="/Report/ExportDealDetail?DealId="+$("#DealId").val()+"&FundId="+$("#FundId").val()+"&ExportTypeId="+exportTypeId;
			window.open(url,"exportdeal",features);
		}
	}
}
$.extend(window,{
	formatROI: function (d) {
		if(d==null) {
			return "";
		}
		d=parseFloat(d).toFixed(2);
		if(isNaN(d)) {
			return "";
		}
		if(d.toString()=="0.00") {
			return "";
		}
		else {
			return d.toString()+"x";
		}
	}
});
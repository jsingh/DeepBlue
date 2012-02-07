var dealDetailReport={
	init: function () {
		jHelper.waterMark();
	}
	,onSubmit: function (frm) {
		try {
			var target=$("#ReportContent");
			target.html("<center>"+jHelper.loadingHTML()+"</center>");
			$.post("/Report/DealDetailReport",$(frm).serializeForm(),function (data) {
				target.empty();
				if($.trim(data.Error)!="") {
					jAlert(data.Error);
				} else {
					$("#ReportTemplate").tmpl(data.Data).appendTo(target);
				}
			},"JSON");
		} catch(e) { jAlert(e); }
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
		var width=300;var height=200;var left=(screen.availWidth/2)-(width/2);var top=(screen.availHeight/2)-(height/2);var features="width="+width+",height="+height+",left="+left+",top="+top+",location=no,menubar=no,toobar=no,scrollbars=yes,resizable=yes,status=yes";
		if(exportTypeId=="1"||exportTypeId=="4") {
			url="/Report/ExportDealDetail?DealId="+$("#DealId").val()+"&FundId="+$("#FundId").val()+"&ExportTypeId="+exportTypeId;
			window.open(deepBlue.rootUrl+url,"exportexcel",features);
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
$(document).ready(function () {
	$(window).resize(function () {
		investorLibrary.resizeWindow();
	});
});
var investorLibrary={
	resizeWindow: function () {
		var w=$(window).width();
		var diff=1263;
		var setw=0;
		if($.browser.msie)
			diff=1259;
		if(w<diff)
			setw=128;
		else
			setw=174;
		$(".fund-title").width(setw);
	}
	,search: function () {
		var loading = $("#SpnLoading");
		loading.html(jHelper.loadingHTML());
		var p=new Array();
		p[p.length]={ name: "investorId",value: $("#InvestorID").val() };
		p[p.length]={ name: "fundId",value: $("#FundID").val() };
		var grid=$("#InvestorLibraryList");
		grid.flexOptions({ params: p, newp: 1 });
		grid.flexReload();
	}
	,onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		jHelper.jqCheckBox(t);
		
	}
	,onInit: function (g) {
	}
	,onTemplate: function (tbody,data) {
		var loading = $("#SpnLoading");
		loading.html("");
		investorLibrary.LibraryData=data;
		var target=$("#InvestorLibraryList");
		target.empty();
		if(data.FlexGridData.total>0) {
			$("#GridTemplate").tmpl(data).appendTo(target);
			investorLibrary.resizeWindow();
		}
	}
	,paging: function (index,that) {
		$(that).html(jHelper.loadingHTML());
		var p=new Array();
		var grid=$("#InvestorLibraryList");
		grid.flexOptions({ newp: index });
		grid.flexReload();
	}
	,expand: function (id,that) {
		var display="";
		var img=$("img",that).get(0);
		if(img.src.indexOf("Minus")>0) {
			display="none";
			img.src=img.src.replace("Minus","Pluss");
		} else {
			img.src=img.src.replace("Pluss","Minus");
		}
		$("#"+id).css("display",display);
	}
	,LibraryData: null
	,LastInvestor: null
}
$.extend(window,{
	removeLastInvestor: function () {
		investorLibrary.LastInvestor=null;
		return "true";
	}
	,getInvestor: function (fundId,investorId) {
		var investorFund=null;
		$.each(investorLibrary.LibraryData.Library,function (i,item) {
			if(item.FundID==fundId) {
				$.each(item.FundInformations,function (j,fund) {
					if(fund.InvestorID==investorId) {
						investorFund=fund;
						return;
					}
				});
			}
		});
		return investorFund;
	}
	,getCommimentAmount: function (fundId,investorId) {
		var investor=getInvestor(fundId,investorId);
		if(investor)
			return formatCurrency(investor.CommitmentAmount);
		else
			return "";
	}
	,getUnfundedAmount: function (fundId,investorId) {
		var investor=getInvestor(fundId,investorId);
		if(investor)
			return formatCurrency(investor.UnfundedAmount);
		else
			return "";
	}
	,getFundClose: function (fundId,investorId) {
		var investor=getInvestor(fundId,investorId);
		if(investor)
			return (investor.FundClose==null?"":investor.FundClose);
		else
			return "";
	}
	,getCommittedDate: function (fundId,investorId) {
		var investor=getInvestor(fundId,investorId);
		if(investor)
			return (investor.CommittedDate==null?"":formatDate(investor.CommittedDate));
		else
			return "";
	}
	,getCloseDate: function (fundId,investorId) {
		var investor=getInvestor(fundId,investorId);
		if(investor)
			return (investor.CloseDate==null?"":formatDate(investor.CloseDate));
		else
			return "";
	}
});
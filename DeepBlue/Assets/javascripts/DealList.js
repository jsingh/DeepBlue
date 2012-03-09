var dealList={
	onSubmit: function (p) {
		p.params=null;
		p.params=new Array();
		p.params[p.params.length]={ "name": "isNotClose","value": "true" };
		p.params[p.params.length]={ "name": "fundID","value": $("#FundID").val() };
		p.params[p.params.length]={ "name": "dealID","value": $("#DealID").val() };
		return true;
	}
	,onTemplate: function (tbody,data) {
		$("#GridTemplate").tmpl(data).appendTo(tbody);
	}
	,onGridSuccess: function (t,g) {
		var dealID=parseInt($("#DealID").val());
		var fundID=parseInt($("#FundID").val());
		$(".deal-list",t).each(function () {
			var dl=this;
			$(dl)
			.flexigrid({
				usepager: true
				,useBoxStyle: false
				,url: "/Deal/DealList"
				,onSubmit: function (p) {
					p.params=null;
					p.params=new Array();
					p.params[p.params.length]={ "name": "isNotClose","value": "true" };
					p.params[p.params.length]={ "name": "fundId","value": $(dl).attr("fundid") };
					p.params[p.params.length]={ "name": "dealId","value": dealID };
					return true;
				}
				,onTemplate: function (tbody,data) {
					$("#DealGridTemplate").tmpl(data).appendTo(tbody);
				}
				,rpOptions: [25,50,100,200]
				,rp: 25
				,resizeWidth: false
				,method: "GET"
				,sortname: "DealName"
				,sortorder: ""
				,autoload: false
				,height: 0
				,onSuccess: function (t,g) {
					jHelper.gridEditRow(t);
				}
			});
		});
		$("tbody tr",t).each(function () {
			var tdlen=$("td",this).length;
			if(tdlen<4) {
				$("td:last",this).attr("colspan",(5-$("td",this).length));
			}
		});
		if(dealID>0) {
			var tr=$("tbody tr:first",t);
			$(tr).click();
		}
	}
	,expandFund: function (fundId) {
		var expandImg=$("#ExpandImg_"+fundId);
		var expandRow=$("#ExpandRow_"+fundId);
		if(expandRow.css("display")=="none") {
			expandRow.css("display","");
			expandImg.attr("src",jHelper.getImagePath("uparrow.png"));
			$("#FundDealList",expandRow).flexReload();
		} else {
			expandRow.css("display","none");
			expandImg.attr("src",jHelper.getImagePath("downarrow.png"));
		}
	}
	,search: function () {
		$("#DealList").flexReload();
	}
	,searchDeal: function (request,response) {
		$.getJSON("/Deal/FindDeals?term="+request.term+"&fundId="+$("#FundID").val(),function (data) {
			response(data);
		});
	}
}
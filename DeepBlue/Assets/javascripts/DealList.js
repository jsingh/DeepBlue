var dealList={
	onSubmit: function (p) {
		p.params=[{ name: "isNotClose",value: "true"}];
		return true;
	}
	,onTemplate: function (tbody,data) {
		$("#GridTemplate").tmpl(data).appendTo(tbody);
	}
	,onGridSuccess: function (t,g) {
		$(".deal-list",t).each(function () {
			var dl = this;
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
		});
		});
		$("tbody tr",t).each(function () {
			var tdlen=$("td",this).length;
			if(tdlen<4) {
				$("td:last",this).attr("colspan",(5-$("td",this).length));
			}
		});
	}
	,expandFund: function (fundId) {
		var expandImg=$("#ExpandImg_"+fundId);
		var expandRow=$("#ExpandRow_"+fundId);
		if(expandRow.css("display")=="none") {
			expandRow.css("display","");
			expandImg.attr("src","/Assets/images/uparrow.png");
			$("#DealList",expandRow).flexReload();
		} else {
			expandRow.css("display","none");
			expandImg.attr("src","/Assets/images/downarrow.png");
		}
	}
}
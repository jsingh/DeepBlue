var dealReport={
	pageIndex: 1
	,init: function () {
		$(document).ready(function () {
			var lnk=$("#lnkExport");
			var pos=$(lnk).position();
			var menu=$("#ExportMenu");
			$("li",menu).hover(function () { $(this).addClass("sel"); },function () { $(this).removeClass("sel"); });
			menu.css({ "left": pos.left-55,"top": pos.top+20 });
			lnk.toggle(function () { menu.show(); },function () { menu.hide(); });
		});
	}
	,onSubmit: function (p) {
		var reportCnt=$("#ReportContent");
		var h=reportCnt.height();
		p.rp=parseInt((h/55));
		p.newp=dealReport.pageIndex;
		$("#ReportLoading").show();
		return true;
	}
	,onSuccess: function (t,p) {
		$("#ReportLoading").hide();
		var tfoot=$("tfoot",t).get(0);
		if(!tfoot) {
			tfoot=document.createElement("tfoot");
			var trviewmore=document.createElement("tr");
			var td=document.createElement("td");td.colSpan=6;
			td.innerHTML="<a href='javascript:dealReport.viewMore();'>View More</a>";
			$(trviewmore).append(td);
			$(tfoot).append(trviewmore);
			$(t).append(tfoot);
		}
		if(p.pages<=p.newp) { $(tfoot).remove(); }
	}
	,onRowBound: function (tr,row) {
		var trempty=document.createElement("tr");
		var td=document.createElement("td");
		td.colSpan=6;
		trempty.className="emptyrow";
		$(trempty).append(td);
		$("td:last",tr).html("<img id='expandimg' src='/Assets/images/arrow_down.png'/>");
		$(tr).before(trempty);
	}
	,onRowClick: function (row,tr) {
		var dealId=row.cell[0];
		var url="/Deal/DealUnderlyingDetails/?dealId="+dealId;
		var rowId="Deal_"+dealId;
		var trExpand;
		trExpand=document.getElementById(rowId);
		var expandimg=$("#expandimg",tr).get(0);
		if(expandimg.src.indexOf("arrow_down.png")> -1) { expandimg.src=expandimg.src.replace("arrow_down.png","arrow_up.png");$(tr).addClass("expandrow"); } else { expandimg.src=expandimg.src.replace("arrow_up.png","arrow_down.png");$(tr).removeClass("expandrow"); }
		if(!trExpand) {
			trExpand=document.createElement("tr");
			trExpand.className=tr.className;
			trExpand.id="Deal_"+dealId;
			var td=document.createElement("td");
			td.colSpan=6;
			td.innerHTML="<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...";
			$(trExpand).append(td);
			$(tr).after(trExpand);
			$.getJSON(url,function (data) {
				$(td).empty();
				$("#DealDetailTemplate").tmpl(data).appendTo(td);
				var tblUnderlyingFund=$("#tblUnderlyingFund",trExpand);
				var tblUnderlyingDirect=$("#tblUnderlyingDirect",trExpand);
				dealReport.setIndex(tblUnderlyingFund);
				dealReport.setIndex(tblUnderlyingDirect);
				dealReport.setDateValue(tblUnderlyingFund);
				dealReport.setDateValue(tblUnderlyingDirect);
				dealReport.setDollarValue(tblUnderlyingFund);
				dealReport.setDollarValue(tblUnderlyingDirect);
			});
		} else {
			if(trExpand.style.display=="none") { trExpand.style.display=""; } else { trExpand.style.display="none"; }
		}
	}
	,onChangeSort: function (t,param) {
		$("tbody",t).empty();
		$("#SortName").val(param.sortname);
		$("#SortOrder").val(param.sortorder);
	}
	,setDollarValue: function (tbl) {
		$(".dollarcell",tbl).each(function () {
			this.innerHTML=jHelper.dollarAmount(this.innerHTML.toString());
		});
	}
	,setDateValue: function (tbl) {
		$(".datecell",tbl).each(function () {
			var date=jHelper.formatDate(jHelper.parseJSONDate(this.innerHTML));
			this.innerHTML=date;
		});
	}
	,setIndex: function (tbl) {
		var i=0;
		$("tbody tr",tbl).each(function () {
			i++;$("td:first",this).html(i+".");
		});
	}
	,viewMore: function () {
		dealReport.pageIndex++;
		$("#ReportList").ajaxTableReload();
	}
	,selectFund: function (id) {
		var grid=$("#ReportList");
		$("#FundId").val(id);
		var param=[{ name: "fundId",value: id}];
		$("tbody",grid).empty();
		grid.ajaxTableOptions({ params: param });
		grid.ajaxTableReload();
	}
	,exportDeal: function (exportTypeId) {
		var fundId=$("#FundId").val();
		var url;
		var features="width="+1+",height="+1;
		if(exportTypeId==1||exportTypeId==2) {
			url="/Deal/Export?fundId="+fundId+"&exportTypeId="+exportTypeId+"&sortName="+$("#SortName").val()+"&sortOrder="+$("#SortOrder").val();
		} else {
			url="/Deal/ExportDetail?IsPrint=true&FundId="+fundId+"&SortName="+$("#SortName").val()+"&SortOrder="+$("#SortOrder").val();
		}
		window.open(url,"exportdeal",features);
	}
}
var viewActivities={
	pageIndex: 1
	,isViewMore: false
	,init: function () {
		$(document).ready(function () {
			var lnk=$("#lnkExport");
			var pos=$(lnk).position();
			var menu=$("#ExportMenu");
			$("li",menu).hover(function () { $(this).addClass("sel"); },function () { $(this).removeClass("sel"); });
			lnk.toggle(function () { menu.show(); },function () { menu.hide(); });
			jHelper.waterMark();
		});
	}
	,onSubmit: function (p) {
		if(viewActivities.isViewMore==true) {
			p.newp=p.newp+1;
		}
		viewActivities.isViewMore=false;
		return true;
	}
	,onSuccess: function (t,p) {
		$("#TabHeader").show();
		$("#ReportLoading").hide();
		$("tbody tr",t).addClass("row").css("cursor","pointer");
		var tfoot=$("tfoot","#ViewMoreDetail").get(0);
		if(!tfoot) {
			tfoot=document.createElement("tfoot");
			var trviewmore=document.createElement("tr");
			tfoot.className="rep_tfoot";
			var td=document.createElement("td");td.colSpan=10;
			td.style.textAlign="center";
			td.innerHTML="<a href='javascript:viewActivities.viewMore();'>View more</a>&nbsp;"+jHelper.imageHTML("vmarrow.png");
			$(trviewmore).append(td);
			$(tfoot).append(trviewmore);
			$("#ViewMoreDetail").append(tfoot);
		}
		if(p.pages<=p.newp) { $(tfoot).remove(); }
	}
	,onRowBound: function (tr,row) {
		var trempty=document.createElement("tr");
		var td=document.createElement("td");
		$(trempty).append(td);
		$("td:last",tr).html("<img id='expandimg' src='"+jHelper.getImagePath("arrow_down.png")+"'/>");
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
			// trExpand.style.background = "#E4E5EA";
			trExpand.id="Deal_"+dealId;
			var td=document.createElement("td");
			td.colSpan=10;
			td.className="expandRowBg";
			td.innerHTML=jHelper.loadingHTML();
			$(trExpand).attr("dealname",$("td:eq(2)",tr).html());
			$(trExpand).append(td);
			$(tr).after(trExpand);
			$.getJSON(url,function (data) {
				$(td).empty();
				$("#DealDetailTemplate").tmpl(data).appendTo(td);
				var tblUnderlyingFund=$("#tblUnderlyingFund",trExpand);
				var tblUnderlyingDirect=$("#tblUnderlyingDirect",trExpand);


				viewActivities.setDateValue(tblUnderlyingFund);
				viewActivities.setDateValue(tblUnderlyingDirect);

				viewActivities.setDollarValue(tblUnderlyingFund);
				viewActivities.setDollarValue(tblUnderlyingDirect);
			});
		} else {
			if(trExpand.style.display=="none") { trExpand.style.display=""; } else { trExpand.style.display="none"; }
		}
	}
	,onChangeSort: function (t,p) {
		p.newp=1;
		$("tbody",t).empty();
		$("#SortName").val(p.sortname);
		$("#SortOrder").val(p.sortorder);
	}
	,setDollarValue: function (tbl) {
		$(".dollarcell",tbl).each(function () {
			var amt;
			try {
				amt=jHelper.dollarAmount(this.innerHTML.toString());
				if(amt=="$NaN.00") { amt=""; }
			} catch(e) { amt=""; }
			this.innerHTML=amt;
		});
	}
	,setDateValue: function (tbl) {
		$(".datecell",tbl).each(function () {
			var date;
			try {
				date=jHelper.formatDate(jHelper.parseJSONDate(this.innerHTML.toString()));
			} catch(e) { date=""; }
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
		viewActivities.isViewMore=true;
		$("#ReportList").ajaxTableReload();
	}
	,selectFund: function (id,name) {
		try {
			$("#DealReportMain").show();
			var grid=$("#ReportList");
			$("#FundId").val(id);
			$("#FundName").val(name);
			$("#SpnFundName").html(name);
			var param=[{ name: "fundId",value: id}];
			viewActivities.pageIndex=1;
			$("tbody",grid).empty();
			grid.ajaxTableOptions({ params: param });
			grid.ajaxTableReload();
		} catch(e) {
			alert(e);
		}
	}
	,exportDeal: function () {
		var exportTypeId=$("#ExportId").val();
		var fundId=$("#FundId").val();
		var url;
		var width=300;var height=200;var left=(screen.availWidth/2)-(width/2);var top=(screen.availHeight/2)-(height/2);var features="width="+width+",height="+height+",left="+left+",top="+top+",location=no,menubar=no,toobar=no,scrollbars=yes,resizable=yes,status=yes";
		if(exportTypeId=="1"||exportTypeId=="4") {
			url="/Deal/Export?fundId="+fundId+"&exportTypeId="+exportTypeId+"&sortName="+$("#SortName").val()+"&sortOrder="+$("#SortOrder").val();
			window.open(deepBlue.rootUrl+url,"exportexcel",features);
		}
	}
	,printArea: function () {
		$("#ReportBox").printArea();
	}
	,selectDealTab: function (that,detailid) {
		$(".section-tab").removeClass("section-tab-sel");
		$(".section-det").hide();
		var detailbox=$("#"+detailid);
		var SearchCBox=$("#SearchCBox");
		SearchCBox.hide();
		$(that).addClass("section-tab-sel");
		detailbox.show();
	}
	,viewActivity: function (id,ufid,dudId,dufId,fundName) {
		var addNewFund=$("#DealReportMain");
		var addNewFundTab;
		var editDealRow=$("#Deal_"+id);
		addNewFundTab=$("#TabDealGrid");
		var data={ id: id,DealName: $(editDealRow).attr("dealname")+" - "+fundName };
		var tab=$("#Tab"+id);
		var editbox=$("#Edit"+id);
		tab.remove();
		editbox.remove();
		$("#ActivityListTemplate").tmpl(data).insertAfter(addNewFund);
		$("#TabTemplate").tmpl(data).insertAfter(addNewFundTab);
		editbox=$("#Edit"+id);
		tab=$("#Tab"+id);
		viewActivities.expand(editbox);
		viewActivities.setUpActivity(editbox,id,ufid,dudId,dufId);
		//dealDirect.undirectList(id,editbox);
		$(".center",tab).click();
	}
	,setUpActivity: function (box,dealId,ufid,dudId,dufId) {
		$(".grid",box).each(function () {
			var grid=$(this);
			var gridId=grid.attr("id");
			if(grid.flexExist()==true) {
				grid.flexReload();
			} else {
				var isAutoLoad=false;
				if(gridId=="tblCC") {
					isAutoLoad=true;
				} else {
					isAutoLoad=false;
				}
				grid.flexigrid({
					usepager: true
					,useBoxStyle: false
					,url: grid.attr("url")
					,onSubmit: function (p) {
						p.params=null;
						p.params=new Array();
						switch(gridId) {
							case "tblCC":
							case "tblCD":
							case "tblPRCC":
							case "tblPRCD":
							case "tblSD":
								p.params[p.params.length]={ "name": "underlyingFundID","value": ufid };
								break;
							case "tblUA":
								p.params[p.params.length]={ "name": "dealUnderlyingFundID","value": dufId };
								break;
							case "tblUFV":
							case "tblUFVH":
								p.params[p.params.length]={ "name": "dealUnderlyingFundID","value": dufId };
								p.params[p.params.length]={ "name": "underlyingFundID","value": ufid };
								break;
						}
						p.params[p.params.length]={ "name": "dealID","value": dealId };
						p.params[p.params.length]={ "name": "dealUnderlyingDirectID","value": dudId };
						return true;
					}
					,onSuccess: function (t,g) {
						if(g.isChangeSort==true) {
							return;
						}
						var flexigrid=$(t).parents(".flexigrid:first");
						var rows=$("tbody tr",t).length;
						if(rows>0) {
							flexigrid.show();
						} else {
							flexigrid.hide();
						}
						switch(gridId) {
							case "tblCC":
								$("#tblCD").flexReload();
								break;
							case "tblCD":
								$("#tblPRCC").flexReload();
								break;
							case "tblPRCC":
								$("#tblPRCD").flexReload();
								break;
							case "tblPRCD":
								$("#tblSD").flexReload();
								break;
							case "tblSD":
								$("#tblUA").flexReload();
								break;
							case "tblUA":
								$("#tblUFV").flexReload();
								break;
							case "tblUFV":
								$("#tblUFVH").flexReload();
								break;
						}
					}
					,onTemplate: function (tbody,data) {
						var tmplName=grid.attr("templatename");
						$("#"+tmplName).tmpl(data).appendTo(tbody);
						var flexigrid=grid.parents(".flexigrid:first");
						var rows=$("tr",tbody).length;
						if(rows>0) {
							flexigrid.show();
						} else {
							flexigrid.hide();
						}
					}
					,rpOptions: [25,50,100,200]
					,rp: 25
					,resizeWidth: false
					,method: "GET"
					,sortname: grid.attr("sortname")
					,sortorder: grid.attr("sortorder")
					,autoload: isAutoLoad
					,height: 0
				});
			}
		});
	}
	,deleteTab: function (id,isConfirm) {
		var isRemove=true;
		if(isConfirm) {
			isRemove=confirm("Are you sure you want to remove this activity?");
		}
		if(isRemove) {
			$("#Tab"+id).remove();
			$("#Edit"+id).remove();
			$("#tabdel"+id).remove();
			$("#TabDealGrid").click();
		}
	}
	,expand: function (box) {
		$(".recon-headerbox",box).unbind('click').click(function () {
			$(".recon-headerbox",box).show();
			$(".recon-expandheader",box).hide();
			$(".recon-detail",box).hide();
			$(this).hide();
			var parent=$(this).parent();
			$(".recon-expandheader",parent).show();
			var detail=$(".recon-detail",parent);
			var display=detail.attr("issearch");
			if(display!="true") {
				detail.hide();
			} else {
				detail.show();
			}
		});
		$(".recon-expandheader",box).unbind('click').click(function () {
			var expandheader=$(this);
			var parent=$(expandheader).parent();
			expandheader.hide();
			var detail=$(".recon-detail",parent);
			detail.hide();
			$(".recon-headerbox",parent).show();
		});
	}
}
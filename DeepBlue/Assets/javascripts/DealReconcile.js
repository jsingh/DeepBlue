var dealReconcile={
	setFundId: function (id) {
		$("#ReconcileFundId").val(id);
		//dealReconcile.submit(0);
	}
	,init: function () {
		$(document).ready(function () {
			var rfundname=$("#ReconcileFundName");
			var rufundname=$("#ReconcileUnderlyingFundName");
			var rfundid=$("#ReconcileFundId");
			var rufundid=$("#ReconcileUnderlyingFundId");
			rfundname
				.autocomplete(
				{
					source: "/Fund/FindFunds"
				,minLength: 1
				,autoFocus: true
				,select: function (event,ui) {
					rfundid.val(0);
					rfundname.val("");
					rufundid.val(0);
					rufundname.val("");
					dealReconcile.setFundId(ui.item.id);
				}
				,appendTo: "body",delay: 300
				});
			rufundname
				.autocomplete(
				{
					source: function (request,response) {
						$.getJSON("/Deal/FindReconcileUnderlyingFunds"+"?term="+request.term+"&fundId="+rfundid.val(),function (data) {
							response(data);
						});
					}
				,minLength: 1
				,autoFocus: true
				,select: function (event,ui) {
					rufundid.val(ui.item.id);
					//dealReconcile.submit(0);
				}
				,appendTo: "body",delay: 300
				});

		});
	}
	,getFundId: function () {
		return parseInt($("#ReconcileFundId").val());
	}
	,changeDate: function () {
		//dealReconcile.submit(0);
	}
	,submit: function (type) {
		try {
			var frm=document.getElementById("frmReconcile");
			var fundId=$("#ReconcileFundId").val();
			var sdate=$("#ReconStartDate",frm);
			var edate=$("#ReconEndDate",frm);
			sdate.val(sdate.val().replace("START DATE",""));
			edate.val(edate.val().replace("END DATE",""));
			var target;
			var loading;
			switch(type.toString()) {
				case "1": target=$("#RGUFCC");loading=$("#RGUFCC");break;
				case "2": target=$("#RGUFCD");loading=$("#RGUFCD");break;
				case "3": target=$("#RGCC");loading=$("#RGCC");break;
				case "4": target=$("#RGCD");loading=$("#RGCD");break;
				case "5": target=$("#RGDD");loading=$("#RGDD");break;
				default: target=$("#ReconcilReport");loading=$("#SpnReconLoading");break;
			}
			target.empty();
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
			var param=$(frm).serializeForm();
			param[param.length]={ name: "ReconcileType",value: type };
			$.ajax({
				type: "POST",
				url: "/Deal/ReconcileList",
				data: param,
				dataType: "json",
				cache: false,
				success: function (data) {
					loading.empty();
					dealReconcile.generateReport(data,target,type);
				},
				error: function (data) { try { if(p.onError) { p.onError(data); } } catch(e) { } }
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,generateReport: function (data,target,type) {
		if($.trim(data.Error)!="") {
			jAlert(data.Error);
		}
		var expand=false;
		var item;
		var templateName="";
		if(data.Results!=null) {
			if(type=="0") {
				item={
					UFCCItems: function () { return { ReconcileTypeId: 1,Items: dealReconcile.findJSON(data.Results,1)} }
					,UFCDItems: function () { return { ReconcileTypeId: 2,Items: dealReconcile.findJSON(data.Results,2)} }
					,CCItems: function () { return { ReconcileTypeId: 3,Items: dealReconcile.findJSON(data.Results,3)} }
					,CDItems: function () { return { ReconcileTypeId: 4,Items: dealReconcile.findJSON(data.Results,4)} }
					,DDItems: function () { return { ReconcileTypeId: 5,Items: dealReconcile.findJSON(data.Results,5)} }
					,FundExpenses: function () { return { ReconcileTypeId: 6,Items: data.FundExpenses} }
				};
				expand=true;
				templateName="ReconcileReportTemplate";
			} else {
				item={ ReconcileTypeId: type,Items: data.Results };
				templateName="ReconcileGridTemplate";
			}
			$("#"+templateName).tmpl(item).appendTo(target);
			$("table[type='regrid']",target).each(function () {
				var tbl=dealReconcile.setFlexGrid($(this));
				var gridData={};
				if(type=="0") {
					switch(this.id) {
						case "tbl1": if(type=="0") { gridData.Results=dealReconcile.findJSON(data.Results,1);gridData.total=data.TotalRows[1]; } break;
						case "tbl2": if(type=="0") { gridData.Results=dealReconcile.findJSON(data.Results,2);gridData.total=data.TotalRows[2]; } break;
						case "tbl3": if(type=="0") { gridData.Results=dealReconcile.findJSON(data.Results,3);gridData.total=data.TotalRows[3]; } break;
						case "tbl4": if(type=="0") { gridData.Results=dealReconcile.findJSON(data.Results,4);gridData.total=data.TotalRows[4]; } break;
						case "tbl5": if(type=="0") { gridData.Results=dealReconcile.findJSON(data.Results,5);gridData.total=data.TotalRows[5]; } break;
					}
				} else {
					gridData=item;
				}
				gridData.page=data.page;
				tbl.flexAddData(gridData);
			});
			var frmReconcile=$("#frmReconcile");
			var refundlist=$("#REFundExpenseList")
			.flexigrid({
				usepager: true
				,useBoxStyle: false
				,url: "/Deal/ReconcileFundExpenseList"
				,onSubmit: function (p) {
					if(p.newp==undefined) { p.newp=1; }
					p.params=null;
					p.params=frmReconcile.serializeForm();
					p.params[p.params.length]={ "name": "PageSize","value": p.rp };
					p.params[p.params.length]={ "name": "PageIndex","value": p.newp };
					return true;
				}
				,onTemplate: function (tbody,jsondata) {
					var data={ total: jsondata.total,Items: jsondata.Results };
					$("#RECFundExpenseTemplate").tmpl(data).appendTo(tbody);
				}
				,rpOptions: [25,50,100,200]
				,rp: 25
				,resizeWidth: false
				,sortname: ""
				,sortorder: ""
				,autoload: false
				,height: 0
			});
			var fedata={ Results: data.FundExpenses,total: data.totalFundExpenses };
			refundlist.flexAddData(fedata);
			dealReconcile.applyDatePicker(target);
			if(expand) {
				dealReconcile.expand();
			}
		}
	}
	,setFlexGrid: function (tbl) {
		var frmReconcile=$("#frmReconcile");
		var retype=tbl.attr("id").replace("tbl","");
		return tbl
					.flexigrid({
						usepager: true
					,useBoxStyle: false
					,url: "/Deal/ReconcileList"
					,onBeforeAddData: function (data) {
						if(data.TotalRows) {
							data.total=data.TotalRows[retype];
						}
					}
					,onSubmit: function (p) {
						if(p.newp==undefined) { p.newp=1; }
						p.params=null;
						p.params=frmReconcile.serializeForm();
						p.params[p.params.length]={ "name": "ReconcileType","value": retype };
						p.params[p.params.length]={ "name": "PageSize","value": p.rp };
						p.params[p.params.length]={ "name": "PageIndex","value": p.newp };
						return true;
					}
					,onTemplate: function (tbody,jsondata) {
						var data={ ReconcileTypeId: retype,Items: jsondata.Results };
						$("#RECItemBoundTemplate").tmpl(data).appendTo(tbody);
					}
					,rpOptions: [25,50,100,200]
					,rp: 25
					,resizeWidth: false
					,sortname: ""
					,sortorder: ""
					,autoload: false
					,height: 0
					});
	}
	,findJSON: function (data,typeId) {
		var arr=[];
		$.each(data,function (i,item) {
			if(item.ReconcileTypeId==typeId)
				arr.push(item);
		});
		return arr;
	}
	,save: function (frmid,spnid,typeid) {
		var frm=document.getElementById(frmid);
		var param=$(frm).serializeForm();
		var loading=$("#"+spnid);
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		var tbl=$("#tbl"+typeid,frm);
		var totalRows=($("tbody tr",tbl).length);
		param[param.length]={ name: "TotalRows",value: totalRows };
		$.post("/Deal/CreateReconcile",param,function (data) {
			loading.empty();
			if($.trim(data)!="") {
				jAlert(data);
			} else {
				jAlert("Reconcile Saved.");
				//dealReconcile.submit(typeid);
				$("#tbl"+typeid).flexReload();
			}
		});
	}
	,expand: function () {
		$(".recon-headerbox").unbind('click').click(function () {
			$(".recon-headerbox").show();
			$(".recon-expandheader").hide();
			$(".recon-detail").hide();
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
		$(".recon-expandheader").unbind('click').click(function () {
			var expandheader=$(this);
			var parent=$(expandheader).parent();
			expandheader.hide();
			var detail=$(".recon-detail",parent);
			detail.hide();
			$(".recon-headerbox",parent).show();
		});
	}
	,checkReconcile: function (chk,txtid,dt) {
		if(chk.checked) {
			var txt=$("#"+txtid);
			var PaymentDate=$("#"+txtid.replace("PaidOn","PaymentDate"));
			if($.trim(txt.val())=="") {
				txt.val(PaymentDate.val());
			}
		}
	}
	,checkParentId: function (pid,txt,target) {
		$(".datefield",target).each(function () {
			var parentid=parseInt($(this).attr("parentid"));
			if(isNaN(parentid)) { parentid=0; }
			if(parentid>0) {
				if(parentid==pid) {
					this.value=txt.value;
				}
			}
		});
	}
	,applyDatePicker: function (target) {
		$(".datefield",target).each(function () {
			var txt=this;
			$(this)
			.datepicker({ changeMonth: true,changeYear: true,onSelect: function () {
				setTimeout(function () {
					var parentid=parseInt($(txt).attr("parentid"));
					if(isNaN(parentid)) { parentid=0; }
					if(parentid>0) {
						dealReconcile.checkParentId(parentid,txt,target);
					}
					if(txt.id.indexOf("PaidOn")>0) {
						var tr=$(txt).parents("tr:first");
						var chk=$(":checkbox",tr).get(0);
						if($.trim(txt.value)!="") {
							if(chk) { chk.checked=true; }
						}
					}
				},100);
			}
			});
		});
	}
}
 
var dealReconcile={
	setFundId: function (id) {
		$("#ReconcileFundId").val(id);
		dealReconcile.submit(0);
	}
	,getFundId: function () {
		return parseInt($("#ReconcileFundId").val());
	}
	,changeDate: function () {
		dealReconcile.submit(0);
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
				default: target=$("#ReconcilReport");loading=$("#SpnReconLoading");break;
			}
			target.empty();
			if(parseInt(fundId)>0) {
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
			}
		} catch(e) {
			alert(e);
		}
		return false;
	}
	,generateReport: function (data,target,type) {
		if($.trim(data.Error)!="") {
			alert(data.Error);
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
				};
				expand=true;
				templateName="ReconcileReportTemplate";
			} else {
				item={ ReconcileTypeId: type,Items: data.Results };
				templateName="ReconcileGridTemplate";
			}
			$("#"+templateName).tmpl(item).appendTo(target);
			dealReconcile.applyDatePicker(target);
			if(expand) {
				dealReconcile.expand();
			}
		}
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
		var totalRows=($("#ReconcileBdy tr",frm).length);
		param[param.length]={ name: "TotalRows",value: totalRows };
		$.post("/Deal/CreateReconcile",param,function (data) {
			loading.empty();
			if($.trim(data)!="") {
				alert(data);
			} else {
				alert("Reconcile Saved.");
				dealReconcile.submit(typeid);
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
		$(".recon-expandtitle",".recon-expandheader").unbind('click').click(function () {
			var expandheader=$(this).parents(".recon-expandheader:first");
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
			if($.trim(txt.val())=="") {
				txt.val(dt);
			}
		}
	}
	,applyDatePicker: function (target) {
		$(".datefield",target).each(function () {
			var txt=this;
			$(this).datepicker({ changeMonth: true,changeYear: true,onSelect: function () {
				setTimeout(function () {
					var tr=$(txt).parents("tr:first");
					var chk=$(":checkbox",tr).get(0);
					if($.trim(txt.value)!="") {
						if(chk) { chk.checked=true; }
					}
				},100);
			}
			});
		});
	}
}
 
var dealReconcile={
	setFundId: function (id) {
		$("#ReconcileFundId").val(id);
		dealReconcile.submit();
	}
	,getFundId: function () {
		return parseInt($("#ReconcileFundId").val());
	}
	,submit: function () {
		try {
			var frm=document.getElementById("frmReconcile");
			var loading=$("#SpnReconLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
			var sdate=$("#ReconStartDate",frm);
			var edate=$("#ReconEndDate",frm);
			sdate.val(sdate.val().replace("START DATE",""));
			edate.val(edate.val().replace("END DATE",""));
			var target=$("#ReconcilReport");
			target.empty();
			$.ajax({
				type: "POST",
				url: "/Deal/ReconcileList",
				data: $(frm).serializeArray(),
				dataType: "json",
				cache: false,
				success: function (data) {
					loading.empty();
					var item={
						UFCCItems: function () { return { Items: dealReconcile.findJSON(data.Results,1)} }
						,UFCDItems: function () { return { Items: dealReconcile.findJSON(data.Results,2)} }
						,CCItems: function () { return { Items: dealReconcile.findJSON(data.Results,3)} }
						,CDItems: function () { return { Items: dealReconcile.findJSON(data.Results,4)} }
					};
					$("#ReconcileReportTemplate").tmpl(item).appendTo(target);
					jHelper.applyDatePicker(target);
					dealReconcile.expand();
				},
				error: function (data) { try { if(p.onError) { p.onError(data); } } catch(e) { } }
			});
		} catch(e) {
			alert(e);
		}
		return false;
	}
	,findJSON: function (data,typeId) {
		var arr=[];
		$.each(data,function (i,item) {
			if(item.ReconcileTypeId==typeId)
				arr.push(item);
		});
		return arr;
	}
	,save: function (img) {
		var tr=$(img).parents("tr:first");
		var param=jHelper.serialize(tr);
		img.src="/Assets/images/ajax.jpg";
		$.post("/Deal/SaveReconcile",param,function (data) {
			img.src="/Assets/images/save.png";
			if($.trim(data)!="") {
				alert(data);
			} else {
				alert("Reconcile Saved.");
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
}
$.extend(window,{
	formatDate: function (dt) { return jHelper.formatDate(jHelper.parseJSONDate(dt)); }
	,formatCurrency: function (d) { return jHelper.dollarAmount(d.toString()); }
});
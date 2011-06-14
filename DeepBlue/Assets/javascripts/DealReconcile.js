var dealReconcile={
	isViewMore: false
	,init: function () {
		$(document).ready(function () {
			jHelper.waterMark($("body"));
			$("#PageIndex").val(1);
			dealReconcile.calcPageSize();
		});
		$(window).resize(function () {
			dealReconcile.calcPageSize();
		});
	}
	,calcPageSize: function () {
		var pageSize=parseInt($("#ReconcileBox").height()/40);
		$("#PageSize").val(pageSize);
	}
	,setFund: function (id,name) {
		$("#FundId").val(id);
		this.isViewMore=false;
		this.search();
	}
	,clearFund: function (txt) {
		if($.trim(txt.value)=="") {
			$("#FundId").val(0);
			this.isViewMore=false;
			this.search();
		}
	}
	,submit: function () {
		this.isViewMore=false;
		this.search();
	}
	,search: function () {
		try {
			var frm=document.getElementById("frmReconcile");
			var loading=$("#SpnLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
			if(dealReconcile.isViewMore==false) {
				$("#PageIndex").val(1);
			}
			$.ajax({
				type: "POST",
				url: "/Deal/ReconcileList",
				data: $(frm).serializeArray(),
				dataType: "json",
				cache: false,
				success: function (data) {
					var i;var tbl=$("#ReconcileList");
					var target=$("tbody",tbl);
					if(dealReconcile.isViewMore==false) {
						target.empty();
					}
					loading.empty();
					var ps=parseInt($("#PageSize").val());
					var pages=Math.ceil(data.Total/ps);
					if(data.Results.length>0&&pages>1) {
						$("tfoot",tbl).show();
					} else {
						$("tfoot",tbl).hide();
					}
					for(i=0;i<data.Results.length;i++) {
						var row=data.Results[i];
						var item={ EventType: row.EventType
										,UnderlyingFundName: row.UnderlyingFundName
										,FundName: row.FundName
										,FormatCCAmount: jHelper.dollarAmount(row.CapitalCallAmount.toString())
										,FormatDAmount: jHelper.dollarAmount(row.DistributionAmount.toString())
										,FormatPRDate: jHelper.formatDate(jHelper.parseJSONDate(row.PaymentOrReceivedDate.toString()))
						};
						$("#ReconcileTemplate").tmpl(item).appendTo(target);
					}
				},
				error: function (data) { try { if(p.onError) { p.onError(data); } } catch(e) { } }
			});
		} catch(e) {
			alert(e);
		}
		return false;
	}
	,viewMore: function () {
		var pi=$("#PageIndex").val();
		$("#PageIndex").val(parseInt(pi)+1);
		dealReconcile.isViewMore=true;
		this.search();
	}
}
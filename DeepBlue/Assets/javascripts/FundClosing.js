﻿var fundClosing={
	add: function (that) {
		var flexigrid=$(that).parents(".flexigrid:first");
		var row=$("#Row0",flexigrid).get(0);
		if(!row) {
			var tbody=$(".middlec table tbody",flexigrid);
			var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,"","","",false,0]}] };
			$("#GridTemplate").tmpl(data).prependTo(tbody);
			var tr=$("tr:first",tbody);
			fundClosing.setup(tr);
			this.editRow(tr);
			$("#Add",tr).show();
		}
	}
	,edit: function (img) {
		var tr=$(img).parents("tr:first");
		this.editRow(tr);
		$("#Save",tr).show();
	}
	,editRow: function (tr) {
		$(".show",tr).hide();
		$(".hide",tr).show();
		$(":input:first",tr).focus();
	}
	,deleteRow: function (img,id) {
		if(confirm("Are you sure you want to delete this fund closing?")) {
			var tr=$(img).parents("tr:first");
			var imgsrc=img.src;
			$(img).attr("src",jHelper.getImagePath("ajax.jpg"));
			img.src=imgsrc;
			var dt=new Date();
			var url="/Admin/DeleteFundClosing/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					tr.remove();
					jHelper.applyFlexGridClass($(".middlec:first"));
				}
			});
		}
	}
	,save: function (img,id) {
		var tr=$("#Row"+id);
		var param=jHelper.serialize(tr);
		var url="/Admin/UpdateFundClosing";
		var imgsrc=img.src;
		$(img).attr("src",jHelper.getImagePath("ajax.jpg"));
		$.post(url,param,function (data) {
			img.src=imgsrc;
			var arr=data.split("||");
			if(arr[0]!="True") {
				jAlert(data);
			} else {
				$.getJSON("/Admin/EditFundClosing?_"+(new Date).getTime()+"&id="+arr[1],function (loadData) {
					$("#GridTemplate").tmpl(loadData).insertAfter(tr);
					$(tr).remove();
					var newTR=$("#Row"+arr[1]);
					jHelper.applyFlexGridClass($(".middlec:first"));
					jHelper.checkValAttr(newTR);
					fundClosing.setup(newTR);jHelper.gridEditRow(newTR);
				});
			}
		});
	}
	,onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		fundClosing.setup(t);
		jHelper.gridEditRow(t);
	}
	,setup: function (target) {
		$("#FundName",target).each(function () {
			var tr=$(this).parents("tr:first");
			$(this).autocomplete(
			{ source: "/Fund/FindFunds",minLength: 1
			,select: function (event,ui) {
				$("#FundId",tr).val(ui.item.id);
			},appendTo: "body",delay: 300
			}
			);
		});
		jHelper.applyDatePicker(target);
		jHelper.jqCheckBox(target);
	}
	,onInit: function (g) {
		var data={ name: "Add Fund Closing" };
		$("#AddButtonTemplate").tmpl(data).prependTo(g.pDiv);
	}
	,onTemplate: function (tbody,data) {
		$("#GridTemplate").tmpl(data).appendTo(tbody);
	}
	,resizeGV: function (g) {
		var admain=$(".admin-main");
		var bDivBox=$(g.bDivBox);
		bDivBox.css("height","auto");
		var ah=admain.height()-220;
		var h=bDivBox.height();
		if(h>ah) {
			bDivBox.height(ah);
		}
	}
}
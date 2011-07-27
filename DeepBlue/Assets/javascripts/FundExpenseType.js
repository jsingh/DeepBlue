var fundExpenseType={
	add: function (that) {
		var flexigrid=$(that).parents(".flexigrid:first");
		var tbody=$(".bDiv table tbody",flexigrid);
		var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,"",false]}] };
		$("#GridTemplate").tmpl(data).prependTo(tbody);
		var tr=$("tr:first",tbody);
		this.editRow(tr);
		$("#Add",tr).show();
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
		if(confirm("Are you sure you want to delete this fund expense type?")) {
			var tr=$(img).parents("tr:first");
			var imgsrc=img.src;
			$(img).attr("src","/Assets/images/ajax.jpg");
			img.src=imgsrc;
			var dt=new Date();
			var url="/Admin/DeleteFundExpenseType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					tr.remove();
				}
			});
		}
	}
	,save: function (img,id) {
		var tr=$("#Row"+id);
		var param=jHelper.serialize(tr);
		var url="/Admin/UpdateFundExpenseType";
		var imgsrc=img.src;
		$(img).attr("src","/Assets/images/ajax.jpg");
		$.post(url,param,function (data) {
			img.src=imgsrc;
			var arr=data.split("||");
			if(arr[0]!="True") {
				alert(data);
			} else {
				$.getJSON("/Admin/EditFundExpenseType?_"+(new Date).getTime()+"&id="+arr[1],function (loadData) {
					$("#GridTemplate").tmpl(loadData).insertAfter(tr);
					$(tr).remove();
					var newTR=$("#Row"+arr[1]);
					jHelper.checkValAttr(newTR);
				});
			}
		});
	}
	,onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		$(window).resize();
	}
	,onInit: function (g) {
		var data={ name: "Add Fund Expense Type" };
		$("#AddButtonTemplate").tmpl(data).prependTo(g.pDiv);
//		$(window).resize(function () {
//			fundExpenseType.resizeGV(g);
//		});
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
var entityMenu={
	add: function (that) {
		var flexigrid=$(that).parents(".flexigrid:first");
		var row=$("#Row0",flexigrid).get(0);
		if(!row) {
			var tbody=$(".middlec table tbody",flexigrid);
			var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,"",0,"","",""]}] };
			$("#GridTemplate").tmpl(data).prependTo(tbody);
			var tr=$("tr:first",tbody);
			entityMenu.setParentMenuSearch(tr);
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
		if(confirm("Are you sure you want to delete this entity menu?")) {
			var tr=$(img).parents("tr:first");
			var imgsrc=img.src;
			$(img).attr("src",jHelper.getImagePath("ajax.jpg"));
			img.src=imgsrc;
			var dt=new Date();
			var url="/Admin/DeleteEntityMenu/"+id+"?t="+dt.getTime();
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
		var url="/Admin/UpdateEntityMenu";
		var imgsrc=img.src;
		$(img).attr("src",jHelper.getImagePath("ajax.jpg"));
		$.post(url,param,function (data) {
			img.src=imgsrc;
			var arr=data.split("||");
			if(arr[0]!="True") {
				jAlert(data);
			} else {
				$.getJSON("/Admin/EditEntityMenu?_"+(new Date).getTime()+"&id="+arr[1],function (loadData) {
					$("#GridTemplate").tmpl(loadData).insertAfter(tr);
					$(tr).remove();
					var newTR=$("#Row"+arr[1]);
					jHelper.applyFlexGridClass($(".middlec:first"));
					entityMenu.setParentMenuSearch(newTR);
					jHelper.checkValAttr(newTR);entityMenu.gridEditRow(newTR);
				});
			}
		});
	}
	,onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		entityMenu.gridEditRow(t);
		entityMenu.setParentMenuSearch(t);
	}
	,updateSortOrder: function (tr,prevtr) {
		var entityMenuID=$("#EntityMenuID",tr).val();
		var alterEntityMenuID=$("#EntityMenuID",prevtr).val();
		var params=new Array();
		params[params.length]={ "name": "entityMenuID","value": entityMenuID };
		params[params.length]={ "name": "alterEntityMenuID","value": alterEntityMenuID };
		$.post("/Admin/UpdateEntityMenuSortOrder",params);
	}
	,sortUp: function (that) {
		var tr=$(that).parents("tr:first");
		var t=$(tr).parents("table:first");
		var prevtr=$(tr).prev();
		var th=$("th",prevtr).length;
		if(th>0) {
			return;
		}
		$(prevtr).before(tr);
		var entityID=$("#EntityMenuID",tr);
		jHelper.applyFlexGridClass(t);
		entityMenu.updateSortOrder(tr,prevtr);
	}
	,sortDown: function (that) {
		var tr=$(that).parents("tr:first");
		var t=$(tr).parents("table:first");
		var prevtr=$(tr).next();
		var th=$("th",prevtr).length;
		if(th>0) {
			return;
		}
		$(prevtr).after(tr);
		jHelper.applyFlexGridClass(t);
		entityMenu.updateSortOrder(tr,prevtr);
	}
	,gridEditRow: function (target) {
		/*
		$("td",target)
		.each(function () {
		var td=$(this);
		td.css("cursor","pointer");
		var img=$(".editbtn",td).get(0);
		if(!img) {
		td.click(function () {
		var l=$(":input:visible",this).length;
		if(l<=0) {
		var tr=$(this).parents("tr:first");
		$(".editbtn",tr).click();
		}
		});
		}
		});
		*/
	}
	,setParentMenuSearch: function (target) {
		$("#MenuName",target)
		.each(function () {
			var txt=$(this);
			var tr=$(txt).parents("tr:first");
			var menuID=$("#MenuID",tr);
			txt
			.autocomplete({
				appendTo: "body"
				,minLength: 1
				,delay: 300
				,select: function (event,ui) { menuID.val(ui.item.id); }
				,source: function (request,response) {
					$.getJSON("/Admin/FindMenus"+"?term="+request.term,function (data) {
						response(data);
					});
				}
			});
		});
	}
	,onInit: function (g) {
		var data={ name: "Add Menu" };
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
 
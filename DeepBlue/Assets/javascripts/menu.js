var menu={
	timeout: 500
	,closetimer: 0
	,ddmenuitem: 0
	,subclosetimer: 0
	,subddmenuitem: 0
	,stopMenuClose: false
	,mopen: function (that,id) {
		menu.mcancelclosetime();
		if(menu.ddmenuitem) { menu.ddmenuitem.style.display='none'; }
		$(".mdiv").hide();
		$(".subul").hide();
		menu.ddmenuitem=document.getElementById(id);
		if(menu.ddmenuitem) {
			menu.ddmenuitem.style.display='block';
		}
	}
	,msubopen: function (that,id) {
		// cancel close timer
		menu.msubcancelclosetime();
		menu.mcancelclosetime();
		var smenu=document.getElementById(id);
		if($.trim($(smenu).html())!="") {
			$(".innersub-select").hide().removeClass("innersub-select");
			if(id!='') {
				if(menu.subddmenuitem) menu.subddmenuitem.style.display='none';
				$(that).parents(".mdiv:first").addClass("subext");
				menu.subddmenuitem=document.getElementById(id);
				menu.subddmenuitem.style.display='block';
			} else {
				$(".subul").hide();$(".subext").each(function () { $(this).removeClass("subext"); });
			}
		}
	}
	,mclose: function () {
		return;
		if(menu.ddmenuitem) {
			menu.ddmenuitem.style.display='none';
			$("#arrow").hide();
			$(".mdiv").hide();$(".subul").hide();
			$(".current").css("display","block");
		}
	}
	,mclickclose: function () {
		if(menu.ddmenuitem) { menu.ddmenuitem.style.display='none';$("#arrow").hide(); }
	}
	,mclosetime: function () {
		menu.closetimer=window.setTimeout(menu.mclose,menu.timeout);
	}
	,mcancelclosetime: function () {
		if(menu.closetimer) {
			window.clearTimeout(menu.closetimer);
			menu.closetimer=null;
		}
	}
	,msubclose: function () {
		$(".subext").each(function () { $(this).removeClass("subext"); });
	}
	,msubclosetime: function () {
		//menu.subclosetimer=window.setTimeout(menu.msubclose,timeout);
	}
	,msubcancelclosetime: function () {
		if(menu.subclosetimer) {
			window.clearTimeout(menu.subclosetimer);
			menu.subclosetimer=null;
		}
	}
	,add: function (that) {
		var flexigrid=$(that).parents(".flexigrid:first");
		var row=$("#Row0",flexigrid).get(0);
		if(!row) {
			var tbody=$(".middlec table tbody",flexigrid);
			var data={ "page": 0,"total": 0,"rows": [{ "cell": [0,"",0,"","",""]}] };
			$("#GridTemplate").tmpl(data).prependTo(tbody);
			var tr=$("tr:first",tbody);
			menu.setParentMenuSearch(tr);
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
		if(confirm("Are you sure you want to delete this menu?")) {
			var tr=$(img).parents("tr:first");
			var imgsrc=img.src;
			$(img).attr("src",jHelper.getImagePath("ajax.jpg"));
			img.src=imgsrc;
			var dt=new Date();
			var url="/Admin/DeleteMenu/"+id+"?t="+dt.getTime();
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
		var url="/Admin/UpdateMenu";
		var imgsrc=img.src;
		$(img).attr("src",jHelper.getImagePath("ajax.jpg"));
		$.each(param,function (i,p) {
			if(p.name=="ParentMenuID"&&p.value=="0") {
				p.value="";
			}
		});
		$.post(url,param,function (data) {
			img.src=imgsrc;
			var arr=data.split("||");
			if(arr[0]!="True") {
				jAlert(data);
			} else {
				$.getJSON("/Admin/EditMenu?_"+(new Date).getTime()+"&id="+arr[1],function (loadData) {
					$("#GridTemplate").tmpl(loadData).insertAfter(tr);
					$(tr).remove();
					var newTR=$("#Row"+arr[1]);
					jHelper.applyFlexGridClass($(".middlec:first"));
					menu.setParentMenuSearch(newTR);
					jHelper.checkValAttr(newTR);jHelper.gridEditRow(newTR);
				});
			}
		});
	}
	,onGridSuccess: function (t,g) {
		jHelper.checkValAttr(t);
		jHelper.gridEditRow(t);
		menu.setParentMenuSearch(t);
	}
	,setParentMenuSearch: function (target) {
		$("#ParentMenuName",target)
		.each(function () {
			var txt=$(this);
			var tr=$(txt).parents("tr:first");
			var parentMenuID=$("#ParentMenuID",tr);
			var menuID=$("#MenuID",tr);
			txt
			.autocomplete({
				appendTo: "body"
				,minLength: 1
				,delay: 300
				,select: function (event,ui) { parentMenuID.val(ui.item.id); }
				,source: function (request,response) {
					$.getJSON("/Admin/FindMenus"+"?term="+request.term+"&menuID="+menuID.val(),function (data) {
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

//document.onclick=mclickclose;
 
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
}

//document.onclick=mclickclose;
 
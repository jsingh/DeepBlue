var deepBlue={
	indexPage: false
	,init: function () {
		$(document).ready(function () {
			var cnt=$("#content");
			var menu=$("#menu");
			var submenu=$("#submenu");
			var topheader=$("#topheader");
			var mnuresize=$.cookie("mnu-resize");
			topheader.click(function () { deepBlue.hideSubMenu(); });
			cnt.click(function () { deepBlue.hideSubMenu(); });
			submenu.hide();
			if(deepBlue.indexPage) {
				submenu.show();
			}
			if(mnuresize=="true") {
				menu.addClass("minimize");
				$("#navminimize").addClass("downarrow");
			}
			deepBlue.setCookie("mnu-resize",mnuresize);
			$(".topmenu").click(function () {
				submenu.show();
				deepBlue.resize();
				$(".tab-sel").removeClass("tab-sel");
				var showmenu=$(this);
				var arrow=$("#arrow");
				arrow.hide();
				if(showmenu.attr("nosubmenu")!="true") {
					showmenu.addClass("tab-sel");
					deepBlue.setArrow(showmenu);
				}
			});
			$(".innersub-select").each(function () {
				$(this).parents(".sub-select:first").addClass("subext");
			});
			deepBlue.setArrow($(".tab-sel:first"));
			$(window).resize(function () {
				setTimeout(function () {
					deepBlue.resize();
				});
			});
			deepBlue.resize();
		});
	}
	,hideSubMenu: function () {
		$("#submenu").hide();deepBlue.resize();
	}
	,minimize: function (that) {
		var menu=$("#menu");
		if(menu.hasClass("minimize")) {
			menu.removeClass("minimize");
			$(that).removeClass("downarrow");
		} else {
			menu.addClass("minimize");
			$(that).addClass("downarrow");
		}
		deepBlue.setCookie("mnu-resize",menu.hasClass("minimize"));
		deepBlue.resize();
	}
	,setCookie: function (name,value) {
		$.cookie(name,value,{ expires: 7 });
	}
	,resize: function () {
		var menu=$("#menu");
		var cnt=$("#content");
		var leftmenu=$("#leftmenu");
		var pos=cnt.offset();
		var h=($(window).height()-pos.top)-35;
		var cntheight=cnt.height();
		cnt.css("min-height",h);
		leftmenu.css("min-height",h-47);
	}
	,setArrow: function (showmenu) {
		if(showmenu.get(0)) {
			var pos=showmenu.offset();
			var arrow=$("#arrow");
			arrow.show();
			arrow.css({ "left": pos.left+((showmenu.width()/2)) });
		}
	}
}
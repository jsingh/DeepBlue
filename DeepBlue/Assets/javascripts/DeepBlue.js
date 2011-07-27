var deepBlue={
	init: function () {
		$(document).ready(function () {
			var menu=$("#menu");
			var mnuresize=$.cookie("mnu-resize");
			if(mnuresize=="true") {
				menu.addClass("minimize");
				$("#navminimize").removeClass("downarrow");
			}
			deepBlue.setCookie("mnu-resize",mnuresize);
			$(".topmenu").click(function () {
				$(".tab-sel").each(function () {
					//if($(this).hasClass("current")==false) {
					$(this).removeClass("tab-sel");
					// }
				});
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
	,minimize: function (that) {
		var menu=$("#menu");
		if(menu.hasClass("minimize")) {
			menu.removeClass("minimize");
			$(that).addClass("downarrow");
		} else {
			menu.addClass("minimize");
			$(that).removeClass("downarrow");
		}
		deepBlue.setCookie("mnu-resize",menu.hasClass("minimize"));
		deepBlue.resize();
	}
	,setCookie: function (name,value) {
		$.cookie(name,value,{ expires: 7 });
	}
	,resize: function () {
		var menu=$("#menu");
		var h=($(window).height()-158);
		if(menu.hasClass("minimize")) {
			h=h+10;
		} else {
			h=h-36;
		}
		var cnt=$("#content");
		var cntheight=cnt.height();
		cnt.css("min-height",h);
		var admain=$(".admin-main:first");
		var leftmenu=$("#leftmenu");
		admain.width(($(window).width()-leftmenu.width())-55);
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
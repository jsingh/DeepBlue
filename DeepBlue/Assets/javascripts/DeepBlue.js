var DeepBlue={
	init: function () {
		$(document).ready(function () {
			$(".topmenu").click(function () {
				$(".tab-sel").each(function () {
					if($(this).hasClass("current")==false) { $(this).removeClass("tab-sel") }
				});
				var showmenu=$(this);
				var arrow=$("#arrow");
				arrow.hide();
				if(showmenu.attr("nosubmenu")!="true") {
					showmenu.addClass("tab-sel");
					DeepBlue.setArrow(showmenu);
				}
			});
			$(".innersub-select").each(function () {
				$(this).parents(".sub-select:first").addClass("subext");
			});
			DeepBlue.setArrow($(".tab-sel:first"));
			DeepBlue.layout();
		});
	}
	,setArrow: function (showmenu) {
		if(showmenu.get(0)) {
			var pos=showmenu.offset();
			var arrow=$("#arrow");
			arrow.show();
			arrow.css({ "left": pos.left+((showmenu.width()/2)) });
		}
	}
	,layout: function () {
		var header=document.getElementById("header");
		var submenu=document.getElementById("submenu");
		var content=document.getElementById("content");
		content.style.top=$(header).height()+$(submenu).height()+"px";
	}
}
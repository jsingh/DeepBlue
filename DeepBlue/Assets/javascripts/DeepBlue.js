var deepBlue={
	indexPage: false
	,topMenuResize: false
	,init: function () {
		$(document).ready(function () {
			var cnt=$("#content");
			var menu=$("#menu");
			var submenu=$("#submenu");
			var topheader=$("#topheader");
			var mnuresize=deepBlue.getCookie("mnu-resize");

			//topheader.click(function () { deepBlue.hideSubMenu(); });
			//cnt.click(function () { deepBlue.hideSubMenu(); });
			submenu.hide();
			if(mnuresize=="true") {
				menu.addClass("minimize");
				$("#navminimize").addClass("downarrow");
			}
			deepBlue.setCookie("mnu-resize",menu.hasClass("minimize"));
			deepBlue.showSubMenu();
			if(deepBlue.indexPage) {
				submenu.show();
			}
			$(".topmenu").click(function () {
				submenu.show();
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
			deepBlue.ajaxSetup();
		});
	}
	,ajaxSetup: function () {
		$.ajaxSetup({
			cache: false
			,complete: function (jqXHR,textStatus) {
				if(textStatus=="timeout") {
					jAlert("Ajax request is timeout");
				}
			}
			,beforeSend: function (jqXHR,settings) {
				if(settings.url.indexOf("/Assets/javascripts")>0) {
					if(settings.url.indexOf("LogOn.js")>0) {
						$.get("/Account/LogOff");
						deepBlue.redirectLogOn();
					}
					return false;
				}
			}
			,error: function (x,jqxhr,settings,exception) {
				if(x.responseText.indexOf('WILLOWRIDGE :: LogOn')>0) {
					$.get("/Account/LogOff");
					deepBlue.redirectLogOn();
				} else {
					if(x.status==0) {
						jAlert('You are offline!!\n Please Check Your Network.');
					} else if(x.status==404) {
						jAlert('Requested URL not found.');
					} else if(x.status==500) {
						jAlert('Internel Server Error.');
					} else if(e=='parsererror') {
						jAlert('Error.\nParsing JSON Request failed.');
					} else if(e=='timeout') {
						jAlert('Request Time out.');
					} else {
						jAlert('Unknow Error.\n'+x.responseText);
					}
				}
			}
		});
	}
	,redirectLogOn: function () {
		if(parent) {
			parent.window.location.href="/Account/LogOn";
		} else {
			location.href="/Account/LogOn";
		}
	}
	,hideSubMenu: function () {
		$("#submenu").hide();
	}
	,showSubMenu: function () {
		var cnt=$("#content");
		var submenu=$("#submenu");
		var topheader=$("#topheader");
		var mnuresize=deepBlue.getCookie("mnu-resize");

		if(mnuresize=="true") {
			submenu.hide();
			topheader.click(function () { deepBlue.hideSubMenu(); });
			cnt.click(function () { deepBlue.hideSubMenu(); });
		} else {
			topheader.unbind('click');
			cnt.unbind('click');
			submenu.show();
		}
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
		deepBlue.showSubMenu();
	}
	,setCookie: function (name,value) {
		var expiredays=365;
		var expireDate=new Date();
		expireDate.setTime(expireDate.getTime()+(expiredays*24*3600*1000));
		document.cookie=name+"="+escape(value)+
		((expiredays==null)?"":"; expires="+expireDate.toGMTString()+"; domain="+escape(document.domain)+"; path=/");
	}
	,getCookie: function (name) {
		if(document.cookie.length>0) {
			begin=document.cookie.indexOf(name+"=");
			if(begin!= -1) {
				begin+=name.length+1;
				end=document.cookie.indexOf(";",begin);
				if(end== -1) end=document.cookie.length;
				return unescape(document.cookie.substring(begin,end));
			}
		}
		return null;
	}
	,resize: function () {
		var menu=$("#menu");
		var cnt=$("#content");
		var leftmenu=$("#leftmenu");
		var pos=cnt.offset();
		var h=($(window).height()-pos.top)-35;
		var cntheight=cnt.height();
		cnt.css("min-height",h);
		var w=$(window).width();
		//$(".admin-main").width(w-282);
		//leftmenu.css("min-height",h-35);
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
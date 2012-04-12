$(document).ready(function(){
	deepBlue.init();
});
var deepBlue={
	indexPage: false
	,rootUrl:""
	,topMenuResize: false
	,init: function () {
		deepBlue.ajaxSetup();
	}
	,ajaxSetup: function () {
		$.ajaxSetup({
			cache: false
			,dataType: "html"
			,complete: function (jqXHR,textStatus) {
				if(jqXHR.responseText.indexOf("/LogOn.js")>0) {
					deepBlue.redirectLogOn();
				} else {
					if(textStatus=="timeout") {
						jAlert("Ajax request is timeout");
					}
				}
			}
			,error: function (x,jqxhr,settings,exception) {
				if(x.responseText.indexOf(':: LogOn')>0) {
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
			parent.window.location.href=deepBlue.rootUrl+"/Account/LogOn";
		} else {
			location.href=deepBlue.rootUrl+"/Account/LogOn";
		}
	}
}
$.ajaxPrefilter( function( options ) {
	if(deepBlue.rootUrl!=""){
		if(options.url.indexOf(deepBlue.rootUrl)<0){
			options.url = deepBlue.rootUrl + options.url;
		}
	}
});
window.onunload=function () { $("#LoadingPage").show(); };

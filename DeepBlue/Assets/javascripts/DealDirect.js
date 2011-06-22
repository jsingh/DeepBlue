var dealDirect={
}


,selectTab: function (type,lnk) {
alert("hai");
		var UA=$("#EQdetail");
		var SA=$("#FixedIncome");
		var SUD=$("#SearchUDirect");
		UA.hide();SA.hide();SUD.hide();
		$(".tablnk").removeClass("select");$(lnk).addClass("select");
		switch(type) {
			case "E": UA.show();break;
			case "F": SA.show();SUD.show();break;
		}
	}
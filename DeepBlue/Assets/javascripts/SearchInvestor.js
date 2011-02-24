var searchInvestor={
	menuName: ''
	,onBeginSearch: function () {
		$("#investorlist").css("display","");
		$("#SearchDetail").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	},
	onSuccess: function (investors) {
		var i;
		var $searchDetail=$("#SearchDetail");
		$searchDetail.html("");
		for(i=0;i<investors.length;i++) {
			if(i==0) {
				$searchDetail.append('<div class="searchTitle">Investors:</div>');
			}
			var lnk='';
			if(searchInvestor.menuName=="Transaction")
				lnk="/Transaction/New/"+investors[i].id;
			else
				lnk="/Investor/Edit/"+investors[i].id;
			$searchDetail.append("<li><a href='"+lnk+"'>"+investors[i].label+"</a></li>");
		}
	}
	,onSubmit: function (frm,menuName) {
		searchInvestor.menuName=menuName;
		searchInvestor.onBeginSearch();
		$.getJSON("/Investor/FindInvestors/?term="+$("#search",frm).val(),function (data) {
			searchInvestor.onSuccess(data);
		});
		return false;
	}
    ,onKeyUp: function (txt,menuName) {
    	if(txt.value.length>=1) {
    		var frm=$(txt).parents("form").get(0);
    		searchInvestor.onSubmit(frm,menuName);
    	}
    	else {
    		$("#SearchDetail").hide();
    	}
    }

}
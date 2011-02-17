var Home={
	menuName: '',
	init: function () {
		$(document).ready(function () {
			$("#home_menu").get(0).className="tab_unsel";
			$("#member_menu").get(0).className="tab_unsel";
			switch(Home.menuName) {
				case "Home":
					$("#home_menu").get(0).className="tab_sel";
					break;
				case "Member":
					$("#member_menu").get(0).className="tab_sel";
					break;
			}
		});
	}
};
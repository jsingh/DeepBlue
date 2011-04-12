var DeepBlue={
	init: function () {
		$(document).ready(function () {
			var layoutSettings=
    {
    	Name: "Main",
    	Dock: $.layoutEngine.DOCK.FILL,
    	EleID: "DepMain",
    	Margin: 0,
    	Children: [
                        {
                        	Name: "Top",
                        	Dock: $.layoutEngine.DOCK.TOP,
                        	EleID: "header",
                        	Margin: 0,
                        	Height: 100
                        }
                        ,{
                        	Name: "Fill",
                        	Dock: $.layoutEngine.DOCK.FILL,
                        	EleID: "content",
                        	Margin: 0
                        }
                        ,{
                        	Name: "Bottom",
                        	Dock: $.layoutEngine.DOCK.BOTTOM,
                        	EleID: "footer",
                        	Margin: 0,
                        	Height: 35
                        }
                    ]
    };
			$.layoutEngine(layoutSettings);
		});
	}
}
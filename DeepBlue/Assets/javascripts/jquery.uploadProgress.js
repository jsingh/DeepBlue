(function ($) {

	$.fn.uploadProgress=function (settings) {
		/// <summary>Add ajax upload with a progress bar to form</summary>
		var config={
			id: Math.floor(Math.random()*10000000)
			,url: "/Progress/"
			,onBeforeSubmit: null
			,onProgress: null
			,onSuccess: null
		};

		if(settings) $.extend(config,settings);

		var stopChecProgress=false;

		var checkProgress=function ($form) {
			/// <summary>Check on and show progress</summary>
			$.getJSON(addProgressId(config.url,$form.data("id")),null,function (data,success) {
				if(config.onProgress) {
					config.onProgress(data);
				}
				if(stopChecProgress==false) {
					checkProgress($form);
				}
			});
		}
		var beforeSubmit=function () {
			if(config.onBeforeSubmit) {
				config.onBeforeSubmit();
			}
		}

		var addProgressId=function (url,id) {
			/// <summary>Check for and add if need the ?</summary>
			return url.concat(url.indexOf("?")> -1?"":"?","progressId=",id)
		}

		this.each(function () {
			var $form=$(this);
			var id=config.id++;
			$form
                .data("id",id)
                .ajaxForm({
                	form: $form,
                	url: addProgressId($form.attr('action'),$form.data("id")),
                	beforeSubmit: function (data,$form,config) {
                		stopChecProgress=false;
                		beforeSubmit();
                		setTimeout(function () {
                			checkProgress($form);
                		},1000);
                	},
                	success: function (data) {
                		stopChecProgress=true;
                		if(config.onSuccess) {
                			config.onSuccess(data);
                		}
                	}
                });

		});

		return this;
	}

})(jQuery);

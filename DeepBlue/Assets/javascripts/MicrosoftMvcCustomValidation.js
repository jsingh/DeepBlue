﻿Sys.Mvc.ValidatorRegistry.validators.remoteVal=function (rule) {
	var url=rule.ValidationParameters.url;
	var validateParameterName=rule.ValidationParameters.validateParameterName;
	var parameterNames=rule.ValidationParameters.parameterNames;
	return function (value,context) {  // anonymous function
		if(!value||!value.length) {
			return true;
		}

		if(context.eventName=='blur') {
			var newUrl=((url.indexOf('?')<0)?(url+'?'):(url+'&'))
            +encodeURIComponent(validateParameterName)+'='+encodeURIComponent(value);
			if(parameterNames!="") {
				var arr=parameterNames.split(",");
				var i;
				for(i=0;i<arr.length;i++) {
					newUrl+="&"+encodeURIComponent(arr[i])+'='+$('#'+arr[i]).val();
				}
			}
			var completedCallback=function (executor) {
				if(executor.get_statusCode()!=200) {
					return; // there was an error
				}

				var responseData=executor.get_responseData();
				if(responseData!='OK') {
					// add error to validation message
					var newMessage=(responseData=='FAIL'?
                    rule.ErrorMessage:responseData);
					if(newMessage!='') {
						alert(newMessage);
					}
					context.fieldContext.addError(newMessage);
				}
			};

			var r=new Sys.Net.WebRequest();
			r.set_url(newUrl);
			r.set_httpVerb('GET');
			r.add_completed(completedCallback);
			r.invoke();
			return true; // optimistically assume success
		}
	};
};



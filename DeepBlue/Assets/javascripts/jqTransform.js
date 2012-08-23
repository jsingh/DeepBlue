(function($) {

	$.fn.jqCheckBox=function() {
		return this.each(function() {
			if($(this).attr("nojqTranform")!="true") {

				if($(this).hasClass('jqHidden')) { return; }

				var $input=$(this);
				var inputSelf=this;

				var display=$input.attr("display");
				var align=$input.attr("align");
				var displayWidth=$input.attr("displaywidth");

				var selectCls="jqCheckBox-Select";
				var cls="jqCheckBox";

				var yes=document.createElement("div");
				var no=document.createElement("div");

				yes.innerHTML="Yes";
				no.innerHTML="No";

				var $yes=$(yes);
				var $no=$(no);

				$yes.addClass(cls).addClass("jqCR");
				$no.addClass(cls);

				if($input.hasClass("hide")) {
					$yes.addClass("hide");
					$no.addClass("hide");
					$input.removeClass("hide").hide();
				}

				$input.before(yes);
				$yes.after(no);

				if($.trim(display)!="") {
					var disp=document.createElement("div");
					disp.innerHTML=display;
					disp.className="jqCDisplay";
					if($.trim(displayWidth)!="")
						$(disp).css("width",displayWidth);

					$yes.before(disp);
				}

				$yes.unbind('click').click(function() {
					if(inputSelf.checked) return false;
					// Toggle checkbox checked status.
					inputSelf.checked=!inputSelf.checked;
					// Trigger ONLY click event hanlders on the checkbox.
					$input.triggerHandler("click");
					setClass();
					return false;
				});

				$no.unbind('click').click(function() {
					if(inputSelf.checked==false) return false;
					// Toggle checkbox checked status.
					inputSelf.checked=!inputSelf.checked;
					// Trigger ONLY click event hanlders on the checkbox.
					$input.triggerHandler("click");
					setClass();
					return false;
				});

				var setClass=function() {
					if(inputSelf.checked) {
						$yes.addClass(selectCls);
						$no.removeClass(selectCls);
					} else {
						$no.addClass(selectCls);
						$yes.removeClass(selectCls);
					}
				}
				setClass();
				$input.addClass("jqHidden");
			}
		});
	};

	$.fn.jqTransCheckBox=function() {
		return this.each(function() {

			if($(this).attr("nojnice")!="true") {

				if($(this).hasClass('jqTransformHidden')) { return; }

				var $input=$(this);
				var inputSelf=this;

				//set the click on the label
				//var oLabel = jqTransformGetLabel($input);
				//oLabel && oLabel.click(function() { aLink.trigger('click'); });
				var aLink=$('<a href="#" class="jqTransformCheckbox"></a>');
				//wrap and add the link
				$input.addClass('jqTransformHidden').wrap('<span class="jqTransformCheckboxWrapper"></span>').parent().prepend(aLink);
				//on change, change the class of the link
				$input.change(function() {
					this.checked&&aLink.addClass('jqTransformChecked')||aLink.removeClass('jqTransformChecked');
					return true;
				});
				// Click Handler, trigger the click and change event on the input
				aLink.click(function() {
					//do nothing if the original input is disabled
					if($input.attr('disabled')) { return false; }
					//trigger the envents on the input object

					inputSelf.checked=!inputSelf.checked;
					$input.triggerHandler("click");
					$input.trigger("change");

					//$input.trigger('click').trigger("change");
					return false;
				});

				//            // set the default state
				this.checked&&aLink.addClass('jqTransformChecked');

			}
		});
	};

})(jQuery);
 
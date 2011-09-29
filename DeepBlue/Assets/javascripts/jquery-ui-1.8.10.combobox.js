(function ($) {
	$.widget("ui.combobox",{
		_create: function () {
			var self=this,
					select=this.element.hide(),
					selected=select.children(":selected"),
					value=selected.val()?selected.text():"";

			var ul=$("#refDropDown");
			if(!ul.get(0)) {
				ul=$("<ul id='refDropDown' class='contextMenu'><li class='refresh'><a href='#refresh'>Refresh</a></li></ul>");
				$("body").append(ul);
			}

			var cbox=this.cbox=$("<div class='jqComboBox'>")
			.insertAfter(select);

			var cleft=$("<div class='left'>");
			var input=this.input=$("<input selectid='"+select.attr("id")+"' id='jqCBSTextBox_"+select.attr("name")+"' name='jqCBSTextBox_"+select.attr("name")+"'>");
			cbox.append(cleft);
			cleft.append(input);



			var w=$(select).css("width");
			if(w.indexOf("px")>0) {
				w=parseInt($.trim(w.replace("px","")))-34;
				if(w>0) {
					$(input).css("width",w);
				}
			}
			if(select.hasClass("hide")) {
				select.removeClass("hide");
				select.hide();
				$(cbox).addClass("hide");
			}
			input
				.val(value)
				.autocomplete({
					delay: 0,
					minLength: 0,
					source: function (request,response) {
						var matcher=new RegExp($.ui.autocomplete.escapeRegex(request.term),"i");
						response(select.children("option").map(function () {
							var text=$(this).text();
							if(this.value&&(!request.term||matcher.test(text)))
								return {
									label: text.replace(
										new RegExp(
											"(?![^&;]+;)(?!<[^<>]*)("+
											$.ui.autocomplete.escapeRegex(request.term)+
											")(?![^<>]*>)(?![^&;]+;)","gi"
										),"<strong>$1</strong>"),
									value: text,
									option: this
								};
						}));
					},
					select: function (event,ui) {
						if(ui.item.option) {
							ui.item.option.selected=true;
							self._trigger("selected",event,{
								item: ui.item.option
							});
							$(select).change();
						} else {
							$("option:selected",select).each(function () {
								this.selected=false;
							});
							input.val("");
							$(select).change();
						}
					}
				});

			input.data("autocomplete")._renderItem=function (ul,item) {
				return $("<li></li>")
						.data("item.autocomplete",item)
						.append("<a>"+item.label+"</a>")
						.appendTo(ul);
			};

			var rightbtn=this.button=$("<div>")
					.insertAfter(cleft)
					.addClass("right")
					.click(function () {
						// close if already visible
						if(input.autocomplete("widget").is(":visible")) {
							input.autocomplete("close");
							return;
						}

						// work around a bug (likely same cause as #5265)
						$(this).blur();

						// pass empty string as value to search for, displaying all results
						input.autocomplete("search","");
						input.focus();
					});

			var refresh=select.attr("refresh");
			if(refresh=="true") {
				var actionName=select.attr("action");
				cbox
					.contextMenu({
						menu: 'refDropDown'
					},
					function (action,el,pos) {
						if(action=="refresh") {
							input.val("Loading...");
							$.getJSON("/Admin/SelectList?actionName="+actionName,function (data) {
								input.val("");
								var ddl=select.get(0);
								ddl.options.length=null;
								$.each(data,function (i,item) {
									listItem=new Option(item.Text,item.Value,false,false);
									ddl.options[ddl.options.length]=listItem;
								});
								rightbtn.click();
							});
						}
					});
			}
		},

		destroy: function () {
			try {
				this.input.remove();
				this.button.remove();
				this.cbox.remove();
				this.element.show();
				$.Widget.prototype.destroy.call(this);
			} catch(e) {
				jAlert(e);
			}
		}

		,remove: function () {
			this.cbox.remove();
		}

		,hide: function () {
			this.cbox.hide();
		}
		,show: function () {
			this.cbox.show();
		}
	});
})(jQuery);
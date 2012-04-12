(function ($) {
	$.addFlex=function (t,p) {
		if(t.grid) { return false; }
		p=$.extend({
			height: 0
			,url: false
			,method: 'POST'
			,dataType: 'json'
			,errormsg: 'Connection Error'
			,usepager: false
			,page: 1
			,total: 1
			,useRp: true
			,rp: 25
			,rpOptions: [25,50,100,200]
			,title: false
			,pagestat: 'Displaying {from} to {to} of {total} items'
			,procmsg: 'Loading...'
			,nomsg: 'No items'
			,hideOnSubmit: true
			,autoload: true
			,blockOpacity: 0.5
			,sortname: ''
			,sortorder: ''
			,resizeWidth: true
			,tableName: ''
			,exportExcel: false
			,onChangeSort: false
			,onSuccess: false
			,onRowClick: false
			,onRowBound: false
			,onSubmit: false
			,onInit: false
			,onBeforeAddData: false
			,onTemplate: false
			,width: 0
			,useBoxStyle: true
		},p);
		$(t).show();
		var g={
			hset: {}
			,isChangeSort: false
			,gridpaging: null
			,addData: function (data) {
				if(p.onBeforeAddData) { p.onBeforeAddData(data); }
				if(p.preProcess) { data=p.preProcess(data); }
				if(p.dataType=='xml') { p.total= +$('rows total',data).text(); } else { p.total=data.total; }
				var tbody=document.createElement('tbody');
				this.buildpager(data);
				if(p.onTemplate) {
					p.onTemplate(tbody,data);
				}
				$('tr',t).unbind();
				$("tbody",t).remove();
				$(t).append(tbody);
				tbody=null;data=null;
				if(p.onSuccess) { p.onSuccess(t,g); }
				g.isChangeSort=false;
			},
			changeSort: function (th) {
				if(this.loading) { return true; }
				if(p.sortname==$(th).attr('sortname')) {
					if(p.sortorder=='asc') { p.sortorder='desc'; }
					else { p.sortorder='asc'; }
				}
				$(th).addClass('sorted').siblings().removeClass('sorted');
				$('.sdesc',this.bDiv).removeClass('sdesc');
				$('.sasc',this.bDiv).removeClass('sasc');
				$('div span',th).addClass('s'+p.sortorder);
				p.sortname=$(th).attr('sortname');
				g.isChangeSort=true;
				if(p.onChangeSort) {
					p.onChangeSort(p.sortname,p.sortorder);
				} else {
					this.populate();
				}
			}
			,buildpager: function (data) {
				try {
					if(p.usepager==false) {
					  	$(g.gridpaging).remove();
						return;
					}
					if((data.total>=p.rp)==false) {
						$(g.gridpaging).remove();
						return;
					}
					else {
						data.totalpages=Math.ceil(p.total/p.rp);
						var gridpaging=$("<div class='paging control-group'></div>");
						var tmpl="";
						tmpl+="<div class='control-group pull-left'>";
						tmpl+="<select id='rows' class='input-mini'>";
						$.each(p.rpOptions,function (i,row) {
							tmpl+="<option value='"+row+"'>"+row+"</option>";
						});
						tmpl+="</select>";
						tmpl+="</div>";
						tmpl+="<div class='control-group pull-right'>";
						tmpl+="Displaying "+(((p.newp-1)*p.rp)+1)+" to "+(p.newp*p.rp)+" of "+data.total+" items&nbsp;&nbsp;";
						var disabled="";
						if(p.newp==1) { disabled="disabled"; }
						tmpl+="<a href='#' index='1' id='first' class='btn btn-primary "+disabled+"'>First</a>&nbsp;&nbsp;";
						disabled="";
						if(p.newp==1) { disabled="disabled"; }
						tmpl+="<a href='#' index='"+(p.newp-1)+"' id='previous' class='btn btn-primary "+disabled+"'>Previous</a>&nbsp;";

						var iPageCount=5;
						var iPageCountHalf=Math.floor(iPageCount/2);
						var iPages=data.totalpages;
						var iCurrentPage=p.newp;
						var iStartButton,iEndButton,i,iLen;
						/* Pages calculation */
						if(iPages<iPageCount) {
							iStartButton=1;
							iEndButton=iPages;
						}
						else if(iCurrentPage<=iPageCountHalf) {
							iStartButton=1;
							iEndButton=iPageCount;
						}
						else if(iCurrentPage>=(iPages-iPageCountHalf)) {
							iStartButton=iPages-iPageCount+1;
							iEndButton=iPages;
						}
						else {
							iStartButton=iCurrentPage-Math.ceil(iPageCount/2)+1;
							iEndButton=iStartButton+iPageCount-1;
						}
						/* Build the dynamic list */
						var count=0;
						for(i=iStartButton;i<=iEndButton;i++) {
							var selectClass="";
							if(i==p.newp) { selectClass=" select btn-inverse"; }
							tmpl+="<a href='#' index='"+i+"' class='btn  btn-primary"+selectClass+"'>"+i+"</a>&nbsp;";
						}
						disabled="";

						if(p.newp==data.totalpages) { disabled="disabled"; }
						tmpl+="<a href='#' index='"+(p.newp+1)+"' id='next' class='btn btn-primary "+disabled+"'>Next</a>&nbsp;";
						disabled="";
						if(p.newp==data.totalpages) { disabled="disabled"; }
						tmpl+="<a href='#' index='"+(data.totalpages)+"' id='last' class='btn btn-primary "+disabled+"'>Last</a>";
						tmpl+="</div>";
						gridpaging.append(tmpl);
						$(g.gridpaging).remove();
						g.gridpaging = gridpaging;
						$(t).after(gridpaging);
						$("#rows",gridpaging)
						.val(p.rp)
						.change(function () {
							p.rp=this.value;
							g.populate();
						});
						$(".btn:not(.select):not(.disabled)",gridpaging)
						.hover(function () {
							$(this).addClass("btn-inverse");
						},function () {
							$(this).removeClass("btn-inverse");
						})
						.click(function () {
							p.newp=parseInt($(this).attr("index"));
							if(isNaN(p.newp)) { p.newp=0; }
							if(p.newp>0) {
								g.populate();
							}
						})
						;
					}
				} catch(e) {
					alert(e);
				}
			}
			,populate: function () {
				if(p.onSubmit) {
					var gh=p.onSubmit(p);
					if(!gh) { return false; }
				}
				if(!p.url) { return false; }
				if(!p.newp) { p.newp=1; }
				var param=[];
				if(p.usepager) {
					param=[
					 { name: 'pageIndex',value: p.newp }
					,{ name: 'pageSize',value: p.rp }
					,{ name: 'sortName',value: p.sortname }
					,{ name: 'sortOrder',value: p.sortorder }
				];
				}
				if(p.params) {
					for(var pi=0;pi<p.params.length;pi++) param[param.length]=p.params[pi];
				}
				$.ajax({
					type: p.method,
					url: p.url,
					data: param,
					dataType: p.dataType,
					cache: false,
					success: function (data) {
						g.addData(data);
					},
					error: function (data) { try { if(p.onError) { p.onError(data); } } catch(e) { } }
				});
			},
			exportExcel: function () {
			},
			setPagingEvent: function (pDiv) {
			},
			pager: 0
		};
		$('thead tr:first th',t).each
			(
			 	function () {
			 		var thdiv=document.createElement('div');
			 		thdiv.innerHTML="<span>"+this.innerHTML+"</span>";
			 		if($(this).attr('sortname')) {
			 			$(this)
						.addClass("issortcol")
						.click(function (e) { g.changeSort(this); });
			 			var sname=$(this).attr('sortname');
			 			if(sname==p.sortname) {
			 				$(this).addClass("sorted");
			 				if(p.sortorder=='') {
			 					p.sortorder='asc';
			 				}
			 				$("span",thdiv).addClass('s'+p.sortorder);
			 			}
			 		}
			 		if(this.hide) { $(this).hide(); }
			 		var align=$(this).attr("align");
			 		$(thdiv).css("text-align",align);
			 		$(thdiv).width("100%");
			 		$(this).empty().append(thdiv).removeAttr('width');
			 	}
			);
		t.p=p;
		t.grid=g;
		if(p.url&&p.autoload) {
			g.populate();
		}
		if(p.onInit) {
			p.onInit(g);
		}
		return t;
	};

	var docloaded=false;
	$(document).ready(function () { docloaded=true });
	$.fn.flexigrid=function (p) {
		return this.each(function () {
			if(!docloaded) {
				$(this).hide();
				var t=this;
				$(document).ready
					(
						function () {
							$.addFlex(t,p);
						}
					);
			} else {
				$.addFlex(this,p);
			}
		});
	};
	$.fn.flexExist=function (p) {
		return this.each(function () {
			if(this.grid&&this.p.url) {
				return true;
			}
		});
	};
	$.fn.flexReload=function (p) {
		return this.each(function () {
			if(this.grid&&this.p.url) {
				this.p.page=1;
				this.p.newp=1;

				this.grid.populate();
			}
		});
	};
	$.fn.flexRemoveSortClass=function (p) {
		return this.each(function () {
			if(this.grid) {
				$(".sorted",this.t).removeClass("sorted");
				$(".sasc",this.t).removeClass("sasc");
				$(".sdesc",this.t).removeClass("sdesc");
			}
		});
	};
	$.fn.flexOptions=function (p) {
		return this.each(function () {
			if(this.grid) { $.extend(this.p,p); }
		});
	};
	$.fn.flexToggleCol=function (cid,visible) {
		return this.each(function () {
			if(this.grid) { this.grid.toggleCol(cid,visible); }
		});
	};
	$.fn.flexAddData=function (data) {
		return this.each(function () {
			if(this.grid) { this.grid.addData(data); }
		});
	};
	$.fn.noSelect=function (p) {
		if(p==null) {
			prevent=true;
		} else {
			prevent=p;
		}
		if(prevent) {
			return this.each(function () {
				if($.browser.msie||$.browser.safari) { $(this).bind('selectstart',function () { return false; }); }
				else if($.browser.mozilla) {
					$(this).css('MozUserSelect','none');
					$('body').trigger('focus');
				}
				else if($.browser.opera) { $(this).bind('mousedown',function () { return false; }); }
				else { $(this).attr('unselectable','on'); }
			});
		} else {
			return this.each(function () {
				if($.browser.msie||$.browser.safari) { $(this).unbind('selectstart'); }
				else if($.browser.mozilla) { $(this).css('MozUserSelect','inherit'); }
				else if($.browser.opera) { $(this).unbind('mousedown'); }
				else { $(this).removeAttr('unselectable','on'); }
			});
		}
	};
})(jQuery);
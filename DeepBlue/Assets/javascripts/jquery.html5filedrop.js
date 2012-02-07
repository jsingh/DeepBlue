(function ($) {
	$.addHtml5FileDrop=function (t,p) {
		if(t.fileUpload) { return false; }
		p=$.extend({
			url: '',
			refresh: 1000,
			paramname: 'userfile',
			maxfiles: 25,
			maxfilesize: 1, // MBs
			data: {},
			drop: empty,
			dragEnter: empty,
			dragOver: empty,
			dragLeave: empty,
			docEnter: empty,
			docOver: empty,
			docLeave: empty,
			beforeEach: empty,
			beforeValid: empty,
			beforeSend: empty,
			afterAll: empty,
			rename: empty,
			error: function (err,file,i) { alert(err); },
			uploadPending: empty,
			uploadStarted: empty,
			uploadFinished: empty,
			progressUpdated: empty,
			speedUpdated: empty
		},p);
		var g={
			files_count: 0
			,filesRejected: 0
			,filesDone: 0
			,files: null
			,stop_loop: false
			,pendingWork: null
			,errors: new Array()
			,set: function () {
				_target=this;
				$(t)
				.bind('drop',g.drop)
				.bind('dragenter',g.dragEnter)
				.bind('dragover',g.dragOver)
				.bind('dragleave',g.dragLeave);
				$(document)
				.bind('drop',g.docDrop)
				.bind('dragenter',g.docEnter)
				.bind('dragover',g.docOver)
				.bind('dragleave',g.docLeave);
			}
			,drop: function (e) {
				g.uploadPending(null);
				p.drop(e);
				if(e.dataTransfer) {
					g.files=e.dataTransfer.files;
				} else {
					if(g.files) {
						g.upload();
						return false;
					}
				}

				if(g.files) {
					g.files_count=g.files.length;
					for(var i=0;i<g.files_count;i++) {
						p.uploadStarted(i,g.files[i],g.files_count,_target);
					}
					if(g.beforeValid()==false) {
						var uploadStart=function () { g.upload(); }
						g.uploadPending(uploadStart);
						return false;
					} else {
						g.upload();
					}
					e.preventDefault();
				}
				return false;
			}
			,getBuilder: function (filename,filedata,boundary) {
				var dashdash='--',
					crlf='\r\n',
					builder='';

				$.each(p.data,function (i,item) {
					//if(typeof val==='function') val=val();
					builder+=dashdash;
					builder+=boundary;
					builder+=crlf;
					builder+='Content-Disposition: form-data; name="'+item.name+'"';
					builder+=crlf;
					builder+=crlf;
					builder+=item.value;
					builder+=crlf;
				});

				builder+=dashdash;
				builder+=boundary;
				builder+=crlf;
				builder+='Content-Disposition: form-data; name="'+p.paramname+'"';
				builder+='; filename="'+filename+'"';
				builder+=crlf;

				builder+='Content-Type: application/octet-stream';
				builder+=crlf;
				builder+=crlf;

				builder+=filedata;
				builder+=crlf;

				builder+=dashdash;
				builder+=boundary;
				builder+=dashdash;
				builder+=crlf;
				return builder;
			}

			,progress: function (e) {
				if(e.lengthComputable) {
					var percentage=Math.round((e.loaded*100)/e.total);
					if(this.currentProgress!=percentage) {

						this.currentProgress=percentage;
						p.progressUpdated(this.index,this.file,this.currentProgress);

						var elapsed=new Date().getTime();
						var diffTime=elapsed-this.currentStart;
						if(diffTime>=p.refresh) {
							var diffData=e.loaded-this.startData;
							var speed=diffData/diffTime; // KB per second
							p.speedUpdated(this.index,this.file,speed);
							this.startData=e.loaded;
							this.currentStart=elapsed;
						}
					}
				} else {
					p.progressUpdated(this.index,this.file,70);
				}
			}

			,upload: function () {
				if(g.beforeValid()==false) {
					return false;
				}
				g.stop_loop=false;
				if(!g.files) {
					p.error(g.errors[0]);
					return false;
				}
				g.filesDone=0;
				g.filesRejected=0;

				if(g.files_count>p.maxfiles) {
					p.error(g.errors[1]);
					return false;
				}
				for(var i=0;i<g.files_count;i++) {
					if(g.stop_loop) return false;
					try {
						if(g.beforeEach(g.files[i],i)!=false) {
							if(i===g.files_count) return;
							var reader=new FileReader(),
										max_file_size=1048576*p.maxfilesize;

							reader.index=i;
							if(g.files[i].size>max_file_size) {
								p.error(g.errors[2],g.files[i],i);
								g.filesRejected++;
								continue;
							}

							reader.onloadend=g.send;
							reader.readAsBinaryString(g.files[i]);
						} else {
							g.filesRejected++;
						}
					} catch(err) {
						p.error(g.errors[0]);
						return false;
					}
				}
			}

			,send: function (e) {
				// Sometimes the index is not attached to the
				// event object. Find it by size. Hack for sure.

				if(e.target.index==undefined) {
					e.target.index=getIndexBySize(e.total);
				}

				if(g.beforeValid()==false) {
					return false;
				}

				g.beforeSend();

				var xhr=new XMLHttpRequest(),
					upload=xhr.upload,
					file=g.files[e.target.index],
					index=e.target.index,
					start_time=new Date().getTime(),
					boundary='------multipartformboundary'+(new Date).getTime(),
					builder;

				newName=g.rename(file.name);
				if(typeof newName==="string") {
					builder=g.getBuilder(newName,e.target.result,boundary);
				} else {
					builder=g.getBuilder(file.name,e.target.result,boundary);
				}

				upload.index=index;
				upload.file=file;
				upload.downloadStartTime=start_time;
				upload.currentStart=start_time;
				upload.currentProgress=0;
				upload.startData=0;
				upload.addEventListener("progress",g.progress,false);

				xhr.open("POST",p.url,true);
				xhr.setRequestHeader('content-type','multipart/form-data; boundary='+boundary);

				xhr.sendAsBinary(builder);

				//p.uploadStarted(index,file,g.files_count);

				xhr.onload=function () {
					if(xhr.responseText) {
						var now=new Date().getTime(),
						timeDiff=now-start_time,
						result=p.uploadFinished(index,file,jQuery.parseJSON(xhr.responseText),timeDiff);
						g.filesDone++;
						if(g.filesDone==g.files_count-g.filesRejected) {
							g.afterAll();
						}
						if(result===false) g.stop_loop=true;
					}
				};
			}
			,getIndexBySize: function (size) {
				for(var i=0;i<g.files_count;i++) {
					if(g.files[i].size==size) {
						return i;
					}
				}

				return undefined;
			}

			,rename: function (name) {
				return p.rename(name);
			}
			,beforeEach: function (file,index) {
				return p.beforeEach(file,index);
			}

			,uploadPending: function (uploadStart) {
				g.pendingWork=uploadStart;
				return p.uploadPending(uploadStart);
			}

			,beforeSend: function () {
				var data=p.beforeSend();
				p.data=data;
			}

			,beforeValid: function () {
				return p.beforeValid();
			}

			,afterAll: function () {
				g.uploadPending(null);
				return p.afterAll();
			}

			,dragEnter: function (e) {
				clearTimeout(g.doc_leave_timer);
				e.preventDefault();
				p.dragEnter(e);
			}

			,dragOver: function (e) {
				clearTimeout(g.doc_leave_timer);
				e.preventDefault();
				p.docOver(e);
				p.dragOver(e);
			}

			,dragLeave: function (e) {
				clearTimeout(g.doc_leave_timer);
				p.dragLeave(e);
				e.stopPropagation();
			}

			,docDrop: function (e) {
				e.preventDefault();
				p.docLeave(e);
				return false;
			}

			,docEnter: function (e) {
				clearTimeout(g.doc_leave_timer);
				e.preventDefault();
				p.docEnter(e);
				return false;
			}

			,docOver: function (e) {
				clearTimeout(g.doc_leave_timer);
				e.preventDefault();
				p.docOver(e);
				return false;
			}

			,docLeave: function (e) {
				g.doc_leave_timer=setTimeout(function () {
					p.docLeave(e);
				},200);
			}
		};
		t.fileUpload=g;
		g.set();
	};
	jQuery.event.props.push("dataTransfer");


	var docloaded=false;
	$(document).ready(function () { docloaded=true });
	$.fn.filedrop=function (p) {
		return this.each(function () {
			if(!docloaded) {
				$(this).hide();
				var t=this;
				$(document).ready
					(
						function () {
							$.addHtml5FileDrop(t,p);
						}
					);
			} else {
				$.addHtml5FileDrop(this,p);
			}
		});
	};

	$.fn.filedropPending=function () {
		var pending=null;
		this.each(function () {
			pending=this.fileUpload.pendingWork;
		});
		return pending;
	};

	$.fn.filedropPendingUpload=function () {
		return this.each(function () {
			if(this.fileUpload) {
				if(this.fileUpload.pendingWork)
					this.fileUpload.pendingWork();
			}
		});
	};

	function empty() { }

	try {
		if(XMLHttpRequest.prototype.sendAsBinary) return;
		XMLHttpRequest.prototype.sendAsBinary=function (datastr) {
			function byteValue(x) {
				return x.charCodeAt(0)&0xff;
			}
			var ords=Array.prototype.map.call(datastr,byteValue);
			var ui8a=new Uint8Array(ords);
			this.send(ui8a.buffer);
		}
	} catch(e) { }

})(jQuery);
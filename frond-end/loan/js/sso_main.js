$(function(){
	var url_param;
	var arg = window.location.href;
	var sp = arg.split("?");
	if(sp.length > 1){
		if(sp[1].indexOf("param=") == 0){
			$("#taskForm").addClass("hide");
			url_param = sp[1].split("&")[0];
			$("#iframe0").attr('src','./Modal/task/myTask/index.html?'+url_param);
		}
	}
}

);


openDaiban = function(){
	$("#iframe0").attr('src','./Modal/task/myTask/index.html?'+url_param);
}
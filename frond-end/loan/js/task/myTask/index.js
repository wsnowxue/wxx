var table_1;
var handle_1;

//案例：获取get的参数
//注意：实际上应该获取server端返回的参数
var url_param;
var arg = window.location.href;
var sp = arg.split("?");
if(sp.length > 1){
	if(sp[1].indexOf("param=") == 0){
		$("#taskForm").addClass("hide");
		url_param = sp[1].split("&")[0].split("=")[1];
		sp = url_param.split(":");
	}
}

table_1 = function (params) {
	//根据get的参数，搜索数据
	tableData(params, $.extend($("#taskForm").values(), {
		isProcessed: false,ftCode:sp[0],flowNode:sp[1]
	}), interUrl.myTask.searchTaskList);

};



tableEvent_1 = {
	//数据展现区
};


handle_1 = function (value, row, index) {
	//按钮控制区
	//打开tab页
	return comn.addTab({
		title: .....,
		href: ....
	});
};



$("#btn-search").click(function () {
	$("#table1").bootstrapTable("refresh", {url: "..."});
});
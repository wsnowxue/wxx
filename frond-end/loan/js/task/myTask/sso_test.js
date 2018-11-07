openWindow = function (router) {
    
    window.open(window.location.origin + router)

	return;
};
downloadKpi()
function downloadKpi(){
    var data = { "state":1, "message":"操作成功。", "data":[ { "task_type":1, "task_count":1 }, { "task_type":2, "task_count":3 }, { "task_type":3, "task_count":1 }, { "task_type":4, "task_count":2 }, { "task_type":6, "task_count":4 } ], "total":5 }
    data = data.data
    var detail = {
        '1':'待考核',
        '2':'待审核',
        '3':'待自评',
        '4':'待归档',
        '6':'方案退回修改',
    }
    var babelES = {
        // 这是es6写的，请将转成es5之后使用
        es6:function (data) { 
            // var html = `
            // <li>
            //     <a class="has-arrow" href="javascript:void(0);" onclick="openWindow('/#/dashboard')">
            //         二手车评估流程（<span class="fff3333">20</span>）
            //     </a>
            // </li>`
            // var htmlArr = []
            // for (var i = 0; i < data.length; i++) {
            //     htmlArr.push(`
            //     <li>
            //         <a class="has-arrow" href="javascript:void(0);" onclick="openWindow('./dashboard')">
            //             ${detail[data[i].task_type]}（<span class="fff3333">${data[i].task_count}</span>）
            //         </a>
            //     </li>
            //     `)
            // }
            // return htmlArr
        },
        es5:function(data){
            var htmlArr = []
            for (var i = 0; i < data.length; i++) {
                htmlArr.push("\n                <li>\n                    <a class=\"has-arrow\" href=\"javascript:void(0);\" onclick=\"openWindow('/#/dashboard')\">\n                        " + detail[data[i].task_type] + "\uFF08<span class=\"fff3333\">" + data[i].task_count + "</span>\uFF09\n                    </a>\n                </li>\n                ");
            }
            return htmlArr
        }
    }
    console.log('ajax');
    
    $.ajax({
        url:'/Login/GetSelfTasksByUserId',
        data:{userId:70},
        dataType:'json',
        success:function(res){
            var data1 = res.data.splice(0,5)
            var data2 = res.data
            $("#menu1").append(babelES.es5(data1).join(''))
            $("#menu2").append(babelES.es5(data2).join(''))
        }
    })
}
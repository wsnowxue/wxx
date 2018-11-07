/**
 * Created by hyb on 16/1/11.
 */
//走流程公用方法

//流程流转人员列表
var tableEvent_sign, handle_sign, p2 = {};

tableEvent_sign = {
    "click .role": function (e, a, item, index) {
        p2 = {nextNodeUserName: item.userName, nextNodeUserId: a}
    }
};

handle_sign = function (value, row, index) {
    return ["<input type='radio' name='userId' class='role' value='" + value + "'/>"].join("");
};

//确认提交或退回模态框
var sureModal='<div class="modal fade" id="sureModal">'+
    '<div class="modal-dialog">'+
    '<div class="modal-content">'+
    '<div class="modal-header">'+
    '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>'+
    '<h4 class="modal-title">提示信息</h4>'+
    '</div>'+
    '<div class="modal-body">'+
    '<p class="tipText"></p>'+
    '</div>'+
    '<div class="modal-footer">'+
    '<button type="button" class="btn btn-primary" id="sureOption">确定</button>'+
    '<button type="button" class="btn btn-default" data-dismiss="modal">取消</button>'+
    '</div></div></div></div>';

function oppSureModal(text){
    if($("#sureModal").length>0){
        $("#sureModal").modal("show");
    }else{
        $("body").append(sureModal);
        $("#sureModal").find(".tipText").text(text);
        $("#sureModal").modal("show");
    }
}

//提交流程
//参数说明:_url1=预提交接口 _url2=准提交接口 _url3=提交结束跳转页面路径  _data=请求参数
function flowSubmit(_url1,_url2,_url3,_data){
    comn.ajax({
        url: _url1,
        data: _data,
        success: function (res0) {
            var nextNodeUserName=res0.data.userTasks[0].userName;
            var nextNodeUserId=res0.data.userTasks[0].userId;
            var nodeCode={nodeCode:res0.data.nextFlowNodeCode};
            var p3={nextNodeUserName:nextNodeUserName,nextNodeUserId:nextNodeUserId};
            if(res0.data.userTasks.length>1){
                table_sign = function (params) {
                    var p=params.data;
                    params.success({'total':res0.data.userTasks.length, rows: res0.data.userTasks});
                    params.complete();
                };
                tableEvent_sign = {
                    "click .role": function (e, a, item, index) {
                        p2 = {nextNodeUserName: item.userName, nextNodeUserId: a}
                    }
                };

                handle_sign = function (value, row, index) {
                    return ["<input type='radio' name='userId' class='role' value='" + value + "'/>"].join("");
                };
                $("#nextNode").html(res0.data.nextFlowNodeName);
                $("#table_sign").bootstrapTable();
                $("#table_sign").bootstrapTable('load', res0.data.userTasks);
                $("#signModal").modal("show");
                setTimeout("$('#table_sign').find('tr').eq(1).find('[name=\"userId\"]').prop('checked','checked')",500);
                p2=p3;
                $("#select-sign-btn").unbind("click").click(function(){
                    comn.ajax({
                        url: _url2,
                        data: $.extend(_data,p2),
                        success: function (res2) {
                            $("#signModal").modal("hide");
                            tip({content:res2.message});
                            comn.closeTab();
                        }
                    })
                })
            }else{
                comn.ajax({
                    url: _url2,
                    data: $.extend(_data,p3),
                    success: function (res4) {
                        tip({content:res4.message});
                        comn.closeTab();
                    }
                })
            }
        }
    })
}

//流程退回
function flowBack2Pre(){
    //退回上一步
    comn.ajax({
        url: interUrl.loanCancel.back2pre,
        data: loanApplyId,
        success: function (res1) {
            tip({content: res1.message});
            comn.closeTab();
        }
    });
}
//流程退回至业务录入
function flowBackOffice(){
    comn.ajax({
        url: "loan/cancel/process/flowBackOffice",
        data: loanApplyId,
        success: function (res1) {
            tip({content: res1.message});
            comn.closeTab();
        }
    });
}
//关闭作废流程
function flowCloseLoanApply(){
    $("#sureModal").modal("hide");
    comn.ajax({
        url: interUrl.loanCancel.close,
        data: loanApplyId,
        success: function (res) {
            tip({content: res.message});
            comn.closeTab();
        }
    });
}


//撤销
function flowCancel(){
    $("#sureModal").modal("hide");
    comn.ajax({
        url:interUrl.loanCancel.cancel,
        data:loanApplyId,
        success:function(res){
            console.log(res.message);
            tip({content:res.message || "撤销成功!"});
            comn.closeTab();
        }
    })
}
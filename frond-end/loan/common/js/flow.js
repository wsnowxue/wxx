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

//提交流程
//参数说明:_url1=预提交接口 _url2=准提交接口 _url3=提交结束跳转页面路径  _data=请求参数
function flowSubmit(_url1,_url2,_url3,_data){
    comn.ajax({
        url: _url1,
        data: _data,
        success: function (res0) {
        	if(!res0.data.userTasks){
        		tip({
        			content:"下一节点无可提交人！"
        		})
        		return;
        	}
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

//车商流程撤销
function flowCancel(){
    $("#sureModal").modal("hide");
    comn.ajax({
        url:interUrl.carDealer.cancel,
        data:{dealerId:$("#dealerId").val()},
        success:function(res){
            tip({content:res.message || "取消成功"});
            comn.closeTab();
        }
    })
}

//车商返回上一步
function flowBack2PreCar(){
    $("#sureModal").modal("hide");
    comn.ajax({
        url:interUrl.carDealer.back2pre,
        data:{dealerId:dealerId},
        success:function(res){
            tip({content:res.message || "返回成功"});
            comn.closeTab();
        }
    })
}

//车商退回客户经理
function carDealerBack2launch(){
    $("#sureModal").modal("hide");
    comn.ajax({
        url:interUrl.carDealer.carDealerBack2launch,
        data:{dealerId:dealerId},
        success:function(res){
            tip({content:res.message || "退回成功"});
            comn.closeTab();
        }
    })
}


//流程退回
function flowBack2Pre(){
    //退回上一步
    comn.ajax({
        url: interUrl.myTask.back2pre,
        data: loanApplyId,
        success: function (res1) {
            tip({content: res1.message});
            comn.closeTab();
        }
    });
}


//退回至贷款修改发起
function back2motifyLanuch(){
	 comn.ajax({
	        url: "loan/modify/process/motifyLanuch",
	        data: loanApplyId,
	        success: function (res1) {
	            tip({content: res1.message});
	            comn.closeTab();
	        }
	    });
}
//退回至业务签单
function back2billresearch(){
	 comn.ajax({
	        url: "loanApply/back2billresearch",
	        data: loanApplyId,
	        success: function (res1) {
	            tip({content: res1.message});
	            comn.closeTab();
	        }
	    });
}
//诚易融流程退回
function chengyirongFlowBack2Pre(){
    //退回上一步
    comn.ajax({
        url: interUrl.myTask.chengyirongBack2pre,
        data: loanApplyId,
        success: function (res1) {
            tip({content: res1.message});
            comn.closeTab();
        }
    });
}

//流程退回贷款发起
function back2LoanLaunch(){
    //退回上一步
    comn.ajax({
        url: interUrl.myTask.back2LoanLaunch,
        data: loanApplyId,
        success: function (res1) {
            tip({content: res1.message});
            comn.closeTab();
        }
    });
}
//诚易融流程退回贷款发起
function back2ChengyirongLoanLaunch(){
    //退回上一步
    comn.ajax({
        url: interUrl.myTask.back2ChengyirongLoanLaunch,
        data: loanApplyId,
        success: function (res1) {
            tip({content: res1.message});
            comn.closeTab();
        }
    });
}

//关闭贷款
function flowCloseLoanApply(){
    $("#sureModal").modal("hide");
    comn.ajax({
        url: interUrl.myTask.closeLoanApply,
        data: loanApplyId,
        success: function (res) {
            tip({content: res.message});
            comn.closeTab();
        }
    });
}

//退回内勤
function flowBack2BudgetOfficeStaff(){
    comn.ajax({
        url: interUrl.myTask.back2BudgetOfficeStaff,
        data:loanApplyId,
        success: function (res) {
            tip({content:res.message});
            comn.closeTab();
        }
    });
}

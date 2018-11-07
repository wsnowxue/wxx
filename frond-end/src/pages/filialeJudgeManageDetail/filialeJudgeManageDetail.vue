<template>
    <div class="filialeJudgeManageAdd">
        <!-- 标题 -->
        <div class="text-center">{{templete_name}}</div>
        <div v-if="flag">
            <ul class="clear child-float">
                <li>创建时间：{{checkDetails.create_time}}</li>
                <li>创建人：{{checkDetails.creator_user_name}}</li>
                <li>审批状态：{{checkDetails.templete_check_statue | filterCheck}}</li>
                <li v-if="checkDetails.templete_check_statue == 2">退回原因：{{checkDetails.back_reason}}</li>
            </ul>
        </div>
        <div class="main-space"></div>
        <!-- 表格 -->
        <el-table
        :data="tableData3"
        :span-method="objectSpanMethod"
        border
        style="width: 100%">
            <el-table-column 
            :width="$store.state.width"
            prop="dimension_name"
            label="考核维度" show-overflow-tooltip>
            </el-table-column>
            <el-table-column 
            :width="$store.state.width"
            prop="org_name"
            label="部门" show-overflow-tooltip>
            </el-table-column>
            <el-table-column
            prop="detail_name"
            label="细项指标" show-overflow-tooltip>
            </el-table-column>
            <el-table-column
            :width="$store.state.width"
            prop="base_score"
            label="基础分值（100）" show-overflow-tooltip>
            </el-table-column>
            <el-table-column
            prop="indicator_formula_name"
            label="指标公式" show-overflow-tooltip>
            </el-table-column>
            <el-table-column
            prop="check_method_name"
            label="考核办法" show-overflow-tooltip>
            </el-table-column>
        </el-table>
        <div class="text-center back-page">
            <el-button @click="backPage">返回</el-button>
        </div>
    </div>
</template>

<script>
import filialeJudgeManageDetail from "./filialeJudgeManageDetail.js";
export default {
  ...filialeJudgeManageDetail
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

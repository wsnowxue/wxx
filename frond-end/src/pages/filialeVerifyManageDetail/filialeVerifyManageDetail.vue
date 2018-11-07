<template>
    <div class="filiale-judge-manage">
        <!-- 标题 -->
        <div class="text-center">{{checkDetails.templete_name}}</div>
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
        <el-table :data="tableData" :span-method="objectSpanMethod" border style="width: 100%">
            <el-table-column
            prop="dimension_name"
            show-overflow-tooltip
            label="考核维度">
            </el-table-column>
            <el-table-column
            prop="detail_name"
            show-overflow-tooltip
            label="细项指标及权重">
            </el-table-column>
            <el-table-column
            prop="base_score"
            label="基础分值（100）">
            </el-table-column>
            <el-table-column
            prop="indicator_formula_name"
            show-overflow-tooltip
            label="指标公式">
            </el-table-column>
            <el-table-column
            prop="check_method_name"
            show-overflow-tooltip
            label="考核办法">
            </el-table-column>
        </el-table>
        <!-- 审批 -->
        <div class="follow-table" v-if="!flag && _has('/submitFilialeVerify'+type)">
            <el-form :inline="true" :rules="rules" :model="tableSearch" ref="tableSearch" label-width="100px" class="demo-form-inline">
                <el-form-item label="审批结果：" prop="check_result">
                    <el-radio v-model="tableSearch.check_result" label="1">通过</el-radio>
                    <el-radio v-model="tableSearch.check_result" label="2">退回</el-radio>
                </el-form-item>
                <el-form-item label="退回原因：" prop="check_suggest" v-if="tableSearch.check_result == '2'">
                    <el-input
                    type="textarea"
                    :autosize="{ minRows: 2, maxRows: 4}"
                    placeholder="请输入内容"
                    v-model="tableSearch.check_suggest">
                    </el-input>
                </el-form-item>
            </el-form>
        </div>
        <div class="text-center back-page">
            <el-button @click="backPage">返回</el-button>
            <el-button v-if="!flag" type="primary" @click="submitTable('tableSearch')">提交</el-button>
        </div>
    </div>
</template>

<script>
import filialeVerifyManageDetail from "./filialeVerifyManageDetail.js";
export default {
  ...filialeVerifyManageDetail
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

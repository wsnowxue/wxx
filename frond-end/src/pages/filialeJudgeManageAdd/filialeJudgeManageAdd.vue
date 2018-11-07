<template>
    <div class="filialeJudgeManageAdd">
        <!-- 顶部 -->
        <el-form :inline="true" :model="tableDetail" ref="tableDetail" class="demo-form-inline">
            <el-form-item label="考核方案名称：" prop="templete_name">
                <el-input v-model="tableDetail.templete_name" placeholder="请输入考核方案名称"></el-input>
            </el-form-item>
            <!-- <el-form-item prop="allow_beyond_limit">
                <el-checkbox v-model="tableDetail.allow_beyond_limit">允许评分超过基础分值</el-checkbox>
            </el-form-item> -->
            <el-form-item class="open-dialog">
                <el-button type="primary" @click="dialogVisible = true;addProject('tableObject')"> <i class="el-icon-circle-plus-outline"></i> 新增</el-button>
            </el-form-item>
        </el-form>
        <div class="main-space"></div>
        <!-- 表格 -->
        <el-table
        :data="tableData4"
        :span-method="objectSpanMethod"
        border
        show-summary
        style="width: 100%">
            <el-table-column 
            label="考核维度" show-overflow-tooltip>
                <template slot-scope="scope">
                    <el-select v-model="scope.row.dimension_id" @change="changeDimension(scope.row,scope.$index)" filterable placeholder="请选择考核维度">
                        <el-option
                        v-for="item in dimensions"
                        :key="item.id"
                        :label="item.dimension_name"
                        :value="item.id">
                        </el-option>
                    </el-select>
                </template>
            </el-table-column>
            <el-table-column label="选择部门">
                <template slot-scope="scope">
                    <!-- @change="changeDimension(scope.row,scope.$index)"  -->
                    <el-select v-model="scope.row.org_id" filterable placeholder="请选择部门">
                        <el-option
                        v-for="item in departments"
                        :key="item.F_Id"
                        :label="item.F_FullName"
                        :value="item.F_Id">
                        </el-option>
                    </el-select>
                </template>
            </el-table-column>
                
            <el-table-column
            label="细项指标" show-overflow-tooltip>
                <template slot-scope="scope">
                    <el-select v-model="scope.row.detail_id" @change="changeRow(scope.row)" filterable placeholder="请选择细项指标">
                        <el-option
                        v-for="item in details"
                        :key="item.id"
                        :label="item.detail_name"
                        :value="item.id">
                        </el-option>
                    </el-select>
                </template>
            </el-table-column>
                
            <el-table-column :width="$store.state.width" prop="base_score" label="基础分值（100）">
                <template slot-scope="scope">
                    <!-- {{scope.row.detail_obj}} -->
                    <el-input-number v-model="scope.row.base_score" placeholder="请输入分值" :precision="2" :min="0" :step="0.1"></el-input-number>
                </template>
            </el-table-column>
            <el-table-column
            prop="detail_obj.formule"
            label="指标公式" show-overflow-tooltip>
            </el-table-column>
            <el-table-column
            prop="detail_obj.check_method_name"
            label="考核办法" show-overflow-tooltip>
            </el-table-column>
            <el-table-column :width="$store.state.width" label="操作">
            <template slot-scope="scope">
                <!-- <el-button
                size="mini"
                @click="addRow(scope.$index, scope.row)">添加行</el-button> -->
                <el-button
                size="mini"
                @click="deleteRow(scope.$index, scope.row)">删除</el-button>
            </template>
            </el-table-column>
        </el-table>
        <div class="text-center back-page">
            <el-button @click="backPage">返回</el-button>
            <el-button type="primary" @click="selectPerson()">提交</el-button>
        </div>

        <!-- 弹窗 -->
        <el-dialog
            title="新增考核指标"
            :visible.sync="dialogVisible"
            width="500px" class="performance-dialog">
            <el-form :model="tableObject" status-icon ref="tableObject" :rules="rules" label-width="100px" class="demo-ruleForm">
                <el-form-item label="考核维度：" prop="dimension_id">
                    <el-select v-model="tableObject.dimension_id" filterable placeholder="请选择考核维度">
                        <el-option
                        v-for="item in dimensions"
                        :key="item.id"
                        :label="item.dimension_name"
                        :value="item.id">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="细项指标：" prop="detail_id">
                    <el-select v-model="tableObject.detail_id" @change="detailChange(tableObject.detail_id)" filterable placeholder="请选择细项指标">
                        <el-option
                        v-for="item in details"
                        :key="item.id"
                        :label="item.detail_name"
                        :value="item.id">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="基础分值：" prop="base_score">
                    <el-input-number v-model="tableObject.base_score" placeholder="请输入基础分值" :precision="2" :min="0" :step="0.1"></el-input-number>
                </el-form-item>
                <el-form-item label="指标公式：" prop="formule">
                    <el-input v-model="tableObject.formule" :disabled="true"></el-input>
                </el-form-item>
                <el-form-item label="考核办法：" prop="check_method_name">
                    <el-input v-model="tableObject.check_method_name" :disabled="true"></el-input>
                </el-form-item>
                
                <el-form-item>
                    <el-button type="primary" @click="checkDialog('tableObject')">确认</el-button>
                    <!-- <el-button @click="resetForm('tableObject')">重置</el-button> -->
                </el-form-item>
            </el-form>
        </el-dialog>
        <!-- 弹窗 选择审核人 -->
        <el-dialog
            title="选择审核人"
            :visible.sync="dialogCheckPerson"
            width="500px" class="performance-dialog" @close="closeDialog">
            <el-form :model="tableObject2" status-icon ref="tableObject2" label-width="100px" class="demo-ruleForm">
                
                <el-form-item label="审核人：" prop="checker_list">
                    <div class="item-examiner" v-if="checker_list.length > 0" v-for="(item,index) in checker_list" :key="item.checker_id">
                        <div class="examiner">
                            <span class="el-icon-error" @click="deleteExaminer(index)"></span>
                            <div class="examiner-name" :title="item.checker_name">{{item.checker_name.substr(0,2)}}</div>
                        </div>
                        <span class="el-icon-back"></span>
                    </div>
                    <div class="examiner item-examiner plus-examiner" @click="innerVisible = true;sourceChecker = 2">
                        <span class="el-icon-plus"></span>
                    </div>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="submitTable('tableObject2')">确认提交该绩效方案</el-button>
                    <!-- <el-button @click="resetForm('tableObject')">重置</el-button> -->
                </el-form-item>
            </el-form>
            <!-- 审核人表格 -->
            <el-dialog title="请选择" width="40%" :visible.sync="innerVisible" class="performance-dialog" append-to-body>
                <el-table :data="gridData" ref="singleTable" stripe border style="width: 100%" highlight-current-row @row-click="handleCurrentClick">
                    <el-table-column type="index" prop="id" label="序号"></el-table-column>
                    <el-table-column prop="role_name" label="角色名称"></el-table-column>
                    <el-table-column prop="organize_name" label="机构名称"></el-table-column>
                    <el-table-column prop="department_name" label="部门"></el-table-column>
                    <el-table-column prop="name" label="姓名"></el-table-column>
                </el-table>
            </el-dialog>
        </el-dialog>
        
    </div>
</template>

<script>
import filialeJudgeManageAdd from "./filialeJudgeManageAdd.js";
export default {
  ...filialeJudgeManageAdd
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="less" scoped>
    @import url(./filialeJudgeManageAdd.less);
</style>

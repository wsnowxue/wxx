<template>
<!-- 考核管理-分公司/个人页面 -->
    <div class="filiale-judge-manage">
        <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
            <el-form-item label="绩效考核名称：" prop="assessment_name">
                <el-input v-model="tableSearch.assessment_name" placeholder="请输入绩效考核名称"></el-input>
            </el-form-item>
            <el-form-item label="考核方案名称：" prop="templete_name">
                <el-input v-model="tableSearch.templete_name" placeholder="请输入考核考核名称"></el-input>
            </el-form-item>
            <el-form-item label="考核状态：" prop="assessment_statue">
                <el-select v-model="tableSearch.assessment_statue" placeholder="--请选择--">
                    <el-option label="上传中" value="0"></el-option>
                    <el-option label="待归档" value="1"></el-option>
                    <el-option label="已归档" value="2"></el-option>
                </el-select>
            </el-form-item>
            <div class="text-center">
                <el-form-item>
                    <el-button @click="resetForm('tableSearch')"><i class="el-icon-circle-close-outline"></i> 清除查询条件</el-button>
                    <el-button type="primary" @click="getData('tableSearch')"><i class="el-icon-search"></i> 查询</el-button>
                </el-form-item>
            </div>
            <div class="text-right operation">
                <el-button type="primary" v-if="_has('/addtask'+type)" @click="outerVisible.flag = true"><i class="el-icon-circle-plus-outline"></i> 发起新考核</el-button>
            </div>
            
        </el-form>
        <div class="main-space"></div> 
        <!-- 表格 -->
        <el-table :data="tableData" stripe border style="width: 100%" @selection-change="handleSelectionChange">
            <el-table-column type="selection" label="序号"></el-table-column>
            <el-table-column v-for="item in tableThead" :key="item.id" :prop="item.value" :label="item.name" show-overflow-tooltip></el-table-column>
            <el-table-column :width="$store.state.width200" prop="start_end_time" label="考核起止时间"></el-table-column>
            <el-table-column :width="$store.state.width" prop="assessment_sponsor_name" label="考核发起人"></el-table-column>
            <el-table-column :width="$store.state.width" prop="create_time" label="创建时间"></el-table-column>
            <el-table-column :width="$store.state.width" prop="assessment_statue_name" label="考核状态"></el-table-column>
            <el-table-column :width="$store.state.width290" label="操作">
                <template slot-scope="scope">
                    <el-button type="text" v-if="scope.row.show_opbtn" @click="download(scope.row)">下载模板</el-button>
                    <el-upload
                        class="upload-demo inline-block"
                        action="/UpLoad/UpLoadFile"
                        v-model="scope.row.file_name"
                        ref="upload"
                        accept=".xls,.xlsx"
                        :on-success="uploadSuccess"
                        :on-remove="removeUpload"
                        :auto-upload="true"
                        v-if="scope.row.show_opbtn">
                        <el-button size="text" @click="uploadData(scope.row)">上传数据</el-button>
                    </el-upload>
                    <!-- <el-button type="text" @click="toJudgeFilialeDetail(scope.row)" v-if="scope.row.assessment_statue == '0' && scope.row.filing_people == $store.state.userData.UserId">上传数据</el-button> -->
                    <el-button type="text" v-if="_has('/judgeManageFilialeDetail'+type)" @click="toJudgeFilialeDetail(scope.row)">查看详情</el-button>
                    <el-button type="text" @click="checkProcess(scope.row)" v-if="scope.row.assessment_statue == '0' && scope.row.filing_people == $store.state.userData.UserId">查看上传进度</el-button>
                    <el-button type="text" @click="SaveBranchAssessmentResult(scope.row)" v-if="scope.row.assessment_statue == '1' && scope.row.saveResult">归档</el-button>
                </template>
            </el-table-column>
        </el-table>
        <div class="main-space"></div>    
        <!-- 分页 -->
        <el-pagination
        @size-change="sizeChange"
        @current-change="currentChange"
        :current-page.sync="currentPage"
        :page-sizes="pageNumber"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total">
        </el-pagination>
        
        <!-- 发起考核弹窗 -->
        <examiner :obj="outerVisible" ref="examiner" @refreshList="getData"></examiner>


        <!-- 查看上传 -->
        <el-dialog
            title="查看上传进度"
            :visible.sync="seeProcess"
            width="500px" class="performance-dialog" @close="closeDialog">
            <div>
                {{unuploadBranch}}未上传
            </div>
        </el-dialog>
    </div>
</template>

<script>
import judgeManageFiliale from "./judgeManageFiliale";
export default judgeManageFiliale
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

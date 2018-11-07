<template>
<!-- 个人考核详情 -->
    <div>
        <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
            <el-form-item label="员工姓名：" prop="personal_name">
                <el-input v-model="tableSearch.personal_name" placeholder="请输入员工姓名"></el-input>
            </el-form-item>
            <el-form-item label="手机号码：" prop="telphone">
                <el-input v-model="tableSearch.telphone" placeholder="请输入手机号码"></el-input>
            </el-form-item>
            
            <div class="text-center">
                <el-form-item>
                    <el-button @click="resetForm('tableSearch')"><i class="el-icon-circle-close-outline"></i> 清除查询条件</el-button>
                    <el-button type="primary" @click="getData('tableSearch')"><i class="el-icon-search"></i> 查询</el-button>
                </el-form-item>
            </div>
            <div class="text-right operation">
                <el-button type="primary" @click="download()" v-if="flag">考核结果导出</el-button>
            </div>
        </el-form>
        <div class="main-space"></div> 
        <!-- 表格 -->
        <el-table :data="tableData" stripe border style="width: 100%" @sort-change="getData2">
            <el-table-column type="index" label="序号"></el-table-column>
            <el-table-column v-for="item in tableThead" :key="item.id" :prop="item.value" :label="item.name" show-overflow-tooltip></el-table-column>
            <el-table-column label="考核得分" prop="score" sortable='custom' v-if="finished">
                <template slot-scope="scope">
                    <span type="text" v-if="scope.row.assessment_detail_statue == 0">--</span>
                    <span type="text" v-if="scope.row.assessment_detail_statue == 1">{{scope.row.score}}</span>
                </template>
            </el-table-column>
            <el-table-column label="部门得分" prop="score" sortable='custom' v-if="!finished">
                <template slot-scope="scope">
                    <span type="text" v-if="scope.row.assessment_detail_statue == 0">--</span>
                    <span type="text" v-if="scope.row.assessment_detail_statue == 1">{{scope.row.score}}</span>
                </template>
            </el-table-column>
            <el-table-column :width="$store.state.width" label="操作" v-if="flag">
                <template slot-scope="scope">
                    <el-button type="text" @click="toJudgeManageFilialeCheck(scope.row)" v-if="_has('/judgeManageFilialeCheck'+type)">查看详情</el-button>
                    <!-- <el-button type="text" @click="toJudgeManageFilialeCheck(scope.row)" v-if="_has('/judgeManageFilialeChecked'+type)">考核</el-button> -->
                </template>
            </el-table-column>
        </el-table>
        <div class="main-space"></div>    
        <!-- 分页 -->
        <el-pagination @size-change="sizeChange" @current-change="currentChange" :current-page.sync="currentPage" :page-sizes="pageNumber" layout="total, sizes, prev, pager, next, jumper" :total="total">
        </el-pagination>
        <el-dialog title="重新导入指标量值" width="40%" :visible.sync="reUploadOpen" class="performance-dialog" append-to-body @close="closeDialog">
            <el-upload
                class="upload-demo"
                action="/UpLoad/UpLoadFile"
                v-model="file_name"
                :limit="1"               
                ref="upload"
                :on-success="uploadSuccess"
                :on-remove="removeUpload"
                :auto-upload="true">
                <el-button type="primary" :disabled="disabled">选择文件</el-button>
            </el-upload>
            <div>
                <el-button type="primary" v-if="disabled" @click="ReAssessment">确定导入</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
import judgePersonDetail from './judgePersonDetail.js'
export default {
    ...judgePersonDetail
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

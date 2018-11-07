<template>
<!-- 分公司考核详情 -->
    <div>
        <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
            <el-form-item label="分公司名称：" prop="branch_name">
                <el-input v-model="tableSearch.branch_name" placeholder="请输入分公司名称"></el-input>
            </el-form-item>
            <!-- <el-form-item label="考核状态：" prop="assessment_detail_statue">
                <el-select v-model="tableSearch.assessment_detail_statue" placeholder="--请选择--">
                    <el-option label="考核中" value="0"></el-option>
                    <el-option label="已完成" value="1"></el-option>
                </el-select>
            </el-form-item> -->
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
        <el-table :data="tableData" stripe border style="width: 100%" :default-sort = "{prop: 'score',order: 'descending'}" @sort-change='sortChange'>
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
            <el-table-column label="操作" v-if="flag">
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
        
    </div>
</template>

<script>
import judgeFilialeDetail from './judgeFilialeDetail.js'
export default {
    ...judgeFilialeDetail
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

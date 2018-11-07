<template>
<!-- 分公司/个人考核方案管理页面 -->
    <div class="filiale-judge-manage">
        <!-- 搜索 -->
        <el-form :inline="true" :model="tableSearch" ref="tableSearch" class="demo-form-inline">
            <el-form-item label="考核方案名称：" prop="templete_name">
                <el-input v-model="tableSearch.templete_name" placeholder="请输入考核方案名称"></el-input>
            </el-form-item>
            <el-form-item label="审核状态：" prop="templete_check_statue">
                <el-select v-model="tableSearch.templete_check_statue" placeholder="--请选择--">
                    <el-option v-for="item in templeteCheckStatueList" :key="item.id" :label="item.name" :value="item.id"></el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="启用状态：" prop="statue">
                <el-select v-model="tableSearch.statue" placeholder="--请选择--">
                    <el-option v-for="item in statueList" :key="item.id" :label="item.name" :value="item.id"></el-option>
                </el-select>
            </el-form-item>
            
            <div class="text-center">
                <el-form-item>
                    <el-button @click="resetForm('tableSearch')"><i class="el-icon-circle-close-outline"></i> 清除查询条件</el-button>
                    <el-button type="primary" @click="getData('tableSearch')"><i class="el-icon-search"></i> 查询</el-button>
                </el-form-item>
            </div>
            <div v-if="_has('/templeteapi/addtemplete'+type)" class="text-right operation">
                <el-button type="primary" @click="routerToAdd('new')"> <i class="el-icon-circle-plus-outline"></i> 新增考核方案</el-button>
            </div>
        </el-form>
        <div class="main-space"></div>
        <!-- 表格 -->
        <el-table
        :data="tableData"
        stripe border
        style="width: 100%">
            <el-table-column
            v-for="item in tableThead"
            :key="item.value"
            :prop="item.value"
            :label="item.name" show-overflow-tooltip>
            </el-table-column>
            <el-table-column :width="$store.state.width" label="审核状态">
                <template slot-scope="scope">
                    {{scope.row.templete_check_statue | filterCheckStatue}}
                </template>
            </el-table-column>
            <el-table-column :width="$store.state.width" label="启用状态">
                <template slot-scope="scope">
                    {{scope.row.templete_check_statue != 1?"—— ——":(scope.row.statue ?"已启用":"已停用")}}
                </template>
            </el-table-column>
            
            <el-table-column :width="$store.state.width" label="操作">
                <template slot-scope="scope">
                    <!-- todo 判断按钮显示方式 -->
                    <el-button type="text" v-if="_has('/templeteapi/gettempletedetail'+type)" @click="routerToDetail(scope.row)">查看详情</el-button>
                    <el-button type="text" v-if="_has('/templeteapi/changetempletestatue'+type) && scope.row.statue == 0 && scope.row.templete_check_statue == 1" @click="openChangeStatue(scope.row)">启用</el-button>
                    <el-button type="text" v-if="_has('/templeteapi/changetempletestatue'+type) && scope.row.statue == 1 && scope.row.templete_check_statue == 1" @click="openChangeStatue(scope.row)">停用</el-button>
                    <!-- <el-button type="text" v-if="_has('/templeteapi/changetempletestatue'+type) && scope.row.templete_check_statue == 1 && scope.row.statue == 0" @click="openChangeStatue(scope.row)">启用</el-button> -->
                    <!-- <el-button type="text" v-if="_has('/templeteapi/changetempletestatue'+type) && scope.row.templete_check_statue == 1 && scope.row.statue == 1" @click="openChangeStatue(scope.row)">停用</el-button> -->
                    <!-- <el-button type="text" v-if="_has('/templeteapi/gettempletechange0'+type) && scope.row.templete_check_statue == 2" @click="routerToAdd('change',scope.row)">修改</el-button> -->
                    <el-button type="text" v-if="_has('/templeteapi/gettempletechange'+type) && scope.row.templete_check_statue == 2 && scope.row.creator_user_id == $store.state.userData.UserId" @click="routerToAdd('change',scope.row)">修改</el-button>
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
        :page-size="10"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total">
        </el-pagination>
    </div>
</template>

<script>
import filialeJudgeManage from "./filialeJudgeManage.js";
export default {
  ...filialeJudgeManage
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
